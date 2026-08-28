using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using obrazo.Data;
using obrazo.Models.Entities;
using obrazo.Models.Enum;

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

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var userIdClaim = User.FindFirst("userID")?.Value;
            if (!int.TryParse(userIdClaim, out int UserId))
                return Unauthorized();
            var userMedia = Context.UserMedias.Where(x => x.Id == id).Include(um => um.Media).FirstOrDefault();

            if (userMedia == null)
                return NotFound();
            if (userMedia.UserId != UserId) return Forbid();
            return Ok(userMedia);
        }

        [Authorize]
        [HttpGet("user")]
        public IActionResult GetByUser()
        {
            var UserIdClaim = User.FindFirst("userID")?.Value;
            if (!int.TryParse(UserIdClaim, out int UserId))
                return Unauthorized();
            var userMedia = Context.UserMedias.Where(x => x.UserId == UserId).Include(um => um.Media).ToList();
            if (!userMedia.Any())
                return Problem(
                    detail: "Você não possui nenhuma mídia armazenada... :p",
                    statusCode: StatusCodes.Status404NotFound
                );
            return Ok(userMedia);
        }

        [Authorize]
        [HttpGet("type/{type}")]
        public IActionResult GetByType(int type)
        {
            var UserIdClaim = User.FindFirst("userID")?.Value;
            if (!int.TryParse(UserIdClaim, out int UserId))
                return Unauthorized();
            var userMedia = Context.UserMedias.Include(um => um.Media).Where(x => x.UserId == UserId && x.Media.Type == (MediaType)type).ToList();
            string nameType = ((MediaType)type).ToString();
            if (!userMedia.Any())
                return Problem(
                    detail: $"Você não possui nenhum(a) {nameType.ToLower()} armazenado(a)... :p",
                    statusCode: StatusCodes.Status404NotFound
                );
            if (!Enum.IsDefined(typeof(MediaType), type))
                return BadRequest("Tipo de mídia inválido.");
            return Ok(userMedia);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody] UserMedia novoUserMedia)
        {
            var userIdClaim = User.FindFirst("userID")?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized("Token inválido.");

            novoUserMedia.UserId = userId;

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

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UserMedia updated)
        {
            var userIdClaim = User.FindFirst("userID")?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized();


            if (!ValidateUserMedia(updated, out string errorMessageValid))
            {
                return BadRequest(errorMessageValid);
            }
            var existing = Context.UserMedias.Find(id);
            if (existing == null) return NotFound();
            if (existing.UserId != userId) return Forbid();
            existing.Nota = updated.Nota;
            existing.Opiniao = updated.Opiniao;
            existing.Status = updated.Status;
            existing.DataAssistiu = updated.DataAssistiu;

            Context.SaveChanges();
            return Ok(existing);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var userIdClaim = User.FindFirst("userID")?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            var userMedia = Context.UserMedias.Find(id);
            if (userMedia == null) return NotFound();
            if (userMedia.UserId != userId) return Forbid();

            Context.UserMedias.Remove(userMedia);
            Context.SaveChanges();
            return NoContent();
        }

        private bool ValidateExistence(UserMedia newUserMedia, out string errorMessage)
        {
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