using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("TB_Trans_Log")]
    public class LogActivity
    {
        [Key]
        public int Id { get; set; }
        public string Response { get; set; }
        public string Email { get; set; }

        public DateTimeOffset CreateDate { get; set; }

    }
}
