using EffortTrackingSystem.API.Models.Repository.IRepository;
using EffortTrackingSystem.Models.Dtos;
using EffortTrackingSystem.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EffortTrackingSystem.API.Models.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext db;

        public CategoryRepository(AppDbContext _db)
        {
            db = _db;
        }

        public bool CreateCategory(Category category)
        {
            db.Categories.Add(category);
            return save();
        }

        public bool DeleteCategory(Category category)
        {
            db.Categories.Remove(category);
            return save();
        }

        public ICollection<Category> GetAllCategories(int? departmentId)
        {
            var categoriesIQuerable = db.Categories.AsQueryable();
            if(departmentId != null)
            {
                categoriesIQuerable = categoriesIQuerable.Where(cat => cat.DepartmentId == departmentId);
            }
            return categoriesIQuerable.Include(x => x.Department).ToList();
        }

        public Category GetCategory(int CategoryId)
        {
            return db.Categories.FirstOrDefault(a => a.Id == CategoryId);
        }

        public bool IsCategoryExists(string CategoryName)
        {
            bool result = db.Categories.Any(a => a.Name.ToLower().Trim() == CategoryName.ToLower().Trim());
            return result;
        }

        public bool IsCategoryExists(int CategoryId)
        {
            return db.Categories.Any(a => a.Id == CategoryId);
        }

        public bool UpdateCategory(UpdateCategoryDto updateCategoryDto)
        {
            var categoryDB = db.Categories.FirstOrDefault(x => x.Id == updateCategoryDto.Id);

            categoryDB.Name = updateCategoryDto.Name;
            categoryDB.Description = updateCategoryDto.Description;

            return save();
        }

        public bool save()
        {
            return db.SaveChanges() > 0 ? true : false;
        }
    }
}
