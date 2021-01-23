using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EffortTrackingSystem.API.Models.Repository.IRepository;
using EffortTrackingSystem.Models.Dtos;
using EffortTrackingSystem.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EffortTrackingSystem.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public class CategoryController : ControllerBase
    {
        private ICategoryRepository categoryRepo;
        private readonly IMapper mapper;

        public CategoryController(ICategoryRepository categoryRepo, IMapper mapper)
        {
            this.categoryRepo = categoryRepo;
            this.mapper = mapper;
        }

        //[Authorize]
        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CategoryDto>))]
        public IActionResult GetAllCategories(int? departmentId)
        {
            try
            {
                var categories = categoryRepo.GetAllCategories(departmentId);
                var categoriesDto = new List<CategoryDto>();
                foreach (var curr in categories)
                {
                    categoriesDto.Add(mapper.Map<CategoryDto>(curr));
                }
                return Ok(categoriesDto);
            }
            catch(Exception ex)
            {
                #region Production Environment
                // In Production Environment : -
                // Log in exception.
                //return StatusCode(StatusCodes.Status500InternalServerError, "Error In Performing operation");
                #endregion

                // In Development Environment : -
                var exception = ex.InnerException != null ? ex.Message + "\r\n\r\n" + ex.InnerException.Message : ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, exception);
            }
        }


        //[Authorize]
        [HttpGet("{categoryId:int}", Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategory(int categoryId)
        {
            try
            {
                var category = categoryRepo.GetCategory(categoryId);
                if (category == null)
                {
                    return NotFound();
                }
                var categoryDto = mapper.Map<CategoryDto>(category);
                return Ok(categoryDto);
            }
            catch (Exception ex)
            {
                #region Production Environment
                // In Production Environment : -
                // Log in exception.
                //return StatusCode(StatusCodes.Status500InternalServerError, "Error In Performing operation");
                #endregion

                // In Development Environment : -
                var exception = ex.InnerException != null ? ex.Message + "\r\n\r\n" + ex.InnerException.Message : ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, exception);
            }
        }


        //[Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CategoryDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            try
            {
                #region Comment
                if (createCategoryDto == null)
                {
                    return BadRequest(ModelState);
                }
                #endregion

                if (categoryRepo.IsCategoryExists(createCategoryDto.Name))
                {
                    //ModelState.AddModelError("", "This category already exists!");
                    return BadRequest("This category already exists!");
                }

                var category = mapper.Map<Category>(createCategoryDto);

                if (!categoryRepo.CreateCategory(category))
                {
                    //ModelState.AddModelError("", $"something went wrong while saving category {category.Name}");
                    return StatusCode(500, $"something went wrong while saving category {category.Name}");
                }

                var categoryDto = mapper.Map<CategoryDto>(category);
                return CreatedAtRoute("GetCategory", new { version = HttpContext.GetRequestedApiVersion().ToString(), categoryId = categoryDto.Id }, categoryDto);
            }
            catch (Exception ex)
            {
                #region Production Environment
                // In Production Environment : -
                // Log in exception.
                //return StatusCode(StatusCodes.Status500InternalServerError, "Error In Performing operation");
                #endregion

                // In Development Environment : -
                var exception = ex.InnerException != null ? ex.Message + "\r\n\r\n" + ex.InnerException.Message : ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, exception);
            }
        }


        //[Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{categoryId:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateCategory(int categoryId, [FromBody] UpdateCategoryDto updateCategoryDto)
        {
            try
            {
                if (updateCategoryDto == null || categoryId != updateCategoryDto.Id)
                {
                    return BadRequest("Bad Parameters");
                }

                if (!categoryRepo.UpdateCategory(updateCategoryDto))
                {
                    //ModelState.AddModelError("", $"something went wrong while updating category {updateCategoryDto.Name}");
                    return StatusCode(500, $"something went wrong while updating category {updateCategoryDto.Name}");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                #region Production Environment
                // In Production Environment : -
                // Log in exception.
                //return StatusCode(StatusCodes.Status500InternalServerError, "Error In Performing operation");
                #endregion

                // In Development Environment : -
                var exception = ex.InnerException != null ? ex.Message + "\r\n\r\n" + ex.InnerException.Message : ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, exception);
            }
        }


        //[Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("{categoryId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteCategory(int categoryId)
        {
            try
            {
                if (!categoryRepo.IsCategoryExists(categoryId))
                {
                    return NotFound();
                }

                var category = categoryRepo.GetCategory(categoryId);

                if (!categoryRepo.DeleteCategory(category))
                {
                    //ModelState.AddModelError("", $"something went wrong while deleting category {category.Name}");
                    return StatusCode(500, $"something went wrong while deleting category {category.Name}");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                #region Production Environment
                // In Production Environment : -
                // Log in exception.
                //return StatusCode(StatusCodes.Status500InternalServerError, "Error In Performing operation");
                #endregion

                // In Development Environment : -
                var exception = ex.InnerException != null ? ex.Message + "\r\n\r\n" + ex.InnerException.Message : ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, exception);
            }
        }
    }
}
