using EffortTrackingSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EffortTrackingSystem.Models.Dtos
{
    public class CategoryDto
    {
        public Int16 Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Int16 DepartmentId { get; set; }

        public DepartmentDto Department { get; set; }

        //public ICollection<Mission> Missions { get; set; }

    }
}
