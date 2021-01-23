using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EffortTrackingSystem.Models.Dtos
{
    public class CreateUserDto
    {
        [Required]
        //[EmailAddress]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email format")] // use eva domain
        [StringLength(50)]
        public string Email { get; set; }

        public Int16 DepartmentId { get; set; }
    }
}
