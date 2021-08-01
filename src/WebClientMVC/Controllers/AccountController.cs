using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebClientMVC.Controllers
{
    public class AccountController : Controller
    {
        // GET
        [Route("/account/signin", Name = "signin")]
        [Authorize]
        public async Task<IActionResult> SignIn()
        {
            return await Task.Run(() => RedirectToAction("Index", "Home"));
        }

        [Route("/account/signout", Name = "signout")]
        public async Task<IActionResult> SignOutAsync()
        {
            return await Task.Run(() => SignOut("Cookie", "oidc"));
        }
    }
}