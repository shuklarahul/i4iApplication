using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorldRef.Models
{
    public class Trainer_profileDAO
    {
        public int TrainerProfileID { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string UserNo { get; set; }
        public string TrainerImage { get; set; }
        public string Iam { get; set; }
        public Nullable<int> Age { get; set; }
        public string Country { get; set; }
        public string LocatedAt { get; set; }
        public string SpeakLanguages { get; set; }
        public string IntrestedIndustries { get; set; }
        public Nullable<bool> AnswerQuery { get; set; }
        public Nullable<bool> HelthIssue { get; set; }
        public string PreferredCountry { get; set; }
        public Nullable<bool> Passport { get; set; }
        public string Keyword { get; set; }
        public string IndustriesName { get; set; }
        public string IndustriesID { get; set; }
        public string CountryID { get; set; }
        public string LanguageID { get; set; }
        public Nullable<bool> FluentEnglish { get; set; }
        public string LanguageName { get; set; } 
        public string CountryName { get; set; }
        public string Organisation { get; set; }
        public string Designation { get; set; } 
        public Nullable<bool> IsActive { get; set; }
        public string CompleteName { get; set; }
        public string PassportNumber { get; set; }
        public string IssuePlace { get; set; }
        public string IssueDate { get; set; }
        public string ExpiryDate { get; set; }
        public string Citizenship { get; set; }
        public string Topic { get; set; }
        public string TimeDuration { get; set; }
        public string TimeDurationID { get; set; }
        public string Price { get; set; }
        public string Currency { get; set; }
        public string CurrencyID { get; set; }
        public string[] hCount { get; set; }
        public string[] topichidden { get; set; }
        public string AllData { get; set; }
        public List<Trainer_profileDAO> IamList { get; set; }
        public List<SelectListItem> LanguagesListModel { get; set; }
        public List<SelectListItem> CountryListModel { get; set; }
        public List<SelectListItem> PreferredCountryListModel { get; set; }
        public string PreferredCountryID { get; set; }
        public string PreferredCountryName { get; set; }
        public List<SelectListItem> CountryPreferredListModel { get; set; }
        public List<SelectListItem> currencyListModel { get; set; }
        public List<SelectListItem> timeDurationListModel { get; set; }
         public List<SelectListItem> IndustriesListModel { get; set; }
        
       
     
          public class SelectListItem
        {
            public string Text { get; set; }
            public int Value { get; set; }
        }
        public string Text { get; set; }
        public int Value { get; set; }
        public string Name  { get; set; }
        public string hiddenname { get; set; }
        public List<Trainer_profileDAO> _listselect { get; set; }      
        public List<string> LanguageList { get; set; }
        public List<string> IndustriesList { get; set; }
        public List<string> countryList { get; set; }
        public List<string> timeDuration{ get; set; }
        public List<string> currency { get; set; }

        public List<string> TopicList { get; set; }


        public List<Trainer_profileDAO> IamSelect()
        {
            List<Trainer_profileDAO> list = new List<Trainer_profileDAO>();

            list.Add(new Trainer_profileDAO() { Text = "-Select-", Name = ""});

            list.Add(new Trainer_profileDAO() { Text = "Freelance Trainer", Name = "Freelance Trainer" });

            list.Add(new Trainer_profileDAO() { Text = "Engineer", Name = "Engineer" });
            list.Add(new Trainer_profileDAO() { Text = "Professor", Name = "Professor" });
            list.Add(new Trainer_profileDAO() { Text = "Employed", Name = "Employed"});
          
            

            return list;
        }
        public string[] topics { get; set; }


        public List<Trainer_profileDAO> IndustriesDataTrainer { get; set; }

        
    }


    
    
}