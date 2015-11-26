using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorldRef.BusinessLayer.IndustryKnowledge.KnowledgeQueryQuestion
{
    public class QueryQustion : BaseAccessor, IQueryQuestion
    {

        private List<KnowledgeQueryModel> _listQueryModel = new List<KnowledgeQueryModel>();

        public int AddQuery(KnowledgeQueryModel queryModel)
        {
            int queryId = 0;
            try
            {

                queryId = (from ind in DatabaseConnection.KnowledgeQueries
                           where ind.Query == queryModel.Query && ind.IndustryId == queryModel.IndustryId
                               && ind.IndustryCatId == queryModel.IndustryCatId && ind.IndustrySubCatId == queryModel.IndustrySubCatId
                           select ind.Id).FirstOrDefault();
                if (queryId == 0)
                {
                    DatabaseConnection.KnowledgeQueries.Add(new KnowledgeQuery()
                    {
                        IndustryId = queryModel.IndustryId,
                        IndustryCatId = queryModel.IndustryCatId,
                        IndustrySubCatId = queryModel.IndustrySubCatId,
                        Query = queryModel.Query
                    });

                    DatabaseConnection.SaveChanges();

                    queryId = (from ind in DatabaseConnection.KnowledgeQueries
                               where ind.Query == queryModel.Query && ind.IndustryId == queryModel.IndustryId
                                   && ind.IndustryCatId == queryModel.IndustryCatId && ind.IndustrySubCatId == queryModel.IndustrySubCatId
                               select ind.Id).FirstOrDefault();
                }
                else
                {
                    queryId = 0;
                }

            }
            catch (Exception ex)
            {
                queryId = -1;
                new KnowledgeException(ex.Message);
            }
            return queryId;
        }

        public List<KnowledgeQueryModel> ListQueryWithoutAnswer(int NumberOfQuestion)
        {
            try
            {
                var queries = (from ind in DatabaseConnection.KnowledgeQueries
                               join ans in DatabaseConnection.KnowledgeQueryAnswers on ind.Id equals ans.QueryID

                               select ind).FirstOrDefault();
            }
            catch (Exception ex)
            {
            }
            return _listQueryModel;
        }
    }
}