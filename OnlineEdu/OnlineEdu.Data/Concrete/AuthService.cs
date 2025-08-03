using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OnlineEdu.Data.Abstract;
using OnlineEdu.DTOs.DTOs.AuthDTOs;
using OnlineEdu.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEdu.Data.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtService _jwtService;
        private readonly AdminCredentials _adminCredentials;

        public AuthService(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IJwtService jwtService,
            IOptions<AdminCredentials> adminCredentials)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
            _adminCredentials = adminCredentials.Value;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                if (request.Username == _adminCredentials.Username &&
                    request.Password == _adminCredentials.Password)
                {
                    var adminToken = _jwtService.GenerateToken(request.Username, "SuperAdmin");

                    return new LoginResponse
                    {
                        Success = true,
                        Token = adminToken,
                        Username = request.Username,
                        Role = "SuperAdmin",
                        ExpirationTime = DateTime.UtcNow.AddMinutes(60),
                        Message = "Admin login successfully"
                    };
                }

                var user = await _userManager.FindByEmailAsync(request.Username) ??
                          await _userManager.FindByNameAsync(request.Username);

                if (user == null)
                {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Username or password incorrect"
                    };
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

                if (!result.Succeeded)
                {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Username or password incorrect"
                    };
                }

                var role = await GetUserRoleAsync(user.UserName!);
                var token = _jwtService.GenerateToken(user.UserName!, role);

                return new LoginResponse
                {
                    Success = true,
                    Token = token,
                    Username = user.UserName!,
                    Role = role,
                    ExpirationTime = DateTime.UtcNow.AddMinutes(60),
                    Message = "Login successfully"
                };
            }
            catch (Exception ex)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<LoginResponse> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var existingUser = await _userManager.FindByNameAsync(request.Username);
                if (existingUser != null)
                {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Username already exists"
                    };
                }

                var existingEmail = await _userManager.FindByEmailAsync(request.Email);
                if (existingEmail != null)
                {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Email already exists"
                    };
                }

                var user = new IdentityUser
                {
                    UserName = request.Username,
                    Email = request.Email
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return new LoginResponse
                    {
                        Success = false,
                        Message = $"Registration failed: {errors}"
                    };
                }

                await EnsureRoleExistsAsync("User");
                await _userManager.AddToRoleAsync(user, "User");

                var token = _jwtService.GenerateToken(user.UserName, "User");

                return new LoginResponse
                {
                    Token = token,
                    Username = user.UserName,
                    Role = "User",
                    ExpirationTime = DateTime.UtcNow.AddHours(1),
                    Success = true,
                    Message = "Registration successful"
                };
            }
            catch (Exception ex)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = $"Registration failed: {ex.Message}"
                };
            }
        }

        public async Task<bool> ChangeUserRoleAsync(string username, string role)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user == null) return false;

                var currentRoles = await _userManager.GetRolesAsync(user);

                if (currentRoles.Any())
                {
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                }

                await EnsureRoleExistsAsync(role);

                var result = await _userManager.AddToRoleAsync(user, role);
                return result.Succeeded;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> GetUserRoleAsync(string username)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user == null) return string.Empty;

                var roles = await _userManager.GetRolesAsync(user);
                return roles.FirstOrDefault() ?? "User";
            }
            catch
            {
                return string.Empty;
            }
        }

        public async Task<List<string>> GetAllUsersAsync()
        {
            try
            {
                var users = _userManager.Users.Select(u => u.UserName).ToList();
                return users ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }

        private async Task EnsureRoleExistsAsync(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}