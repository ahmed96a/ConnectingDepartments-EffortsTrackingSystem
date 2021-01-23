using System;
using System.Collections.Generic;
using System.Text;

namespace EffortTrackingSystem.Models.Dtos
{
    public class LoginAndRegisterDto
    {
        public LoginDto loginDto { set; get; }
        public RegisterDto registerDto { set; get; }
    }
}
