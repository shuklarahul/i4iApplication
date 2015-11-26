using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorldRef.Models.PromotionLibraryModels
{
    public class PromotionProductListDAO
    {
        public int PromotionProductID { get; set; }
        public int PromotionLibraryID { get; set; }
        public string ProductName { get; set; }
        public string ProductBrochure { get; set; }
        public string ProductIndustry { get; set; }
        public string URSFormat { get; set; }
        public List<PromotionProductListDAO> _Productlist { get; set; }
        public List<PromotionOtherDocument> documentlist { get; set; }
        public List<PromotionCertificate> certificatelist { get; set; }
        public List<PromotionIndustryDAO> cindustrylist { get; set; }
        public List<PromotionLibraryDAO> objlistpromotionlib { get; set; }
        public List<PromotionLibraryDAO> worlrefUserList { get; set; }

    }
}