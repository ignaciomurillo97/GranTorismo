using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gran_Torismo_API.Models
{
    public class LoginRequest
    {
        [Key]
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Msg { get; set; }
        public int IdCard { get; set; }
    }

    public partial class RegisterResponse
    {
        public string Response { get; set; }
    }
}