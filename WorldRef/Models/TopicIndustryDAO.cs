using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorldRef.Models
{
    public class TopicIndustryDAO
    {
        public int IndustryTopicID { get; set; }
        public int Id { get; set; }
        public int IndustriesID { get; set; }
        public string Topic { get; set; }
        public int CurrencyID { get; set; }
        public decimal Price { get; set; }
        public int TimeDurationID { get; set; }
        public string TimeDuration { get; set; }
    }
}