using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorldRef.BusinessLayer
{
    /// <summary>
    /// It will have all the functionality of like and dislike
    /// </summary>
    public class LikeRating
    {
        private I4IDBEntities context = new I4IDBEntities();

        /// <summary>
        /// Add Like or dislike to Db
        /// </summary>
        /// <returns></returns>
        [Obsolete("Not in use")]
        public string AddLikeOrDislike(int ProjectId, string LikeOrDisLike, string Comment, string Null)
        {
            string ReturnString = "Success";
            try
            {
                ProjectLikeDisLike projectLike = (from likeTb in context.ProjectLikeDisLikes
                                                  where likeTb.projectId == ProjectId
                                                  select likeTb).FirstOrDefault();
                if (projectLike != null)
                {
                    int? TotalLike = projectLike.totalLike;
                    int? TotalDisLike = Convert.ToInt32(projectLike.totalDislike);

                    if (LikeOrDisLike == "Like")
                        projectLike.totalLike = TotalLike + 1;
                    else
                        projectLike.totalDislike = (TotalDisLike + 1).ToString();

                }
                else
                {
                    ProjectLikeDisLike projectLikeDislike = new ProjectLikeDisLike();
                    projectLikeDislike.projectId = ProjectId;
                    if (LikeOrDisLike == "Like")
                    {
                        projectLikeDislike.totalLike = 1;
                        projectLikeDislike.totalDislike = "0";
                    }
                    else
                    {
                        projectLikeDislike.totalLike = 0;
                        projectLikeDislike.totalDislike = "1";
                    }

                    context.ProjectLikeDisLikes.Add(projectLikeDislike);
                }

                ProjectLikeComment projectComent = new ProjectLikeComment();
                projectComent.projectId = ProjectId;
                projectComent.comment = Comment;
                projectComent.date = DateTime.Now;

                context.ProjectLikeComments.Add(projectComent);
                context.SaveChanges();
            }
            catch
            {
                ReturnString = "Fail";
            }
            return ReturnString;
        }

        /// <summary>
        /// Add Like or dislike to Db
        /// </summary>
        /// <returns></returns>
        public string AddLikeOrDislike(int projectId, string LikeOrDisLike, string UserId)
        {
            string ReturnString = "Success";
            try
            {
                if (string.IsNullOrEmpty(CheckUserAlreadyLiked(projectId, Convert.ToInt32(UserId))))
                {
                    ProjectLikeDisLike projectLike = (from likeTb in context.ProjectLikeDisLikes
                                                      where likeTb.projectId == projectId
                                                      select likeTb).FirstOrDefault();
                    if (projectLike != null)
                    {
                        int? TotalLike = projectLike.totalLike;
                        projectLike.totalLike = TotalLike + 1;
                        projectLike.flag = true;
                    }
                    else
                    {
                        ProjectLikeDisLike projectLikeDislike = new ProjectLikeDisLike();
                        projectLikeDislike.projectId = projectId;
                        projectLikeDislike.totalLike = 1;
                        projectLikeDislike.flag = true;

                        context.ProjectLikeDisLikes.Add(projectLikeDislike);
                    }

                    context.ProjectLikeHistories.Add(new ProjectLikeHistory()
                    {
                        ProjectId = projectId,
                        userId = Convert.ToInt32(UserId),
                        likeOn = DateTime.Now
                    });
                    context.SaveChanges();
                    SendEmailForLikeAndComment(projectId, Convert.ToInt32(UserId), string.Empty);
                }
                else
                {
                    ReturnString = "You Already Liked this Project";
                }
            }
            catch
            {
                ReturnString = "Fail";
            }
            return ReturnString;
        }
        /// <summary>
        /// Add credit
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="LikeOrDisLike"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        #region code for credit
        public string AddCreditRequest(int projectId, string LikeOrDisLike, string UserId)
        {
            string ReturnString = "Success";
            try
            {
                if (string.IsNullOrEmpty(CheckUserAlreadyCredits(projectId, Convert.ToInt32(UserId))))
                {


                    int userID = Convert.ToInt32(UserId);

                    var details = (from r in context.RegisterUsers
                                   where r.Id == userID
                                   select r).FirstOrDefault();
                    LinkedInUser l = new LinkedInUser();
                    l.id = projectId;
                    l.userId = userID;
                    l.FirstName = details.UserFirstName;
                    l.LastName = details.UserLastName;
                    l.Email = details.Email;
                    l.CreditM = false;
                    l.IsApproved = false;
                    context.LinkedInUsers.Add(l);
                    context.SaveChanges();
                    ReturnString = "submitted";
                    //SendEmailForLikeAndComment(projectId, Convert.ToInt32(UserId), string.Empty);

                }
                else
                {
                    ReturnString = "AlreadyRequested";
                }
            }
            catch
            {
                ReturnString = "Fail";
            }
            return ReturnString;
        }
        #endregion
        public string AddImageVideoLike(int projectId, int projectImageId, int UserId)
        {
            try
            {
                if (string.IsNullOrEmpty(CheckUserAlreadyLiked(projectId, UserId, projectImageId)))
                {
                    ProjectImagesVideosLike projectImage = (from like in context.ProjectImagesVideosLikes
                                                            where like.ProjectId == projectId && like.ProjectImageId == projectImageId
                                                            select like).FirstOrDefault();
                    if (projectImage != null)
                    {
                        projectImage.TotalLikes = projectImage.TotalLikes + 1;
                    }
                    else
                    {
                        context.ProjectImagesVideosLikes.Add(new ProjectImagesVideosLike()
                        {
                            ProjectId = projectId,
                            ProjectImageId = projectImageId,
                            TotalLikes = 1,
                            modifiedOn = DateTime.Now
                        });
                    }

                    context.ProjectImageLikeHistories.Add(new ProjectImageLikeHistory()
                    {
                        ProjectId = projectId,
                        ProjectImageId = projectImageId,
                        userId = UserId,
                        likedOn = DateTime.Now
                    });

                    context.SaveChanges();
                    SendEmailForLikeAndComment(projectId, UserId, string.Empty);
                }
                else
                {
                    return "You Already Liked this Project";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return string.Empty;
        }

        public string CheckUserAlreadyLiked(int ProjectId, int userid, int ProjectImageId = 0)
        {
            if (ProjectImageId == 0)
            {
                ProjectLikeHistory projectLike = (from like in context.ProjectLikeHistories
                                                  where like.ProjectId == ProjectId && like.userId == userid
                                                  select like).FirstOrDefault();
                if (projectLike != null)
                {
                    return projectLike.userId.ToString();
                }

            }
            else
            {
                ProjectImageLikeHistory projectImage = (from like in context.ProjectImageLikeHistories
                                                        where like.ProjectId == ProjectId && like.ProjectImageId == ProjectImageId && like.userId == userid
                                                        select like).FirstOrDefault();

                if (projectImage != null)
                {
                    return projectImage.userId.ToString();
                }

            }
            return string.Empty;
        }

        public string CheckUserAlreadyCredits(int ProjectId, int userid)
        {


            LinkedInUser linkRequest = (from l in context.LinkedInUsers
                                        where l.id == ProjectId && l.userId == userid
                                        select l).FirstOrDefault();
            if (linkRequest != null)
            {
                return linkRequest.userId.ToString();
            }
            return string.Empty;
        }

        public string AddCommentProject(int ProjectId, int userid, string review)
        {
            try
            {
                context.ProjectReviews.Add(new ProjectReview()
                {
                    ProjectId = ProjectId,
                    userId = userid,
                    Review = review,
                    CreatedOn = DateTime.Now,
                    flag = true,
                    Show = false,
                    IsAdminApproved = false,
                    Publish = false,
                    UnPublish = false

                });

                context.SaveChanges();
                SendEmailForLikeAndComment(ProjectId, userid, review);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "Your Review has been sent and is subjected to approval.";
        }

        public string AddCommentProject(int ProjectId, int userid, string review, int ProjectImageId)
        {
            try
            {
                context.ProjectImageVideoComments.Add(new ProjectImageVideoComment()
                {
                    ProjectId = ProjectId,
                    ProjectImageId = ProjectImageId,
                    Review = review,
                    UserId = userid,
                    createdOn = DateTime.Now,
                    Show = false
                });

                context.SaveChanges();
                SendEmailForLikeAndComment(ProjectId, userid, review);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "Your Review has been sent and is subjected to approval.";
        }

        private string SendEmailForLikeAndComment(int projectId, int userId, string review)
        {
            Email email = new Email();
            try
            {
                dynamic emailAddress = (from user in context.RegisterUsers
                                        join worldRef in context.WorldRefExcelDataProjects on user.Id equals worldRef.userid
                                        where worldRef.id == projectId
                                        select new { user.Email, worldRef.ProjectName }).FirstOrDefault();

                string UserName = (from user in context.RegisterUsers
                                   where user.Id == userId
                                   select user.UserFirstName).FirstOrDefault();

                string Subject = "WorldRef Project Liked";
                string Body = "<div style='font-size:15px; font-family:Calibri Light;'>" + emailAddress.ProjectName + " is Liked by " + UserName + "</div>";

                if (!string.IsNullOrEmpty(review))
                {
                    Body = "<div style='font-size:15px; font-family:Calibri Light;'>" + emailAddress.ProjectName + " is Reviewed by " + UserName + ".<br/>Review :" + review + "</div>";
                }
                email.SendMail("", emailAddress.Email, Subject, Body);
            }
            catch (Exception ex)
            {

            }
            return string.Empty;
        }

        protected string GetIPAddress()
        {
            System.Web.HttpContext httpContext = System.Web.HttpContext.Current;
            string ipAddress = httpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return httpContext.Request.ServerVariables["REMOTE_ADDR"];
        }

        /// <summary>
        /// Get the total like of a project
        /// </summary>
        /// <returns></returns>
        public string GetTotalLike(int projectId)
        {
            int? likes = 0;
            try
            {
                likes = (from likeTb in context.ProjectLikeDisLikes
                         where likeTb.projectId == projectId
                         select likeTb.totalLike).FirstOrDefault();
            }
            catch
            {
                likes = 0;
            }
            return ((likes != null) ? likes.ToString() : "0");
        }

        /// <summary>
        /// Get the total dislike of a project
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public string GetTotalDislike(int projectId)
        {
            string DisLike = "0";
            try
            {
                DisLike = (from likeTb in context.ProjectLikeDisLikes
                           where likeTb.projectId == projectId
                           select likeTb.totalDislike).FirstOrDefault();
            }
            catch
            {
                DisLike = "0";
            }
            return (!string.IsNullOrEmpty(DisLike) ? DisLike : "0");
        }

        /// <summary>
        /// Add or Update Rating to a project
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="Rating"></param>
        /// <returns></returns>
        public string AddRating(int ProjectId, string Rating, string ratingtype)
        {
            string returnStatus = "Success";
            try
            {
                ProjectRating rating = (from rate in context.ProjectRatings where rate.projectId == ProjectId select rate).FirstOrDefault();
                if (rating != null)
                {
                    if (ratingtype == "P")
                    {
                        rating.rating = Rating;//1 for high, 2 for Optimum and 3 for Expensive.
                    }
                    if (ratingtype == "Q")
                    {
                        rating.QualityRating = Rating;
                    }
                    if (ratingtype == "R")
                    {
                        rating.UserRating = Rating;
                    }


                }

                else
                {
                    ProjectRating projRating = new ProjectRating();
                    if (ratingtype == "i4irating")
                    {
                        projRating.projectId = ProjectId;
                        projRating.rating = Rating;
                        context.ProjectRatings.Add(projRating);
                    }
                    if (ratingtype == "Qualityrating")
                    {
                        projRating.projectId = ProjectId;
                        projRating.QualityRating = Rating;
                        context.ProjectRatings.Add(projRating);
                    }
                    if (ratingtype == "userrating")
                    {
                        projRating.projectId = ProjectId;
                        projRating.UserRating = Rating;
                        context.ProjectRatings.Add(projRating);
                    }
                }
                ProjectRatingHistory projHis = new ProjectRatingHistory();
                projHis.ProjectId = ProjectId;
                projHis.rating = Rating;
                projHis.date = DateTime.Now;
                context.ProjectRatingHistories.Add(projHis);
                context.SaveChanges();
            }
            catch
            {
                returnStatus = "Fail";
            }
            return returnStatus;
        }

        /// <summary>
        /// Get the rating of a project
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public string GetRating(int ProjectId)
        {
            string rating = "0";
            try
            {
                rating = (from rate in context.ProjectRatings where rate.projectId == ProjectId select rate.rating).FirstOrDefault();
            }
            catch
            {
            }
            return (!string.IsNullOrEmpty(rating) ? rating : "0");
        }

        /// <summary>
        /// List of all comments
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllComment()
        {
            List<string> listAllComments = new List<string>();

            return listAllComments;
        }

        /// <summary>
        /// Get all the comments of a project
        /// </summary>
        /// <returns></returns>
        public List<ReviewList> GetCommentsOfProject(int ProjectId)
        {
            List<ReviewList> listComments = new List<ReviewList>();

            var adminReview = (from review in context.ProjectReviews
                               where review.ProjectId == ProjectId
                               select new { review.Review }).AsEnumerable();

            var comments = (from review in context.ProjectReviews
                            where review.ProjectId == ProjectId && review.Show == true && review.IsAdminApproved == true && review.Publish == true
                            select review).ToList();

            foreach (var rvr in comments)
            {
                string userName = (from user in context.RegisterUsers
                                   where user.Id == rvr.userId
                                   select user.UserFirstName).FirstOrDefault();

                listComments.Add(new ReviewList()
                {
                    ContactPerson = userName,
                    Review = rvr.Review
                });
            }

            return listComments;
        }

        public List<ReviewList> GetCommentsOfProject(int ProjectId, int ProjectImageid)
        {
            List<ReviewList> listComments = new List<ReviewList>();

            var comments = (from review in context.ProjectImageVideoComments
                            where review.ProjectId == ProjectId && review.ProjectImageId == ProjectImageid && review.Show == true
                            select review).ToList();

            foreach (var rvr in comments)
            {
                string userName = (from user in context.RegisterUsers
                                   where user.Id == rvr.UserId
                                   select user.UserFirstName).FirstOrDefault();

                listComments.Add(new ReviewList()
                {
                    ContactPerson = userName,
                    Review = rvr.Review
                });
            }

            return listComments;
        }

        #region code for like image in searchImages
        /// <summary>
        /// Add LikeImages in Db
        /// </summary>
        /// <returns>string</returns>
        public string AddLikeImages(int projectId, string ProjectImageId, string LikeOrDisLike, string UserID, string userId)  //this function is used to save like Image data in database.their are 5 parameters to pass which are related to project.
        {
            int usrId = Convert.ToInt32(userId);
            int uId = Convert.ToInt32(UserID);
            int imgId = Convert.ToInt32(ProjectImageId);
            string ReturnString = "Success";
            try
            {
                if (context.ImageLikeAndComments.Any(o => o.ProjectId == projectId && o.ImageId == imgId && o.UserId == uId && o.ImgLikeFlag == true))//check duplicate ids in ImageLikeAndComments table.
                {
                    ReturnString = "You Already Liked this Project";

                }

                else
                {
                    ImageLikeAndComment imgLike = (from likeTb in context.ImageLikeAndComments //fetch image which are liked by user.
                                                   where likeTb.ProjectId == projectId && likeTb.PImageLikeId == imgId && likeTb.UserId == uId
                                                   select likeTb).FirstOrDefault();



                    ImageLikeAndComment objimag = new ImageLikeAndComment();
                    objimag.ImageId = imgId;
                    objimag.UserId = uId;
                    objimag.ProjectId = projectId;
                    objimag.ImgLikeFlag = true;
                    objimag.ImageTestimonials = "";
                    objimag.CmtImgFlag = false;

                    context.ImageLikeAndComments.Add(objimag);// add data in ImageLikeAndComments table.
                    context.SaveChanges(); //save data in ImageLikeAndComments table.
                }
            }
            catch
            {
                ReturnString = "Fail";
            }
            return ReturnString;
        }


        //public string CheckUserAlreadyLikedImage(int ProjectId, int userid,string UserID)//Action for check user already like project.
        //{
        //    if (ProjectImageId == 0)
        //    {
        //        ImageLikeAndComment projectLike = (from like in context.ImageLikeAndComments
        //                                           where like.ProjectId == ProjectId && like.UserId == userid && like.PImageLikeId == ProjectImageId
        //                                          select like).FirstOrDefault();
        //        if (projectLike != null)
        //        {
        //            return projectLike.UserId.ToString();
        //        }

        //    }
        //    else
        //    {
        //        ImageLikeAndComment projectLike = (from like in context.ImageLikeAndComments
        //                                           where like.ProjectId == ProjectId && like.UserId == userid && like.PImageLikeId == ProjectImageId
        //                                           select like).FirstOrDefault();
        //        if (projectLike != null)
        //        {
        //            return projectLike.UserId.ToString();
        //        }

        //    }
        //    return string.Empty;
        //}
        #endregion
        #region code for Like Videos
        public string AddLikeVideos(int projectId, string ProjectImageId, string LikeOrDisLike, string UserID, string userId)  //this function is used to save like Videos data in database.their are 5 parameters to pass which are related to project.
        {
            int usrId = Convert.ToInt32(userId);
            int uId = Convert.ToInt32(UserID);
            int imgId = Convert.ToInt32(ProjectImageId);
            string ReturnString = "Success";
            try
            {
                if (context.VideosLikeAndComments.Any(o => o.ProjectId == projectId && o.VideosId == imgId && o.UserId == uId && o.VideosLikeFlag == true))//check duplicate ids in VideosLikeAndComments table.
                {
                    ReturnString = "You Already Liked this Project Videos.";

                }

                else
                {
                    VideosLikeAndComment vidLike = (from likeTb in context.VideosLikeAndComments //fetch videos which are liked by user.
                                                    where likeTb.ProjectId == projectId && likeTb.PVideosId == imgId && likeTb.UserId == uId
                                                    select likeTb).FirstOrDefault();



                    VideosLikeAndComment objvideos = new VideosLikeAndComment();
                    objvideos.VideosId = imgId;
                    objvideos.UserId = uId;
                    objvideos.ProjectId = projectId;
                    objvideos.VideosLikeFlag = true;
                    objvideos.VideosTestimonials = "";
                    objvideos.cmtVidFlag = false;

                    context.VideosLikeAndComments.Add(objvideos);// add data in VideosLikeAndComments table.
                    context.SaveChanges(); //save data in VideosLikeAndComments table.
                }
            }
            catch
            {
                ReturnString = "Fail";
            }
            return ReturnString;//return a string.
        }

        /// <summary>
        /// List of all comments on project videos.
        /// </summary>
        /// <returns></returns>
        public List<ReviewList> GetCommentsOfProjectVideos(int ProjectId, int ProjectImageid)
        {
            List<ReviewList> listComments = new List<ReviewList>();

            var comments = (from review in context.ProjectImageVideoComments  //retrive all testimonials from database.
                            where review.ProjectId == ProjectId && review.ProjectImageId == ProjectImageid && review.Show == true
                            select review).ToList();

            foreach (var rvr in comments)
            {
                string userName = (from user in context.RegisterUsers //find username from registeruser tables.
                                   where user.Id == rvr.UserId
                                   select user.UserFirstName).FirstOrDefault();

                listComments.Add(new ReviewList()//add username and testimonials in list.
                {
                    ContactPerson = userName,
                    Review = rvr.Review
                });
            }

            return listComments; //return list of testimonials.
        }




        #endregion

    }

    public class ReviewList
    {
        public string ContactPerson { get; set; }
        public string Review { get; set; }
    }
}