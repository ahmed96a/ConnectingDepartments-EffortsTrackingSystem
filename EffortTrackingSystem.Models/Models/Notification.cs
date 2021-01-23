using EffortTrackingSystem.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EffortTrackingSystem.Models.Models
{
    public class Notification
    {
        public int Id { get; set; }

        //public string SenderId { get; set; }

        //public string SenderEmail { get; set; }

        public string ReceiverId { get; set; }

        //public string ReceiverEmail { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Details { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedDate { get; set; }

        // 1-to-* relation with ApplicationUser Table. one Notification belong to one receiver.
        [ForeignKey("ReceiverId")]
        public virtual ApplicationUser Receiver { get; set; }
    }
}
