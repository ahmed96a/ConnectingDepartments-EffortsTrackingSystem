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
using Microsoft.Extensions.Configuration;

namespace EffortTrackingSystem.Web.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public DepartmentController(IDepartmentRepository departmentRepository, IConfiguration configuration, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        //[Authorize(Policy = "UserPolicy")]
        
        public IActionResult Index()
        {
            try
            {
                var result = _departmentRepository.GetAllAsync(_configuration["APIConstants:DepartmentAPIPath"]).Result;
                if(result is List<DepartmentDto>)
                {
                    var departmentDtos = (List<DepartmentDto>)result;
                    return View(departmentDtos);
                }

                ViewBag.ApiException = result.ToString();
                return View(new List<DepartmentDto>());
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
                return View(new List<DepartmentDto>());
            }
        }


        


        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDepartmentDto model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var result = await _departmentRepository.CreateAsync(_configuration["APIConstants:DepartmentAPIPath"], model, HttpContext.Session.GetString("JWToken"));

                    if(result == null)
                    {
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("", result);
                }
                return View(model);
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
                ModelState.AddModelError("", exception);
                return View(model);
            }
        }


        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _departmentRepository.GetAsync(_configuration["APIConstants:DepartmentAPIPath"], id, HttpContext.Session.GetString("JWToken"));

            if (result is DepartmentDto)
                return View(_mapper.Map<UpdateDepartmentDto>((DepartmentDto)result));

            return View(new UpdateDepartmentDto());
        }


        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateDepartmentDto updateDepartmentDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _departmentRepository.UpdateAsync(_configuration["APIConstants:DepartmentAPIPath"], updateDepartmentDto, HttpContext.Session.GetString("JWToken"));

                    if (result == null)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.error = (string)result;
                        //ModelState.AddModelError("", (string)result);
                    }
                }
                return View(updateDepartmentDto);
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
                return View(updateDepartmentDto);
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _departmentRepository.GetAsync(_configuration["APIConstants:DepartmentAPIPath"], id, HttpContext.Session.GetString("JWToken"));

            if (result is DepartmentDto)
                return View((DepartmentDto)result);

            return View(new DepartmentDto());
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _departmentRepository.DeleteAsync(_configuration["APIConstants:DepartmentAPIPath"], id, HttpContext.Session.GetString("JWToken"));

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

        #region Details
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
        //        var result = await _departmentRepository.GetAsync(_configuration["APIConstants:DepartmentAPIPath"], id);

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
