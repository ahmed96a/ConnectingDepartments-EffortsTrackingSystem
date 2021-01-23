using EffortTrackingSystem.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EffortTrackingSystem.Models.Dtos
{
    public class AccountLoginResponse
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public string Token { get; set; }

        public UserDto User { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}
