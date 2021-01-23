using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using EffortTrackingSystem.API.Hubs;
using EffortTrackingSystem.API.Models.Repository.IRepository;
using EffortTrackingSystem.Models.Dtos;
using EffortTrackingSystem.Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace EffortTrackingSystem.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public class MissionController : ControllerBase
    {
        private IMissionRepository missionRepo;
        private IUserRepository userRepo;
        private INotificationRepository notificationRepo;
        private readonly IHubContext<NotificationHub> notificationHub;
        private readonly IMapper mapper;

        #region Comments
        // There is no need to check if ModelState is valid or check if model equal null, because by default in .Net Core WebAPI,
        // if there is a model validation error, a badrequest contains the model validations messages will be returned automatically, without even hitting the action method.
        #endregion

        public MissionController(IMissionRepository missionRepository, IUserRepository userRepository, INotificationRepository notificationRepository, IHubContext<NotificationHub> notificationHub, IMapper mapper)
        {
            missionRepo = missionRepository;
            userRepo = userRepository;
            notificationRepo = notificationRepository;
            this.notificationHub = notificationHub;
            this.mapper = mapper;
        }


        [HttpPut("AssignTaskToUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public IActionResult AssignTaskToUser(string userId, int missionId)
        {
            try
            {
                if(userId == null)
                {
                    return BadRequest("Bad Request, Bad Url");
                }

                if(!missionRepo.IsMissionExists(missionId) || !userRepo.UserExistsById(userId).Result)
                {
                    //return NotFound();
                    return NotFound("Mission not found.");
                }

                var result = missionRepo.AssignTaskToUser(userId, missionId);

                if (result)
                {
                    return Ok();
                }

                return BadRequest("Bad Request");
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


        [HttpGet("[action]/{departmentId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MissionDto>))]
        public IActionResult GetAllMissionsForDepartment(int departmentId)
        {
            try
            {
                var missions = missionRepo.GetAllMissionsForDepartment(departmentId);
                var missionsDto = new List<MissionDto>();
                foreach (var curr in missions)
                {
                    missionsDto.Add(mapper.Map<MissionDto>(curr));
                }
                return Ok(missionsDto);
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


        [HttpGet("[action]/{categoryId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MissionDto>))]
        public IActionResult GetAllMissionsForCategory(int categoryId)
        {
            try
            {
                var missions = missionRepo.GetAllMissionsForCategory(categoryId);
                var missionsDto = new List<MissionDto>();
                foreach (var curr in missions)
                {
                    missionsDto.Add(mapper.Map<MissionDto>(curr));
                }
                return Ok(missionsDto);
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

        // -----------

        [HttpGet("[action]/{missionId:int}", Name = "GetMission")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MissionDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetMission(int missionId)
        {
            try
            {
                var mission = missionRepo.GetMission(missionId);

                if (mission == null)
                {
                    return NotFound("Not Found");
                }

                var missionsDto = mapper.Map<MissionDto>(mission);
                return Ok(missionsDto);
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


        [HttpGet("[action]/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MissionDto>))]
        public IActionResult GetAllSendedMissionsOfUser(string userId)
        {
            try
            {
                var missions = missionRepo.GetAllSendedMissionsOfUser(userId);
                var missionsDto = new List<MissionDto>();
                foreach (var curr in missions)
                {
                    missionsDto.Add(mapper.Map<MissionDto>(curr));
                }
                return Ok(missionsDto);
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

        [HttpGet("[action]/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MissionDto>))]
        public IActionResult GetAllReceivedMissionsOfUser(string userId)
        {
            try
            {
                var missions = missionRepo.GetAllReceivedMissionsOfUser(userId);
                var missionsDto = new List<MissionDto>();
                foreach (var curr in missions)
                {
                    missionsDto.Add(mapper.Map<MissionDto>(curr));
                }
                return Ok(missionsDto);
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

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MissionDto>))]
        public IActionResult GetAllFilteredMissions(int? departmentId, int? categoryId, string missionType, string missionState, string userId)
        {
            try
            {
                var missions = missionRepo.GetAllFilteredMissions(departmentId, categoryId, missionType, missionState, userId);
                var missionsDto = new List<MissionDto>();
                foreach (var curr in missions)
                {
                    missionsDto.Add(mapper.Map<MissionDto>(curr));
                }
                return Ok(missionsDto);
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


        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MissionsCountDto))]
        public IActionResult GetAllFilteredMissionsCount(string missionType, string userId)
        {
            try
            {
                var missionsCountDto = missionRepo.GetAllFilteredMissionsCount(missionType, userId);
                return Ok(missionsCountDto);
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


        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DepartmentMissionsCountDto))]
        public IActionResult GetAllDepartmentMissionsCount(string departmentId)
        {
            try
            {
                var missionsCountDto = missionRepo.GetAllDepartmentMissionsCount(departmentId);
                return Ok(missionsCountDto);
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

        // -----------


        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(MissionDto))]
        public IActionResult SendMission([FromBody] CreateMissionDto createMissionDto)
        {
            try
            {
                var mission = mapper.Map<Mission>(createMissionDto);

                mission.TaskState = 0;
                mission.Is_Completed = false;
                mission.Is_Approved = false;
                mission.Task_Date = DateTime.Now;

                if (!missionRepo.CreateMission(mission))
                {
                    //ModelState.AddModelError("", $"something went wrong while sending mission (Task) {mission.Name}");
                    return StatusCode(500, $"something went wrong while sending mission (Task) {mission.Name}");
                }

                // Notification Module
                // -----------------------------------


                // if the mission is sent to specific employee, then create notification to this employee. 
                if (mission.ReceiverId != null)
                {
                    Notification notification = new Notification();

                    notification.ReceiverId = mission.ReceiverId;
                    notification.Subject = "New Mission Request.";

                    #region Set Priority
                    string result = "";
                    var priority = mission.Priority;
                    if (priority == 0)
                        result = "Low";
                    else if (priority == 1)
                        result = "Normal";
                    else if (priority == 2)
                        result = "High";
                    else if (priority == 3)
                        result = "Urgent";
                    #endregion

                    var senderName = userRepo.GetUserAsync(mission.SenderId).Result;
                    notification.Details = $"Request for a mission for you.<br>" +
                        $"Requester: {senderName}.<br>" +
                        $"Priority: {result}.<br>" +
                        $"Mission Date: {mission.Task_Date}.<br>" +
                        $"Mission Expected Deadline: {mission.Expected_Deadline}.<br>" +
                        $"Mission Subject: {mission.Name}.<br>" +
                        $"Mission Description: {mission.Description}.<br>";

                    notification.CreatedDate = DateTime.Now;
                    notification.IsRead = false;

                    if (notificationRepo.CreateNotification(notification))
                    {
                        notificationHub.Clients.User(notification.ReceiverId).SendAsync("NewNotification", notification.ReceiverId).Wait();
                    }
                }


                // -----------------------------------

                var missionDto = mapper.Map<MissionDto>(mission);
                return CreatedAtRoute("GetMission", new { version = HttpContext.GetRequestedApiVersion().ToString(), missionId = missionDto.Id }, missionDto);
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


        [HttpPut("[action]/{missionId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult AcceptMission(int missionId)
        {
            try
            {
                var mission = missionRepo.GetMission(missionId);

                if (mission == null)
                {
                    return NotFound("Mission Not Found");
                }

                mission.TaskState = 1;

                if (!missionRepo.save())
                {
                    //ModelState.AddModelError("", $"something went wrong while accepting mission (Task) {mission.Name}");
                    return StatusCode(500, $"something went wrong while accepting mission (Task) {mission.Name}");
                }

                //var missionDto = mapper.Map<MissionDto>(mission);
                //return Ok(missionDto);

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


        [HttpPut("[action]/{missionId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult RefuseMission(int missionId)
        {
            var mission = missionRepo.GetMission(missionId);

            if (mission == null)
            {
                return NotFound();
            }

            mission.TaskState = 2;

            if (!missionRepo.save())
            {
                //ModelState.AddModelError("", $"something went wrong while refusing mission (Task) {mission.Name}");
                return StatusCode(500, $"something went wrong while refusing mission (Task) {mission.Name}");
            }

            //var missionDto = mapper.Map<MissionDto>(mission);
            //return Ok(missionDto);
            return NoContent();
        }


        [HttpPut("[action]/{missionId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult CompleteMission(int missionId)
        {
            var mission = missionRepo.GetMission(missionId);
            if (mission == null)
            {
                return NotFound();
            }
            mission.Is_Completed = true;
            mission.Task_CompleteDate = DateTime.Now;

            if (!missionRepo.save())
            {
                //ModelState.AddModelError("", $"something went wrong while completing mission (Task) {mission.Name}");
                return StatusCode(500, $"something went wrong while completing mission (Task) {mission.Name}");
            }

            //var missionDto = mapper.Map<MissionDto>(mission);
            //return Ok(missionDto);
            return NoContent();
        }


        [HttpPut("[action]/{missionId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ApproveMission(int missionId)
        {
            var mission = missionRepo.GetMission(missionId);
            if (mission == null)
            {
                return NotFound();
            }
            mission.Is_Approved = true;

            if (!missionRepo.save())
            {
                //ModelState.AddModelError("", $"something went wrong while approving mission (Task) {mission.Name}");
                return StatusCode(500, $"something went wrong while approving mission (Task) {mission.Name}");
            }

            //var missionDto = mapper.Map<MissionDto>(mission);
            //return Ok(missionDto);
            return NoContent();
        }


        // UpdateMission()

        // DeleteMission()
    }
}
