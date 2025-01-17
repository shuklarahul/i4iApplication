﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WorldRef.Models;

namespace WorldRef.BusinessLayer
{

    /// <summary>
    /// Search World Ref will be responsible for all the 
    /// search opertion .
    /// </summary>
    public class SearchWorldRef
    {
        protected I4IDBEntities context = new I4IDBEntities();

        #region Search Filter

        public List<WorldRefSearchModel> ReturnInitialResult(string searchResult)
        {
            List<WorldRefSearchModel> listExcelProject = new List<WorldRefSearchModel>();
            List<ImageStructure> imageStructure = new List<ImageStructure>();
            List<LinkedInUser> appCredit = new List<LinkedInUser>();
            //live running code start
            var projects = (from worldRefProject in context.WorldRefExcelDataProjects
                            join Keywords in context.ProjectSearchKeywords on worldRefProject.id equals Keywords.ProjectId into result
                            from rr in result.DefaultIfEmpty()
                            where ((worldRefProject.OrganizationName.Contains(searchResult) ||
                                worldRefProject.CustomerName.Contains(searchResult) || worldRefProject.ProjectName.Contains(searchResult) || worldRefProject.Type.Contains(searchResult) ||
                                worldRefProject.Year.Contains(searchResult) || worldRefProject.Country.Contains(searchResult)
                                || worldRefProject.CustomerIndustryType.Contains(searchResult)
                                    || worldRefProject.Status.Contains(searchResult) || (rr.SearchKeyword.Contains(searchResult) && rr.isDeleted == false)) && worldRefProject.IsAuthorized == 1)
                            select worldRefProject).AsEnumerable();
            //end
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


            return listExcelProject;
        }

