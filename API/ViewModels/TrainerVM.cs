using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class TrainerVM
    {
        public string EmpId { get; set; }
        public string EmployeeName { get; set; }
        public int TrainingId { get; set; }
        public string TrainingTitle { get; set; }
        public int QuestionId { get; set; }
        public string QuestionDesc { get; set; }
    }

    public class TrainerFeedbackVM
    {
        public int Id { get; set; }        
        public string EmpId { get; set; }
        public string EmployeeName { get; set; }
        public int TrainingId { get; set; }
        public string TrainingTitle { get; set; }
        public string Trainer { get; set; }
        public int QuestionId { get; set; }
        public string QuestionDesc { get; set; }
        public string Review { get; set; }
        public double Rate { get; set; }
    }

    public class TrainerRoleVM
    {
        public string EmpId { get; set; }
        public string EmployeeName { get; set; }
        public string RoleName { get; set; }
    }
}
