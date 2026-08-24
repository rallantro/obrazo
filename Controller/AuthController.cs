using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using obrazo.Data;
using obrazo.DTO;
using obrazo.Services;

namespace obrazo.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext Context;

        public AuthController(AppDbContext context)
        {
            Context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = Context.Users.FirstOrDefault(x => x.Name == request.Name);
            if (user == null)
            {
                return Unauthorized("Usuário ou senha inválidos.");
            }
            bool valid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!valid)
            {
                return Unauthorized("Deu senha invália." + "O que chegou foi:" + user.PasswordHash);
            }
            
            return Ok( new { userId = user.Id, message = "Login bem-sucedido!", token = TokenService.GerarToken(user)});
        }
    }
}