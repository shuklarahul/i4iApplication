using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorldRef.BusinessLayer.IndustryCategory
{
    public class IndustryCategoryAccessor : BaseAccessor, IIndustryCategoryAccessor
    {

        private IndustryCategoryModel CatModel = new IndustryCategoryModel();
        private List<IndustryCategoryModel> listCatModel = new List<IndustryCategoryModel>();

        public int AddIndustryCategory(IndustryCategoryModel categoryModel)
        {
            int CategoryId = 0;
            try
            {
                CategoryId = (from cat in DatabaseConnection.IndustryCategories
                              where cat.CategoryName.ToLower() == categoryModel.CategoryName.ToLower()
                              select cat.Id).FirstOrDefault();
                if (CategoryId == 0)
                {
                    DatabaseConnection.IndustryCategories.Add(new WorldRef.IndustryCategory()
                    {
                        IndustryId = categoryModel.IndustryId,
                        CategoryName = categoryModel.CategoryName,
                        createdon = DateTime.Now
                    });

                    DatabaseConnection.SaveChanges();

                    CategoryId = (from cat in DatabaseConnection.IndustryCategories
                                  where cat.CategoryName.ToLower() == categoryModel.CategoryName.ToLower()
                                  select cat.Id).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                CategoryId = 0;
            }
            return CategoryId;
        }

        public IndustryCategoryModel GetParticularIndustryCategory(string CategoryName)
        {
            try
            {
                WorldRef.IndustryCategory category = (from cat in DatabaseConnection.IndustryCategories
                                                            where cat.CategoryName.ToLower() == CategoryName.ToLower()
                                                            select cat).FirstOrDefault();
                CatModel.IndustryId = category.IndustryId;
                CatModel.Id = category.Id;
                CatModel.CategoryName = category.CategoryName;
            }
            catch (Exception ex)
            {
                new KnowledgeException(ex.Message);
            }

            return CatModel;

        }

        public IndustryCategoryModel GetParticularIndustryCategory(int CategoryId)
        {
            try
            {
                WorldRef.IndustryCategory category = (from cat in DatabaseConnection.IndustryCategories
                                                            where cat.Id == CategoryId
                                                            select cat).FirstOrDefault();
                CatModel.IndustryId = category.IndustryId;
                CatModel.Id = category.Id;
                CatModel.CategoryName = category.CategoryName;
            }
            catch (Exception ex)
            {
                new KnowledgeException(ex.Message);
            }

            return CatModel;
        }

        public List<IndustryCategoryModel> GetAllCategoryIndustry(int IndustryId)
        {
            try
            {
                var categories = (from cat in DatabaseConnection.IndustryCategories
                                  where cat.IndustryId == IndustryId
                                  select cat).AsEnumerable();

                foreach (var category in categories)
                {
                    listCatModel.Add(new IndustryCategoryModel()
                    {
                        Id = category.Id,
                        IndustryId = category.IndustryId,
                        CategoryName = category.CategoryName
                    });
                }
            }
            catch (Exception ex)
            {
                new KnowledgeException(ex.Message);
            }

            return listCatModel;
        }
    }
}