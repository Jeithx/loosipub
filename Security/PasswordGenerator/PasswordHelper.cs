using System.Text;

namespace Core.Utilities.Security.PasswordGenerator
{
    public static class PasswordHelper
    {
        private static readonly Random _random = new Random();
        private const string UpperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string LowerChars = "abcdefghijklmnopqrstuvwxyz";
        private const string Digits = "0123456789";
        private const string SpecialChars = "!@#$%^&*()-_=+<>?";

        public static string GeneratePassword(int length = 8)
        {
            if (length < 8) throw new ArgumentException("Şifre uzunluğu en az 8 olmalıdır.");

            var password = new StringBuilder();

            password.Append(UpperChars[_random.Next(UpperChars.Length)]);
            password.Append(LowerChars[_random.Next(LowerChars.Length)]);
            password.Append(Digits[_random.Next(Digits.Length)]);
            password.Append(SpecialChars[_random.Next(SpecialChars.Length)]);

            string allChars = UpperChars + LowerChars + Digits + SpecialChars;
            for (int i = 4; i < length; i++)
            {
                password.Append(allChars[_random.Next(allChars.Length)]);
            }

            return new string(password.ToString().ToCharArray().OrderBy(x => _random.Next()).ToArray());
        }
    }
}
