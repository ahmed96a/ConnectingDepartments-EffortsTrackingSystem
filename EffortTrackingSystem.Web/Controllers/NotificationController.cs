using EffortTrackingSystem.Models.Dtos;
using EffortTrackingSystem.Web.Models.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EffortTrackingSystem.Web.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IConfiguration _configuration;

        public NotificationController(INotificationRepository notificationRepository, IConfiguration configuration)
        {
            _notificationRepository = notificationRepository;
            _configuration = configuration;
        }

        public async Task<IActionResult> GetNotifications()
        {
            try
            {
                var result = await _notificationRepository.GetNotificationsAsync(_configuration["APIConstants:NotificationAPIPath"], Request.Cookies["JWToken"]);

                if (result is List<NotificationDto>)
                {
                    return View((List<NotificationDto>)result);
                }

                ViewBag.ApiException = result;
                return View(new List<NotificationDto>());
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

        public async Task<ActionResult> OpenNotification(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }

                var result = await _notificationRepository.OpenNotificationAsync(_configuration["APIConstants:NotificationAPIPath"], id.Value, Request.Cookies["JWToken"]);

                if (result is NotificationDto)
                {
                    return View((NotificationDto)result);
                }
                else if(result is null)
                {
                    return NotFound();
                }

                ViewBag.ApiException = result;
                return View(new NotificationDto());
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

        public async Task<ActionResult> MarkAsRead(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }

                var result = await _notificationRepository.MarkAsReadAsync(_configuration["APIConstants:NotificationAPIPath"], id.Value, Request.Cookies["JWToken"]);

                if (result is bool)
                {
                    return RedirectToAction("GetNotifications");
                }
                else if(result == null)
                {
                    return NotFound();
                }

                ViewBag.ApiException = result;
                return RedirectToAction("GetNotifications");
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
