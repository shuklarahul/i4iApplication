using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorldRef.Models.PromotionLibraryModels
{
    public class PromotionIndustryDAO
    {
        public int PromotionIndustriesID { get; set; }
        public Nullable<int> PromotionLibraryID { get; set; }
        public Nullable<int> IndustriesID { get; set; }
        public string IndustriesName { get; set; }
    }
}