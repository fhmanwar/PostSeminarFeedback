using API.Context;
using API.Models;
using API.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class TypeTrainingRepo : BaseRepo<TypeTraining, MyContext>
    {
        public TypeTrainingRepo(MyContext context) : base(context)
        {

        }
    }

    public class TrainingRepo : BaseRepo<Training, MyContext>
    {
        readonly MyContext _context;
        IConfiguration _configuration;
        public TrainingRepo(MyContext context, IConfiguration config) : base(context)
        {
            _context = context;
            _configuration = config;
        }

        public override async Task<List<Training>> GetAll()
        {
            var data = await _context.Trainings.Include("Employee").Include("Type").Where(x => x.isDelete == false).ToListAsync();
            if (data.Count == 0 || data == null)
            {
                return null;
            }
            return data;
        }
        public override async Task<Training> GetID(int Id)
        {
            var data = await _context.Trainings.Include("Employee").Include("Type").SingleOrDefaultAsync(x => x.Id == Id && x.isDelete == false);
            if (data != null)
            {
                return data;
            }
            return null;
        }
    }

    public class QuestionRepo : BaseRepo<Question, MyContext>
    {
        readonly MyContext _context;
        IConfiguration _configuration;
        public QuestionRepo(MyContext context, IConfiguration config) : base(context)
        {
            _context = context;
            _configuration = config;
        }

        public override async Task<List<Question>> GetAll()
        {
            var data = await _context.Questions.Include("Training").Include(x => x.Training.Employee).Where(x => x.isDelete == false).ToListAsync();
            if (data.Count == 0)
            {
                return null;
            }
            return data;
        }
        public override async Task<Question> GetID(int Id)
        {
            var data = await _context.Questions.Include("Training").Include(x => x.Training.Employee).SingleOrDefaultAsync(x => x.Id == Id && x.isDelete == false);
            if (data != null)
            {
                return data;
            }
            return null;
        }
    }

    public class FeedbackRepo : BaseRepo<Feedback, MyContext>
    {
        readonly MyContext _context;
        IConfiguration _configuration;
        public FeedbackRepo(MyContext context, IConfiguration config) : base(context)
        {
            _context = context;
            _configuration = config;
        }

        public override async Task<List<Feedback>> GetAll()
        {
            var data = await _context.Feedbacks.Include("Question").Include(x => x.Question.Training.Employee).Include("Employee").Where(x => x.isDelete == false).ToListAsync();
            if (data.Count == 0)
            {
                return null;
            }
            return data;
        }
        public override async Task<Feedback> GetID(int Id)
        {
            var data = await _context.Feedbacks.Include("Question").Include(x => x.Question.Training.Employee).Include("Employee").SingleOrDefaultAsync(x => x.Id == Id && x.isDelete == false);
            if (data != null)
            {
                return data;
            }
            return null;
        }
    }
}
