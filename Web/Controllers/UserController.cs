using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using API.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class UserController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44337/api/users/")
        };
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult LoadData()
        {
            IEnumerable<UserVM> userVM = null;
            //client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("token"));
            var resTask = client.GetAsync("");
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<List<UserVM>>();
                readTask.Wait();
                userVM = readTask.Result;
            }
            else
            {
                userVM = Enumerable.Empty<UserVM>();
                ModelState.AddModelError(string.Empty, "Server Error try after sometimes.");
            }
            return Json(userVM);

        }
    }
}
