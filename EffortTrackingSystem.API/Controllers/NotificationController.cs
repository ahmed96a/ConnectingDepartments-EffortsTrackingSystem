using AutoMapper;
using EffortTrackingSystem.API.Models.Repository.IRepository;
using EffortTrackingSystem.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EffortTrackingSystem.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public class NotificationController : ControllerBase
    {
        private INotificationRepository notificationRepo;
        private readonly IMapper mapper;

        #region Comments
        // There is no need to check if ModelState is valid or check if model equal null, because by default in .Net Core WebAPI,
        // if there is a model validation error, a badrequest contains the model validations messages will be returned automatically, without even hitting the action method.
        #endregion

        public NotificationController(INotificationRepository notificationRepository, IMapper mapper)
        {
            notificationRepo = notificationRepository;
            this.mapper = mapper;
        }

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<NotificationDto>))]
        public IActionResult GetNotifications()
        {
            try
            {
                var userId = User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var notifications = notificationRepo.GetNotifications(userId);
                var notificationsDto = new List<NotificationDto>();
                foreach (var curr in notifications)
                {
                    notificationsDto.Add(mapper.Map<NotificationDto>(curr));
                }
                return Ok(notificationsDto);
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


        [HttpGet("[action]/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NotificationDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult OpenNotification(int id)
        {
            try
            {
                var currentUserId = User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var notification = notificationRepo.OpenNotification(id, currentUserId);

                if (notification == null)
                    return NotFound();

                var notificationDto = mapper.Map<NotificationDto>(notification);
                return Ok(notificationDto);
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


        [HttpGet("[action]/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NotificationDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult MarkAsRead(int id)
        {
            try
            {
                var currentUserId = User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var result = notificationRepo.MarkAsRead(id, currentUserId);
                if (result == null)
                    return NotFound();
                return Ok();
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
