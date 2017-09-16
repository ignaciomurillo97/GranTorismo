using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gran_Torismo_API.Models
{

    public class User
    {
        public decimal IdCard { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
    }
    public class Client
    {
        public User User { get; set; }
        public decimal AccountNumber { get; set; }
    }

    public class Owner
    {
        public User User { get; set; }
    }

    public class Admin
    {
        public User User { get; set; }
    }
}