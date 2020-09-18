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
    public class TrainersController : ControllerBase
    {
        private readonly MyContext _context;
        public TrainersController(MyContext myContext)
        {
            _context = myContext;
        }

        [HttpGet("trainer/{id}")]
        public async Task<List<TrainerVM>> GetTrainer(string id)
        {
            var getId = await _context.Trainings
                            .Include(x => x.Employee)
                            .Where(x => x.Employee.EmpId == id)
                            .ToListAsync();
            if (getId.Count == 0)
            {
                return null;
            }
            List<TrainerVM> list = new List<TrainerVM>();
            foreach (var item in getId)
            {
                var trainer = new TrainerVM()
                {
                    EmpId = item.Employee.EmpId,
                    EmployeeName = item.Employee.Name,
                    TrainingId = item.Id,
                    TrainingTitle = item.Title,
                };
                list.Add(trainer);
            }
            return list;
        }

        [HttpGet("trainquest/{id}")]
        public async Task<List<TrainerVM>> GetTrainerQuestion(string id)
        {
            var getQues = await _context.Questions.Include("Training").Include(x => x.Training.Employee).Where(x => x.isDelete == false && x.Training.Employee.EmpId == id).ToListAsync();
            if (getQues.Count == 0)
            {
                return null;
            }
            List<TrainerVM> list = new List<TrainerVM>();
            foreach (var item in getQues)
            {
                var trainer = new TrainerVM()
                {
                    EmpId = item.Training.Employee.EmpId,
                    EmployeeName = item.Training.Employee.Name,
                    TrainingId = item.Training.Id,
                    TrainingTitle = item.Training.Title,
                    QuestionId = item.Id,
                    QuestionDesc  = item.QuestionDesc,
                };
                list.Add(trainer);
            }
            return list;
        }

        [HttpGet("trainfeedback/{id}")]
        public async Task<List<TrainerFeedbackVM>> GetTrainerfeedback(string id)
        {
            var getfeed = await _context.Feedbacks.Include("Employee").Include("Question").Include(x => x.Question.Training).Include(x => x.Question.Training.Employee).Where(x => x.isDelete == false && x.Question.Training.Employee.EmpId == id).ToListAsync();
            if (getfeed.Count == 0)
            {
                return null;
            }
            List<TrainerFeedbackVM> list = new List<TrainerFeedbackVM>();
            foreach (var item in getfeed)
            {
                var trainer = new TrainerFeedbackVM()
                {
                    Id = item.Id,
                    Review = item.Review,
                    Rate = item.Rate,
                    EmpId = item.Employee.EmpId,
                    EmployeeName = item.Employee.Name,
                    TrainingId = item.Question.Training.Id,
                    TrainingTitle = item.Question.Training.Title,
                    Trainer = item.Question.Training.Employee.Name,
                    QuestionId = item.Question.Id,
                    QuestionDesc = item.Question.QuestionDesc,
                };
                list.Add(trainer);
            }
            return list;
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class TopTrainersController : ControllerBase
    {
        private readonly MyContext _context;
        public TopTrainersController(MyContext myContext)
        {
            _context = myContext;
        }

        [HttpGet]
        [Route("top")]
        public async Task<List<TopTrainingVM>> GetTrainer()
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
                    if (item2.Rate > 4.0) {
                        getCount5++;
                    } else if (item2.Rate > 3.0) {
                        getCount4++;
                    } else if (item2.Rate > 2.0) {
                        getCount3++;
                    } else if (item2.Rate > 1.0) {
                        getCount2++;
                    } else if (item2.Rate == 1.0) {
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

        
    }
}
