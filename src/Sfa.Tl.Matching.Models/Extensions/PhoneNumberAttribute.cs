using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sfa.Tl.Matching.Models.Extensions
{
    public class PhoneNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (IsRequired && value == null)
                return new ValidationResult($"You must enter a telephone number for the {FieldName} number");

            if (!IsRequired && value == null)
                return ValidationResult.Success;

            var phoneNumber = value.ToString();

            if (IsRequired && string.IsNullOrWhiteSpace(phoneNumber))
                return new ValidationResult($"You must enter a telephone number for the {FieldName} number");
            if (!phoneNumber!.Any(char.IsDigit))
                return new ValidationResult("You must enter a telephone number using numbers");
            if (!Regex.IsMatch(phoneNumber, @"^(?:.*\d.*){7,}$"))
                return new ValidationResult("You must enter a telephone number that has 7 or more numbers");
            return ValidationResult.Success;
        }

        public string FieldName { get; set; }
        public bool IsRequired { get; set; }
    }
}