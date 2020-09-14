using Bcrypt = BCrypt.Net.BCrypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using API.Context;
using API.Models;
using API.Services;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MyContext _context;
        AttrEmail attrEmail = new AttrEmail();
        RandomDigit randDig = new RandomDigit();
        SmtpClient client = new SmtpClient();
        public IConfiguration _configuration;

        public UsersController(MyContext myContext, IConfiguration config)
        {
            _context = myContext;
            _configuration = config;
        }

        [HttpGet]
        public async Task<List<UserVM>> GetAll()
        {
            List<UserVM> list = new List<UserVM>();
            var getData = await _context.UserRole.Include("Role").Include("User").Include(x => x.User.Employee).ToListAsync();
            if (getData.Count == 0)
            {
                return null;
            }
            foreach (var item in getData)
            {
                var user = new UserVM()
                {
                    Id = item.User.Id,
                    Name = item.User.Employee.Name,
                    NIK = item.User.Employee.NIK,
                    Site = item.User.Employee.AssignmentSite,
                    Email = item.User.Email,
                    Password = item.User.Password,
                    Phone = item.User.Employee.Phone,
                    Address = item.User.Employee.Address,
                    RoleID = item.Role.Id,
                    RoleName = item.Role.Name,
                    VerifyCode = item.User.VerifyCode,
                };
                list.Add(user);
            }
            return list;
        }

        [HttpGet("{id}")]
        public UserVM GetID(string id)
        {
            var getData = _context.UserRole.Include("Role").Include("User").Include(x => x.User.Employee).SingleOrDefault(x => x.UserId == id);
            if (getData == null || getData.Role == null || getData.User == null)
            {
                return null;
            }
            var user = new UserVM()
            {
                Id = getData.User.Id,
                Name = getData.User.Employee.Name,
                NIK = getData.User.Employee.NIK,
                Site = getData.User.Employee.AssignmentSite,
                Email = getData.User.Email,
                Password = getData.User.Password,
                Phone = getData.User.Employee.Phone,
                Address = getData.User.Employee.Address,
                RoleID = getData.Role.Id,
                RoleName = getData.Role.Name,
            };
            return user;
        }

        [HttpPost]
        public IActionResult Create(UserVM userVM)
        {
            if (ModelState.IsValid)
            {
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(attrEmail.mail, attrEmail.pass);

                var code = randDig.GenerateRandom();
                var fill = "Hi " + userVM.Name + "\n\n"
                          + "Please verifty Code for this Apps : \n"
                          + code
                          + "\n\nThank You";

                MailMessage mm = new MailMessage("donotreply@domain.com", userVM.Email, "Create Email", fill);
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                client.Send(mm);

                var user = new User
                {
                    Email = userVM.Email,
                    Password = Bcrypt.HashPassword(userVM.Password),
                    VerifyCode = code,
                };
                _context.Users.Add(user);
                var uRole = new UserRole
                {
                    UserId = user.Id,
                    RoleId = "2"
                };
                _context.UserRole.Add(uRole);
                var emp = new Employee
                {
                    EmpId = user.Id,
                    Name = userVM.Name,
                    NIK = userVM.NIK,
                    AssignmentSite = userVM.Site,
                    Phone = userVM.Phone,
                    Address = userVM.Address,
                    CreateData = DateTimeOffset.Now,
                    isDelete = false
                };
                _context.Employees.Add(emp);
                _context.SaveChanges();
                return Ok("Successfully Created");
            }
            return BadRequest("Not Successfully");
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, UserVM userVM)
        {
            if (ModelState.IsValid)
            {
                var getData = _context.UserRole.Include("Role").Include("User").Include(x => x.User.Employee).SingleOrDefault(x => x.UserId == id);
                //var getId = _context.Users.SingleOrDefault(x => x.Id == id);
                getData.User.Employee.Name = userVM.Name;
                getData.User.Email = userVM.Email;
                getData.User.Employee.Phone = userVM.Phone;
                if (!Bcrypt.Verify(userVM.Password, getData.User.Password))
                {
                    getData.User.Password = Bcrypt.HashPassword(userVM.Password);
                }
                getData.RoleId = userVM.RoleID;

                _context.UserRole.Update(getData);
                _context.SaveChanges();
                return Ok("Successfully Updated");
            }
            return BadRequest("Not Successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var getId = _context.Users.Find(id);
            _context.Users.Remove(getId);
            _context.SaveChanges();
            return Ok(new { msg = "Successfully Delete" });
        }
    }
}
