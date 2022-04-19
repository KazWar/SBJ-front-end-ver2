using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.CampaignForms;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.CampaignProcesses;
using RMS.SBJ.CampaignProcesses.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using RMS.SBJ.Forms;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_CampaignForms)]
    public class CampaignFormsController : RMSControllerBase
    {
        private readonly ICampaignFormsAppService _campaignFormsAppService;
        private readonly IFormLocalesAppService _formLocalesAppService;

        public CampaignFormsController(ICampaignFormsAppService campaignFormsAppService, IFormLocalesAppService formLocalesAppService)
        {
            _campaignFormsAppService = campaignFormsAppService;
            _formLocalesAppService = formLocalesAppService;
        }

        public ActionResult Index()
        {
            var model = new CampaignFormsViewModel
			{
				FilterText = ""
			};

            return View(model);
        }





        [AbpMvcAuthorize(AppPermissions.Pages_CampaignForms_Create, AppPermissions.Pages_CampaignForms_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetCampaignFormForEditOutput getCampaignFormForEditOutput;

				if (id.HasValue){
					getCampaignFormForEditOutput = await _campaignFormsAppService.GetCampaignFormForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getCampaignFormForEditOutput = new GetCampaignFormForEditOutput{
						CampaignForm = new CreateOrEditCampaignFormDto()
					};
				}

				var viewModel = new CreateOrEditCampaignFormModalViewModel()
				{
					CampaignForm = getCampaignFormForEditOutput.CampaignForm,
					CampaignName = getCampaignFormForEditOutput.CampaignName,
					FormVersion = getCampaignFormForEditOutput.FormVersion,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}


        public async Task<PartialViewResult> ViewCampaignFormModal(long id)
        {
			var getCampaignFormForViewDto = await _campaignFormsAppService.GetCampaignFormForView(id);

            var model = new CampaignFormViewModel()
            {
                CampaignForm = getCampaignFormForViewDto.CampaignForm
                , CampaignName = getCampaignFormForViewDto.CampaignName 

                , FormVersion = getCampaignFormForViewDto.FormVersion 

            };

            return PartialView("_ViewCampaignFormModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_CampaignForms_Create, AppPermissions.Pages_CampaignForms_Edit)]
        public PartialViewResult CampaignLookupTableModal(long? id, string displayName)
        {
            var viewModel = new CampaignFormCampaignLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_CampaignFormCampaignLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_CampaignForms_Create, AppPermissions.Pages_CampaignForms_Edit)]
        public PartialViewResult FormLookupTableModal(long? id, string displayName)
        {
            var viewModel = new CampaignFormFormLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_CampaignFormFormLookupTableModal", viewModel);
        }

    }
}