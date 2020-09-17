using API.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class RoleVM 
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreateData { get; set; }
        public DateTimeOffset UpdateDate { get; set; }
        public DateTimeOffset DeleteData { get; set; }
        public bool isDelete { get; set; }
    }

    public class UserVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NIK { get; set; }
        public string Site { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string RoleID { get; set; }
        public string RoleName { get; set; }
        public string VerifyCode { get; set; }
    }
}
