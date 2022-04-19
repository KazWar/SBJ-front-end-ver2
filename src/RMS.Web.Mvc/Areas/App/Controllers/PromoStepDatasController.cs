using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.PromoStepDatas;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.PromoPlanner;
using RMS.PromoPlanner.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Promos)]
    public class PromoStepDatasController : RMSControllerBase
    {
        private readonly IPromoStepDatasAppService _promoStepDatasAppService;

        public PromoStepDatasController(IPromoStepDatasAppService promoStepDatasAppService)
        {
            _promoStepDatasAppService = promoStepDatasAppService;
        }

        public ActionResult Index()
        {
            var model = new PromoStepDatasViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_Promos)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetPromoStepDataForEditOutput getPromoStepDataForEditOutput;

				if (id.HasValue){
					getPromoStepDataForEditOutput = await _promoStepDatasAppService.GetPromoStepDataForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getPromoStepDataForEditOutput = new GetPromoStepDataForEditOutput{
						PromoStepData = new CreateOrEditPromoStepDataDto()
					};
				getPromoStepDataForEditOutput.PromoStepData.ConfirmationDate = DateTime.Now;
				}

				var viewModel = new CreateOrEditPromoStepDataModalViewModel()
				{
					PromoStepData = getPromoStepDataForEditOutput.PromoStepData,
					PromoStepDescription = getPromoStepDataForEditOutput.PromoStepDescription,
					PromoPromocode = getPromoStepDataForEditOutput.PromoPromocode,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewPromoStepDataModal(int id)
        {
			var getPromoStepDataForViewDto = await _promoStepDatasAppService.GetPromoStepDataForView(id);

            var model = new PromoStepDataViewModel()
            {
                PromoStepData = getPromoStepDataForViewDto.PromoStepData
                , PromoStepDescription = getPromoStepDataForViewDto.PromoStepDescription 

                , PromoPromocode = getPromoStepDataForViewDto.PromoPromocode 

            };

            return PartialView("_ViewPromoStepDataModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Promos)]
        public PartialViewResult PromoStepLookupTableModal(int? id, string displayName)
        {
            var viewModel = new PromoStepDataPromoStepLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_PromoStepDataPromoStepLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_Promos)]
        public PartialViewResult PromoLookupTableModal(long? id, string displayName)
        {
            var viewModel = new PromoStepDataPromoLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_PromoStepDataPromoLookupTableModal", viewModel);
        }

    }
}