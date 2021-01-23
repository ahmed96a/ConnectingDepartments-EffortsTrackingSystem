using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EffortTrackingSystem.Models.Dtos
{
    public class CreateRoleDto
    {
        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string Name { get; set; }
    }
}
