using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EffortTrackingSystem.Models.Dtos;
using EffortTrackingSystem.Web.Models.Repository;
using EffortTrackingSystem.Web.Models.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace EffortTrackingSystem.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IConfiguration configuration;
        private readonly IDepartmentRepository departmentRepository;
        private readonly IMapper mapper;

        public CategoryController(ICategoryRepository categoryRepository, IConfiguration configuration, IDepartmentRepository departmentRepository, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.configuration = configuration;
            this.departmentRepository = departmentRepository;
            this.mapper = mapper;
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var departmentId = Convert.ToInt16(User.FindFirst(x => x.Type == "DepartmentId").Value);
                var resultCategories = await categoryRepository.GetAllAsync(configuration["APIConstants:CategoryAPIPath"], departmentId, HttpContext.Session.GetString("JWToken"));
                if (resultCategories is List<CategoryDto>)
                {
                    var categoryDtos = (List<CategoryDto>)resultCategories;

                    return View(categoryDtos);
                }

                ViewBag.ApiException = resultCategories.ToString();
                return View(new List<CategoryDto>());
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
                //ModelState.AddModelError("", exception);
                ViewBag.WebException = exception;
                return View(new List<CategoryDto>());
            }
        }

        [Authorize]
        public async Task<IActionResult> GetAllCategoryJson(int? departmentId)
        {
            try
            {
                var resultCategories = await categoryRepository.GetAllAsync(configuration["APIConstants:CategoryAPIPath"], departmentId, HttpContext.Session.GetString("JWToken"));
                if (resultCategories is List<CategoryDto>)
                {
                    var categoryDtos = (List<CategoryDto>)resultCategories;

                    return Json(categoryDtos);
                }

                ViewBag.ApiException = resultCategories.ToString();
                return Json(new List<CategoryDto>());
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
                //ModelState.AddModelError("", exception);
                ViewBag.WebException = exception;
                return Json(new List<CategoryDto>());
            }
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var resultDepartments = await departmentRepository.GetAllAsync(configuration["APIConstants:DepartmentAPIPath"]);

            if (resultDepartments is List<DepartmentDto>)
            {
                var departmentDtos = (List<DepartmentDto>)resultDepartments;
                ViewBag.departments = new SelectList(departmentDtos, "Id", "Name"); ;
            }
            return View();
        }


        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await categoryRepository.CreateAsync(configuration["APIConstants:CategoryAPIPath"], model, HttpContext.Session.GetString("JWToken"));

                    if (result == null)
                    {
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("", result);
                }
                return View(model);
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
                ModelState.AddModelError("", exception);
                return View(model);
            }
        }


        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // Get List of department
            var resultDepartments = await departmentRepository.GetAllAsync(configuration["APIConstants:DepartmentAPIPath"]);

            if (resultDepartments is List<DepartmentDto>)
            {
                var departmentDtos = (List<DepartmentDto>)resultDepartments;
                ViewBag.departments = new SelectList(departmentDtos, "Id", "Name"); ;
            }

            // Get the Category
            var result = await categoryRepository.GetAsync(configuration["APIConstants:CategoryAPIPath"], id, HttpContext.Session.GetString("JWToken"));

            if (result is CategoryDto)
                return View(mapper.Map<UpdateCategoryDto>((CategoryDto)result));

            return View(new UpdateCategoryDto());
        }


        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateCategoryDto updateCategoryDto)
        {
            try
            {
                var result = await categoryRepository.UpdateAsync(configuration["APIConstants:CategoryAPIPath"], updateCategoryDto, HttpContext.Session.GetString("JWToken"));

                if (result == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = (string)result;
                    //ModelState.AddModelError("", (string)result);
                }
                return View(updateCategoryDto);
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
                ModelState.AddModelError("", exception);
                return View(updateCategoryDto);
            }
        }


        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            // Get the Category
            var result = await categoryRepository.GetAsync(configuration["APIConstants:CategoryAPIPath"], id, HttpContext.Session.GetString("JWToken"));

            if (result is CategoryDto)
                return View((CategoryDto)result);

            return View(new CategoryDto());

        }


        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                // HttpContext.Session.GetString("JWToken")
                var result = await categoryRepository.DeleteAsync(configuration["APIConstants:CategoryAPIPath"], id, HttpContext.Session.GetString("JWToken"));

                if (result is string)
                {
                    ViewBag.error = (string)result;
                    return View();
                }
                else
                {
                    return RedirectToAction("Index");
                }

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
                ModelState.AddModelError("", exception);
                return View();
            }
        }

        #region
        //[HttpGet]
        //public IActionResult Get()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Get(int id)
        //{
        //    try
        //    {
        //        // HttpContext.Session.GetString("JWToken")
        //        var result = await categoryRepository.GetAsync(configuration["APIConstants:CategoryAPIPath"], id, "asd");

        //        if (result is string)
        //        {
        //            ViewBag.error = (string)result;
        //            //ModelState.AddModelError("", (string)result);
        //        }
        //        else
        //        {
        //            return View((DepartmentDto)result);
        //        }
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        #region Production Environment
        //        // In Production Environment : -
        //        // Log in exception.
        //        //return StatusCode(StatusCodes.Status500InternalServerError, "Error In Performing operation");
        //        #endregion

        //        // In Development Environment : -
        //        var exception = ex.InnerException != null ? ex.Message + "\r\n\r\n" + ex.InnerException.Message : ex.Message;
        //        ModelState.AddModelError("", exception);
        //        return View();
        //    }
        //}
        #endregion

    }
}
