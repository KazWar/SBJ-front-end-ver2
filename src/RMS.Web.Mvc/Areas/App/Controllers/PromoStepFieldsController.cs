using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.PromoStepFields;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.PromoPlanner;
using RMS.PromoPlanner.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_PromoStepFields)]
    public class PromoStepFieldsController : RMSControllerBase
    {
        private readonly IPromoStepFieldsAppService _promoStepFieldsAppService;

        public PromoStepFieldsController(IPromoStepFieldsAppService promoStepFieldsAppService)
        {
            _promoStepFieldsAppService = promoStepFieldsAppService;
        }

        public ActionResult Index()
        {
            var model = new PromoStepFieldsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_PromoStepFields_Create, AppPermissions.Pages_PromoStepFields_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetPromoStepFieldForEditOutput getPromoStepFieldForEditOutput;

				if (id.HasValue){
					getPromoStepFieldForEditOutput = await _promoStepFieldsAppService.GetPromoStepFieldForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getPromoStepFieldForEditOutput = new GetPromoStepFieldForEditOutput{
						PromoStepField = new CreateOrEditPromoStepFieldDto()
					};
				}

				var viewModel = new CreateOrEditPromoStepFieldModalViewModel()
				{
					PromoStepField = getPromoStepFieldForEditOutput.PromoStepField,
					PromoStepDescription = getPromoStepFieldForEditOutput.PromoStepDescription,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewPromoStepFieldModal(int id)
        {
			var getPromoStepFieldForViewDto = await _promoStepFieldsAppService.GetPromoStepFieldForView(id);

            var model = new PromoStepFieldViewModel()
            {
                PromoStepField = getPromoStepFieldForViewDto.PromoStepField
                , PromoStepDescription = getPromoStepFieldForViewDto.PromoStepDescription 

            };

            return PartialView("_ViewPromoStepFieldModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_PromoStepFields_Create, AppPermissions.Pages_PromoStepFields_Edit)]
        public PartialViewResult PromoStepLookupTableModal(int? id, string displayName)
        {
            var viewModel = new PromoStepFieldPromoStepLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_PromoStepFieldPromoStepLookupTableModal", viewModel);
        }

    }
}