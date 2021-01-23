using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EffortTrackingSystem.API.Models;
using EffortTrackingSystem.API.Models.Repository.IRepository;
using EffortTrackingSystem.Models.DtoResponses;
using EffortTrackingSystem.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EffortTrackingSystem.API.Controllers
{

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public class UserController : ControllerBase
    {
        private IUserRepository _userRepository;
        private readonly IMapper _mapper;

        #region Comments
        // There is no need to check if ModelState is valid or check if model equal null, because by default in .Net Core WebAPI,
        // if there is a model validation error, a badrequest contains the model validations messages will be returned automatically, without even hitting the action method.
        #endregion

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        //[Authorize]
        [HttpGet("[action]/{userId}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser(string userId)
        {
            try
            {
                var user = await _userRepository.GetUserAsync(userId);

                if (user != null)
                {
                    var userDto = _mapper.Map<UserDto>(user);
                    return Ok(userDto);
                }

                return NotFound();
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

        //[Authorize]
        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserDto>))]
        public IActionResult GetAllUsers(int? departmentId)
        {
            try
            {
                var users = _userRepository.GetAllUsers(departmentId);
                var userDtos = new List<UserDto>();
                foreach (var user in users)
                {
                    userDtos.Add(_mapper.Map<UserDto>(user));
                }

                return Ok(userDtos);
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserDto>))]
        public IActionResult GetAllNonAdminUsers(int? departmentId)
        {
            try
            {
                var users = _userRepository.GetAllNonAdminUsers(departmentId);
                var userDtos = new List<UserDto>();
                foreach (var user in users)
                {
                    userDtos.Add(_mapper.Map<UserDto>(user));
                }

                return Ok(userDtos);
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


        //[Authorize(Roles = "Admin, SuperAdmin")]
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralResponse))]
        public async Task<IActionResult> CreateUser([FromBody]CreateUserDto model, string passwordResetLink)
        {
            try
            {
                if(await _userRepository.UserExistsByEmail(model.Email))
                {
                    return BadRequest(new GeneralResponse
                    {
                        IsSuccess = false,
                        Message = "User is already registered."
                    });
                }

                // Fix this data
                // ----------------------
                var user = _mapper.Map<ApplicationUser>(model);

                user.UserName = user.Email;
                user.FullName = "";
                user.HireDate = DateTime.Now;
                user.JobTitle = "";
                //user.DepartmentId = Convert.ToInt16(User.FindFirst("DepartmentId").Value);
                user.DepartmentId = model.DepartmentId;
                // ----------------------

                var generalReponse = await _userRepository.CreateUserAsync(user, passwordResetLink);

                if (generalReponse.IsSuccess)
                {
                    var userDto = _mapper.Map<UserDto>(user);
                    return CreatedAtRoute("GetUser", new { version = HttpContext.GetRequestedApiVersion().ToString(), userId =  user.Id }, userDto);
                }

                return BadRequest(generalReponse);
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


        //[AllowAnonymous]
        [HttpPut("[action]")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralResponse))]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordDto model)
        {
            try
            {
                if (!await _userRepository.UserExistsByEmail(model.Email))
                    return NotFound("Email Not Found");

                var accountResponse = await _userRepository.ResetPasswordAsync(model);

                if (accountResponse.IsSuccess)
                {
                    return NoContent();
                }

                return BadRequest(accountResponse);
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


        //[Authorize(Roles = "Admin, SuperAdmin")]
        [HttpDelete("[action]/{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                // That not required because userId is required parameter not optional parameter (since userId is part of the route of that action method)
                // so we don't have to check if that parameter == null or not, because if the user doesn't provide that userId, the API will return 405 Not allowed method by default.
                /*
                if (userId == null)
                    return BadRequest();
                */

                if (!await _userRepository.UserExistsById(userId))
                    return NotFound("User Not Found");

                var user = await _userRepository.GetUserAsync(userId);

                var accountResponse = await _userRepository.DeleteUserAsync(user);

                if(accountResponse.IsSuccess)
                {
                    return NoContent();
                }

                return BadRequest(accountResponse);
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
