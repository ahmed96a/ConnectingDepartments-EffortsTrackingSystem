using EffortTrackingSystem.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EffortTrackingSystem.Web.Models.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<object> GetAllUsersAsync(string Url, int? departmentId, string token);

        Task<object> GetAllNonAdminUsersAsync(string Url, int? departmentId, string token);

        Task<object> GetUserAsync(string Url, string userId, string token);

        Task<object> CreateUserAsync(string Url, CreateUserDto createUserDto, string passwordResetLink, string token);

        Task<object> ResetPasswordAsync(string Url, ResetPasswordDto resetPasswordDto, string token);

        Task<object> DeleteUserAsync(string Url, string userId, string token);
    }
}
