//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZeroHunger.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class Employee
    {
        public int empId { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string phoneNo { get; set; }
        public Nullable<int> ngoId { get; set; }
        public string image { get; set; }
        public Nullable<System.DateTime> createdAt { get; set; }
    
        public virtual NGO NGO { get; set; }
    }
}