using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace ApiOne.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("home")]
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => Ok("Api one here, hello !"));
        }
        
        [HttpGet("/secret")]
        [Authorize]
        public async Task<IActionResult> Secret()
        {
            var user = User;
            return await Task.Run(() => Ok("Wow ! How do you know api one secret ?"));
        }
        
        [HttpGet("/top/secret")]
        [Authorize(Policy = PolicyStore.TopSecret)]
        public async Task<IActionResult> TopSecret()
        {
            var claims = User.Claims;
            return await Task.Run(() => Ok("Top secret, do not share !"));
        }
    }
}