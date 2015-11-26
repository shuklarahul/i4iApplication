using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;

namespace WorldRef.BusinessLayer.IndustryCategory
{
    
        interface IIndustryCategoryAccessor
        {
            int AddIndustryCategory(IndustryCategoryModel categoryModel);

            IndustryCategoryModel GetParticularIndustryCategory(string CategoryName);

            IndustryCategoryModel GetParticularIndustryCategory(int CategoryId);

            List<IndustryCategoryModel> GetAllCategoryIndustry(int IndustryId);
        }
   
}