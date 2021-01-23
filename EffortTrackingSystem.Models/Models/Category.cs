using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EffortTrackingSystem.Models.Models
{
    public class Category
    {
        public Int16 Id { get; set; }

        [Required]
        [MaxLength(100)]
        [StringLength(100, MinimumLength = 5)]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        [StringLength(500)]
        public string Description { get; set; }

        [ForeignKey("Department")]
        public Int16 DepartmentId { get; set; }

        public Department Department { get; set; }

        public ICollection<Mission> Missions { get; set; }

    }
}
