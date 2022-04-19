using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.RegistrationJsonDatas;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.RegistrationJsonData;
using RMS.SBJ.RegistrationJsonData.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_RegistrationJsonDatas)]
    public class RegistrationJsonDatasController : RMSControllerBase
    {
        private readonly IRegistrationJsonDatasAppService _registrationJsonDatasAppService;

        public RegistrationJsonDatasController(IRegistrationJsonDatasAppService registrationJsonDatasAppService)
        {
            _registrationJsonDatasAppService = registrationJsonDatasAppService;
        }

        public ActionResult Index()
        {
            var model = new RegistrationJsonDatasViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_RegistrationJsonDatas_Create, AppPermissions.Pages_RegistrationJsonDatas_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
            GetRegistrationJsonDataForEditOutput getRegistrationJsonDataForEditOutput;

            if (id.HasValue)
            {
                getRegistrationJsonDataForEditOutput = await _registrationJsonDatasAppService.GetRegistrationJsonDataForEdit(new EntityDto<long> { Id = (long)id });
            }
            else
            {
                getRegistrationJsonDataForEditOutput = new GetRegistrationJsonDataForEditOutput
                {
                    RegistrationJsonData = new CreateOrEditRegistrationJsonDataDto()
                };
                getRegistrationJsonDataForEditOutput.RegistrationJsonData.DateCreated = DateTime.Now;
            }

            var viewModel = new CreateOrEditRegistrationJsonDataModalViewModel()
            {
                RegistrationJsonData = getRegistrationJsonDataForEditOutput.RegistrationJsonData,
                RegistrationFirstName = getRegistrationJsonDataForEditOutput.RegistrationFirstName,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewRegistrationJsonDataModal(long id)
        {
            var getRegistrationJsonDataForViewDto = await _registrationJsonDatasAppService.GetRegistrationJsonDataForView(id);

            var model = new RegistrationJsonDataViewModel()
            {
                RegistrationJsonData = getRegistrationJsonDataForViewDto.RegistrationJsonData
                ,
                RegistrationFirstName = getRegistrationJsonDataForViewDto.RegistrationFirstName

            };

            return PartialView("_ViewRegistrationJsonDataModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_RegistrationJsonDatas_Create, AppPermissions.Pages_RegistrationJsonDatas_Edit)]
        public PartialViewResult RegistrationLookupTableModal(long? id, string displayName)
        {
            var viewModel = new RegistrationJsonDataRegistrationLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_RegistrationJsonDataRegistrationLookupTableModal", viewModel);
        }

    }
}