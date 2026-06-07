using System.ComponentModel.DataAnnotations;

namespace CollegeApp.Validations
{
    public class DateCheckAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var date = (DateTime?)value;

            if(date < DateTime.Now)
            {
                return new ValidationResult(ErrorMessage = "The Admission date must not be in Past");
            }

            return ValidationResult.Success;
        }
    }
}
