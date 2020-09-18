using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class TrainerController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44337/api/trainers/")
        };

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MyFeedback()
        {
            return View("~/Views/Trainer/Feedback.cshtml");
        }

        public IActionResult Loadtrainer()
        {
            IEnumerable<TrainerVM> trainerVM = null;
            var getId = HttpContext.Session.GetString("id");
            //client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("token"));
            var resTask = client.GetAsync("trainer/" + getId);
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
        public IActionResult LoadtrainerQuest()
        {
            IEnumerable<TrainerVM> trainerVM = null;
            var getId = HttpContext.Session.GetString("id");
            //client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("token"));
            var resTask = client.GetAsync("trainquest/" + getId);
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
        public IActionResult Loadtrainerfeedback()
        {
            IEnumerable<TrainerFeedbackVM> trainerVM = null;
            var getId = HttpContext.Session.GetString("id");
            //client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("token"));
            var resTask = client.GetAsync("trainfeedback/" + getId);
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<List<TrainerFeedbackVM>>();
                readTask.Wait();
                trainerVM = readTask.Result;
            }
            else
            {
                trainerVM = Enumerable.Empty<TrainerFeedbackVM>();
                ModelState.AddModelError(string.Empty, "Server Error try after sometimes.");
            }
            return Json(trainerVM);
        }
    }
}
