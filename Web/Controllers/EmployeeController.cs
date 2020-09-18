using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using API.Models;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public IActionResult LoadtrainerQuest(string id)
        {
            IEnumerable<TrainerVM> trainerVM = null;
            //client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("token"));
            var resTask = client.GetAsync("trainers/trainquest/" + id);
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

        //public IActionResult Insert(IEnumerable<Feedback> arrCreate)
        public IActionResult Insert(Feedback datas)
        {
            try
            {
                var feedb = new Feedback
                {
                    UserId = HttpContext.Session.GetString("id"),
                    Rate = datas.Rate,
                    QuestionId = datas.QuestionId
                };
                var json = JsonConvert.SerializeObject(feedb);
                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                //client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("token"));
                if (datas.Id == 0)
                {
                    var result = client.PostAsync("feedbacks", byteContent).Result;
                    return Json(result);
                }
                return Json(404);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
