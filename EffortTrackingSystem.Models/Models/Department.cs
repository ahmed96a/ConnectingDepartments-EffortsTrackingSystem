using EffortTrackingSystem.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EffortTrackingSystem.Models.Models
{
    public class Department
    {
        public Int16 Id { get; set; }

        [Required]
        [MaxLength(100)]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        public ICollection<Category> Categories { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
    }
}
