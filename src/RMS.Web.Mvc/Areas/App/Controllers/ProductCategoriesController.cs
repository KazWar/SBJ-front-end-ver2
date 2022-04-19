using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.ProductCategories;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.Products;
using RMS.SBJ.Products.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_ProductCategories)]
    public class ProductCategoriesController : RMSControllerBase
    {
        private readonly IProductCategoriesAppService _productCategoriesAppService;

        public ProductCategoriesController(IProductCategoriesAppService productCategoriesAppService)
        {
            _productCategoriesAppService = productCategoriesAppService;
        }

        public ActionResult Index()
        {
            var model = new ProductCategoriesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_ProductCategories_Create, AppPermissions.Pages_ProductCategories_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetProductCategoryForEditOutput getProductCategoryForEditOutput;

				if (id.HasValue){
					getProductCategoryForEditOutput = await _productCategoriesAppService.GetProductCategoryForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getProductCategoryForEditOutput = new GetProductCategoryForEditOutput{
						ProductCategory = new CreateOrEditProductCategoryDto()
					};
				}

				var viewModel = new CreateOrEditProductCategoryModalViewModel()
				{
					ProductCategory = getProductCategoryForEditOutput.ProductCategory,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewProductCategoryModal(long id)
        {
			var getProductCategoryForViewDto = await _productCategoriesAppService.GetProductCategoryForView(id);

            var model = new ProductCategoryViewModel()
            {
                ProductCategory = getProductCategoryForViewDto.ProductCategory
            };

            return PartialView("_ViewProductCategoryModal", model);
        }


    }
}