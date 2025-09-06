using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Core.Attributes;

public class CustomRequiredAttribute : ValidationAttribute
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomRequiredAttribute()
    {
        _httpContextAccessor = new HttpContextAccessor();
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            var queryString = httpContext.Request.Query["properties"];
            var propertiesList = queryString.ToString().Split(',');

            if (propertiesList.Length > 0)
            {
                return ValidationResult.Success; // Eğer alan properties parametresinde varsa, doğrulama atlanır
            }
        }

        if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
        {
            return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} boş olamaz");
        }

        return ValidationResult.Success;
    }
}
