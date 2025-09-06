using System.Text;

namespace Core.Utilities.RandomCodeGenerator;

public class OrderNumberHelper
{
    private static readonly Random _random = new Random();

    /// <summary>
    /// Benzersiz sipariş numarası oluşturur
    /// Format: YYYYMMDD-HHMMSS-XXXX (Tarih-Zaman-Random)
    /// </summary>
    /// <returns>Örnek: 20250424-153022-7B4C</returns>
    public static string GenerateOrderNumber()
    {
        var now = DateTime.UtcNow;
        var datePart = now.ToString("yyyyMMdd-HHmmss");
        var randomPart = GenerateRandomString(4);

        return $"{datePart}-{randomPart}";
    }

    private static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        StringBuilder result = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            result.Append(chars[_random.Next(chars.Length)]);
        }

        return result.ToString();
    }

}