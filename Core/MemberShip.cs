using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace Core;

public class MemberShip
{
    [Required]
    [BsonId]
    public int MemberShipId { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    [ValidPrice]
    public double PricePrMonth { get; set; }
    public double? Discount { get; set; }
    
}

public class ValidPrice : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        double price = (double)value;

        if (price < 1)
        {
            return new ValidationResult("Price must be greater than 1 dollar");
        }
        return ValidationResult.Success;
        
    }
}