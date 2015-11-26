using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorldRef.Models
{
    public class CurrentYearDAO
    {
        public int CurrentYearID { get; set; }
        public string CurrentYear1 { get; set; }
        public string ProjectName { get; set; }
        public string CustomerName { get; set; }
        public string ProjectStatus { get; set; }
        public string PaymentStatus { get; set; }

        public List<SelectListItem> CurrentYearProject { get; set; }
        public class SelectListItem
        {
            public string Text { get; set; }
            public int Value { get; set; }
        }
        public string Text { get; set; }
        public int Value { get; set; }
    }
}