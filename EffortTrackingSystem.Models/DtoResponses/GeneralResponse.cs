using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EffortTrackingSystem.Models.DtoResponses
{
    public class GeneralResponse
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}
