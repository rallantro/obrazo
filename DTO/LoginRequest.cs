using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace obrazo.DTO
{
    public class LoginRequest
    {
        public LoginRequest() {}
        public string Name {get; set;} = string.Empty;
        public string Password {get; set;} = string.Empty;

        public LoginRequest(string name, string password)
        {
            Name = name;
            Password = password;
        }
    }
}