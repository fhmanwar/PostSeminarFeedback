using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Controllers
{
    public class DashboardController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44337/api/")
        };
        public IActionResult Index()
        {
            if (HttpContext.Session.IsAvailable)
            {
                if (HttpContext.Session.GetString("lvl") == "Admin")
                {
                    return View();
                }
                else if (HttpContext.Session.GetString("lvl") == "Trainer")
                {
                    return Redirect("/trainer");
                }
                else
                {
                    return Redirect("/employee");
                }
            }
            return RedirectToAction("Login", "Auth");
        }

        [Route("profile")]
        public IActionResult Profile()
        {
            return View("~/Views/Dashboard/Profile.cshtml");
        }

        [Route("GetProfile")]
        public IActionResult GetProfile()
        {
            UserVM data = null;
            var id = HttpContext.Session.GetString("id");
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("token"));
            var resTask = client.GetAsync("users/" + id);
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                data = JsonConvert.DeserializeObject<UserVM>(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server Error.");
            }
            return Json(data);
        }

        [Route("updProfile")]
        public IActionResult UpdProfile(UserVM data)
        {
            var id = HttpContext.Session.GetString("id");
            try
            {
                var json = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                if (data.Id == id)
                {
                    client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("token"));
                    var result = client.PutAsync("users/" + id, byteContent).Result;
                    HttpContext.Session.Remove("name");
                    HttpContext.Session.SetString("name", data.Name);
                    return Json(result);
                }

                return Json(404);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public IActionResult LoadTop()
        {
            IEnumerable<TopTrainingVM> top = null;
            //var token = HttpContext.Session.GetString("token");
            //client.DefaultRequestHeaders.Add("Authorization", token);
            var resTask = client.GetAsync("Charts/toptraining");
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<List<TopTrainingVM>>();
                readTask.Wait();
                top = readTask.Result.OrderByDescending(x => x.Rate).Take(5);
            }
            else
            {
                top = Enumerable.Empty<TopTrainingVM>();
                ModelState.AddModelError(string.Empty, "Server Error try after sometimes.");
            }
            return Json(top);
        }

        public IActionResult LoadTopBar()
        {
            IEnumerable<TopTrainingVM> top = null;
            //var token = HttpContext.Session.GetString("token");
            //client.DefaultRequestHeaders.Add("Authorization", token);
            var resTask = client.GetAsync("Charts/toptraining");
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<List<TopTrainingVM>>();
                readTask.Wait();
                top = readTask.Result;
            }
            else
            {
                top = Enumerable.Empty<TopTrainingVM>();
                ModelState.AddModelError(string.Empty, "Server Error try after sometimes.");
            }
            return Json(top);
        }

        public IActionResult LoadPie()
        {
            IEnumerable<PieChartVM> pie = null;
            //var token = HttpContext.Session.GetString("token");
            //client.DefaultRequestHeaders.Add("Authorization", token);
            var resTask = client.GetAsync("charts/pie");
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<List<PieChartVM>>();
                readTask.Wait();
                pie = readTask.Result;
            }
            else
            {
                pie = Enumerable.Empty<PieChartVM>();
                ModelState.AddModelError(string.Empty, "Server Error try after sometimes.");
            }
            return Json(pie);
        }

        public IActionResult LoadBar()
        {
            IEnumerable<BarChartVM> bar = null;
            //var token = HttpContext.Session.GetString("token");
            //client.DefaultRequestHeaders.Add("Authorization", token);
            var resTask = client.GetAsync("charts/bar");
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<List<BarChartVM>>();
                readTask.Wait();
                bar = readTask.Result;
            }
            else
            {
                bar = Enumerable.Empty<BarChartVM>();
                ModelState.AddModelError(string.Empty, "Server Error try after sometimes.");
            }
            return Json(bar);
        }
    }
}
