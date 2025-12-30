using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Tagim.Application.Extensions;

public static class FluentValidationImageExtensions
{
    private const int MaxFileSize = 5 * 1024 * 1024;
    
    private static readonly string[] AllowedContentTypes = { "image/jpeg", "image/jpg", "image/png" };

    public static IRuleBuilderOptions<T, IFormFile> IsValidImage<T>(this IRuleBuilder<T, IFormFile> ruleBuilder)
    {
        return ruleBuilder
            .NotNull().WithMessage("Fayl seçilməlidir.")
            .Must(file => file.Length > 0).WithMessage("Fayl boş ola bilməz.")
            .Must(file => file.Length <= MaxFileSize)
                .WithMessage("Şəklin həcmi 5MB-dan çox ola bilməz.")
            .Must(file => AllowedContentTypes.Contains(file.ContentType.ToLower()))
                .WithMessage("Yalnız .jpg, .jpeg və .png formatlarına icazə verilir.");
    }
}