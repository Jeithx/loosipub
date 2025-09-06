using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace Core.Utilities.File
{
    public static class ImageBlurHelper
    {
        /// <summary>
        /// IFormFile dosyasına blur efekti uygular ve byte array olarak döner
        /// </summary>
        /// <param name="formFile">Blur uygulanacak resim dosyası</param>
        /// <param name="blurRadius">Blur yarıçapı (varsayılan: 10)</param>
        /// <param name="quality">JPEG kalitesi 1-100 arası (varsayılan: 85)</param>
        /// <returns>Blur uygulanmış resmin byte array'i</returns>
        public static async Task<byte[]> ApplyBlurAsync(IFormFile formFile, float blurRadius = 10f, int quality = 85)
        {
            if (formFile == null || formFile.Length == 0)
                throw new ArgumentException("Form file boş olamaz", nameof(formFile));

            if (blurRadius < 0)
                throw new ArgumentException("Blur yarıçapı negatif olamaz", nameof(blurRadius));

            if (quality < 1 || quality > 100)
                throw new ArgumentException("Kalite değeri 1-100 arasında olmalıdır", nameof(quality));

            // Desteklenen dosya formatlarını kontrol et
            var supportedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/bmp", "image/webp" };
            if (!supportedTypes.Contains(formFile.ContentType.ToLower()))
                throw new NotSupportedException($"Desteklenmeyen dosya formatı: {formFile.ContentType}");

            using var inputStream = formFile.OpenReadStream();
            using var image = await Image.LoadAsync(inputStream);

            // Blur efekti uygula
            image.Mutate(x => x.GaussianBlur(blurRadius));

            using var outputStream = new MemoryStream();

            // Orijinal formatı koru
            var encoder = GetEncoder(formFile.ContentType, quality);
            await image.SaveAsync(outputStream, encoder);

            return outputStream.ToArray();
        }

        /// <summary>
        /// IFormFile dosyasına blur efekti uygular ve yeni IFormFile olarak döner
        /// </summary>
        /// <param name="formFile">Blur uygulanacak resim dosyası</param>
        /// <param name="blurRadius">Blur yarıçapı (varsayılan: 10)</param>
        /// <param name="quality">JPEG kalitesi 1-100 arası (varsayılan: 85)</param>
        /// <returns>Blur uygulanmış yeni IFormFile</returns>
        public static async Task<IFormFile> ApplyBlurAndReturnFormFileAsync(IFormFile formFile, float blurRadius = 10f, int quality = 85)
        {
            if (formFile == null || formFile.Length == 0)
                throw new ArgumentException("Form file boş olamaz", nameof(formFile));

            var blurredBytes = await ApplyBlurAsync(formFile, blurRadius, quality);

            var fileName = string.IsNullOrEmpty(formFile.FileName)
                ? "blurred_image.jpg"
                : Path.GetFileNameWithoutExtension(formFile.FileName) + "_blurred" + Path.GetExtension(formFile.FileName);

            return new BlurredFormFile(
                fileBytes: blurredBytes,
                name: formFile.Name ?? "file",
                fileName: fileName,
                contentType: formFile.ContentType ?? "image/jpeg"
            );
        }

        /// <summary>
        /// IFormFile dosyasına blur efekti uygular ve dosya olarak kaydeder
        /// </summary>
        /// <param name="formFile">Blur uygulanacak resim dosyası</param>
        /// <param name="outputPath">Çıktı dosyasının tam yolu</param>
        /// <param name="blurRadius">Blur yarıçapı (varsayılan: 10)</param>
        /// <param name="quality">JPEG kalitesi 1-100 arası (varsayılan: 85)</param>
        public static async Task ApplyBlurAndSaveAsync(IFormFile formFile, string outputPath, float blurRadius = 10f, int quality = 85)
        {
            if (string.IsNullOrWhiteSpace(outputPath))
                throw new ArgumentException("Çıktı yolu boş olamaz", nameof(outputPath));

            var blurredBytes = await ApplyBlurAsync(formFile, blurRadius, quality);
            await System.IO.File.WriteAllBytesAsync(outputPath, blurredBytes);
        }

        /// <summary>
        /// Birden fazla IFormFile'a aynı anda blur efekti uygular
        /// </summary>
        /// <param name="formFiles">Blur uygulanacak resim dosyaları</param>
        /// <param name="blurRadius">Blur yarıçapı (varsayılan: 10)</param>
        /// <param name="quality">JPEG kalitesi 1-100 arası (varsayılan: 85)</param>
        /// <returns>Blur uygulanmış resimlerin byte array listesi</returns>
        public static async Task<List<byte[]>> ApplyBlurToMultipleAsync(IEnumerable<IFormFile> formFiles, float blurRadius = 10f, int quality = 85)
        {
            var tasks = formFiles.Select(file => ApplyBlurAsync(file, blurRadius, quality));
            var results = await Task.WhenAll(tasks);
            return results.ToList();
        }

        /// <summary>
        /// Dosya formatına göre uygun encoder'ı döner
        /// </summary>
        private static IImageEncoder GetEncoder(string contentType, int quality)
        {
            return contentType.ToLower() switch
            {
                "image/jpeg" or "image/jpg" => new JpegEncoder { Quality = quality },
                "image/png" => new PngEncoder(),
                _ => new JpegEncoder { Quality = quality } // Varsayılan olarak JPEG
            };
        }

        /// <summary>
        /// Resim dosyası olup olmadığını kontrol eder
        /// </summary>
        /// <param name="formFile">Kontrol edilecek dosya</param>
        /// <returns>Resim dosyası ise true</returns>
        public static bool IsImageFile(IFormFile formFile)
        {
            if (formFile == null) return false;

            var supportedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/bmp" };
            return supportedTypes.Contains(formFile.ContentType?.ToLower());
        }
    }

    public class BlurredFormFile : IFormFile
    {
        private readonly byte[] _fileBytes;
        private readonly string _name;
        private readonly string _fileName;
        private readonly string _contentType;
        private readonly IHeaderDictionary _headers;

        public BlurredFormFile(byte[] fileBytes, string name, string fileName, string contentType)
        {
            _fileBytes = fileBytes ?? throw new ArgumentNullException(nameof(fileBytes));
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
            _contentType = contentType ?? "application/octet-stream";
            _headers = new HeaderDictionary();
        }

        public string ContentType => _contentType;

        public string ContentDisposition => $"form-data; name=\"{_name}\"; filename=\"{_fileName}\"";

        public IHeaderDictionary Headers => _headers;

        public long Length => _fileBytes.Length;

        public string Name => _name;

        public string FileName => _fileName;

        public Stream OpenReadStream()
        {
            return new MemoryStream(_fileBytes, false);
        }

        public void CopyTo(Stream target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            target.Write(_fileBytes, 0, _fileBytes.Length);
        }

        public async Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            await target.WriteAsync(_fileBytes, 0, _fileBytes.Length, cancellationToken);
        }
    }
}
