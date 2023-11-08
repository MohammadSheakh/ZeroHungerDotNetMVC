using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commerce_Web_Application.DTOs
{
    public class NGODTO
    {
        /*
         * ============status==============
            Active = 'Active',
            Inactive = 'Inactive',
            Suspended = 'Suspended',
            LocalSeller = 'Local Seller',
            VerifiedSeller = 'Verified Seller',
            PremiumSupportSeller = 'Premium Support Seller',
            BestSeller = 'Top Seller',
            TopSeller = 'Top Seller',
         */
        public int ngoId { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string address { get; set; }
        public string phoneNo { get; set; }
        public string image { get; set; }
    }
}