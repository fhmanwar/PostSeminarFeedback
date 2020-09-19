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
    public class ChartsController : ControllerBase
    {
        private readonly MyContext _context;
        public ChartsController(MyContext myContext)
        {
            _context = myContext;
        }

        [HttpGet]
        [Route("toptraining")]
        public async Task<List<TopTrainingVM>> GetTop()
        {
            var getTrainer = await _context.Trainings
                            .Include(x => x.Employee)
                            .Include("Type")
                            .Join(
                                    _context.UserRole,
                                    emp => emp.UserId,
                                    uRole => uRole.UserId,
                                    (emp, uRole) => new { UserRole = uRole, Trainings = emp })
                            .Where(x => x.Trainings.Employee.isDelete == false && x.UserRole.Role.Name == "Trainer")
                            .ToListAsync();
            //5 star - 252
            //4 star - 124
            //3 star - 40
            //2 star - 29
            //1 star - 33
            //(5 * 252 + 4 * 124 + 3 * 40 + 2 * 29 + 1 * 33) / (252 + 124 + 40 + 29 + 33) = 4.11 and change
            if (getTrainer.Count == 0)
            {
                return null;
            }
            List<TopTrainingVM> list = new List<TopTrainingVM>();
            var getCount5 = 0.0;
            var getCount4 = 0.0;
            var getCount3 = 0.0;
            var getCount2 = 0.0;
            var getCount1 = 0.0;
            foreach (var item in getTrainer)
            {
                var getRate = await _context.Feedbacks
                                    .Include("Employee")
                                    .Include("Question")
                                    .Include(x => x.Question.Training)
                                    .Include(x => x.Question.Training.Employee)
                                    .Where(x => x.Question.Training.Employee.EmpId == item.Trainings.Employee.EmpId && x.Question.Training.Title == item.Trainings.Title)
                                    .ToListAsync();
                foreach (var item2 in getRate)
                {
                    if (item2.Rate > 4.0)
                    {
                        getCount5++;
                    }
                    else if (item2.Rate > 3.0)
                    {
                        getCount4++;
                    }
                    else if (item2.Rate > 2.0)
                    {
                        getCount3++;
                    }
                    else if (item2.Rate > 1.0)
                    {
                        getCount2++;
                    }
                    else if (item2.Rate == 1.0)
                    {
                        getCount1++;
                    }
                }

                var sum = (getCount5 + getCount4 + getCount3 + getCount2 + getCount1);
                var sumMultiply = ((getCount5 * 5) + (getCount4 * 4) + (getCount3 * 3) + (getCount2 * 2) + (getCount1 * 1));
                var total = sumMultiply / sum;
                var top = new TopTrainingVM()
                {
                    Title = item.Trainings.Title,
                    Trainer = item.Trainings.Employee.Name,
                    TypeTraining = item.Trainings.Type.Name,
                    Rate = total,
                };
                list.Add(top);
            }
            return list;
        }


        [HttpGet]
        [Route("pie")]
        public async Task<List<PieChartVM>> GetPie()
        { // total feedback pada title

            //var user = new UserVM();
            //var getData = await _context.Divisions
            //                    .Join(
            //                        _context.Departments,
            //                        di => di.DepartmentId,
            //                        de => de.Id,
            //                        (di,de) => new { Divisions = di, Departments = de })
            //                    .Where(x => x.Divisions.isDelete == false)
            //                    .ToListAsync();

            //var getData = await _context.Divisions.Include("Department").Where(x => x.isDelete == false).ToListAsync();
            //var data = await _context.Divisions
            //                .Join(_context.Departments, 
            //                        di => di.DepartmentId, 
            //                        de => de.Id, 
            //                        (di, de) => new { 
            //                            Divisions = di, Departments = de 
            //                        }).GroupBy(q => q.Departments.Name).Select(q => new
            //                        {
            //                            GroupId = q.Key,
            //                            Count = q.Count()
            //                        }).ToListAsync();

            var getLengthTitle = await _context.Feedbacks
                                    .Include("Question")
                                    .Include(x => x.Question.Training)
                                    .Where(x => x.isDelete == false)
                                    .GroupBy(grup => grup.Question.Training.Title)
                                    .Select(y => new PieChartVM
                                    {
                                        Title = y.Key,
                                        Total = y.Count()
                                    })
                                    .ToListAsync();
            return getLengthTitle;
        }

        [HttpGet]
        [Route("bar")]
        public async Task<List<PieChartVM>> GetBar()
        { // total feedback pada title

            var getLengthTitle = await _context.Feedbacks
                                    .Include("Question")
                                    .Include(x => x.Question.Training)
                                    .Where(x => x.isDelete == false)
                                    .GroupBy(grup => grup.Question.Training.Title)
                                    .Select(y => new PieChartVM
                                    {
                                        Title = y.Key,
                                        Total = y.Count()
                                    })
                                    .ToListAsync();
            return getLengthTitle;
        }

    }
}
