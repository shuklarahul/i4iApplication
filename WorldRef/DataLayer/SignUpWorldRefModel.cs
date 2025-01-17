﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorldRef.DataLayer
{
    public class SignUpWorldRefModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string UserNo { get; set; }
        public string RecruitmentType { get; set; }
        public string ContactNumber { get; set; }
        public string ContactCode { get; set; }
        public List<SelectListItem> TypeList { get; set; }
        public List<SelectListItem> RecruitersTypeList { get; set; }
        public string Email { get; set; }
        public string Industry { get; set; }
        public string Country { get; set; }
        public string ProfileFileName { get; set; }
        public string ProfilePath { get; set; }
        public string OtherType { get; set; }
        // It will save in the Product 
        public string CompanyLogo { get; set; }
        public string PhotoAttach { get; set; }
        // It will save in the region
        public string AlternateEmail { get; set; }
        public string UserRole { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Upload)]
        HttpPostedFileBase ImageUpload { get; set; }
        public List<SelectListItem> CountryList { get; set; }
        public List<SelectListItem> IndustryList { get; set; }
        public string OtherIndustryName { get; set; }
        public int IndustriesID { get; set; }
        public string ProfileUrl { get; set; }
        public string OrgName { get; set; }
        public bool RecruiterFlag { get; set; }
        public string ParentOrganisationName { get; set; }
        public string BusinessUnitName { get; set; }
        public string UploaderType { get; set; }
        public string OrganisationName { get; set; }
        public string BussinessUnitName { get; set; }
        public string MyCompany { get; set; }
        public string RecoveryMail { get; set; }
        public string OtherMail { get; set; }
        public string OfficialNumber { get; set; }
        public string Language { get; set; }
        public string BusinessInterestCountry { get; set; }
        public List<SelectListItem> BusinessInterestCountryList { get; set; }
        public List<SelectListItem> LanguageList { get; set; }
    }
}