using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WorldRef.Models
{
    public class AssociateProjectDAO
    {
        public int Id { get; set; }
        public int AssociateProjectID { get; set; }
        public string ContactNumber { get; set; }
        public string CustomerName { get; set; }
        public string ProjectName { get; set; }
        public string Industry { get; set; }
        public string ProjectInquiryType { get; set; }
        public Nullable<decimal> SalesMarginExpectation { get; set; }
        public string CustomerType { get; set; }
        public string ProjectInquiryStatus { get; set; }
        public string ImportantDate { get; set; }
        public string InquirySubmissionDate { get; set; }
        public string OfferSubmissionDate { get; set; }
        public string LetterIntentReleaseDate { get; set; }
        public string ContractSigningDate { get; set; }
        public string FirstPaymentdate { get; set; }
        public string CompletePaymentdate { get; set; }
        public string ContractExecutionDate { get; set; }
        public string CollaborateWithUs { get; set; }
        public string AssociateWithi4i { get; set; }
        public string Description { get; set; }
        public string Attachment { get; set; }
        public string UserNo { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ReasonForDisapprove { get; set; }
        public string Status { get; set; }
        public Nullable<bool> IsActive { get; set; }     
        public Nullable<System.DateTime> TechnicalDiscussionDate { get; set; }
        public Nullable<System.DateTime> ReleaseLetterIntentDate { get; set; }
        public Nullable<System.DateTime> ContractNegotiationDate { get; set; }                   
        public string Email { get; set; }
        public string Capacity { get; set; }
        public string OfferDocument { get; set; }
        public string OfferSubmissionDateStr { get; set; }
        public string TechnicalDiscussionDateStr { get; set; }
        public string ReleaseLetterIntentDateStr { get; set; }
        public string ContractNegotiationDateStr { get; set; }
        public string ContractSigningDateStr { get; set; }
        public string AttachmentStr { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string CurrentYear { get; set; }
        public List<AssociateProject> abc { get; set; }
        public string TimeDifference { get; set; }       
        public List<SelectListItem> IndustryList { get; set; }
        public List<AssociateProjectDAO> ProjectInquiryTypeList { get; set; }
        public List<AssociateProjectDAO> GetCustomerTypeList { get; set; }
        public List<AssociateProjectDAO> GetprojectInquiryStatusList { get; set; }
        public List<AssociateProjectDAO> CollaborateTypesList { get; set; }
        public List<AssociateProjectDAO> Associatewithi4iList { get; set; }
        public string IndustriesID { get; set; }       
        public List<AssociateProjectDAO> ProjectInquiryTypes()
        {
            List<AssociateProjectDAO> list = new List<AssociateProjectDAO>();

            list.Add(new AssociateProjectDAO() { Name = "-Select-", Value = "" });

            list.Add(new AssociateProjectDAO() { Name = "Needs Design Engineering & Consulting", Value = "Engineering & Consulting" });

            list.Add(new AssociateProjectDAO() { Name = "Needs Plant/Machinery/Equipment Supply", Value = "Needs Plant/Machinery/Equipment Supply" });

            list.Add(new AssociateProjectDAO() { Name = "Needs EPC/Civil Work/local Contractor", Value = "Needs EPC/Civil Work/local Contractor" });

            list.Add(new AssociateProjectDAO() { Name = "Needs Investment", Value = "Needs Investment" });
            //list.Add(new AssociateProjectDAO() { Name = "Needs Design", Value = "Needs Design" });
            list.Add(new AssociateProjectDAO() { Name = "Needs Local Supply/Services", Value = "Needs Local Supply/Services" });
            list.Add(new AssociateProjectDAO() { Name = "Needs Industrial Raw Material", Value = "Miscellaneous Needs" });
            list.Add(new AssociateProjectDAO() { Name = "Miscellaneous Needs", Value = "Miscellaneous Needs" });
            return list;
        }
        public List<AssociateProjectDAO> GetCustomerType()
        {
            List<AssociateProjectDAO> list = new List<AssociateProjectDAO>();

            list.Add(new AssociateProjectDAO() { Name = "-Select-", Value = "" });

            list.Add(new AssociateProjectDAO() { Name = "High Quality Sensitive", Value = "High Quality Sensitive" });

            list.Add(new AssociateProjectDAO() { Name = "Needs Value for Money", Value = "Needs Value for Money" });

            list.Add(new AssociateProjectDAO() { Name = "High Price Sensitive", Value = "High Price Sensitive" });
            return list;
        }       
        public List<AssociateProjectDAO> CollaborateTypes()
        {
            List<AssociateProjectDAO> list = new List<AssociateProjectDAO>();

            list.Add(new AssociateProjectDAO() { Name = "-Select-", Value = "" });

            list.Add(new AssociateProjectDAO() { Name = "Yes", Value = "Yes" });

            list.Add(new AssociateProjectDAO() { Name = "No", Value = "No" });
           
            return list;
        }
        public List<AssociateProjectDAO> AssociateWithi4is()
        {
            List<AssociateProjectDAO> list = new List<AssociateProjectDAO>();

            list.Add(new AssociateProjectDAO() { Name = "-Select-", Value = "" });

            list.Add(new AssociateProjectDAO() { Name = "Yes", Value = "Yes" });

            list.Add(new AssociateProjectDAO() { Name = "No", Value = "No" });

            return list;
        }
        public List<AssociateProjectDAO> GetProjectInquiryStatus()
        {
            List<AssociateProjectDAO> list = new List<AssociateProjectDAO>();

            list.Add(new AssociateProjectDAO() { Name = "-Select-", Value = "" });
            list.Add(new AssociateProjectDAO() { Name = "Tender Open", Value = "Tender Open" });
            list.Add(new AssociateProjectDAO() { Name = "Pre-feasibility Study Finished", Value = "Pre-feasibility Study Finished" });
            list.Add(new AssociateProjectDAO() { Name = "feasibility Study Finished", Value = "feasibility Study Finished" });
            list.Add(new AssociateProjectDAO() { Name = "Basic Enginnering Completed", Value = "Basic Enginnering Completed" });
            list.Add(new AssociateProjectDAO() { Name = "Detailed Enginnering Completed", Value = "Detailed Enginnering Completed" });
            list.Add(new AssociateProjectDAO() { Name = "Financially Closed", Value = "Financially Closed" });
            list.Add(new AssociateProjectDAO() { Name = "Financially Closure in Process", Value = "Financially Closure in Process" });
            list.Add(new AssociateProjectDAO() { Name = "Project Development stage", Value = "Project Development stage" });
            list.Add(new AssociateProjectDAO() { Name = "To be finalised within 1 month", Value = "To be finalised within 1 month" });
            list.Add(new AssociateProjectDAO() { Name = "To be finalised within 3 months", Value = "To be finalised within 3 months" });
            list.Add(new AssociateProjectDAO() { Name = "To be finalised within 6 months", Value = "To be finalised within 6 months" });
            list.Add(new AssociateProjectDAO() { Name = "To be finalised this year", Value = "To be finalised this year" });
            list.Add(new AssociateProjectDAO() { Name = "To be finalised next year", Value = "To be finalised next year" });
            list.Add(new AssociateProjectDAO() { Name = "Offer not needed now", Value = "Offer not needed now" });
            list.Add(new AssociateProjectDAO() { Name = "Offer needed", Value = "Offer needed" });
            list.Add(new AssociateProjectDAO() { Name = "Under Development", Value = "Under Development" });
            list.Add(new AssociateProjectDAO() { Name = "offer submission but contract not yet signed", Value = "offer submission but contract not yet signed" });
            list.Add(new AssociateProjectDAO() { Name = "Signed Contracts", Value = "Signed Contracts" });
            list.Add(new AssociateProjectDAO() { Name = "Contract Executed", Value = "Contract Executed" });
            list.Add(new AssociateProjectDAO() { Name = "Under execution", Value = "Under execution" });
            list.Add(new AssociateProjectDAO() { Name = "Contract Signed with someone else", Value = "Contract Signed with someone else" });
            list.Add(new AssociateProjectDAO() { Name = "Non Disclosure Agreement signing needed", Value = "Non Disclosure Agreement signing needed" });
            list.Add(new AssociateProjectDAO() { Name = "Other", Value = "Other" });
            return list;
        }
        public string IndustriesName { get; set; }
        public bool MatchID { get; set; }       
        public List<SelectListItem> IndustriesListModel { get; set; }
        public List<ProjectContactDetail> projectDetail { get; set; }
        public class SelectListItem
        {
            public string Text { get; set; }
            public int Value { get; set; }
        }
        public string Text { get; set; }
    }
}
