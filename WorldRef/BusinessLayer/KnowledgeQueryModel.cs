using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorldRef.BusinessLayer
{
    public class KnowledgeQueryModelNew
    {
        public int Id { get; set; }
        public int IndustryId { get; set; }
        public int IndustryCatId { get; set; }
        public int IndustrySubId { get; set; }
        public string IndustryCategoryName { get; set; }
        public string IndustrySubCatName { get; set; }
        public string query { get; set; }
    }
}