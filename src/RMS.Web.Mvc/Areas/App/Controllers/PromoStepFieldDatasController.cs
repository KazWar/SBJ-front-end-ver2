using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.PromoStepFieldDatas;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.PromoPlanner;
using RMS.PromoPlanner.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_PromoStepFieldDatas)]
    public class PromoStepFieldDatasController : RMSControllerBase
    {
        private readonly IPromoStepFieldDatasAppService _promoStepFieldDatasAppService;

        public PromoStepFieldDatasController(IPromoStepFieldDatasAppService promoStepFieldDatasAppService)
        {
            _promoStepFieldDatasAppService = promoStepFieldDatasAppService;
        }

        public ActionResult Index()
        {
            var model = new PromoStepFieldDatasViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_Promos)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetPromoStepFieldDataForEditOutput getPromoStepFieldDataForEditOutput;

				if (id.HasValue){
					getPromoStepFieldDataForEditOutput = await _promoStepFieldDatasAppService.GetPromoStepFieldDataForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getPromoStepFieldDataForEditOutput = new GetPromoStepFieldDataForEditOutput{
						PromoStepFieldData = new CreateOrEditPromoStepFieldDataDto()
					};
				}

				var viewModel = new CreateOrEditPromoStepFieldDataModalViewModel()
				{
					PromoStepFieldData = getPromoStepFieldDataForEditOutput.PromoStepFieldData,
					PromoStepFieldDescription = getPromoStepFieldDataForEditOutput.PromoStepFieldDescription,
					PromoStepDataDescription = getPromoStepFieldDataForEditOutput.PromoStepDataDescription,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewPromoStepFieldDataModal(int id)
        {
			var getPromoStepFieldDataForViewDto = await _promoStepFieldDatasAppService.GetPromoStepFieldDataForView(id);

            var model = new PromoStepFieldDataViewModel()
            {
                PromoStepFieldData = getPromoStepFieldDataForViewDto.PromoStepFieldData
                , PromoStepFieldDescription = getPromoStepFieldDataForViewDto.PromoStepFieldDescription 

                , PromoStepDataDescription = getPromoStepFieldDataForViewDto.PromoStepDataDescription 

            };

            return PartialView("_ViewPromoStepFieldDataModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Promos)]
        public PartialViewResult PromoStepFieldLookupTableModal(int? id, string displayName)
        {
            var viewModel = new PromoStepFieldDataPromoStepFieldLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_PromoStepFieldDataPromoStepFieldLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_Promos)]
        public PartialViewResult PromoStepDataLookupTableModal(int? id, string displayName)
        {
            var viewModel = new PromoStepFieldDataPromoStepDataLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_PromoStepFieldDataPromoStepDataLookupTableModal", viewModel);
        }

    }
}