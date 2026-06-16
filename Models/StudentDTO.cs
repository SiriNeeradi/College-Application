using CollegeApp.Validations;
using System.ComponentModel.DataAnnotations;

namespace CollegeApp.Models
{
    public class StudentDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Nmae should not be empty")]
        [StringLength(30)]
        public string StudentName { get; set; }
        [Range(18, 22)]
        public int Age { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Address should not be empty")]
        public string Address { get; set; }

        public DateTime DOB { get; set; }

        //public string Password { get; set; }
        //[Compare(nameof(Password))]
        //public string ConfirmPassword { get; set; }
        //[DateCheck]
        //public DateTime AdmissionDate { get; set; }

    }
}
