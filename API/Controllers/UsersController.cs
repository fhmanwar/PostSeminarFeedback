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
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly MyContext _context;
        public IConfiguration _configuration;

        public RolesController(MyContext myContext, IConfiguration config)
        {
            _context = myContext;
            _configuration = config;
        }

        //[Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<List<RoleVM>> GetAll()
        {
            List<RoleVM> list = new List<RoleVM>();
            var getData = await _context.Roles.Where(x => x.isDelete == false).ToListAsync();
            if (getData.Count == 0)
            {
                return null;
            }
            foreach (var item in getData)
            {
                var user = new RoleVM()
                {
                    Id = item.Id,
                    Name = item.Name,
                    CreateData = item.CreateData,
                    UpdateDate = item.UpdateDate
                };
                list.Add(user);
            }
            return list;
        }

        //[Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("{id}")]
        public RoleVM GetID(string id)
        {
            var getData = _context.Roles.SingleOrDefault(x => x.Id == id);
            if (getData == null)
            {
                return null;
            }
            var role = new RoleVM()
            {
                Id = getData.Id,
                Name = getData.Name,
                CreateData = getData.CreateData,
                UpdateDate = getData.UpdateDate
            };
            return role;
        }

        [HttpPost]
        public IActionResult Create(RoleVM roleVM)
        {
            if (ModelState.IsValid)
            {
                var role = new Role
                {
                    Name = roleVM.Name,
                    CreateData = DateTimeOffset.Now,
                    isDelete = false
                };
                _context.Roles.Add(role);
                _context.SaveChanges();
                return Ok("Successfully Created");
            }
            return BadRequest("Not Successfully");
        }

        //[Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("{id}")]
        public IActionResult Update(string id, RoleVM roleVM)
        {
            if (ModelState.IsValid)
            {
                var getData = _context.Roles.SingleOrDefault(x => x.Id == id);
                getData.Name = roleVM.Name;
                getData.UpdateDate = DateTimeOffset.Now;

                _context.Roles.Update(getData);
                _context.SaveChanges();
                return Ok("Successfully Updated");
            }
            return BadRequest("Not Successfully");
        }

        //[Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var getData = _context.Roles.SingleOrDefault(x => x.Id == id);
            if (getData == null)
            {
                return BadRequest("Not Successfully");
            }
            getData.DeleteData = DateTimeOffset.Now;
            getData.isDelete = true;

            _context.Entry(getData).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok(new { msg = "Successfully Delete" });
        }

    }

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

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<List<UserVM>> GetAll()
        {
            List<UserVM> list = new List<UserVM>();
            var getData = await _context.UserRole.Include("Role").Include("User").Include(x => x.User.Employee).Where(x => x.User.Employee.isDelete == false).ToListAsync();
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

        [Authorize(AuthenticationSchemes = "Bearer")]
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
                var checkRole = _context.Roles.SingleOrDefault(x => x.Name == "User");
                var uRole = new UserRole
                {
                    UserId = user.Id,
                    RoleId = checkRole.Id
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

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("{id}")]
        public IActionResult Update(string id, UserVM userVM)
        {
            if (ModelState.IsValid)
            {
                var getData = _context.UserRole.Include("Role").Include("User").Include(x => x.User.Employee).SingleOrDefault(x => x.UserId == id);
                //var getId = _context.Users.SingleOrDefault(x => x.Id == id);
                getData.User.Employee.Name = userVM.Name;
                getData.User.Employee.NIK = userVM.NIK;
                getData.User.Employee.AssignmentSite = userVM.Site;
                getData.User.Employee.Phone = userVM.Phone;
                getData.User.Employee.Address = userVM.Address;
                getData.User.Email = userVM.Email;
                if (userVM.Password != null)
                {
                    if (!Bcrypt.Verify(userVM.Password, getData.User.Password))
                    {
                        getData.User.Password = Bcrypt.HashPassword(userVM.Password);
                    }
                }

                if (userVM.RoleID != null)
                {
                    getData.RoleId = userVM.RoleID;
                }

                _context.UserRole.Update(getData);
                _context.SaveChanges();
                return Ok("Successfully Updated");
            }
            return BadRequest("Not Successfully");
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var getData = _context.Employees.Include("User").SingleOrDefault(x => x.EmpId == id);
            if (getData == null)
            {
                return BadRequest("Not Successfully");
            }
            getData.DeleteData = DateTimeOffset.Now;
            getData.isDelete = true;

            _context.Entry(getData).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok(new { msg = "Successfully Delete" });
        }

    }

    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        readonly MyContext _context;
        public IConfiguration _configuration;

        public AuthsController(MyContext myContext, IConfiguration config)
        {
            _context = myContext;
            _configuration = config;
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(UserVM userVM)
        {
            UsersController _usersController = new UsersController(_context, _configuration);
            if (ModelState.IsValid)
            {
                return _usersController.Create(userVM);
            }
            return BadRequest("Data Not Valid");
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(UserVM userVM)
        {
            if (ModelState.IsValid)
            {
                var getData = _context.UserRole.Include("Role").Include("User").Include(x => x.User.Employee).SingleOrDefault(x => x.User.Email == userVM.Email);
                if (getData == null)
                {
                    return NotFound("Email Not Found");
                }
                else if (userVM.Password == null || userVM.Password.Equals(""))
                {
                    return BadRequest("Password must filled");
                }
                else if (!Bcrypt.Verify(userVM.Password, getData.User.Password))
                {
                    return BadRequest("Password is Wrong");
                }
                else
                {
                    if (getData != null)
                    {
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
                            VerifyCode = getData.User.VerifyCode,
                        };
                        return Ok(GetJWT(user));
                    }
                    return BadRequest("Invalid credentials");
                }
            }
            return BadRequest("Data Not Valid");
        }

        [HttpPost]
        [Route("code")]
        public IActionResult VerifyCode(UserVM userVM)
        {
            if (ModelState.IsValid)
            {
                var getData = _context.UserRole.Include("Role").Include("User").Include(x => x.User.Employee).SingleOrDefault(x => x.User.Email == userVM.Email);
                if (getData == null)
                {
                    return NotFound();
                }
                else if (userVM.VerifyCode != getData.User.VerifyCode)
                {
                    return BadRequest("Your Code is Wrong");
                }
                else
                {
                    getData.User.VerifyCode = null;
                    _context.SaveChanges();
                    var user = new UserVM()
                    {
                        Id = getData.User.Id,
                        Name = getData.User.Employee.Name,
                        Email = getData.User.Email,
                        Password = getData.User.Password,
                        Phone = getData.User.Employee.Phone,
                        RoleID = getData.Role.Id,
                        RoleName = getData.Role.Name,
                        VerifyCode = getData.User.VerifyCode,
                    };
                    return StatusCode(200, GetJWT(user));
                }
            }
            return BadRequest("Data Not Valid");
        }

        private string GetJWT(UserVM userVM)
        {
            var claims = new List<Claim> {
                            new Claim("Id", userVM.Id),
                            new Claim("Name", userVM.Name),
                            new Claim("Email", userVM.Email),
                            new Claim("RoleName", userVM.RoleName),
                            new Claim("VerifyCode", userVM.VerifyCode == null ? "" : userVM.VerifyCode),
                        };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                            _configuration["Jwt:Issuer"],
                            _configuration["Jwt:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(1),
                            signingCredentials: signIn
                        );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
