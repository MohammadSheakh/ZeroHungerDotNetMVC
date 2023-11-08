using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeroHunger.DTOs.Employee
{
    public class EmployeeDTO
    {
        public int empId { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string phoneNo { get; set; }
        public Nullable<int> ngoId { get; set; }
        public string image { get; set; }
        public Nullable<System.DateTime> createdAt { get; set; }
    }
}