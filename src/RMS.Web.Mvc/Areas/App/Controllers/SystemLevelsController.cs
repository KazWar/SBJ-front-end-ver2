using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.SystemLevels;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.SystemTables;
using RMS.SBJ.SystemTables.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_SystemLevels)]
    public class SystemLevelsController : RMSControllerBase
    {
        private readonly ISystemLevelsAppService _systemLevelsAppService;

        public SystemLevelsController(ISystemLevelsAppService systemLevelsAppService)
        {
            _systemLevelsAppService = systemLevelsAppService;
        }

        public ActionResult Index()
        {
            var model = new SystemLevelsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_SystemLevels_Create, AppPermissions.Pages_SystemLevels_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetSystemLevelForEditOutput getSystemLevelForEditOutput;

				if (id.HasValue){
					getSystemLevelForEditOutput = await _systemLevelsAppService.GetSystemLevelForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getSystemLevelForEditOutput = new GetSystemLevelForEditOutput{
						SystemLevel = new CreateOrEditSystemLevelDto()
					};
				}

				var viewModel = new CreateOrEditSystemLevelModalViewModel()
				{
					SystemLevel = getSystemLevelForEditOutput.SystemLevel,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewSystemLevelModal(long id)
        {
			var getSystemLevelForViewDto = await _systemLevelsAppService.GetSystemLevelForView(id);

            var model = new SystemLevelViewModel()
            {
                SystemLevel = getSystemLevelForViewDto.SystemLevel
            };

            return PartialView("_ViewSystemLevelModal", model);
        }


    }
}