        public List<WorldRefSearchModel> ReturnInitialResultByCountry(string searchResult, string[] countryFilter)
        {
            List<WorldRefSearchModel> listExcelProject = new List<WorldRefSearchModel>();

            List<ImageStructure> imageStructure = new List<ImageStructure>();
            List<LinkedInUser> appCredit = new List<LinkedInUser>();
            if (countryFilter == null && !string.IsNullOrEmpty(searchResult))
            {
                return ReturnInitialResult(searchResult);
            }
            else if (string.IsNullOrEmpty(searchResult) && countryFilter != null)
            {
                return ReturnInitialResultOnlyByCountry(countryFilter);
            }
            else if (!string.IsNullOrEmpty(searchResult) && countryFilter != null)
            {
                foreach (string countryName in countryFilter)
                {

                    var projects = (from worldRefProject in context.WorldRefExcelDataProjects
                                    join Keywords in context.ProjectSearchKeywords on worldRefProject.id equals Keywords.ProjectId into result
                                    from rr in result.DefaultIfEmpty()
                                    where (worldRefProject.OrganizationName.Contains(searchResult) ||
                                        worldRefProject.CustomerName.Contains(searchResult) || worldRefProject.ProjectName.Contains(searchResult) || worldRefProject.Type.Contains(searchResult) ||
                                        worldRefProject.Year.Contains(searchResult) || worldRefProject.Country.Contains(searchResult)
                                        || worldRefProject.CustomerIndustryType.Contains(searchResult)
                                    || worldRefProject.Status.Contains(searchResult) || (rr.SearchKeyword.Contains(searchResult) && rr.isDeleted == false)) && (worldRefProject.Country == countryName) && worldRefProject.IsAuthorized == 1
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
            }
            return listExcelProject;
        }

        public List<WorldRefSearchModel> ReturnInitialResultOnlyByCountry(string[] countryFilter)
        {
            List<WorldRefSearchModel> listExcelProject = new List<WorldRefSearchModel>();
            List<ImageStructure> imageStructure = new List<ImageStructure>();
            List<LinkedInUser> appCredit = new List<LinkedInUser>();
            foreach (string countryName in countryFilter)
            {

                var projects = (from worldRefProject in context.WorldRefExcelDataProjects
                                where (worldRefProject.Country == countryName) && worldRefProject.IsAuthorized == 1
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

        public List<WorldRefExcelDataModel> ReturnCountryOnly()
        {
            List<WorldRefExcelDataModel> listExcelProject = new List<WorldRefExcelDataModel>();

            var Countries = (from Coun in context.WorldRefExcelDataProjects where Coun.IsAuthorized == 1 select Coun).AsEnumerable();

            var countt = Countries.GroupBy(x => x.Country).Select(x => x.First());

            foreach (var country in countt)
            {
                if (!string.IsNullOrEmpty(country.Country))
                {
                    int index = listExcelProject.FindIndex(x => x.Country.ToLower() == country.Country.ToLower());
                    if (index == -1)
                    {
                        listExcelProject.Add(new WorldRefExcelDataModel()
                        {
                            Country = country.Country
                        });
                    }
                }

            }

            return listExcelProject;
        }

        #endregion

        #region Search Image Filter

        //public List<WorldRefSearchModel> ReturnFilterImages()
        //{
        //    List<WorldRefSearchModel> listImages = new List<WorldRefSearchModel>();


        //    //var projects = (from worldRefProject in context.WorldRefExcelDataProjects
        //    //                join Keywords in context.ProjectSearchKeywords on worldRefProject.id equals Keywords.ProjectId into result
        //    //                from rr in result.DefaultIfEmpty()
        //    //                where ((worldRefProject.OrganizationName.Contains(searchResult) ||
        //    //                    worldRefProject.CustomerName.Contains(searchResult) || worldRefProject.ProjectName.Contains(searchResult) || worldRefProject.Type.Contains(searchResult) ||
        //    //                    worldRefProject.Year.Contains(searchResult) || worldRefProject.Country.Contains(searchResult)
        //    //                    || worldRefProject.CustomerIndustryType.Contains(searchResult)
        //    //                        || worldRefProject.Status.Contains(searchResult) || (rr.SearchKeyword.Contains(searchResult) && rr.isDeleted == false)) && worldRefProject.IsAuthorized == 1)
        //    //                select worldRefProject).AsEnumerable();


        //    var projects = (from excel in context.WorldRefExcelDataProjects
        //                    join image in context.ProjectImages on excel.id equals image.ProjectId into result
        //                    from rr in result.DefaultIfEmpty()
        //                    where ((excel.OrganizationName.Contains(") ||
        //                        worldRefProject.CustomerName.Contains(searchResult) || worldRefProject.ProjectName.Contains(searchResult) || worldRefProject.Type.Contains(searchResult) ||
        //                        worldRefProject.Year.Contains(searchResult) || worldRefProject.Country.Contains(searchResult)
        //                        || worldRefProject.CustomerIndustryType.Contains(searchResult)
        //                            || worldRefProject.Status.Contains(searchResult) || (rr.SearchKeyword.Contains(searchResult) && rr.isDeleted == false)) && worldRefProject.IsAuthorized == 1)
        //                    select worldRefProject).AsEnumerable();


        //    //var projectImages = (from image in context.ProjectImages
        //    //                     join excel in context.WorldRefExcelDataProjects on image.ProjectId equals excel.id
        //    //                     where excel.IsAuthorized == 1 && ((image.link == false) && ((image.ImagePath.Contains(".bmp"))
        //    //                          || (image.ImagePath.Contains(".jpg")) || (image.ImagePath.Contains(".png"))))
        //    //                     select new { image, excel }).AsEnumerable();

        //    foreach (var project in projectImages)
        //    {
        //        string organizationName = string.Empty;
        //        string customer = string.Empty;
        //        string projectName = string.Empty;
        //        string type = string.Empty;
        //        string year = string.Empty;
        //        string country = string.Empty;
        //        string emailid = string.Empty;               

        //        if (project.excel.IsOrganization == false)
        //        {
        //            organizationName = project.excel.OrganizationName;
        //        }
        //        if (project.excel.IsCustomer == false)
        //        {
        //            customer = project.excel.CustomerName;
        //        }
        //        if (project.excel.IsProject == false)
        //        {
        //            projectName = project.excel.ProjectName;
        //        }
        //        if (project.excel.IsType == false)
        //        {
        //            type = project.excel.Type;
        //        }
        //        if (project.excel.IsYear == false)
        //        {
        //            year = project.excel.Year;
        //        }
        //        if (project.excel.IsCountry == false)
        //        {
        //            country = project.excel.Country;
        //        }
        //        if (project.excel.IsEmail == false)
        //        {
        //            string emailOfUser = (from rr in context.RegisterUsers where rr.Id == project.excel.userid select rr.Email).FirstOrDefault();
        //            emailid = emailOfUser;
        //        }
        //        var abc = (from c in context.ProjectLikeDisLikes select c);


        //        //int? totalLikes = (from like in context.ProjectImagesVideosLikes 
        //        //            where like.ProjectId==project.excel.id && like.ProjectImageId==project.image.id
        //        //                select like.TotalLikes).FirstOrDefault();

        //        var likecount = 0;
        //        var likflag = false;
        //        foreach (var like in abc)
        //        {
        //            if (project.excel.id == like.projectId)
        //            {
        //                likecount = Convert.ToInt32(like.totalLike);
        //                likflag = (bool)(like.flag);
        //            }
        //        }
        //            int index = listImages.FindIndex(x => x.ImageId == project.image.id);
        //            if (index == -1)
        //            {
        //                listImages.Add(new WorldRefSearchModel()
        //                {
        //                    id = project.image.ProjectId,
        //                    ImageId = project.image.id,
        //                    OrganizationName = organizationName,
        //                    ProjectName = projectName,
        //                    Country = country,
        //                    Type = type,
        //                    Year = year,
        //                    countApprove = Convert.ToInt32(likecount),
        //                    countflg = likflag.ToString(),
        //                    CustomerName = customer,
        //                    ImageName = project.image.ImageName,
        //                    ImagePath = project.image.ImagePath,

        //                    CustomerIndustryType = project.excel.CustomerIndustryType
        //                    //TotalLikes = !string.IsNullOrEmpty(totalLikes.ToString()) ? totalLikes.ToString() : "0"
        //                });
        //            }
        //    }

        //    return listImages;
        //}

        public List<WorldRefSearchModel> ReturnFilterImages(string searchResult, string[] countryFilter)
        {
            List<WorldRefSearchModel> listImages = new List<WorldRefSearchModel>();
            if (countryFilter == null && string.IsNullOrEmpty(searchResult))
            {
                return ReturnFilterImages(searchResult);
            }
            else if (countryFilter != null && string.IsNullOrEmpty(searchResult))
            {
                return ReturnFilterImages(countryFilter);
            }
            else if (countryFilter == null && !string.IsNullOrEmpty(searchResult))
            {
                return ReturnFilterImages(searchResult);
            }

            foreach (string countryName in countryFilter)
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
                                    ) && (worldRefProject.Country == countryName) && worldRefProject.IsAuthorized == 1)
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
                            CustomerName = customer,
                            ImageName = project.ImageName,
                            ImagePath = project.ImagePath,
                            CustomerIndustryType = project.worldRefProject.CustomerIndustryType,
                            TotalLikes = !string.IsNullOrEmpty(likecount.ToString()) ? likecount.ToString() : "0"
                        });
                    }
                }

            }

