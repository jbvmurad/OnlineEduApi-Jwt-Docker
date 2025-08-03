using OnlineEdu.DTOs.DTOs.AuthDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEdu.Data.Abstract
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<LoginResponse> RegisterAsync(RegisterRequest request);
        Task<bool> ChangeUserRoleAsync(string username, string role); 
        Task<string> GetUserRoleAsync(string username);
        Task<List<string>> GetAllUsersAsync(); 
    }

}
