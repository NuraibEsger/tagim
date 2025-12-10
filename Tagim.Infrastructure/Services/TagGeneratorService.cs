using Tagim.Application.Interfaces;

namespace Tagim.Infrastructure.Services;

public class TagGeneratorService : ITagGeneratorService
{
    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    
    public string GenerateTags()
    {
        var randomString = new string(Enumerable.Repeat(Chars, 8)
            .Select(s => s[Random.Shared.Next(s.Length)]).ToArray());

        return $"TAG-{randomString}";
    }
}