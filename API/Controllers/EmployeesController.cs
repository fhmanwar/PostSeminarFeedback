using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        //[HttpGet("trainfeedback/{id}")]
        //public async Task<List<TrainerFeedbackVM>> GetTrainerfeedback(string id)
        //{
        //    var getfeed = await _context.Feedbacks.Include("Employee").Include("Question").Include(x => x.Question.Training).Include(x => x.Question.Training.Employee).Where(x => x.isDelete == false && x.Question.Training.Employee.EmpId == id).ToListAsync();
        //    if (getfeed.Count == 0)
        //    {
        //        return null;
        //    }
        //    List<TrainerFeedbackVM> list = new List<TrainerFeedbackVM>();
        //    foreach (var item in getfeed)
        //    {
        //        var trainer = new TrainerFeedbackVM()
        //        {
        //            Id = item.Id,
        //            Review = item.Review,
        //            Rate = item.Rate,
        //            EmpId = item.Employee.EmpId,
        //            EmployeeName = item.Employee.Name,
        //            TrainingId = item.Question.Training.Id,
        //            TrainingTitle = item.Question.Training.Title,
        //            Trainer = item.Question.Training.Employee.Name,
        //            QuestionId = item.Question.Id,
        //            QuestionDesc = item.Question.QuestionDesc,
        //        };
        //        list.Add(trainer);
        //    }
        //    return list;
        //}
    }
}
