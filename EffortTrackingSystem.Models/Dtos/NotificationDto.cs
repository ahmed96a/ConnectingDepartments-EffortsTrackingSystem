using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EffortTrackingSystem.Models.Dtos
{
    public class NotificationDto
    {
        public int Id { get; set; }

        public string ReceiverId { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Details { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
