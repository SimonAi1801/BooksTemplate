using Books.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Books.Core.Validations
{
    public class IsbnValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(value is string isbn))
            {
                throw new ArgumentException("Value is not a string", nameof(value));
            }

            if (!Book.CheckIsbn(isbn))
            {
                return new ValidationResult($"Isbn {isbn} is not valid!");
            }

            return ValidationResult.Success;
        }
    }
}
