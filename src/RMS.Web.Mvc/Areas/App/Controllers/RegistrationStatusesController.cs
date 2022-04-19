using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.RegistrationStatuses;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.CodeTypeTables.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_RegistrationStatuses)]
    public class RegistrationStatusesController : RMSControllerBase
    {
        private readonly IRegistrationStatusesAppService _registrationStatusesAppService;

        public RegistrationStatusesController(IRegistrationStatusesAppService registrationStatusesAppService)
        {
            _registrationStatusesAppService = registrationStatusesAppService;
        }

        public ActionResult Index()
        {
            var model = new RegistrationStatusesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_RegistrationStatuses_Create, AppPermissions.Pages_RegistrationStatuses_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetRegistrationStatusForEditOutput getRegistrationStatusForEditOutput;

				if (id.HasValue){
					getRegistrationStatusForEditOutput = await _registrationStatusesAppService.GetRegistrationStatusForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getRegistrationStatusForEditOutput = new GetRegistrationStatusForEditOutput{
						RegistrationStatus = new CreateOrEditRegistrationStatusDto()
					};
				}

				var viewModel = new CreateOrEditRegistrationStatusModalViewModel()
				{
					RegistrationStatus = getRegistrationStatusForEditOutput.RegistrationStatus,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewRegistrationStatusModal(long id)
        {
			var getRegistrationStatusForViewDto = await _registrationStatusesAppService.GetRegistrationStatusForView(id);

            var model = new RegistrationStatusViewModel()
            {
                RegistrationStatus = getRegistrationStatusForViewDto.RegistrationStatus
            };

            return PartialView("_ViewRegistrationStatusModal", model);
        }


    }
}