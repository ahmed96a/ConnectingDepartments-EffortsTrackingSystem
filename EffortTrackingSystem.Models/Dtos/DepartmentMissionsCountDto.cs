using System;
using System.Collections.Generic;
using System.Text;

namespace EffortTrackingSystem.Models.Dtos
{
    public class DepartmentMissionsCountDto
    {
        // S => Sending, R => Receiver
        public int SWaitingCount { get; set; }

        public int SRunningCount { get; set; }

        public int SCompletedCount { get; set; }

        public int SRefusedCount { get; set; }

        public int SApprovedCount { get; set; }

        public int RWaitingCount { get; set; }

        public int RRunningCount { get; set; }

        public int RCompletedCount { get; set; }

        public int RRefusedCount { get; set; }

        public int RApprovedCount { get; set; }
    }
}
