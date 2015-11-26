using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorldRef.Models
{
    public class SupplierDAO
    {
        public long Id { get; set; }
        public string Company { get; set; }
        public string ContactPersonName { get; set; }
        public string phone { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string password { get; set; }
    }
}