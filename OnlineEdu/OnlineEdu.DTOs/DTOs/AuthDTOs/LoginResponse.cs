using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEdu.DTOs.DTOs.AuthDTOs
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime ExpirationTime { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
