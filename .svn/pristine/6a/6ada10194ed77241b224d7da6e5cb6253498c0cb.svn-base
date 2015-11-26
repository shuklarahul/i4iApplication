using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WorldRef.BusinessLayer;
using System.Web.Mvc;
using WorldRef.BusinessLayer.IndustryCategory;
using WorldRef.BusinessLayer.IndustrySubCategory;
using WorldRef.Models;
namespace WorldRef.BusinessLayer

{
    public class KnowledgeLogic
    {
        private I4IDBEntities context = new I4IDBEntities();
        private IIndustryCategoryAccessor _industryCategory = new IndustryCategoryAccessor();
        private IIndustrySubCatAccessor _industrySubCat = new IndustrySubCategoryAccessor();
        private IndustryCategoryModel _categoryModel = new IndustryCategoryModel();
        private IndustrySubCatModel _subCatModel = new IndustrySubCatModel();


        /// <summary>
        /// Returns All Industry in list
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetIndustry(bool ThroughJason = false)
        {
            List<SelectListItem> SelectIndustry = new List<SelectListItem>();

            var content = (from p in context.Industries
                           select new { p.IndustriesID, p.IndustriesName }).AsEnumerable();

            if (ThroughJason == false)
            {
                // SelectIndustry.Add(new SelectListItem() { Text = "Select Industry", Value = "0" });
            }

            foreach (var item in content)
            {
                SelectIndustry.Add(new SelectListItem() { Text = item.IndustriesName, Value = item.IndustriesID.ToString() });

            }
            if (ThroughJason == false)
            {
                // SelectIndustry.Add(new SelectListItem() { Text = "Other", Value = "Other" });
            }
            return SelectIndustry;

        }

        /// <summary>
        /// Returns All Industry Category In List
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetIndustryCategory(int IndustryId, bool ThroughJason = false)
        {
            List<SelectListItem> SelectIndustry = new List<SelectListItem>();
            List<IndustryCategoryModel> IndustryCat = _industryCategory.GetAllCategoryIndustry(IndustryId);

            if (ThroughJason == false)
            {
                SelectIndustry.Add(new SelectListItem() { Text = "Select Category", Value = "0" });
            }

            foreach (IndustryCategoryModel item in IndustryCat)
            {
                SelectIndustry.Add(new SelectListItem() { Text = item.CategoryName, Value = item.Id.ToString() });

            }
            if (ThroughJason == false)
            {
                SelectIndustry.Add(new SelectListItem() { Text = "Other", Value = "Other" });
            }
            return SelectIndustry;

        }

        /// <summary>
        /// Returns All Industry Category In List
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetIndustrySubCategory(int IndustryId, int IndustryCatId, bool ThroughJason = false)
        {
            List<SelectListItem> SelectIndustry = new List<SelectListItem>();
            List<IndustrySubCatModel> IndustryCat = _industrySubCat.GetAllCategorySubIndustry(IndustryId, IndustryCatId);

            if (ThroughJason == false)
            {
                SelectIndustry.Add(new SelectListItem() { Text = "Select Sub Category", Value = "0" });
            }

            foreach (IndustrySubCatModel item in IndustryCat)
            {
                SelectIndustry.Add(new SelectListItem() { Text = item.SubCategoryName, Value = item.Id.ToString() });
            }
            if (ThroughJason == false)
            {
                SelectIndustry.Add(new SelectListItem() { Text = "Other", Value = "Other" });
            }
            return SelectIndustry;

        }

        public int AddIndustryCategory(int industryId, string CategoryName)
        {
            _categoryModel.IndustryId = industryId;
            _categoryModel.CategoryName = CategoryName;
            return _industryCategory.AddIndustryCategory(_categoryModel);
        }

        public int AddIndustrySubCategory(int industry, int industryCatId, string SubCategoryName)
        {
            _subCatModel.IndustryId = industry;
            _subCatModel.CategoryId = industryCatId;
            _subCatModel.SubCategoryName = SubCategoryName;

            return _industrySubCat.SubAddIndustryCategory(_subCatModel);
        }

