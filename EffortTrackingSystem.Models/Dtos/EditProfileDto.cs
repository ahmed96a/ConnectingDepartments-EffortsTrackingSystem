using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EffortTrackingSystem.Models.Dtos
{
    public class EditProfileDto
    {
        [Required]
        public string Id { get; set; }


        [RegularExpression("[a-zA-Z][a-zA-Z ]+", ErrorMessage = "Invalid, Only Letters and spaces are avilable")]
        [StringLength(100, MinimumLength = 5)]
        public string FullName { get; set; }


        [RegularExpression("^[0-9]+$", ErrorMessage = "Invalid, Only Numbes are Allowed")]
        [StringLength(15, MinimumLength = 11)]
        public string PhoneNumber { get; set; }


        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }


        [StringLength(100, MinimumLength = 5)]
        public string JobTitle { get; set; }


        [RegularExpression("^[0-9]+$", ErrorMessage = "Invalid, Only Numbes are Allowed")]
        [StringLength(15, MinimumLength = 11)]
        [Display(Name = "PhoneNumber 2(Optional)")]
        public string Phone2 { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }


        [StringLength(20, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }


        [StringLength(20, MinimumLength = 6)]
        [Compare("NewPassword")]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }
    }
}
