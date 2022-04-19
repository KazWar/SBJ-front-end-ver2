using RMS.SBJ.Products.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.ProductCategories
{
    public class CreateOrEditProductCategoryModalViewModel
    {
       public CreateOrEditProductCategoryDto ProductCategory { get; set; }

	   
       
	   public bool IsEditMode => ProductCategory.Id.HasValue;
    }
}