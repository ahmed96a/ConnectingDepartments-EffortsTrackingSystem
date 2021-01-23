using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using EffortTrackingSystem.Models.DtoResponses;
using EffortTrackingSystem.Models.Dtos;
using EffortTrackingSystem.Web.Models.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.Extensions.Configuration;

namespace EffortTrackingSystem.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}


        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> GetAllUsers(int? departmentId)
        {
            try
            {
                var result = await _userRepository.GetAllUsersAsync(_configuration["APIConstants:UserAPIPath"], departmentId, HttpContext.Session.GetString("JWToken"));

                if (result is List<UserDto>)
                {
                    return View((List<UserDto>)result);
                }

                ViewBag.ApiException = result;
                return View(new List<UserDto>());
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
                return View();
            }
        }


        [Authorize]
        public async Task<IActionResult> GetAllNonAdminUsersJson(int? departmentId)
        {
            try
            {
                var result = await _userRepository.GetAllNonAdminUsersAsync(_configuration["APIConstants:UserAPIPath"], departmentId, HttpContext.Session.GetString("JWToken"));

                if (result is List<UserDto>)
                {
                    var userDtos = (List<UserDto>)result;
                    
                    return Json(userDtos);
                }

                ViewBag.ApiException = result;
                return Json(new List<UserDto>());
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
                return Json(new List<UserDto>());
            }
        }

        [Authorize]
        public async Task<IActionResult> GetUser(string id)
        {
            try
            {
                if (id == null)
                    return BadRequest();

                var result = await _userRepository.GetUserAsync(_configuration["APIConstants:UserAPIPath"], id, HttpContext.Session.GetString("JWToken"));

                if(result is null)
                {
                    return NotFound();
                }

                if (result is UserDto)
                {
                    return View((UserDto)result);
                }

                ViewBag.ApiException = result;
                return View(new UserDto());
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
                return View();
            }
        }


        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();        
        }


        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var resetPasswordActionLink = Url.Action("ResetPassword", "User", null, Request.Scheme);
                    var result = await _userRepository.CreateUserAsync(_configuration["APIConstants:UserAPIPath"], createUserDto, resetPasswordActionLink, HttpContext.Session.GetString("JWToken"));

                    if (result == null)
                    {
                        return RedirectToAction("GetAllUsers");
                    }
                    else if (result is GeneralResponse)
                    {
                        var generalResponse = (GeneralResponse)result;
                        foreach (var error in generalResponse.Errors)
                        {
                            ModelState.AddModelError("", error);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", result.ToString());
                    }
                }
                return View(createUserDto);
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
                return View();
            }
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _userRepository.ResetPasswordAsync(_configuration["APIConstants:UserAPIPath"], resetPasswordDto, HttpContext.Session.GetString("JWToken"));

                    if (result == null)
                    {
                        return RedirectToAction("Index","Home");
                    }
                    else if (result is GeneralResponse)
                    {
                        var generalResponse = (GeneralResponse)result;
                        foreach (var error in generalResponse.Errors)
                        {
                            ModelState.AddModelError("", error);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", result.ToString());
                    }
                }
                return View(resetPasswordDto);
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
                return View();
            }
        }


        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id) // userId
        {
            try
            {
                if (id == null)
                    return BadRequest();

                var result = await _userRepository.GetUserAsync(_configuration["APIConstants:UserAPIPath"], id, HttpContext.Session.GetString("JWToken"));

                if (result is null)
                    return NotFound();

                if (result is UserDto)
                    return View((UserDto)result);

                return View(new UserDto());
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
                return View();
            }
        }


        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserConfirmed(string id)
        {
            try
            {
                var result = await _userRepository.DeleteUserAsync(_configuration["APIConstants:UserAPIPath"], id, HttpContext.Session.GetString("JWToken"));

                if(result is null)
                {
                    return RedirectToAction("GetAllUsers");
                }
                else if(result is GeneralResponse)
                {
                    var generalResponse = (GeneralResponse)result;
                    foreach (var error in generalResponse.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
                else
                {
                    ViewBag.error = (string)result;
                }
                return View();
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
    }
}
