using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EffortTrackingSystem.API.Models.Repository.IRepository;
using EffortTrackingSystem.Models.Dtos;
using EffortTrackingSystem.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EffortTrackingSystem.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public class DepartmentController : ControllerBase
    {
        private IDepartmentRepository departmentRepo;
        private readonly IMapper mapper;

        public DepartmentController(IDepartmentRepository departmentRepo, IMapper mapper)
        {
            this.departmentRepo = departmentRepo;
            this.mapper = mapper;
        }


        [HttpGet]
        //[Authorize(Policy = "UserPolicy")]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DepartmentDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Will returned in case the user isn't authenticated at all
        [ProducesResponseType(StatusCodes.Status403Forbidden)] // Will returned in case the the user is authenticated but not have the priviliges to access that resource.
        public IActionResult GetAllDepartments()
        {
            try
            {
                var departments = departmentRepo.GetAllDepartments();
                var departmentsDto = new List<DepartmentDto>();
                foreach (var curr in departments)
                {
                    departmentsDto.Add(mapper.Map<DepartmentDto>(curr));
                }
                return Ok(departmentsDto);
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
        [HttpGet("{departmentId:int}", Name = "GetDepartment")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DepartmentDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetDepartment(int departmentId)
        {
            try
            {
                var department = departmentRepo.GetDepartment(departmentId);
                if (department == null)
                {
                    return NotFound();
                }
                var departmentDto = mapper.Map<DepartmentDto>(department);
                return Ok(departmentDto);
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
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(DepartmentDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public IActionResult CreateDepartment([FromBody] CreateDepartmentDto createDepartmentDto)
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

                if (departmentRepo.IsDepartmentExists(createDepartmentDto.Name))
                {
                    //ModelState.AddModelError("", "This department already exists!");
                    return StatusCode(400, "This department already exists!");
                }

                var department = mapper.Map<Department>(createDepartmentDto);

                if (!departmentRepo.CreateDepartment(department))
                {
                    //ModelState.AddModelError("", $"something went wrong while saving department {department.Name}");
                    return StatusCode(500, $"something went wrong while saving department {department.Name}");
                }

                var departmentDto = mapper.Map<DepartmentDto>(department);

                return CreatedAtRoute("GetDepartment", new { version = HttpContext.GetRequestedApiVersion().ToString(), departmentId = departmentDto.Id }, departmentDto);
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
        [HttpPut("{departmentId:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateDepartment(int departmentId, [FromBody] UpdateDepartmentDto updateDepartmentDto)
        {
            try
            {
                if (updateDepartmentDto == null || departmentId != updateDepartmentDto.Id)
                {
                    return BadRequest();
                }

                if (!departmentRepo.UpdateDepartment(updateDepartmentDto))
                {
                    //ModelState.AddModelError("", $"something went wrong while updating department {updateDepartmentDto.Name}");
                    return StatusCode(500, $"something went wrong while updating department {updateDepartmentDto.Name}");
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
        [HttpDelete("{departmentId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteDepartment(int departmentId)
        {
            try
            {
                if (!departmentRepo.IsDepartmentExists(departmentId))
                {
                    return NotFound();
                }

                var department = departmentRepo.GetDepartment(departmentId);

                if (!departmentRepo.DeleteDepartment(department))
                {
                    //ModelState.AddModelError("", $"something went wrong while deleting department {department.Name}");
                    return StatusCode(500, $"something went wrong while deleting department {department.Name}");
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
    }
}