        public int AddIndustry(string IndustryName)
        {
            int industryId = (from indu in context.Industries
                              where indu.IndustriesName.ToLower() == IndustryName.ToLower()
                              select indu.IndustriesID
                               ).FirstOrDefault();
            if (industryId == 0)
            {
                context.Industries.Add(new WorldRef.Industry() { IndustriesName = IndustryName });
                context.SaveChanges();
            }

            return industryId;
        }

        /// <summary>
        /// Add knowledge Query to database
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        public string AddKnowledgeQuery(KnowledgeQueryModel queryModel)
        {
            string returnString = string.Empty;

            try
            {
                int ind = int.Parse(queryModel.Industry);
                int cat = int.Parse(queryModel.IndustryCategoryId);
                int subCat = int.Parse(queryModel.IndustrySubCatId);

                var question = (from q in context.KnowledgeQueries
                                where q.Query.ToLower() == queryModel.Query.ToLower() && q.IndustryId == ind
                                && q.IndustryCatId == cat && q.IndustrySubCatId == subCat
                                select q).FirstOrDefault();

                if (question != null)
                {
                    return returnString = "Question Already Asked.";
                }

                context.KnowledgeQueries.Add(new KnowledgeQuery()
                {
                    IndustryId = ind,
                    IndustryCatId = Convert.ToInt32(queryModel.IndustryCategoryId),
                    IndustrySubCatId = Convert.ToInt32(queryModel.IndustrySubCatId),
                    Query = queryModel.Query,
                    CreatedOn = DateTime.Now
                });
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                returnString = "Fail";
            }

            return returnString;
        }

        /// <summary>
        /// Add knowledge Query to database
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        public string AddQueryAnswer(KnowledgeQueryModel queryModel)
        {
            string returnString = string.Empty;

            try
            {
                context.KnowledgeQueryAnswers.Add(new KnowledgeQueryAnswer()
                {
                    QueryID = queryModel.QueryId,
                    UserId = queryModel.UserId,
                    QueryAnswer = queryModel.QueryAnswer,
                    createdOn = DateTime.Now,
                    Relevant = queryModel.Relevant
                });
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                returnString = "Fail";
            }

            return returnString;
        }



        public string AddQueryAnswerByUser(KnowledgeQueryModel queryModel)
        {
            string returnString = string.Empty;

            try
            {
                context.KnowledgeQueryAnswers.Add(new KnowledgeQueryAnswer()
                {
                    QueryID = queryModel.QueryId,
                    UserId = queryModel.UserId,
                    QueryAnswer = queryModel.QueryAnswer,
                    createdOn = DateTime.Now,
                    Relevant = queryModel.Relevant
                });
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                returnString = "Fail";
            }

            return returnString;
        }

        public string AddQueryAnswerByLogedInUser(KnowledgeQueryModel queryModel)
        {
            string returnString = string.Empty;

            try
            {
                context.KnowledgeQueryAnswers.Add(new KnowledgeQueryAnswer()
                {
                    QueryID = queryModel.QueryId,
                    UserId = queryModel.UserId,
                    QueryAnswer = queryModel.QueryAnswer,
                    createdOn = DateTime.Now,
                    Relevant = queryModel.Relevant
                });
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                returnString = "Fail";
            }

            return returnString;
        }



        /// <summary>
        /// Get All Queries
        /// </summary>
        /// <returns></returns>
        public List<KnowledgeQueryModel> GetAllQueries()
        {
            List<KnowledgeQueryModel> queryModel = new List<KnowledgeQueryModel>();
            try
            {
                var queries = (from queryies in context.KnowledgeQueries
                               join industry in context.Industries on queryies.IndustryId equals industry.IndustriesID
                               select new { queryies, industry }).AsEnumerable();

                foreach (var query in queries)
                {
                    queryModel.Add(new KnowledgeQueryModel()
                    {
                        Id = query.queryies.Id,
                        IndustryID = query.queryies.IndustryId,
                        Query = query.queryies.Query,
                        IndustryName = query.industry.IndustriesName
                    });
                }
            }
            catch (Exception ex)
            {
            }
            return queryModel;
        }

