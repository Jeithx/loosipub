namespace Core.Utilities.GenerateUniqueUsername
{
    public static class GenerateUniqueUsername
    {
        public static string Generate(string firstName, string lastName)
        {
            string username = $"{firstName.ToLower()}.{lastName.ToLower()}";
            username = username.Replace(" ", "");
            string uniqueId = Guid.NewGuid().ToString("N").Substring(0, 6); // İlk altı karakter
            username = $"{username}.{uniqueId}";

            return username;
        }
    }
}
