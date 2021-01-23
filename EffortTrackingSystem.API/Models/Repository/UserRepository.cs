using EffortTrackingSystem.API.Models.Repository.IRepository;
using EffortTrackingSystem.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EffortTrackingSystem.API.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Routing;
using EffortTrackingSystem.Models.DtoResponses;
using Microsoft.EntityFrameworkCore;

namespace EffortTrackingSystem.API.Models.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EmailHelper _emailHelper;

        public UserRepository(UserManager<ApplicationUser> userManager, EmailHelper emailHelper)
        {
            _userManager = userManager;
            _emailHelper = emailHelper;
        }

        public async Task<ApplicationUser> GetUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user != null && !user.IsDeleted ? user : null;
        }

        public List<ApplicationUser> GetAllUsers(int? departmentId)
        {
            var usersIQuerable = _userManager.Users.Where(x => x.IsDeleted == false);
            usersIQuerable = departmentId != null ? usersIQuerable.Where(x => x.DepartmentId == departmentId) : usersIQuerable;
            return usersIQuerable.Include(x => x.Department).ToList();
        }

        public List<ApplicationUser> GetAllNonAdminUsers(int? departmentId)
        {
            var adminUsers = _userManager.GetUsersInRoleAsync("Admin").Result;
            var superAdminUsers = _userManager.GetUsersInRoleAsync("SuperAdmin").Result;

            var adminsId = adminUsers.Select(x => x.Id); // Get Id of all admins
            foreach (var item in superAdminUsers)
            {
                adminsId.Append(item.Id);
            }

            var usersIQuerable = _userManager.Users.Where(x => x.IsDeleted == false && !adminsId.Contains(x.Id));
            usersIQuerable = departmentId != null ? usersIQuerable.Where(x => x.DepartmentId == departmentId) : usersIQuerable;
            return usersIQuerable.Include(x => x.Department).ToList();
        }

        public async Task<GeneralResponse> CreateUserAsync(ApplicationUser user, string passwordResetLink)
        {
            var result = await _userManager.CreateAsync(user);

            if(result.Succeeded)
            {
                #region Add Role
                var resultRole = await _userManager.AddToRoleAsync(user, "User");
                if (resultRole.Succeeded)
                {
                    // Send mail to the user, to set it's password.

                    var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    passwordResetLink += $"?email={user.Email}&token={resetToken}";

                    var body = $"<p>Click on the that link to set your password <a href='{passwordResetLink}'>Click Here</a></p>";
                    EmailModel emailModel = new EmailModel(user.Email, "Set Password", body, true);
                    _emailHelper.SendEmail(emailModel);

                    return new GeneralResponse
                    {
                        IsSuccess = true,
                        Message = "Account is Created Successfully."
                    };
                }
                return new GeneralResponse
                {
                    IsSuccess = false,
                    Message = "Account Creation Failed in adding him to Role.",
                    Errors = resultRole.Errors.Select(x => x.Description)
                };
                #endregion
            }

            return new GeneralResponse
            {
                IsSuccess = false,
                Message = "Account Creation Failed.",
                Errors = result.Errors.Select(x => x.Description)
            };
        }

        public async Task<bool> UserExistsByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null && !user.IsDeleted ? true : false;
        }

        public async Task<bool> UserExistsById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user != null && !user.IsDeleted ? true : false;
        }

        public async Task<GeneralResponse> DeleteUserAsync(ApplicationUser user)
        {
            user.IsDeleted = true;
            var result = await _userManager.UpdateAsync(user);

            if(result.Succeeded)
            {
                return new GeneralResponse
                {
                    IsSuccess = true,
                    Message = "User deleted successfully."
                };
            }

            return new GeneralResponse
            {
                IsSuccess = false,
                Message = "Invalid Operation.",
                Errors = result.Errors.Select(x => x.Description)
            };
        }

        public async Task<GeneralResponse> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            resetPasswordDto.Token = resetPasswordDto.Token.Replace(" ", "+"); // https://stackoverflow.com/questions/27241658/token-invalid-on-reset-password-with-asp-net-identity
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);

            if (result.Succeeded)
            {
                return new GeneralResponse
                {
                    IsSuccess = true,
                    Message = "Reset Password Confirmed"
                };
            }

            // Display validation errors. For example, password reset token already
            // used to change the password or password complexity rules not met
            return new GeneralResponse
            {
                IsSuccess = false,
                Message = "Invalid",
                Errors = result.Errors.Select(x => x.Description)
            };
        }
    }
}
