using EffortTrackingSystem.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EffortTrackingSystem.Web.Models.Repository.IRepository
{
    public interface IAccountRepository
    {
        Task<object> RegisterAsync(string Url, RegisterDto registerDto);

        Task<object> LoginAsync(string Url, LoginDto loginDto);

        Task<object> EditProfileAsync(string Url, EditProfileDto editProfileDto, string token);

        Task<object> GetAllRolesAsync(string Url, string token);

        Task<object> GetRoleAsync(string Url, string roleId, string token);

        Task<object> CreateRoleAsync(string Url, CreateRoleDto createRoleDto, string token);

        Task<object> UpdateRoleAsync(string Url, string roleId, UpdateRoleDto createRoleDto, string token);

        Task<object> DeleteRoleAsync(string Url, string roleId, string token);
    }
}
