using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EffortTrackingSystem.Models.DtoResponses;
using EffortTrackingSystem.Models.Dtos;
using EffortTrackingSystem.Web.Models.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EffortTrackingSystem.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IMissionRepository _missionRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public DashboardController(IMissionRepository missionRepository, IDepartmentRepository departmentRepository, IUserRepository userRepository, IConfiguration configuration)
        {
            _missionRepository = missionRepository;
            _departmentRepository = departmentRepository;
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> GetAllDepartmentJson(int? departmentId)
        {
            try
            {
                var resultDepartments = await _departmentRepository.GetAllAsync(_configuration["APIConstants:DepartmentAPIPath"]);
                if (resultDepartments is List<DepartmentDto>)
                {
                    var departmentDtos = (List<DepartmentDto>)resultDepartments;

                    return Json(departmentDtos);
                }

                ViewBag.ApiException = resultDepartments.ToString();
                return Json(new List<DepartmentDto>());
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
        public async Task<IActionResult> Index()
        {
            try
            {
                var departmentId = User.FindFirst(x => x.Type == "DepartmentId").Value;
                var Url = _configuration["APIConstants:MissionAPIPath"] + $"GetAllDepartmentMissionsCount?departmentId={departmentId}";
                var result = await _missionRepository.GetAllDepartmentMissionsCountAsync(Url, HttpContext.Session.GetString("JWToken"));

                if (result is DepartmentMissionsCountDto)
                {
                    return View((DepartmentMissionsCountDto)result);
                }

                ViewBag.ApiException = result;
                return View(new DepartmentMissionsCountDto());
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

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> AssignTaskToUser()
        {
            try
            {
                var departmentId = User.FindFirst(x => x.Type == "DepartmentId").Value;
                int? categoryId = null;
                var missionType = "Received";
                string userId = null;
                var Url = _configuration["APIConstants:MissionAPIPath"] + $"GetAllFilteredMissions?departmentId={departmentId}&categoryId={categoryId}&missionType={missionType}&userId={userId}";
                var result = await _missionRepository.GetAllFilteredMissionsAsync(Url, HttpContext.Session.GetString("JWToken"));

                if (result is List<MissionDto>)
                {
                    return View((List<MissionDto>)result);
                }

                ViewBag.ApiException = result;
                return View(new List<MissionDto>());
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


        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> AssignDetails(int? id) // mission id
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }

                var result = await _missionRepository.GetMissionAsync(_configuration["APIConstants:MissionAPIPath"], id.Value, HttpContext.Session.GetString("JWToken"));

                if (result is null)
                {
                    return NotFound();
                }

                if (result is MissionDto)
                {
                    return View((MissionDto)result);
                }

                ViewBag.ApiException = result;
                return View(new MissionDto());
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


        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> AssignDetails(string userId, int? missionId)
        {
            try
            {
                if (userId == null || missionId == null)
                {
                    return BadRequest();
                }

                var result = await _missionRepository.AssignTaskToUserAsync(_configuration["APIConstants:MissionAPIPath"], userId, missionId.Value, HttpContext.Session.GetString("JWToken"));

                if (result == null)
                {
                    return RedirectToAction("AssignTaskToUser");
                }

                ViewBag.ApiException = result;
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
                //ModelState.AddModelError("", exception);
                ViewBag.WebException = exception;
                return View();
            }
        }


        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> DepartmentTasks(int? id, string name) // department id
        {
            try
            {
                ViewBag.name = name;
                if (id == null)
                {
                    return BadRequest();
                }

                var result = await _missionRepository.GetAllMissionsForDepartmentAsync(_configuration["APIConstants:MissionAPIPath"], id.Value, HttpContext.Session.GetString("JWToken"));

                if (result is List<MissionDto>)
                {
                    return View((List<MissionDto>)result);
                }

                ViewBag.ApiException = result;
                return View(new List<MissionDto>());
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
                return View(new List<MissionDto>());
            }
        }


        public async Task<IActionResult> MissionDetails(int? id, string name) // Get mission Details mission id
        {
            try
            {
                ViewBag.Department = name;
                if (id == null)
                {
                    return BadRequest();
                }

                var result = await _missionRepository.GetMissionAsync(_configuration["APIConstants:MissionAPIPath"], id.Value, HttpContext.Session.GetString("JWToken"));

                if (result is null)
                {
                    return NotFound();
                }

                if (result is MissionDto)
                {
                    return View((MissionDto)result);
                }

                ViewBag.ApiException = result;
                return View(new MissionDto());
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


        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        public IActionResult AddEmployee()
        {
            return View();
        }


        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> AddEmployee(CreateUserDto createUserDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var resetPasswordActionLink = Url.Action("ResetPassword", "User", null, Request.Scheme);
                    var result = await _userRepository.CreateUserAsync(_configuration["APIConstants:UserAPIPath"], createUserDto, resetPasswordActionLink, HttpContext.Session.GetString("JWToken"));

                    if (result == null)
                    {
                        return RedirectToAction("Index");
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


        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                var departmentId = Convert.ToInt32(User.FindFirst(x => x.Type == "DepartmentId").Value);
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
                return View(new List<UserDto>());
            }
        }


        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> DeleteEmployee(string id) // userId
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
                return View(new UserDto());
            }
        }


        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpPost, ActionName("DeleteEmployee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEmployeeConfirmed(string id)
        {
            try
            {
                var result = await _userRepository.DeleteUserAsync(_configuration["APIConstants:UserAPIPath"], id, HttpContext.Session.GetString("JWToken"));

                if (result is null)
                {
                    return RedirectToAction("GetEmployees");
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
