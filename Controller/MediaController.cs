using System.ComponentModel;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using obrazo.Models.Entities;
using obrazo.Services.Interfaces;

namespace obrazo.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class MediaController : ControllerBase
    {
        private readonly IMediaService mediaService;

        public MediaController(IMediaService _mediaService)
        {
            mediaService = _mediaService;
        }


        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            Media result;
            try
            {
                result = await mediaService.GetById(id);
            }
            catch (KeyNotFoundException argument)
            {
                return Problem(
                    detail: argument.Message,
                    statusCode: StatusCodes.Status404NotFound
                );
            }
            catch (UnauthorizedAccessException argument)
            {
                return Problem(
                detail: argument.Message,
                statusCode: StatusCodes.Status403Forbidden);
            }
            return Ok(result);
        }

        [Authorize]
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? name, [FromQuery] int? type)
        {
            List<Media> medias;
            try
            {
                medias = await mediaService.Search(name, type);
            }
            catch (InvalidEnumArgumentException arg)
            {
                return Problem(
                     detail: arg.Message,
                     statusCode: StatusCodes.Status400BadRequest
                 );
            }
            catch (ArgumentException arg)
            {
                return Problem(
                    detail: arg.Message,
                    statusCode: StatusCodes.Status404NotFound
                );
            }
            return Ok(medias);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Media novaMedia)
        {
            try
            {
                await mediaService.Create(novaMedia);
            }
            catch (KeyNotFoundException arg)
            {
                return Problem(
                    detail: arg.Message,
                    statusCode: StatusCodes.Status404NotFound
                );
            }
            catch (ArgumentException arg)
            {
                return Problem(
                    detail: arg.Message,
                    statusCode: StatusCodes.Status400BadRequest
                );
            }
            catch (DuplicateNameException arg)
            {
                return Problem(
                    detail: arg.Message,
                    statusCode: StatusCodes.Status409Conflict
                );
            }

            return CreatedAtAction(nameof(GetById), new { id = novaMedia.Id }, novaMedia);
        }
    }
}