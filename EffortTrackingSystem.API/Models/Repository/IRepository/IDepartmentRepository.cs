using EffortTrackingSystem.Models.Dtos;
using EffortTrackingSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EffortTrackingSystem.API.Models.Repository.IRepository
{
    public interface IDepartmentRepository
    {
        ICollection<Department> GetAllDepartments();

        Department GetDepartment(int DepartmentId);

        bool IsDepartmentExists(string DepartmentName);

        bool IsDepartmentExists(int DepartmentId);

        bool CreateDepartment(Department department);

        bool UpdateDepartment(UpdateDepartmentDto updateDepartmentDto);

        bool DeleteDepartment(Department department);

        bool save();


    }
}
