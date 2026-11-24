using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using obrazo.Data;
using obrazo.DTO;
using obrazo.Models.Entities;
using obrazo.Services;

namespace obrazo.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext Context;

        public UserController(AppDbContext context)
        {
            Context = context;
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Get()
        {
            var UserIdClaim = User.FindFirst("userID")?.Value;
            if (!int.TryParse(UserIdClaim, out int intID))
                return BadRequest("O ID deve ser um número inteiro.");

            var user = Context.Users.Find(intID);
            if (user == null)
                return NotFound();
            return Ok(user.Name);
        }

        [HttpPost]
        public IActionResult Post([FromBody] RegisterRequest registerRequest)
        {
            if (Context.Users.Any(x => x.Name == registerRequest.Name))
                return BadRequest("Nome de Usuário em uso.");
            string hashedPassword = PasswordService.HashPassword(registerRequest.Password);
            var user = new User
            {
                Name = registerRequest.Name,
                PasswordHash = hashedPassword
            };
            Context.Users.Add(user);
            Context.SaveChanges();
            return StatusCode(201, new { id = user.Id, name = user.Name });
        }

        [Authorize]
        [HttpPut]
        public IActionResult Put([FromBody] ChangePasswordRequest changePasswordRequest)
        {
            var UserIdClaim = User.FindFirst("userID")?.Value;
            if (!int.TryParse(UserIdClaim, out int intID))
                return BadRequest();
            var user = Context.Users.Find(intID);
            if (user == null)
                return NotFound();

            bool valid = PasswordService.VerifyPassword(changePasswordRequest.OldPassword, user.PasswordHash);
            if (!valid)
                return BadRequest(new { message = "Senha incorreta." });
            if (changePasswordRequest.NewPassword == changePasswordRequest.OldPassword)
                return BadRequest(new { message = "A nova senha precisa ser diferente da original!"});
            string newHashed = PasswordService.HashPassword(changePasswordRequest.NewPassword);
            user.PasswordHash = newHashed;
            Context.SaveChanges();
            return StatusCode(200, new { message = "Senha alterada com sucesso!" });
        }
    }
}