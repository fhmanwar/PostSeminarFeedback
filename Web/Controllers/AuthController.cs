using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
    public class AuthController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44337/api/auths/")
        };

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

        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/login");
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

        [Route("validate")]
        public IActionResult Validate(UserVM userVM)
        {
            var json = JsonConvert.SerializeObject(userVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (userVM.Name == null)
            { 
                HttpResponseMessage result = null;
                if (userVM.VerifyCode != null)
                { // Verify Code
                    result = client.PostAsync("code/", byteContent).Result;
                }
                else if (userVM.VerifyCode == null)
                { // Login
                    result = client.PostAsync("login/", byteContent).Result;
                }

                if (result.IsSuccessStatusCode)
                {
                    var data = result.Content.ReadAsStringAsync().Result;
                    if (data != null)
                    {
                        HttpContext.Session.SetString("token", "Bearer " + data);
                        var handler = new JwtSecurityTokenHandler();
                        var tokenS = handler.ReadJwtToken(data);
                        var jwtPayloadSer = JsonConvert.SerializeObject(tokenS.Payload.ToDictionary(x => x.Key, x => x.Value));
                        var jwtPayloadDes = JsonConvert.DeserializeObject(jwtPayloadSer).ToString();
                        var account = JsonConvert.DeserializeObject<UserVM>(jwtPayloadSer);

                        if (!account.VerifyCode.Equals(""))
                        {
                            return Json(new { status = true, msg = "VerifyCode" });
                        }
                        else if (account.RoleName != null)
                        {
                            HttpContext.Session.SetString("id", account.Id);
                            HttpContext.Session.SetString("name", account.Name);
                            HttpContext.Session.SetString("email", account.Email);
                            HttpContext.Session.SetString("lvl", account.RoleName);
                            if (account.RoleName == "Admin")
                            {
                                return Json(new { status = true, msg = "Login Successfully !" });
                            }
                            return Json(new { status = true, msg = "Login Successfully !" });
                        }
                        return Json(new { status = false, msg = "You Don't Have Permissions! Please Contact Administrator" });
                    }
                    return Json(new { status = false, msg = result.Content.ReadAsStringAsync().Result });
                }
                return Json(new { status = false, msg = result.Content.ReadAsStringAsync().Result });
            }
            else if (userVM.Name != null)
            { // Register
                var result = client.PostAsync("register/", byteContent).Result;
                if (result.IsSuccessStatusCode)
                {
                    return Json(new { status = true, code = result, msg = "Register Success! " });
                }
                return Json(new { status = false, msg = result.Content.ReadAsStringAsync().Result });
            }
            return Redirect("/login");
        }
    }
}
