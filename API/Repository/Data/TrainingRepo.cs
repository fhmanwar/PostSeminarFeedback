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
            List<TrainingVM> list = new List<TrainingVM>();
            var data = await _context.Trainings.Include("User").Include("Type").Where(x => x.isDelete == false).ToListAsync();
            if (data.Count == 0)
            {
                return null;
            }
            return data;
        }
        public override async Task<Training> GetID(int Id)
        {
            var data = await _context.Trainings.Include("User").Include("Type").SingleOrDefaultAsync(x => x.Id == Id && x.isDelete == false);
            if (data != null)
            {
                return data;
            }
            return null;
        }
    }
}