            return listImages;
        }

        public List<WorldRefSearchModel> ReturnFilterImages(string searchResult)
        {
            List<WorldRefSearchModel> listImages = new List<WorldRefSearchModel>();
            var projects = (from worldRefProject in context.WorldRefExcelDataProjects
                            join projImg in context.ProjectImages on worldRefProject.id equals projImg.ProjectId
                            join Keywords in context.ProjectSearchKeywords on worldRefProject.id equals Keywords.ProjectId
                              into result
                            from rr in result.DefaultIfEmpty()
                            where (((worldRefProject.OrganizationName.Contains(searchResult) ||
                                worldRefProject.CustomerName.Contains(searchResult) || worldRefProject.ProjectName.Contains(searchResult) || worldRefProject.Type.Contains(searchResult) ||
                                worldRefProject.Year.Contains(searchResult) || worldRefProject.Country.Contains(searchResult)
                                || worldRefProject.CustomerIndustryType.Contains(searchResult)
                                    || worldRefProject.Status.Contains(searchResult)
                                           || (rr.SearchKeyword.Contains(searchResult) && rr.isDeleted == false)
                                    ) && worldRefProject.IsAuthorized == 1
                                &&
                                (projImg.link == false) && ((projImg.ImagePath.Contains(".bmp"))
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
                        if (project.worldRefProject.id == like.ProjectId && project.id == like.ImageId)
                        {
                            int TotalLikeCount = (from s in context.ImageLikeAndComments
                                                  where s.ProjectId == project.worldRefProject.id && s.ImageId == project.id && s.ImgLikeFlag == true
                                                  select s).Count();
                            likecount = Convert.ToInt32(TotalLikeCount);
                            likflag = (bool)(like.ImgLikeFlag);
                        }
                    }
                }
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
                        userid = project.worldRefProject.userid,
                        CustomerName = customer,
                        ImageName = project.ImageName,
                        ImagePath = project.ImagePath,
                        CustomerIndustryType = project.worldRefProject.CustomerIndustryType,
                        countflg = likflag.ToString(),
                        TotalLikes = !string.IsNullOrEmpty(likecount.ToString()) ? likecount.ToString() : "0"
                    });
                }
            }


            return listImages;
        }

        public List<WorldRefSearchModel> ReturnFilterImages(string[] countryFilter)
        {
            List<WorldRefSearchModel> listImages = new List<WorldRefSearchModel>();

            foreach (string countryName in countryFilter)
            {
                var projects = (from worldRefProject in context.WorldRefExcelDataProjects
                                join projImg in context.ProjectImages on worldRefProject.id equals projImg.ProjectId
                                where (((worldRefProject.Country == countryName) && worldRefProject.IsAuthorized == 1)
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

                    //int? totalLikes = (from like in context.ProjectImagesVideosLikes
                    //                   where like.ProjectId == project.worldRefProject.id && like.ProjectImageId == project.id
                    //                   select like.TotalLikes).FirstOrDefault();
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
                            CustomerName = customer,
                            ImageName = project.ImageName,
                            ImagePath = project.ImagePath,
                            CustomerIndustryType = project.worldRefProject.CustomerIndustryType,
                            TotalLikes = !string.IsNullOrEmpty(likecount.ToString()) ? likecount.ToString() : "0"
                        });
                    }
                }

            }

            return listImages;
        }

        #endregion


        #region Search Video Filter

        public List<WorldRefSearchModel> ReturnFilterVideos()
        {
            List<WorldRefSearchModel> listImages = new List<WorldRefSearchModel>();

            var projectImages = (from image in context.ProjectImages
                                 join excel in context.WorldRefExcelDataProjects on image.ProjectId equals excel.id
                                 where excel.IsAuthorized == 1 && (((image.link == false) && ((image.ImagePath.Contains(".avi"))
                                     || (image.ImagePath.Contains(".mp4")) || (image.ImagePath.Contains(".wmv")) || (image.ImagePath.Contains(".3gp")))) || (image.link == true))
                                 select new { image, excel }).AsEnumerable();//get list of videos and and project from database bassed on join. 

            foreach (var project in projectImages)
            {
                string organizationName = string.Empty;
                string customer = string.Empty;
                string projectName = string.Empty;
                string type = string.Empty;
                string year = string.Empty;
                string country = string.Empty;
                string emailid = string.Empty;

                if (project.excel.IsOrganization == false)
                {
                    organizationName = project.excel.OrganizationName;
                }
                if (project.excel.IsCustomer == false)
                {
                    customer = project.excel.CustomerName;
                }
                if (project.excel.IsProject == false)
                {
                    projectName = project.excel.ProjectName;
                }
                if (project.excel.IsType == false)
                {
                    type = project.excel.Type;
                }
                if (project.excel.IsYear == false)
                {
                    year = project.excel.Year;
                }
                if (project.excel.IsCountry == false)
                {
                    country = project.excel.Country;
                }
                if (project.excel.IsEmail == false)
                {
                    string emailOfUser = (from rr in context.RegisterUsers where rr.Id == project.excel.userid select rr.Email).FirstOrDefault();//check email id duplicate in database RegisterUsers table  
                    emailid = emailOfUser;
                }


                var abc = (from c in context.VideosLikeAndComments select c); //find all data from VideosLikeAndComments.
                var likecount = 0;  //likecount is a var intilised by 0.
                var likflag = false;//likflag is a var intilised by false.
                if (abc.ToString() == "")//check if var abc is null.
                { }
                else
                {
                    foreach (var like in abc)
                    {
                        if (project.excel.id == like.ProjectId && project.excel.id == like.VideosId)
                        {
                            int TotalLikeCount = (from s in context.VideosLikeAndComments//retrive count of total likes.
                                                  where s.ProjectId == project.excel.id && s.VideosId == project.excel.id && s.VideosLikeFlag == true
                                                  select s).Count();
                            likecount = Convert.ToInt32(TotalLikeCount);//initilised totalcount in likecount.
                            likflag = (bool)(like.VideosLikeFlag);      //initilised VideosLikeFlag to likeflag.
                        }
                    }
                }


                int index = listImages.FindIndex(x => x.ImageId == project.image.id); //find index in list.
                if (index == -1)
                {
                    listImages.Add(new WorldRefSearchModel()//add total details in listimages.
                    {
                        id = project.excel.id,
                        ImageId = project.image.id,
                        OrganizationName = organizationName,
                        ProjectName = projectName,
                        Country = country,
                        Type = type,
                        Year = year,
                        CustomerName = customer,
                        ImageName = project.image.ImageName,
                        ImagePath = project.image.ImagePath,
                        Link = project.image.link,
                        userid = project.excel.userid,
                        CustomerIndustryType = project.excel.CustomerIndustryType,
                        countflg = likflag.ToString(),
                        TotalLikes = !string.IsNullOrEmpty(likecount.ToString()) ? likecount.ToString() : "0"
                    });
                }
            }

            return listImages;//return list of count.
        }

        public List<WorldRefSearchModel> ReturnFilterVideos(string searchResult, string[] countryFilter)
        {
            List<WorldRefSearchModel> listImages = new List<WorldRefSearchModel>();

            if (countryFilter != null && string.IsNullOrEmpty(searchResult))//check condition is true
            {
                return ReturnFilterVideos(countryFilter);//return ReturnFilterVideos
            }
            else if (countryFilter == null && !string.IsNullOrEmpty(searchResult))//check condition is true
            {
                return ReturnFilterVideos(searchResult);//return ReturnFilterVideos
            }

            return listImages;
        }

        public List<WorldRefSearchModel> ReturnFilterVideos(string searchResult)
        {
            List<WorldRefSearchModel> listImages = new List<WorldRefSearchModel>();

            var projects = (from worldRefProject in context.WorldRefExcelDataProjects
                            join projImg in context.ProjectImages on worldRefProject.id equals projImg.ProjectId
                            join Keywords in context.ProjectSearchKeywords on worldRefProject.id equals Keywords.ProjectId
                              into result
                            from rr in result.DefaultIfEmpty()
                            where (((worldRefProject.OrganizationName.Contains(searchResult) ||
                                worldRefProject.CustomerName.Contains(searchResult) || worldRefProject.ProjectName.Contains(searchResult) || worldRefProject.Type.Contains(searchResult) ||
                                worldRefProject.Year.Contains(searchResult) || worldRefProject.Country.Contains(searchResult)
                                || worldRefProject.CustomerIndustryType.Contains(searchResult)
                                    || worldRefProject.Status.Contains(searchResult)
                                      || (rr.SearchKeyword.Contains(searchResult) && rr.isDeleted == false)
                                    ) && worldRefProject.IsAuthorized == 1) &&
                                ((projImg.link == false) && ((projImg.ImagePath.Contains(".avi"))
                                         || (projImg.ImagePath.Contains(".mp4")) || (projImg.ImagePath.Contains(".wmv")) || (projImg.ImagePath.Contains(".3gp"))) || (projImg.link == true)))
                            select new { projImg.ImageName, projImg.ImagePath, projImg.link, projImg.id, projImg.ProjectId, worldRefProject }).AsEnumerable();//get list of videos and and project from database bassed on join.

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
                    string emailOfUser = (from rr in context.RegisterUsers where rr.Id == project.worldRefProject.userid select rr.Email).FirstOrDefault();//check email id duplicate in database RegisterUsers table  
                    emailid = emailOfUser;
                }



                var abc = (from c in context.VideosLikeAndComments select c);//find all data from VideosLikeAndComments.
                var likecount = 0;   //likecount is a var intilised by 0.
                var likflag = false; //likflag is a var intilised by false.
                if (abc.ToString() == "")//check if var abc is null.
                { }
                else
                {
                    foreach (var like in abc)
                    {
                        if (project.worldRefProject.id == like.ProjectId && project.id == like.VideosId)
                        {
                            int TotalLikeCount = (from s in context.VideosLikeAndComments//retrive count of total likes.
                                                  where s.ProjectId == project.ProjectId && s.VideosId == project.id && s.VideosLikeFlag == true
                                                  select s).Count();
                            likecount = Convert.ToInt32(TotalLikeCount);//initilised totalcount in likecount.
                            likflag = (bool)(like.VideosLikeFlag);      //initilised VideosLikeFlag to likeflag.
                        }
                    }
                }


                int index = listImages.FindIndex(x => x.ImageId == project.id);
                if (index == -1)
                {
                    listImages.Add(new WorldRefSearchModel()//add total details in listimages.
                    {
                        id = project.worldRefProject.id,
                        ImageId = project.id,
                        OrganizationName = organizationName,
                        ProjectName = projectName,
                        Country = country,
                        Type = type,
                        Year = year,
                        userid = project.worldRefProject.userid,
                        CustomerName = customer,
                        ImageName = project.ImageName,
                        ImagePath = project.ImagePath,
                        Link = project.link,
                        CustomerIndustryType = project.worldRefProject.CustomerIndustryType,
                        countflg = likflag.ToString(),
                        TotalLikes = !string.IsNullOrEmpty(likecount.ToString()) ? likecount.ToString() : "0"
                    });
                }
            }

            return listImages;//return list of count.
        }

        public List<WorldRefSearchModel> ReturnFilterVideos(string[] countryFilter)
        {
            List<WorldRefSearchModel> listImages = new List<WorldRefSearchModel>();

            foreach (string countryName in countryFilter)
            {
                var projects = (from worldRefProject in context.WorldRefExcelDataProjects
                                join projImg in context.ProjectImages on worldRefProject.id equals projImg.ProjectId
                                where (((worldRefProject.Country == countryName) && worldRefProject.IsAuthorized == 1) &&
                                (((projImg.link == false) && ((projImg.ImagePath.Contains(".avi"))
                                             || (projImg.ImagePath.Contains(".mp4")) || (projImg.ImagePath.Contains(".wmv")) || (projImg.ImagePath.Contains(".3gp")))) || (projImg.link == true)))

                                select new { projImg.ImageName, projImg.ImagePath, projImg.link, projImg.id, worldRefProject }).AsEnumerable();//get list of videos and and project from database bassed on join.

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
                        string emailOfUser = (from rr in context.RegisterUsers where rr.Id == project.worldRefProject.userid select rr.Email).FirstOrDefault();//check duplicate emailid in registeruser table.
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
                            if (project.worldRefProject.id == like.ProjectId && project.id == like.VideosId)
                            {
                                int TotalLikeCount = (from s in context.VideosLikeAndComments//retrive count of total likes.
                                                      where s.ProjectId == project.id && s.VideosId == project.id && s.VideosLikeFlag == true
                                                      select s).Count();
                                likecount = Convert.ToInt32(TotalLikeCount); //initilised totalcount in likecount.
                                likflag = (bool)(like.VideosLikeFlag);       //initilised VideosLikeFlag to likeflag.
                            }
                        }
                    }

                    int index = listImages.FindIndex(x => x.ImageId == project.id);
                    if (index == -1)
                    {
                        listImages.Add(new WorldRefSearchModel()//add total details in listimages.
                        {
                            id = project.worldRefProject.id,
                            ImageId = project.id,
                            OrganizationName = organizationName,
                            ProjectName = projectName,
                            Country = country,
                            Type = type,
                            Year = year,
                            userid = project.worldRefProject.userid,
                            CustomerName = customer,
                            ImageName = project.ImageName,
                            ImagePath = project.ImagePath,
                            Link = project.link,
                            CustomerIndustryType = project.worldRefProject.CustomerIndustryType,
                            countflg = likflag.ToString(),
                            TotalLikes = !string.IsNullOrEmpty(likecount.ToString()) ? likecount.ToString() : "0"
                        });
                    }
                }

            }

            return listImages;//return list of count.
        }

        #endregion



        public List<WorldRefExcelDataModel> ReturnInitialResultById(int id)
        {
            List<WorldRefExcelDataModel> listExcelProject = new List<WorldRefExcelDataModel>();

            var projects = (from worldRefProject in context.WorldRefExcelDataProjects
                            where worldRefProject.id == id
                                && worldRefProject.IsAuthorized == 1
                            select worldRefProject).AsEnumerable();

            foreach (var project in projects)
            {
                string organizationName = string.Empty;
                string customer = string.Empty;
                string projectName = string.Empty;
                string type = string.Empty;
                string year = string.Empty;
                string country = string.Empty;
                string emailid = string.Empty;

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
                if (project.IsEmail == false)
                {
                    string emailOfUser = (from rr in context.RegisterUsers where rr.Id == project.userid select rr.Email).FirstOrDefault();
                    emailid = emailOfUser;
                }
                listExcelProject.Add(new WorldRefExcelDataModel()
                {
                    id = project.id,
                    OrganizationName = organizationName,
                    CustomerName = customer,
                    ProjectName = projectName,
                    Type = type,
                    Year = year,
                    Country = country,
                    EmailId = emailid

                });
            }

            return listExcelProject;
        }

        public ProjectSearchModel ReturnParticularSearchRef(int? projectId)
        {
            WorldRefExcelDataProject project = (from excel in context.WorldRefExcelDataProjects
                                                where excel.id == projectId
                                                select excel).FirstOrDefault();
            ProjectSearchModel SearchModel = new ProjectSearchModel();
            List<ImageStructure> ListOfImages = new List<ImageStructure>();
            List<ImageStructure> ListOfVideos = new List<ImageStructure>();

            if (project != null)
            {
                string organizationName = string.Empty;
                string customer = string.Empty;
                string projectName = string.Empty;
                string type = string.Empty;
                string year = string.Empty;
                string country = string.Empty;
                string emailid = string.Empty;

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
                if (project.IsEmail == false)
                {
                    string emailOfUser = (from rr in context.RegisterUsers where rr.Id == project.userid select rr.Email).FirstOrDefault();
                    emailid = emailOfUser;
                }

                SearchModel.id = project.id;
                SearchModel.OrganizationName = organizationName;
                SearchModel.CustomerName = customer;
                SearchModel.ProjectName = projectName;
                SearchModel.Type = type;
                SearchModel.Year = year;
                SearchModel.Country = country;
                SearchModel.EmailId = emailid;

                var projectImages = (from img in context.ProjectImages
                                     where img.ProjectId == project.id && ((img.link == false) && ((img.ImagePath.Contains(".bmp"))
                                      || (img.ImagePath.Contains(".jpg")) || (img.ImagePath.Contains(".png"))))
                                     select img).AsEnumerable();

                foreach (var proImg in projectImages)
                {
                    ListOfImages.Add(new ImageStructure()
                    {
                        ImageName = proImg.ImageName,
                        ImagePath = proImg.ImagePath,
                        Link = proImg.link
                    });
                }

                var projectVideos = (from img in context.ProjectImages
                                     where img.ProjectId == project.id && (((img.link == false) && ((img.ImagePath.Contains(".avi"))
                                     || (img.ImagePath.Contains(".mp4")) || (img.ImagePath.Contains(".wmv")) || (img.ImagePath.Contains(".3gp")))) || (img.link == true))
                                     select img).AsEnumerable();

                foreach (var proImg in projectVideos)
                {
                    ListOfVideos.Add(new ImageStructure()
                    {
                        ImageName = proImg.ImageName,
                        ImagePath = proImg.ImagePath,
                        Link = proImg.link
                    });
                }
                SearchModel.ListOfImages = ListOfImages;
                SearchModel.ListOfVideos = ListOfVideos;
            }
            return SearchModel;
        }
    }
}