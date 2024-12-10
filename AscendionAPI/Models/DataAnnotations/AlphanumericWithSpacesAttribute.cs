using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AscendionAPI.Models.DataAnnotations;

public class AlphanumericWithSpacesAttribute : ValidationAttribute
{
    // Regular expression to allow letters, digits, and spaces
    private const string Pattern = @"^[a-zA-Z0-9 ]*$";

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        // Allow null or empty values (use [Required] for mandatory fields)
        if (value == null || string.IsNullOrEmpty(value.ToString()))
        {
            return ValidationResult.Success;
        }

        // Check if the value matches the regex
        if (Regex.IsMatch(value.ToString(), Pattern))
        {
            return ValidationResult.Success;
        }

        // Validation failed, return error message
        return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} can only contain letters, digits, and spaces.");
    }
}

