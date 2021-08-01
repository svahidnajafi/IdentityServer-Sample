using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebClientMVC.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = User;
            return await Task.Run(View);
        }

        [HttpGet]
        [Route("/secret", Name = "secret")]
        [Authorize]
        public async Task<IActionResult> Secret()
        {
            return await Task.Run(View);
        }
        
        [HttpGet]
        [Route("/top/secret", Name = "topSecret")]
        [Authorize(Policy = PolicyStore.TopSecret)]
        public async Task<IActionResult> TopSecret()
        {
            return await Task.Run(View);
        }

        [HttpGet]
        [Route("/god", Name = "god")]
        [Authorize(Policy = PolicyStore.God)]
        public async Task<IActionResult> God()
        {
            return await Task.Run(View);
        }
    }
}