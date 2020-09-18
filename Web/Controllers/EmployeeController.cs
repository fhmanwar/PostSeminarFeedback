using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using API.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class EmployeeController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44337/api/")
        };

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Loadtrainer()
        {
            IEnumerable<UserVM> dataVM = null;
            //client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("token"));
            var resTask = client.GetAsync("employees/gettrainer/");
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<List<UserVM>>();
                readTask.Wait();
                dataVM = readTask.Result;
            }
            else
            {
                dataVM = Enumerable.Empty<UserVM>();
                ModelState.AddModelError(string.Empty, "Server Error try after sometimes.");
            }
            return Json(dataVM);
        }
        public IActionResult Loadtitle(string id)
        {
            IEnumerable<TrainerVM> trainerVM = null;
            //client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("token"));
            var resTask = client.GetAsync("trainers/trainer/" + id);
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<List<TrainerVM>>();
                readTask.Wait();
                trainerVM = readTask.Result;
            }
            else
            {
                trainerVM = Enumerable.Empty<TrainerVM>();
                ModelState.AddModelError(string.Empty, "Server Error try after sometimes.");
            }
            return Json(trainerVM);
        }
    }
}
