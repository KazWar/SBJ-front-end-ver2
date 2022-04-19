using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.ProductCategoryYearPOs;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.PromoPlanner;
using RMS.PromoPlanner.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_ProductCategoryYearPos)]
    public class ProductCategoryYearPOsController : RMSControllerBase
    {
        private readonly IProductCategoryYearPosAppService _productCategoryYearPOsAppService;

        public ProductCategoryYearPOsController(IProductCategoryYearPosAppService productCategoryYearPOsAppService)
        {
            _productCategoryYearPOsAppService = productCategoryYearPOsAppService;
        }

        public ActionResult Index()
        {
            var model = new ProductCategoryYearPOsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_ProductCategoryYearPos_Create, AppPermissions.Pages_ProductCategoryYearPos_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetProductCategoryYearPoForEditOutput getProductCategoryYearPOForEditOutput;

				if (id.HasValue){
					getProductCategoryYearPOForEditOutput = await _productCategoryYearPOsAppService.GetProductCategoryYearPoForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getProductCategoryYearPOForEditOutput = new GetProductCategoryYearPoForEditOutput{
						ProductCategoryYearPo = new CreateOrEditProductCategoryYearPoDto()
					};
				}

				var viewModel = new CreateOrEditProductCategoryYearPOModalViewModel()
				{
					ProductCategoryYearPO = getProductCategoryYearPOForEditOutput.ProductCategoryYearPo,
					ProductCategoryCode = getProductCategoryYearPOForEditOutput.ProductCategoryCode,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewProductCategoryYearPOModal(long id)
        {
			var getProductCategoryYearPOForViewDto = await _productCategoryYearPOsAppService.GetProductCategoryYearPoForView(id);

            var model = new ProductCategoryYearPOViewModel()
            {
                ProductCategoryYearPo = getProductCategoryYearPOForViewDto.ProductCategoryYearPo
                , ProductCategoryCode = getProductCategoryYearPOForViewDto.ProductCategoryCode 

            };

            return PartialView("_ViewProductCategoryYearPOModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_ProductCategoryYearPos_Create, AppPermissions.Pages_ProductCategoryYearPos_Edit)]
        public PartialViewResult ProductCategoryLookupTableModal(long? id, string displayName)
        {
            var viewModel = new ProductCategoryYearPOProductCategoryLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_ProductCategoryYearPOProductCategoryLookupTableModal", viewModel);
        }

    }
}