        /// <summary>
        /// Get queries Filtered by Industry
        /// </summary>
        /// <param name="industryId"></param>
        /// <returns></returns>
        public List<KnowledgeQueryModel> GetAllQueries(int industryId)
        {
            List<KnowledgeQueryModel> queryModel = new List<KnowledgeQueryModel>();
            try
            {
                var queries = (from queryies in context.KnowledgeQueries
                               join industry in context.Industries on queryies.IndustryId equals industry.IndustriesID
                               where queryies.IndustryId == industryId
                               select new { queryies, industry }).AsEnumerable();

                foreach (var query in queries)
                {
                    queryModel.Add(new KnowledgeQueryModel()
                    {
                        Id = query.queryies.Id,
                        IndustryID = query.queryies.IndustryId,
                        Query = query.queryies.Query,
                        IndustryName = query.industry.IndustriesName
                    });
                }
            }
            catch (Exception ex)
            {
            }
            return queryModel;
        }

        /// <summary>
        /// Get queries Filtered by Industry
        /// </summary>
        /// <param name="industryId"></param>
        /// <returns></returns>
        public List<KnowledgeQueryModel> GetAllQueriesNotAnswered(int industryId, int userId)
        {
            List<KnowledgeQueryModel> queryModel = new List<KnowledgeQueryModel>();
            try
            {
                var queries = (from queryies in context.KnowledgeQueries
                               join industry in context.Industries on queryies.IndustryId equals industry.IndustriesID
                               join qans in context.KnowledgeQueryAnswers on queryies.Id equals qans.QueryID
                               where queryies.IndustryId == industryId && qans.Relevant != false
                               select new { queryies, industry }).AsEnumerable();

                foreach (var query in queries)
                {
                    queryModel.Add(new KnowledgeQueryModel()
                    {
                        Id = query.queryies.Id,
                        IndustryID = query.queryies.IndustryId,
                        Query = query.queryies.Query,
                        IndustryName = query.industry.IndustriesName
                    });
                }
            }
            catch (Exception ex)
            {
            }
            return queryModel;
        }


        /// <summary>
        /// ForParticular Industries
        /// </summary>
        /// <returns></returns>
        public List<KnowledgeQueryModel> GetAllQueriesWithAnswer(string[] industryFilter)
        {
            List<KnowledgeQueryModel> queryModel = new List<KnowledgeQueryModel>();

            foreach (string industryName in industryFilter)
            {

                var queries = (from q in context.KnowledgeQueries
                               join industry in context.Industries on q.IndustryId equals industry.IndustriesID
                               where industry.IndustriesName.ToLower() == industryName.ToLower()
                               select new { q, industry }).AsEnumerable();

                foreach (var query in queries)
                {
                    List<KnowledgeAnswerModel> Ans = new List<KnowledgeAnswerModel>();


                    var AnswerList = (from ans in context.KnowledgeQueryAnswers
                                      join train in context.Trainer_Profile on ans.UserId equals train.Id
                                      where ans.Relevant == true && ans.QueryID == query.q.Id
                                      select new { ans, train }).AsEnumerable();
                    if (AnswerList != null)
                    {
                        foreach (var answer in AnswerList)
                        {
                            Ans.Add(new KnowledgeAnswerModel()
                            {
                                KnowledgeAnswer = answer.ans.QueryAnswer,
                                TrainerName = answer.train.UserName,
                                ImagePath = answer.train.TrainerImage
                            });
                        }
                    }

                    queryModel.Add(new KnowledgeQueryModel()
                    {
                        Id = query.q.Id,
                        IndustryID = query.q.IndustryId,
                        Query = query.q.Query,
                        IndustryName = query.industry.IndustriesName,
                        Answers = Ans
                    });



                }
            }
            return queryModel;
        }

