using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.PromoProducts;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.PromoPlanner;
using RMS.PromoPlanner.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_PromoProducts)]
    public class PromoProductsController : RMSControllerBase
    {
        private readonly IPromoProductsAppService _promoProductsAppService;

        public PromoProductsController(IPromoProductsAppService promoProductsAppService)
        {
            _promoProductsAppService = promoProductsAppService;
        }

        public ActionResult Index()
        {
            var model = new PromoProductsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_PromoProducts_Create, AppPermissions.Pages_PromoProducts_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetPromoProductForEditOutput getPromoProductForEditOutput;

				if (id.HasValue){
					getPromoProductForEditOutput = await _promoProductsAppService.GetPromoProductForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getPromoProductForEditOutput = new GetPromoProductForEditOutput{
						PromoProduct = new CreateOrEditPromoProductDto()
					};
				}

				var viewModel = new CreateOrEditPromoProductModalViewModel()
				{
					PromoProduct = getPromoProductForEditOutput.PromoProduct,
					PromoPromocode = getPromoProductForEditOutput.PromoPromocode,
					ProductCtn = getPromoProductForEditOutput.ProductCtn,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewPromoProductModal(long id)
        {
			var getPromoProductForViewDto = await _promoProductsAppService.GetPromoProductForView(id);

            var model = new PromoProductViewModel()
            {
                PromoProduct = getPromoProductForViewDto.PromoProduct
                , PromoPromocode = getPromoProductForViewDto.PromoPromocode 

                , ProductCtn = getPromoProductForViewDto.ProductCtn 

            };

            return PartialView("_ViewPromoProductModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_PromoProducts_Create, AppPermissions.Pages_PromoProducts_Edit)]
        public PartialViewResult PromoLookupTableModal(long? id, string displayName)
        {
            var viewModel = new PromoProductPromoLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_PromoProductPromoLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_PromoProducts_Create, AppPermissions.Pages_PromoProducts_Edit)]
        public PartialViewResult ProductLookupTableModal(long? id, string displayName)
        {
            var viewModel = new PromoProductProductLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_PromoProductProductLookupTableModal", viewModel);
        }

    }
}