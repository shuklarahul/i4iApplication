﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;
using WorldRef.Models;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;

namespace WorldRef.BusinessLayer
{
    /// <summary>
    /// Read Excel will have all the functionality 
    /// to Read the excel when upload
    /// </summary>
    public class ReadExcel
    {
        #region Private Members

        private string _excelPath = string.Empty;
        private OleDbConnection oledbConn = null;
        private DataSet _resultDataSet = new DataSet();
       
        private DataTable data = new DataTable();
        private I4IDBEntities context = new I4IDBEntities();

        //SqlConnection con = new SqlConnection();
        string con = ConfigurationManager.ConnectionStrings["ConString"].ToString();
        SqlDataAdapter adap;
        DataSet ds;

        #endregion

        #region Public Members

        /// <summary>
        /// ExcelPath Will contain the path of the excel sheet
        /// </summary>
        public string ExcelPath
        {
            get
            {
                return _excelPath;
            }
            set
            {
                _excelPath = value;
            }
        }

        /// <summary>
        /// ResultDataSet will have all the data
        /// </summary>
        public DataSet ResultDataSet
        {
            get
            {
                return _resultDataSet;
            }
            set
            {
                _resultDataSet = value;
            }
        }

        public DataTable DataTbl
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }


        #endregion


        /// <summary>
        /// Read Data From Excel
        /// </summary>
        /// <returns></returns>
        public string ReadDataFromExcel()
        {
            string ReturnStatus = "Success";

            try
            {
                if (Path.GetExtension(ExcelPath) == ".xls")
                {
                    oledbConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + ExcelPath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"");
                }
                else if (Path.GetExtension(ExcelPath) == ".xlsx")
                {
                    oledbConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ExcelPath + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;'");
                }
                oledbConn.Open();

                OleDbCommand command = new OleDbCommand(); ;
                OleDbDataAdapter oldbAdapter = null;
                DataSet dataSet = new DataSet();

                DataTable dt = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                int numberOfSheets = 0;

                if (dt != null)
                {
                    numberOfSheets = dt.AsEnumerable().Cast<DataRow>().Where(row => row["TABLE_NAME"].ToString().EndsWith("$")).Count();
                }

                for (int i = 1; i <= numberOfSheets; i++)
                {
                    command.Connection = oledbConn;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "Select * from [Sheet" + i + "$]";
                    oldbAdapter = new OleDbDataAdapter(command);

                    oldbAdapter.Fill(dataSet);

                }
                _resultDataSet = dataSet;

            }
            catch (Exception ex)
            {
                ReturnStatus = "Mohit" + ex.Message.ToString();
                throw ex;
            }
            return ReturnStatus;
        }

        /// <summary>
        /// Add the data From the excel file to the 
        /// database
        /// </summary>
        /// <param name="worldExcelModel"></param>
        /// <returns></returns>
        public string AddData(WorldRefExcelDataModel worldExcelModel)
        {
            string ReturnStatus = "Success";
            try
            {
                if (ResultDataSet.Tables.Count > 0)
                {
                    foreach (DataTable dataTable in ResultDataSet.Tables)
                    {
                        if (dataTable.Rows.Count > 0)
                        {
                            foreach (DataRow dataRow in dataTable.Rows)
                            {

                                if (!string.IsNullOrEmpty(dataRow[0].ToString()) || !string.IsNullOrEmpty(dataRow[1].ToString()) || !string.IsNullOrEmpty(dataRow[2].ToString())
                                    || !string.IsNullOrEmpty(dataRow[3].ToString()) || !string.IsNullOrEmpty(dataRow[4].ToString()) || !string.IsNullOrEmpty(dataRow[5].ToString())
                                    || !string.IsNullOrEmpty(dataRow[6].ToString()) || !string.IsNullOrEmpty(dataRow[7].ToString()))
                                {
                                    context.WorldRefExcelDataProjects.Add(new WorldRefExcelDataProject()
                                    {
                                        OrganizationName = dataRow[0].ToString(),
                                        CustomerName = dataRow[1].ToString(),
                                        CustomerIndustryType = dataRow[2].ToString(),
                                        ProjectName = dataRow[3].ToString(),
                                        Type = dataRow[4].ToString(),
                                        Status = dataRow[5].ToString(),
                                        Year = dataRow[6].ToString(),
                                        Country = dataRow[7].ToString(),
                                        Description = dataRow[8].ToString(),
                                        //  EmailId = dataRow[6].ToString(),
                                        IsOrganization = worldExcelModel.IsOrganization,
                                        IsCustomer = worldExcelModel.IsCustomer,
                                        IsProject = worldExcelModel.IsProject,
                                        IsType = worldExcelModel.IsType,
                                        IsYear = worldExcelModel.IsYear,
                                        IsCountry = false,
                                        IsEmail = worldExcelModel.IsEmail,
                                        IsStatus = worldExcelModel.IsStatus,
                                        ExcelPath = worldExcelModel.ExcelPath,
                                        ExcelName = worldExcelModel.ExcelName,
                                        userid = worldExcelModel.userid,
                                        IsAuthorized = 0,
                                        Createdon = DateTime.Now
                                    });
                                    context.SaveChanges();

                                    int projectId = (from projId in context.WorldRefExcelDataProjects.Where(x => x.id == context.WorldRefExcelDataProjects.Max(b => b.id))
                                                     select projId.id).FirstOrDefault();

                                    AddSearchKeywords(projectId, dataRow[9].ToString());

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnStatus = "Error";
                throw ex;
            }

            return string.Empty;
        }

        public string AddSearchKeywords(int projectId, string searchKeywords)
        {
            if (!string.IsNullOrEmpty(searchKeywords))
            {
                string[] splitSearchKeyword = searchKeywords.Split(',');

                foreach (string keyword in splitSearchKeyword)
                {
                    if (!string.IsNullOrEmpty(keyword) && keyword.ToLower() != "etc" && keyword.ToLower() != "etc.")
                    {

                        ProjectSearchKeyword CheckExists = (from projId in context.ProjectSearchKeywords
                                                            where
                                                                projId.SearchKeyword == keyword &&
                                                                projId.ProjectId == projectId
                                                            select projId).FirstOrDefault();
                        if (CheckExists == null)
                        {
                            context.ProjectSearchKeywords.Add(new ProjectSearchKeyword()
                            {
                                ProjectId = projectId,
                                SearchKeyword = keyword,
                                createdOn = DateTime.Now,
                                isDeleted = false
                            });
                        }
                        else
                        {
                            CheckExists.isDeleted = false;
                        }
                        context.SaveChanges();
                    }
                }
            }
            return string.Empty;
        }


        public string AddParticularProject(WorldRefExcelDataModel worldExcelModel)
        {
            string ReturnString = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(worldExcelModel.OrganizationName) || !string.IsNullOrEmpty(worldExcelModel.CustomerName) || !string.IsNullOrEmpty(worldExcelModel.CustomerIndustryType)
                                  || !string.IsNullOrEmpty(worldExcelModel.ProjectName) || !string.IsNullOrEmpty(worldExcelModel.Type) || !string.IsNullOrEmpty(worldExcelModel.Status)
                                  || !string.IsNullOrEmpty(worldExcelModel.Year) || !string.IsNullOrEmpty(worldExcelModel.Country))
                {
                    context.WorldRefExcelDataProjects.Add(new WorldRefExcelDataProject()
                    {
                        OrganizationName = worldExcelModel.OrganizationName,
                        CustomerName = worldExcelModel.CustomerName,
                        CustomerIndustryType = worldExcelModel.CustomerIndustryType,
                        ProjectName = worldExcelModel.ProjectName,
                        Type = worldExcelModel.Type,
                        Status = worldExcelModel.Status,
                        Year = worldExcelModel.Year,
                        Country = worldExcelModel.Country,
                        Description = worldExcelModel.Description,
                        IsOrganization = false,
                        IsCustomer = worldExcelModel.IsCustomer,
                        IsProject = worldExcelModel.IsProject,
                        IsType = false,
                        IsYear = worldExcelModel.IsYear,
                        IsCountry = false,
                        IsEmail = worldExcelModel.IsEmail,
                        IsStatus = worldExcelModel.IsStatus,
                        ExcelPath = worldExcelModel.ExcelPath,
                        ExcelName = worldExcelModel.ExcelName,
                        userid = worldExcelModel.userid,
                        IsAuthorized = 0,
                        IsAdminAuthorized = false,
                        IsAdminUnAuthorized = false,
                        Createdon = DateTime.Now,
                        UserEditFlag = false
                    });

                    context.SaveChanges();

                    int projectId = (from projId in context.WorldRefExcelDataProjects.Where(x => x.id == context.WorldRefExcelDataProjects.Max(b => b.id))
                                     select projId.id).FirstOrDefault();

                    AddSearchKeywords(projectId, worldExcelModel.SearchKeywords);
                }
            }
            catch
            {
                ReturnString = "Fail";
            }

            return ReturnString;
        }

        /// <summary>
        /// It will read all the data
        /// </summary>
        /// <returns></returns>
        public List<WorldRefExcelDataModel> ReadAllData()
        {
            List<WorldRefExcelDataModel> listResult = new List<WorldRefExcelDataModel>();

            var result = (from excelProject in context.WorldRefExcelDataProjects select excelProject).AsEnumerable();

            foreach (var project in result)
            {
                string organizationName = string.Empty;
                string Customer = string.Empty;
                string Project = string.Empty;
                string Type = string.Empty;
                string Year = string.Empty;
                string Country = string.Empty;
                string emailid = string.Empty;

                if (project.IsOrganization == true)
                {
                    organizationName = project.OrganizationName;
                }
                if (project.IsCustomer == true)
                {
                    Customer = project.CustomerName;
                }
                if (project.IsProject == true)
                {
                    Project = project.ProjectName;
                }
                if (project.IsType == true)
                {
                    Type = project.Type;
                }
                if (project.IsYear == true)
                {
                    Year = project.Year;
                }
                if (project.IsCountry == true)
                {
                    Country = project.Country;
                }
                if (project.IsEmail == true)
                {
                    emailid = project.EmailId;
                }
                listResult.Add(new WorldRefExcelDataModel()
                {
                    id = project.id,
                    OrganizationName = organizationName,
                    CustomerName = Customer,
                    ProjectName = Project,
                    Type = Type,
                    Year = Year,
                    Country = Country,
                    EmailId = emailid

                });
            }
            return listResult;
        }

        /// <summary>
        /// Return Approve data
        /// </summary>
        /// <returns></returns>
        public List<WorldRefExcelDataModel> ApproveData()
        {
            List<WorldRefExcelDataModel> listResult = new List<WorldRefExcelDataModel>();

            var result = (from excelProject in context.WorldRefExcelDataProjects where excelProject.IsAuthorized == 1 select excelProject).AsEnumerable();

            foreach (var project in result)
            {
                string organizationName = string.Empty;
                string Customer = string.Empty;
                string Project = string.Empty;
                string Type = string.Empty;
                string Year = string.Empty;
                string Country = string.Empty;
                string emailid = string.Empty;

                if (project.IsOrganization == true)
                {
                    organizationName = project.OrganizationName;
                }
                if (project.IsCustomer == true)
                {
                    Customer = project.CustomerName;
                }
                if (project.IsProject == true)
                {
                    Project = project.ProjectName;
                }
                if (project.IsType == true)
                {
                    Type = project.Type;
                }
                if (project.IsYear == true)
                {
                    Year = project.Year;
                }
                if (project.IsCountry == true)
                {
                    Country = project.Country;
                }
                if (project.IsEmail == true)
                {
                    emailid = project.EmailId;
                }
                listResult.Add(new WorldRefExcelDataModel()
                {
                    id = project.id,
                    OrganizationName = organizationName,
                    CustomerName = Customer,
                    ProjectName = Project,
                    Type = Type,
                    Year = Year,
                    Country = Country,
                    EmailId = emailid


                });
            }
            return listResult;
        }

        /// <summary>
        /// Return DisApprove data
        /// </summary>
        /// <returns></returns>
        public List<WorldRefExcelDataModel> DisApproveData()
        {
            List<WorldRefExcelDataModel> listResult = new List<WorldRefExcelDataModel>();

            var result = (from excelProject in context.WorldRefExcelDataProjects where excelProject.IsAuthorized == 1 select excelProject).AsEnumerable();

            foreach (var project in result)
            {
                string organizationName = string.Empty;
                string Customer = string.Empty;
                string Project = string.Empty;
                string Type = string.Empty;
                string Year = string.Empty;
                string Country = string.Empty;
                string emailid = string.Empty;

                if (project.IsOrganization == true)
                {
                    organizationName = project.OrganizationName;
                }
                if (project.IsCustomer == true)
                {
                    Customer = project.CustomerName;
                }
                if (project.IsProject == true)
                {
                    Project = project.ProjectName;
                }
                if (project.IsType == true)
                {
                    Type = project.Type;
                }
                if (project.IsYear == true)
                {
                    Year = project.Year;
                }
                if (project.IsCountry == true)
                {
                    Country = project.Country;
                }
                if (project.IsEmail == true)
                {
                    emailid = project.EmailId;
                }
                listResult.Add(new WorldRefExcelDataModel()
                {
                    id = project.id,
                    OrganizationName = organizationName,
                    CustomerName = Customer,
                    ProjectName = Project,
                    Type = Type,
                    Year = Year,
                    Country = Country,
                    EmailId = emailid,
                    DisApproveReason = project.DisApproveReason

                });
            }
            return listResult;
        }

        /// <summary>
        /// It will read all the data that will be Approved by Admin
        /// </summary>
        /// <returns></returns>
        public List<WorldRefExcelDataModel> ReadAllDataToApproveByAdmin()
        {
            List<WorldRefExcelDataModel> listResult = new List<WorldRefExcelDataModel>();

            var result = (from excelProject in context.WorldRefExcelDataProjects
                          join
                           reg in context.RegisterUsers on excelProject.userid equals reg.Id
                          where excelProject.IsAuthorized == 0
                          select new { excelProject, reg }).AsEnumerable().OrderByDescending(x => x.excelProject.id);

            foreach (var project in result)
            {
                string rating = "0";

                rating = (from rate in context.ProjectRatings
                          where rate.projectId == project.excelProject.id
                          select rate.rating).FirstOrDefault();

                rating = !string.IsNullOrEmpty(rating) ? rating : "0";

                listResult.Add(new WorldRefExcelDataModel()
                {
                    id = project.excelProject.id,
                    OrganizationName = project.excelProject.OrganizationName,
                    CustomerName = project.excelProject.CustomerName,
                    ProjectName = project.excelProject.ProjectName,
                    Type = project.excelProject.Type,
                    Year = project.excelProject.Year,
                    Country = project.excelProject.Country,
                    // EmailId = project.EmailId,
                    Createdon = project.excelProject.Createdon,
                    UserName = project.reg.UserFirstName,
                    UserEditFlag = Convert.ToBoolean(project.excelProject.UserEditFlag),
                    Rating = rating

                });
            }
            return listResult;
        }

        /// <summary>
        /// Approve or disapprove the project by admin
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="Reason"></param>
        /// <returns></returns>
        public string ApproveOrDisApprove(int id, string status, string Reason)
        {
            string ReturnStatus = "Success";
            try
            {
                WorldRefExcelDataProject excelProject = (from excel in context.WorldRefExcelDataProjects where excel.id == id select excel).FirstOrDefault();

                if (excelProject != null)
                {
                    excelProject.IsAuthorized = Convert.ToInt32(status);
                    excelProject.IsAdminAuthorized = true;
                    excelProject.IsAdminUnAuthorized = true;
                    if (!string.IsNullOrEmpty(Reason))
                    {
                        excelProject.DisApproveReason = Reason;
                    }
                    context.SaveChanges();
                }
            }
            catch
            {
                ReturnStatus = "Fail";
            }
            return ReturnStatus;
        }

        /// <summary>
        /// Add Images and videos to the database
        /// Images and videos are saved in the upload folder
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="ImagesPath"></param>
        /// <returns></returns>
        public string AddImageOrVideo(int? projectId, List<ImageStructure> ImagesPath)
        {
            string ReturnStatus = "Success";

            try
            {
                foreach (ImageStructure image in ImagesPath)
                {
                    context.ProjectImages.Add(new ProjectImage()
                    {
                        ProjectId = projectId,
                        ImageName = image.ImageName,
                        ImagePath = image.ImagePath,
                        CreatedOn = DateTime.Now,
                        link = image.Link
                    });


                    context.SaveChanges();
                }
            }
            catch
            {
            }
            return ReturnStatus;
        }
        public string AdCertificate(int? projectId, string filename)
        {
            string ReturnStatus = "Success";

            try
            {
                ProjectImage proImgModel = (from d in context.ProjectImages
                                            where d.id == projectId
                                            select d).Single();
                proImgModel.ProjectCertificates = filename;
                context.SaveChanges();
            }
            catch
            {
            }
            return ReturnStatus;
        }
        /// <summary>
        /// Return all the Apporve data of a  user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<WorldRefExcelDataModel> AllApproveDataUser(int userId)
        {
            List<WorldRefExcelDataModel> listResult = new List<WorldRefExcelDataModel>();

            //WorldRefExcelDataProject wdp = new WorldRefExcelDataProject();


            List<Int32> crCount = new List<Int32>();
            var result = (from excelProject in context.WorldRefExcelDataProjects
                          join users in context.RegisterUsers on excelProject.userid equals users.Id
                          where excelProject.IsAuthorized == 1 && excelProject.userid == userId
                          select new { excelProject, users }).AsEnumerable();
            var creditCount = (from m in context.LinkedInUsers
                               where m.CreditM == false
                               group m by new { m.id } into resultsSet
                               select new { resultsSet.Key.id });

            foreach (var cr in creditCount)
            {
                crCount.Add(Convert.ToInt32(cr.id));
            }
            //n.userId == userId &&
            var reviw = (from n in context.ProjectReviews where n.flag == true select new { n.ProjectId, n.flag });


            var abc = (from c in context.ProjectLikeDisLikes select c);
            foreach (var project in result)
            {
                var rvw = 0;
                foreach (var rv in reviw)
                {
                    if (rv.ProjectId == project.excelProject.id)
                        rvw = 1;
                }
                var creditThem = 0;
                if (crCount.Contains(project.excelProject.id))
                {
                    creditThem = 1;
                }
                var likecount = 0;
                var likflag = false;
                foreach (var like in abc)
                {
                    if (project.excelProject.id == like.projectId)
                    {
                        likecount = Convert.ToInt32(like.totalLike);
                        likflag = (bool)(like.flag);
                    }
                }

                listResult.Add(new WorldRefExcelDataModel()
                {
                    id = project.excelProject.id,
                    OrganizationName = project.excelProject.OrganizationName,
                    CustomerName = project.excelProject.CustomerName,
                    ProjectName = project.excelProject.ProjectName,
                    Type = project.excelProject.Type,
                    Year = project.excelProject.Year,
                    Country = project.excelProject.Country,
                    Createdon = project.excelProject.Createdon,
                    Status = project.excelProject.Status,
                    CustomerIndustryType = project.excelProject.CustomerIndustryType,
                    UserType = project.users.Type,
                    creditM = creditThem,
                    countApprove = Convert.ToInt32(likecount),
                    countflg = likflag.ToString(),
                    reviewFag = rvw.ToString(),
                    Description = project.excelProject.Description,
                    IsAdminAuthorized = Convert.ToBoolean(project.excelProject.IsAdminAuthorized),
                    IsAdminUnAuthorized = Convert.ToBoolean(project.excelProject.IsAdminUnAuthorized),
                    SearchKeywords = String.Join(",", context.ProjectSearchKeywords.Where(x => x.ProjectId == project.excelProject.id && x.isDeleted == false).Select(p => p.SearchKeyword))
                });
            }

            var wdp = (from a in context.WorldRefExcelDataProjects
                       where a.IsAdminAuthorized == true && a.userid == userId
                       select a).AsEnumerable();

            I4IDBEntities db = new I4IDBEntities();
            if (wdp != null)
            {
                foreach (var ii in wdp)
                {
                    var c = db.WorldRefExcelDataProjects.Where(a => a.id.Equals(ii.id)).FirstOrDefault();
                    if (c != null)
                    {
                        c.IsAdminAuthorized = false;
                    }
                }
                db.SaveChanges();
            }
            return listResult;
        }
        public List<WorldRefExcelDataModel> AllApproveDataUserAdmin()
        {
            //List<WorldRefExcelDataModel> listResult = new List<WorldRefExcelDataModel>();

            //var result = (from excelProject in context.WorldRefExcelDataProjects where excelProject.IsAuthorized == 1 select excelProject).AsEnumerable().OrderByDescending(x => x.id);

            //foreach (var project in result)
            //{
            //    string rating = "0";
            //    string qualityrat = "0";
            //    string userrating = "0";
            //    string userName = string.Empty;
            //    string Review = string.Empty;
            //    rating = (from rate in context.ProjectRatings
            //              where rate.projectId == project.id
            //              select rate.rating).FirstOrDefault();
            //    qualityrat = (from rate in context.ProjectRatings
            //                  where rate.projectId == project.id
            //                  select rate.QualityRating).FirstOrDefault();
            //    userrating = (from rate in context.ProjectRatings
            //                  where rate.projectId == project.id
            //                  select rate.UserRating).FirstOrDefault();
            //    Review = (from rate in context.ProjectReviews
            //              where rate.ProjectId == project.id && rate.userId == 0
            //              select rate.Review).FirstOrDefault();
            //    userName = (from user in context.RegisterUsers
            //                where
            //                    user.Id == project.userid
            //                select user.UserFirstName).FirstOrDefault();
            //    rating = !string.IsNullOrEmpty(rating) ? rating : "0";
            //    qualityrat = !string.IsNullOrEmpty(rating) ? qualityrat : "0";
            //    userrating = !string.IsNullOrEmpty(rating) ? userrating : "0";
            //    listResult.Add(new WorldRefExcelDataModel()
            //    {
            //        id = project.id,
            //        OrganizationName = project.OrganizationName,
            //        CustomerName = project.CustomerName,
            //        ProjectName = project.ProjectName,
            //        Type = project.Type,
            //        Year = project.Year,
            //        Country = project.Country,
            //        //    EmailId = project.EmailId,
            //        Createdon = project.Createdon,
            //        Rating = rating,
            //        QualityRating = qualityrat,
            //        UserRating = userrating,
            //        UserName = userName,
            //        UserType = Review
            //    });

            //}
            //return listResult;



            List<WorldRefExcelDataModel> listResult = new List<WorldRefExcelDataModel>();

            var result = (from excelProject in context.WorldRefExcelDataProjects where excelProject.IsAuthorized == 1 select excelProject).AsEnumerable().OrderByDescending(x => x.id);
            var reviw = (from n in context.ProjectReviews where n.Show == true && n.Publish == true && n.AdminFlag == true select new { n.ProjectId, n.flag });
            foreach (var project in result)
            {
                var rvw1 = 0;
                foreach (var rv in reviw)
                {
                    if (rv.ProjectId == project.id)
                        rvw1 = 1;
                }
                string rating = "0";
                string qualityrat = "0";
                string userrating = "0";
                string userName = string.Empty;
                string Review = string.Empty;
                string Status = string.Empty;
                string searchkey = string.Empty;
                rating = (from rate in context.ProjectRatings
                          where rate.projectId == project.id
                          select rate.rating).FirstOrDefault();
                qualityrat = (from rate in context.ProjectRatings
                              where rate.projectId == project.id
                              select rate.QualityRating).FirstOrDefault();
                userrating = (from rate in context.ProjectRatings
                              where rate.projectId == project.id
                              select rate.UserRating).FirstOrDefault();
                Review = (from rate in context.ProjectReviews
                          where rate.ProjectId == project.id && rate.userId == 0
                          select rate.Review).FirstOrDefault();
                userName = (from user in context.RegisterUsers
                            where
                                user.Id == project.userid
                            select user.UserFirstName).FirstOrDefault();


                Status = (from user in context.WorldRefExcelDataProjects
                          where
                              user.id == project.userid
                          select user.Status).FirstOrDefault();

                rating = !string.IsNullOrEmpty(rating) ? rating : "0";
                qualityrat = !string.IsNullOrEmpty(rating) ? qualityrat : "0";
                userrating = !string.IsNullOrEmpty(rating) ? userrating : "0";
                Status = !string.IsNullOrEmpty(rating) ? userrating : "Not Available";


                listResult.Add(new WorldRefExcelDataModel()
                {
                    id = project.id,
                    SearchKeywords = String.Join(",", context.ProjectSearchKeywords.Where(x => x.ProjectId == project.id && x.isDeleted == false).Select(p => p.SearchKeyword)),
                    OrganizationName = project.OrganizationName,
                    CustomerName = project.CustomerName,
                    ProjectName = project.ProjectName,
                    Type = project.Type,
                    Year = project.Year,
                    Country = project.Country,
                    //    EmailId = project.EmailId,
                    Createdon = project.Createdon,
                    Rating = rating,
                    QualityRating = qualityrat,
                    UserRating = userrating,
                    UserName = userName,
                    UserType = Review,
                    reviewFag = rvw1.ToString(),
                    Status = project.Status

                });

            }
            return listResult;


        }

        /// <summary>
        /// Return all the disApprove project of a user
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<WorldRefExcelDataModel> AllDisApproveDataUser(int UserId)
        {
            List<WorldRefExcelDataModel> listResult = new List<WorldRefExcelDataModel>();





            var result = (from excelProject in context.WorldRefExcelDataProjects
                          join users in context.RegisterUsers on excelProject.userid equals users.Id
                          where excelProject.IsAuthorized == 2 && excelProject.userid == UserId
                          select new { excelProject, users }).AsEnumerable();

            foreach (var project in result)
            {
                listResult.Add(new WorldRefExcelDataModel()
                {
                    id = project.excelProject.id,
                    OrganizationName = project.excelProject.OrganizationName,
                    CustomerName = project.excelProject.CustomerName,
                    ProjectName = project.excelProject.ProjectName,
                    Type = project.excelProject.Type,
                    Year = project.excelProject.Year,
                    Country = project.excelProject.Country,
                    //  EmailId = project.EmailId,
                    Createdon = project.excelProject.Createdon,
                    // listOfImages=images,
                    Status = project.excelProject.Status,
                    CustomerIndustryType = project.excelProject.CustomerIndustryType,
                    UserType = project.users.Type,
                    Description = project.excelProject.Description,
                    DisApproveReason = project.excelProject.DisApproveReason,
                    IsAdminAuthorized = Convert.ToBoolean(project.excelProject.IsAdminAuthorized),
                    IsAdminUnAuthorized = Convert.ToBoolean(project.excelProject.IsAdminUnAuthorized)

                });
            }
            var wdp = (from a in context.WorldRefExcelDataProjects
                       where a.IsAdminUnAuthorized == true && a.userid == UserId
                       select a).AsEnumerable();

            I4IDBEntities db = new I4IDBEntities();
            if (wdp != null)
            {
                foreach (var ii in wdp)
                {
                    var c = db.WorldRefExcelDataProjects.Where(a => a.id.Equals(ii.id)).FirstOrDefault();
                    if (c != null)
                    {
                        c.IsAdminUnAuthorized = false;
                    }
                }
                db.SaveChanges();
            }

            return listResult;
        }

        /// <summary>
        /// Return all the uploaded project by a user
        /// </summary>
        /// <returns></returns>
        public List<WorldRefExcelDataModel> ReadAllDataUser(int userId)
        {
            List<WorldRefExcelDataModel> listResult = new List<WorldRefExcelDataModel>();
            List<Int32> crCount = new List<Int32>();
            var result = (from excelProject in context.WorldRefExcelDataProjects
                          join users in context.RegisterUsers on excelProject.userid equals users.Id
                          where excelProject.userid == userId
                          orderby excelProject.Createdon descending
                          select new { excelProject, users }).AsEnumerable();
            List<ImageStructure> images;

            var creditCount = (from m in context.LinkedInUsers
                               where m.CreditM == false
                               group m by new { m.id } into resultsSet
                               select new { resultsSet.Key.id });
            foreach (var cr in creditCount)
            {
                crCount.Add(Convert.ToInt32(cr.id));
            }

            var reviw = (from n in context.ProjectReviews where n.userId == userId && n.flag == true select new { n.ProjectId, n.flag });


            var abc = (from c in context.ProjectLikeDisLikes select c);

            int i = 0;
            foreach (var project in result)
            {
                var rvw = 0;
                foreach (var rv in reviw)
                {
                    if (rv.ProjectId == project.excelProject.id)
                        rvw = 1;
                }
                if (project.excelProject.IsAuthorized == 2 || project.excelProject.IsAuthorized == 0)
                {

                    images = new List<ImageStructure>();
                    var ProjectImages = context.ProjectImages.Where(x => x.ProjectId == project.excelProject.id).AsEnumerable();
                    foreach (var projectImage in ProjectImages)
                    {
                        images.Add(new ImageStructure()
                        {
                            ImageName = projectImage.ImageName,
                            ImagePath = projectImage.ImagePath,
                            Link = projectImage.link
                        });
                    }
                    var creditThem = 0;
                    if (crCount.Contains(project.excelProject.id))
                    {
                        creditThem = 1;
                    }

                    listResult.Add(new WorldRefExcelDataModel()
                    {
                        id = project.excelProject.id,
                        OrganizationName = project.excelProject.OrganizationName,
                        CustomerName = project.excelProject.CustomerName,
                        ProjectName = project.excelProject.ProjectName,
                        Type = project.excelProject.Type,
                        Year = project.excelProject.Year,
                        Country = project.excelProject.Country,
                        Createdon = project.excelProject.Createdon,
                        listOfImages = images,
                        Status = project.excelProject.Status,
                        CustomerIndustryType = project.excelProject.CustomerIndustryType,
                        UserType = project.users.Type,
                        creditM = creditThem,
                        countApprove = 0,
                        countflg = false.ToString(),
                        reviewFag = rvw.ToString(),
                        IsAuthorized = Convert.ToInt32(project.excelProject.IsAuthorized),
                        Description = project.excelProject.Description,
                        SearchKeywords = String.Join(",", context.ProjectSearchKeywords.Where(x => x.ProjectId == project.excelProject.id && x.isDeleted == false).Select(p => p.SearchKeyword))
                    });


                }

                else
                {
                    images = new List<ImageStructure>();
                    var ProjectImages = context.ProjectImages.Where(x => x.ProjectId == project.excelProject.id).AsEnumerable();
                    foreach (var projectImage in ProjectImages)
                    {
                        images.Add(new ImageStructure()
                        {
                            ImageName = projectImage.ImageName,
                            ImagePath = projectImage.ImagePath,
                            Link = projectImage.link
                        });
                    }
                    var creditThem = 0;
                    if (crCount.Contains(project.excelProject.id))
                    {
                        creditThem = 1;
                    }
                    var likecount = 0;
                    var likflag = false;
                    foreach (var like in abc)
                    {
                        if (project.excelProject.id == like.projectId)
                        {
                            likecount = Convert.ToInt32(like.totalLike);
                            likflag = (bool)(like.flag);
                        }
                    }

                    listResult.Add(new WorldRefExcelDataModel()
                    {
                        id = project.excelProject.id,
                        OrganizationName = project.excelProject.OrganizationName,
                        CustomerName = project.excelProject.CustomerName,
                        ProjectName = project.excelProject.ProjectName,
                        Type = project.excelProject.Type,
                        Year = project.excelProject.Year,
                        Country = project.excelProject.Country,
                        Createdon = project.excelProject.Createdon,
                        listOfImages = images,
                        Status = project.excelProject.Status,
                        CustomerIndustryType = project.excelProject.CustomerIndustryType,
                        UserType = project.users.Type,
                        creditM = creditThem,
                        countApprove = Convert.ToInt32(likecount),
                        countflg = likflag.ToString(),
                        reviewFag = rvw.ToString(),
                        Description = project.excelProject.Description,
                        IsAuthorized = Convert.ToInt32(project.excelProject.IsAuthorized),
                        SearchKeywords = String.Join(",", context.ProjectSearchKeywords.Where(x => x.ProjectId == project.excelProject.id && x.isDeleted == false).Select(p => p.SearchKeyword))
                    });
                    i++;




                }


            }
            return listResult;
        }

        /// <summary>
        /// return all the Images and videos of a project 
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public List<ImageStructure> ViewImagesOfProject(int? ProjectId)
        {
            List<ImageStructure> images = new List<ImageStructure>();
            var ProjectImages = context.ProjectImages.Where(x => x.ProjectId == ProjectId).AsEnumerable();

            foreach (var projectImage in ProjectImages)
            {
                images.Add(new ImageStructure()
                {
                    ImageName = projectImage.ImageName,
                    ImagePath = projectImage.ImagePath,
                    Link = projectImage.link
                });
            }
            return images;
        }

        public WorldRefExcelDataModel ReadParticularProject(int projectId)
        {
            var project = (from excel in context.WorldRefExcelDataProjects
                           join users in context.RegisterUsers on excel.userid equals users.Id
                           where excel.id == projectId
                           select new { excel, users }).AsEnumerable();
            foreach (var proj in project)
            {
                WorldRefExcelDataModel projectModel = new WorldRefExcelDataModel()
                {
                    id = proj.excel.id,
                    OrganizationName = proj.excel.OrganizationName,
                    CustomerName = proj.excel.CustomerName,
                    ProjectName = proj.excel.ProjectName,
                    Type = proj.excel.Type,
                    Year = proj.excel.Year,
                    Country = proj.excel.Country,
                    IsOrganization = proj.excel.IsOrganization,
                    IsCustomer = proj.excel.IsCustomer,
                    IsProject = proj.excel.IsProject,
                    IsType = proj.excel.IsType,
                    IsYear = proj.excel.IsYear,
                    IsCountry = proj.excel.IsCountry,
                    IsEmail = proj.excel.IsEmail,
                    IsStatus = proj.excel.IsStatus,
                    UserType = proj.users.Type,
                    CustomerIndustryType = proj.excel.CustomerIndustryType,
                    Status = proj.excel.Status,
                    Description = proj.excel.Description,
                    SearchKeywords = String.Join(",", context.ProjectSearchKeywords.Where(x => x.ProjectId == proj.excel.id && x.isDeleted == false).Select(p => p.SearchKeyword))
                };
                return projectModel;
            }
            return null;
        }

        public List<LinkedInUser> readCreditforApprove(int projectId)
        {
            List<LinkedInUser> objlistLinkedin = new List<LinkedInUser>();
            var project = (from l in context.LinkedInUsers
                           where l.id == projectId && l.CreditM == false
                           select new { l }).AsEnumerable();

            int SerialNo = 0;
            foreach (var proj in project)
            {
                objlistLinkedin.Add(new LinkedInUser()
                {
                    Industry = "hdnLinkedinId" + SerialNo,
                    LinkedinUserID = proj.l.LinkedinUserID,
                    FirstName = proj.l.FirstName,
                    LastName = proj.l.LastName
                });
                SerialNo++;
            }
            return objlistLinkedin;
        }

        public List<LinkedInUser> ListOfCreditApproved(int projectId)
        {
            List<LinkedInUser> objlistLinkedin = new List<LinkedInUser>();
            var project = (from l in context.LinkedInUsers
                           where l.id == projectId && l.CreditM == true
                           select new { l }).AsEnumerable();
            foreach (var proj in project)
            {
                objlistLinkedin.Add(new LinkedInUser()
                {

                    LinkedinUserID = proj.l.LinkedinUserID,
                    FirstName = proj.l.FirstName,
                    LastName = proj.l.LastName,
                    Designation = proj.l.Designation,
                    Industry = proj.l.Industry
                });

            }
            return objlistLinkedin;
        }

        public string EditParticularProject(WorldRefExcelDataModel projectModel, int projectId)
        {
            //projectModel.id
            int pid = Convert.ToInt32(projectId);
            WorldRefExcelDataProject project = (from excel in context.WorldRefExcelDataProjects
                                                where excel.id == pid
                                                select excel).FirstOrDefault();

            if (project != null)
            {
                project.OrganizationName = projectModel.OrganizationName;
                project.CustomerName = projectModel.CustomerName;
                project.ProjectName = projectModel.ProjectName;
                project.Type = projectModel.Type;
                project.Year = projectModel.Year;
                project.Country = projectModel.Country;
                project.IsOrganization = projectModel.IsOrganization;
                project.IsCustomer = projectModel.IsCustomer;
                project.IsProject = projectModel.IsProject;
                project.IsType = false;
                project.IsYear = projectModel.IsYear;
                project.IsCountry = false;
                project.IsEmail = projectModel.IsEmail;
                project.IsStatus = projectModel.IsStatus;
                project.Description = projectModel.Description;
                project.CustomerIndustryType = projectModel.CustomerIndustryType;
                project.Status = projectModel.Status;
                project.Createdon = DateTime.Now;
                project.IsAuthorized = 0;
                project.UserEditFlag = true;
                project.ProjectLocation = projectModel.ProjectLocation;
                context.SaveChanges();

                EditSearchKeywords(projectModel.id, projectModel.SearchKeywords);

                return "Success";

            }

            return string.Empty;
        }

        public string EditSearchKeywords(int projectId, string searchKeywords)
        {
            var CheckExists = (from projId in context.ProjectSearchKeywords
                               where projId.ProjectId == projectId
                               select projId).ToList();

            CheckExists.ForEach(x => x.isDeleted = true);
            context.SaveChanges();

            AddSearchKeywords(projectId, searchKeywords);

            return "Success";
        }


        public List<WorldRefExcelDataModel> UserSearch()
        {
            List<WorldRefExcelDataModel> listResult = new List<WorldRefExcelDataModel>();
            var result = (from user in context.User_Details select user);
            foreach (var project in result)
            {
                string APIKey = "428806b68b312f487fb25da070164595b08102940df9daab54773534c980fa81";
                string ipAddress = project.LocationIP;
                var country = string.Format("http://api.ipinfodb.com/v3/ip-city/?key={0}&ip={1}&format=json", APIKey, ipAddress);
                List<Location> locations = new List<Location>();
                using (WebClient client = new WebClient())
                {
                    string json = client.DownloadString(country);
                    Location location = new JavaScriptSerializer().Deserialize<Location>(json);
                    locations.Add(location);
                }
                listResult.Add(new WorldRefExcelDataModel()
                {
                    UserName = project.UserName,
                    SearchKeywords = project.Keywords,
                    EmailId = project.EmailId,
                    IPAddress = locations[0].CityName + ", " + locations[0].CountryName
                });
            }
            return listResult;

        }
        public List<WorldRefExcelDataModel> userSearch1(string KeyWord)
        {
            List<WorldRefExcelDataModel> listResult = new List<WorldRefExcelDataModel>();

            string[] splitSearchKeyword = KeyWord.Split(',');

            foreach (string keyword in splitSearchKeyword)
            {
                if (!string.IsNullOrEmpty(keyword) && keyword.ToLower() != "etc" && keyword.ToLower() != "etc.")
                {

                    var result = (from user in context.User_Details where user.Keywords.Contains(keyword) select user);
                    foreach (var project in result)
                    {
                        listResult.Add(new WorldRefExcelDataModel()
                        {
                            UserName = project.UserName,
                            SearchKeywords = project.Keywords,
                            EmailId = project.EmailId,
                            IPAddress = project.LocationIP
                        });
                    }
                }
            }
            return listResult;

        }
        public DataTable userSeaKeywords(string KeyWord)
        {
            List<WorldRefExcelDataModel> listResult = new List<WorldRefExcelDataModel>();
            DataTable dt = new DataTable();
            dt.Columns.Add("UserName");
            dt.Columns.Add("Keywords");
            dt.Columns.Add("LocationIP");

            dt.Columns.Add("EmailId");

            string[] splitSearchKeyword = KeyWord.Split(',');

            foreach (string keyword in splitSearchKeyword)
            {
                if (!string.IsNullOrEmpty(keyword) && keyword.ToLower() != "etc" && keyword.ToLower() != "etc.")
                {

                    var result = (from user in context.User_Details where user.Keywords.Contains(keyword) select user);

                    foreach (var project in result)
                    {
                        DataRow drUser = dt.NewRow();
                        drUser["UserName"] = project.UserName;
                        drUser["Keywords"] = project.Keywords;
                        drUser["LocationIP"] = project.LocationIP;

                        drUser["EmailId"] = project.EmailId;

                        dt.Rows.Add(drUser);
                        dt.AcceptChanges();

                    }
                }
                else
                {
                    var result = (from user in context.User_Details select user);

                    foreach (var project in result)
                    {
                        DataRow drUser = dt.NewRow();
                        drUser["UserName"] = project.UserName;
                        drUser["Keywords"] = project.Keywords;
                        drUser["LocationIP"] = project.LocationIP;

                        drUser["EmailId"] = project.EmailId;

                        dt.Rows.Add(drUser);
                        dt.AcceptChanges();
                    }
                }
            }
            return dt;
        }



        #region added by shuklaji
        public string ReadDataFromExcelToUpdate()
        {
            string ReturnStatus = "Success";

            try
            {
                if (Path.GetExtension(ExcelPath) == ".xls")
                {
                    oledbConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + ExcelPath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"");
                }
                else if (Path.GetExtension(ExcelPath) == ".xlsx")
                {
                    oledbConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ExcelPath + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;'");
                }
                oledbConn.Open();

                OleDbCommand command = new OleDbCommand(); ;
                OleDbDataAdapter oldbAdapter = null;
                DataSet dataSet = new DataSet();

                DataTable dt = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                int numberOfSheets = 0;

                if (dt != null)
                {
                    numberOfSheets = dt.AsEnumerable().Cast<DataRow>().Where(row => row["TABLE_NAME"].ToString().EndsWith("$")).Count();
                }

                for (int i = 1; i <= numberOfSheets; i++)
                {
                    command.Connection = oledbConn;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "Select * from [Sheet" + i + "$]";
                    oldbAdapter = new OleDbDataAdapter(command);

                    oldbAdapter.Fill(dataSet);

                }
                DataTable dt1 = dataSet.Tables[0];
                ArrayList duplicateList = new ArrayList();

                string query = "select distinct ProjectName from [WorldRefExcelDataProject] ";
                adap = new SqlDataAdapter(query, con);
                ds = new DataSet();
                adap.Fill(ds);




                foreach (DataRow drow in dt1.Rows)
                {
                    string project = Convert.ToString(drow["Project/Product Reference Name"]);

                    bool exists = ds.Tables[0].AsEnumerable().Where(c => c.Field<string>("ProjectName").Equals(project)).Count() > 0;
                    
                    

                    if (exists==true)
                    {
                        duplicateList.Add(drow);

                    }

                }

                foreach (DataRow dRow in duplicateList)
                {
                    dt1.Rows.Remove(dRow);
                }
                data = dt1;

                //_resultDataSet = dt1;

            }
            catch (Exception ex)
            {
                ReturnStatus = "Mohit" + ex.Message.ToString();
                throw ex;
            }
            return ReturnStatus;
        }

        public string SaveData(WorldRefExcelDataModel worldExcelModel)
        {
            string ReturnStatus = "Success";
            try
            {
                //if (data.Rows.Count > 0)
                //{
                    //foreach (DataTable dataTable in ResultDataSet.Tables)
                    //{
                        if (data.Rows.Count > 0)
                        {
                            foreach (DataRow dataRow in data.Rows)
                            {

                                if (!string.IsNullOrEmpty(dataRow[0].ToString()) || !string.IsNullOrEmpty(dataRow[1].ToString()) || !string.IsNullOrEmpty(dataRow[2].ToString())
                                    || !string.IsNullOrEmpty(dataRow[3].ToString()) || !string.IsNullOrEmpty(dataRow[4].ToString()) || !string.IsNullOrEmpty(dataRow[5].ToString())
                                    || !string.IsNullOrEmpty(dataRow[6].ToString()) || !string.IsNullOrEmpty(dataRow[7].ToString()))
                                {
                                    context.WorldRefExcelDataProjects.Add(new WorldRefExcelDataProject()
                                    {
                                        OrganizationName = dataRow[0].ToString(),
                                        CustomerName = dataRow[1].ToString(),
                                        CustomerIndustryType = dataRow[2].ToString(),
                                        ProjectName = dataRow[3].ToString(),
                                        Type = dataRow[4].ToString(),
                                        Status = dataRow[5].ToString(),
                                        Year = dataRow[6].ToString(),
                                        Country = dataRow[7].ToString(),
                                        Description = dataRow[8].ToString(),
                                        //  EmailId = dataRow[6].ToString(),
                                        IsOrganization = worldExcelModel.IsOrganization,
                                        IsCustomer = worldExcelModel.IsCustomer,
                                        IsProject = worldExcelModel.IsProject,
                                        IsType = worldExcelModel.IsType,
                                        IsYear = worldExcelModel.IsYear,
                                        IsCountry = false,
                                        IsEmail = worldExcelModel.IsEmail,
                                        IsStatus = worldExcelModel.IsStatus,
                                        ExcelPath = worldExcelModel.ExcelPath,
                                        ExcelName = worldExcelModel.ExcelName,
                                        userid = worldExcelModel.userid,
                                        IsAuthorized = 0,
                                        Createdon = DateTime.Now
                                    });
                                    context.SaveChanges();

                                    int projectId = (from projId in context.WorldRefExcelDataProjects.Where(x => x.id == context.WorldRefExcelDataProjects.Max(b => b.id))
                                                     select projId.id).FirstOrDefault();

                                    AddSearchKeywords(projectId, dataRow[9].ToString());

                                }
                            }
                        }
                  //  }
               // }
            }
            catch (Exception ex)
            {
                ReturnStatus = "Error";
                throw ex;
            }

            return string.Empty;
        }
        #endregion
    }
    public static class customextension
    {
        public static DataTable ToDataTable<TSource>(this IList<TSource> data)
        {
            DataTable dataTable = new DataTable(typeof(TSource).Name);
            PropertyInfo[] props = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ??
                    prop.PropertyType);
            }

            foreach (TSource item in data)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
    } 














}