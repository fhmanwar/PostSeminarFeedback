using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class AuthController : Controller
    {
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [Route("verify")]
        public IActionResult Verify()
        {
            return View();
        }

        [Route("notfound")]
        public IActionResult Notfound()
        {
            return View();
        }
    }
}
