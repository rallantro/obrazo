using System.ComponentModel;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using obrazo.Models.Entities;
using obrazo.Models.Enum;
using obrazo.Services.Interfaces;

namespace obrazo.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class MediaUserController : ControllerBase
    {
        private readonly IUserMediaService UserMediaService;

        public MediaUserController(IUserMediaService userMediaService)
        {
            UserMediaService = userMediaService;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var userIdClaim = User.FindFirst("userID")?.Value;
            if (!int.TryParse(userIdClaim, out int UserId))
                return Unauthorized();
            UserMedia? result;
            try
            {
                result = await UserMediaService.GetById(id, UserId);
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
        [HttpGet("user")]
        public async Task<IActionResult> GetByUser()
        {
            var UserIdClaim = User.FindFirst("userID")?.Value;
            if (!int.TryParse(UserIdClaim, out int UserId))
                return Unauthorized();
            List<UserMedia> userMedia;
            try
            {
                userMedia = await UserMediaService.GetByUser(UserId);
            }
            catch (ArgumentException arg)
            {
                return Problem(
                    detail: arg.Message,
                    statusCode: StatusCodes.Status404NotFound
                );

            }
            return Ok(userMedia);
        }

        [Authorize]
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? name, [FromQuery] int? type)
        {
            var UserIdClaim = User.FindFirst("userID")?.Value;
            if (!int.TryParse(UserIdClaim, out int UserId))
                return Unauthorized();
            List<UserMedia> userMedia;
            try
            {
                userMedia = await UserMediaService.Search(UserId, name, type);
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
            return Ok(userMedia);
        }

        [Authorize]
        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetByType(int type)
        {
            var UserIdClaim = User.FindFirst("userID")?.Value;
            if (!int.TryParse(UserIdClaim, out int UserId))
                return Unauthorized();
            if (!Enum.IsDefined(typeof(MediaType), type))
            {
                return BadRequest("Tipo de mídia inválido.");
            }
            List<UserMedia> userMedia;
            try
            {
                userMedia = await UserMediaService.GetByType(UserId, type);
            }
            catch (ArgumentException arg)
            {
                return Problem(
                    detail: arg.Message,
                    statusCode: StatusCodes.Status404NotFound
                );

            }
            return Ok(userMedia);
        }

        [Authorize]
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetByStatus(int status)
        {
            var UserIdClaim = User.FindFirst("userID")?.Value;
            if (!int.TryParse(UserIdClaim, out int UserId))
                return Unauthorized();
            if (!Enum.IsDefined(typeof(MediaStatus), status))
            {
                return BadRequest("Tipo de staus inválido.");
            }
            List<UserMedia> userMedia;
            try
            {
                userMedia = await UserMediaService.GetByStatus(UserId, status);
            }
            catch (ArgumentException arg)
            {
                return Problem(
                    detail: arg.Message,
                    statusCode: StatusCodes.Status404NotFound
                );
            }
            return Ok(userMedia);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserMedia novoUserMedia)
        {
            var userIdClaim = User.FindFirst("userID")?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized("Token inválido.");
            try
            {
                await UserMediaService.Create(novoUserMedia, userId);
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

            return CreatedAtAction(nameof(Get), new { id = novoUserMedia.Id }, novoUserMedia);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserMedia updated)
        {
            var userIdClaim = User.FindFirst("userID")?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized();
            try
            {
                await UserMediaService.Update(id, updated, userId);
            }
            catch (KeyNotFoundException arg)
            {
                return Problem(
                    detail: arg.Message,
                    statusCode: StatusCodes.Status404NotFound
                );
            }
            catch (UnauthorizedAccessException arg)
            {
                return Problem(
                    detail: arg.Message,
                    statusCode: StatusCodes.Status403Forbidden
                );
            }
            catch (ArgumentException arg)
            {
                return Problem(
                    detail: arg.Message,
                    statusCode: StatusCodes.Status400BadRequest
                );
            }

            return Ok();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userIdClaim = User.FindFirst("userID")?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized();
            try
            {
                await UserMediaService.Delete(id, userId);
            }
            catch (KeyNotFoundException arg)
            {
                return Problem(
                    detail: arg.Message,
                    statusCode: StatusCodes.Status404NotFound
                );
            }
            catch (UnauthorizedAccessException arg)
            {
                return Problem(
                    detail: arg.Message,
                    statusCode: StatusCodes.Status403Forbidden
                );
            }
            return NoContent();
        }
    }
}