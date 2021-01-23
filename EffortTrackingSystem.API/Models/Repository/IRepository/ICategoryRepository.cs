using EffortTrackingSystem.Models.Dtos;
using EffortTrackingSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EffortTrackingSystem.API.Models.Repository.IRepository
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetAllCategories(int? departmentId);

        Category GetCategory(int CategoryId);

        bool IsCategoryExists(string CategoryName);

        bool IsCategoryExists(int CategoryId);

        bool CreateCategory(Category category);

        bool UpdateCategory(UpdateCategoryDto category);

        bool DeleteCategory(Category category);

        bool save();
    }
}
