using GameCenter.Core.Services.PlatformService;
using GameCenter.Dtos.PlatformDto;
using Microsoft.AspNetCore.Mvc;

namespace GameCenter.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformsService _platformService;

        public PlatformsController(IPlatformsService platformService)
        {
            _platformService = platformService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlatforms()
        {
            var result = await _platformService.GetPlatforms();

            if (!result.Any())
            {
                return NoContent();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddPlatform([FromBody] PlatformDto platformDto)
        {
            var result = await _platformService.AddPlatform(platformDto);

            if (!result)
            {
                return BadRequest("Platform with given name already exists");
            }

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePlatform([FromBody] PlatformDto platformDto)
        {
            var result = await _platformService.DeletePlatform(platformDto);
            if (!result)
            {
                return BadRequest("There is no platform with provided name");
            }

            return Ok();
        }

        [HttpPut("/{newName}")]
        public async Task<IActionResult> UpdatePlatform([FromBody] PlatformDto platformDto, [FromRoute] string newName)
        {
            var result = await _platformService.UpdatePlatform(platformDto, newName);
            if (!result)
            {
                return BadRequest("No platform");
            }

            return Ok();
        }
    }
}
