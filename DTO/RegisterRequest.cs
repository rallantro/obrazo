using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace obrazo.DTO
{
    public class RegisterRequest
    {
        public RegisterRequest() { }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha precisa ter no mínimo 6 caracteres.")]
        [RegularExpression(@".*\d.*", ErrorMessage = "A senha precisa ter no mínimo 1 número.")]
        public string Password { get; set; } = string.Empty;

        public RegisterRequest(string name, string password)
        {
            Name = name;
            Password = password;
        }
    }
}