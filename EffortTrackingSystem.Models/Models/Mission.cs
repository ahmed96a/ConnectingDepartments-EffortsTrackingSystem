using EffortTrackingSystem.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EffortTrackingSystem.Models.Models
{
    public class Mission
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        [StringLength(100, MinimumLength = 3)]
        public string Description { get; set; }

        [ForeignKey("Sender")]
        public string SenderId { get; set; }

        // Optional, in case the sender doesn't specify a receiver.
        [ForeignKey("Receiver")]
        public string ReceiverId { get; set; }

        // 0 => Low, 1 => Normal, 2 => High, 3 => Urgent
        public byte Priority { get; set; } = 1;

        [Required]
        public DateTime Task_Date { get; set; }

        [Required]
        public DateTime Expected_Deadline { get; set; }

        [Required]
        public DateTime Task_CompleteDate { get; set; }

        // 0 => not seen, 1 => approved , 2 => refused/declined)
        public byte TaskState { get; set; } = 0;

        public bool Is_Completed { get; set; } = false;

        public bool Is_Approved { get; set; } = false;

        [MaxLength(500)]
        [StringLength(500)]
        public string Cancel_Reason { get; set; }

        public string Attachment_Url { get; set; }

        [ForeignKey("Category")]
        public Int16 Category_Id { get; set; }

        public ApplicationUser Sender { get; set; }

        public ApplicationUser Receiver { get; set; }

        public Category Category { get; set; }
    }
}
