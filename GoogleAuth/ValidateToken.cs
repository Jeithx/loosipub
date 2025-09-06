using System.Net.Http.Headers;

namespace CorexPack.Utilities.GoogleAuth
{
    public static class ValidateToken
    {
        public static async Task<bool> ValidateGoogleIdToken(string idToken)
        {
            var client = new HttpClient();
            var response = await client.GetAsync($"https://oauth2.googleapis.com/tokeninfo?id_token={idToken}");

            if (response.IsSuccessStatusCode)
            {
                var tokenInfo = await response.Content.ReadAsStringAsync();
                return true;
            }

            return false;
        }

        public static async Task<bool> ValidateGoogleAccessToken(string accessToken)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync("https://www.googleapis.com/oauth2/v3/userinfo");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var tokenInfo = await response.Content.ReadAsStringAsync();
                return true;
            }

            return false;
        }
    }
}
