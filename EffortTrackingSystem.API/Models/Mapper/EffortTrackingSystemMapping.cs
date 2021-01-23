using AutoMapper;
using EffortTrackingSystem.Models.Dtos;
using EffortTrackingSystem.Models.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EffortTrackingSystem.API.Models.Mapper
{
    public class EffortTrackingSystemMapping : Profile
    {
        public EffortTrackingSystemMapping()
        {
            CreateMap<ApplicationUser, RegisterDto>().ReverseMap();

            CreateMap<ApplicationUser, UserDto>().ReverseMap();

            CreateMap<CreateUserDto, ApplicationUser>().ReverseMap();

            CreateMap<EditProfileDto, ApplicationUser>().ReverseMap();

            // ----------------

            CreateMap<Department, DepartmentDto>().ReverseMap();

            CreateMap<Department, CreateDepartmentDto>().ReverseMap();

            CreateMap<Department, UpdateDepartmentDto>().ReverseMap();

            // ----------------

            CreateMap<Category, CategoryDto>().ReverseMap();

            CreateMap<Category, CreateCategoryDto>().ReverseMap();

            CreateMap<Category, UpdateCategoryDto>().ReverseMap();

            // ----------------

            CreateMap<Mission, MissionDto>().ReverseMap();

            CreateMap<Mission, CreateMissionDto>().ReverseMap();

            // ----------------

            CreateMap<IdentityRole, RoleDto>().ReverseMap();

            CreateMap<IdentityRole, CreateRoleDto>().ReverseMap();

            CreateMap<IdentityRole, UpdateRoleDto>().ReverseMap();

            // -----------------

            CreateMap<Notification, NotificationDto>().ReverseMap();
        }
    }
}
