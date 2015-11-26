using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorldRef.Models
{
    public class Customer_DetailsDAO
    {
        public int CustomerDetailsID { get; set; }
        public Nullable<int> BookTrainerID { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string ContactNumber { get; set; }
        public string Country { get; set; }
        public string Organisation { get; set; }
    }
}