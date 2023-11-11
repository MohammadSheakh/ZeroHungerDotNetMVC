using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZeroHunger.DTOs.FoodSource;
using ZeroHunger.EF;

namespace ZeroHunger.DTOs.CollectRequstForm
{
    public class CollectRequstFormDTO
    {
        public int reqFormId { get; set; }
        public string requestStatus { get; set; }
        public Nullable<System.DateTime> createdAt { get; set; }
        public Nullable<int> employeeId { get; set; }
        public Nullable<int> ngoId { get; set; }
        public Nullable<int> foodSourceId { get; set; }
        public virtual EF.FoodSource FoodSource { get; set; }// after removing virtual // ekhane age FoodSourceDTO chilo 
        public virtual NGO NGO { get; set; } // after removing virtual
    }
}