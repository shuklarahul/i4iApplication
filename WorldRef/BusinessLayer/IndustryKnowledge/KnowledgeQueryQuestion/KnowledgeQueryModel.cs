using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorldRef.BusinessLayer.IndustryKnowledge.KnowledgeQueryQuestion
{
    public class KnowledgeQueryModel
    {
        public int Id { get; set; }
        public int IndustryId { get; set; }
        public int IndustryCatId { get; set; }
        public int IndustrySubCatId { get; set; }
        public string Query { get; set; }
    }
}