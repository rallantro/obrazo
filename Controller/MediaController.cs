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
    public class MediaController : ControllerBase
    {
        private readonly AppDbContext Context;

        public MediaController(AppDbContext context)
        {
            Context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var medias = Context.Medias.ToList();

            return Ok(medias);

        }

        [HttpPost]
        public IActionResult Post([FromBody] Media novaMedia)
        {
            Context.Medias.Add(novaMedia);
            Context.SaveChanges();
            return CreatedAtAction(nameof(Get), new { id = novaMedia.Id }, novaMedia);
        }
    }
}