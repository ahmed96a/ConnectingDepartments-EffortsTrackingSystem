using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using EffortTrackingSystem.Models.DtoResponses;
using EffortTrackingSystem.Models.Dtos;
using EffortTrackingSystem.Web.Models.Repository.IRepository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace EffortTrackingSystem.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AccountController(IDepartmentRepository departmentRepository, IUserRepository userRepository, IAccountRepository accountRepository, IConfiguration configuration, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _userRepository = userRepository;
            _departmentRepository = departmentRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult Index(LoginAndRegisterDto loginAndRegisterDto)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            var result = _departmentRepository.GetAllAsync(_configuration["APIConstants:DepartmentAPIPath"]).Result;

            if (result is List<DepartmentDto>)
            {
                var departmentDtos = (List<DepartmentDto>)result;
                ViewBag.DepartmentsSelectList = new SelectList(departmentDtos, "Id", "Name");
            }
            if (loginAndRegisterDto.loginDto != null || loginAndRegisterDto.registerDto != null)
                return View(loginAndRegisterDto);
            return View();
        }

        //[HttpGet]
        //public IActionResult Register()
        //{
        //    var result = _departmentRepository.GetAllAsync(_configuration["APIConstants:DepartmentAPIPath"]).Result;

        //    if (result is List<DepartmentDto>)
        //    {
        //        var departmentDtos = (List<DepartmentDto>)result;
        //        ViewBag.DepartmentsSelectList = new SelectList(departmentDtos, "Id", "Name");
        //    }

        //    return View();
        //}

        //[HttpPost]
        //public IActionResult Register(RegisterDto model)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var result = _accountRepository.RegisterAsync(_configuration["APIConstants:AccountAPIPath"], model).Result;

        //            if (result is null)
        //            {
        //                return View("RegisterSuccess");
        //            }
        //            else if (result is GeneralResponse)
        //            {
        //                var generalResponse = (GeneralResponse)result;
        //                foreach (var error in generalResponse.Errors)
        //                {
        //                    ModelState.AddModelError("", error);
        //                }
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("", result.ToString());
        //            }
        //        }

        //        var resultDepart = _departmentRepository.GetAllAsync(_configuration["APIConstants:DepartmentAPIPath"]).Result;
        //        if (resultDepart is List<DepartmentDto>)
        //        {
        //            var departmentDtos = (List<DepartmentDto>)resultDepart;
        //            ViewBag.DepartmentsSelectList = new SelectList(departmentDtos, "Id", "Name");
        //        }
        //        return View(model);
        //    }
        //    catch(Exception ex)
        //    {
        //        #region Production Environment
        //        // In Production Environment : -
        //        // Log in exception.
        //        //return StatusCode(StatusCodes.Status500InternalServerError, "Error In Performing operation");
        //        #endregion

        //        // In Development Environment : -
        //        var exception = ex.InnerException != null ? ex.Message + "\r\n\r\n" + ex.InnerException.Message : ex.Message;
        //        ModelState.AddModelError("", exception);

        //        var result = _departmentRepository.GetAllAsync(_configuration["APIConstants:DepartmentAPIPath"]).Result;
        //        if (result is List<DepartmentDto>)
        //        {
        //            var departmentDtos = (List<DepartmentDto>)result;
        //            ViewBag.DepartmentsSelectList = new SelectList(departmentDtos, "Id", "Name");
        //        }
        //        return View(model);
        //    }
        //}

        [HttpPost]
        public IActionResult Register(LoginAndRegisterDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _accountRepository.RegisterAsync(_configuration["APIConstants:AccountAPIPath"], model.registerDto).Result;

                    if (result is null)
                    {
                        return View("RegisterSuccess");
                    }
                    else if (result is GeneralResponse)
                    {
                        var generalResponse = (GeneralResponse)result;
                        foreach (var error in generalResponse.Errors)
                        {
                            ModelState.AddModelError("", error);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", result.ToString());
                    }
                }

                var resultDepart = _departmentRepository.GetAllAsync(_configuration["APIConstants:DepartmentAPIPath"]).Result;
                if (resultDepart is List<DepartmentDto>)
                {
                    var departmentDtos = (List<DepartmentDto>)resultDepart;
                    ViewBag.DepartmentsSelectList = new SelectList(departmentDtos, "Id", "Name");
                }
                return View("Index", model);
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
                ModelState.AddModelError("", exception);

                var result = _departmentRepository.GetAllAsync(_configuration["APIConstants:DepartmentAPIPath"]).Result;
                if (result is List<DepartmentDto>)
                {
                    var departmentDtos = (List<DepartmentDto>)result;
                    ViewBag.DepartmentsSelectList = new SelectList(departmentDtos, "Id", "Name");
                }
                return View("Index",model);
            }
        }


        //[HttpGet]
        //public IActionResult Login()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Login(LoginDto model)
        //{
        //    try
        //    {
        //        var result = _accountRepository.LoginAsync(_configuration["APIConstants:AccountAPIPath"], model).Result;

        //        if (result is AccountLoginResponse && ((AccountLoginResponse)result).IsSuccess == true)
        //        {
        //            var accountLoginResponse = (AccountLoginResponse)result;

        //            #region Set the token in the session, For Backend Authentication
        //            // ---------------------------------

        //            #region Cookie For Token
        //            //CookieOptions cookieOptions = new CookieOptions
        //            //{
        //            //    //Secure = true,
        //            //    HttpOnly = true
        //            //};
        //            // Response.Cookies.Append("JWToken", accountLoginResponse.Token, cookieOptions); // Set the token in the cookie
        //            #endregion

        //            HttpContext.Session.SetString("JWToken", accountLoginResponse.Token);

        //            // ---------------------------------
        //            #endregion

        //            var userDto = accountLoginResponse.User;

        //            #region To be able to use the Cookie Authentication in the front-end for authentication we have to add a PrincipleClaim and and pass ClaimsIdentity to it.
        //            // --------------------------------------------

        //            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

        //            // Here we add Claims to that ClaimsIdentity variable, so we can use it throughout our application. we will add Username and role to our ClaimIdentity
        //            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userDto.Id));
        //            identity.AddClaim(new Claim(ClaimTypes.Email, userDto.Email));
        //            identity.AddClaim(new Claim(ClaimTypes.Name, userDto.FullName));
        //            identity.AddClaim(new Claim("DepartmentId", userDto.Department.Id.ToString()));
        //            foreach (var role in userDto.Roles)
        //            {
        //                identity.AddClaim(new Claim(ClaimTypes.Role, role));
        //            }

        //            // create ClaimsPrincipal
        //            var principal = new ClaimsPrincipal(identity); // User property, represents that ClaimsPrincipal variable.

        //            // the last step is to sign the user in
        //            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        //            // --------------------------------------------
        //            #endregion


        //            return RedirectToAction("Index", "Home");
        //        }
        //        else if (result is AccountLoginResponse && ((AccountLoginResponse)result).IsSuccess == false)
        //        {
        //            var accountLoginResponse = (AccountLoginResponse)result;
        //            ModelState.AddModelError("", accountLoginResponse.Message);
        //            return View(model);
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", result.ToString());
        //            return View(model);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        #region Production Environment
        //        // In Production Environment : -
        //        // Log in exception.
        //        //return StatusCode(StatusCodes.Status500InternalServerError, "Error In Performing operation");
        //        #endregion

        //        // In Development Environment : -
        //        var exception = ex.InnerException != null ? ex.Message + "\r\n\r\n" + ex.InnerException.Message : ex.Message;
        //        ModelState.AddModelError("", exception);
        //        return View(model);
        //    }
        //}


        [HttpPost]
        public async Task<IActionResult> Login(LoginAndRegisterDto model)
        {
            try
            {
                var result = _accountRepository.LoginAsync(_configuration["APIConstants:AccountAPIPath"], model.loginDto).Result;

                if (result is AccountLoginResponse && ((AccountLoginResponse)result).IsSuccess == true)
                {
                    var accountLoginResponse = (AccountLoginResponse)result;

                    #region Set the token in the session, For Backend Authentication
                    // ---------------------------------

                    #region Cookie To Store Token
                    CookieOptions cookieOptions = new CookieOptions
                    {
                        //Secure = true,
                        HttpOnly = true
                    };
                    Response.Cookies.Append("JWToken", accountLoginResponse.Token, cookieOptions); // Set the token in the cookie
                    #endregion

                    #region Session To Store Token
                    //HttpContext.Session.SetString("JWToken", accountLoginResponse.Token);
                    #endregion

                    TempData["JWToken"] = accountLoginResponse.Token; // to use it by javascript to save the token in the browser so we can use it in signalr.

                    // ---------------------------------
                    #endregion

                    var userDto = accountLoginResponse.User;

                    #region To be able to use the Cookie Authentication in the front-end for authentication we have to add a PrincipleClaim and and pass ClaimsIdentity to it.
                    // --------------------------------------------

                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

                    // Here we add Claims to that ClaimsIdentity variable, so we can use it throughout our application. we will add Username and role to our ClaimIdentity
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userDto.Id));
                    identity.AddClaim(new Claim(ClaimTypes.Email, userDto.Email));
                    identity.AddClaim(new Claim(ClaimTypes.Name, userDto.FullName));
                    identity.AddClaim(new Claim("DepartmentId", userDto.Department.Id.ToString()));
                    identity.AddClaim(new Claim("DepartmentName", userDto.Department.Name));
                    foreach (var role in userDto.Roles)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, role));
                    }

                    // create ClaimsPrincipal
                    var principal = new ClaimsPrincipal(identity); // User property, represents that ClaimsPrincipal variable.

                    // the last step is to sign the user in
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    // --------------------------------------------
                    #endregion


                    return RedirectToAction("Index", "Home");
                }
                else if (result is AccountLoginResponse && ((AccountLoginResponse)result).IsSuccess == false)
                {
                    var accountLoginResponse = (AccountLoginResponse)result;
                    ModelState.AddModelError("", accountLoginResponse.Message);
                }
                else
                {
                    ModelState.AddModelError("", result.ToString());
                }

                var resultDepart = _departmentRepository.GetAllAsync(_configuration["APIConstants:DepartmentAPIPath"]).Result;
                if (resultDepart is List<DepartmentDto>)
                {
                    var departmentDtos = (List<DepartmentDto>)resultDepart;
                    ViewBag.DepartmentsSelectList = new SelectList(departmentDtos, "Id", "Name");
                }
                return View("Index", model);
            }
            catch (Exception ex)
            {
                #region Production Environment
                // In Production Environment : -
                // Log in exception.
                //return StatusCode(StatusCodes.Status500InternalServerError, "Error In Performing operation");
                #endregion

                // In Development Environment : -
                var resultDepart = _departmentRepository.GetAllAsync(_configuration["APIConstants:DepartmentAPIPath"]).Result;
                if (resultDepart is List<DepartmentDto>)
                {
                    var departmentDtos = (List<DepartmentDto>)resultDepart;
                    ViewBag.DepartmentsSelectList = new SelectList(departmentDtos, "Id", "Name");
                }

                var exception = ex.InnerException != null ? ex.Message + "\r\n\r\n" + ex.InnerException.Message : ex.Message;
                ModelState.AddModelError("", exception);
                return View("Index", model);
            }
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            try
            {
                var userId = User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;

                var result = await _userRepository.GetUserAsync(_configuration["APIConstants:UserAPIPath"], userId, HttpContext.Session.GetString("JWToken"));

                if (result is null)
                    return NotFound();

                if (result is UserDto)
                    return View(_mapper.Map<EditProfileDto>((UserDto)result));

                return View(new EditProfileDto());
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
                //ModelState.AddModelError("", exception);
                ViewBag.WebException = exception;
                return View(new EditProfileDto());
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditProfileDto editProfileDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _accountRepository.EditProfileAsync(_configuration["APIConstants:AccountAPIPath"], editProfileDto, HttpContext.Session.GetString("JWToken"));

                    if (result == null)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else if(result is GeneralResponse)
                    {
                        var generalResponse = (GeneralResponse)result;
                        foreach (var error in generalResponse.Errors)
                        {
                            ModelState.AddModelError("", error);
                        }
                    }
                    else
                    {
                        ViewBag.error = (string)result;
                        //ModelState.AddModelError("", (string)result);
                    }
                }
                return View(editProfileDto);
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
                ModelState.AddModelError("", exception);
                return View(editProfileDto);
            }
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();

            Response.Cookies.Delete("JWToken");

            //HttpContext.Session.Remove("JWToken");

            return RedirectToAction("Index", "Home");
        }

        // --------------------------

        [Authorize(Roles = "SuperAdmin")]
        public IActionResult GetAllRoles()
        {
            try
            {
                var result = _accountRepository.GetAllRolesAsync(_configuration["APIConstants:AccountAPIPath"], HttpContext.Session.GetString("JWToken")).Result;
                if (result is List<RoleDto>)
                {
                    var roleDtos = (List<RoleDto>)result;
                    return View(roleDtos);
                }

                ViewBag.ApiException = result.ToString();
                return View(new List<RoleDto>());
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
                //ModelState.AddModelError("", exception);
                ViewBag.WebException = exception;
                return View(new List<RoleDto>());
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateRole(CreateRoleDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _accountRepository.CreateRoleAsync(_configuration["APIConstants:AccountAPIPath"], model, "asd").Result;

                    if (result == null)
                    {
                        return RedirectToAction("GetAllRoles");
                    }
                    else if (result is GeneralResponse)
                    {
                        var generalResponse = (GeneralResponse)result;
                        foreach (var error in generalResponse.Errors)
                        {
                            ModelState.AddModelError("", error);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", result.ToString());
                    }
                }
                return View(model);
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
                ModelState.AddModelError("", exception);
                return View(model);
            }
        }


        // --------------------------
    }
}
