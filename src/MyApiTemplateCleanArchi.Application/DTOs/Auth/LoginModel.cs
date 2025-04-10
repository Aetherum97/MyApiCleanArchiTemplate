using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApiTemplateCleanArchi.Application.DTOs.Auth
{
    public class LoginModel
    {
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
