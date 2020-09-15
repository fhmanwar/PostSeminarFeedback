using API.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("TB_M_Training")]
    public class Training : BaseModel
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Target { get; set; }
        public string Location { get; set; }
        public DateTimeOffset Schedule { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        [ForeignKey("Type")]
        public int TypeId { get; set; }

        public DateTimeOffset CreateData { get; set; }
        public DateTimeOffset UpdateDate { get; set; }
        public DateTimeOffset DeleteData { get; set; }
        public bool isDelete { get; set; }

        public User User { get; set; }
        public TypeTraining Type { get; set; }

    }
}
