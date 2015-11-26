using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorldRef.Models
{
    public class TrainerProfileDAO
    {
         public int Id { get; set; }
        public string UserNo { get; set; }
        public string DocumentType { get; set; }
        public string Name { get; set; }
        public string IndustriesName { get; set; }
        public string CountryName { get; set; }
        public int CountryID { get; set; }
        public string Industry { get; set; }
        public string Topic { get; set; }
        public string Time { get; set; }
        public string Charges { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }       
        public string SampleAttach { get; set; }       
        public string ContactNo { get; set; } 
        public Nullable<System.DateTime> Date { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string phone { get; set; }
        public string UserRole { get; set; }
        public string Company { get; set; }
        public string Password { get; set; }
        public string ConfPassword { get; set; }
        public string ProfileAttach { get; set; }
        public string PhotoAttach { get; set; }     
        public int SampleId { get; set; }     
        public bool Checkbox { get; set; }
        public string DateStr { get; set; }
        public string SampleAttachPath { get; set; }
        public string ProfileAttachPath { get; set; }

        public string IndustriesID { get; set; }
      
        public string PhotoAttachPath { get; set; }
        public List<SelectListItem> CountryListModel { get; set; }
        public List<SelectListItem> IndustriesListModel { get; set; }


        public List<TrainerProfileDAO> DocumentTypeList { get; set; }

        public List<TrainerProfileDAO> DocumentTypes()
        {
            List<TrainerProfileDAO> list = new List<TrainerProfileDAO>();

            list.Add(new TrainerProfileDAO() { Text = "Select Document Type", Value = "Select Document Type" });

            list.Add(new TrainerProfileDAO() { Text = "Sample Document", Value = "Sample Document" });

            list.Add(new TrainerProfileDAO() { Text = "Detailed Document", Value = "Detailed Document" });

          
            return list;
        }

        public class SelectListItem
        {
            public string Text { get; set; }
            public int Value { get; set; }
        }
        public string Text { get; set; }
        public string Value { get; set; }
        public string hiddenname { get; set; }
    }
}