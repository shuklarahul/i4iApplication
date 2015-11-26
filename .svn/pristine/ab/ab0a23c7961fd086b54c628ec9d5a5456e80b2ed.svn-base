using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorldRef.BusinessLayer.IndustrySubCategory
{
    interface IIndustrySubCatAccessor
    {
        int SubAddIndustryCategory(IndustrySubCatModel subcategoryModel);

        IndustrySubCatModel GetParticularIndustrySubCategory(string SubCategoryName);

        IndustrySubCatModel GetParticularIndustrySubCategory(int SubCategoryId);

        List<IndustrySubCatModel> GetAllCategorySubIndustry(int IndustryId, int CategoryId);
    }
}