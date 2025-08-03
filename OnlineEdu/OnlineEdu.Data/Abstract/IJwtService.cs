using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEdu.Data.Abstract
{
    public interface IJwtService
    {
        string GenerateToken(string username, string role);
        bool ValidateToken(string token);
        string GetUsernameFromToken(string token);
        string GetRoleFromToken(string token);
    }
}
