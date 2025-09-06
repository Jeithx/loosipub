namespace Core.Utilities.RandomCodeGenerator
{
    public class RandomCodeGenerator
    {
        public string GenerateCode()
        {
            var random = new Random();
            const string characters = "0123456789";
            return new string(Enumerable.Repeat(characters, 9)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
