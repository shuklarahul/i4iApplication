using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorldRef.BusinessLayer.IndustrySubCategory
{
    public class IndustrySubCategoryAccessor : BaseAccessor, IIndustrySubCatAccessor
    {
        private IndustrySubCatModel SubCatModel = new IndustrySubCatModel();
        private List<IndustrySubCatModel> listSubCatModel = new List<IndustrySubCatModel>();

        public int SubAddIndustryCategory(IndustrySubCatModel subcategoryModel)
        {
            int SubCategoryId = 0;
            try
            {
                SubCategoryId = (from sub in DatabaseConnection.IndustrySubCategories
                                 where sub.SubCategoryName.ToLower() == subcategoryModel.SubCategoryName.ToLower()
                                 select sub.Id).FirstOrDefault();
                if (SubCategoryId == 0)
                {
                    DatabaseConnection.IndustrySubCategories.Add(new WorldRef.IndustrySubCategory()
                    {
                        IndustryId = subcategoryModel.IndustryId,
                        IndusCategoryId = subcategoryModel.CategoryId,
                        SubCategoryName = subcategoryModel.SubCategoryName,
                        createdon = DateTime.Now

                    });

                    DatabaseConnection.SaveChanges();

                    SubCategoryId = (from sub in DatabaseConnection.IndustrySubCategories
                                     where sub.SubCategoryName.ToLower() == subcategoryModel.SubCategoryName.ToLower()
                                     select sub.Id).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                SubCategoryId = 0;
                new KnowledgeException(ex.Message);
            }
            return SubCategoryId;
        }

        public IndustrySubCatModel GetParticularIndustrySubCategory(string SubCategoryName)
        {
            try
            {
                WorldRef.IndustrySubCategory category = (from cat in DatabaseConnection.IndustrySubCategories
                                                               where cat.SubCategoryName.ToLower() == SubCategoryName.ToLower()
                                                               select cat).FirstOrDefault();

                SubCatModel.Id = category.Id;
                SubCatModel.IndustryId = category.IndusCategoryId;
                SubCatModel.SubCategoryName = category.SubCategoryName;
                SubCatModel.CategoryId = category.IndusCategoryId;
            }
            catch (Exception ex)
            {
                new KnowledgeException(ex.Message);
            }
            return SubCatModel;
        }

        public IndustrySubCatModel GetParticularIndustrySubCategory(int SubCategoryId)
        {
            try
            {
                WorldRef.IndustrySubCategory category = (from cat in DatabaseConnection.IndustrySubCategories
                                                               where cat.Id == SubCategoryId
                                                               select cat).FirstOrDefault();

                SubCatModel.Id = category.Id;
                SubCatModel.IndustryId = category.IndusCategoryId;
                SubCatModel.SubCategoryName = category.SubCategoryName;
                SubCatModel.CategoryId = category.IndusCategoryId;
            }
            catch (Exception ex)
            {
                new KnowledgeException(ex.Message);
            }
            return SubCatModel;
        }

        public List<IndustrySubCatModel> GetAllCategorySubIndustry(int IndustryId, int CategoryId)
        {
            try
            {
                var categories = (from cat in DatabaseConnection.IndustrySubCategories
                                  where cat.IndustryId == IndustryId && cat.IndusCategoryId == CategoryId
                                  select cat).AsEnumerable();

                foreach (var category in categories)
                {
                    listSubCatModel.Add(new IndustrySubCatModel()
                    {
                        Id = category.Id,
                        IndustryId = category.IndustryId,
                        CategoryId = category.IndusCategoryId,
                        SubCategoryName = category.SubCategoryName
                    });
                }
            }
            catch (Exception ex)
            {
                new KnowledgeException(ex.Message);
            }

            return listSubCatModel;
        }
    }
}