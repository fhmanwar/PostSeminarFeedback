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

        [HttpGet]
        [Route("Alltrainer")]
        public async Task<List<TrainerRoleVM>> GetTrainer()
        {
            var getTrainer = await _context.UserRole.Include("Role").Include("User").Include(x => x.User.Employee)
                            .Where(x => x.User.Employee.isDelete == false && x.Role.Name == "Trainer")
                            .ToListAsync();
            if (getTrainer.Count == 0)
            {
                return null;
            }
            List<TrainerRoleVM> list = new List<TrainerRoleVM>();
            foreach (var item in getTrainer)
            {
                var trainer = new TrainerRoleVM()
                {
                    EmpId = item.User.Employee.EmpId,
                    EmployeeName = item.User.Employee.Name,
                    RoleName = item.Role.Name,
                };
                list.Add(trainer);
            }
            return list;
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

}
