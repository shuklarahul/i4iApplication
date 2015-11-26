using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorldRef.BusinessLayer;
using WorldRef.DataLayer;
using WorldRef.Models;

namespace WorldRef.Controllers
{
    public class UploaderController : Controller
    {
        public I4IDBEntities context = new I4IDBEntities();
        //
        // GET: /Uploader/
        public ActionResult UploadExcel()
        {
            SignUpWorldRefModel signUpModel = new SignUpWorldRefModel();
            signUpModel = GetAllSignUpDetails();
            return View(signUpModel);
        }

        [HttpPost]
        public ActionResult UploadExcel(FormCollection formCollection)
        {
            WorldRefExcelDataModel worldExcelModel = new WorldRefExcelDataModel();
            string retStatus = "";
            try
            {
                worldExcelModel.IsOrganization = formCollection["Organization"] == null ? false : true;
                worldExcelModel.IsCustomer = formCollection["Customer"] == null ? false : true;
                worldExcelModel.IsProject = formCollection["Project"] == null ? false : true;
                worldExcelModel.IsType = formCollection["Type"] == null ? false : true;
                worldExcelModel.IsYear = formCollection["Year"] == null ? false : true;
                worldExcelModel.IsStatus = formCollection["Status"] == null ? false : true;
                //   worldExcelModel.IsCountry = formCollection["Country"] == null ? false : true;
                // worldExcelModel.IsEmail = formCollection["Email"] == null ? false : true;
                worldExcelModel.userid = Convert.ToInt32(Request.Cookies["UserId"].Value);

                ReadExcel readExcel = new ReadExcel();

                string path = string.Empty;
                string filename = string.Empty;
                string SavePath = string.Empty;
                string Extension = string.Empty;
                foreach (string upload in Request.Files)
                {
                    SavePath = Guid.NewGuid().ToString();
                    path = AppDomain.CurrentDomain.BaseDirectory + "uploads/";
                    filename = Path.GetFileName(Request.Files[upload].FileName);

                    Extension = Path.GetExtension(Request.Files[upload].FileName);

                    if (Extension.ToLower() != ".xls" && Extension.ToLower() != ".xlsx")
                    {
                        TempData.Clear();
                        TempData.Add("ErrorMessage", "Please Upload Correct format.Please Download the Format");
                        return RedirectToAction("UploadExcel");
                    }

                    Request.Files[upload].SaveAs(Path.Combine(path, SavePath + Extension));
                }
                readExcel.ExcelPath = Path.Combine(path, SavePath + Extension);
                worldExcelModel.ExcelPath = Path.Combine(SavePath + Extension).ToString();
                worldExcelModel.ExcelName = filename;
                retStatus = readExcel.ReadDataFromExcel();
                readExcel.AddData(worldExcelModel);

                TempData.Clear();
                TempData.Add("ErrorMessage", "Thank you for taking a step forward to glorifying yourself in WorlRef Map");
            }
            catch (Exception ex)
            {
                TempData.Clear();
                TempData.Add("ErrorMessage", ex.Message.ToString());//"Please Upload Correct format.Please Download the Format");
                return RedirectToAction("UploadExcel");
            }
            return RedirectToAction("UploadExcel");
        }

        public FileResult DownLoadAttachment()
        {
            string FullFilePath = AppDomain.CurrentDomain.BaseDirectory + "uploads/Reference List Format.xlsx";
            string contentType = "application/pdf";
            return File(FullFilePath, contentType, "Reference List Format.xlsx");
        }


        public FileResult DownLoadGuidlines()
        {
            string FullFilePath = AppDomain.CurrentDomain.BaseDirectory + "uploads/Guidelines.docx";
            string contentType = "application/pdf";
            return File(FullFilePath, contentType, "Guidelines.docx");
        }


        #region All SignUp Details
        private SignUpWorldRefModel GetAllSignUpDetails()
        {
            SignUpWorldRefModel signUpModel = new SignUpWorldRefModel();
            List<SelectListItem> selectListCountry = new List<SelectListItem>();
            List<SelectListItem> selectList = new List<SelectListItem>();//Developer, Investor, Fabricator, Procurement Staff, Others
            //selectList.Add(new SelectListItem() { Text = "I am", Value = "0" });
            selectList.Add(new SelectListItem() { Text = "Manufacturer", Value = "Manufacturer" });
            selectList.Add(new SelectListItem() { Text = "Contractor", Value = "Contractor" });
            selectList.Add(new SelectListItem() { Text = "Consultant", Value = "Consultant" });
            selectList.Add(new SelectListItem() { Text = "Trading Company", Value = "Trading Company" });
            selectList.Add(new SelectListItem() { Text = "Project Owner’s/Procurement Staff", Value = "Project Owner’s/Procurement Staff" });
            selectList.Add(new SelectListItem() { Text = "Developer", Value = "Developer" });
            selectList.Add(new SelectListItem() { Text = "Investor", Value = "Investor" });
            selectList.Add(new SelectListItem() { Text = "Fabricator", Value = "Fabricator" });
            selectList.Add(new SelectListItem() { Text = "Raw Material Supplier", Value = "Raw Material Supplier" });
            selectList.Add(new SelectListItem() { Text = "Others", Value = "Others" });
            signUpModel.TypeList = selectList;

            var content = (from p in context.Countries
                           select new { p.CountryID, p.CountryName }).AsEnumerable();

            selectListCountry.Add(new SelectListItem() { Text = "Select Country", Value = "0" });

            foreach (var item in content)
            {
                selectListCountry.Add(new SelectListItem() { Text = item.CountryName, Value = item.CountryName });

            }
            signUpModel.CountryList = selectListCountry;
            KnowledgeLogic knowIndustry = new KnowledgeLogic();
            signUpModel.IndustryList = knowIndustry.GetIndustry();
            List<SelectListItem> selectList1 = new List<SelectListItem>();
            selectList1.Add(new SelectListItem() { Text = "Type of Recruitment", Value = "0" });
            selectList1.Add(new SelectListItem() { Text = "I recruit for my Organisation only", Value = "I recruit for my Organisation only" });
            selectList1.Add(new SelectListItem() { Text = "I recruit for Other Organisations only", Value = "I recruit for Other Organisations only" });
            selectList1.Add(new SelectListItem() { Text = "Both: I recruit for my and other organisations", Value = "Both: I recruit for my and other organisations" });
            signUpModel.RecruitersTypeList = selectList1;
            return signUpModel;
        }
        #endregion
	}
}