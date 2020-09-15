using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class TrainingVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Target { get; set; }
        public string Location { get; set; }
        public DateTimeOffset Schedule { get; set; }
        public string TypeID { get; set; }
        public string TypeName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string RoleID { get; set; }
        public string RoleName { get; set; }
    }
}
