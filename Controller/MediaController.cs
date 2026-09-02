using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [Authorize]
        [HttpGet("title/{name}")]
        public IActionResult GetByName(string name)
        {
            var selectMedia = Context.Medias.Where(x => EF.Functions.Like(x.Name, $"%{name}%")).ToList();
            if (!selectMedia.Any())
                return Problem(
                    detail: "Não há nenhuma obra com esse nome.",
                    statusCode: StatusCodes.Status404NotFound
                );
            return Ok(selectMedia);
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