        /// <summary>
        /// Get All Queries With Answer
        /// </summary>
        /// <param name="industryId"></param>
        /// <returns></returns>
        public List<KnowledgeQueryModel> GetAllQueriesWithAnswer()
        {
            List<KnowledgeQueryModel> queryModel = new List<KnowledgeQueryModel>();

            var queries = (from q in context.KnowledgeQueries
                           join industry in context.Industries on q.IndustryId equals industry.IndustriesID
                           select new { q, industry }).AsEnumerable();

            foreach (var query in queries)
            {
                List<KnowledgeAnswerModel> Ans = new List<KnowledgeAnswerModel>();


                var AnswerList = (from ans in context.KnowledgeQueryAnswers
                                  join train in context.Trainer_Profile on ans.UserId equals train.Id
                                  where ans.Relevant == true && ans.QueryID == query.q.Id
                                  select new { ans, train }).AsEnumerable();
                if (AnswerList != null)
                {
                    foreach (var answer in AnswerList)
                    {
                        Ans.Add(new KnowledgeAnswerModel()
                        {
                            KnowledgeAnswer = answer.ans.QueryAnswer,
                            TrainerName = answer.train.UserName,
                            ImagePath = answer.train.TrainerImage
                        });
                    }
                }

                queryModel.Add(new KnowledgeQueryModel()
                {
                    Id = query.q.Id,
                    IndustryID = query.q.IndustryId,
                    Query = query.q.Query,
                    IndustryName = query.industry.IndustriesName,
                    Answers = Ans
                });



            }
            return queryModel;
        }


        /// <summary>
        /// Get queries Filtered by Industry
        /// </summary>
        /// <param name="industryId"></param>
        /// <returns></returns>
        public List<KnowledgeQueryModel> GetAllQueriesByUser(int userId)
        {
            List<KnowledgeQueryModel> queryModel = new List<KnowledgeQueryModel>();

            try
            {
                var queries = (from queryies in context.KnowledgeQueries
                               join industry in context.Industries on queryies.IndustryId equals industry.IndustriesID
                               join user in context.TopicIndustries on queryies.IndustryId equals user.IndustriesID
                               where user.Id == userId
                               select new { queryies, industry }).AsEnumerable();

                foreach (var query in queries)
                {
                    var queryAnswer = (from ans in context.KnowledgeQueryAnswers
                                       where ans.QueryID == query.queryies.Id && ans.UserId == userId
                                       select ans.ID).FirstOrDefault();

                    if (queryAnswer == 0)
                    {

                        if (!queryModel.Any(x => x.Id == query.queryies.Id))
                        {


                            queryModel.Add(new KnowledgeQueryModel()
                            {
                                Id = query.queryies.Id,
                                IndustryID = query.queryies.IndustryId,
                                Query = query.queryies.Query,
                                IndustryName = query.industry.IndustriesName,
                                Createdon = query.queryies.CreatedOn
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return queryModel;
        }

        public List<KnowledgeQueryModel> GetAllQueriesByUserToLoginUser(int userId)
        {
            List<KnowledgeQueryModel> queryModel = new List<KnowledgeQueryModel>();

            try
            {
                var queries = (from queryies in context.KnowledgeQueries
                               join industry in context.Industries on queryies.IndustryId equals industry.IndustriesID
                               join user in context.UserIndustries on queryies.IndustryId equals user.IndustryId
                               where user.UserId == userId
                               select new { queryies, industry }).AsEnumerable();

                foreach (var query in queries)
                {
                    var queryAnswer = (from ans in context.KnowledgeQueryAnswers
                                       where ans.QueryID == query.queryies.Id && ans.UserId == userId
                                       select ans.ID).FirstOrDefault();

                    if (queryAnswer == 0)
                    {

                        if (!queryModel.Any(x => x.Id == query.queryies.Id))
                        {


                            queryModel.Add(new KnowledgeQueryModel()
                            {
                                Id = query.queryies.Id,
                                IndustryID = query.queryies.IndustryId,
                                Query = query.queryies.Query,
                                IndustryName = query.industry.IndustriesName,
                                Createdon = query.queryies.CreatedOn
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return queryModel;
        }
    }
}