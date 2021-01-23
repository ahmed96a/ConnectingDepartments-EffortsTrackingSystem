using EffortTrackingSystem.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EffortTrackingSystem.Web.Models.Repository.IRepository
{
    public interface ICategoryRepository
    {
        Task<object> GetAllAsync(string Url, int? departmentId, string token);
        Task<string> CreateAsync(string Url, CreateCategoryDto createCategoryDto, string token);

        Task<object> GetAsync(string Url, int CategoryId, string token);

        Task<string> UpdateAsync(string Url, UpdateCategoryDto updateCategoryDto, string token);
        Task<string> DeleteAsync(string Url, int CategoryId, string token);
    }
}
