using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace Core.Utilities.File
{
    public class FileHelper
    {
        public static string UploadFile(IFormFile file, string path, string? name = null)
        {
            Guid guid = Guid.NewGuid();
            string fileName = string.IsNullOrEmpty(name) ? guid.ToString() + Path.GetExtension(file.FileName) : name + Path.GetExtension(file.FileName);
            string directoryPath = Path.Combine("wwwroot", path);

            // Directory yoksa oluştur
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string filePath = Path.Combine(directoryPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return fileName;
        }

        public static byte[] GetUploadFile(IFormFile src)
        {
            byte[] fileBytes = null;
            var fileName = ContentDispositionHeaderValue.Parse(src.ContentDisposition).FileName.Trim('"');

            using (var fileStream = src.OpenReadStream())
            using (var ms = new MemoryStream())
            {
                fileStream.CopyTo(ms);
                fileBytes = ms.ToArray();
            }
            return fileBytes;
        }

        public static string GetImageSrc(byte[] fileBytes)
        {
            var base64 = Convert.ToBase64String(fileBytes);
            var imgsrc = string.Format("data:image/jpg;base64,{0}", base64);
            return imgsrc;
        }

        public static bool DeleteFile(string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                return true;
            }
            return false;
        }
    }
}
