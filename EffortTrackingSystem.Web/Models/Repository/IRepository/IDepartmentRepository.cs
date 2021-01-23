using EffortTrackingSystem.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EffortTrackingSystem.Web.Models.Repository.IRepository
{
    public interface IDepartmentRepository
    {
        Task<string> CreateAsync(string Url, CreateDepartmentDto createDepartmentDto, string token);

        Task<object> GetAllAsync(string Url);

        Task<object> GetAsync(string Url, int DepartmentId, string token);

        Task<string> UpdateAsync(string Url, UpdateDepartmentDto updateDepartmentDto, string token);

        Task<string> DeleteAsync(string Url, int DepartmentId, string token);
    }
}
