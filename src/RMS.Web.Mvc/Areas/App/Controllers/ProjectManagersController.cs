using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.ProjectManagers;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.Company;
using RMS.SBJ.Company.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_ProjectManagers)]
    public class ProjectManagersController : RMSControllerBase
    {
        private readonly IProjectManagersAppService _projectManagersAppService;

        public ProjectManagersController(IProjectManagersAppService projectManagersAppService)
        {
            _projectManagersAppService = projectManagersAppService;
        }

        public ActionResult Index()
        {
            var model = new ProjectManagersViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_ProjectManagers_Create, AppPermissions.Pages_ProjectManagers_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetProjectManagerForEditOutput getProjectManagerForEditOutput;

				if (id.HasValue){
					getProjectManagerForEditOutput = await _projectManagersAppService.GetProjectManagerForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getProjectManagerForEditOutput = new GetProjectManagerForEditOutput{
						ProjectManager = new CreateOrEditProjectManagerDto()
					};
				}

				var viewModel = new CreateOrEditProjectManagerModalViewModel()
				{
					ProjectManager = getProjectManagerForEditOutput.ProjectManager,
					AddressPostalCode = getProjectManagerForEditOutput.AddressPostalCode,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewProjectManagerModal(long id)
        {
			var getProjectManagerForViewDto = await _projectManagersAppService.GetProjectManagerForView(id);

            var model = new ProjectManagerViewModel()
            {
                ProjectManager = getProjectManagerForViewDto.ProjectManager
                , AddressPostalCode = getProjectManagerForViewDto.AddressPostalCode 

            };

            return PartialView("_ViewProjectManagerModal", model);
        }


    }
}