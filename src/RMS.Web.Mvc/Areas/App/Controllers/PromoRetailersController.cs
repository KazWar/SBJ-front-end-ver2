using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.PromoRetailers;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.PromoPlanner;
using RMS.PromoPlanner.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_PromoRetailers)]
    public class PromoRetailersController : RMSControllerBase
    {
        private readonly IPromoRetailersAppService _promoRetailersAppService;

        public PromoRetailersController(IPromoRetailersAppService promoRetailersAppService)
        {
            _promoRetailersAppService = promoRetailersAppService;
        }

        public ActionResult Index()
        {
            var model = new PromoRetailersViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_PromoRetailers_Create, AppPermissions.Pages_PromoRetailers_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetPromoRetailerForEditOutput getPromoRetailerForEditOutput;

				if (id.HasValue){
					getPromoRetailerForEditOutput = await _promoRetailersAppService.GetPromoRetailerForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getPromoRetailerForEditOutput = new GetPromoRetailerForEditOutput{
						PromoRetailer = new CreateOrEditPromoRetailerDto()
					};
				}

				var viewModel = new CreateOrEditPromoRetailerModalViewModel()
				{
					PromoRetailer = getPromoRetailerForEditOutput.PromoRetailer,
					PromoPromocode = getPromoRetailerForEditOutput.PromoPromocode,
					RetailerCode = getPromoRetailerForEditOutput.RetailerCode,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewPromoRetailerModal(long id)
        {
			var getPromoRetailerForViewDto = await _promoRetailersAppService.GetPromoRetailerForView(id);

            var model = new PromoRetailerViewModel()
            {
                PromoRetailer = getPromoRetailerForViewDto.PromoRetailer
                , PromoPromocode = getPromoRetailerForViewDto.PromoPromocode 

                , RetailerCode = getPromoRetailerForViewDto.RetailerCode 

            };

            return PartialView("_ViewPromoRetailerModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_PromoRetailers_Create, AppPermissions.Pages_PromoRetailers_Edit)]
        public PartialViewResult PromoLookupTableModal(long? id, string displayName)
        {
            var viewModel = new PromoRetailerPromoLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_PromoRetailerPromoLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_PromoRetailers_Create, AppPermissions.Pages_PromoRetailers_Edit)]
        public PartialViewResult RetailerLookupTableModal(long? id, string displayName)
        {
            var viewModel = new PromoRetailerRetailerLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_PromoRetailerRetailerLookupTableModal", viewModel);
        }

    }
}