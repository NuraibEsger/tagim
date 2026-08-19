using System.Text.RegularExpressions;
using Tagim.Domain.Enums;

namespace Tagim.Application.Extensions;

public static class LicensePlatePatterns
{
    private const string Letters = "ABCDEGHJKLMNOPRSTUVXYZ";
    
    public const string Car = @"^\d{2}-[" + Letters + @"]{2}-\d{3}$";               
    public const string PublicTransport = Car;                                      
    public const string Motorcycle = @"^\d{2}\s[" + Letters + @"]\s\d{3}$";          
    public const string Trailer = @"^\d{2}\s[" + Letters + @"]{2}\s\d{3}$";          
    public const string Diplomat = @"^\d{3}\s[" + Letters + @"]\s\d{3}$";            
    public const string ForeignCompany = @"^[" + Letters + @"]\s\d{3}\s\d{3}$";
    
    public static readonly string Any = $"({Car})|({Motorcycle})|({Trailer})|({Diplomat})|({ForeignCompany})";
    
    public static bool IsValid(string? plate)
    {
        if (string.IsNullOrWhiteSpace(plate))
            return false;

        return Regex.IsMatch(plate.Trim().ToUpper(), $"^(?:{Any})$");
    }
    
    public static bool IsValidForType(string? plate, VehicleType type)
    {
        if (string.IsNullOrWhiteSpace(plate))
            return false;

        var pattern = type switch
        {
            VehicleType.Car => Car,
            VehicleType.PublicTransport => PublicTransport,
            VehicleType.Motorcycle => Motorcycle,
            VehicleType.Trailer => Trailer,
            VehicleType.Diplomat => Diplomat,
            VehicleType.ForeignCompany => ForeignCompany,
            _ => Any
        };

        return Regex.IsMatch(plate.Trim().ToUpper(), pattern);
    }
}