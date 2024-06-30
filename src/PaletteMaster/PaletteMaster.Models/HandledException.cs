using System.ComponentModel.DataAnnotations;

namespace PaletteMaster.Models;

public struct HandledException
{
    public ValidationResult ValidationResult { get; set; }
    public string Message { get; set; }
    public bool IsValidationError => ValidationResult != null;
}