using Core.Entities.DTO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;

namespace Core.Utilities.Recaptcha
{
    public class RecaptchaHelper
    {
        private readonly IConfiguration _configuration;

        public RecaptchaHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool Recaptcha(object response)
        {
            bool isActive = _configuration["GoogleRecaptcha:IsActive"] == null ? false : bool.Parse(_configuration["GoogleRecaptcha:IsActive"]);

            if (!isActive)
                return true;

            string secretKey = _configuration["GoogleRecaptcha:secret-key"];

            if (string.IsNullOrEmpty(secretKey))
                return false;

            var client = new WebClient();
            var GoogleReply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            var captchaResponse = JsonConvert.DeserializeObject<reCaptcha>(GoogleReply);
            if (captchaResponse.Success)
                return true;
            else
                return false;
        }
    }
}
