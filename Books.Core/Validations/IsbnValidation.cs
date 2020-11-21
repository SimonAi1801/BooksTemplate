using System.ComponentModel.DataAnnotations;

namespace Books.Core.Validations
{
  public class IsbnValidation : ValidationAttribute
  {
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      return ValidationResult.Success;
    }
  }
}
