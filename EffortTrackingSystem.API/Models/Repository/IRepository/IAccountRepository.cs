using EffortTrackingSystem.Models.DtoResponses;
using EffortTrackingSystem.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EffortTrackingSystem.API.Models.Repository.IRepository
{
    public interface IAccountRepository
    {
        Task<GeneralResponse> RegisterUserAsync(RegisterDto registerDto);

        Task<AccountLoginResponse> LoginUserAsync(LoginDto loginDto);

        Task<GeneralResponse> EditProfileAsync(EditProfileDto editProfileDto);

        // --------------------------------------

        #region Add Roles
        IEnumerable<IdentityRole> GetAllRoles();

        IdentityRole GetRole(string id);

        bool IsRoleExists(string id);

        Task<GeneralResponse> CreateRoleAsync(IdentityRole identityRole);

        Task<GeneralResponse> UpdateRoleAsync(IdentityRole identityRole);

        Task<GeneralResponse> DeleteRoleAsync(IdentityRole identityRole);
        #endregion

        // --------------------------------------
    }
}
