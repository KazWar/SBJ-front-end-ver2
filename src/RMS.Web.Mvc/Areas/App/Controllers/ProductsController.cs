using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.Products;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.Products;
using RMS.SBJ.Products.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Products)]
    public class ProductsController : RMSControllerBase
    {
        private readonly IProductsAppService _productsAppService;

        public ProductsController(IProductsAppService productsAppService)
        {
            _productsAppService = productsAppService;
        }

        public ActionResult Index()
        {
            var model = new ProductsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_Products_Create, AppPermissions.Pages_Products_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetProductForEditOutput getProductForEditOutput;

				if (id.HasValue){
					getProductForEditOutput = await _productsAppService.GetProductForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getProductForEditOutput = new GetProductForEditOutput{
						Product = new CreateOrEditProductDto()
					};
				}

				var viewModel = new CreateOrEditProductModalViewModel()
				{
					Product = getProductForEditOutput.Product,
					ProductCategoryDescription = getProductForEditOutput.ProductCategoryDescription,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewProductModal(long id)
        {
			var getProductForViewDto = await _productsAppService.GetProductForView(id);

            var model = new ProductViewModel()
            {
                Product = getProductForViewDto.Product
                , ProductCategoryDescription = getProductForViewDto.ProductCategoryDescription 

            };

            return PartialView("_ViewProductModal", model);
        }


    }
}