using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorldRef.DataLayer;
using WorldRef.Models;
using WorldRef.BusinessLayer;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Net;
using System.Runtime.Serialization.Json;
using Facebook;
namespace WorldRef.Controllers
{
    public class WorldRefController : Controller
    {
        string FullSearchString;
        public I4IDBEntities context = new I4IDBEntities();
        private LikeRating likeRating;
        public WorldRefController()
        {
            likeRating = new LikeRating();
        }
        #region code for worldref home page added by rahul shukla Date : 16-11-15
        public ActionResult WorldRefIndex()
        {
            SignUpWorldRefModel signUpModel = new SignUpWorldRefModel();
            signUpModel = GetAllSignUpDetails();

            return View(signUpModel);
        }
        #endregion
        #region All SignUp Details added by rahul shukla date : 16-11-15
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
            #region added by taps on 11-05-2015
            //get list for business interest country
            List<SelectListItem> selectListBusinessCountry = new List<SelectListItem>();
            var country = (from p in context.Countries
                           select new { p.CountryID, p.CountryName }).AsEnumerable();

            foreach (var item in country)
            {
                selectListBusinessCountry.Add(new SelectListItem() { Text = item.CountryName, Value = item.CountryName });

            }
            signUpModel.BusinessInterestCountryList = selectListBusinessCountry;

            //get list for language
            List<SelectListItem> SelectLanguageList = new List<SelectListItem>();
            var language = (from p in context.Languages
                            select new { p.LanguageID, p.LanguageName }).AsEnumerable();
            SelectLanguageList.Add(new SelectListItem() { Text = "Select Language", Value = "0" });
            foreach (var item1 in language)
            {
                SelectLanguageList.Add(new SelectListItem() { Text = item1.LanguageName, Value = item1.LanguageID.ToString() });
            }
            signUpModel.LanguageList = SelectLanguageList;
            #endregion
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
        #region Partial View for Sign in
        public ActionResult _SignIN()
        {
            return PartialView();
        }
        #endregion
        #region code for login uploader in WorldrefIndex added by Rahul  Shukla Date :29/09/2015

        public JsonResult LoginIndex(string UserName, string Password, string returnUrl)
        {
            SignUpWorldRef signup = new SignUpWorldRef();
            SignUpWorldRefModel signWorldModel = signup.Login(UserName, Password);
            if (signWorldModel != null)
            {
                HttpCookie userNameCookies = new HttpCookie("username");
                userNameCookies.Value = signWorldModel.Name;
                Response.Cookies.Add(userNameCookies);

                HttpCookie userNameCookiesq = new HttpCookie("UsrNo");
                userNameCookiesq.Value = signWorldModel.UserNo;
                Response.Cookies.Add(userNameCookiesq);




                HttpCookie genraluserPhoto = new HttpCookie("GenuserPhoto");
                genraluserPhoto.Value = signWorldModel.PhotoAttach;
                Response.Cookies.Add(genraluserPhoto);

                HttpCookie userNoCookies = new HttpCookie("UName");
                userNoCookies.Value = UserName;
                Response.Cookies.Add(userNoCookies);

                HttpCookie userNameCookies1 = new HttpCookie("UserId");
                userNameCookies1.Value = signWorldModel.Id.ToString();
                Response.Cookies.Add(userNameCookies1);



                HttpCookie userNameCookies2 = new HttpCookie("UserRole");
                userNameCookies2.Value = signWorldModel.UserRole.ToString();
                Response.Cookies.Add(userNameCookies2);

                if (signWorldModel.UserRole == "W")

                    return Json("ue", JsonRequestBehavior.AllowGet);
                else
                    return Json("re", JsonRequestBehavior.AllowGet);
            }
            else
            {
                signWorldModel = signup.SignInCommonUser(UserName, Password);
                if (signWorldModel != null)
                {
                    HttpCookie userNameCookies = new HttpCookie("Cusername");
                    userNameCookies.Value = signWorldModel.Name;
                    Response.Cookies.Add(userNameCookies);

                    HttpCookie userNameCookies1 = new HttpCookie("CUserId");
                    userNameCookies1.Value = signWorldModel.Id.ToString();
                    Response.Cookies.Add(userNameCookies1);
                    return Json("wi", JsonRequestBehavior.AllowGet);

                }
                else
                {

                    TempData.Clear();

                    return Json("nr", JsonRequestBehavior.AllowGet);
                }
            }

        }
        #endregion
        #region code for save linkedin profile data in database
        public JsonResult SaveLinkedInProfile(string projid, string firstName, string email, string lastName, string Industry, string picUrl)
        {
            try
            {
                string UserNo = getUserNo();
                string password = GeneratePassword();
                RegisterUser objregister = new RegisterUser();
                objregister.UserFirstName = firstName;
                objregister.Email = email;
                objregister.UserLastName = lastName;
                objregister.Industries = Industry;
                objregister.UserNo = UserNo;
                objregister.Password = password;
                objregister.UserRole = "L";
                int creditcount = 0;
                //int userId = (from x in context.RegisterUsers
                //              where x.Email == email
                //              select x.Email).FirstOrDefault().Count();


                //int uId = Convert.ToInt32(userId.f);
                //if (userId > 0)
                //{
                //    var userNo = (from x in context.RegisterUsers
                //                  where x.Email == email
                //                  select x.UserNo).SingleOrDefault();
                //    string UserNumber = userNo.ToString();

                //    string UsName = UserNumber;
                //    HttpCookie userNoCookies = new HttpCookie("UserName");
                //    userNoCookies.Value = UsName;
                //    Response.Cookies.Add(userNoCookies);
                //    Response.Cookies.Set(userNoCookies);

                //}
                //else
                //{
                //    context.RegisterUsers.Add(objregister);
                //    context.SaveChanges();
                //    string UsName = UserNo;
                //    HttpCookie userNoCookies = new HttpCookie("UserName");
                //    userNoCookies.Value = UsName;
                //    Response.Cookies.Add(userNoCookies);
                //    Response.Cookies.Set(userNoCookies);
                //}
                int proid = Convert.ToInt32(projid);
                creditcount = (from x in context.LinkedInUsers
                               where x.id == proid && x.Email == email
                               select new { x.Email, x.id }).Count();
                if (creditcount > 0)
                {
                }
                else
                {
                    LinkedInUser objLuser = new LinkedInUser();
                    objLuser.id = Convert.ToInt32(projid);
                    objLuser.FirstName = firstName;
                    objLuser.LastName = lastName;
                    objLuser.Email = email;
                    objLuser.Industry = Industry;
                    objLuser.CreditM = false;
                    objLuser.picUrl = picUrl;
                    context.LinkedInUsers.Add(objLuser);
                    context.SaveChanges();
                }
                int projectId = Convert.ToInt32(projid);
                var emailid = (from x in context.LinkedInUsers
                               where x.id == projectId
                               select x.Email);
                string linkedEmailid = emailid.ToString();
            }
            catch (Exception ex)
            {

            }
            return Json("", JsonRequestBehavior.AllowGet);
        }


