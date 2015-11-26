using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace WorldRef.Models
{
    public class KnowledgeQueryModel
    {
        public int Id { get; set; }
        public int IndustryID { get; set; }
        public string Industry { get; set; }
        public string Query { get; set; }
        public List<SelectListItem> IndustryList { get; set; }
        public string IndustryCategoryId { get; set; }
        public List<SelectListItem> IndustryCatList { get; set; }
        public string IndustrySubCatId { get; set; }
        public List<SelectListItem> IndustrySubCatList { get; set; }
        public string IndustryName { get; set; }
        public string IndustryCatName { get; set; }
        public string IndustrySubCatName { get; set; }
        public string QueryAnswer { get; set; }
        public bool Relevant { get; set; }
        public int QueryId { get; set; }
        public int UserId { get; set; }
        public DateTime? Createdon { get; set; }
        public List<KnowledgeAnswerModel> Answers { get; set; }
   
    }

    public class KnowledgeAnswerModel
    {
        public string KnowledgeAnswer { get; set; }
        public string TrainerName { get; set; }
        public string ImagePath { get; set; }
    }

}