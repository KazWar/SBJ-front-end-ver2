using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.PromoSteps;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.PromoPlanner;
using RMS.PromoPlanner.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_PromoSteps)]
    public class PromoStepsController : RMSControllerBase
    {
        private readonly IPromoStepsAppService _promoStepsAppService;

        public PromoStepsController(IPromoStepsAppService promoStepsAppService)
        {
            _promoStepsAppService = promoStepsAppService;
        }

        public ActionResult Index()
        {
            var model = new PromoStepsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_PromoSteps_Create, AppPermissions.Pages_PromoSteps_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetPromoStepForEditOutput getPromoStepForEditOutput;

				if (id.HasValue){
					getPromoStepForEditOutput = await _promoStepsAppService.GetPromoStepForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getPromoStepForEditOutput = new GetPromoStepForEditOutput{
						PromoStep = new CreateOrEditPromoStepDto()
					};
				}

				var viewModel = new CreateOrEditPromoStepModalViewModel()
				{
					PromoStep = getPromoStepForEditOutput.PromoStep,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewPromoStepModal(int id)
        {
			var getPromoStepForViewDto = await _promoStepsAppService.GetPromoStepForView(id);

            var model = new PromoStepViewModel()
            {
                PromoStep = getPromoStepForViewDto.PromoStep
            };

            return PartialView("_ViewPromoStepModal", model);
        }


    }
}