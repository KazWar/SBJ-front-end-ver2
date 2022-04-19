using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.ProcessEvents;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.CodeTypeTables.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_ProcessEvents)]
    public class ProcessEventsController : RMSControllerBase
    {
        private readonly IProcessEventsAppService _processEventsAppService;

        public ProcessEventsController(IProcessEventsAppService processEventsAppService)
        {
            _processEventsAppService = processEventsAppService;
        }

        public ActionResult Index()
        {
            var model = new ProcessEventsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_ProcessEvents_Create, AppPermissions.Pages_ProcessEvents_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetProcessEventForEditOutput getProcessEventForEditOutput;

				if (id.HasValue){
					getProcessEventForEditOutput = await _processEventsAppService.GetProcessEventForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getProcessEventForEditOutput = new GetProcessEventForEditOutput{
						ProcessEvent = new CreateOrEditProcessEventDto()
					};
				}

				var viewModel = new CreateOrEditProcessEventModalViewModel()
				{
					ProcessEvent = getProcessEventForEditOutput.ProcessEvent,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewProcessEventModal(long id)
        {
			var getProcessEventForViewDto = await _processEventsAppService.GetProcessEventForView(id);

            var model = new ProcessEventViewModel()
            {
                ProcessEvent = getProcessEventForViewDto.ProcessEvent
            };

            return PartialView("_ViewProcessEventModal", model);
        }


    }
}