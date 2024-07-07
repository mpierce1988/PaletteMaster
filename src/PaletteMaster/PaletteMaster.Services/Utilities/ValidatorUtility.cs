using System.ComponentModel.DataAnnotations;
using PaletteMaster.Models;

namespace PaletteMaster.Services.Utilities;

public static class ValidatorUtility
{
    public static List<ValidationResult>? ValidateObject(object request)
    {
        // Validate request parameters
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(request, null, null);

        if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
        {
            return validationResults;
        }

        return null;
    }
    
    public static bool TryValidateObject(object request, out List<ValidationResult> validationResults)
    {
        // Validate request parameters
        validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(request, null, null);

        return Validator.TryValidateObject(request, validationContext, validationResults, true);
    }
}