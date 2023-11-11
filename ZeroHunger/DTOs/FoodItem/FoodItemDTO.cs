using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ZeroHunger.EF;

namespace ZeroHunger.DTOs.FoodItem
{
    public class FoodItemDTO
    {
        public int foodItemId { get; set; }
        [Required]
        [MinLength(3)]
        public string name { get; set; }
        [Required]
        
        public Nullable<int> quantity { get; set; }
        [Required]
        public string quantityUnit { get; set; }
        [Required]
        public Nullable<System.DateTime> maxPreservationTime { get; set; }
        public Nullable<int> sourceId { get; set; }
        public virtual EF.FoodSource FoodSource { get; set; }
    }
}