using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Context;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly MyContext _context;
        public EmployeesController(MyContext myContext)
        {
            _context = myContext;
        }

        [HttpGet]
        [Route("getTrainer")]
        public async Task<List<UserVM>> GetOnlyTrainer()
        {
            var getfeed = await _context.UserRole.Include("User").Include("Role").Include(x => x.User.Employee).Where(x => x.User.Employee.isDelete == false && x.Role.Name == "Trainer").ToListAsync();
            if (getfeed.Count == 0)
            {
                return null;
            }
            List<UserVM> list = new List<UserVM>();
            foreach (var item in getfeed)
            {
                var trainer = new UserVM()
                {
                    Id = item.User.Id,
                    Name = item.User.Employee.Name,
                    RoleID = item.Role.Id,
                    RoleName = item.Role.Name,
                };
                list.Add(trainer);
            }
            return list;
        }
    }
}
