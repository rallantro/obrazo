using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using obrazo.Data;
using obrazo.Models.Entities;

namespace obrazo.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class MediaUserController : ControllerBase
    {
        private readonly AppDbContext Context;

        public MediaUserController(AppDbContext context)
        {
            Context = context;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var userMedia = Context.UserMedias.Where(x => x.Id == id).Include(um => um.Media).FirstOrDefault();
            if (userMedia == null)
                return NotFound();
            return Ok(userMedia);
        }

        [HttpGet("user/{UserId}")]
        public IActionResult GetByUser(int UserId)
        {
            var userMedia = Context.UserMedias.Where(x => x.UserId == UserId).Include(um => um.Media).ToList();
            if (!userMedia.Any())
                return NotFound();
            return Ok(userMedia);
        }

        [HttpPost]
        public IActionResult Post([FromBody] UserMedia novoUserMedia)
        {
            if (!ValidateExistence(novoUserMedia, out string errorMessageExistence))
            {
                return BadRequest(errorMessageExistence);
            }
            if (!ValidateDuplicate(novoUserMedia, out string errorMessage))
            {
                return BadRequest(errorMessage);
            }
            if (!ValidateUserMedia(novoUserMedia, out string errorMessageValid))
            {
                return BadRequest(errorMessageValid);
            }
            Context.UserMedias.Add(novoUserMedia);
            Context.SaveChanges();
            return CreatedAtAction(nameof(Get), new { id = novoUserMedia.Id }, novoUserMedia);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UserMedia updated)
        {
            if (!ValidateUserMedia(updated, out string errorMessageValid))
            {
                return BadRequest(errorMessageValid);
            }
            var existing = Context.UserMedias.Find(id);
            if (existing == null) return NotFound();

            existing.Nota = updated.Nota;
            existing.Opiniao = updated.Opiniao;
            existing.Status = updated.Status;
            existing.DataAssistiu = updated.DataAssistiu;

            Context.SaveChanges();
            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var userMedia = Context.UserMedias.Find(id);
            if (userMedia == null) return NotFound();

            Context.UserMedias.Remove(userMedia);
            Context.SaveChanges();
            return NoContent();
        }

        private bool ValidateExistence(UserMedia newUserMedia, out string errorMessage)
        {
            if (!Context.Users.Any(u => u.Id == newUserMedia.UserId))
            {
                errorMessage = "Usuário não encontrado.";
                return false;
            }
            if (!Context.Medias.Any(m => m.Id == newUserMedia.MediaId))
            {
                errorMessage = "Mídia não encontrada.";
                return false;
            }
            errorMessage = "";
            return true;
        }

        private bool ValidateDuplicate(UserMedia newUserMedia, out string errorMessage)
        {
            if (Context.UserMedias.Any(x => x.UserId == newUserMedia.UserId && x.MediaId == newUserMedia.MediaId))
            {
                errorMessage = "Esta mídia já está no diário do usuário.";
                return false;
            }
            errorMessage = "";
            return true;
        }

        private bool ValidateUserMedia(UserMedia newUserMedia, out string errorMessage)
        {
            if (newUserMedia.Nota < 0 || newUserMedia.Nota > 5)
            {
                errorMessage = "A nota deve ser entre 0 e 5.";
                return false;
            }
            if (newUserMedia.DataAssistiu > DateTime.Today)
            {
                errorMessage = "A data não pode ser no futuro.";
                return false;
            }
            errorMessage = "";
            return true;
        }
    }
}