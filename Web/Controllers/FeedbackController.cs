using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Controllers
{
    public class FeedbackController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44337/api/feedbacks/")
        };

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LoadData()
        {
            IEnumerable<Feedback> feedbacks = null;
            //client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("token"));
            var resTask = client.GetAsync("");
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<List<Feedback>>();
                readTask.Wait();
                feedbacks = readTask.Result;
            }
            else
            {
                feedbacks = Enumerable.Empty<Feedback>();
                ModelState.AddModelError(string.Empty, "Server Error try after sometimes.");
            }
            return Json(feedbacks);

        }

        public IActionResult GetById(int Id)
        {
            Feedback datas = null;
            //client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("token"));
            var resTask = client.GetAsync("" + Id);
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                datas = JsonConvert.DeserializeObject<Feedback>(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server Error.");
            }
            return Json(datas);
        }

        public IActionResult InsertOrUpdate(Feedback datas, int id)
        {
            try
            {
                var json = JsonConvert.SerializeObject(datas);
                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                //client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("token"));
                if (datas.Id == 0)
                {
                    var result = client.PostAsync("", byteContent).Result;
                    return Json(result);
                }
                else if (datas.Id == id)
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
