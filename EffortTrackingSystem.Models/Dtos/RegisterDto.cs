using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EffortTrackingSystem.Models.Dtos
{
    public class RegisterDto
    {
        /*
        [Required]
        [RegularExpression("^[a-zA-Z0-9_]+$", ErrorMessage = "Invalid, Only letters, digits and underscore are available")]
        [StringLength(15, MinimumLength = 5)]
        public string UserName { get; set; }
        */

        [Required]
        //[EmailAddress]
        //[RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email format")] // use eva domain
        [RegularExpression("^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@evapharma.com$", ErrorMessage = "Invalid email, must end with evapharma.com")]
        [StringLength(50)]
        public string Email { get; set; }


        [Required]
        [RegularExpression("[a-zA-Z][a-zA-Z ]+", ErrorMessage = "Invalid, Only Letters and spaces are avilable")]
        [StringLength(100, MinimumLength = 5)]
        public string FullName { get; set; }


        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string JobTitle { get; set; }


        [Required]
        public Int16 DepartmentId { get; set; }


        [Required]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Invalid, Only Numbes are Allowed")]
        [StringLength(15, MinimumLength = 11)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Phone 2 (Optional)")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Invalid, Only Numbes are Allowed")]
        [StringLength(15, MinimumLength = 11)]
        public string Phone2 { get; set; }


        [Required]
        //[Range(typeof(DateTime), "01/01/1997", "01/01/2021")] // see part 82,81, MVC (we will need to create custom attribute)
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }


        [Required]
        [StringLength(20, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
