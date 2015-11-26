using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WorldRef.Models.PromotionLibraryModels
{
    public class PromotionLibraryDAO
    {
        public int PromotionLibraryID { get; set; }
        public string PName { get; set; }
        public string PEmailId { get; set; }
        public string PContactNumber { get; set; }
        public string PReference { get; set; }
        public string PCompanyProfile { get; set; }
        public string PCorporatePresentation { get; set; }
        public string PURFFormat { get; set; }
        public int IndustriesID { get; set; }
        public string IndustriesName { get; set; }
        public int PromotionProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductBrochure { get; set; }
        // public List<SelectListItem> IndustList { get; set; } 
        public int CertificationID { get; set; }
        public string CertificateName { get; set; }
        public string CertificateAttachment { get; set; }

        public int POtherDocumentID { get; set; }
        public string OtherDocumentName { get; set; }
        public string DocumentAttachment { get; set; }
        public string ReasonForDisapproved { get; set; }

        public string AllData { get; set; }
        public string AllData1 { get; set; }
        public string AllData2 { get; set; }

        public string USerNo { get; set; }
        public int Id { get; set; }

        public string CountryName { get; set; }
        public string UserFirstName { get; set; }

        public List<SelectListItem> IndustriesListModel { get; set; }
        public class SelectListItem
        {
            public string Text { get; set; }
            public int Value { get; set; }
        }


        public List<PromotionLibraryDAO> details { get; set; }




        public string Text { get; set; }
        public int Value { get; set; }
    }
}