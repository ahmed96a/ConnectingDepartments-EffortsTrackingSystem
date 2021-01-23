using AutoMapper;
using EffortTrackingSystem.Models.Dtos;
using EffortTrackingSystem.Models.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EffortTrackingSystem.Web.Models.Mapper
{
    public class EffortTrackingSystemMapping : Profile
    {
        public EffortTrackingSystemMapping()
        {

            // ----------------

            CreateMap<Department, DepartmentDto>().ReverseMap();

            CreateMap<Department, CreateDepartmentDto>().ReverseMap();

            CreateMap<Department, UpdateDepartmentDto>().ReverseMap();

            CreateMap<DepartmentDto, UpdateDepartmentDto>().ReverseMap();

            // ----------------

            CreateMap<Category, CategoryDto>().ReverseMap();

            CreateMap<Category, CreateCategoryDto>().ReverseMap();

            CreateMap<Category, UpdateCategoryDto>().ReverseMap();

            CreateMap<CategoryDto, UpdateCategoryDto>().ReverseMap();

            // ----------------

            CreateMap<Mission, MissionDto>().ReverseMap();

            CreateMap<Mission, CreateMissionDto>().ReverseMap();

            // ----------------

            CreateMap<IdentityRole, RoleDto>().ReverseMap();

            CreateMap<IdentityRole, CreateRoleDto>().ReverseMap();

            CreateMap<IdentityRole, UpdateRoleDto>().ReverseMap();

            // ----------------

            CreateMap<UserDto, EditProfileDto>().ReverseMap();
        }
    }
}
