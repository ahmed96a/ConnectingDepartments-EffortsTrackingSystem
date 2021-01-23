using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EffortTrackingSystem.Models.Dtos
{
    public class MissionDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string SenderId { get; set; }

        public string ReceiverId { get; set; }

        public byte Priority { get; set; } = 1; // 0 => Low, 1 => Normal, 2 => High, 3 => Urgent

        public DateTime Task_Date { get; set; }

        public DateTime Expected_Deadline { get; set; }

        public DateTime Task_CompleteDate { get; set; }

        public byte TaskState { get; set; } = 0;

        public bool Is_Completed { get; set; }

        public bool Is_Approved { get; set; }

        public string Cancel_Reason { get; set; }

        public string Attachment_Url { get; set; }

        public Int16 Category_Id { get; set; }

        public CategoryDto Category { get; set; }

        public UserDto Sender { get; set; }

        public UserDto Receiver { get; set; }

    }
}
