using EffortTrackingSystem.Models.DtoResponses;
using EffortTrackingSystem.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EffortTrackingSystem.API.Models.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetUserAsync(string userId);

        List<ApplicationUser> GetAllUsers(int? departmentId);

        List<ApplicationUser> GetAllNonAdminUsers(int? departmentId);

        Task<bool> UserExistsByEmail(string email);

        Task<bool> UserExistsById(string id);

        Task<GeneralResponse> CreateUserAsync(ApplicationUser user, string passwordResetLink);

        Task<GeneralResponse> DeleteUserAsync(ApplicationUser user);

        Task<GeneralResponse> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    }
}
