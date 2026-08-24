using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = Context.Users.Find(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Post([FromBody] User novoUser)
        {
            Context.Users.Add(novoUser);
            Context.SaveChanges();
            return CreatedAtAction(nameof(Get), new { id = novoUser.Id }, novoUser);
        }
    }
}