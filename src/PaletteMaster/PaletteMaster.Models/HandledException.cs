using System.ComponentModel.DataAnnotations;

namespace PaletteMaster.Models;

public struct HandledException
{
    public List<ValidationResult> ValidationResults { get; set; } = new();
    public string Message { get; set; }
    public bool IsValidationError => ValidationResults.Count > 0;

    public HandledException(string message)
    {
        Message = message;
    }
    
    public HandledException(List<ValidationResult> validationResults, string message = "Validation Failed")
    {
        ValidationResults = validationResults;
        Message = message;
    }
}