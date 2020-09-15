using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class QuestionVM
    {
        public int Id { get; set; }
        public string QuestionDesc { get; set; }
        public int TrainingId { get; set; }
    }
}
