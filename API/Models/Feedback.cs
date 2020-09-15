using API.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("TB_Trans_Feedback")]
    public class Feedback : BaseModel
    {
        [Key]
        public int Id { get; set; }
        public string Review { get; set; }
        public float Rate { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("Question")]
        public int QuestionId { get; set; }

        public DateTimeOffset CreateData { get; set; }
        public DateTimeOffset UpdateDate { get; set; }
        public DateTimeOffset DeleteData { get; set; }
        public bool isDelete { get; set; }

        public User User { get; set; }
        public Question Question { get; set; }

    }
}
