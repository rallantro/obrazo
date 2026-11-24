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
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var media = Context.Medias.Where(x => x.Id == id).FirstOrDefault();
            if (media == null)
                return Problem(
                    detail: "Nenhuma obra encontrada com os critérios informados.",
                    statusCode: StatusCodes.Status404NotFound
                );
            return Ok(media);

        }

        [Authorize]
        [HttpGet("search")]
        public IActionResult Search([FromQuery] string? name, [FromQuery] int? type)
        {
            var selectMedia = Context.Medias.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                selectMedia = selectMedia.Where(x => EF.Functions.Like(x.Name, $"%{name}%"));
            }
            if (type != null)
            {
                if (!Enum.IsDefined(typeof(MediaType), type.Value))
                {
                    return BadRequest("Tipo de mídia inválido.");
                }
                selectMedia = selectMedia.Where(x => x.Type == (MediaType)type.Value);
            }
            var resultado = selectMedia.ToList();
            if (resultado.Count == 0)
                return Problem(
                    detail: "Nenhuma obra encontrada com os critérios informados.",
                    statusCode: StatusCodes.Status404NotFound
                );
            return Ok(resultado);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody] Media novaMedia)
        {
            if (!ValidateDuplicate(novaMedia, out string errorMessageValid))
            {
                return BadRequest(errorMessageValid);
            }
            Context.Medias.Add(novaMedia);
            Context.SaveChanges();
            return CreatedAtAction(nameof(Get), new { id = novaMedia.Id }, novaMedia);
        }

        private bool ValidateDuplicate(Media media, out string errorMessage)
        {
            if (Context.Medias.Any(x => x.Name == media.Name && x.Type == media.Type))
            {
                errorMessage = "Esta mídia já está cadastrada";
                return false;
            }
            errorMessage = "";
            return true;
        }
    }
}