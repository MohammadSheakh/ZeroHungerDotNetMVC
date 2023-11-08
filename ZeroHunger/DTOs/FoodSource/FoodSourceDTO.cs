using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeroHunger.DTOs.FoodSource
{
    public class FoodSourceDTO
    {
        public int sourceId { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string address { get; set; }
        public string phoneNo { get; set; }
        public string image { get; set; }
        public string sourceType { get; set; }
    }
}