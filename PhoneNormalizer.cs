using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Tool
{
    public class PhoneNormalizer
    {
        public static string NormalizePhoneNumber(string rawPhone)
        {
            if (string.IsNullOrWhiteSpace(rawPhone))
                return null;

            // Tüm karakterlerden sadece rakamları al
            string digitsOnly = Regex.Replace(rawPhone, @"\D", "");

            // 90 ile başlıyorsa zaten ülke kodu var demektir
            if (digitsOnly.StartsWith("90") && digitsOnly.Length == 12)
            {
                return $"+{digitsOnly}";
            }

            // 0 ile başlıyorsa Türkiye numarasıdır, baştaki 0'ı at ve 90 ekle
            if (digitsOnly.StartsWith("0") && digitsOnly.Length == 11)
            {
                return $"+90{digitsOnly.Substring(1)}";
            }

            // Eğer numara 10 haneliyse (örneğin 5050500555), 90 ekle
            if (digitsOnly.Length == 10)
            {
                return $"+90{digitsOnly}";
            }

            // Başka durumlar varsa: geçersiz numara
            return null;
        }
    }
}
