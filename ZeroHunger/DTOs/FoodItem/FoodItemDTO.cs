using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeroHunger.DTOs.FoodItem
{
    public class FoodItemDTO
    {
        public int foodItemId { get; set; }
        public string name { get; set; }
        public Nullable<int> quantity { get; set; }
        public string quantityUnit { get; set; }
        public Nullable<System.DateTime> maxPreservationTime { get; set; }
        public Nullable<int> sourceId { get; set; }

    }
}