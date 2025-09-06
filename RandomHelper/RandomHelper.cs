using MessagingToolkit.QRCode.Codec;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.RandomHelper
{
    public class RandomHelper
    {
        public static string GenerateUniqueGuid(int length)
        {
            Guid guid = Guid.NewGuid();
            string guidString = guid.ToString("N");

            if (length <= guidString.Length)
            {
                return guidString.Substring(0, length);
            }
            else
            {
                return guidString.PadRight(length, '0');
            }
        }
        public static (string, string) FullNameSmash(string fullName)
        {
            string[] parts = fullName.TrimStart().TrimEnd().Split(' ');
            if (string.IsNullOrEmpty(fullName))
            {
                return ("-", "");
            }
            switch (parts.Length)
            {
                case 2:
                    return (parts[0], parts[1]);
                case 3:
                    return (parts[0] + " " + parts[1], parts[2]);
                case 4:
                    return (parts[0] + " " + parts[1] + " " + parts[2], parts[3]);
                default:
                    return (fullName, "-");
            }
        }
        public static string ClearSpace(string title, int? length)
        {
            if (length == null)
            {
                return string.Concat(title.Where(c => !char.IsWhiteSpace(c)));
            }
            if (title.Length > length)
            {
                var cleanTitle = string.Concat(title.Where(c => !char.IsWhiteSpace(c)));
                return cleanTitle.Substring((int)(cleanTitle.Length - length));
            }
            else
                return title;
        }    

        #region Hash
        private const int SaltSize = 32; // Salt boyutu 32 byte
        private const int HashSize = 64; // Hash boyutu 64 byte
        private const int Iterations = 10000; // PBKDF2 için iterasyon sayısı
        public static string HashedString(string value)
        {
            // Salt oluştur
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            // Hash oluştur
            var pbkdf2 = new Rfc2898DeriveBytes(value, salt, Iterations);
            var hash = pbkdf2.GetBytes(HashSize);

            // Hash ve salt'ı birleştir
            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            // Base64 olarak dönüştür ve döndür
            var base64Hash = Convert.ToBase64String(hashBytes);

            return base64Hash;
        }

        // Hash doğrulama
        public static bool VerifyString(string value, string base64Hash)
        {
            // Hash'i byte dizisine dönüştür
            var hashBytes = Convert.FromBase64String(base64Hash);

            // Salt'ı al
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            // Hash'i oluştur
            var pbkdf2 = new Rfc2898DeriveBytes(value, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            // Hash'i doğrula
            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    return false; // Doğrulama başarısız
                }
            }
            return true; // Doğrulama başarılı
        }
        #endregion

        #region Encrytpe-Decrypte
        // AES şifreleme için anahtar ve IV oluştur
        private static readonly byte[] AesKey = Encoding.UTF8.GetBytes("d22574c85ddba4ec9be80abbb0d3269adb2763cfa8286e00de8acae465baf977");
        private static readonly byte[] AesIV = Encoding.UTF8.GetBytes("853ed4f2491b5982dca1f289da80afd6");

        public static string Encrypt(string plainText)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");

            string encrypted;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = AesKey;
                aesAlg.IV = AesIV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }

            return encrypted;
        }

        public static string Decrypt(string cipherText)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");

            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = AesKey;
                aesAlg.IV = AesIV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        #endregion
    }
}
