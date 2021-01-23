using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EffortTrackingSystem.Models.Dtos
{
    public class CreateMissionDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Description { get; set; }

        [Required]
        public string SenderId { get; set; }

        [Display(Name = "Receiver (Optional)")]
        public string ReceiverId { get; set; }

        // 0 => Low, 1 => Normal, 2 => High, 3 => Urgent  
        public byte Priority { get; set; } = 1;

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Expected Deadline")]
        public DateTime Expected_Deadline { get; set; }

        [Display(Name = "Attachment")]
        public IFormFile Attachment_Url { get; set; }

        [Required]
        [Display(Name = "Department")]
        public Int16? Department_Id { get; set; }

        [Required]
        [Display(Name = "Category")]
        public Int16? Category_Id { get; set; }
    }
}
