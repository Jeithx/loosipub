using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace CoreXNugetPackage.Utilities.Cloudinary
{
    public static class CloudinaryHelper
    {
        // içine string verirsen bunu geçici temp dosya oluşturup döner. daha sonra bu dosyayı cloudinary'ye upload edebilirsin
        public static async Task<string> WriteContentToFile(string jsonContent)
        {
            var tempFileName = Path.GetTempFileName().Replace(".tmp", ".json"); // Geçici bir dosya yolu oluştur
            await File.WriteAllTextAsync(tempFileName, jsonContent);
            return tempFileName;
        }

        public async static Task<string> UploadFileToCloudinary(string filePath, CloudinaryDotNet.Cloudinary _cloudinary)
        {
            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(filePath),
                PublicId = "order_payload_" + Guid.NewGuid().ToString(), // Benzersiz bir Public ID
                Invalidate = true
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUrl.ToString();  // Güvenli bir URL döner
        }
    }

    public class CloudinarySettings
    {
        public string CloudName { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
    }


    #region Kullanım örnegi - Dependency ile enjekte et
    //    if (file != null) //Update olurken yüklenmediyse null gelir, eski veri update edilmez
    //        {
    //            ImageUploadResult uploadResult;
    //            using (var stream = file.OpenReadStream())
    //            {
    //                var imageUploadParams = new ImageUploadParams
    //                {
    //                    File = new FileDescription(file.FileName, stream)
    //                };

    //// Cloudinary'e asenkron olarak dosya yüklemesi yapılıyor.
    //uploadResult = await _cloudinary.UploadAsync(imageUploadParams);
    //educationContentRd.Image = uploadResult.Url.ToString();
    //            }
    //        }



    //Appsetting içeriği

    //"CloudinarySettings": {
    //  "CloudName": "",
    //  "ApiKey": "",
    //  "ApiSecret": ""
    //},
    #endregion
}
