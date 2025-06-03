using System.ComponentModel.DataAnnotations;

namespace Core;

public class User
{
    [Required]
    public string Role { get; set; }
    [Required]
    public string FullName { get; set; }
    [Required]
    [Password]
    public string Password { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [PhoneLimit]
    public string PhoneNumber { get; set; }
    [Required]
    [RequiredAge]
    public int Age { get; set; }
}

public class RequiredAge : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value != null)
        {
            if(value as int? < 18)
                return new ValidationResult("Cannot sign up if under 18 years old");
            
            if(value as int? > 100)
                return new ValidationResult("Age must be less than 100");
            
            return ValidationResult.Success;
        }

        return new ValidationResult("Age must exist i guess");
    }
}

public class PhoneLimit : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value != null)
        {
            if(value.ToString().Any(char.IsWhiteSpace))
            {
                return new ValidationResult("Phone number must not contain spaces");
            }
            if (!int.TryParse(value as string, out int phoneNumber))
            {
                return new ValidationResult("Phone number can only contain numbers");
            }
            if (value.ToString().Length != 8)
            {
                return new ValidationResult("Phone number must consist of 8 digits");
            }
            return ValidationResult.Success;
        }
        return new ValidationResult("Phone number can only contain numbers");
    }
}

public class Password : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value != null)
        {
            var password = value as string;
            if(password.Length < 8)
                return new ValidationResult("Password must contain at least 8 characters");
            if(!password.Any(char.IsUpper))
                return new ValidationResult("Password must contain at least 1 uppercase letter");
            if(!password.Any(char.IsAsciiDigit))
                return new ValidationResult("Password must contain at least 1 digit");
            return ValidationResult.Success;
        }

        return new ValidationResult("Password must exist i guess");
    }
}