        public JsonResult SignInwithLinkedin(string firstName, string email, string lastName, string Industry, string picUrl)
        {
            try
            {
                string UserNo = getUserNo();
                string password = GeneratePassword();
                RegisterUser objregister = new RegisterUser();
                objregister.UserFirstName = firstName;
                objregister.Email = email;
                objregister.UserLastName = lastName;
                objregister.Industries = Industry;
                objregister.UserNo = UserNo;
                objregister.Password = password;
                objregister.UserRole = "L";


                var userId = (from x in context.RegisterUsers
                              where x.Email == email
                              select x.Email).Count();

                if (userId > 0)
                {
                    var model = (from x in context.RegisterUsers
                                 where x.Email == email && x.UserRole == "L"
                                 select new { x.UserNo, x.Id, x.UserRole }).SingleOrDefault();
                    string UserNumber = model.UserNo.ToString();
                    string Uid = model.Id.ToString();
                    string urole = model.UserRole.ToString();
                    string UsName = UserNumber;
                    HttpCookie userNoCookies = new HttpCookie("UserName");
                    userNoCookies.Value = UsName;
                    Response.Cookies.Add(userNoCookies);
                    Response.Cookies.Set(userNoCookies);

                    HttpCookie userNameCookies1 = new HttpCookie("UserId");
                    userNameCookies1.Value = Uid;
                    Response.Cookies.Add(userNameCookies1);
                    HttpCookie userNameCookies5 = new HttpCookie("UName");
                    userNameCookies5.Value = UserNo;
                    Response.Cookies.Add(userNameCookies5);


                    HttpCookie userNameCookies2 = new HttpCookie("UserRole");
                    userNameCookies2.Value = urole;
                    Response.Cookies.Add(userNameCookies2);

                    return Json("Success", JsonRequestBehavior.AllowGet);

                }
                else
                {
                    context.RegisterUsers.Add(objregister);
                    context.SaveChanges();
                    string UsName = UserNo;
                    HttpCookie userNoCookies = new HttpCookie("UserName");
                    userNoCookies.Value = UsName;
                    Response.Cookies.Add(userNoCookies);
                    Response.Cookies.Set(userNoCookies);

                    //var model = (from x in context.RegisterUsers
                    //             where x.Email == email
                    //             select new { x.UserNo, x.Id, x.UserRole }).SingleOrDefault();
                    var Userid = objregister.Id;

                    HttpCookie userNameCookies1 = new HttpCookie("UserId");
                    userNameCookies1.Value = Userid.ToString();
                    Response.Cookies.Add(userNameCookies1);


                    HttpCookie userNameCookies2 = new HttpCookie("UserRole");
                    userNameCookies2.Value = "L";
                    Response.Cookies.Add(userNameCookies2);

                }



            }
            catch (Exception ex)
            {

            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        private string GeneratePassword()
        {
            string strPwdchar = "abcdefghijklmnopqrstuvwxyz0123456789#+@&$ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string strPwd = "";
            Random rnd = new Random();
            for (int i = 0; i <= 7; i++)
            {
                int iRandom = rnd.Next(0, strPwdchar.Length - 1);
                strPwd += strPwdchar.Substring(iRandom, 1);
            }
            return strPwd;
        }
        private string getUserNo()
        {
            string UserName = string.Empty;

            UserName = GetUniqueKey(7);
            using (I4IDBEntities context = new I4IDBEntities())
            {
                var isexistKey = context.RegisterUsers.Where(m => m.UserNo == UserName).FirstOrDefault();
                if (isexistKey != null)
                {
                    getUserNo();
                }
            }
            return UserName;
        }
        public static string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[62];
            chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[maxSize];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }
        #endregion
        #region code for sign out  added by rahul shukla modified date: 29/09/2015
        public ActionResult SignOut()
        {
            Response.Cookies.Clear();
            FormsAuthentication.SignOut();

            string[] myCookie = Request.Cookies.AllKeys;
            foreach (string cookie in myCookie)
            {
                Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            }

            Session.Clear();

            return RedirectToAction("WorldrefIndex");
        }
        #endregion
        #region code for save uploader details in database added by rahul shukla dated :01/10/2015
        [HttpPost]
        public ActionResult SignUp(SignUpWorldRefModel signModel, FormCollection formCollection, string ProfileReason)
        {

            ISignUpWorldRef signup = new SignUpWorldRef();
            Email email = new Email();

            var IsRegistered =
                (from q in context.RegisterUsers where q.Email.ToLower() == signModel.Email.ToLower() && q.UserRole == "W" select q.UserNo).
                    FirstOrDefault();

            if (IsRegistered != null)
            {
                TempData.Clear();
                TempData.Add("ErrorMessage", "Email Id Already Registered");
                return RedirectToAction("WorldrefIndex");
            }
            if (ProfileReason == "BusinessUnit")
            {
                signModel.UploaderType = ProfileReason;

            }
            else
            {

                signModel.UploaderType = ProfileReason;
            }
            string path = string.Empty;
            string filename = string.Empty;
            string Extension = string.Empty;
            string guid = string.Empty;
            string CompanyLogo = string.Empty;
            string CompanyProfile = string.Empty;

            for (int i = 0; i < Request.Files.Count; i++)
            {
                guid = Guid.NewGuid().ToString();
                path = AppDomain.CurrentDomain.BaseDirectory + "uploads/";
                bool existPath = Directory.Exists(path);
                if (!existPath) Directory.CreateDirectory(path);

                filename = Path.GetFileName(Request.Files[i].FileName);
                Extension = Path.GetExtension(Request.Files[i].FileName);
                if (i == 0)
                {
                    CompanyProfile = Path.Combine(guid + Extension).ToString();
                }
                else
                {
                    CompanyLogo = Path.Combine(guid + Extension).ToString();
                }
                Request.Files[i].SaveAs(Path.Combine(path, guid + Extension));
            }
            signModel.OrganisationName = formCollection["ParentOrganisationName"];
            signModel.BussinessUnitName = formCollection["BusinessUnitName"];
            signModel.MyCompany = formCollection["Type"];
            signModel.OfficialNumber = formCollection["ContactNumber"];
            signModel.RecoveryMail = formCollection["AlternateEmail"];
            signModel.OtherMail = formCollection["OtherMail"];
            //OtherMail
            signModel.ProfileFileName = CompanyProfile;
            signModel.CompanyLogo = CompanyLogo;
            //added by taps
            signModel.BusinessInterestCountry = formCollection["BusinessInterestCountry"];          //business interest country
            signModel.ProfileUrl = formCollection["ProfileUrl"];
            signModel.ProfilePath = Path.Combine(guid + Extension).ToString();
            string userName = formCollection["ProfileUrl"];
            signModel.UserName = email.getUserNo();
            //signModel.UserName = email.getUserNo();
            signModel.Password = email.GeneratePassword();
            //  signModel.ContactNumber = signModel.ContactCode + "-" + signModel.ContactNumber;
            if (signModel.Type == "Others")
            {
                signModel.Type = signModel.OtherType;
            }

            if (!string.IsNullOrEmpty(signModel.OtherIndustryName))
            {
                AddOtherIndustry(signModel.OtherIndustryName);
                signModel.Industry = (from indus in context.Industries
                                      where indus.IndustriesName == signModel.OtherIndustryName
                                      select indus.IndustriesID).FirstOrDefault().ToString();
            }
            else
            {

                signModel.Industry = formCollection["Industry"];

            }
            string status = signup.Add(signModel);
            if (status == "Success")
            {
                TempData.Clear();
                var callbackUrl1 = Url.Action("PromotionLibraryform", "PromotionLibrary", new { }, protocol: Request.Url.Scheme);
                //Thank you for Signing Up with us. Please do not forget to submit your material at Promotional Library [Link] of i4i. Please do not forget to submit your material at <a href = '" + callbackUrl1 + "'>
                string textDisplay = "Thank you for Signing Up with us. The Username and Password has been sent to your email address.";
                //   string textDisplay = "Thank you for signing up with us.The Username and Password has been sent to your email address.You may continue to sign in.";
                string UserId = (from db in context.RegisterUsers where db.UserNo == signModel.UserName select db.Id).FirstOrDefault().ToString();
                // TempData.Add("ErrorMessage", textDisplay);
                TempData.Add("DisplayMessage", textDisplay);
                AddUserIndustry(Convert.ToInt32(UserId), AddIndustryToArray(formCollection, formCollection["Industry"].ToString()));
                string link = ConfigurationManager.AppSettings["WebLink"].ToString();
                var callbackUrl = Url.Action("ChangeUserNameAndPassword", "WorldRef", new { userId = UserId }, protocol: Request.Url.Scheme);

                string Subject = "Welcome to WorldRef";
                string Body = "<div style='font-size:15px; font-family:Calibri Light;'>Thank you for Registering with World Ref.<br/> <br/> Your username is : " + signModel.UserName + " <br/> Your password is : " + signModel.Password;
                Body = Body + "<br/><br/> In case you wish to change your username and password, please <a href = '" + callbackUrl + "'>Click here</a><br/><br/>";
                Body = Body +
                       " WorldRef gives you maximum exposure to utilize your experiences and achievements in the engineering industry.<br/><br/>Please upload your documents on our <a href = '" + callbackUrl1 + "'>Promotional Library</a> for access to our associates and customers, and for our review.</div>";// Promotional Library
                email.SendMail(signModel.Name, signModel.Email, Subject, Body);
                TempData["UserID"] = UserId;
                return RedirectToAction("SecondLevelSignUp");
            }
            else
            {
                TempData.Clear();
                TempData.Add("ErrorMessage", "Failed !.Please try again");
                return RedirectToAction("WorldrefIndex");
            }
        }
        #endregion
        private void AddOtherIndustry(string industry)
        {
            int ValueProvider;
            int industryId = (from indu in context.Industries
                              where indu.IndustriesName.ToLower() == industry.ToLower()
                              select indu.IndustriesID
                                ).FirstOrDefault();

            if (industryId == 0)
            {
                if (int.TryParse(industry, out ValueProvider))
                {
                    int industryValue = Convert.ToInt32(industry);
                    industryId = (from indu in context.Industries
                                  where indu.IndustriesID == industryValue
                                  select indu.IndustriesID
                                    ).FirstOrDefault();
                    if (industryId == 0)
                    {
                        context.Industries.Add(new Industry() { IndustriesName = industry });
                    }
                }
                else
                {
                    context.Industries.Add(new Industry() { IndustriesName = industry });
                }
                context.SaveChanges();
            }

        }
        private void AddUserIndustry(int userId, string[] industries)
        {
            foreach (string industry in industries)
            {
                if (!string.IsNullOrEmpty(industry))
                {
                    AddOtherIndustry(industry);
                    context.UserIndustries.Add(new UserIndustry()
                    {
                        UserId = userId,
                        IndustryId = GetIndustryId(industry),
                        createdOn = DateTime.Now
                    });
                    context.SaveChanges();
                }
            }
        }
        private int GetIndustryId(string IndustryName)
        {
            int ValueProvider;
            int industryId = (from indu in context.Industries
                              where indu.IndustriesName.ToLower() == IndustryName.ToLower()
                              select indu.IndustriesID
                                ).FirstOrDefault();
            if (industryId == 0)
            {
                if (int.TryParse(IndustryName, out ValueProvider))
                {
                    int indiesId = Convert.ToInt32(IndustryName);
                    industryId = (from indu in context.Industries
                                  where indu.IndustriesID == indiesId
                                  select indu.IndustriesID
                                ).FirstOrDefault();
                }
            }

            return industryId;
        }
        private string[] AddIndustryToArray(FormCollection frm, string DropDownvalue)
        {
            int i = 0;
            string[] industries = new string[50];
            foreach (var key in frm.AllKeys)
            {
                if (key.Contains("txt"))
                {
                    string valueOfLink = Request.Form[key];
                    industries[i] = valueOfLink;
                    i++;
                }
            }
            string[] dropValues = DropDownvalue.Split(',');

            foreach (string indus in dropValues)
            {
                industries[i] = indus;
                i++;
            }

            return industries;
        }
        #region code for get all country code on the basis of country added by rahul shukla Date :16-11-15
        public JsonResult GetCountryCode(string Countryname)
        {
            var content = (from p in context.Countries
                           where p.CountryName.ToLower() == Countryname.ToLower()
                           select new { p.Code }).FirstOrDefault();

            string CountryCode = string.Empty;

            if (content != null)
            {
                CountryCode = content.Code;
            }

            return Json(CountryCode, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region code for check username availability on recruiter sign up  added by rahul shukla dated:05/10/2015
        public JsonResult CheckUsername(string userName)
        {
            string IsAvailable;
            var getData = context.RegisterUsers.FirstOrDefault(m => m.UserFirstName == userName.Trim());
            if (getData == null)
            {
                IsAvailable = "Available";
            }
            else
            {
                IsAvailable = "Not Available";
            }
            return Json(IsAvailable, JsonRequestBehavior.AllowGet);
            // return IsAvailable;
        }
        #endregion
        #region code for save recruiter details in database added by rahul shukla modified date 05/10/2015
        [HttpPost]
        public ActionResult SaveRecruiters(FormCollection fc)
        {
            string emailId = fc["txtEmailAdd"];
            string Uname = fc["txtRecName"];
            SignUpWorldRefModel signModel = new SignUpWorldRefModel();
            RegisterUser register = new RegisterUser()
            {
                UserFirstName = fc["txtRecName"],
                Email = fc["txtEmailAdd"],
                phone = fc["txtConNum"],
                OrgName = fc["txtOrgName"],
                Industries = fc["IndustriesID"],
                RecruitmentType = fc["RecruitmentType"],
                RecruiterFlag = false,
                UserNo = getUserNo(),
                Password = GeneratePassword(),
                UserRole = "P",// user role for recruiters.
                Date = DateTime.Now
            };
            string OrgnisationName = fc["txtOrgName"];
            var Emailid = (from m in context.RegisterUsers where m.Company == OrgnisationName select new { m.Email }).FirstOrDefault();

            context.RegisterUsers.Add(register);
            context.SaveChanges();
            Email email = new Email();
            string Subject = "Welcome to WorldRef";
            string Body = "<div style='font-size:15px; font-family:Calibri Light;'>Thank you for Registering with World Ref.<br/> <br/> Your username is :" + signModel.UserNo + " <br/> Your password is :" + signModel.Password + " ";

            Body = Body +
                   " WorldRef gives you maximum exposure to utilize your experiences and achievements in the engineering industry.<br/><br/></div>";// Promotional Library
            email.SendMail(Uname, emailId, Subject, Body);

            return RedirectToAction("WorldrefIndex");
        }
        #endregion
        #region code Common User Registration on worldref added by rahul shukla dated :05/10/2015
        [HttpPost]
        public ActionResult SignUpUser(FormCollection fc, HttpPostedFileBase[] files)
        {
            SignUpWorldRef world = new SignUpWorldRef();
            string path = string.Empty;
            string filename = string.Empty;
            string SavePath = string.Empty;
            string Extension = string.Empty;
            try
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase postedFiles = Request.Files[i];

                    if (postedFiles != null)
                    {
                        SavePath = Guid.NewGuid().ToString();
                        Extension = Path.GetExtension(postedFiles.FileName);

                        filename = System.IO.Path.GetFileName(postedFiles.FileName);

                        postedFiles.SaveAs(Server.MapPath("~/uploads/" + SavePath + Extension));

                    }
                }

            }
            catch
            {


            }
            //UserNo = getUserNo(),
            string UserName = getUserNo();
            string CPerson = fc["txtName"];
            string CompanyName = fc["txtOrganization"];
            string ContactNumber = fc["txtContactno"];
            string emailId = fc["txtGemail"];
            string password = fc["txtGPassword"];
            string msg = world.SignUpLikeUser(CPerson, CompanyName, ContactNumber, emailId, password, UserName, filename);

            TempData.Clear();
            TempData.Add("ErrorMessage", msg);
            return RedirectToAction("WorldrefIndex");
            //return View(world.SignUpLikeUser( CPerson,CompanyName,ContactNumber,emailId,password,UserName,filename));
        }
        #endregion
        #region code for Search Image on worldref Added by rahul shukla dated:05/10/2015

        [HttpPost]
        public ActionResult WorldrefIndexImageSearch(FormCollection frm)
        {
            return RedirectToAction("SearchImages");
        }
        public ActionResult WorldrefIndexImageSearch()//Action methode for searchimages on index page
        {
            SignUpWorldRefModel signUpModel = new SignUpWorldRefModel();
            List<SelectListItem> selectListCountry = new List<SelectListItem>();
            List<SelectListItem> selectList = new List<SelectListItem>();//Developer, Investor, Fabricator, Procurement Staff, Others
            selectList.Add(new SelectListItem() { Text = "I am", Value = "0" });
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
            signUpModel = GetAllSignUpDetails();
            return View(signUpModel);
        }
        #endregion
        #region code for Search Videos On worldref Added by rahul shukla Dated : 05/10/2015
        [HttpPost]
        public ActionResult WorldrefIndexVideosSearch(FormCollection frm)
        {
            return RedirectToAction("SearchVideos");
        }
        public ActionResult WorldrefIndexVideosSearch()//Action methode for searchimages on index page
        {
            SignUpWorldRefModel signUpModel = new SignUpWorldRefModel();
            List<SelectListItem> selectListCountry = new List<SelectListItem>();
            List<SelectListItem> selectList = new List<SelectListItem>();//Developer, Investor, Fabricator, Procurement Staff, Others
            selectList.Add(new SelectListItem() { Text = "I am", Value = "0" });
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
            signUpModel = GetAllSignUpDetails();
            return View(signUpModel);
        }
        #endregion

        #region code for update reference added by rahul shukla Date :19-11-15
        public ActionResult UpdateReferences()
        {
            return View();
        
        }

        #endregion
        #region code for show experience list Added by rahul shukla dated : 05/10/2015
        
        public ActionResult ListExcelProjectsUser(string Command)
        {
            string status = "";
            if (Command == "UpdateRefrence")
            {
                status = "Update";
                TempData["state"] = status;
               
                return RedirectToAction("UpdateReferences");
            }
            else
            {
                if (Request.Cookies["UserId"] == null)
                {
                    return RedirectToAction("WorldrefIndex");
                }
                ReadExcel excel = new ReadExcel();
                int UserId = Convert.ToInt32(Request.Cookies["UserId"].Value);

                var cnt = (from s in context.WorldRefExcelDataProjects
                           where s.IsAuthorized == 1 && s.IsAdminAuthorized == true && s.userid == UserId
                           select s).Count();
                int count = Convert.ToInt32(cnt);
                ViewBag.approveCount = count;

                var cntDis = (from s in context.WorldRefExcelDataProjects
                              where s.IsAuthorized == 2 && s.IsAdminUnAuthorized == true && s.userid == UserId
                              select s).Count();
                int countDis = Convert.ToInt32(cntDis);
                ViewBag.DisapproveCount = countDis;
                List<WorldRefExcelDataModel> worldExcelProject = excel.ReadAllDataUser(UserId);
                return View(worldExcelProject);
            }
        }
        #endregion
        #region Add Project
        public PartialViewResult AddProjects()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult AddProject(FormCollection form)
        {
            try
            {
                int UserId = Convert.ToInt32(Request.Cookies["UserId"].Value);
                WorldRefExcelDataModel worldExcelModel = new WorldRefExcelDataModel();
                worldExcelModel.ProjectName = form["Project"].ToString();
                worldExcelModel.OrganizationName = form["OrganizationName"].ToString();
                worldExcelModel.CustomerName = form["CustomerName"].ToString();
                worldExcelModel.CustomerIndustryType = form["CustomerIndustryType"].ToString();
                worldExcelModel.Type = form["Type"].ToString();
                worldExcelModel.Country = form["Country"].ToString();
                worldExcelModel.Status = form["Status"].ToString();
                worldExcelModel.Year = form["Year"].ToString();
                worldExcelModel.Description = form["Description"].ToString();
                worldExcelModel.IsOrganization = form["chkOrganization"] == null ? false : true;
                worldExcelModel.IsCustomer = form["chkCustomer"] == null ? false : true;
                worldExcelModel.IsProject = form["chkProject"] == null ? false : true;
                //worldExcelModel.IsType = form["chkStatus"] == null ? false : true;
                worldExcelModel.IsYear = form["chkYear"] == null ? false : true;
                worldExcelModel.IsStatus = form["chkStatus"] == null ? false : true;
                worldExcelModel.userid = UserId;

                ReadExcel excel = new ReadExcel();
                excel.AddParticularProject(worldExcelModel);
                return RedirectToAction("ListExcelProjectsUser");
            }
            catch
            {

            }
            return View();
        }

        public JsonResult ListExcelProjectsUserApproved()
        {
            ReadExcel excel = new ReadExcel();
            int UserId = Convert.ToInt32(Request.Cookies["UserId"].Value);
            List<WorldRefExcelDataModel> worldExcelProject = excel.AllApproveDataUser(UserId);
            return Json(worldExcelProject, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ListExcelProjectsUserDisApproved()
        {
            ReadExcel excel = new ReadExcel();
            int UserId = Convert.ToInt32(Request.Cookies["UserId"].Value);
            List<WorldRefExcelDataModel> worldExcelProject = excel.AllDisApproveDataUser(UserId);
            return Json(worldExcelProject, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveDescription(int id, string DescriptionString)
        {
            WorldRefExcelDataProject excelProject = (from excel in context.WorldRefExcelDataProjects where excel.id == id select excel).FirstOrDefault();

            if (excelProject != null)
            {
                excelProject.Description = DescriptionString;
                context.SaveChanges();
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Edit Project

        public PartialViewResult EditparticularProject(int projectId)
        {
            TempData["projId"] = projectId.ToString();
            ReadExcel excel = new ReadExcel();
            WorldRefExcelDataModel projectModel = excel.ReadParticularProject(projectId);
            return PartialView(projectModel);
        }
        public ActionResult EditProject(WorldRefExcelDataModel excelData)
        {
            int projeId = Convert.ToInt32(TempData["projId"]);
            System.Threading.Thread.Sleep(5000);
            ReadExcel excel = new ReadExcel();
            string returnString = excel.EditParticularProject(excelData, projeId);
            if (!string.IsNullOrEmpty(returnString))
            {
                TempData["msg"] = "This changes is subjected to approval from i4i. So this project is moved to All Projects.";
                TempData["vColor"] = "red";
                return RedirectToAction("ListExcelProjectsUser");
            }

            return View();
        }
        #endregion
        #region code for add project image and videos added by rahul shukla modified date : 21-10-2015
        public PartialViewResult _addProjectImage(int ProjectId = 0)
        {
            ProjectImageModel projectImage = new ProjectImageModel();
            projectImage.ProjectId = ProjectId;
            return PartialView(projectImage);
        }
        [HttpPost]
        public ActionResult AddProjectImages(ProjectImageModel ProjectModel, HttpPostedFileBase[] files, FormCollection frm)
        {
            ReadExcel read = new ReadExcel();
            List<ImageStructure> projectImage = new List<ImageStructure>();
            try
            {
                string path = string.Empty;
                string filename = string.Empty;
                string SavePath = string.Empty;
                string Extension = string.Empty;

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase postedFiles = Request.Files[i];

                    if (postedFiles != null)
                    {
                        SavePath = Guid.NewGuid().ToString();
                        Extension = Path.GetExtension(postedFiles.FileName);

                        filename = System.IO.Path.GetFileName(postedFiles.FileName);

                        postedFiles.SaveAs(Server.MapPath("~/uploads/" + SavePath + Extension));

                        projectImage.Add(new ImageStructure()
                        {
                            ImageName = filename,
                            ImagePath = Path.Combine(SavePath + Extension).ToString(),
                            Link = false
                        });
                    }
                }

                foreach (var key in frm.AllKeys)
                {
                    if (key.Contains("txt"))
                    {
                        string valueOfLink = Request.Form[key];

                        if (!string.IsNullOrEmpty(valueOfLink))
                        {
                            if (valueOfLink.Contains("youtube"))
                            {
                                valueOfLink = valueOfLink.Replace("watch?v=", "embed/");
                            }
                            projectImage.Add(new ImageStructure()
                            {
                                ImageName = "",
                                ImagePath = valueOfLink,
                                Link = true
                            });
                        }
                    }
                }

                TempData.Clear();
                TempData.Add("SuccessMessage", "Images/videos uploaded successfully.");
            }
            catch
            {
                TempData.Clear();
                TempData.Add("ErrorMessage", "Image/Videos Not Uploaded Successfully.Please Try Again");
            }
            read.AddImageOrVideo(ProjectModel.ProjectId, projectImage);
            return RedirectToAction("ListExcelProjectsUser");
        }
        #endregion
        #region code for showcredit for approve and list of approve credit added by rahul shukla
        public PartialViewResult ShowCreditForApprove(int projectId)
        {
            ReadExcel excel = new ReadExcel();
            List<LinkedInUser> objlinkedin = excel.readCreditforApprove(projectId);
            return PartialView(objlinkedin);
        }
        public PartialViewResult ListOfApprovedCredit(int projectId)
        {

            ReadExcel excel = new ReadExcel();
            List<LinkedInUser> objlinkedin = excel.ListOfCreditApproved(projectId);
            return PartialView(objlinkedin);
        }
        public JsonResult UpdateForApproveCredit(string linkedid, string Designation)
        {
            int Lid = Convert.ToInt32(linkedid);

            LinkedInUser c = (from x in context.LinkedInUsers
                              where x.LinkedinUserID == Lid
                              select x).First();
            c.IsApproved = true;
            c.CreditM = true;
            c.Designation = Designation;
            c.Approvedate = DateTime.Now;
            context.SaveChanges();
            return Json("aa", JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region AdminAprroveReview

        public PartialViewResult AdminReviewList1(int? projectId)
        {
            ReviewViewModel viewModel = new ReviewViewModel();

            List<ReviewModel> proj = new List<ReviewModel>();
            List<ReviewModel> img = new List<ReviewModel>();

            var readReviewProject = (from Project in context.ProjectReviews
                                     join
                                         rr in context.RegisterUsers on Project.userId equals rr.Id
                                     where Project.ProjectId == projectId && Project.flag == true
                                     select new { Project, rr }).AsEnumerable();

            //I4IDBEntities db = new I4IDBEntities();
            //if (readReviewProject != null)
            //{
            //    foreach (var ii in readReviewProject)
            //    {
            //       // ProjectReview c = (from s in db.ProjectReviews where s.ProjectId == projectId select s).FirstOrDefault();
            //      var c = db.ProjectReviews.Where(a => a.ProjectId==(ii.Project.ProjectId)).FirstOrDefault();
            //        if (c != null)
            //        {
            //            c.flag = false;
            //        }
            //    }
            //    db.SaveChanges();
            //}




            //var readReviewImage = (from Img in context.ProjectImageVideoComments
            //                       join
            //                            rr in context.RegisterUsers on Img.UserId equals rr.Id
            //                       where Img.ProjectId == projectId
            //                       select new { Img, rr }).AsEnumerable();


            //foreach (var Image in readReviewImage)
            //{
            //    img.Add(new ReviewModel()
            //    {
            //        Id = Image.Img.Id,
            //        ImageId = Image.Img.ProjectImageId,
            //        Review = Image.Img.Review,
            //        Show = Image.Img.Show,
            //        userName = Image.rr.UserFirstName
            //    });
            //}
            //viewModel.ImagesReview = img;

            foreach (var item in readReviewProject)
            {
                proj.Add(new ReviewModel()
                {
                    Id = item.Project.Id,
                    ImageId = 0,
                    Review = item.Project.Review,
                    Show = item.Project.Show,
                    userName = item.rr.UserFirstName
                });
            }
            viewModel.ProjectReviews = proj;

            return PartialView(viewModel);
        }

        [HttpPost]
        public ActionResult AdminReviewList(FormCollection frm)
        {
            var chckedValues = frm.GetValues("ids");

            return View();
        }

        public JsonResult PublishProjectReview(int id, string Show, string review)
        {
            string returnString = "Success";
            try
            {
                var project = (from proj in context.ProjectReviews
                               where proj.Id == id
                               select proj).FirstOrDefault();

                if (project != null)
                {
                    project.Review = review;
                    project.Show = true;
                    project.flag = false;
                    project.IsAdminApproved = false;
                    project.Publish = true;
                    project.AdminFlag = true;
                    project.UnPublish = false;
                }
                context.SaveChanges();
            }
            catch
            {
                returnString = "Fail";
            }
            return Json(returnString, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UnPublishProjectReviewUser(int id, string Show, string review)
        {
            string returnString = "Success";
            try
            {
                var project = (from proj in context.ProjectReviews
                               where proj.Id == id
                               select proj).FirstOrDefault();

                if (project != null)
                {
                    project.Review = review;
                    project.Show = false;
                    project.flag = false;
                    project.IsAdminApproved = false;
                    project.Publish = false;
                    project.UnPublish = true;
                }
                context.SaveChanges();
            }
            catch
            {
                returnString = "Fail";
            }
            return Json(returnString, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UnPublishProjectReviewAdmin(int id, string Show, string review)
        {
            string returnString = "Success";
            try
            {
                var project = (from proj in context.ProjectReviews
                               where proj.Id == id
                               select proj).FirstOrDefault();

                if (project != null)
                {
                    project.Review = review;
                    project.Show = false;
                    project.flag = false;
                    project.IsAdminApproved = false;
                    project.Publish = false;
                    project.UnPublish = true;
                }
                context.SaveChanges();
            }
            catch
            {
                returnString = "Fail";
            }
            return Json(returnString, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult TotalpblishedReview(string id)
        //{
        //    int uID = Convert.ToInt32(Request.Cookies["UserId"].Value);
        //    int pid = Convert.ToInt32(id);
        //    List<ProjectReview> lstTotal = new List<ProjectReview>();
        //    // string returnString = "Success";
        //    try
        //    {
        //        var project = (from proj in context.ProjectReviews
        //                       where proj.ProjectId == pid && proj.userId == uID && proj.Publish == true
        //                       select proj).AsEnumerable();
        //        foreach (var datat in project)
        //        {
        //            lstTotal.Add(new ProjectReview()
        //            {
        //                Review = datat.Review

        //            });

        //        }

        //    }
        //    catch
        //    {
        //        //  returnString = "Fail";
        //    }
        //    return Json(lstTotal, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult TotalUnpblishedReview(string id)
        {
            int uID = Convert.ToInt32(Request.Cookies["UserId"].Value);
            int pid = Convert.ToInt32(id);
            List<ProjectReview> lstTotal = new List<ProjectReview>();

            try
            {
                var project = (from proj in context.ProjectReviews
                               where proj.ProjectId == pid && proj.userId == uID && proj.UnPublish == true
                               select proj).AsEnumerable();
                foreach (var datat in project)
                {
                    lstTotal.Add(new ProjectReview()
                    {
                        Review = datat.Review

                    });

                }

            }
            catch
            {
                //  returnString = "Fail";
            }
            return Json(lstTotal, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PublishImageReview(int id, string Show, string review)
        {
            string returnString = "Success";
            try
            {
                var project = (from proj in context.ProjectImageVideoComments
                               where proj.Id == id
                               select proj).FirstOrDefault();

                if (project != null)
                {
                    project.Review = review;
                    project.Show = Convert.ToBoolean(Show);
                }
                context.SaveChanges();
            }
            catch
            {
                returnString = "Fail";
            }
            return Json(returnString, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region code for get all user who likes the projects
        public JsonResult GetProjectLikeUserOnProject(int ProjectId)
        {

            List<string> LikedUser = new List<string>();

            LikedUser = (from user in context.RegisterUsers
                         join project in context.ProjectLikeHistories on user.Id equals project.userId
                         where project.ProjectId == ProjectId
                         select user.UserFirstName).ToList();
            var chk = (from b in context.ProjectLikeDisLikes
                       where b.projectId == ProjectId && b.flag == true
                       select b).FirstOrDefault();
            if (chk == null)
            {

            }
            else
            {
                ProjectLikeDisLike v = (from b in context.ProjectLikeDisLikes
                                        where b.projectId == ProjectId && b.flag == true
                                        select b).FirstOrDefault();
                v.flag = false;
                context.SaveChanges();
            }


            return Json(LikedUser, JsonRequestBehavior.AllowGet);
        }
        #endregion\
        #region Give Images
        public class Demo
        {
            public string image { get; set; }
            public bool? link { get; set; }
        }
        public PartialViewResult _deleteProjectImage(int? ProjectId = 0)
        {
            SearchWorldRef search = new SearchWorldRef();
            ProjectSearchModel projectImages = search.ReturnParticularSearchRef(ProjectId);
            return PartialView(projectImages);
        }

        public JsonResult Image(int projectId, int index, string ButtonType)
        {
            var dir = Server.MapPath("/uploads/");
            List<Demo> imagePath = new List<Demo>();
            string returnPath = string.Empty;
            bool? link = false;
            try
            {
                imagePath = (from img in context.ProjectImages where img.ProjectId == projectId select new Demo { image = img.ImagePath, link = img.link }).ToList();

                if (imagePath.Count() > 0)
                {
                    returnPath = imagePath[index].image;
                    link = imagePath[index].link;
                }

            }
            catch
            {
                try
                {
                    if (imagePath.Count() > 0)
                    {
                        int indexReturn;
                        if (ButtonType == "next")
                        {
                            returnPath = imagePath[0].image;
                            link = imagePath[0].link;
                            indexReturn = 0;
                        }
                        else
                        {
                            int count = imagePath.Count();

                            returnPath = imagePath[count - 1].image;
                            link = imagePath[count - 1].link;
                            indexReturn = count - 1;
                        }
                        ImageStructure imgStruct1 = new ImageStructure()
                        {
                            ImagePath = returnPath,
                            Link = link,
                            ImgIndex = indexReturn.ToString()
                        };

                        return Json(imgStruct1, JsonRequestBehavior.AllowGet);
                    }
                }
                catch
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }
                // return Json("", JsonRequestBehavior.AllowGet);
            }

            ImageStructure imgStruct = new ImageStructure()
            {
                ImagePath = returnPath,
                Link = link,
                ImgIndex = index.ToString()
            };

            return Json(imgStruct, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteImages(int? ProjectId)
        {
            if (ProjectId == null)
            {
                ProjectId = Convert.ToInt32(TempData["ProjectId"]);
                TempData.Remove("ProjectId");
            }
            SearchWorldRef search = new SearchWorldRef();
            ProjectSearchModel projectImages = search.ReturnParticularSearchRef(ProjectId);
            return View(projectImages);
        }

        [HttpPost]
        public ActionResult DeleteImages(FormCollection form)
        {
            var chckedValues = form.GetValues("ids");

            if (chckedValues == null)
            {
                TempData.Clear();
                TempData.Add("ErrorMessage", "Please select at least one checkbox");
                TempData.Add("ProjectId", form["ProjectId"].ToString());
                return RedirectToAction("DeleteImages");
            }

            ReadExcel readExcel = new ReadExcel();

            foreach (var checkedValue in chckedValues)
            {
                ProjectImage image = (from img in context.ProjectImages
                                      where img.ImagePath == checkedValue
                                      select img).FirstOrDefault();
                if (image != null)
                {
                    context.ProjectImages.Remove(image);
                    context.SaveChanges();
                }
            }
            TempData.Clear();
            TempData.Add("ErrorMessage", "Images Deleted Successfully");
            return RedirectToAction("ListExcelProjectsUser");
        }
        #endregion
        #region Refer

        public ActionResult Refer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Refer(string EmailAddress, FormCollection frm)
        {
            Email email = new Email();
            int userId = Convert.ToInt32(Request.Cookies["UserId"].Value);

            string Firstname = (from q in context.RegisterUsers
                                where q.Id == userId
                                select q.UserFirstName).FirstOrDefault();

            TempData.Clear();
            string textDisplay = "Email Send Successfully.";
            TempData.Add("ErrorMessage", textDisplay);

            var callbackUrl = Url.Action("worldrefindex", "WorldRef", new { }, protocol: Request.Url.Scheme);
            string Subject = "WorldRef for Engineering";
            string Body = "<div style='font-size:15px; font-family:Calibri Light;'> " + Firstname + " has referred you to participate in the WorldRef.<br/><br/> WorldRef is a platform that gives you maximum exposure to utilize your experiences and achievements in the engineering industry. ";
            // Body = Body + "<br/> To Change your username and password ,please click this link below :<br/><a href = '" + callbackUrl + "'>Click here </a><br/>";
            Body = Body +
                   "<br/><br/> You can search your projects at WorldRef <a href = '" + callbackUrl + "'>here</a>   after uploading them.</div>";
            email.SendMail("", EmailAddress, Subject, Body);

            foreach (var key in frm.AllKeys)
            {
                if (key.Contains("txt"))
                {
                    string valueOfLink = Request.Form[key];

                    if (!string.IsNullOrEmpty(valueOfLink))
                    {
                        email.SendMail("", valueOfLink, Subject, Body);
                    }
                }
            }

            return RedirectToAction("Refer");
        }

        #endregion
        #region code for list of recruiter 07/10/2015 added by rahul shukla
        public ActionResult Recruiters()
        {
            RegisterUserDAO objRegisterusr = new RegisterUserDAO();
            List<RegisterUserDAO> objList = new List<RegisterUserDAO>();
            var list = (from g in context.RegisterUsers where g.OrgName == "gswi" && g.RecruiterFlag == false select g);
            foreach (var data in list)
            {
                objList.Add(new RegisterUserDAO()
                {
                    UserFirstName = data.UserFirstName,
                    Email = data.Email,
                    Id = data.Id

                });
            }

            return View(objList);
        }
        #endregion
        #region code for approve recuiter by uploader and send mail to username and password 07/10/2015 added by rahul shukla
        public JsonResult SendUsernamrePwd(string Uname, string email, int ID)
        {
            RegisterUserDAO obj = new RegisterUserDAO();
            Email objEmail = new Email();
            string userName = objEmail.getUserNo();
            string pwd = objEmail.GeneratePassword();
            RegisterUser objR = (from n in context.RegisterUsers where n.Id == ID select n).First();
            objR.UserNo = userName;
            objR.Password = pwd;
            objR.RecruiterFlag = true;
            context.SaveChanges();
            string Subject = "Welcome to WorldRef";
            string Body = "<div style='font-size:15px; font-family:Calibri Light;'>Thank you for Registering with World Ref.<br/> <br/> Your username is : " + userName + " <br/> Your password is : " + pwd;
            Body = Body + "<br/><br/> In case you wish to change your username and password, please <br/><br/>";
            Body = Body +
                   " WorldRef gives you maximum exposure to utilize your experiences and achievements in the engineering industry.<br/><br/>Please upload your documents on our for access to our associates and customers, and for our review.</div>";// Promotional Library
            objEmail.SendMail(Uname, email, Subject, Body);
            return Json("Success", JsonRequestBehavior.AllowGet);

        }
        #endregion
        #region code for search project added by rahul shukla modified date : 20-10-2015
        public JsonResult MoveToSearchPage(string searchResult)
        {
            TempData.Clear();
            TempData["searchResult"] = string.IsNullOrEmpty(searchResult) ? "" : searchResult;
            return Json("/WorldRef/Search?SearchValue=" + searchResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Search(string SearchValue, FormCollection frm)
        {
            if (TempData.ContainsKey("searchResult"))
            {
                SearchValue = TempData["searchResult"].ToString();
                TempData.Clear();
            }
            else
            {
            }
            if (Request.Cookies["UserRole"] != null)
            {
                var cookieUserRoleVal = Request.Cookies["UserRole"].Value;
                ViewBag.cookieUserRole = cookieUserRoleVal;


            }
            else
            {
                ViewBag.cookieUserRole = "";
            }
            ViewBag.SearchString = SearchValue;
            ViewBag.TypeList = TypeSignUp();
            ViewBag.CountryList = CountrySignUp();
            if (Request.Cookies["UserId"] == null)
            { }
            else
            {
                ViewData["LoginId"] = Request.Cookies["UserId"].Value;
            }
            KnowledgeLogic knowIndustry = new KnowledgeLogic();
            ViewBag.IndustryList = knowIndustry.GetIndustry();
            SignUpWorldRefModel signUpModel = new SignUpWorldRefModel();
            signUpModel = GetAllSignUpDetails();
            return View(signUpModel);
        }
        public JsonResult SearchCountryWise(string searchString, string[] ArrayIndustry, string[] ArrayCountry)
        {
            List<WorldRefSearchModel> FinalResult = new List<WorldRefSearchModel>();
            List<WorldRefSearchModel> listProject = new List<WorldRefSearchModel>();
            SearchWorldRefExtension ext = new SearchWorldRefExtension();
            User_Details objUdetails = new User_Details();
            List<Int32> allProjectsId = new List<Int32>();
            DataTable dtNA = new DataTable();
            List<WorldRefSearchModel> finalRes = new List<WorldRefSearchModel>();
            WorldRefSearchModel obj = new WorldRefSearchModel();

            if (searchString != null && ArrayIndustry == null && ArrayCountry == null)
            {
                FullSearchString = searchString;
            }
            else
            {
                if (ArrayIndustry != null)
                {

                    for (int i = 0; i < ArrayIndustry.Length; i++)
                    {
                        FullSearchString += ArrayIndustry[i].ToString() + ",";
                    }
                    FullSearchString = FullSearchString.Remove(FullSearchString.Length - 1);
                }
                if (ArrayCountry != null)
                {
                    for (int i = 0; i < ArrayCountry.Length; i++)
                    {
                        FullSearchString += ArrayCountry[i].ToString() + ",";
                    }
                    FullSearchString = FullSearchString.Remove(FullSearchString.Length - 1);

                }
                FullSearchString += ' ' + searchString;
            }
          //  string[] splitData = FullSearchString.Split(',', ' ');
            //foreach (string splitVal in splitData)
           // {
            //    listProject = ext.DecideFilter(splitVal);
            listProject = ext.DecideFilter(FullSearchString);
                foreach (WorldRefSearchModel model in listProject)
                {

                    FinalResult.Add(model);

                }
          //  }

            dtNA = ToDataTable<WorldRefSearchModel>(FinalResult);

            for (int ij = 0; ij < dtNA.Rows.Count; ij++)
            {
                for (int ll = 0; ll < FinalResult.Count; ll++)
                {
                    if (dtNA.Rows[ij]["id"].ToString() == FinalResult[ll].id.ToString())
                    {
                        if (allProjectsId.Contains(Convert.ToInt32(FinalResult[ll].id)))
                        { }
                        else
                        {
                            allProjectsId.Add(Convert.ToInt32(FinalResult[ll].id));
                            finalRes.Add(FinalResult[ll]);
                        }
                    }
                }
            }
            if (Request.Cookies["UserRole"] != null)
            {
                string UsrName = Request.Cookies["UName"].Value;
                string uRole = Request.Cookies["UserRole"].Value;
                var data = (from s in context.RegisterUsers
                            where s.UserNo == UsrName && s.UserRole == uRole
                            select new { s.UserNo, s.Email }).FirstOrDefault();
                string IPAdd = string.Empty;
                IPAdd = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(IPAdd))
                    IPAdd = Request.ServerVariables["REMOTE_ADDR"];
                if (string.IsNullOrEmpty(data.Email))
                    objUdetails.EmailId = data.Email;
                objUdetails.LastLogin = DateTime.Now;
                objUdetails.LocationIP = IPAdd;
                objUdetails.UserName = data.UserNo;
                objUdetails.Keywords = searchString;
                context.User_Details.Add(objUdetails);
                context.SaveChanges();
                var cookieUserRoleVal = Request.Cookies["UserRole"].Value;
                ViewBag.cookieUserRole = cookieUserRoleVal;
            }
            else
            {
                string IPAdd = string.Empty;
                IPAdd = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(IPAdd))
                    IPAdd = Request.ServerVariables["REMOTE_ADDR"];
                objUdetails.LastLogin = DateTime.Now;
                objUdetails.LocationIP = IPAdd;
                objUdetails.Keywords = searchString;
                context.User_Details.Add(objUdetails);
                context.SaveChanges();
                ViewBag.cookieUserRole = "";
            }

            ViewBag.count = listProject.Count;
            ViewBag.SearchString = searchString;
            obj.AllData = finalRes;
            return Json(finalRes, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Indusry Filter

        public JsonResult GetAllIndustry()
        {
            SearchWorldRefExtension read = new SearchWorldRefExtension();
            List<WorldRefExcelDataModel> list = read.GetAllIndustry();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region code for get all industry added by rahul shukla Date :16-11-15
        public JsonResult GetAllCountries()
        {
            SearchWorldRef read = new SearchWorldRef();
            List<WorldRefExcelDataModel> list = read.ReturnCountryOnly();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion
        public DataTable ToDataTable<T>(List<WorldRefSearchModel> items)
        {
            DataTable dataTable = new DataTable(typeof(WorldRefSearchModel).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(WorldRefSearchModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (WorldRefSearchModel item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            DataView dv = dataTable.DefaultView;
            dv.Sort = "id desc";
            DataTable sortedDT = dv.ToTable();
            sortedDT.Columns.Add("idCount");
            Int32 cc = 0;
            int mn = 1;
            for (int q = 1; q < sortedDT.Rows.Count; q++)
            {
                if (sortedDT.Rows[q - 1]["id"].ToString() == sortedDT.Rows[q]["id"].ToString())
                {
                    //sortedDT.Rows[q].Delete();
                    //sortedDT.AcceptChanges();
                    sortedDT.Rows[q - 1]["idCount"] = 0;
                    sortedDT.Rows[q]["idCount"] = cc + 2;
                    cc++;
                    mn++;
                }
                else
                {
                    cc = 0;
                    if (mn == 1)
                        sortedDT.Rows[q - 1]["idCount"] = 1;
                    else
                        sortedDT.Rows[q]["idCount"] = 1;
                }
            }

            DataView dview = sortedDT.DefaultView;
            dview.Sort = "idCount desc";
            DataTable finalDt = dview.ToTable();

            DataRow[] rowsArray = finalDt.Select("idCount <> 0");

            //foreach (DataRow dr in rowsArray)
            //{
            //    finalDt.Rows.Add(dr);
            //    finalDt.AcceptChanges();
            //}

            //finalDt.Rows.Add(rowsArray);
            //finalDt.AcceptChanges();

            List<DataRow> rowsToDelete = new List<DataRow>();
            foreach (DataRow DR in finalDt.Rows)
            {
                if (DR["idCount"].ToString() == "0")
                    rowsToDelete.Add(DR);
            }

            foreach (var r in rowsToDelete)
                finalDt.Rows.Remove(r);

            return finalDt;
        }
        private List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private T GetItem<T>(DataRow dr)
        {
            System.Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, !String.IsNullOrEmpty(dr[column.ColumnName].ToString()) ? dr[column.ColumnName] : "", null);
                    else
                        continue;
                }
            }
            return obj;
        }
        public List<SelectListItem> TypeSignUp()
        {
            List<SelectListItem> selectList = new List<SelectListItem>();

            selectList.Add(new SelectListItem() { Text = "I am", Value = "0" });
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
            return selectList;

        }
        public List<SelectListItem> CountrySignUp()
        {
            List<SelectListItem> selectListCountry = new List<SelectListItem>();
            var content = (from p in context.Countries
                           select new { p.CountryID, p.CountryName }).AsEnumerable();
            selectListCountry.Add(new SelectListItem() { Text = "Select Country", Value = "0" });

            foreach (var item in content)
            {
                selectListCountry.Add(new SelectListItem() { Text = item.CountryName, Value = item.CountryName });

            }
            return selectListCountry;
        }
        public PartialViewResult DownloadProfile(int ProjectId)
        {
            string companyProfile = (from rr in context.RegisterUsers
                                     join worldexcel in context.WorldRefExcelDataProjects
                                         on rr.Id equals worldexcel.userid
                                     where worldexcel.id == ProjectId
                                     select rr.PhotoAttach).FirstOrDefault();
            TempData["Path"] = companyProfile;
            return PartialView();
        }
        public JsonResult GetAllComents(int projectId, string usRiD, string UserIp)
        {
            string returnString = "login";
            if (Request.Cookies["UserId"] != null)
            {
                string uid = Request.Cookies["UserId"].Value.ToString();
                if (uid == usRiD)
                {
                    return Json("userProject", JsonRequestBehavior.AllowGet);
                }
                else
                {

                    List<ReviewList> comment = likeRating.GetCommentsOfProject(projectId);


                    var chk = (from b in context.ProjectReviews
                               where b.ProjectId == projectId && b.IsAdminApproved == true && b.Show == true
                               select b).FirstOrDefault();
                    if (chk == null)
                    {

                    }
                    else
                    {
                        ProjectReview v = (from b in context.ProjectReviews
                                           where b.ProjectId == projectId && b.IsAdminApproved == true && b.Show == true
                                           select b).FirstOrDefault();
                        v.flag = false;
                        context.SaveChanges();
                    }

                    return Json(comment, JsonRequestBehavior.AllowGet);
                }


            }

            else if (Request.Cookies["CUserId"] != null)
            {
                UserIp = Request.Cookies["CUserId"].Value.ToString();
                List<ReviewList> comment = likeRating.GetCommentsOfProject(projectId);
                return Json(comment, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(returnString, JsonRequestBehavior.AllowGet);

            }

            //List<ReviewList> comment = likeRating.GetCommentsOfProject(projectId);
            //return Json(comment, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ShowLikeComments(ExternalClassModel obj, string Imageid, string ProjectId, string UserId)
        {
            int pid = Convert.ToInt32(ProjectId);//assign projectid in global var pid. 
            int imgid = Convert.ToInt32(Imageid);//assign Imageid in global var imgid. 
            if (Request.Cookies["UserId"] == null)//check user cookies is null.
            {
                return Json("login", JsonRequestBehavior.AllowGet);//return string.
            }
            else
            {



                int TotalLikeCount = (from s in context.ImageLikeAndComments//find count of total likes from ImageLikeAndComments .
                                      where s.ProjectId == pid && s.ImageId == imgid && s.ImgLikeFlag == true
                                      select s).Count();

                var imgPath = (from d in context.ProjectImages where d.id == imgid select new { d.ImagePath }).FirstOrDefault();//find image path from ProjectImages. 

                obj.TotalLikeCount = TotalLikeCount;
                obj.imgPath = imgPath.ImagePath.ToString();

                List<ExternalClassModel> objComments = new List<ExternalClassModel>();
                var coments = (from g in context.RegisterUsers join f in context.ImageLikeAndComments on g.Id equals f.UserId where f.ImageId == imgid && f.ProjectId == pid && f.CmtImgFlag == true select new { f.ImageTestimonials, g.ProfileAttach }).AsEnumerable();//find ImageTestimonials and profileattach from registeruser and ImageLikeAndComments from join. 
                if (coments.Count() == 0)//check comment count is zero.
                {
                    objComments.Add(new ExternalClassModel()//add totallikecount and imagepath in list.
                    {

                        TotalLikeCount = obj.TotalLikeCount,
                        imgPath = obj.imgPath

                    });

                }
                else
                {
                    foreach (var data in coments)
                    {
                        objComments.Add(new ExternalClassModel()//add data in list.
                        {
                            ImageTestimonials = data.ImageTestimonials,
                            ProfileAttach = data.ProfileAttach,
                            TotalLikeCount = obj.TotalLikeCount,
                            imgPath = obj.imgPath

                        });
                    }
                }
                return Json(objComments, JsonRequestBehavior.AllowGet);//return data.
            }
        }
        public JsonResult ShowLikeCommentsOnVideos(ExternalClassModel obj, string Imageid, string ProjectId, string UserId)
        {
            int pid = Convert.ToInt32(ProjectId);//assign projectid in global var pid. 
            int imgid = Convert.ToInt32(Imageid);//assign Imageid in global var imgid. 
            if (Request.Cookies["UserId"] == null)//check user cookies is null.
            {
                return Json("login", JsonRequestBehavior.AllowGet);//return string.
            }
            else
            {



                int TotalLikeCount = (from s in context.VideosLikeAndComments//find count of total likes from ImageLikeAndComments .
                                      where s.ProjectId == pid && s.VideosId == imgid && s.VideosLikeFlag == true
                                      select s).Count();

                var imgPath = (from d in context.ProjectImages where d.id == imgid select new { d.ImagePath }).FirstOrDefault();//find image path from ProjectImages. 

                obj.TotalLikeCount = TotalLikeCount;
                obj.imgPath = imgPath.ImagePath.ToString();

                List<ExternalClassModel> objComments = new List<ExternalClassModel>();
                var coments = (from g in context.RegisterUsers join f in context.VideosLikeAndComments on g.Id equals f.UserId where f.VideosId == imgid && f.ProjectId == pid && f.cmtVidFlag == true select new { f.VideosTestimonials, g.ProfileAttach }).AsEnumerable();//find VideosTestimonials and profileattach from registeruser and ImageLikeAndComments from join. 
                if (coments.Count() == 0)//check comment count is zero.
                {
                    objComments.Add(new ExternalClassModel()//add totallikecount and imagepath in list.
                    {

                        TotalLikeCount = obj.TotalLikeCount,
                        imgPath = obj.imgPath

                    });

                }
                else
                {
                    foreach (var data in coments)
                    {
                        objComments.Add(new ExternalClassModel()//add data in list.
                        {
                            VideosTestimonials = data.VideosTestimonials,
                            ProfileAttach = data.ProfileAttach,
                            TotalLikeCount = obj.TotalLikeCount,
                            imgPath = obj.imgPath

                        });
                    }
                }
                return Json(objComments, JsonRequestBehavior.AllowGet);//return data.
            }
        }
        #region code for show project location on map added by rahul shukla modified date:06-11-2015
        public JsonResult ShowProjectLocation(string id)
        {
            int pid = Convert.ToInt32(id);

            var projectlocation = (from d in context.WorldRefExcelDataProjects where d.id == pid select d.ProjectLocation).FirstOrDefault();

            return Json(projectlocation, JsonRequestBehavior.AllowGet);

        }
        #endregion
        #region code for add customer crtificates added by rahul shukla modified date : 07-11-2015
        public PartialViewResult AddMoreCertificates(int ProjectId = 0)
        {
            ProjectImageModel projectImage = new ProjectImageModel();
            projectImage.ProjectId = ProjectId;
            TempData["pid"] = ProjectId;
            return PartialView(projectImage);

        }
        [HttpPost]
        public ActionResult AddCertificate(ProjectImageModel ProjectModel, HttpPostedFileBase[] files, FormCollection frm)
        {
            int PrId = Convert.ToInt32(TempData["pid"]);
            ReadExcel read = new ReadExcel();
            List<ProjectImageModel> projectImage = new List<ProjectImageModel>();
            string path = string.Empty;
            string filename = string.Empty;
            string SavePath = string.Empty;
            string Extension = string.Empty;
            try
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase postedFiles = Request.Files[i];

                    if (postedFiles != null)
                    {
                        SavePath = Guid.NewGuid().ToString();
                        Extension = Path.GetExtension(postedFiles.FileName);

                        filename = System.IO.Path.GetFileName(postedFiles.FileName);

                        postedFiles.SaveAs(Server.MapPath("~/uploads/" + SavePath + Extension));

                    }
                }
                TempData.Clear();
                TempData.Add("SuccessMessage", "Certificate uploaded successfully.");
            }
            catch
            {
                TempData.Clear();
                TempData.Add("ErrorMessage", "Certificate Not Uploaded Successfully.Please Try Again");
            }
            read.AdCertificate(PrId, filename);
            return RedirectToAction("ListExcelProjectsUser");
        }
        public PartialViewResult ShowCertificates(int ProjectId)
        {

            string customerCertificates = (from rr in context.ProjectImages

                                           where rr.id == ProjectId
                                           select rr.ProjectCertificates).FirstOrDefault();
            TempData["Path"] = customerCertificates;
            return PartialView();
        }
        #endregion
        #region code for forget username and password added by rahul shukla modified date 08/05/2015
        public ActionResult ChangeUserNameAndPassword(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                userId = Convert.ToInt32(Request.Cookies["UserId"].Value).ToString();
            }
            ViewData["userId"] = userId;
            return View();
        }
        #endregion
        #region code for serachImage added by rahul shukla modified date : 04-11-2015
        public JsonResult MoveToSearchImagePage(string searchResult)
        {
            // TempData.Clear();
            TempData["searchResult"] = string.IsNullOrEmpty(searchResult) ? "" : searchResult;
            return Json("/WorldRef/SearchImages?SearchValue=" + searchResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SearchImages(string SearchValue, FormCollection frm)
        {
            if (TempData.ContainsKey("searchResult"))
            {
                SearchValue = TempData["searchResult"].ToString();
                TempData.Clear();
            }
            else
            {
            }
            if (Request.Cookies["UserRole"] != null)
            {
                var cookieUserRoleVal = Request.Cookies["UserRole"].Value;
                ViewBag.cookieUserRole = cookieUserRoleVal;
            }
            else
            {
                ViewBag.cookieUserRole = "";
            }
            ViewBag.SearchString = SearchValue;
            ViewBag.TypeList = TypeSignUp();
            ViewBag.CountryList = CountrySignUp();
            KnowledgeLogic knowIndustry = new KnowledgeLogic();
            ViewBag.IndustryList = knowIndustry.GetIndustry();
            SignUpWorldRefModel signUpModel = new SignUpWorldRefModel();
            signUpModel = GetAllSignUpDetails();
            return View(signUpModel);
        }
        public JsonResult SearchImagesFilter(string searchString, string[] ArrayIndustry, string[] ArrayCountry)
        {
            TempData.Clear();
            SearchWorldRefExtension search = new SearchWorldRefExtension();
            User_Details objUdetails = new User_Details();
            List<Int32> allProjectsId = new List<Int32>();
            List<WorldRefSearchModel> FinalResult = new List<WorldRefSearchModel>();

            DataTable dtNA = new DataTable();
            List<WorldRefSearchModel> finalRes = new List<WorldRefSearchModel>();

            if (ArrayIndustry != null)
            {

                for (int i = 0; i < ArrayIndustry.Length; i++)
                {
                    FullSearchString += ArrayIndustry[i].ToString() + ",";
                }
                FullSearchString += FullSearchString.Substring(0, FullSearchString.Length - 1);
            }
            if (ArrayCountry != null)
            {
                for (int i = 0; i < ArrayCountry.Length; i++)
                {
                    FullSearchString += ArrayCountry[i].ToString() + ",";
                }
                FullSearchString += FullSearchString.Substring(0, FullSearchString.Length - 1);

            }
            FullSearchString += searchString;
            string[] splitData = FullSearchString.Split(',', ' ');
            foreach (string splitVal in splitData)
            {

                List<WorldRefSearchModel> listImages = search.DecideFilterOnImage(splitVal);

                foreach (WorldRefSearchModel model in listImages)
                {

                    FinalResult.Add(model);

                }
            }
            dtNA = ToDataTable<WorldRefSearchModel>(FinalResult);

            for (int ij = 0; ij < dtNA.Rows.Count; ij++)
            {
                for (int ll = 0; ll < FinalResult.Count; ll++)
                {
                    if (dtNA.Rows[ij]["id"].ToString() == FinalResult[ll].id.ToString())
                    {
                        if (allProjectsId.Contains(Convert.ToInt32(FinalResult[ll].id)))
                        { }
                        else
                        {
                            allProjectsId.Add(Convert.ToInt32(FinalResult[ll].id));
                            finalRes.Add(FinalResult[ll]);
                        }
                    }
                }
            }
            return Json(finalRes, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region code for all icons on grid added by rahul shukla modified date : 05-11-15
        public JsonResult Aski4i(string emailId, string ContactNumber, string CompanyName, int ProjectId, string Cperson, string designation)
        {
            Email email = new Email();
            string userEmail = ConfigurationManager.AppSettings["EMAIL"].ToString();
            string returnString = "Your query send successfully. We will get in touch shortly.";
            if (!string.IsNullOrEmpty(userEmail))
            {
                string Subject = "Project Inquiry";
                string Body = "<div style='font-size:15px; font-family:Calibri Light;'>Below are the details.<br/><br/>Company Name :" + CompanyName + " <br/> Contact Person : " + Cperson + "<br/> Contact Number : " + ContactNumber + " <br/> EmailId : " + emailId + " <br/> Designation : " + designation + "</div>";
                // Body = Body + " In future to Add your new projects ,you may sign in here and may add at 'add new project '[Link]. 'Add New Project' link will ask for the login and will open the company to the page where his all the projects are there.";
                email.SendMail("Admin", userEmail, Subject, Body);
                email.SendMail(Cperson, emailId, Subject, Body);
            }
            else
            {
                returnString = "Error ! Try Again Later";
            }

            return Json(returnString, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddLikeDislike(int ProjectId, string UserID, string LikeOrDisLike, string UserIp)
        {
            string returnString = "login";
            if (Request.Cookies["UserId"] != null)
            {
                UserIp = Request.Cookies["UserId"].Value.ToString();
                if (UserIp == UserID)
                {
                    return Json("userProject", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    returnString = likeRating.AddLikeOrDislike(ProjectId, LikeOrDisLike, UserIp);
                    return Json(returnString, JsonRequestBehavior.AllowGet);
                }
            }
            else if (Request.Cookies["CUserId"] != null)
            {
                UserIp = Request.Cookies["CUserId"].Value.ToString();
                returnString = likeRating.AddLikeOrDislike(ProjectId, LikeOrDisLike, UserIp);
                return Json(returnString, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(returnString, JsonRequestBehavior.AllowGet);

            }
            //return Json("Success", JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddLikeDislikeComment(int ProjectId, string Review, string UserIp)
        {
            string returnString = "Please login in to review this project.";
            if (Request.Cookies["UserId"] == null && Request.Cookies["CUserId"] == null)
            {
                return Json(returnString, JsonRequestBehavior.AllowGet);
            }
            else if (Request.Cookies["CUserId"] != null)
            {
                UserIp = Request.Cookies["CUserId"].Value.ToString();
            }
            else
            {
                UserIp = Request.Cookies["UserId"].Value.ToString();
            }
            returnString = likeRating.AddCommentProject(ProjectId, Convert.ToInt32(UserIp), Review);
            return Json(returnString, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreditThem(int ProjectId, string Userid, string LikeOrDisLike, string UserIp)
        {
            string returnString = "login";
            if (Request.Cookies["UserId"] != null)
            {
                UserIp = Request.Cookies["UserId"].Value.ToString();

                if (UserIp == Userid)
                {
                    return Json("userProject", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    returnString = likeRating.AddCreditRequest(ProjectId, LikeOrDisLike, UserIp);
                    return Json(returnString, JsonRequestBehavior.AllowGet);
                }
            }
            else if (Request.Cookies["CUserId"] != null)
            {
                UserIp = Request.Cookies["CUserId"].Value.ToString();
                returnString = likeRating.AddCreditRequest(ProjectId, LikeOrDisLike, UserIp);
                return Json(returnString, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(returnString, JsonRequestBehavior.AllowGet);

            }
        }

        public JsonResult GetProjectLikeUser(int ProjectId)
        {

            List<string> LikedUser = new List<string>();

            LikedUser = (from user in context.RegisterUsers
                         join project in context.ProjectLikeHistories on user.Id equals project.userId
                         where project.ProjectId == ProjectId
                         select user.UserFirstName).ToList();

            return Json(LikedUser, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddCommentOnImage(int projectId, int ImageId, int UserId, string Review, string UserIp)
        {

            string returnString = "login";
            if (Request.Cookies["UserId"] != null)//check user cookies is not null.
            {

                UserIp = Request.Cookies["UserId"].Value.ToString();//assign userid to userip.
                int uid = Convert.ToInt32(UserIp);
                if (UserIp == UserId.ToString())//check userip and userid is same.
                {
                    return Json("userProject", JsonRequestBehavior.AllowGet);//return userProject action.
                }
                else
                {
                    ImageLikeAndComment objimageComment = new ImageLikeAndComment();
                    if (context.ImageLikeAndComments.Any(o => o.ProjectId == projectId && o.ImageId == ImageId && o.UserId == uid && o.ImageTestimonials == string.Empty))//check any data exist in ImageLikeAndComments. 
                    {
                        ImageLikeAndComment c = (from x in context.ImageLikeAndComments
                                                 where x.ProjectId == projectId && x.ImageId == ImageId && x.UserId == uid
                                                 select x).First();//retrive data from ImageLikeAndComments.
                        c.ImageTestimonials = Review;
                        c.CmtImgFlag = true;
                        context.SaveChanges(); //save data in database table.

                        int id = c.PImageLikeId;
                        var ss = from p in context.ProjectImages
                                 join i in context.ImageLikeAndComments on p.id equals i.ImageId
                                 join r in context.RegisterUsers on ImageId equals r.Id
                                 where i.ProjectId == projectId && i.ImageId == ImageId && i.UserId == uid && i.PImageLikeId == id
                                 select new
                                 {
                                     p.ImagePath,
                                     i.ImageTestimonials,
                                     r.ProfileAttach

                                 };//retrive record from ProjectImages ,ImageLikeAndComments and RegisterUsers tables using join.
                        return Json(ss, JsonRequestBehavior.AllowGet);//return var ss.


                    }

                    else
                    {
                        objimageComment.ProjectId = projectId;
                        objimageComment.ImageId = ImageId;
                        objimageComment.UserId = uid;
                        objimageComment.ImgLikeFlag = false;
                        objimageComment.CmtImgFlag = true;
                        objimageComment.ImageTestimonials = Review;

                        context.ImageLikeAndComments.Add(objimageComment);
                        context.SaveChanges();

                        Int32 id = Convert.ToInt32(objimageComment.PImageLikeId);

                        var query = from p in context.ProjectImages
                                    join i in context.ImageLikeAndComments on p.id equals i.ImageId
                                    join r in context.RegisterUsers on ImageId equals r.Id
                                    where i.PImageLikeId == id
                                    select new
                                    {
                                        p.ImagePath,
                                        i.ImageTestimonials,
                                        r.ProfileAttach

                                    };
                        //retrive record from ProjectImages ,ImageLikeAndComments and RegisterUsers tables using join.




                        return Json(query, JsonRequestBehavior.AllowGet);//return data.


                    }

                }
            }
            else
            {

                return Json(returnString, JsonRequestBehavior.AllowGet);//return string.
            }
        }

        public JsonResult AddCommentOnVideos(int projectId, int ImageId, int UserId, string Review, string UserIp)
        {

            string returnString = "login";
            if (Request.Cookies["UserId"] != null)//check user cookies is not null.
            {

                UserIp = Request.Cookies["UserId"].Value.ToString();//assign userid to userip.
                int uid = Convert.ToInt32(UserIp);
                if (UserIp == UserId.ToString())//check userip and userid is same.
                {
                    return Json("userProject", JsonRequestBehavior.AllowGet);//return userProject action.
                }
                else
                {
                    VideosLikeAndComment objimageComment = new VideosLikeAndComment();
                    if (context.VideosLikeAndComments.Any(o => o.ProjectId == projectId && o.VideosId == ImageId && o.UserId == uid && o.VideosTestimonials == string.Empty))//check any data exist in VideosLikeAndComments. 
                    {
                        VideosLikeAndComment c = (from x in context.VideosLikeAndComments
                                                  where x.ProjectId == projectId && x.VideosId == ImageId && x.UserId == uid
                                                  select x).First();//retrive data from VideosLikeAndComment.
                        c.VideosTestimonials = Review;
                        c.cmtVidFlag = true;
                        context.SaveChanges();//save data in database table.

                        int id = c.PVideosId;

                        var ss = from p in context.ProjectImages
                                 join i in context.VideosLikeAndComments on p.id equals i.VideosId
                                 join r in context.RegisterUsers on ImageId equals r.Id
                                 where i.ProjectId == projectId && i.VideosId == ImageId && i.UserId == uid && i.PVideosId == id
                                 select new
                                 {
                                     p.ImagePath,
                                     i.PVideosId,
                                     r.ProfileAttach

                                 };//retrive record from ProjectImages ,ImageLikeAndComments and RegisterUsers tables using join.
                        return Json(ss, JsonRequestBehavior.AllowGet);//return var ss.


                    }

                    else
                    {
                        objimageComment.ProjectId = projectId;
                        objimageComment.VideosId = ImageId;
                        objimageComment.UserId = uid;
                        objimageComment.VideosLikeFlag = false;
                        objimageComment.cmtVidFlag = true;
                        objimageComment.VideosTestimonials = Review;

                        context.VideosLikeAndComments.Add(objimageComment);
                        context.SaveChanges();

                        Int32 id = Convert.ToInt32(objimageComment.PVideosId);

                        var query = from p in context.ProjectImages
                                    join i in context.VideosLikeAndComments on p.id equals i.VideosId
                                    join r in context.RegisterUsers on ImageId equals r.Id
                                    where i.PVideosId == id
                                    select new
                                    {
                                        p.ImagePath,
                                        i.VideosTestimonials,
                                        r.ProfileAttach

                                    };//retrive record from ProjectImages ,ImageLikeAndComments and RegisterUsers tables using join.




                        return Json(query, JsonRequestBehavior.AllowGet);//return string.


                    }

                }
            }
            else
            {

                return Json(returnString, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Profile URL added by taps on 6th nov 2015
        public ActionResult SecondLevelSignUp()
        {
            SignUpWorldRefModel signModel = new SignUpWorldRefModel();
            //signModel.UserNo = TempData["UserID"].ToString();
            return View();
        }
        [HttpPost]
        public ActionResult SecondLevelSignUp(SignUpWorldRefModel signModel)
        {
            int uid = Convert.ToInt16(TempData["UserID"].ToString());
            RegisterUser abc = (from p in context.RegisterUsers
                                where p.Id == uid
                                select p).Single();
            abc.ProfileUrl = signModel.ProfileUrl;
            context.SaveChanges();
            return RedirectToAction("WorldrefIndex");
        }
        #endregion
        public JsonResult TotalpblishedReview(string id)
        {
            //  int uID = Convert.ToInt32(Request.Cookies["UserId"].Value);
            int pid = Convert.ToInt32(id);
            List<ProjectReview> lstTotal = new List<ProjectReview>();
            // string returnString = "Success";
            try
            {
                var project = (from proj in context.ProjectReviews
                               where proj.ProjectId == pid && proj.Publish == true
                               select proj).AsEnumerable();
                foreach (var datat in project)
                {
                    lstTotal.Add(new ProjectReview()
                    {
                        Review = datat.Review

                    });

                }

            }
            catch
            {
                //  returnString = "Fail";
            }
            return Json(lstTotal, JsonRequestBehavior.AllowGet);
        }
        #region code for serachVideos added by rahul shukla modified date : 16-11-2015
        public JsonResult MoveToSearchVideosPage(string searchResult)
        {
            // TempData.Clear();
            TempData["searchResult"] = string.IsNullOrEmpty(searchResult) ? "" : searchResult;
            return Json("/WorldRef/SearchVideos?SearchValue=" + searchResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SearchVideos(string SearchValue, FormCollection frm)
        {
            if (TempData.ContainsKey("searchResult"))
            {
                SearchValue = TempData["searchResult"].ToString();
                TempData.Clear();
            }
            else
            {
            }
            if (Request.Cookies["UserRole"] != null)
            {
                var cookieUserRoleVal = Request.Cookies["UserRole"].Value;
                ViewBag.cookieUserRole = cookieUserRoleVal;
            }
            else
            {
                ViewBag.cookieUserRole = "";
            }
            ViewBag.SearchString = SearchValue;
            ViewBag.TypeList = TypeSignUp();
            ViewBag.CountryList = CountrySignUp();
            KnowledgeLogic knowIndustry = new KnowledgeLogic();
            ViewBag.IndustryList = knowIndustry.GetIndustry();
            SignUpWorldRefModel signUpModel = new SignUpWorldRefModel();
            signUpModel = GetAllSignUpDetails();
            return View(signUpModel);
        }
        public JsonResult SearchVideosFilter(string searchString, string[] ArrayIndustry, string[] ArrayCountry)
        {
            TempData.Clear();
            SearchWorldRefExtension search = new SearchWorldRefExtension();
            User_Details objUdetails = new User_Details();
            List<Int32> allProjectsId = new List<Int32>();
            List<WorldRefSearchModel> FinalResult = new List<WorldRefSearchModel>();

            DataTable dtNA = new DataTable();
            List<WorldRefSearchModel> finalRes = new List<WorldRefSearchModel>();

            if (ArrayIndustry != null)
            {

                for (int i = 0; i < ArrayIndustry.Length; i++)
                {
                    FullSearchString += ArrayIndustry[i].ToString() + ",";
                }
                FullSearchString += FullSearchString.Substring(0, FullSearchString.Length - 1);
            }
            if (ArrayCountry != null)
            {
                for (int i = 0; i < ArrayCountry.Length; i++)
                {
                    FullSearchString += ArrayCountry[i].ToString() + ",";
                }
                FullSearchString += FullSearchString.Substring(0, FullSearchString.Length - 1);

            }
            FullSearchString += searchString;
            string[] splitData = FullSearchString.Split(',', ' ');
            foreach (string splitVal in splitData)
            {

                List<WorldRefSearchModel> listImages = search.ReturnFilterVideos(splitVal);

                foreach (WorldRefSearchModel model in listImages)
                {

                    FinalResult.Add(model);

                }
            }
            dtNA = ToDataTable<WorldRefSearchModel>(FinalResult);

            for (int ij = 0; ij < dtNA.Rows.Count; ij++)
            {
                for (int ll = 0; ll < FinalResult.Count; ll++)
                {
                    if (dtNA.Rows[ij]["id"].ToString() == FinalResult[ll].id.ToString())
                    {
                        //if (allProjectsId.Contains(Convert.ToInt32(FinalResult[ll].id)))
                        //{ }
                        //else
                        //{
                            allProjectsId.Add(Convert.ToInt32(FinalResult[ll].id));
                            finalRes.Add(FinalResult[ll]);
                        //}
                    }
                }
            }
            return Json(finalRes, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region added by Himanshu
        #region code for View profile page of world ref user

        public JsonResult WorldrefUserProfileDetails()
        {
            WorldRefSearchModel objModel = new WorldRefSearchModel();
            List<WorldRefSearchModel> listExcelProject = new List<WorldRefSearchModel>();
            List<ImageStructure> imageStructure = new List<ImageStructure>();
            List<LinkedInUser> appCredit = new List<LinkedInUser>();
            List<RegisterUserDAO> registeruserProfile = new List<RegisterUserDAO>();

            string profileName = TempData["ProfileName"] != null ? TempData["ProfileName"].ToString() : "";
            TempData.Keep("ProfileName");
            var userId = (from x in context.RegisterUsers
                          where x.ProfileUrl == profileName && x.UserRole == "W"
                          select x.Id).FirstOrDefault();
            int profileid = Convert.ToInt32(userId);
            //profileid = Convert.ToInt16(objModel.id);

            var projectlist = (from project in context.WorldRefExcelDataProjects
                               where project.userid == profileid && project.IsAuthorized == 1
                               select project).AsEnumerable();

            foreach (var project in projectlist)
            {
                imageStructure = new List<ImageStructure>();
                appCredit = new List<LinkedInUser>();
                registeruserProfile = new List<RegisterUserDAO>();
                string organizationName = string.Empty;
                string customer = string.Empty;
                string projectName = string.Empty;
                string type = string.Empty;
                string year = string.Empty;
                string country = string.Empty;
                string emailid = string.Empty;
                string status = string.Empty;

                if (project.IsOrganization == false)
                {
                    organizationName = project.OrganizationName;
                }
                if (project.IsCustomer == false)
                {
                    customer = project.CustomerName;
                }
                if (project.IsProject == false)
                {
                    projectName = project.ProjectName;
                }
                if (project.IsType == false)
                {
                    type = project.Type;
                }
                if (project.IsYear == false)
                {
                    year = project.Year;
                }
                if (project.IsCountry == false)
                {
                    country = project.Country;
                }
                if (project.IsStatus == false)
                {
                    status = project.Status;
                }
                string userType = string.Empty;

                userType = (from rr in context.RegisterUsers where rr.Id == project.userid select rr.Type).FirstOrDefault();

                if (project.IsEmail == false)
                {
                    string emailOfUser = (from rr in context.RegisterUsers where rr.Id == project.userid select rr.Email).FirstOrDefault();

                    emailid = emailOfUser;
                }
                var img = (from img1 in context.ProjectImages
                           where (img1.ProjectId == project.id)
                           select img1).AsEnumerable();

                foreach (var ig in img)
                {
                    imageStructure.Add(new ImageStructure()
                    {
                        ImageName = ig.ImageName,
                        ImagePath = ig.ImagePath,
                        ImgIndex = "0",
                        Link = ig.link
                    });
                }

                var likes = (from like in context.ProjectLikeDisLikes
                             where like.projectId == project.id
                             select like).FirstOrDefault();

                string rating = "0";

                rating = (from rate in context.ProjectRatings
                          where rate.projectId == project.id
                          select rate.rating).FirstOrDefault();

                rating = !string.IsNullOrEmpty(rating) ? rating : "0";
                string qualityrat = "0";
                qualityrat = (from rate in context.ProjectRatings
                              where rate.projectId == project.id
                              select rate.QualityRating).FirstOrDefault();
                qualityrat = !string.IsNullOrEmpty(qualityrat) ? qualityrat : "0";


                string userrating = "0";

                userrating = (from rate in context.ProjectRatings
                              where rate.projectId == project.id
                              select rate.UserRating).FirstOrDefault();

                userrating = !string.IsNullOrEmpty(userrating) ? userrating : "0";

                var pictureUrl = (from r in context.LinkedInUsers
                                  where r.id == project.id && r.CreditM == true
                                  select new { r.picUrl, r.FirstName, r.Designation, r.Industry }).AsEnumerable();

                foreach (var data in pictureUrl)
                {
                    appCredit.Add(new LinkedInUser()
                    {
                        picUrl = data.picUrl,
                        Designation = data.Designation,
                        FirstName = data.FirstName,
                        Industry = data.Industry


                    });
                }

                string Tlikes = "0";
                string TDisLikes = "0";

                if (likes != null)
                {
                    Tlikes = likes.totalLike.ToString();
                    TDisLikes = likes.totalDislike;
                }


                //select userfirstname,company,photoattach from registeruser where profileurl='mohit' and userrole='W'

                var profileDetails = (from d in context.RegisterUsers
                                      where d.ProfileUrl == profileName && d.UserRole == "W"
                                      select new { d.UserFirstName, d.Company, d.PhotoAttach }).AsEnumerable();
                foreach (var data in profileDetails)
                {
                    registeruserProfile.Add(new RegisterUserDAO()
                    {
                        UserFirstName = data.UserFirstName,
                        Company = data.Company,
                        PhotoAttach = data.PhotoAttach

                    });
                }


                dynamic registerField = (from rr in context.RegisterUsers where rr.Id == project.userid select new { rr.Type, rr.Products }).FirstOrDefault();


                int index = listExcelProject.FindIndex(x => x.id == project.id);
                if (index == -1)
                {
                    listExcelProject.Add(new WorldRefSearchModel()
                    {
                        id = project.id,
                        OrganizationName = organizationName,
                        CustomerName = customer,
                        ProjectName = projectName,
                        Type = type,
                        Year = year,
                        Country = country,
                        EmailId = emailid,
                        TotalLikes = Tlikes,
                        TotalDislikes = TDisLikes,
                        ListOfImage = imageStructure,
                        ListOfApprovedCredits = appCredit,
                        Rating = rating,
                        profilelist = registeruserProfile,
                        QualityRating = qualityrat,
                        UserRating = userrating,
                        CustomerIndustryType = project.CustomerIndustryType,
                        Status = status,
                        Description = project.Description,
                        UserType = registerField.Type,
                        CompanyLogo = registerField.Products // company logo
                    });

                }
            }
            // objModel.modeldataList = listExcelProject;
            //objModel.profilelist = registeruserProfile;
            return Json(listExcelProject, JsonRequestBehavior.AllowGet);
        }
        public ActionResult WorldrefUserProfile(string id)
        {
            WorldRefSearchModel obj = new WorldRefSearchModel();
            TempData["ProfileName"] = id;
            TempData.Keep();
            if (string.IsNullOrEmpty(id))
            {
                return View(obj);
            }
            else
            {
                GetProfileData(id, out obj);
                ViewBag.id = obj.id;
            }

            return View(obj);


        }

        // [ChildActionOnly]
        public ActionResult WorldrefUserProfileEdit(string profilename)
        {
            WorldRefSearchModel obj = new WorldRefSearchModel();
            TempData["ProfileName"] = profilename;

            GetProfileData(profilename, out obj);

            if (string.IsNullOrEmpty(obj.Rating))
            {
                ViewBag.id = 0;
            }
            return View(obj);
        }
        /// <summary>
        /// it used to extract data from db which is used by the view to display data to user
        /// </summary>
        /// <param name="profilename">used to extract data from database registeruser table</param>
        /// <param name="obj">it is the WorldRefSearchModel object which is further used in view to extract data</param>
        private void GetProfileData(string profilename, out WorldRefSearchModel obj)
        {

            obj = new WorldRefSearchModel();
            var result = (from _reguser in context.RegisterUsers                                          //retrive basic information from db based on profilename 
                          join _useredit in context.UserProfile_Edit                                      //doing join on RegisterUsers and UserProfile_Edit
                          on _reguser.Id equals _useredit.UserId into joineddata                          //basen on id and userid respectively
                          from _useredit in joineddata.DefaultIfEmpty()
                          where _reguser.ProfileUrl == profilename
                          select new
                          {
                              _reguser.Id,
                              _reguser.UserFirstName,
                              _reguser.Company,
                              _reguser.ProfileAttach,
                              _useredit.FinancialRating,
                              _useredit.ISO,
                              _useredit.Iso_FileName,
                              _useredit.Iso_ExpiryDate,
                              _useredit.OHSOS,
                              _useredit.Ohsos_FileName,
                              _useredit.Ohsos_ExpiryDate,
                              _useredit.EmployeeNo,
                              _useredit.Revenue,
                              _useredit.FinancialIssuedBy,
                              _reguser.CountryName
                          }).ToList();
            int userid = result[0].Id;
            obj.id = userid;
            obj.UserFirstName = result[0].UserFirstName;
            obj.Company = result[0].Company;
            obj.ProfileAttach = result[0].ProfileAttach;
            obj.CountryName = result[0].CountryName;
            obj.id = userid;
            var industry = (from ind in context.Industries                                    //retrive industry name from db based on contentid of registeruser table 
                            where ind.IndustriesID == userid
                            select new { ind.IndustriesName }).FirstOrDefault();
            obj.IndustriesName = industry.IndustriesName;
            if (result[0].FinancialRating != null)                                                         //fill the WorldRefSearchModel object from registeruser table
            {
                obj.Rating = result[0].FinancialRating;
                obj.ISO = result[0].ISO;
                obj.Iso_FileName = result[0].Iso_FileName;
                obj.Iso_ExpiryDate = result[0].Iso_ExpiryDate;
                obj.OHSOS = result[0].OHSOS;
                obj.Ohsos_FileName = result[0].Ohsos_FileName;
                obj.Ohsos_ExpiryDate = result[0].Ohsos_ExpiryDate;
                obj.EmployeeNo = result[0].EmployeeNo;
                obj.Revenue = result[0].Revenue;
                obj._ratingid = Convert.ToInt16(result[0].FinancialIssuedBy);
                //obj.UserEdit_id = result[0].ID;
                ViewBag.id = result[0].Id;
                int a = Convert.ToInt32(result[0].Id);

                var UserReward = (from data in context.User_Rewards                                        //retrive the rewards of user from user_ reward table 
                                  where data.Userid == a
                                  select new { data.Id, data.RewardName, data.RewardFileName }).AsEnumerable();
                foreach (var _user in UserReward)
                {
                    obj._userreward.Add(new Models.Rewards
                    {
                        Id = _user.Id,
                        RewardName = _user.RewardName,
                        RewardFileName = _user.RewardFileName
                    });
                }

                var Certificate = (from _pcert in context.User_ProductCertificates                              //retrive the product certificates information of user
                                   join _pcertvalue in context.User_Certificate_DrpValue                        //by doing join on User_ProductCertificate table ,User_Certificate_DrpValue
                                   on _pcert.CertificateName equals _pcertvalue.Id                              //and User_Certificate_DrpValue_Common as we want the combine data
                                   join _pissuedby in context.User_Certificate_DrpValue_Common                  //of all this table.
                                   on _pcert.CertificateIssuedBy equals _pissuedby.Id
                                   select new { _pcert.Id, _pcert.CertificateAttactedFile, _pcert.Startdate, _pcert.Enddate, _pcertvalue.Name, Issuedname = _pissuedby.Name, _pcert.CertificateName, _pcert.CertificateIssuedBy, _pcert.CameFrom });

                var ProductCertificate = (from _product in Certificate
                                          where _product.CameFrom == "ProductCertificate"
                                          select _product);

                foreach (var data in ProductCertificate)
                {
                    obj._productcertificate.Add(new ProductCertificate_Edit
                    {
                        Id = data.Id,
                        CertificateName = data.Name,
                        CertificateIssuedBy = data.Issuedname,
                        CertificateAttactedFile = data.CertificateAttactedFile,
                        Startdate = data.Startdate,
                        Enddate = data.Enddate,
                        CertificateId = data.CertificateName,
                        IssuedId = data.CertificateIssuedBy
                    });
                }

                var ManagementCertificate = (from _product in Certificate                                       //retrive the management certificates of user
                                             where _product.CameFrom == "ManagementCertificate"                 //from Certificate extract above
                                             select _product);

                foreach (var data in ManagementCertificate)
                {
                    obj._managementcertificateedit.Add(new ManagementCertificate_Edit
                    {
                        Id = data.Id,
                        CertificateName = data.Name,
                        CertificateIssuedBy = data.Issuedname,
                        CertificateAttactedFile = data.CertificateAttactedFile,
                        Startdate = data.Startdate,
                        Enddate = data.Enddate,
                        CertificateId = data.CertificateName,
                        IssuedId = data.CertificateIssuedBy
                    });
                }
                string financeid = obj._ratingid.ToString();
                var financeissuedby = (from _d in obj.GetFinancialRating
                                       where _d.Value == financeid
                                       select _d.Text).FirstOrDefault();

                ViewBag.financeissuedby = financeissuedby;
                int percentage = PercentageComplete(obj);
                ViewBag.percentage = percentage;

            }
        }
        /// <summary>
        /// calculate the percentage of profile page completed by the user
        /// </summary>
        /// <param name="obj">WorldRefSearchModel object</param>
        /// <returns>it returns integar value of total percentage</returns>
        private int PercentageComplete(WorldRefSearchModel obj)
        {
            int percentage = 0;
            if (!string.IsNullOrEmpty(obj.Rating))
                percentage = 10;
            if (obj._userreward.Count > 0)
                percentage = percentage + 30;
            if (obj._productcertificate.Count > 0)
                percentage = percentage + 20;
            if (obj._managementcertificateedit.Count > 0)
                percentage = percentage + 20;
            if (!string.IsNullOrEmpty(obj.EmployeeNo))
                percentage = percentage + 10;
            if (!string.IsNullOrEmpty(obj.Revenue))
                percentage = percentage + 10;

            return percentage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fc"></param>
        /// <param name="registeruserProfile"></param>
        /// <param name="ISO_file"></param>
        /// <param name="OHSAS_file"></param>
        /// <param name="AddReward"></param>
        /// <param name="AddCertificate"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult WorldrefUserProfileEdit(FormCollection fc, WorldRefSearchModel registeruserProfile, HttpPostedFileBase ISO_file, HttpPostedFileBase OHSAS_file, HttpPostedFileBase[] AddReward, HttpPostedFileBase[] AddProductCertificate, HttpPostedFileBase[] AddManagementCertificate)
        {
            User_Rewards _userreward = new User_Rewards();
            User_Certificates _usercertificates = new User_Certificates();
            User_ProductCertificates _userproductcertificates = new User_ProductCertificates();
            string newName = string.Empty;
            string isofilename = string.Empty;
            string isoexirydate = string.Empty;
            string ohsasfilename = string.Empty;
            string ohsasexpirydate = string.Empty;
            int id = 0;
            int k = 0;
            int g = 0;
            if (registeruserProfile.id == 0)
            {
                return View(registeruserProfile);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    id = Convert.ToInt16(registeruserProfile.id);

                    if (fc["hdnreward"] != "0")
                    {
                        int rewardid = Convert.ToInt16(fc["hdnreward"]);
                        if (AddReward != null)
                        {
                            foreach (var fl1 in AddReward)
                            {
                                if (fl1 != null)
                                {
                                    newName = DateTime.Now.Ticks.ToString();
                                    string Extension = Path.GetExtension(fl1.FileName);
                                    string filename = System.IO.Path.GetFileName(fl1.FileName);
                                    var name = fl1.FileName.Split('.');
                                    newName = newName + name[0] + "_AddReward" + Extension;
                                    fl1.SaveAs(Server.MapPath("~/uploads/AddReward/" + newName));
                                }
                            }
                        }
                        User_Rewards _Updatereward = context.User_Rewards.First(x => x.Id == rewardid);
                        if (!string.IsNullOrEmpty(newName))
                        {
                            _Updatereward.RewardFileName = newName;
                        }
                        _Updatereward.RewardName = (fc["Rewards0"] != null ? fc["Rewards0"].ToString() : "");
                        _Updatereward.ModifiedDate = DateTime.Now;
                        context.SaveChanges();
                    }
                    else
                    {
                        if (AddReward != null)
                        {
                            foreach (var fl1 in AddReward)
                            {
                                if (fl1 != null)
                                {
                                    newName = DateTime.Now.Ticks.ToString();
                                    string Extension = Path.GetExtension(fl1.FileName);
                                    string filename = System.IO.Path.GetFileName(fl1.FileName);
                                    var name = fl1.FileName.Split('.');
                                    newName = newName + name[0] + "_AddReward" + Extension;
                                    fl1.SaveAs(Server.MapPath("~/uploads/AddReward/" + newName));
                                    _userreward.Userid = id;
                                    _userreward.RewardName = (fc["Rewards" + k + ""] != null ? fc["Rewards" + k + ""].ToString() : "");
                                    _userreward.RewardFileName = newName;
                                    _userreward.CreatedDate = DateTime.Now;
                                    context.User_Rewards.Add(_userreward);
                                    context.SaveChanges();
                                    k++;
                                }
                            }
                        }
                    }

                    if (fc["hdnproductcertificate"] != "0")
                    {
                        newName = "";
                        int productcertificateid = Convert.ToInt16(fc["hdnproductcertificate"]);
                        if (AddProductCertificate != null)
                            foreach (var fl1 in AddProductCertificate)
                            {
                                if (fl1 != null)
                                {
                                    newName = DateTime.Now.Ticks.ToString();
                                    string Extension = Path.GetExtension(fl1.FileName);
                                    string filename = System.IO.Path.GetFileName(fl1.FileName);
                                    var name = fl1.FileName.Split('.');
                                    newName = newName + name[0] + "AddCertificate" + Extension;
                                    fl1.SaveAs(Server.MapPath("~/uploads/AddCertificate/" + newName));
                                }
                                User_ProductCertificates userproductcertificates = context.User_ProductCertificates.First(x => x.Id == productcertificateid);
                                if (!string.IsNullOrEmpty(newName))
                                {
                                    userproductcertificates.CertificateAttactedFile = newName;
                                }
                                userproductcertificates.CertificateName = Convert.ToInt32(fc["ProductCertificateId" + g + ""].ToString());
                                userproductcertificates.CertificateIssuedBy = Convert.ToInt32(fc["issuedby" + g + ""].ToString());
                                userproductcertificates.Startdate = (fc["startproductcertificate" + g + ""] != null ? fc["startproductcertificate" + g + ""].ToString() : "");
                                userproductcertificates.Enddate = (fc["endproductcertificate" + g + ""] != null ? fc["endproductcertificate" + g + ""].ToString() : "");
                                userproductcertificates.ModifiedDate = DateTime.Now;
                                context.SaveChanges();
                            }
                    }
                    else
                    {
                        if (AddProductCertificate != null)
                        {
                            foreach (var fl1 in AddProductCertificate)
                            {
                                if (fl1 != null)
                                {
                                    newName = DateTime.Now.Ticks.ToString();
                                    string Extension = Path.GetExtension(fl1.FileName);
                                    string filename = System.IO.Path.GetFileName(fl1.FileName);
                                    var name = fl1.FileName.Split('.');
                                    newName = newName + name[0] + "AddCertificate" + Extension;
                                    fl1.SaveAs(Server.MapPath("~/uploads/AddCertificate/" + newName));
                                    _userproductcertificates.Userid = id;
                                    _userproductcertificates.CameFrom = "ProductCertificate";
                                    _userproductcertificates.CertificateName = Convert.ToInt32(fc["ProductCertificateId" + g + ""].ToString());
                                    _userproductcertificates.CertificateIssuedBy = Convert.ToInt32(fc["issuedby" + g + ""].ToString());
                                    _userproductcertificates.CertificateAttactedFile = newName;
                                    _userproductcertificates.Startdate = (fc["startproductcertificate" + g + ""] != null ? fc["startproductcertificate" + g + ""].ToString() : "");
                                    _userproductcertificates.Enddate = (fc["endproductcertificate" + g + ""] != null ? fc["endproductcertificate" + g + ""].ToString() : "");
                                    _userproductcertificates.CreatedDate = DateTime.Now;
                                    context.User_ProductCertificates.Add(_userproductcertificates);
                                    context.SaveChanges();
                                    g++;
                                }
                            }
                        }

                    }

                    g = 0;

                    if (fc["hdnmanagementcertificate"] != "0")
                    {
                        newName = "";
                        int managementcertificateid = Convert.ToInt16(fc["hdnmanagementcertificate"]);
                        if (AddManagementCertificate != null)
                            foreach (var fl1 in AddManagementCertificate)
                            {
                                if (fl1 != null)
                                {
                                    newName = DateTime.Now.Ticks.ToString();
                                    string Extension = Path.GetExtension(fl1.FileName);
                                    string filename = System.IO.Path.GetFileName(fl1.FileName);
                                    var name = fl1.FileName.Split('.');
                                    newName = newName + name[0] + "AddCertificate" + Extension;
                                    fl1.SaveAs(Server.MapPath("~/uploads/AddCertificate/" + newName));
                                }
                                User_ProductCertificates userproductcertificates = context.User_ProductCertificates.First(x => x.Id == managementcertificateid);
                                if (!string.IsNullOrEmpty(newName))
                                {
                                    userproductcertificates.CertificateAttactedFile = newName;
                                }
                                userproductcertificates.CertificateName = Convert.ToInt32(fc["ManagementCertificateId" + g + ""].ToString());
                                userproductcertificates.CertificateIssuedBy = Convert.ToInt32(fc["managedissuedby" + g + ""].ToString());
                                userproductcertificates.Startdate = (fc["startmanagementcertificate" + g + ""] != null ? fc["startmanagementcertificate" + g + ""].ToString() : "");
                                userproductcertificates.Enddate = (fc["endmanagementcertificate" + g + ""] != null ? fc["endmanagementcertificate" + g + ""].ToString() : "");
                                userproductcertificates.ModifiedDate = DateTime.Now;
                                context.SaveChanges();
                            }
                    }
                    else
                    {
                        if (AddManagementCertificate != null)
                        {
                            foreach (var fl1 in AddManagementCertificate)
                            {
                                if (fl1 != null)
                                {
                                    newName = DateTime.Now.Ticks.ToString();
                                    string Extension = Path.GetExtension(fl1.FileName);
                                    string filename = System.IO.Path.GetFileName(fl1.FileName);
                                    var name = fl1.FileName.Split('.');
                                    newName = newName + name[0] + "AddCertificate" + Extension;
                                    fl1.SaveAs(Server.MapPath("~/uploads/AddCertificate/" + newName));
                                    _userproductcertificates.Userid = id;
                                    _userproductcertificates.CameFrom = "ManagementCertificate";
                                    _userproductcertificates.CertificateName = Convert.ToInt32(fc["ManagementCertificateId" + g + ""].ToString());
                                    _userproductcertificates.CertificateIssuedBy = Convert.ToInt32(fc["managedissuedby" + g + ""].ToString());
                                    _userproductcertificates.CertificateAttactedFile = newName;
                                    _userproductcertificates.Startdate = (fc["startmanagementcertificate" + g + ""] != null ? fc["startmanagementcertificate" + g + ""].ToString() : "");
                                    _userproductcertificates.Enddate = (fc["endmanagementcertificate" + g + ""] != null ? fc["endmanagementcertificate" + g + ""].ToString() : "");
                                    _userproductcertificates.CreatedDate = DateTime.Now;
                                    context.User_ProductCertificates.Add(_userproductcertificates);
                                    context.SaveChanges();
                                    g++;
                                }
                            }
                        }

                    }

                    //if (AddCertificate != null)
                    //{
                    //    foreach (var fl1 in AddCertificate)
                    //    {
                    //        if (fl1 != null)
                    //        {
                    //            newName = DateTime.Now.Ticks.ToString();
                    //            string Extension = Path.GetExtension(fl1.FileName);
                    //            string filename = System.IO.Path.GetFileName(fl1.FileName);
                    //            var name = fl1.FileName.Split('.');
                    //            newName = newName + name[0] + "AddCertificate" + Extension;
                    //            fl1.SaveAs(Server.MapPath("~/uploads/AddCertificate/" + newName));
                    //            _usercertificates.Userid = id;
                    //            _usercertificates.CertificateName = (fc["Certificate" + g + ""] != null ? fc["Certificate" + g + ""].ToString() : "");
                    //            _usercertificates.CertificateFileName = newName;
                    //            _usercertificates.CreatedDate = DateTime.Now;
                    //            context.User_Certificates.Add(_usercertificates);
                    //            context.SaveChanges();
                    //            g++;
                    //        }
                    //    }
                    ////}
                    //if (ISO_file != null)
                    //{
                    //    newName = DateTime.Now.Ticks.ToString();
                    //    string Extension = Path.GetExtension(ISO_file.FileName);
                    //    string filename = System.IO.Path.GetFileName(ISO_file.FileName);
                    //    var name = ISO_file.FileName.Split('.');
                    //    newName = newName + name[0] + "_ISO";
                    //    ISO_file.SaveAs(Server.MapPath("~/uploads/ISO/" + newName + Extension));
                    //    isofilename = newName + Extension;
                    //    //isoexirydate = fc["datepicker"].ToString();
                    //}

                    //if (OHSAS_file != null)
                    //{
                    //    newName = DateTime.Now.Ticks.ToString();
                    //    string Extension = Path.GetExtension(OHSAS_file.FileName);
                    //    string filename = System.IO.Path.GetFileName(OHSAS_file.FileName);
                    //    var name = OHSAS_file.FileName.Split('.');
                    //    newName = newName + name[0] + "_OHSAS";
                    //    OHSAS_file.SaveAs(Server.MapPath("~/uploads/OHSAS/" + newName + Extension));
                    //    ohsasfilename = newName + Extension;
                    //    // ohsasexpirydate = fc["fileUpOHSAS"].ToString();
                    //}


                    //context.sp_insert_update_UserProfile_Edit(registeruserProfile.Rating, fc["ISO_File"].ToString(), fc["OHSAS_File"].ToString(), registeruserProfile.EmployeeNo, registeruserProfile.Revenue, id.ToString()
                    //    , (string.IsNullOrEmpty(isofilename) ? registeruserProfile.Iso_FileName : isofilename), registeruserProfile.Iso_ExpiryDate,
                    //    (string.IsNullOrEmpty(ohsasfilename) ? registeruserProfile.Ohsos_FileName : ohsasfilename), registeruserProfile.Ohsos_ExpiryDate, "");


                    context.sp_insert_update_UserProfile_Edit(registeruserProfile.Rating, "", "", registeruserProfile.EmployeeNo, registeruserProfile.Revenue, id.ToString()
                            , "", "", "", "", "", Convert.ToInt16(registeruserProfile._ratingid));

                    return RedirectToAction("WorldrefUserProfileEdit", "WorldRef", new { profilename = TempData["ProfileName"].ToString() });
                }

            }

            return View(registeruserProfile);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult RemoveReward(string id, string camefrom)
        {

            if (camefrom == "Reward")
            {
                var userreward = new User_Rewards { Id = Convert.ToInt32(id) };
                context.User_Rewards.Attach(userreward);
                context.User_Rewards.Remove(userreward);
                context.SaveChanges();
            }
            if (camefrom == "Certificate")
            {
                var usercertificate = new User_ProductCertificates { Id = Convert.ToInt32(id) };
                context.User_ProductCertificates.Attach(usercertificate);
                context.User_ProductCertificates.Remove(usercertificate);
                context.SaveChanges();
            }
            return Json("1", JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetProductcertification()
        {
            //if (TempData["Product"] == null)
            //{
            var result = (from data in context.User_Certificate_DrpValue
                          where data.CertificateType == "Product"
                          select new { data.Id, data.Name, data.Title });
            // TempData["Product"] = result;
            //}

            List<ProductCertificate> _productcertificate = new List<ProductCertificate>();
            foreach (var res in result)
            {
                _productcertificate.Add(new ProductCertificate
                {
                    Id = res.Id,
                    Title = res.Title,
                    Name = res.Name
                });
            }
            return Json(_productcertificate, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetManagementcertification()
        {

            var result = (from data in context.User_Certificate_DrpValue
                          where data.CertificateType == "Management"
                          select new { data.Id, data.Name, data.Title });


            List<ManagementCertificate> _managementcertificate = new List<ManagementCertificate>();
            foreach (var res in result)
            {
                _managementcertificate.Add(new ManagementCertificate
                {
                    Id = res.Id,
                    Title = res.Title,
                    Name = res.Name
                });
            }
            return Json(_managementcertificate, JsonRequestBehavior.AllowGet);
        }
        public JsonResult IssuedBy()
        {
            var result = (from data in context.User_Certificate_DrpValue_Common
                          select new { data.Id, data.Name, data.Title });
            List<IssuedCertificate> _issuedcertificate = new List<IssuedCertificate>();
            foreach (var res in result)
            {
                _issuedcertificate.Add(new IssuedCertificate
                {
                    Id = res.Id,
                    Title = res.Title,
                    Name = res.Name
                });
            }
            return Json(_issuedcertificate, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
        #region code for google and facebook  login on worldref added by rahul shukla Date :17-11-15

        [ValidateInput(false)]
        public ActionResult FacebookWorldRef()
        {
            try
            {
                var fb = new FacebookClient();
                var loginUrl = fb.GetLoginUrl(new
                {
                    client_id = ConfigurationManager.AppSettings["facebookAppID"].ToString(),
                    client_secret = ConfigurationManager.AppSettings["facebookAppSecret"].ToString(),
                    redirect_uri = "http://localhost:57345/WorldRef/FacebookCallback?rturl=",
                    //redirect_uri = "http://182.73.96.52:94/WorldRef/FacebookCallback?rturl=",
                    response_type = "code",
                    scope = "email,user_friends" // Add other permissions as needed
                });

                return Redirect(loginUrl.AbsoluteUri);
            }
            catch (Exception ex)
            {
                return Redirect("/login");
            }
        }


        [ValidateInput(false)]
        public ActionResult FacebookCallback(string code)
        {
            try
            {
              var fb = new FacebookClient();
                dynamic result = fb.Post("oauth/access_token", new
                {
                    client_id = ConfigurationManager.AppSettings["facebookAppID"].ToString(),
                    client_secret = ConfigurationManager.AppSettings["facebookAppSecret"].ToString(),
                    grant_type = "authorization_code",
                    redirect_uri = Request.Url.AbsoluteUri,
                    code = code
                });

                var accessToken = result.access_token;

                // Store the access token in the session
                System.Web.HttpContext.Current.Session["AccessToken"] = accessToken;

                // update the facebook client with the access token so 
                // we can make requests on behalf of the user
                fb.AccessToken = accessToken;

                try
                {
                    // Get the user's information
                    dynamic me = fb.Get("me?fields=name,first_name,last_name,id,email,birthday,gender,link,picture");
                    string UserNo = getUserNo();
                    string password = GeneratePassword();
                    RegisterUser objregister = new RegisterUser();
                    objregister.UserFirstName = me.first_name != "" ? me.first_name : "";
                    objregister.Email = me.email != "" ? me.email : "";
                    objregister.UserLastName = me.last_name != "" ? me.last_name : "";
                    objregister.UserNo = UserNo;
                    objregister.Password = password;
                    objregister.UserRole = "F";
                    CheckCookie(objregister, "F", UserNo);
                    return RedirectToAction("Worldrefindex");
                }
                catch
                {
                }
            }
            catch
            {
            }
            return View();
        }



        public ActionResult oauth2callback(string state, string code, string error, string rturl)
        {
            string clientID = ConfigurationManager.AppSettings["gmailclientid"].ToString().Trim();
            string clientSecret = ConfigurationManager.AppSettings["gmailclientsecret"].ToString().Trim();
            string redirectURIs = ConfigurationManager.AppSettings["gmailredirecturl"].ToString().Trim();
            GoogleId gId;
            string url = string.Empty;
            if (error != null)
            {
                ViewBag.error = error;
                return RedirectToAction("/Worldrefindex");
            }
            GoogleResponse responseFromServer = new GoogleResponse();
            try
            {
                HttpWebRequest requestToken = (HttpWebRequest)WebRequest.Create("https://accounts.google.com/o/oauth2/token");
                requestToken.Method = "POST";
                string postData = string.Format(
                    "code={0}&client_id={1}&client_secret={2}&redirect_uri={3}&grant_type=authorization_code",
                    code, clientID, clientSecret, redirectURIs
                );
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                requestToken.ContentType = "application/x-www-form-urlencoded";
                requestToken.ContentLength = byteArray.Length;
                using (Stream dataStream = requestToken.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                    WebResponse response = null;
                    try
                    {
                        response = requestToken.GetResponse();
                    }
                    catch (WebException ex)
                    {
                        //log.LogMe(ex);
                        return RedirectPermanent("/Worldrefindex");
                    }

                    if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            responseFromServer = Deserialise<GoogleResponse>(reader.ReadToEnd());
                            reader.Close();
                            dataStream.Close();
                            response.Close();
                        }
                    }
                    else
                    {
                        dataStream.Close();
                        response.Close();
                        ViewBag.error = ((HttpWebResponse)response).StatusDescription;
                        return RedirectToAction("/Worldrefindex");
                    }
                }
                if (responseFromServer != null && !string.IsNullOrEmpty(responseFromServer.access_token))
                {
                    url = string.Format("https://www.googleapis.com/oauth2/v1/userinfo?access_token={0}", responseFromServer.access_token);

                    HttpWebRequest requestUserInfo = (HttpWebRequest)WebRequest.Create(url);
                    HttpWebResponse responseUserInfo = null;
                    try
                    {
                        responseUserInfo = (HttpWebResponse)requestUserInfo.GetResponse();
                    }
                    catch (WebException ex)
                    {

                        return RedirectPermanent("/Worldrefindex");
                    }
                    if (((HttpWebResponse)responseUserInfo).StatusCode == HttpStatusCode.OK)
                    {
                        using (Stream receiveStream = responseUserInfo.GetResponseStream())
                        {
                            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                            using (StreamReader readStream = new StreamReader(receiveStream, encode))
                            {
                                gId = Deserialise<GoogleId>(readStream.ReadToEnd());
                                responseUserInfo.Close();
                                readStream.Close();

                                #region codeforgettingooglefriendlist

                                //try
                                //{
                                //    List<FetchContactsModel> contactDetails = new List<FetchContactsModel>();
                                //    string App_Name = "MyNetwork Web Application!";
                                //    ContactsQuery query = new ContactsQuery(ContactsQuery.CreateContactsUri("default"));
                                //    RequestSettings rs = new RequestSettings(App_Name, parameters);
                                //    rs.AutoPaging = true;
                                //    ContactsRequest cr = new ContactsRequest(rs);
                                //    Feed<Google.Contacts.Contact> f = cr.GetContacts();

                                //    int count = 0;
                                //    if (f.Entries != null)
                                //    {
                                //        Feed<Google.Contacts.Contact> feed = cr.GetContacts();
                                //        foreach (Google.Contacts.Contact entry in feed.Entries)
                                //        {
                                //            FetchContactsModel contact = new FetchContactsModel
                                //            {
                                //                Name = !string.IsNullOrEmpty(entry.Name.FullName) ? entry.Name.FullName : "",
                                //                EmailId = entry.Emails.Count >= 1 ? entry.Emails[0].Address : "",
                                //                SecondaryEmail = entry.Emails.Count >= 2 ? entry.Emails[1].Address : "",
                                //                Phonenumber = entry.Phonenumbers.Count >= 1 ? entry.Phonenumbers[0].Value : "",
                                //                Source = "Wap",
                                //            };

                                //            contactDetails.Add(contact);
                                //        }
                                //    }

                                //    TempData["gousers"] = contactDetails;
                                //}
                                //catch (Exception ex) { }
                                #endregion

                                string UserNo = getUserNo();
                                string password = GeneratePassword();
                                RegisterUser objregister = new RegisterUser();
                                objregister.UserFirstName = !string.IsNullOrEmpty(gId.given_name) ? gId.given_name.Trim() : "";
                                objregister.Email = !string.IsNullOrEmpty(gId.email) ? gId.email.Trim() : "";
                                objregister.UserLastName = !string.IsNullOrEmpty(gId.family_name) ? gId.family_name.Trim() : "";
                                objregister.UserNo = UserNo;
                                objregister.Password = password;
                                objregister.UserRole = "G";
                                CheckCookie(objregister, "G", UserNo);
                                return RedirectToAction("Worldrefindex");

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return View();
        }

        public static T Deserialise<T>(string json)
        {
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                var serialiser = new DataContractJsonSerializer(typeof(T));
                return (T)serialiser.ReadObject(ms);
            }
        }


        public void CheckCookie(RegisterUser objregister, string logintype, string UserNo)
        {
            string email = objregister.Email != "" ? objregister.Email : "";
            var userId = (from x in context.RegisterUsers
                          where x.Email == email && x.UserRole == logintype
                          select x.Email).Count();


            var model = (from x in context.RegisterUsers
                         where x.Email == email && x.UserRole == logintype
                         select new { x.UserNo, x.Id, x.UserRole }).SingleOrDefault();

            if (userId > 0)
            {
                //var model = (from x in context.RegisterUsers
                //             where x.Email == email && x.UserRole == logintype
                //             select new { x.UserNo, x.Id, x.UserRole }).SingleOrDefault();
                string UserNumber = model.UserNo.ToString();
                string Uid = model.Id.ToString();
                string urole = model.UserRole.ToString();
                string UsName = UserNumber;
                HttpCookie userNoCookies = new HttpCookie("UserName");
                userNoCookies.Value = UsName;
                Response.Cookies.Add(userNoCookies);
                Response.Cookies.Set(userNoCookies);


                HttpCookie userNameCookies1 = new HttpCookie("UserId");
                userNameCookies1.Value = Uid;
                Response.Cookies.Add(userNameCookies1);

                HttpCookie userNameCookies4 = new HttpCookie("UName");
                userNameCookies4.Value = model.UserNo;
                Response.Cookies.Add(userNameCookies4);


                HttpCookie userNameCookies2 = new HttpCookie("UserRole");
                userNameCookies2.Value = urole;
                Response.Cookies.Add(userNameCookies2);


                // return Json("Success", JsonRequestBehavior.AllowGet);

            }
            else
            {
                context.RegisterUsers.Add(objregister);
                context.SaveChanges();
                string UsName = UserNo;
                HttpCookie userNoCookies = new HttpCookie("UserName");
                userNoCookies.Value = UsName;
                Response.Cookies.Add(userNoCookies);
                Response.Cookies.Set(userNoCookies);

                var Userid = objregister.Id;

                HttpCookie userNameCookies1 = new HttpCookie("UserId");
                userNameCookies1.Value = Userid.ToString();
                Response.Cookies.Add(userNameCookies1);

                HttpCookie userNameCookies4 = new HttpCookie("UName");
                userNameCookies4.Value = model.UserNo;
                Response.Cookies.Add(userNameCookies4);
                HttpCookie userNameCookies2 = new HttpCookie("UserRole");
                userNameCookies2.Value = logintype;
                Response.Cookies.Add(userNameCookies2);
            }
        }
        #endregion
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
                        return RedirectToAction("UpdateReferences");
                    }

                    Request.Files[upload].SaveAs(Path.Combine(path, SavePath + Extension));
                }
                readExcel.ExcelPath = Path.Combine(path, SavePath + Extension);
                worldExcelModel.ExcelPath = Path.Combine(SavePath + Extension).ToString();
                worldExcelModel.ExcelName = filename;
                retStatus = readExcel.ReadDataFromExcelToUpdate();
                readExcel.SaveData(worldExcelModel);

                TempData.Clear();
                TempData.Add("ErrorMessage", "Thank you for taking a step forward to glorifying yourself in WorlRef Map");
            }
            catch (Exception ex)
            {
                TempData.Clear();
                TempData.Add("ErrorMessage", ex.Message.ToString());//"Please Upload Correct format.Please Download the Format");
                return RedirectToAction("UpdateReferences");
            }
            return RedirectToAction("UpdateReferences");
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


        #region code for like Image on searchImages date: 23/11/2015 added by rahul shukla

        public JsonResult AddLikeSearchImage(int ProjectId, string UserID, string LikeOrDisLike, string UserIp, string ProjectImageId)
        {
            string returnString = "login";
            if (Request.Cookies["UserId"] != null)//check user cookies is not null.
            {
                UserIp = Request.Cookies["UserId"].Value.ToString();//assign user id in UserIp parameter.
                if (UserIp == UserID)//check UserIp and userid is same .
                {
                    return Json("userProject", JsonRequestBehavior.AllowGet);//return userProject action.
                }
                else
                {
                    returnString = likeRating.AddLikeImages(ProjectId, ProjectImageId, LikeOrDisLike, UserIp, UserID);//call AddLikeImages function from likeRating.cs. 
                    return Json(returnString, JsonRequestBehavior.AllowGet);//return string.
                }
            }
            else if (Request.Cookies["CUserId"] != null)//check general user is not null.
            {
                UserIp = Request.Cookies["CUserId"].Value.ToString();//assign userid in UserIp.
                returnString = likeRating.AddLikeImages(ProjectId, LikeOrDisLike, ProjectImageId, UserID, UserIp);//call AddLikeImages function from likeRating.cs.
                return Json(returnString, JsonRequestBehavior.AllowGet);//return string.
            }
            else
            {
                return Json(returnString, JsonRequestBehavior.AllowGet);//return string.

            }
        }
        public JsonResult AllLikesOnImage(int Imageid, int ProjectId, int usrId)
        {
            List<RegisterUserDAO> obj = new List<RegisterUserDAO>();
            var data = (from s in context.RegisterUsers join d in context.ImageLikeAndComments on s.Id equals d.UserId where d.ImageId == Imageid && d.ProjectId == ProjectId && d.ImgLikeFlag == true select new { s.UserFirstName, s.ProfileAttach }).AsEnumerable();//fetch UserFirstName and ProfileAttach from ImageLikeAndComments and registeruser.
            foreach (var result in data)
            {
                obj.Add(new RegisterUserDAO()
                {
                    UserFirstName = result.UserFirstName,
                    ProfileAttach = result.ProfileAttach
                });//add data in list.

            }
            return Json(obj, JsonRequestBehavior.AllowGet);//return list.
        }
        public JsonResult AllCommentsOnImage(int Imageid, int ProjectId)
        {
            List<ImageLikeCommentDAO> objComments = new List<ImageLikeCommentDAO>();
            var coments = (from g in context.RegisterUsers join f in context.ImageLikeAndComments on g.Id equals f.UserId where f.ImageId == Imageid && f.ProjectId == ProjectId && f.CmtImgFlag == true select new { f.ImageTestimonials, g.ProfileAttach }).AsEnumerable();//fetch UserFirstName and ProfileAttach from ImageLikeAndComments and registeruser.
            foreach (var data in coments)
            {
                objComments.Add(new ImageLikeCommentDAO()
                {
                    ImageTestimonials = data.ImageTestimonials,
                    ProfileAttach = data.ProfileAttach

                });//add data in list.
            }
            return Json(objComments, JsonRequestBehavior.AllowGet);//return list.
        }
        public JsonResult GetAllComentsImage(int projectId, int projectImageId, int UserId)
        {
            List<ReviewList> comment = likeRating.GetCommentsOfProject(projectId, projectImageId);
            return Json(comment, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult Explore()
        {

            return View();
        }
    }
}