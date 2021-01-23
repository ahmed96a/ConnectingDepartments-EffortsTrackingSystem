using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EffortTrackingSystem.API.Models.Repository.IRepository;
using EffortTrackingSystem.Models.DtoResponses;
using EffortTrackingSystem.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EffortTrackingSystem.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public class AccountController : ControllerBase
    {
        private IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        #region Comments
        // There is no need to check if ModelState is valid or check if model equal null, because by default in .Net Core WebAPI,
        // if there is a model validation error, a badrequest contains the model validations messages will be returned automatically, without even hitting the action method.
        #endregion

        public AccountController(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralResponse))]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            try
            {
                var accountRegisterResponse = await _accountRepository.RegisterUserAsync(model);

                if (accountRegisterResponse.IsSuccess)
                {
                    return Ok(accountRegisterResponse);
                }

                return BadRequest(accountRegisterResponse);
            }
            catch (Exception ex)
            {
                #region In Production Environment : -
                // Log in exception.
                //return StatusCode(StatusCodes.Status500InternalServerError, "Error In Performing operation");
                #endregion

                // In Development Environment : -
                var exception = ex.InnerException != null ? ex.Message + "\r\n\r\n" + ex.InnerException.Message : ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, exception);
            }
        }


        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AccountLoginResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AccountLoginResponse))]
        public async Task<IActionResult> Login(LoginDto model)
        {
            try
            {
                var accountLoginResponse = await _accountRepository.LoginUserAsync(model);

                if (accountLoginResponse.IsSuccess)
                {
                    return Ok(accountLoginResponse);
                }

                return BadRequest(accountLoginResponse);
            }
            catch (Exception ex)
            {
                #region In Production Environment : -
                // Log in exception.
                //return StatusCode(StatusCodes.Status500InternalServerError, "Error In Performing operation");
                #endregion
                
                // In Development Environment: -
                var exception = ex.InnerException != null ? ex.Message + "\r\n\r\n" + ex.InnerException.Message : ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, exception);
            }
        }


        //[Authorize]
        [HttpPut("[action]/{userId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralResponse))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> EditProfile(string userId, EditProfileDto editProfileDto)
        {
            try
            {
                if (userId != editProfileDto.Id)
                    return BadRequest();

                var result = await _accountRepository.EditProfileAsync(editProfileDto);
                if (result.IsSuccess == false)
                {
                    return BadRequest(result);
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

        // --------------------------

        // ---------------------------

        #region Add Roles

        //[Authorize(Roles = "SuperAdmin")]
        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<RoleDto>))]
        public IActionResult GetAllRoles()
        {
            try
            {
                var identityRoles = _accountRepository.GetAllRoles();

                var roleDtos = new List<RoleDto>();
                foreach (var role in identityRoles)
                {
                    roleDtos.Add(_mapper.Map<RoleDto>(role));
                }

                return Ok(roleDtos);
            }
            catch (Exception ex)
            {
                #region In Production Environment : -
                // Log in exception.
                //return StatusCode(StatusCodes.Status500InternalServerError, "Error In Performing operation");
                #endregion

                // In Development Environment: -
                var exception = ex.InnerException != null ? ex.Message + "\r\n\r\n" + ex.InnerException.Message : ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, exception);
            }
        }


        //[Authorize(Roles = "SuperAdmin")]
        [HttpGet("[action]/{roleId}", Name = "GetRole")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoleDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetRole(string roleId)
        {
            try
            {
                if (roleId == null)
                {
                    return BadRequest();
                }
                var identityRole = _accountRepository.GetRole(roleId);
                if (identityRole == null)
                {
                    return NotFound();
                }

                var roleDto = _mapper.Map<RoleDto>(identityRole);

                return Ok(roleDto);
            }
            catch (Exception ex)
            {
                #region In Production Environment : -
                // Log in exception.
                //return StatusCode(StatusCodes.Status500InternalServerError, "Error In Performing operation");
                #endregion

                // In Development Environment: -
                var exception = ex.InnerException != null ? ex.Message + "\r\n\r\n" + ex.InnerException.Message : ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, exception);
            }
        }


        //[Authorize(Roles = "SuperAdmin")]
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RoleDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralResponse))]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto createRoleDto)
        {
            try
            {
                #region Comment
                /*
                if (createDepartmentDto == null)
                {
                    return BadRequest(ModelState);
                }
                */
                #endregion

                var identityRole = _mapper.Map<IdentityRole>(createRoleDto);

                var result = await _accountRepository.CreateRoleAsync(identityRole);
                if (result.IsSuccess == false)
                {
                    return BadRequest(result);
                }

                var roleDto = _mapper.Map<RoleDto>(identityRole);

                return CreatedAtRoute("GetRole", new { version = HttpContext.GetRequestedApiVersion().ToString(), roleId = roleDto.Id }, roleDto);
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


        //[Authorize(Roles = "SuperAdmin")]
        [HttpPut("[action]/{roleId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateRole(string roleId, [FromBody] UpdateRoleDto updateRoleDto)
        {
            try
            {
                #region Comment
                /*
                if (createDepartmentDto == null)
                {
                    return BadRequest(ModelState);
                }
                */
                #endregion

                if (roleId != updateRoleDto.Id)
                {
                    return BadRequest();
                }

                var identityRole = _mapper.Map<IdentityRole>(updateRoleDto);

                var result = await _accountRepository.UpdateRoleAsync(identityRole);
                if (result.IsSuccess == false)
                {
                    return BadRequest(result);
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


        //[Authorize(Roles = "SuperAdmin")]
        [HttpDelete("[action]/{roleId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralResponse))]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            try
            {
                if (!_accountRepository.IsRoleExists(roleId))
                {
                    return NotFound();
                }

                var identityRole = _accountRepository.GetRole(roleId);

                var result = await _accountRepository.DeleteRoleAsync(identityRole);

                if (result.IsSuccess == false)
                {
                    return BadRequest(result);
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

        // ---------------------------

        #endregion
    }
}
