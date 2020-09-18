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

        //[HttpGet("trainer/{id}")]
        //public async Task<List<TrainerVM>> GetTrainer(string id)
        //{
        //    var getId = await _context.Trainings
        //                    .Include(x => x.Employee)
        //                    .Where(x => x.Employee.EmpId == id)
        //                    .ToListAsync();
        //    if (getId.Count == 0)
        //    {
        //        return null;
        //    }
        //    List<TrainerVM> list = new List<TrainerVM>();
        //    foreach (var item in getId)
        //    {
        //        var trainer = new TrainerVM()
        //        {
        //            EmpId = item.Employee.EmpId,
        //            EmployeeName = item.Employee.Name,
        //            TrainingId = item.Id,
        //            TrainingTitle = item.Title,
        //        };
        //        list.Add(trainer);
        //    }
        //    return list;
        //}
    }
}
