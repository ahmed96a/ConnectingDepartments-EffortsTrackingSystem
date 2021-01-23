using System;
using System.Collections.Generic;
using System.Text;

namespace EffortTrackingSystem.Models.Dtos
{
    public class MissionsCountDto
    {
        public int WaitingCount { get; set; }

        public int RunningCount { get; set; }

        public int CompletedCount { get; set; }

        public int RefusedCount { get; set; }

        public int ApprovedCount { get; set; }
    }
}
