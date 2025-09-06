using System.Text.RegularExpressions;

namespace Core.Utilities.ToStringForSeo
{
    public class ToStringForSeo
    {
        public string Replace(string inputString)
        {
            inputString = inputString.Replace("Ç", "c");
            inputString = inputString.Replace("ç", "c");
            inputString = inputString.Replace("ğ", "g");
            inputString = inputString.Replace("Ğ", "g");
            inputString = inputString.Replace("Ð", "g");
            inputString = inputString.Replace("ð", "g");
            inputString = inputString.Replace("İ", "i");
            inputString = inputString.Replace("I", "i");
            inputString = inputString.Replace("i", "i");
            inputString = inputString.Replace("ı", "i");
            inputString = inputString.Replace("ý", "i");
            inputString = inputString.Replace("Ý", "i");
            inputString = inputString.Replace("Ö", "o");
            inputString = inputString.Replace("ö", "o");
            inputString = inputString.Replace("Þ", "s");
            inputString = inputString.Replace("þ", "s");
            inputString = inputString.Replace("ş", "s");
            inputString = inputString.Replace("Ş", "s");
            inputString = inputString.Replace("Ü", "u");
            inputString = inputString.Replace("ü", "u");
            inputString = inputString.Replace(" ", "");

            inputString = inputString.Trim().ToLower();
            inputString = Regex.Replace(inputString, @"\s+", "-");
            inputString = Regex.Replace(inputString, @"[^A-Za-z0-9_-]", "");
            inputString = inputString.Replace(" ", "-");

            return inputString;
        }
    }
}
