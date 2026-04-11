using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizI.Infrastructure.Auth
{
    public class AuthDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty; 
        public string Username { get; set; } = string.Empty;
        public Guid Role { get; set; } 
    }
}
