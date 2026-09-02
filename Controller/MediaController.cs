using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using obrazo.Data;
using obrazo.Models.Entities;
using obrazo.Models.Enum;

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
        [HttpGet("search")]
        public IActionResult Search([FromQuery] string? name, [FromQuery] int? type)
        {
            var selectMedia = Context.Medias.AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                selectMedia = selectMedia.Where(x => EF.Functions.Like(x.Name, $"%{name}%"));
            }
            if (type != null)
            {
                selectMedia = selectMedia.Where(x => x.Type == (MediaType)type);
            }
            var resultado = selectMedia.ToList();
            if (!selectMedia.Any())
                return Problem(
                    detail: "Nenhuma obra encontrada com os critérios informados.",
                    statusCode: StatusCodes.Status404NotFound
                );
            return Ok(resultado);
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