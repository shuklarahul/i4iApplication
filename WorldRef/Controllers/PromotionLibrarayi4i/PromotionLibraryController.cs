using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorldRef.Models.PromotionLibraryModels;
using WorldRef.Models;
using WorldRef.BusinessLayer;


namespace WorldRef.Controllers.PromotionLibrarayi4i
{
    public class PromotionLibraryController : Controller
    {
        I4IDBEntities db = new I4IDBEntities();
        #region code for show promotion form

        public ActionResult PromotionLibraryForm()
        {

            List<PromotionLibraryDAO> listResult = new List<PromotionLibraryDAO>();
            PromotionLibraryDAO _objPl = new PromotionLibraryDAO();
            List<PromotionLibraryDAO.SelectListItem> Industrieslist = new List<PromotionLibraryDAO.SelectListItem>();
            var _industries = (from p in db.Industries
                               select new { p.IndustriesID, p.IndustriesName }).AsEnumerable();


            foreach (var item in _industries)
            {
                Industrieslist.Add(new PromotionLibraryDAO.SelectListItem() { Text = item.IndustriesName, Value = item.IndustriesID });

            }
            _objPl.IndustriesListModel = Industrieslist;
            string Uid = Request.Cookies["UserId"].Value;
            int uid = Convert.ToInt32(Uid);
            var uName =(from c in db.RegisterUsers where c.Id==uid select c.UserNo).FirstOrDefault();
           // HttpCookie userNameCookies = new HttpCookie("UserId");
            //userNameCookies.Value = "asad";
            if (!string.IsNullOrEmpty(uName))
            {

                var userdetails = (from t in db.RegisterUsers
                                   join proj in db.WorldRefExcelDataProjects on t.Id equals proj.userid
                                   where t.UserNo == uName
                                   select  new { t.Email, proj.CustomerName, t.Industries, t.phone, t.ProfileAttach }).Distinct().AsEnumerable();
                foreach (var project in userdetails)
                {
                    listResult.Add(new PromotionLibraryDAO()
                    {                        
                        PEmailId = project.Email,
                        PContactNumber = project.phone,
                        CertificateName = project.CustomerName,
                        IndustriesName = project.Industries,
                        PCompanyProfile = project.ProfileAttach

                    });
                }

                _objPl.details = listResult;

                _objPl.PEmailId = _objPl.details[0].PEmailId.ToString();
                _objPl.PName = _objPl.details[0].CertificateName.ToString();
                _objPl.PContactNumber = _objPl.details[0].PContactNumber.ToString();
                _objPl.PCompanyProfile = _objPl.details[0].PCompanyProfile.ToString();
              

            }
            return View(_objPl);
        }
        #endregion
        #region code for save promotion form in database
        public ActionResult SavePromotion(FormCollection fc, HttpPostedFileBase[] file, HttpPostedFileBase[] file1, HttpPostedFileBase[] file2, HttpPostedFileBase[] file3, string chkvalue)
        {

            PromotionLibrary _objPromotion = new PromotionLibrary();
            string result = fc["AllData"];
            string result1 = fc["AllData1"];
            string result2 = fc["AllData2"];
            _objPromotion.PName = fc["PName"];
            _objPromotion.PEmailId = fc["PEmailId"];
            _objPromotion.PContactNumber = fc["PContactNumber"];
            if (file[0] != null)
            {
                _objPromotion.PReference = file[0].FileName;
                file[0].SaveAs(Server.MapPath("~/Content/PromotionFile/" + _objPromotion.PReference));
            }
            if (file[1] != null)
            {
                _objPromotion.PCompanyProfile = file[1].FileName;
                file[1].SaveAs(Server.MapPath("~/Content/PromotionFile/" + _objPromotion.PCompanyProfile));
            }
            //   _objPromotion.IndustriesID = Convert.ToInt32(fc["IndustriesID"]);
            _objPromotion.Status = "A";
            _objPromotion.IsActive = false;
            db.PromotionLibraries.Add(_objPromotion);
            db.SaveChanges();
            var lastID = _objPromotion.PromotionLibraryID;
            string[] abc = result.Split('~');
            PromotionProductList _objproduct = new PromotionProductList();
            for (int i = 0; i < abc.Count() - 1; i++)
            {
                _objproduct.PromotionLibraryID = Convert.ToInt32(lastID);
                _objproduct.ProductName = abc[i].Split(',')[0];
                _objproduct.ProductIndustry = abc[i].Split(',')[1];
                if (file1[i] != null)
                {
                    _objproduct.ProductBrochure = file1[i].FileName;
                    file1[i].SaveAs(Server.MapPath("~/Content/PromotionFile/" + _objproduct.ProductBrochure));
                }
                if (file1[i + 1] != null)
                {
                    _objproduct.URSFormat = file1[i + 1].FileName;
                    file1[i + 1].SaveAs(Server.MapPath("~/Content/PromotionFile/" + _objproduct.URSFormat));
                }
                db.PromotionProductLists.Add(_objproduct);
                db.SaveChanges();
            }
            PromotionCertificate _objprmCertificate = new PromotionCertificate();
            string[] mno = result1.Split('~');
            for (int i = 0; i < mno.Count() - 1; i++)
            {
                _objprmCertificate.PromotionLibraryID = Convert.ToInt32(lastID);
                _objprmCertificate.CertificateName = mno[i];
                if (file2[i] != null)
                    _objprmCertificate.CertificateAttachment = file2[i].FileName;
                file2[i].SaveAs(Server.MapPath("~/Content/PromotionFile/" + _objprmCertificate.CertificateAttachment));
            }
            db.PromotionCertificates.Add(_objprmCertificate);
            db.SaveChanges();
            PromotionOtherDocument _objprmOtherDocument = new PromotionOtherDocument();
            string[] xyz = result2.Split('~');
            for (int i = 0; i < xyz.Count() - 1; i++)
            {
                _objprmOtherDocument.PromotionLibraryID = Convert.ToInt32(lastID);
                _objprmOtherDocument.OtherDocumentName = xyz[i];
                if (file3[i] != null)
                    _objprmOtherDocument.DocumentAttachment = file3[i].FileName;
                file3[i].SaveAs(Server.MapPath("~/Content/PromotionFile/" + _objprmOtherDocument.DocumentAttachment));
            }
            db.PromotionOtherDocuments.Add(_objprmOtherDocument);
            db.SaveChanges();
            string[] ch = chkvalue.Split(',');
            PromotionIndustry objProIndustry = new PromotionIndustry();

            for (int i = 0; i < ch.Count(); i++)
            {
                objProIndustry.PromotionLibraryID = Convert.ToInt32(lastID);
                objProIndustry.IndustriesID = Convert.ToInt32(ch[i]);
                db.PromotionIndustries.Add(objProIndustry);
                db.SaveChanges();

            }

            TempData["Data"] = "Thanks for sharing your material with i4i.<br/> Your request has been received and is subjected to approval from i4i.";
            TempData["vColor"] = "green";
            return RedirectToAction("PromotionLibraryForm");

        }
        #endregion
        #region code for view all products dtails
        public ActionResult PromotionDetails(string PromotionID)
        {
            PromotionProductListDAO obj = new PromotionProductListDAO();
            List<PromotionProductListDAO> objPromotionlist = new List<PromotionProductListDAO>();
            List<PromotionOtherDocument> lstOtherDocuments = new List<PromotionOtherDocument>();
            List<PromotionCertificate> lstCertificate = new List<PromotionCertificate>();
            List<PromotionIndustry> lstpromotionIndustry = new List<PromotionIndustry>();
            int PId = Convert.ToInt32(PromotionID);

            var result = (from pd in db.PromotionProductLists
                          where pd.PromotionLibraryID == PId
                          select new
                          {
                              pd.ProductName,
                              pd.ProductBrochure,
                              pd.URSFormat,
                              pd.ProductIndustry
                          });
            foreach (var data in result)
            {
                TempData["indusId"] = data.ProductIndustry;
                objPromotionlist.Add(new PromotionProductListDAO()
                {
                    ProductName = data.ProductName,
                    ProductBrochure = data.ProductBrochure,
                    URSFormat = data.URSFormat,
                    PromotionLibraryID = PId
                });

            }
            var otherDocuments = (from od in db.PromotionOtherDocuments
                                  where od.PromotionLibraryID == PId
                                  select new
                                  {
                                      od.OtherDocumentName,
                                      od.DocumentAttachment


                                  });

            foreach (var dataDocumnet in otherDocuments)
            {
                lstOtherDocuments.Add(new PromotionOtherDocument()
                {

                    OtherDocumentName = dataDocumnet.OtherDocumentName,
                    DocumentAttachment = dataDocumnet.DocumentAttachment

                });

            }
            var certificate = (from c in db.PromotionCertificates
                               where c.PromotionLibraryID == PId
                               select new
                               {
                                   c.CertificateName,
                                   c.CertificateAttachment
                               });

            foreach (var certificateData in certificate)
            {
                lstCertificate.Add(new PromotionCertificate()
                {

                    CertificateName = certificateData.CertificateName,
                    CertificateAttachment = certificateData.CertificateAttachment

                });

            }

            List<PromotionLibraryDAO> _objlist = new List<PromotionLibraryDAO>();
            List<PromotionLibraryDAO> _objlistuser = new List<PromotionLibraryDAO>();

            int industryId = Convert.ToInt32(TempData["indusId"]);
            string indId = industryId.ToString();
            var dataAssociateName = (from a in db.Industries
                                     join p in db.AssociateProjectIndustries on a.IndustriesID equals p.IndustriesID
                                     join r in db.RegisterUsers on p.Id equals r.Id
                                     where a.IndustriesID == industryId && r.UserRole == "A"
                                     select new { r.UserNo, r.Id, r.CountryName, r.UserFirstName }
                                     );
            foreach (var objData in dataAssociateName)
            {
                _objlist.Add(new PromotionLibraryDAO()
                {
                    USerNo = objData.UserNo,
                    Id = objData.Id,
                    CountryName = objData.CountryName,
                    UserFirstName = objData.UserFirstName
                });
            }

            //code for get worldref user 
            //var re = (from a in db.RegisterUsers
            //          where a.Industries == indId && a.UserRole == "W"
            //          select new { a.UserNo, a.Id, a.CountryName, a.UserFirstName }
            //            );

            //foreach (var objData in re)
            //{
            //    _objlistuser.Add(new PromotionLibraryDAO()
            //    {
            //        USerNo = objData.UserNo,
            //        Id = objData.Id,
            //        CountryName = objData.CountryName,
            //        UserFirstName = objData.UserFirstName
            //    });
            //}


            obj.objlistpromotionlib = _objlist;
            //obj.worlrefUserList = _objlistuser;
            obj.certificatelist = lstCertificate;
            obj._Productlist = objPromotionlist;
            obj.documentlist = lstOtherDocuments;
            return View(obj);

        }
        #endregion
        #region code for Promotion list to Admin
        public ActionResult AdminPromotionList()
        {

            List<PromotionLibraryDAO> a = new List<PromotionLibraryDAO>();
            var result = (from p in db.PromotionLibraries
                          where p.IsActive == false && p.Status == "A"
                          select new
                          {
                              p.PromotionLibraryID,
                              p.PName,
                              p.PEmailId,
                              p.PReference,
                              p.PContactNumber,
                              p.PCompanyProfile
                          }
                          ).AsEnumerable();

            foreach (var Data in result)
            {
                a.Add(new PromotionLibraryDAO
                {
                    PName = Data.PName,
                    PEmailId = Data.PEmailId,
                    PReference = Data.PReference,
                    PContactNumber = Data.PContactNumber,
                    PCompanyProfile = Data.PCompanyProfile,
                    PromotionLibraryID = Data.PromotionLibraryID

                });
            }

            return View(a);
        }
        #endregion
        #region code for Approve/Disapprove promotion list Admin
        [HttpPost]
        public ActionResult AdminPromotionList(FormCollection form, string[] ids)
        {
            List<PromotionLibraryDAO> a = new List<PromotionLibraryDAO>();
            var chckedValues = form.GetValues("ids");
            if (ids == null)
            {
                ViewBag.AlertMsg = "Please select atleast one Project for Approval";
                var result = (from p in db.PromotionLibraries
                              where p.IsActive == false && p.Status == "A"
                              select new
                              {
                                  p.PromotionLibraryID,
                                  p.PName,
                                  p.PEmailId,
                                  p.PReference,
                                  p.PContactNumber,
                                  p.PCompanyProfile
                              }
                         ).AsEnumerable();


                foreach (var Data in result)
                {
                    a.Add(new PromotionLibraryDAO()
                    {
                        PName = Data.PName,
                        PEmailId = Data.PEmailId,
                        PReference = Data.PReference,
                        PContactNumber = Data.PContactNumber,
                        PCompanyProfile = Data.PCompanyProfile,
                        PromotionLibraryID = Data.PromotionLibraryID

                    });
                }
            }
            else
            {
                foreach (var id in chckedValues)
                {
                    int d = Convert.ToInt32(id);
                    PromotionLibrary c = (from x in db.PromotionLibraries
                                          where x.PromotionLibraryID == d
                                          select x).SingleOrDefault();

                    c.IsActive = true;
                    c.Status = "A";

                    db.SaveChanges();
                }

                var result = (from p in db.PromotionLibraries
                              where p.IsActive == false && p.Status == "A"
                              select new
                              {
                                  p.PromotionLibraryID,
                                  p.PName,
                                  p.PEmailId,
                                  p.PReference,
                                  p.PContactNumber,
                                  p.PCompanyProfile
                              }
                          ).AsEnumerable();

                foreach (var Data in result)
                {
                    a.Add(new PromotionLibraryDAO
                    {
                        PName = Data.PName,
                        PEmailId = Data.PEmailId,
                        PReference = Data.PReference,
                        PContactNumber = Data.PContactNumber,
                        PCompanyProfile = Data.PCompanyProfile,
                        PromotionLibraryID = Data.PromotionLibraryID

                    });
                }
            }

            return View(a);
        }
        #endregion
        #region code for all approve list admin
        public JsonResult GetAllApprovedPromotionList()
        {
            List<PromotionLibraryDAO> _objPlDao = new List<PromotionLibraryDAO>();
            var result = (from p in db.PromotionLibraries
                          where p.IsActive == true && p.Status == "A"
                          select new
                          {
                              p.PromotionLibraryID,
                              p.PName,
                              p.PEmailId,
                              p.PReference,
                              p.PContactNumber,
                              p.PCompanyProfile
                          }
                          ).AsEnumerable();

            foreach (var Data in result)
            {
                _objPlDao.Add(new PromotionLibraryDAO
                {
                    PName = Data.PName,
                    PEmailId = Data.PEmailId,
                    PReference = Data.PReference,
                    PContactNumber = Data.PContactNumber,
                    PCompanyProfile = Data.PCompanyProfile,
                    PromotionLibraryID = Data.PromotionLibraryID

                });
            }

            return Json(_objPlDao, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region code for All Disapprove promotion list Admin
        public JsonResult GetAllPromotionDisapproveList()
        {
            List<PromotionLibraryDAO> _objPlDaoList = new List<PromotionLibraryDAO>();
            var result = (from p in db.PromotionLibraries
                          where p.IsActive == false && p.Status == "D"
                          select new
                          {
                              p.PromotionLibraryID,
                              p.PName,
                              p.PEmailId,
                              p.PReference,
                              p.PContactNumber,
                              p.PCompanyProfile,
                              p.ReasonForDisapproved
                          }
                        ).AsEnumerable();

            foreach (var Data in result)
            {
                _objPlDaoList.Add(new PromotionLibraryDAO
                {
                    PName = Data.PName,
                    PEmailId = Data.PEmailId,
                    PReference = Data.PReference,
                    PContactNumber = Data.PContactNumber,
                    PCompanyProfile = Data.PCompanyProfile,
                    PromotionLibraryID = Data.PromotionLibraryID,
                    ReasonForDisapproved = Data.ReasonForDisapproved

                });
            }
            return Json(_objPlDaoList, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region code for send message promotion is dissapproved
        public JsonResult SendDisapproveMesgAdmin(string ID, string VALUE)
        {
            List<PromotionLibraryDAO> msgList = new List<PromotionLibraryDAO>();
            int id = Convert.ToInt32(ID);
            var result = (from s in db.PromotionLibraries
                          where s.PromotionLibraryID == id
                          select s).FirstOrDefault();

            result.ReasonForDisapproved = VALUE;
            result.Status = "D";
            result.IsActive = false;
            db.SaveChanges();


            var result1 = (from p in db.PromotionLibraries
                           join pr in db.PromotionProductLists on p.PromotionLibraryID equals pr.PromotionLibraryID

                           where p.IsActive == false && p.Status == "A"
                           select new
                           {
                               p.PName,
                               p.PContactNumber,
                               p.PCompanyProfile,
                               pr.ProductName,

                               p.PromotionLibraryID
                           }
                        ).AsEnumerable();

            foreach (var Data in result1)
            {
                msgList.Add(new PromotionLibraryDAO
                {
                    PName = Data.PName,
                    PContactNumber = Data.PContactNumber,
                    PCompanyProfile = Data.PCompanyProfile,
                    ProductName = Data.ProductName,

                    PromotionLibraryID = Data.PromotionLibraryID
                });
            }

            return Json(msgList, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region code for show industries wise associates
        public ActionResult ShowIndustryWiseAssociate(string PromotionID)
        {

            PromotionLibraryDAO obj = new PromotionLibraryDAO();

            int plId = Convert.ToInt32(PromotionID);
            TempData["PLibraryID"] = plId.ToString();
            List<PromotionLibraryDAO.SelectListItem> Industrieslist = new List<PromotionLibraryDAO.SelectListItem>();
            var _industries = (from p in db.Industries
                               select new { p.IndustriesID, p.IndustriesName }).AsEnumerable();

            Industrieslist.Add(new PromotionLibraryDAO.SelectListItem() { Text = "Select Industry", Value = 0 });
            foreach (var item in _industries)
            {
                Industrieslist.Add(new PromotionLibraryDAO.SelectListItem() { Text = item.IndustriesName, Value = item.IndustriesID });

            }
            obj.IndustriesListModel = Industrieslist;

            return View(obj);
        }
        #endregion
        #region code for show list of associates
        public JsonResult ShowListOfAssociates(string ID)
        {
            List<PromotionLibraryDAO> _objlist = new List<PromotionLibraryDAO>();
            int id = Convert.ToInt32(ID);
            var Iname = (from i in db.Industries
                         where i.IndustriesID == id
                         select new { i.IndustriesName }
                            );
            string nameIndustries = (Iname.FirstOrDefault().IndustriesName).ToString();

            //            select r.userno,i.industriesName from industries i  join AssociateProjectIndustries ai on ai.industriesID=i.industriesID
            //join RegisterUser r on r.id=ai.id where i.industriesname='Power' and r.userrole='A'
            var dataAssociateName = (from a in db.Industries
                                     join p in db.AssociateProjectIndustries on a.IndustriesID equals p.IndustriesID
                                     join r in db.RegisterUsers on p.Id equals r.Id
                                     where a.IndustriesName == nameIndustries && r.UserRole == "A"
                                     select new { r.UserNo, r.Id, r.CountryName, r.UserFirstName }
                                       );

            foreach (var objData in dataAssociateName)
            {
                _objlist.Add(new PromotionLibraryDAO()
                {
                    USerNo = objData.UserNo,
                    Id = objData.Id,
                    CountryName = objData.CountryName,
                    UserFirstName = objData.UserFirstName
                });
            }
            return Json(_objlist, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region code for promote Associate
        [HttpPost]
        public ActionResult Save(FormCollection fc, string[] ids)
        {
            Promote_Associate objpt = new Promote_Associate();
            //var data = TempData["PLibraryID"];
            var data = fc["proLibId"];
            var chckedValues = fc.GetValues("ids");
            if (ids != null)
            {
                foreach (var x in chckedValues)
                {
                    int asID = Convert.ToInt32(x);
                    var email = (from s in db.RegisterUsers
                                 where s.Id == asID
                                 select new { s.Email });

                    string emailid = (email.FirstOrDefault().Email).ToString();
                    var userName = (from s in db.RegisterUsers
                                    where s.Id == asID
                                    select new { s.UserNo });

                    string Uname = (userName.FirstOrDefault().UserNo).ToString();
                    objpt.PromotionLibraryID = Convert.ToInt32(data);
                    objpt.Id = Convert.ToInt32(x);
                    objpt.IndustriesID = Convert.ToInt32(fc["IndustriesName"]);
                    db.Promote_Associate.Add(objpt);
                    db.SaveChanges();
                    SendMailModels objModel = new SendMailModels();
                    objModel.SendMail(Uname, emailid, "Promotional Library", "<div style='font-size:12pt; font-family:Calibri Light;'>Thank you for your patience. You can sign in <a href='http://182.73.96.52:94/AssociateLoginPage/AssociateLogin' style='border: none;'>Click here </a> and check the promotional</b><br/>material for your products. We assure you for quality,experience and services while we expect that you will make the customer confident and convinced about our local presence and services.</div>");
                    TempData["msg"] = "Promotional material sent to the selected Associate/Worldref User";
                    TempData["vColor"] = "red";
                }
            }
            return RedirectToAction("PromotionDetails", new { PromotionID = data });

        }
        #endregion
        #region code for promotion list to Associate
        public ActionResult AssociatePromotionList()
        {
            List<PromotionLibraryDAO> a = new List<PromotionLibraryDAO>();
            int AssociateID = Convert.ToInt32(Request.Cookies["AssociateId"].Value);

            var i = (from d in db.Promote_Associate
                     where d.Id == AssociateID
                     select d);
            int j = i.Count();

            if (j == 0)
            {
                TempData["msg"] = "Sir, you will be able to access your promotional material soon. Sorry for the inconvenience.";
                TempData["vColor"] = "red";
            }
            else
            {

                var result = (from pl in db.PromotionLibraries
                              join pt in db.Promote_Associate on pl.PromotionLibraryID equals pt.PromotionLibraryID
                              where pt.Id == AssociateID
                              select new
                              {
                                  pl.PromotionLibraryID,
                                  pl.PName,
                                  pl.PEmailId,
                                  pl.PReference,
                                  pl.PContactNumber,
                                  pl.PCompanyProfile
                              }
                              ).AsEnumerable();

                foreach (var Data in result)
                {
                    a.Add(new PromotionLibraryDAO
                    {
                        PName = Data.PName,
                        PEmailId = Data.PEmailId,
                        PReference = Data.PReference,
                        PContactNumber = Data.PContactNumber,
                        PCompanyProfile = Data.PCompanyProfile,
                        PromotionLibraryID = Convert.ToInt32(Data.PromotionLibraryID)

                    });
                }

                return View(a);
            }
            return RedirectToAction("AssociateHome", "AssociateLoginPage");
        }
        #endregion
        #region code for view all products dtails to Associate

        public ActionResult AssociatePromotionDetails(string PromotionID)
        {
            PromotionProductListDAO obj = new PromotionProductListDAO();
            List<PromotionProductListDAO> objPromotionlist = new List<PromotionProductListDAO>();
            List<PromotionOtherDocument> lstOtherDocuments = new List<PromotionOtherDocument>();
            List<PromotionCertificate> lstCertificate = new List<PromotionCertificate>();
            List<PromotionIndustryDAO> lstpromotionIndustry = new List<PromotionIndustryDAO>();
           // List<PromotionIndustry> lstpromotionIndustry = new List<PromotionIndustry>();
            int PId = Convert.ToInt32(PromotionID);
            var result = (from pd in db.PromotionProductLists
                          where pd.PromotionLibraryID == PId
                          select new
                          {
                              pd.ProductName,
                              pd.ProductBrochure,
                              pd.URSFormat

                          });
            foreach (var data in result)
            {
                objPromotionlist.Add(new PromotionProductListDAO()
                {
                    ProductName = data.ProductName,
                    ProductBrochure = data.ProductBrochure,
                    URSFormat = data.URSFormat

                });

            }

            var otherDocuments = (from od in db.PromotionOtherDocuments
                                  where od.PromotionLibraryID == PId
                                  select new
                                  {
                                      od.OtherDocumentName,
                                      od.DocumentAttachment


                                  });

            foreach (var dataDocumnet in otherDocuments)
            {
                lstOtherDocuments.Add(new PromotionOtherDocument()
                {

                    OtherDocumentName = dataDocumnet.OtherDocumentName,
                    DocumentAttachment = dataDocumnet.DocumentAttachment

                });

            }

            var certificate = (from c in db.PromotionCertificates
                               where c.PromotionLibraryID == PId
                               select new
                               {
                                   c.CertificateName,
                                   c.CertificateAttachment
                               });

            foreach (var certificateData in certificate)
            {
                lstCertificate.Add(new PromotionCertificate()
                {

                    CertificateName = certificateData.CertificateName,
                    CertificateAttachment = certificateData.CertificateAttachment

                });

            }

            var industry = (from i in db.Industries
                            join pi in db.PromotionIndustries on i.IndustriesID equals pi.IndustriesID
                            where pi.PromotionLibraryID == PId
                            select new
                            {
                                i.IndustriesName
                            });


            foreach (var ind in industry)
            {
                lstpromotionIndustry.Add(new PromotionIndustryDAO()
                {

                    IndustriesName = ind.IndustriesName

                });

            }
            obj.cindustrylist = lstpromotionIndustry;
            obj.certificatelist = lstCertificate;
            obj._Productlist = objPromotionlist;
            obj.documentlist = lstOtherDocuments;
            return View(obj);

        }


        #endregion
        #region code for promotion list to WorldRef
        public ActionResult WorldRefPromotionList()
        {
            List<PromotionLibraryDAO> a = new List<PromotionLibraryDAO>();
            int WorldRefUserID = Convert.ToInt32(Request.Cookies["UserId"].Value);

            List<Int32> allPromotionId = new List<Int32>();

            var i = (from d in db.Promote_Associate
                     where d.Id == WorldRefUserID
                     select d);
            int j = i.Count();

            if (j == 0)
            {
                TempData["ErrorMessage"] = "Sir, you will be able to access your promotional material soon. Sorry for the inconvenience.";
                TempData["vColor"] = "red";
            }
            else
            {

                var result = (from pl in db.PromotionLibraries
                              join pt in db.Promote_Associate on pl.PromotionLibraryID equals pt.PromotionLibraryID
                              where pt.Id == WorldRefUserID
                              select new
                              {
                                  pl.PromotionLibraryID,
                                  pl.PName,
                                  pl.PEmailId,
                                  pl.PReference,
                                  pl.PContactNumber,
                                  pl.PCompanyProfile
                              }
                              ).AsEnumerable();

                foreach (var Data in result)
                {
                    if (allPromotionId.Contains(Convert.ToInt32(Data.PromotionLibraryID)))
                    { }
                    else
                    {
                        a.Add(new PromotionLibraryDAO
                        {
                            PName = Data.PName,
                            PEmailId = Data.PEmailId,
                            PReference = Data.PReference,
                            PContactNumber = Data.PContactNumber,
                            PCompanyProfile = Data.PCompanyProfile,
                            PromotionLibraryID = Convert.ToInt32(Data.PromotionLibraryID)

                        });
                        allPromotionId.Add(Convert.ToInt32(Data.PromotionLibraryID));
                    }
                }

                return View(a);
            }
            return RedirectToAction("UploadExcel", "WorldRef");
        }


        public ActionResult WorldRefPromotionDetails(string PromotionID)
        {
            PromotionProductListDAO obj = new PromotionProductListDAO();
            List<PromotionProductListDAO> objPromotionlist = new List<PromotionProductListDAO>();
            List<PromotionOtherDocument> lstOtherDocuments = new List<PromotionOtherDocument>();
            List<PromotionCertificate> lstCertificate = new List<PromotionCertificate>();
            List<PromotionIndustryDAO> lstpromotionIndustry = new List<PromotionIndustryDAO>();
           // List<PromotionIndustry> lstpromotionIndustry = new List<PromotionIndustry>();
            int PId = Convert.ToInt32(PromotionID);
            var result = (from pd in db.PromotionProductLists
                          where pd.PromotionLibraryID == PId
                          select new
                          {
                              pd.ProductName,
                              pd.ProductBrochure,
                              pd.URSFormat

                          });
            foreach (var data in result)
            {
                objPromotionlist.Add(new PromotionProductListDAO()
                {
                    ProductName = data.ProductName,
                    ProductBrochure = data.ProductBrochure,
                    URSFormat = data.URSFormat

                });

            }

            var otherDocuments = (from od in db.PromotionOtherDocuments
                                  where od.PromotionLibraryID == PId
                                  select new
                                  {
                                      od.OtherDocumentName,
                                      od.DocumentAttachment


                                  });

            foreach (var dataDocumnet in otherDocuments)
            {
                lstOtherDocuments.Add(new PromotionOtherDocument()
                {

                    OtherDocumentName = dataDocumnet.OtherDocumentName,
                    DocumentAttachment = dataDocumnet.DocumentAttachment

                });

            }

            var certificate = (from c in db.PromotionCertificates
                               where c.PromotionLibraryID == PId
                               select new
                               {
                                   c.CertificateName,
                                   c.CertificateAttachment
                               });

            foreach (var certificateData in certificate)
            {
                lstCertificate.Add(new PromotionCertificate()
                {

                    CertificateName = certificateData.CertificateName,
                    CertificateAttachment = certificateData.CertificateAttachment

                });

            }

            var industry = (from i in db.Industries
                            join pi in db.PromotionIndustries on i.IndustriesID equals pi.IndustriesID
                            where pi.PromotionLibraryID == PId
                            select new
                            {
                                i.IndustriesName
                            });


            foreach (var ind in industry)
            {
                lstpromotionIndustry.Add(new PromotionIndustryDAO()
                {

                    IndustriesName = ind.IndustriesName

                });

            }
            obj.cindustrylist = lstpromotionIndustry;
            obj.certificatelist = lstCertificate;
            obj._Productlist = objPromotionlist;
            obj.documentlist = lstOtherDocuments;
            return View(obj);

        }
        #endregion
    }
	
}