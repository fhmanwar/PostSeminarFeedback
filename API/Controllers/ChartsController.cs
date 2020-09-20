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
        public async Task<IEnumerable<TopTrainingVM>> GetTop()
        {
            var getTrainer = await _context.Trainings
                            .Include(x => x.Employee)
                            .Include("Type")
                            .Join(
                                    _context.UserRole.Include("Role"),
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
            //TopTrainingVM list = new TopTrainingVM();
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
            //var getLimit = list.OrderByDescending(x => x.Rate).Take(5);
            return list;
        }


        [HttpGet]
        [Route("pie")]
        public async Task<List<PieChartVM>> GetPie()
        { // total feedback pada title
            var getLengthTitle = await _context.Feedbacks
                                    .Include("Question")
                                    .Include(x => x.Question.Training)
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
        public async Task<List<BarChartVM>> GetBar()
        { // total feedback pada title
            var getTitle = await _context.Feedbacks
                                    .Include("Question")
                                    .Include(x => x.Question.Training)
                                    .GroupBy(grup => grup.Question.Training.Title)
                                    .Select(y => new PieChartVM
                                    {
                                        Title = y.Key,
                                        Total = y.Count()
                                    })
                                    .ToListAsync();

            if (getTitle.Count == 0)
            {
                return null;
            }
            List<BarChartVM> list = new List<BarChartVM>();
            var getCount5 = 0.0;
            var getCount4 = 0.0;
            var getCount3 = 0.0;
            var getCount2 = 0.0;
            var getCount1 = 0.0;
            foreach (var item in getTitle)
            {                
                var getLengthTitle = await _context.Feedbacks
                                   .Include("Question")
                                   .Include(x => x.Question.Training)
                                   .Where(x => x.Question.Training.Title == item.Title)
                                   .ToListAsync();
                foreach (var item2 in getLengthTitle)
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

                var top = new BarChartVM()
                {
                    Title = item.Title,
                    star1 = getCount5,
                    star2 = getCount4,
                    star3 = getCount3,
                    star4 = getCount2,
                    star5 = getCount1,
                };
                list.Add(top);
            }
            return list;
        }

    }
}
