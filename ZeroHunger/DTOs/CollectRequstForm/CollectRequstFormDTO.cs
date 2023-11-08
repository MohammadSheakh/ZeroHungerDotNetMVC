using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }
}