using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace obrazo.DTO
{
    public class LoginRequest
    {
        public LoginRequest() {}
        public string Name {get; set;} = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string Password {get; set;} = string.Empty;

        public LoginRequest(string name, string password)
        {
            Name = name;
            Password = password;
        }
    }
}