using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Base
{
    public interface BaseModel
    {
        int Id { get; set; }
        DateTimeOffset CreateData { get; set; }
        DateTimeOffset UpdateDate { get; set; }
        DateTimeOffset DeleteData { get; set; }
        bool isDelete { get; set; }
    }
}
