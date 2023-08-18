using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userList = await Task.FromResult(new string[] { "Remek", "Rafał", "Krzychu", "Mati", "Janek" });
            return Ok(userList);
        }
    }
}
