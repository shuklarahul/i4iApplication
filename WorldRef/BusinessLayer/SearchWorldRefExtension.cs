using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WorldRef.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace WorldRef.BusinessLayer
{
    public class SearchWorldRefExtension : SearchWorldRef
    {
        /// <summary>
        ///  It will return all industry 
        /// </summary>
        /// <returns></returns>
        public List<WorldRefExcelDataModel> GetAllIndustry()
        {
            List<WorldRefExcelDataModel> listIndustry = new List<WorldRefExcelDataModel>();

            var Industries = (from Coun in context.WorldRefExcelDataProjects where Coun.IsAuthorized == 1 select Coun).AsEnumerable();

            var countt = Industries.GroupBy(x => x.CustomerIndustryType).Select(x => x.First());

            foreach (var industry in countt)
            {
                if (!string.IsNullOrEmpty(industry.CustomerIndustryType))
                {
                    int index = listIndustry.FindIndex(x => x.CustomerIndustryType.ToLower() == industry.CustomerIndustryType.ToLower());
                    listIndustry.Add(new WorldRefExcelDataModel()
                    {
                        CustomerIndustryType = industry.CustomerIndustryType
                    });
                }
            }
            return listIndustry;
        }
        #region Search Page Filter

        public List<WorldRefSearchModel> ReturnInitialResultByIndustry(string searchResult, string[] industryFilter)
        {
            List<WorldRefSearchModel> listExcelProject = new List<WorldRefSearchModel>();

            List<ImageStructure> imageStructure = new List<ImageStructure>();
            List<LinkedInUser> appCredit = new List<LinkedInUser>();

            foreach (string IndustryName in industryFilter)
            {

                var projects = (from worldRefProject in context.WorldRefExcelDataProjects
                                join Keywords in context.ProjectSearchKeywords on worldRefProject.id equals Keywords.ProjectId
                                  into result
                                from rr in result.DefaultIfEmpty()
                                where (worldRefProject.OrganizationName.Contains(searchResult) ||
                                    worldRefProject.CustomerName.Contains(searchResult) || worldRefProject.ProjectName.Contains(searchResult) || worldRefProject.Type.Contains(searchResult) ||
                                    worldRefProject.Year.Contains(searchResult) || worldRefProject.Country.Contains(searchResult) || worldRefProject.CustomerIndustryType.Contains(searchResult)

                                    || worldRefProject.Status.Contains(searchResult) || (rr.SearchKeyword.Contains(searchResult) && rr.isDeleted == false)) && (worldRefProject.CustomerIndustryType == IndustryName) && worldRefProject.IsAuthorized == 1
                                select worldRefProject).AsEnumerable();

                foreach (var project in projects)
                {
                    imageStructure = new List<ImageStructure>();
                    appCredit = new List<LinkedInUser>();
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
                            id = ig.id,
                            ImageName = ig.ImageName,
                            ImagePath = ig.ImagePath,
                            ImgIndex = "0",
                            Link = ig.link
                        });
                    }

                    var likes = (from like in context.ProjectLikeDisLikes
                                 where like.projectId == project.id
                                 select like).FirstOrDefault();

                    string rating = "N/A";

                    rating = (from rate in context.ProjectRatings
                              where rate.projectId == project.id
                              select rate.rating).FirstOrDefault();

                    rating = !string.IsNullOrEmpty(rating) ? rating : "N/A";
                    if (rating == "1")
                    {
                        rating = "High";
                    }
                    else if (rating == "2")
                    {
                        rating = "Optimum";
                    }
                    else if (rating == "3")
                    {
                        rating = "Expensive";
                    }
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
                                      select new { r.picUrl, r.FirstName, r.Designation, r.Industry, r.id }).AsEnumerable();

                    foreach (var data in pictureUrl)
                    {
                        appCredit.Add(new LinkedInUser()
                        {
                            picUrl = data.picUrl,
                            Designation = data.Designation,
                            FirstName = data.FirstName,
                            Industry = data.Industry,
                            id = data.id

                        });
                    }

                    string Tlikes = "0";
                    string TDisLikes = "0";

                    if (likes != null)
                    {
                        Tlikes = likes.totalLike.ToString();
                        TDisLikes = likes.totalDislike;
                    }

                    dynamic registerField = (from rr in context.RegisterUsers where rr.Id == project.userid select new { rr.Type, rr.ProfileAttach, rr.ProfileUrl }).FirstOrDefault();


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
                            QualityRating = qualityrat,
                            UserRating = userrating,
                            CustomerIndustryType = project.CustomerIndustryType,
                            Status = status,
                            Description = project.Description,
                            UserType = registerField.Type,
                            CompanyLogo = registerField.ProfileAttach,
                            userid = project.userid,
                            ProfileUrl = registerField.ProfileUrl// company logo
                        });

                    }
                }

            }
            return listExcelProject;
        }

        public List<WorldRefSearchModel> ReturnInitialResultOnlyByIndustry(string[] industryFilter)
        {
            List<WorldRefSearchModel> listExcelProject = new List<WorldRefSearchModel>();
            List<ImageStructure> imageStructure = new List<ImageStructure>();
            List<LinkedInUser> appCredit = new List<LinkedInUser>();
            foreach (string industryName in industryFilter)
            {

                var projects = (from worldRefProject in context.WorldRefExcelDataProjects
                                where (worldRefProject.CustomerIndustryType == industryName) && worldRefProject.IsAuthorized == 1
                                select worldRefProject).AsEnumerable();

                foreach (var project in projects)
                {
                    imageStructure = new List<ImageStructure>();
                    appCredit = new List<LinkedInUser>();
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
                            id = ig.id,
                            ImageName = ig.ImageName,
                            ImagePath = ig.ImagePath,
                            ImgIndex = "0",
                            Link = ig.link
                        });
                    }

                    var likes = (from like in context.ProjectLikeDisLikes
                                 where like.projectId == project.id
                                 select like).FirstOrDefault();

                    string rating = "N/A";

                    rating = (from rate in context.ProjectRatings
                              where rate.projectId == project.id
                              select rate.rating).FirstOrDefault();

                    rating = !string.IsNullOrEmpty(rating) ? rating : "N/A";
                    if (rating == "1")
                    {
                        rating = "High";
                    }
                    else if (rating == "2")
                    {
                        rating = "Optimum";
                    }
                    else if (rating == "3")
                    {
                        rating = "Expensive";
                    }
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
                                      select new { r.picUrl, r.FirstName, r.Designation, r.Industry, r.id }).AsEnumerable();

                    foreach (var data in pictureUrl)
                    {
                        appCredit.Add(new LinkedInUser()
                        {
                            picUrl = data.picUrl,
                            Designation = data.Designation,
                            FirstName = data.FirstName,
                            Industry = data.Industry,
                            id = data.id

                        });
                    }

                    string Tlikes = "0";
                    string TDisLikes = "0";

                    if (likes != null)
                    {
                        Tlikes = likes.totalLike.ToString();
                        TDisLikes = likes.totalDislike;
                    }

                    dynamic registerField = (from rr in context.RegisterUsers where rr.Id == project.userid select new { rr.Type, rr.ProfileAttach, rr.ProfileUrl }).FirstOrDefault();


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
                            QualityRating = qualityrat,
                            UserRating = userrating,
                            CustomerIndustryType = project.CustomerIndustryType,
                            Status = status,
                            Description = project.Description,
                            UserType = registerField.Type,
                            CompanyLogo = registerField.ProfileAttach,
                            userid = project.userid,
                            ProfileUrl = registerField.ProfileUrl// company logo
                        });

                    }

                }
            }
            return listExcelProject;
        }

        public List<WorldRefSearchModel> ReturnInitialResultByIndustryAndCountry(string searchResult, string[] industryFilter, string[] countryFilter)
        {
            if (industryFilter.Count() >= countryFilter.Count())
            {
                return ReturnIndustryGreater(searchResult, industryFilter, countryFilter);
            }
            else
            {
                return ReturnCountryGreater(searchResult, industryFilter, countryFilter);
            }
        }

        public List<WorldRefSearchModel> ReturnIndustryGreater(string searchResult, string[] industryFilter, string[] countryFilter)
        {
            List<WorldRefSearchModel> listExcelProject = new List<WorldRefSearchModel>();

            List<ImageStructure> imageStructure = new List<ImageStructure>();
            List<LinkedInUser> appCredit = new List<LinkedInUser>();
            #region Industry Filter

            int i = 0;


            foreach (string IndustryName in industryFilter)
            {
                string countryFilterName = string.Empty;

                if (countryFilter.Count() > i)
                {
                    countryFilterName = countryFilter[i].ToString();
                    i++;
                }
                var projects = (from worldRefProject in context.WorldRefExcelDataProjects
                                join Keywords in context.ProjectSearchKeywords on worldRefProject.id equals Keywords.ProjectId
                                  into result
                                from rr in result.DefaultIfEmpty()
                                where (worldRefProject.OrganizationName.Contains(searchResult) ||
                                    worldRefProject.CustomerName.Contains(searchResult) || worldRefProject.ProjectName.Contains(searchResult) || worldRefProject.Type.Contains(searchResult) ||
                                    worldRefProject.Year.Contains(searchResult) || worldRefProject.Country.Contains(searchResult) || worldRefProject.CustomerIndustryType.Contains(searchResult)
                                    || worldRefProject.CustomerIndustryType.Contains(IndustryName) || worldRefProject.Country.Contains(countryFilterName)
                                    || worldRefProject.Status.Contains(searchResult) || (rr.SearchKeyword.Contains(searchResult) && rr.isDeleted == false)) && (worldRefProject.CustomerIndustryType == IndustryName || worldRefProject.Country == countryFilterName) && worldRefProject.IsAuthorized == 1
                                select worldRefProject).AsEnumerable();

                if (string.IsNullOrEmpty(searchResult))
                {
                    projects = (from worldRefProject in context.WorldRefExcelDataProjects
                                where (worldRefProject.CustomerIndustryType == IndustryName || worldRefProject.Country == countryFilterName) && worldRefProject.IsAuthorized == 1
                                select worldRefProject).AsEnumerable();
                }

                foreach (var project in projects)
                {
                    //int index = listExcelProject.FindIndex(item => item.id == project.id);
                    //if (index == -1)
                    //{

                    imageStructure = new List<ImageStructure>();
                    appCredit = new List<LinkedInUser>();
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
                            id = ig.id,
                            ImageName = ig.ImageName,
                            ImagePath = ig.ImagePath,
                            ImgIndex = "0",
                            Link = ig.link
                        });
                    }

                    var likes = (from like in context.ProjectLikeDisLikes
                                 where like.projectId == project.id
                                 select like).FirstOrDefault();

                    string rating = "N/A";

                    rating = (from rate in context.ProjectRatings
                              where rate.projectId == project.id
                              select rate.rating).FirstOrDefault();

                    rating = !string.IsNullOrEmpty(rating) ? rating : "N/A";
                    if (rating == "1")
                    {
                        rating = "High";
                    }
                    else if (rating == "2")
                    {
                        rating = "Optimum";
                    }
                    else if (rating == "3")
                    {
                        rating = "Expensive";
                    }
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
                                      select new { r.picUrl, r.FirstName, r.Designation, r.Industry, r.id }).AsEnumerable();

                    foreach (var data in pictureUrl)
                    {
                        appCredit.Add(new LinkedInUser()
                        {
                            picUrl = data.picUrl,
                            Designation = data.Designation,
                            FirstName = data.FirstName,
                            Industry = data.Industry,
                            id = data.id

                        });
                    }

                    string Tlikes = "0";
                    string TDisLikes = "0";

                    if (likes != null)
                    {
                        Tlikes = likes.totalLike.ToString();
                        TDisLikes = likes.totalDislike;
                    }

                    dynamic registerField = (from rr in context.RegisterUsers where rr.Id == project.userid select new { rr.Type, rr.ProfileAttach, rr.ProfileUrl }).FirstOrDefault();


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
                            QualityRating = qualityrat,
                            UserRating = userrating,
                            CustomerIndustryType = project.CustomerIndustryType,
                            Status = status,
                            Description = project.Description,
                            UserType = registerField.Type,
                            CompanyLogo = registerField.ProfileAttach,
                            userid = project.userid,
                            ProfileUrl = registerField.ProfileUrl// company logo
                        });

                    }
                    // }
                }
            }

            #endregion

            return listExcelProject;
        }

        public List<WorldRefSearchModel> ReturnCountryGreater(string searchResult, string[] industryFilter, string[] countryFilter)
        {
            List<WorldRefSearchModel> listExcelProject = new List<WorldRefSearchModel>();

            List<ImageStructure> imageStructure = new List<ImageStructure>();
            List<LinkedInUser> appCredit = new List<LinkedInUser>();
            #region Country Filter

            int i = 0;
            string industry = string.Empty;

            foreach (string CountryName in countryFilter)
            {
                if (industryFilter.Count() > i)
                {
                    industry = industryFilter[i].ToString();
                    i++;
                }



                var projects = (from worldRefProject in context.WorldRefExcelDataProjects
                                join Keywords in context.ProjectSearchKeywords on worldRefProject.id equals Keywords.ProjectId
                                  into result
                                from rr in result.DefaultIfEmpty()
                                where (worldRefProject.OrganizationName.Contains(searchResult) ||
                                    worldRefProject.CustomerName.Contains(searchResult) || worldRefProject.ProjectName.Contains(searchResult) || worldRefProject.Type.Contains(searchResult) ||
                                    worldRefProject.Year.Contains(searchResult) || worldRefProject.Country.Contains(searchResult) || worldRefProject.CustomerIndustryType.Contains(searchResult)
                                    || worldRefProject.CustomerIndustryType.Contains(industry) || worldRefProject.Country.Contains(CountryName)
                                    || worldRefProject.Status.Contains(searchResult) || (rr.SearchKeyword.Contains(searchResult) && rr.isDeleted == false)) && (worldRefProject.Country == CountryName || worldRefProject.CustomerIndustryType == industry) && worldRefProject.IsAuthorized == 1
                                select worldRefProject).AsEnumerable();

                if (string.IsNullOrEmpty(searchResult))
                {
                    projects = (from worldRefProject in context.WorldRefExcelDataProjects
                                where (worldRefProject.Country == CountryName || worldRefProject.CustomerIndustryType == industry) && worldRefProject.IsAuthorized == 1
                                select worldRefProject).AsEnumerable();
                }

                foreach (var project in projects)
                {
                    imageStructure = new List<ImageStructure>();
                    appCredit = new List<LinkedInUser>();
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
                            id = ig.id,
                            ImageName = ig.ImageName,
                            ImagePath = ig.ImagePath,
                            ImgIndex = "0",
                            Link = ig.link
                        });
                    }

                    var likes = (from like in context.ProjectLikeDisLikes
                                 where like.projectId == project.id
                                 select like).FirstOrDefault();

                    string rating = "N/A";

                    rating = (from rate in context.ProjectRatings
                              where rate.projectId == project.id
                              select rate.rating).FirstOrDefault();

                    rating = !string.IsNullOrEmpty(rating) ? rating : "N/A";
                    if (rating == "1")
                    {
                        rating = "High";
                    }
                    else if (rating == "2")
                    {
                        rating = "Optimum";
                    }
                    else if (rating == "3")
                    {
                        rating = "Expensive";
                    }
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
                                      select new { r.picUrl, r.FirstName, r.Designation, r.Industry, r.id }).AsEnumerable();

                    foreach (var data in pictureUrl)
                    {
                        appCredit.Add(new LinkedInUser()
                        {
                            picUrl = data.picUrl,
                            Designation = data.Designation,
                            FirstName = data.FirstName,
                            Industry = data.Industry,
                            id = data.id

                        });
                    }

                    string Tlikes = "0";
                    string TDisLikes = "0";

                    if (likes != null)
                    {
                        Tlikes = likes.totalLike.ToString();
                        TDisLikes = likes.totalDislike;
                    }

                    dynamic registerField = (from rr in context.RegisterUsers where rr.Id == project.userid select new { rr.Type, rr.ProfileAttach, rr.ProfileUrl }).FirstOrDefault();


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
                            QualityRating = qualityrat,
                            UserRating = userrating,
                            CustomerIndustryType = project.CustomerIndustryType,
                            Status = status,
                            Description = project.Description,
                            UserType = registerField.Type,
                            CompanyLogo = registerField.ProfileAttach,
                            userid = project.userid,
                            ProfileUrl = registerField.ProfileUrl// company logo
                        });

                    }
                }

            }

            #endregion

            return listExcelProject;
        }
        //public List<WorldRefSearchModel> DecideFilter(string SearchResult, string[] industryFilter, string[] countryFilter)
        //{
        //    List<WorldRefSearchModel> listModel = new List<WorldRefSearchModel>();

        //    if (!string.IsNullOrEmpty(SearchResult) && industryFilter == null && countryFilter == null)
        //    {
        //        listModel = ReturnInitialResult(SearchResult);
        //    }
        //    else if (!string.IsNullOrEmpty(SearchResult) && industryFilter == null && countryFilter != null)
        //    {
        //        listModel = ReturnInitialResultByCountry(SearchResult, countryFilter);
        //    }
        //    else if (string.IsNullOrEmpty(SearchResult) && industryFilter == null && countryFilter != null)
        //    {
        //        listModel = ReturnInitialResultOnlyByCountry(countryFilter);
        //    }
        //    else if (!string.IsNullOrEmpty(SearchResult) && industryFilter != null && countryFilter == null)
        //    {
        //        listModel = ReturnInitialResultByIndustry(SearchResult, industryFilter);
        //    }
        //    else if (string.IsNullOrEmpty(SearchResult) && industryFilter != null && countryFilter == null)
        //    {
        //        listModel = ReturnInitialResultOnlyByIndustry(industryFilter);
        //    }
        //    else if (!string.IsNullOrEmpty(SearchResult) && industryFilter != null && countryFilter != null)
        //    {
        //        listModel = ReturnInitialResultByIndustryAndCountry(SearchResult, industryFilter, countryFilter);
        //    }
        //    else if (string.IsNullOrEmpty(SearchResult) && industryFilter != null && countryFilter != null)
        //    {
        //        listModel = ReturnInitialResultByIndustryAndCountry(string.Empty, industryFilter, countryFilter);
        //    }
        //    return listModel;
        //}

        #endregion
        #region Serach project method
        public List<WorldRefSearchModel> DecideFilter(string splitVal)
        {
            List<WorldRefSearchModel> listModel = new List<WorldRefSearchModel>();
            List<WorldRefSearchModel> listExcelProject = new List<WorldRefSearchModel>();
            List<ImageStructure> imageStructure = new List<ImageStructure>();
            List<LinkedInUser> appCredit = new List<LinkedInUser>();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConString"].ConnectionString.ToString());
            DataSet ds = new DataSet();
            SqlCommand cmd3 = new SqlCommand();
            cmd3.CommandType = CommandType.StoredProcedure;
            cmd3.CommandText = "sp_search_Project";
            cmd3.Parameters.AddWithValue("@Type", splitVal);
            cmd3.Connection = con;
            con.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd3);
            adapter.Fill(ds);
            con.Close();
            var projects = ds.Tables[0].AsEnumerable();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                imageStructure = new List<ImageStructure>();
                appCredit = new List<LinkedInUser>();
                string organizationName = string.Empty;
                string customer = string.Empty;
                string projectName = string.Empty;
                string type = string.Empty;
                string year = string.Empty;
                string country = string.Empty;
                string emailid = string.Empty;
                string status = string.Empty;

                organizationName = dr["OrganizationName"].ToString();
                customer = dr["CustomerName"].ToString();
                projectName = dr["ProjectName"].ToString();
                type = dr["Type"].ToString();
                year = dr["Year"].ToString();
                country = dr["Country"].ToString();
                status = dr["Status"].ToString();
                string userType = string.Empty;

                int uid = Convert.ToInt32(dr["userid"]);
                int idA = Convert.ToInt32(dr["ProjectId"]);
                userType = (from rr in context.RegisterUsers where rr.Id == uid select rr.Type).FirstOrDefault();

                if (dr["IsEmail"].ToString() == "0")
                {
                    string emailOfUser = (from rr in context.RegisterUsers where rr.Id == uid select rr.Email).FirstOrDefault();

                    emailid = emailOfUser;
                }
                var img = (from img1 in context.ProjectImages
                           where (img1.ProjectId == idA)
                           select img1).AsEnumerable();

                foreach (var ig in img)
                {
                    imageStructure.Add(new ImageStructure()
                    {
                        id = ig.id,
                        ImageName = ig.ImageName,
                        ImagePath = ig.ImagePath,
                        ImgIndex = "0",
                        Link = ig.link
                    });
                }

                var likes = (from like in context.ProjectLikeDisLikes
                             where like.projectId == idA
                             select like).FirstOrDefault();

                string rating = "N/A";

                rating = (from rate in context.ProjectRatings
                          where rate.projectId == idA
                          select rate.rating).FirstOrDefault();

                rating = !string.IsNullOrEmpty(rating) ? rating : "N/A";
                if (rating == "1")
                {
                    rating = "High";
                }
                else if (rating == "2")
                {
                    rating = "Optimum";
                }
                else if (rating == "3")
                {
                    rating = "Expensive";
                }
                string qualityrat = "0";
                qualityrat = (from rate in context.ProjectRatings
                              where rate.projectId == idA
                              select rate.QualityRating).FirstOrDefault();
                qualityrat = !string.IsNullOrEmpty(qualityrat) ? qualityrat : "0";

                string userrating = "0";
                userrating = (from rate in context.ProjectRatings
                              where rate.projectId == idA
                              select rate.UserRating).FirstOrDefault();

                userrating = !string.IsNullOrEmpty(userrating) ? userrating : "0";

                var pictureUrl = (from r in context.LinkedInUsers
                                  where r.id == idA && r.CreditM == true
                                  select new { r.picUrl, r.FirstName, r.Designation, r.Industry, r.id }).AsEnumerable();

                foreach (var data in pictureUrl)
                {
                    appCredit.Add(new LinkedInUser()
                    {
                        picUrl = data.picUrl,
                        Designation = data.Designation,
                        FirstName = data.FirstName,
                        Industry = data.Industry,
                        id = data.id

                    });
                }

                string Tlikes = "0";
                string TDisLikes = "0";

                if (likes != null)
                {
                    Tlikes = likes.totalLike.ToString();
                    TDisLikes = likes.totalDislike;
                }

                dynamic registerField = (from rr in context.RegisterUsers where rr.Id == uid select new { rr.Type, rr.ProfileAttach, rr.ProfileUrl }).FirstOrDefault();


                int index = listExcelProject.FindIndex(x => x.id == idA);
                if (index == -1)
                {
                    listExcelProject.Add(new WorldRefSearchModel()
                    {
                        id = idA,
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
                        QualityRating = qualityrat,
                        UserRating = userrating,
                        CustomerIndustryType = dr["CustomerIndustryType"].ToString(),
                        Status = status,
                        Description = dr["Description"].ToString(),
                        UserType = registerField.Type,
                        CompanyLogo = registerField.ProfileAttach,
                        userid = uid,
                        ProfileUrl = registerField.ProfileUrl// company logo
                    });

                }

            }


            return listExcelProject;
        }
        #endregion
        #region Search Image Filter
        public List<WorldRefSearchModel> ReturnIntialResultByIndustryImage(string searchResult, string[] industryFilter)
        {
            List<WorldRefSearchModel> listImages = new List<WorldRefSearchModel>();

            foreach (string countryName in industryFilter)
            {
                var projects = (from worldRefProject in context.WorldRefExcelDataProjects
                                join projImg in context.ProjectImages on worldRefProject.id equals projImg.ProjectId
                                join Keywords in context.ProjectSearchKeywords on worldRefProject.id equals Keywords.ProjectId
                                   into result
                                from rr in result.DefaultIfEmpty()
                                where (((worldRefProject.OrganizationName.Contains(searchResult) ||
                                    worldRefProject.CustomerName.Contains(searchResult) || worldRefProject.ProjectName.Contains(searchResult) || worldRefProject.Type.Contains(searchResult) ||
                                    worldRefProject.Year.Contains(searchResult) || worldRefProject.Country.Contains(searchResult) || worldRefProject.CustomerIndustryType.Contains(searchResult)
                                    || worldRefProject.Status.Contains(searchResult)
                                    || (rr.SearchKeyword.Contains(searchResult) && rr.isDeleted == false)
                                    ) && (worldRefProject.CustomerIndustryType == countryName) && worldRefProject.IsAuthorized == 1)
                                    &&
                                    (((projImg.link == false) && ((projImg.ImagePath.Contains(".bmp"))
                                      || (projImg.ImagePath.Contains(".jpg")) || (projImg.ImagePath.Contains(".png"))))))
                                select new { projImg.ImageName, projImg.ImagePath, projImg.link, projImg.id, worldRefProject }).AsEnumerable();

                foreach (var project in projects)
                {
                    int index = listImages.FindIndex(x => x.ImageId == project.id);
                    if (index == -1)
                    {
                        string organizationName = string.Empty;
                        string customer = string.Empty;
                        string projectName = string.Empty;
                        string type = string.Empty;
                        string year = string.Empty;
                        string country = string.Empty;
                        string emailid = string.Empty;

                        if (project.worldRefProject.IsOrganization == false)
                        {
                            organizationName = project.worldRefProject.OrganizationName;
                        }
                        if (project.worldRefProject.IsCustomer == false)
                        {
                            customer = project.worldRefProject.CustomerName;
                        }
                        if (project.worldRefProject.IsProject == false)
                        {
                            projectName = project.worldRefProject.ProjectName;
                        }
                        if (project.worldRefProject.IsType == false)
                        {
                            type = project.worldRefProject.Type;
                        }
                        if (project.worldRefProject.IsYear == false)
                        {
                            year = project.worldRefProject.Year;
                        }
                        if (project.worldRefProject.IsCountry == false)
                        {
                            country = project.worldRefProject.Country;
                        }
                        if (project.worldRefProject.IsEmail == false)
                        {
                            string emailOfUser = (from rr in context.RegisterUsers where rr.Id == project.worldRefProject.userid select rr.Email).FirstOrDefault();
                            emailid = emailOfUser;
                        }

                        var abc = (from c in context.ImageLikeAndComments select c);
                        var likecount = 0;
                        var likflag = false;
                        if (abc.ToString() == "")
                        { }
                        else
                        {
                            foreach (var like in abc)
                            {
                                if (project.worldRefProject.id == like.ProjectId && project.id == like.PImageLikeId)
                                {
                                    int TotalLikeCount = (from s in context.ImageLikeAndComments
                                                          where s.ProjectId == project.worldRefProject.id && s.PImageLikeId == project.id && s.ImgLikeFlag == true
                                                          select s).Count();
                                    likecount = Convert.ToInt32(TotalLikeCount);
                                    likflag = (bool)(like.ImgLikeFlag);
                                }
                            }
                        }

                        //int? totalLikes = (from like in context.ProjectImagesVideosLikes
                        //                   where like.ProjectId == project.worldRefProject.id && like.ProjectImageId == project.id
                        //                   select like.TotalLikes).FirstOrDefault();
                        listImages.Add(new WorldRefSearchModel()
                        {
                            id = project.worldRefProject.id,
                            OrganizationName = organizationName,
                            ProjectName = projectName,
                            Country = country,
                            Type = type,
                            Year = year,
                            countApprove = likecount,
                            CustomerName = customer,
                            ImageName = project.ImageName,
                            ImagePath = project.ImagePath,
                            userid = project.worldRefProject.userid,
                            CustomerIndustryType = project.worldRefProject.CustomerIndustryType,
                            countflg = likflag.ToString(),
                            TotalLikes = !string.IsNullOrEmpty(likecount.ToString()) ? likecount.ToString() : "0"

                        });
                    }
                }

            }

            return listImages;
        }

        public List<WorldRefSearchModel> ReturnInitialResultOnlyByIndustryImage(string[] industryFilter)
        {
            List<WorldRefSearchModel> listImages = new List<WorldRefSearchModel>();

            foreach (string countryName in industryFilter)
            {
                var projects = (from worldRefProject in context.WorldRefExcelDataProjects
                                join projImg in context.ProjectImages on worldRefProject.id equals projImg.ProjectId
                                where (((worldRefProject.CustomerIndustryType == countryName) && worldRefProject.IsAuthorized == 1)
                                &&
                                ((projImg.link == false) && ((projImg.ImagePath.Contains(".bmp"))
                                             || (projImg.ImagePath.Contains(".jpg")) || (projImg.ImagePath.Contains(".png")))))
                                select new { projImg.ImageName, projImg.ImagePath, projImg.link, projImg.id, worldRefProject }).AsEnumerable();

                foreach (var project in projects)
                {
                    string organizationName = string.Empty;
                    string customer = string.Empty;
                    string projectName = string.Empty;
                    string type = string.Empty;
                    string year = string.Empty;
                    string country = string.Empty;
                    string emailid = string.Empty;

                    if (project.worldRefProject.IsOrganization == false)
                    {
                        organizationName = project.worldRefProject.OrganizationName;
                    }
                    if (project.worldRefProject.IsCustomer == false)
                    {
                        customer = project.worldRefProject.CustomerName;
                    }
                    if (project.worldRefProject.IsProject == false)
                    {
                        projectName = project.worldRefProject.ProjectName;
                    }
                    if (project.worldRefProject.IsType == false)
                    {
                        type = project.worldRefProject.Type;
                    }
                    if (project.worldRefProject.IsYear == false)
                    {
                        year = project.worldRefProject.Year;
                    }
                    if (project.worldRefProject.IsCountry == false)
                    {
                        country = project.worldRefProject.Country;
                    }
                    if (project.worldRefProject.IsEmail == false)
                    {
                        string emailOfUser = (from rr in context.RegisterUsers where rr.Id == project.worldRefProject.userid select rr.Email).FirstOrDefault();
                        emailid = emailOfUser;
                    }
                    var abc = (from c in context.ImageLikeAndComments select c);
                    var likecount = 0;
                    var likflag = false;
                    if (abc.ToString() == "")
                    { }
                    else
                    {
                        foreach (var like in abc)
                        {
                            if (project.worldRefProject.id == like.ProjectId && project.id == like.PImageLikeId)
                            {
                                int TotalLikeCount = (from s in context.ImageLikeAndComments
                                                      where s.ProjectId == project.worldRefProject.id && s.PImageLikeId == project.id && s.ImgLikeFlag == true
                                                      select s).Count();
                                likecount = Convert.ToInt32(TotalLikeCount);
                                likflag = (bool)(like.ImgLikeFlag);
                            }
                        }
                    }

                    //int? totalLikes = (from like in context.ProjectImagesVideosLikes
                    //                   where like.ProjectId == project.worldRefProject.id && like.ProjectImageId == project.id
                    //                   select like.TotalLikes).FirstOrDefault();
                    int index = listImages.FindIndex(x => x.ImageId == project.id);
                    if (index == -1)
                    {
                        listImages.Add(new WorldRefSearchModel()
                        {
                            id = project.worldRefProject.id,
                            ImageId = project.id,
                            OrganizationName = organizationName,
                            ProjectName = projectName,
                            Country = country,
                            Type = type,
                            countApprove = likecount,
                            Year = year,
                            CustomerName = customer,
                            ImageName = project.ImageName,
                            ImagePath = project.ImagePath,
                            userid = project.worldRefProject.userid,
                            CustomerIndustryType = project.worldRefProject.CustomerIndustryType,
                            countflg = likflag.ToString(),
                            TotalLikes = !string.IsNullOrEmpty(likecount.ToString()) ? likecount.ToString() : "0"
                        });
                    }
                }

            }

            return listImages;
        }

        public List<WorldRefSearchModel> ReturnInitialResultByIndustryAndCountryImage(string searchResult, string[] industryFilter, string[] countryFilter)
        {
            if (industryFilter.Count() >= countryFilter.Count())
            {
                return ReturnIndustryGreaterImage(searchResult, industryFilter, countryFilter);
            }
            else
            {
                return ReturnCountryGreaterImage(searchResult, industryFilter, countryFilter);
            }
        }

        public List<WorldRefSearchModel> ReturnIndustryGreaterImage(string searchResult, string[] industryFilter, string[] countryFilter)
        {
            List<WorldRefSearchModel> listImages = new List<WorldRefSearchModel>();
            int i = 0;
            foreach (string industryName in industryFilter)
            {
                string countryFilterName = string.Empty;

                if (countryFilter.Count() > i)
                {
                    countryFilterName = countryFilter[i].ToString();
                    i++;
                }
                var projects = (from worldRefProject in context.WorldRefExcelDataProjects
                                join projImg in context.ProjectImages on worldRefProject.id equals projImg.ProjectId
                                join Keywords in context.ProjectSearchKeywords on worldRefProject.id equals Keywords.ProjectId
                                   into result
                                from rr in result.DefaultIfEmpty()
                                where (((worldRefProject.OrganizationName.Contains(searchResult) ||
                                    worldRefProject.CustomerName.Contains(searchResult) || worldRefProject.ProjectName.Contains(searchResult) || worldRefProject.Type.Contains(searchResult) ||
                                    worldRefProject.Year.Contains(searchResult) || worldRefProject.Country.Contains(searchResult) || worldRefProject.CustomerIndustryType.Contains(searchResult)
                                    || worldRefProject.Status.Contains(searchResult) || worldRefProject.CustomerIndustryType.Contains(industryName) || worldRefProject.Country.Contains(countryFilterName)
                                    || (rr.SearchKeyword.Contains(searchResult) && rr.isDeleted == false)
                                    ) && (worldRefProject.Country == countryFilterName || worldRefProject.CustomerIndustryType == industryName) && worldRefProject.IsAuthorized == 1)
                                    &&
                                    (((projImg.link == false) && ((projImg.ImagePath.Contains(".bmp"))
                                      || (projImg.ImagePath.Contains(".jpg")) || (projImg.ImagePath.Contains(".png"))))))
                                select new { projImg.ImageName, projImg.ImagePath, projImg.link, projImg.id, worldRefProject }).AsEnumerable();

                foreach (var project in projects)
                {
                    string organizationName = string.Empty;
                    string customer = string.Empty;
                    string projectName = string.Empty;
                    string type = string.Empty;
                    string year = string.Empty;
                    string country = string.Empty;
                    string emailid = string.Empty;

                    if (project.worldRefProject.IsOrganization == false)
                    {
                        organizationName = project.worldRefProject.OrganizationName;
                    }
                    if (project.worldRefProject.IsCustomer == false)
                    {
                        customer = project.worldRefProject.CustomerName;
                    }
                    if (project.worldRefProject.IsProject == false)
                    {
                        projectName = project.worldRefProject.ProjectName;
                    }
                    if (project.worldRefProject.IsType == false)
                    {
                        type = project.worldRefProject.Type;
                    }
                    if (project.worldRefProject.IsYear == false)
                    {
                        year = project.worldRefProject.Year;
                    }
                    if (project.worldRefProject.IsCountry == false)
                    {
                        country = project.worldRefProject.Country;
                    }
                    if (project.worldRefProject.IsEmail == false)
                    {
                        string emailOfUser = (from rr in context.RegisterUsers where rr.Id == project.worldRefProject.userid select rr.Email).FirstOrDefault();
                        emailid = emailOfUser;
                    }
                    var abc = (from c in context.ImageLikeAndComments select c);
                    var likecount = 0;
                    var likflag = false;
                    if (abc.ToString() == "")
                    { }
                    else
                    {
                        foreach (var like in abc)
                        {
                            if (project.worldRefProject.id == like.ProjectId && project.id == like.PImageLikeId)
                            {
                                int TotalLikeCount = (from s in context.ImageLikeAndComments
                                                      where s.ProjectId == project.worldRefProject.id && s.PImageLikeId == project.id && s.ImgLikeFlag == true
                                                      select s).Count();
                                likecount = Convert.ToInt32(TotalLikeCount);
                                likflag = (bool)(like.ImgLikeFlag);
                            }
                        }
                    }

                    //int? totalLikes = (from like in context.ProjectImagesVideosLikes
                    //                   where like.ProjectId == project.worldRefProject.id && like.ProjectImageId == project.id
                    //                   select like.TotalLikes).FirstOrDefault();
                    int index = listImages.FindIndex(x => x.ImageId == project.id);
                    if (index == -1)
                    {
                        listImages.Add(new WorldRefSearchModel()
                        {
                            id = project.worldRefProject.id,
                            ImageId = project.id,
                            OrganizationName = organizationName,
                            ProjectName = projectName,
                            Country = country,
                            Type = type,
                            Year = year,
                            countApprove = likecount,
                            CustomerName = customer,
                            ImageName = project.ImageName,
                            ImagePath = project.ImagePath,
                            userid = project.worldRefProject.userid,
                            CustomerIndustryType = project.worldRefProject.CustomerIndustryType,
                            countflg = likflag.ToString(),
                            TotalLikes = !string.IsNullOrEmpty(likecount.ToString()) ? likecount.ToString() : "0"
                        });
                    }
                }

            }

            return listImages;
        }

        public List<WorldRefSearchModel> ReturnCountryGreaterImage(string searchResult, string[] industryFilter, string[] countryFilter)
        {
            List<WorldRefSearchModel> listImages = new List<WorldRefSearchModel>();
            int i = 0;
            foreach (string countryName in countryFilter)
            {
                string industryFilterName = string.Empty;

                if (industryFilter.Count() > i)
                {
                    industryFilterName = industryFilter[i].ToString();
                    i++;
                }
                var projects = (from worldRefProject in context.WorldRefExcelDataProjects
                                join projImg in context.ProjectImages on worldRefProject.id equals projImg.ProjectId
                                join Keywords in context.ProjectSearchKeywords on worldRefProject.id equals Keywords.ProjectId
                                  into result
                                from rr in result.DefaultIfEmpty()
                                where (((worldRefProject.OrganizationName.Contains(searchResult) ||
                                    worldRefProject.CustomerName.Contains(searchResult) || worldRefProject.ProjectName.Contains(searchResult) || worldRefProject.Type.Contains(searchResult) ||
                                    worldRefProject.Year.Contains(searchResult) || worldRefProject.Country.Contains(searchResult) || worldRefProject.CustomerIndustryType.Contains(searchResult)
                                    || worldRefProject.Status.Contains(searchResult)
                                    || (rr.SearchKeyword.Contains(searchResult) && rr.isDeleted == false) || worldRefProject.CustomerIndustryType.Contains(industryFilterName) || worldRefProject.Country.Contains(countryName)
                                    ) && (worldRefProject.Country == countryName || worldRefProject.CustomerIndustryType == industryFilterName) && worldRefProject.IsAuthorized == 1)
                                    &&
                                    (((projImg.link == false) && ((projImg.ImagePath.Contains(".bmp"))
                                      || (projImg.ImagePath.Contains(".jpg")) || (projImg.ImagePath.Contains(".png"))))))
                                select new { projImg.ImageName, projImg.ImagePath, projImg.link, projImg.id, worldRefProject }).AsEnumerable();

                foreach (var project in projects)
                {
                    string organizationName = string.Empty;
                    string customer = string.Empty;
                    string projectName = string.Empty;
                    string type = string.Empty;
                    string year = string.Empty;
                    string country = string.Empty;
                    string emailid = string.Empty;

                    if (project.worldRefProject.IsOrganization == false)
                    {
                        organizationName = project.worldRefProject.OrganizationName;
                    }
                    if (project.worldRefProject.IsCustomer == false)
                    {
                        customer = project.worldRefProject.CustomerName;
                    }
                    if (project.worldRefProject.IsProject == false)
                    {
                        projectName = project.worldRefProject.ProjectName;
                    }
                    if (project.worldRefProject.IsType == false)
                    {
                        type = project.worldRefProject.Type;
                    }
                    if (project.worldRefProject.IsYear == false)
                    {
                        year = project.worldRefProject.Year;
                    }
                    if (project.worldRefProject.IsCountry == false)
                    {
                        country = project.worldRefProject.Country;
                    }
                    if (project.worldRefProject.IsEmail == false)
                    {
                        string emailOfUser = (from rr in context.RegisterUsers where rr.Id == project.worldRefProject.userid select rr.Email).FirstOrDefault();
                        emailid = emailOfUser;
                    }
                    var abc = (from c in context.ImageLikeAndComments select c);
                    var likecount = 0;
                    var likflag = false;
                    if (abc.ToString() == "")
                    { }
                    else
                    {
                        foreach (var like in abc)
                        {
                            if (project.worldRefProject.id == like.ProjectId && project.id == like.PImageLikeId)
                            {
                                int TotalLikeCount = (from s in context.ImageLikeAndComments
                                                      where s.ProjectId == project.worldRefProject.id && s.PImageLikeId == project.id && s.ImgLikeFlag == true
                                                      select s).Count();
                                likecount = Convert.ToInt32(TotalLikeCount);
                                likflag = (bool)(like.ImgLikeFlag);
                            }
                        }
                    }

                    //int? totalLikes = (from like in context.ProjectImagesVideosLikes
                    //                   where like.ProjectId == project.worldRefProject.id && like.ProjectImageId == project.id
                    //                   select like.TotalLikes).FirstOrDefault();
                    int index = listImages.FindIndex(x => x.ImageId == project.id);
                    if (index == -1)
                    {
                        listImages.Add(new WorldRefSearchModel()
                        {
                            id = project.worldRefProject.id,
                            ImageId = project.id,
                            OrganizationName = organizationName,
                            ProjectName = projectName,
                            Country = country,
                            Type = type,
                            countApprove = likecount,
                            Year = year,
                            CustomerName = customer,
                            ImageName = project.ImageName,
                            userid = project.worldRefProject.userid,
                            ImagePath = project.ImagePath,
                            CustomerIndustryType = project.worldRefProject.CustomerIndustryType,
                            countflg = likflag.ToString(),
                            TotalLikes = !string.IsNullOrEmpty(likecount.ToString()) ? likecount.ToString() : "0"
                        });
                    }
                }

            }

            return listImages;
        }

        public List<WorldRefSearchModel> DecideFilterImage(string SearchResult, string[] industryFilter, string[] countryFilter)
        {
            List<WorldRefSearchModel> listModel = new List<WorldRefSearchModel>();

            if (!string.IsNullOrEmpty(SearchResult) && industryFilter == null && countryFilter == null)
            {
                listModel = ReturnFilterImages(SearchResult);
            }
            else if (!string.IsNullOrEmpty(SearchResult) && industryFilter == null && countryFilter != null)
            {
                listModel = ReturnFilterImages(SearchResult, countryFilter);
            }
            else if (string.IsNullOrEmpty(SearchResult) && industryFilter == null && countryFilter != null)
            {
                listModel = ReturnFilterImages(countryFilter);
            }
            else if (!string.IsNullOrEmpty(SearchResult) && industryFilter != null && countryFilter == null)
            {
                listModel = ReturnIntialResultByIndustryImage(SearchResult, industryFilter);
            }
            else if (string.IsNullOrEmpty(SearchResult) && industryFilter != null && countryFilter == null)
            {
                listModel = ReturnInitialResultOnlyByIndustryImage(industryFilter);
            }
            else if (!string.IsNullOrEmpty(SearchResult) && industryFilter != null && countryFilter != null)
            {
                listModel = ReturnInitialResultByIndustryAndCountryImage(SearchResult, industryFilter, countryFilter);
            }
            else if (string.IsNullOrEmpty(SearchResult) && industryFilter != null && countryFilter != null)
            {
                listModel = ReturnInitialResultByIndustryAndCountryImage(string.Empty, industryFilter, countryFilter);
            }
            //else
            //{
            //    listModel = ReturnFilterImages(SearchResult);
            //}
            return listModel;
        }

        #endregion
        #region serach image on project
        public List<WorldRefSearchModel> DecideFilterOnImage(string splitVal)
        {
            List<WorldRefSearchModel> listImages = new List<WorldRefSearchModel>();
            List<WorldRefSearchModel> listModel = new List<WorldRefSearchModel>();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConString"].ConnectionString.ToString());
            DataSet ds = new DataSet();
            SqlCommand cmd3 = new SqlCommand();
            cmd3.CommandType = CommandType.StoredProcedure;
            cmd3.CommandText = "sp_search_Image";
            cmd3.Parameters.AddWithValue("@Type", splitVal);
            cmd3.Connection = con;
            con.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd3);
            adapter.Fill(ds);
            con.Close();
            var projects = ds.Tables[0].AsEnumerable();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string organizationName = string.Empty;
                string customer = string.Empty;
                string projectName = string.Empty;
                string type = string.Empty;
                string year = string.Empty;
                string country = string.Empty;
                string emailid = string.Empty;
                
                int uid = Convert.ToInt32(dr["userid"]);
                int idA = Convert.ToInt32(dr["id"]);
                int Pid = Convert.ToInt32(dr["ProjectId"]);
                if ( dr["IsOrganization"].ToString() == "0")
                {
                   
                    organizationName =dr["OrganizationName"].ToString();
                }
                if (dr["IsCustomer"].ToString() == "0")
                {
                    customer =dr["CustomerName"].ToString();
                }
                if (dr["IsProject"].ToString() == "0")
                {
                    projectName =dr["ProjectName"].ToString();
                }
                if (dr["IsType"].ToString() == "0")
                {
                    type =dr["Type"].ToString();
                }
                if (dr["IsYear"].ToString() == "0")
                {
                    year =dr["Year"].ToString();
                }
                if (dr["IsCountry"].ToString() == "0")
                {
                    country =dr["Country"].ToString();
                }
                if (dr["IsEmail"].ToString() == "0")
                {
                    string emailOfUser = (from rr in context.RegisterUsers where rr.Id == uid select rr.Email).FirstOrDefault();
                    emailid = emailOfUser;
                }
                var abc = (from c in context.ImageLikeAndComments select c).AsEnumerable();
                var likecount = 0;
                var likflag = false;
                if (abc.ToString() == "")
                { }
                else
                {
                    foreach (var like in abc)
                    {
                        if (idA == like.ProjectId && Pid == like.ImageId)
                        {
                            int TotalLikeCount = (from s in context.ImageLikeAndComments
                                                  where s.ProjectId == idA && s.ImageId == Pid && s.ImgLikeFlag == true
                                                  select s).Count();
                            likecount = Convert.ToInt32(TotalLikeCount);
                            likflag = (bool)(like.ImgLikeFlag);
                        }
                    }
                }
                //int index = listImages.FindIndex(x => x.ImageId == Pid);

                //if (index == -1)
                //{
                    listImages.Add(new WorldRefSearchModel()
                    {
                        id = idA,
                        ImageId = Pid,
                        OrganizationName =dr["OrganizationName"].ToString(),
                        ProjectName = dr["ProjectName"].ToString(),
                        Country = dr["Country"].ToString(),
                        Type = dr["Type"].ToString(),
                        Year = dr["Year"].ToString(),
                        userid = uid,
                        CustomerName = dr["CustomerName"].ToString(),
                        ImageName = dr["ImageName"].ToString(),
                        ImagePath = dr["ImagePath"].ToString(),
                        CustomerIndustryType = dr["CustomerIndustryType"].ToString(),
                        countflg = likflag.ToString(),
                        TotalLikes = !string.IsNullOrEmpty(likecount.ToString()) ? likecount.ToString() : "0"
                    });
                //}
            }

            return listImages;
        }


      
        #endregion
        #region code for search videos on worldref added by rahul shukla date :16-11-15
        public List<WorldRefSearchModel> ReturnFilterVideos(string splitVal)
        {
            
            List<WorldRefSearchModel> listImages = new List<WorldRefSearchModel>();
            List<WorldRefSearchModel> listModel = new List<WorldRefSearchModel>();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConString"].ConnectionString.ToString());
            DataSet ds = new DataSet();
            SqlCommand cmd3 = new SqlCommand();
            cmd3.CommandType = CommandType.StoredProcedure;
            cmd3.CommandText = "sp_search_Videos";
            cmd3.Parameters.AddWithValue("@Type", splitVal);
            cmd3.Connection = con;
            con.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd3);
            adapter.Fill(ds);
            con.Close();
            var projects = ds.Tables[0].AsEnumerable();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                    string organizationName = string.Empty;
                    string customer = string.Empty;
                    string projectName = string.Empty;
                    string type = string.Empty;
                    string year = string.Empty;
                    string country = string.Empty;
                    string emailid = string.Empty;
                    int uid = Convert.ToInt32(dr["userid"]);
                    int idA = Convert.ToInt32(dr["id"]);
                    int Pid = Convert.ToInt32(dr["ProjectId"]);
                    if (dr["IsOrganization"].ToString() == "0")
                    {

                        organizationName = dr["OrganizationName"].ToString();
                    }
                    if (dr["IsCustomer"].ToString() == "0")
                    {
                        customer = dr["CustomerName"].ToString();
                    }
                    if (dr["IsProject"].ToString() == "0")
                    {
                        projectName = dr["ProjectName"].ToString();
                    }
                    if (dr["IsType"].ToString() == "0")
                    {
                        type = dr["Type"].ToString();
                    }
                    if (dr["IsYear"].ToString() == "0")
                    {
                        year = dr["Year"].ToString();
                    }
                    if (dr["IsCountry"].ToString() == "0")
                    {
                        country = dr["Country"].ToString();
                    }
                    if (dr["IsEmail"].ToString() == "0")
                    {
                        string emailOfUser = (from rr in context.RegisterUsers where rr.Id == uid select rr.Email).FirstOrDefault();
                        emailid = emailOfUser;
                    }
                    var abc = (from c in context.VideosLikeAndComments select c);//find all data from VideosLikeAndComments
                    var likecount = 0;   //likecount is a var intilised by 0.
                    var likflag = false; //likflag is a var intilised by false.
                    if (abc.ToString() == "")//check if var abc is null.
                    { }
                    else
                    {

                        foreach (var like in abc)
                        {
                            if (idA == like.ProjectId && Pid == like.VideosId)
                            {
                                int TotalLikeCount = (from s in context.VideosLikeAndComments
                                                      where s.ProjectId == idA && s.VideosId == Pid && s.VideosLikeFlag == true
                                                      select s).Count();
                                likecount = Convert.ToInt32(TotalLikeCount);
                                likflag = (bool)(like.VideosLikeFlag);
                            }
                        }
                    }

                    //int index = listImages.FindIndex(x => x.ImageId == Pid);
                    //if (index == -1)
                    //{
                        listImages.Add(new WorldRefSearchModel()//add total details in listimages.
                        {
                        id = idA,
                        ImageId = Pid,
                        OrganizationName =dr["OrganizationName"].ToString(),
                        ProjectName = dr["ProjectName"].ToString(),
                        Country = dr["Country"].ToString(),
                        Type = dr["Type"].ToString(),
                        Year = dr["Year"].ToString(),
                        userid = uid,
                        CustomerName = dr["CustomerName"].ToString(),
                        ImageName = dr["ImageName"].ToString(),
                        ImagePath = dr["ImagePath"].ToString(),
                        Link = Convert.ToBoolean(dr["Link"]),
                        CustomerIndustryType = dr["CustomerIndustryType"].ToString(),
                        countflg = likflag.ToString(),
                        TotalLikes = !string.IsNullOrEmpty(likecount.ToString()) ? likecount.ToString() : "0"
                        });
                   // }
                }
            return listImages;//return list of count.
        }
        #endregion
    }
}