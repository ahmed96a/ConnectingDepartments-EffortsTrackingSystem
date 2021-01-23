using AutoMapper;
using EffortTrackingSystem.API.Models.Repository.IRepository;
using EffortTrackingSystem.Models.DtoResponses;
using EffortTrackingSystem.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EffortTrackingSystem.API.Models.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        #region Comments
        // There is no need to check if model equal null, because by default in .Net Core WebAPI,
        // if there is a model validation error, a badrequest contains the model validations messages will be returned automatically, without even hitting the action method.
        #endregion

        public AccountRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IMapper mapper, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _appDbContext = appDbContext;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<AccountLoginResponse> LoginUserAsync(LoginDto loginDto)
        {
            //var user = await _userManager.FindByEmailAsync(loginDto.Email); // that will not load Department navigation property.
            var user = _appDbContext.Users.Include(x => x.Department).SingleOrDefault(x => x.Email == loginDto.Email);

            if (user != null && !user.IsDeleted)
            {
                var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

                if (result)
                {
                    var depart = _appDbContext.Departments.FirstOrDefault(x => x.Id == user.DepartmentId);
                    // Create JWToken
                    // ---------------------------

                    var tokenHanlder = new JwtSecurityTokenHandler();
                    var tokenSecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Name, user.FullName),
                        new Claim("DepartmentId", user.DepartmentId.ToString()),
                        new Claim("DepartmentName", depart.Name)
                    };

                    // Add roles to claims, we use foreach, in case if the user is in multiple roles.
                    foreach (var role in await _userManager.GetRolesAsync(user))
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var tokenDescriptor = new SecurityTokenDescriptor() // tokenDescriptor will have all the details regarding to tokens.
                    {
                        Issuer = _configuration["JWT:ValidIssuer"],
                        Audience = _configuration["JWT:ValidAudience"],
                        Subject = new ClaimsIdentity(claims),
                        Expires = DateTime.Now.AddDays(2),
                        SigningCredentials = new SigningCredentials(tokenSecretKey, SecurityAlgorithms.HmacSha256)
                    };

                    var token = tokenHanlder.CreateToken(tokenDescriptor);
                    var tokenAsString = tokenHanlder.WriteToken(token);
                    // ---------------------------

                    var userDto = _mapper.Map<UserDto>(user);
                    var userRoleDtos = await _userManager.GetRolesAsync(user);
                    userDto.Roles = userRoleDtos;

                    return new AccountLoginResponse
                    {
                        IsSuccess = true,
                        Message = "User Logined Successfully",
                        Token = tokenAsString,
                        User = userDto
                    };

                }
            }

            return new AccountLoginResponse
            {
                IsSuccess = false,
                Message = "Invalid Login attempt."
            };
        }

        public async Task<GeneralResponse> RegisterUserAsync(RegisterDto registerDto)
        {
            var user = _mapper.Map<ApplicationUser>(registerDto);
            user.UserName = user.Email;
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                #region Add Role
                var resultRole = await _userManager.AddToRoleAsync(user, "User");
                if (resultRole.Succeeded)
                {
                    return new GeneralResponse
                    {
                        IsSuccess = true,
                        Message = "User registered Successfully."
                    };
                }
                return new GeneralResponse
                {
                    IsSuccess = false,
                    Message = "User registeration failed in adding him to Role.",
                    Errors = resultRole.Errors.Select(x => x.Description)
                };
                #endregion

                //return new GeneralResponse
                //{
                //    IsSuccess = true,
                //    Message = "User registered Successfully."
                //};
            }
            return new GeneralResponse
            {
                IsSuccess = false,
                Message = "User registeration failed.",
                Errors = result.Errors.Select(x => x.Description)
            };
        }

        public async Task<GeneralResponse> EditProfileAsync(EditProfileDto editProfileDto)
        {
            var user = await _userManager.FindByIdAsync(editProfileDto.Id);

            if(user == null)
            {
                return new GeneralResponse
                {
                    IsSuccess = false,
                    Message = "User Profile Update has failed."
                };
            }

            if(await _userManager.CheckPasswordAsync(user, editProfileDto.CurrentPassword))
            {
                user.FullName = editProfileDto.FullName;
                user.PhoneNumber = editProfileDto.PhoneNumber;
                user.HireDate = editProfileDto.HireDate;
                user.JobTitle = editProfileDto.JobTitle;
                user.Phone2 = editProfileDto.Phone2;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    if (editProfileDto.NewPassword == null)
                    {
                        return new GeneralResponse
                        {
                            IsSuccess = true,
                            Message = "User Profile Updated Successfully."
                        };
                    }
                    else
                    {
                        var resultPass = await _userManager.ChangePasswordAsync(user, editProfileDto.CurrentPassword, editProfileDto.NewPassword);
                        if(resultPass.Succeeded)
                        {
                            return new GeneralResponse
                            {
                                IsSuccess = true,
                                Message = "User Profile Updated Successfully."
                            };
                        }
                        else
                        {
                            return new GeneralResponse
                            {
                                IsSuccess = false,
                                Message = "User Profile Update has failed.",
                                Errors = resultPass.Errors.Select(x => x.Description)
                            };
                        }
                    }
                }
                else
                {
                    return new GeneralResponse
                    {
                        IsSuccess = false,
                        Message = "User Profile Update has failed.",
                        Errors = result.Errors.Select(x => x.Description)
                    };
                }
            }
            return new GeneralResponse
            {
                IsSuccess = false,
                Message = "User Profile Update has failed, Wrong Passwords.",
            };
        }

        // --------------------------

        #region Add Roles

        public IEnumerable<IdentityRole> GetAllRoles()
        {
            return _roleManager.Roles.ToList();
        }

        public IdentityRole GetRole(string id)
        {
            return _roleManager.Roles.SingleOrDefault(x => x.Id == id);
        }

        public async Task<GeneralResponse> CreateRoleAsync(IdentityRole identityRole)
        {
            var result = await _roleManager.CreateAsync(identityRole);
            if (result.Succeeded)
            {
                return new GeneralResponse
                {
                    IsSuccess = true,
                    Message = "Role Created Successfully."
                };
            }

            return new GeneralResponse
            {
                IsSuccess = false,
                Message = "Role Creation failed.",
                Errors = result.Errors.Select(x => x.Description)
            };
        }

        public async Task<GeneralResponse> UpdateRoleAsync(IdentityRole identityRole)
        {
            var result = await _roleManager.UpdateAsync(identityRole);
            if (result.Succeeded)
            {
                return new GeneralResponse
                {
                    IsSuccess = true,
                    Message = "Role Updated Successfully."
                };
            }

            return new GeneralResponse
            {
                IsSuccess = false,
                Message = "Role Update failed.",
                Errors = result.Errors.Select(x => x.Description)
            };
        }

        public async Task<GeneralResponse> DeleteRoleAsync(IdentityRole identityRole)
        {
            var result = await _roleManager.DeleteAsync(identityRole);
            if (result.Succeeded)
            {
                return new GeneralResponse
                {
                    IsSuccess = true,
                    Message = "Role Deleted Successfully."
                };
            }

            return new GeneralResponse
            {
                IsSuccess = false,
                Message = "Role Delete failed.",
                Errors = result.Errors.Select(x => x.Description)
            };
        }

        public bool IsRoleExists(string id)
        {
            return _roleManager.Roles.Any(x => x.Id == id);
        }

        #endregion

        // --------------------------
    }
}