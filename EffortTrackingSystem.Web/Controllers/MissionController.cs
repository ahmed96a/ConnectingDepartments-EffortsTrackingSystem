using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EffortTrackingSystem.Models.Dtos;
using EffortTrackingSystem.Web.Models.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace EffortTrackingSystem.Web.Controllers
{
    public class MissionController : Controller
    {
        private readonly IMissionRepository _missionRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IConfiguration _configuration;

        public MissionController(IMissionRepository missionRepository, IDepartmentRepository departmentRepository, IConfiguration configuration)
        {
            _missionRepository = missionRepository;
            _departmentRepository = departmentRepository;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ManageReceivedTasks()
        {
            try
            {
                var departmentName = User.FindFirst(x => x.Type == "DepartmentName").Value.ToLower().Trim();
                if (departmentName != "hr" && departmentName != "graphic design")
                    return RedirectToAction("Index", "Home");

                var userId = User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var missionType = "received";
                var Url = _configuration["APIConstants:MissionAPIPath"] + $"GetAllFilteredMissionsCount?missionType={missionType}&userId={userId}";
                var result = await _missionRepository.GetAllFilteredMissionsCountAsync(Url, HttpContext.Session.GetString("JWToken"));

                if (result is MissionsCountDto)
                {
                    return View((MissionsCountDto)result);
                }

                ViewBag.ApiException = result;
                return View(new MissionsCountDto());
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
        [HttpGet]
        public async Task<IActionResult> ManageSentTasks()
        {
            try
            {
                var userId = User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var missionType = "Sended";
                var Url = _configuration["APIConstants:MissionAPIPath"] + $"GetAllFilteredMissionsCount?missionType={missionType}&userId={userId}";
                var result = await _missionRepository.GetAllFilteredMissionsCountAsync(Url, HttpContext.Session.GetString("JWToken"));

                if (result is MissionsCountDto)
                {
                    return View((MissionsCountDto)result);
                }

                ViewBag.ApiException = result;
                return View(new MissionsCountDto());
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

        public async Task<IActionResult> GetAllMissionsForDepartment(int? id) // department id
        {
            try
            {
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
                return View();
            }
        }

        public async Task<IActionResult> GetAllMissionsForCategory(int? id) // category id
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }

                var result = await _missionRepository.GetAllMissionsForCategoryAsync(_configuration["APIConstants:MissionAPIPath"], id.Value, HttpContext.Session.GetString("JWToken"));

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

        public async Task<IActionResult> GetAllSendedMissionsOfUser(string id) // user id
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }

                var result = await _missionRepository.GetAllSendedMissionsOfUserAsync(_configuration["APIConstants:MissionAPIPath"], id, HttpContext.Session.GetString("JWToken"));

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

        public async Task<IActionResult> GetAllReceivedMissionsOfUser(string id) // user id
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }

                var result = await _missionRepository.GetAllReceivedMissionsOfUserAsync(_configuration["APIConstants:MissionAPIPath"], id, HttpContext.Session.GetString("JWToken"));

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

        // Method to collect all the previous methods
        public async Task<IActionResult> GetAllFilteredMissions(int? departmentId, int? categoryId, string missionType, string missionState, string userId)
        {
            ViewBag.taskState = missionState.ToUpper();
            ViewBag.missionType = missionType;
            try
            {
                var Url = _configuration["APIConstants:MissionAPIPath"] + $"GetAllFilteredMissions?departmentId={departmentId}&categoryId={categoryId}&missionType={missionType}&missionState={missionState}&userId={userId}";
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

        public async Task<IActionResult> GetAllFilteredMissionsCount(string missionType, string missionState, string userId)
        {
            try
            {
                var Url = _configuration["APIConstants:MissionAPIPath"] + $"GetAllFilteredMissionsCount?missionType={missionType}&missionState={missionState}&userId={userId}";
                var result = await _missionRepository.GetAllFilteredMissionsCountAsync(Url, HttpContext.Session.GetString("JWToken"));

                if (result is MissionsCountDto)
                {
                    return View((MissionsCountDto)result);
                }

                ViewBag.ApiException = result;
                return View(new MissionsCountDto());
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

        public async Task<IActionResult> GetMission(int? id) // mission id
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }

                var result = await _missionRepository.GetMissionAsync(_configuration["APIConstants:MissionAPIPath"], id.Value, HttpContext.Session.GetString("JWToken"));

                if(result is null)
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

        [HttpGet]
        public async Task<IActionResult> SendMission()
        {
            var resultDepartments = await _departmentRepository.GetAllAsync(_configuration["APIConstants:DepartmentAPIPath"]);

            if (resultDepartments is List<DepartmentDto>)
            {
                var departmentDtos = (List<DepartmentDto>)resultDepartments;
                ViewBag.departments = new SelectList(departmentDtos.Where(x => x.Name.ToLower().Trim() == "hr" || x.Name.ToLower().Trim() == "graphic design").ToList(), "Id", "Name"); ;
            }
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMission(CreateMissionDto createMissionDto)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var result = await _missionRepository.SendMissionAsync(_configuration["APIConstants:MissionAPIPath"], createMissionDto, HttpContext.Session.GetString("JWToken"));

                    if (result == null)
                    {

                        return RedirectToAction("Index","Home");
                    }

                    ViewBag.ApiException = result;
                }
                var resultDepartments = await _departmentRepository.GetAllAsync(_configuration["APIConstants:DepartmentAPIPath"]);

                if (resultDepartments is List<DepartmentDto>)
                {
                    var departmentDtos = (List<DepartmentDto>)resultDepartments;
                    ViewBag.departments = new SelectList(departmentDtos.Where(x => x.Name.ToLower().Trim() == "hr" || x.Name.ToLower().Trim() == "graphicdesign").ToList(), "Id", "Name"); ;
                }
                return View(createMissionDto);
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

        [HttpPost]
        public async Task<IActionResult> AcceptMission(int? id) // mission id
        {
            try
            {
                if(id == null)
                {
                    return BadRequest();
                }

                var result = await _missionRepository.AcceptMissionAsync(_configuration["APIConstants:MissionAPIPath"], id.Value, HttpContext.Session.GetString("JWToken"));

                if (result == null)
                {
                    return RedirectToAction("ManageReceivedTasks");
                }

                ViewBag.ApiException = result; // Use Temp data
                return RedirectToAction("ManageReceivedTasks");
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

        [HttpPost]
        public async Task<IActionResult> RefuseMission(int? id) // mission id
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }

                var result = await _missionRepository.RefuseMissionAsync(_configuration["APIConstants:MissionAPIPath"], id.Value, HttpContext.Session.GetString("JWToken"));

                if (result == null)
                {
                    return RedirectToAction("ManageReceivedTasks");
                }

                ViewBag.ApiException = result; // Use Temp data
                return RedirectToAction("ManageReceivedTasks");
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

        [HttpPost]
        public async Task<IActionResult> CompleteMission(int? id) // mission id
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }

                var result = await _missionRepository.CompleteMissionAsync(_configuration["APIConstants:MissionAPIPath"], id.Value, HttpContext.Session.GetString("JWToken"));

                if (result == null)
                {
                    return RedirectToAction("ManageReceivedTasks");
                }

                ViewBag.ApiException = result; // Use Temp data
                return RedirectToAction("ManageReceivedTasks");
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

        [HttpPost]
        public async Task<IActionResult> ApproveMission(int? id) // mission id
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }

                var result = await _missionRepository.ApproveMissionAsync(_configuration["APIConstants:MissionAPIPath"], id.Value, HttpContext.Session.GetString("JWToken"));

                if (result == null)
                {
                    return RedirectToAction("ManageSentTasks");
                }

                ViewBag.ApiException = result; // Use Temp data
                return RedirectToAction("ManageSentTasks");
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
    }
}
