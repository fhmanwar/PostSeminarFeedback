using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Controllers
{
    public class TypeTrainController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44337/api/typetrains/")
        };

        public IActionResult Index()
        {
            //if (HttpContext.Session.GetString("lvl") == "Admin")
            //{
            //    return View("~/Views/Dashboard/Department.cshtml");
            //}
            //return Redirect("/notfound");
            return View("~/Views/Feedback/TypeTraining.cshtml");
        }

        public IActionResult LoadType()
        {
            IEnumerable<TypeTraining> typeTrains = null;
            //client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("token"));
            var resTask = client.GetAsync("");
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<List<TypeTraining>>();
                readTask.Wait();
                typeTrains = readTask.Result;
            }
            else
            {
                typeTrains = Enumerable.Empty<TypeTraining>();
                ModelState.AddModelError(string.Empty, "Server Error try after sometimes.");
            }
            return Json(typeTrains);

        }

        public IActionResult GetById(int Id)
        {
            TypeTraining typeTrains = null;
            //client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("token"));
            var resTask = client.GetAsync("" + Id);
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                typeTrains = JsonConvert.DeserializeObject<TypeTraining>(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server Error.");
            }
            return Json(typeTrains);
        }

        public IActionResult InsertOrUpdate(TypeTraining typeTrains, int id)
        {
            try
            {
                var json = JsonConvert.SerializeObject(typeTrains);
                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                //client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("token"));
                if (typeTrains.Id == 0)
                {
                    var result = client.PostAsync("", byteContent).Result;
                    return Json(result);
                }
                else if (typeTrains.Id == id)
                {
                    var result = client.PutAsync("" + id, byteContent).Result;
                    return Json(result);
                }

                return Json(404);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult Delete(int id)
        {
            //client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("token"));
            var result = client.DeleteAsync("" + id).Result;
            return Json(result);
        }
    }
    public class TrainingController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44337/api/trainings/")
        };

        public IActionResult Index()
        {
            //if (HttpContext.Session.GetString("lvl") == "Admin")
            //{
            //    return View("~/Views/Dashboard/Department.cshtml");
            //}
            //return Redirect("/notfound");
            return View("~/Views/Feedback/Training.cshtml");
        }

        public IActionResult LoadData()
        {
            IEnumerable<Training> trains = null;
            //client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("token"));
            var resTask = client.GetAsync("");
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<List<Training>>();
                readTask.Wait();
                trains = readTask.Result;
            }
            else
            {
                trains = Enumerable.Empty<Training>();
                ModelState.AddModelError(string.Empty, "Server Error try after sometimes.");
            }
            return Json(trains);

        }

        public IActionResult GetById(int Id)
        {
            Training trains = null;
            //client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("token"));
            var resTask = client.GetAsync("" + Id);
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                trains = JsonConvert.DeserializeObject<Training>(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server Error.");
            }
            return Json(trains);
        }

        public IActionResult InsertOrUpdate(Training trains, int id)
        {
            try
            {
                var json = JsonConvert.SerializeObject(trains);
                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                //client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("token"));
                if (trains.Id == 0)
                {
                    var result = client.PostAsync("", byteContent).Result;
                    return Json(result);
                }
                else if (trains.Id == id)
                {
                    var result = client.PutAsync("" + id, byteContent).Result;
                    return Json(result);
                }

                return Json(404);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult Delete(int id)
        {
            //client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("token"));
            var result = client.DeleteAsync("" + id).Result;
            return Json(result);
        }
    }
}
