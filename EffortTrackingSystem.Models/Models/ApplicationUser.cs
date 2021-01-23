using EffortTrackingSystem.Models.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EffortTrackingSystem.API.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        [MaxLength(100)]
        [StringLength(100, MinimumLength = 5)]
        public string FullName { get; set; }

        [Required]
        public DateTime HireDate { get; set; }

        [Required]
        [MaxLength(100)]
        [StringLength(100, MinimumLength = 5)]
        public string JobTitle { get; set; }

        [MaxLength(15)]
        [StringLength(15, MinimumLength = 11)]
        public string Phone2 { get; set; }

        public bool IsDeleted { get; set; } = false;

        [ForeignKey("Department")]
        public Int16 DepartmentId { get; set; }

        public Department Department { get; set; }

        // https://www.entityframeworktutorial.net/code-first/inverseproperty-dataannotations-attribute-in-code-first.aspx

        [InverseProperty("Sender")]
        public ICollection<Mission> Sent_Missions { get; set; }

        [InverseProperty("Receiver")]
        public ICollection<Mission> Received_Missions { get; set; }

        public ICollection<Notification> Notifications { get; set; }
    }
}
