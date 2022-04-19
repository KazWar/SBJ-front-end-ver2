using RMS.SBJ.Products.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.Products
{
    public class CreateOrEditProductModalViewModel
    {
       public CreateOrEditProductDto Product { get; set; }

	   		public string ProductCategoryDescription { get; set;}


       
	   public bool IsEditMode => Product.Id.HasValue;
    }
}