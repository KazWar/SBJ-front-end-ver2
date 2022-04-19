using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.CampaignCategories;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.CodeTypeTables.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_CampaignCategories)]
    public class CampaignCategoriesController : RMSControllerBase
    {
        private readonly ICampaignCategoriesAppService _campaignCategoriesAppService;

        public CampaignCategoriesController(ICampaignCategoriesAppService campaignCategoriesAppService)
        {
            _campaignCategoriesAppService = campaignCategoriesAppService;
        }

        public ActionResult Index()
        {
            var model = new CampaignCategoriesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_CampaignCategories_Create, AppPermissions.Pages_CampaignCategories_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetCampaignCategoryForEditOutput getCampaignCategoryForEditOutput;

				if (id.HasValue){
					getCampaignCategoryForEditOutput = await _campaignCategoriesAppService.GetCampaignCategoryForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getCampaignCategoryForEditOutput = new GetCampaignCategoryForEditOutput{
						CampaignCategory = new CreateOrEditCampaignCategoryDto()
					};
				}

				var viewModel = new CreateOrEditCampaignCategoryModalViewModel()
				{
					CampaignCategory = getCampaignCategoryForEditOutput.CampaignCategory,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewCampaignCategoryModal(long id)
        {
			var getCampaignCategoryForViewDto = await _campaignCategoriesAppService.GetCampaignCategoryForView(id);

            var model = new CampaignCategoryViewModel()
            {
                CampaignCategory = getCampaignCategoryForViewDto.CampaignCategory
            };

            return PartialView("_ViewCampaignCategoryModal", model);
        }


    }
}