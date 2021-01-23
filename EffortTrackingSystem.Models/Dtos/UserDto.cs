using System;
using System.Collections.Generic;
using System.Text;

namespace EffortTrackingSystem.Models.Dtos
{
    public class UserDto
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime HireDate { get; set; }

        public string JobTitle { get; set; }

        public string Phone2 { get; set; }

        public DepartmentDto Department { get; set; }

        public IList<string> Roles { get; set; }
    }
}
