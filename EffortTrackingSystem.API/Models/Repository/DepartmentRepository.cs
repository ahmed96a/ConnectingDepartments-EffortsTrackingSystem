using EffortTrackingSystem.API.Models.Repository.IRepository;
using EffortTrackingSystem.Models.Dtos;
using EffortTrackingSystem.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace EffortTrackingSystem.API.Models.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext db;

        public DepartmentRepository(AppDbContext _db)
        {
            db = _db;
        }

        public bool CreateDepartment(Department department)
        {
            db.Departments.Add(department);
            return save();
        }

        public bool DeleteDepartment(Department department)
        {
            db.Departments.Remove(department);
            return save();
        }

        public ICollection<Department> GetAllDepartments()
        {
            return db.Departments.Include(x => x.Users).OrderBy(a => a.Name).ToList();
        }

        public Department GetDepartment(int DepartmentId)
        {
            return db.Departments.FirstOrDefault(a => a.Id == DepartmentId);
        }

        public bool IsDepartmentExists(string DepartmentName)
        {
            bool result = db.Departments.Any(a => a.Name.ToLower().Trim() == DepartmentName.ToLower().Trim());
            return result;
        }

        public bool IsDepartmentExists(int DepartmentId)
        {
            return db.Departments.Any(a => a.Id == DepartmentId);
        }

        public bool UpdateDepartment(UpdateDepartmentDto updateDepartmentDto)
        {
            var departmentDB = db.Departments.FirstOrDefault(x => x.Id == updateDepartmentDto.Id);

            departmentDB.Name = updateDepartmentDto.Name;

            return save();
        }
        
        public bool save()
        {
            return db.SaveChanges() > 0 ? true : false;
        }
    }
}
