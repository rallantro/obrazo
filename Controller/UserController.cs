using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using obrazo.Data;
using obrazo.Models.Entities;

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
        public IActionResult Post([FromBody] User novoUser)
        {
            if (string.IsNullOrWhiteSpace(novoUser.PasswordHash))
                return BadRequest("A senha é obrigatória.");

            string HashedPassword = BCrypt.Net.BCrypt.HashPassword(novoUser.PasswordHash, workFactor: 12);
            novoUser.PasswordHash = HashedPassword;
            Context.Users.Add(novoUser);
            Context.SaveChanges();
            return CreatedAtAction(nameof(Get), new { id = novoUser.Id }, novoUser);
        }
    }
}