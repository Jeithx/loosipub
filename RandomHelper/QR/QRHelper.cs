using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using System.Drawing;
using System.Drawing.Imaging;

namespace Core.Utilities.RandomHelper.QR
{
    public static class QRHelper
    {
        public static string? QRGenerate(string title, string filePath, int? scale = 500)
        {
            if (!string.IsNullOrEmpty(title) || !string.IsNullOrEmpty(filePath))
            {
                QRCodeEncoder encoder = new QRCodeEncoder();
                var result = encoder.Encode(title);
                encoder.QRCodeScale = (int)scale!;
                title = title.Replace("http://", ""); //Linkler için url başı silinir, root ile karışıyor.
                title = title.Replace("https://", "");
                var filename = title.ToLower().Trim() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png"; // Dosya adını değiştirdim
                filePath = "wwwroot/" + filePath + "/" + filename; // Dosya yolu oluşturuldu

                // Klasör varlığını kontrol et
                var directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // PNG formatında kaydet
                result.Save(filePath, ImageFormat.Png);
                return filename;
            }
            else
                return null;
        }

        public static string QRDecode(Bitmap bitmap)
        {
            QRCodeDecoder decoder = new QRCodeDecoder();
            string decodedText = decoder.Decode(new QRCodeBitmapImage(bitmap));

            if (!string.IsNullOrEmpty(decodedText))
            {
                return decodedText;
            }
            else
            {
                return "QR kodu çözülemedi.";
            }
        }
    }
}
