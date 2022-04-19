using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.RegistrationHistories;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.RegistrationHistory;
using RMS.SBJ.RegistrationHistory.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_RegistrationHistories)]
    public class RegistrationHistoriesController : RMSControllerBase
    {
        private readonly IRegistrationHistoriesAppService _registrationHistoriesAppService;

        public RegistrationHistoriesController(IRegistrationHistoriesAppService registrationHistoriesAppService)
        {
            _registrationHistoriesAppService = registrationHistoriesAppService;
        }

        public ActionResult Index()
        {
            var model = new RegistrationHistoriesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_RegistrationHistories_Create, AppPermissions.Pages_RegistrationHistories_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
            GetRegistrationHistoryForEditOutput getRegistrationHistoryForEditOutput;

            if (id.HasValue)
            {
                getRegistrationHistoryForEditOutput = await _registrationHistoriesAppService.GetRegistrationHistoryForEdit(new EntityDto<long> { Id = (long)id });
            }
            else
            {
                getRegistrationHistoryForEditOutput = new GetRegistrationHistoryForEditOutput
                {
                    RegistrationHistory = new CreateOrEditRegistrationHistoryDto()
                };
                getRegistrationHistoryForEditOutput.RegistrationHistory.DateCreated = DateTime.Now;
            }

            var viewModel = new CreateOrEditRegistrationHistoryModalViewModel()
            {
                RegistrationHistory = getRegistrationHistoryForEditOutput.RegistrationHistory,
                RegistrationStatusStatusCode = getRegistrationHistoryForEditOutput.RegistrationStatusStatusCode,
                RegistrationFirstName = getRegistrationHistoryForEditOutput.RegistrationFirstName,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewRegistrationHistoryModal(long id)
        {
            var getRegistrationHistoryForViewDto = await _registrationHistoriesAppService.GetRegistrationHistoryForView(id);

            var model = new RegistrationHistoryViewModel()
            {
                RegistrationHistory = getRegistrationHistoryForViewDto.RegistrationHistory
                ,
                RegistrationStatusStatusCode = getRegistrationHistoryForViewDto.RegistrationStatusStatusCode

                ,
                RegistrationFirstName = getRegistrationHistoryForViewDto.RegistrationFirstName

            };

            return PartialView("_ViewRegistrationHistoryModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_RegistrationHistories_Create, AppPermissions.Pages_RegistrationHistories_Edit)]
        public PartialViewResult RegistrationStatusLookupTableModal(long? id, string displayName)
        {
            var viewModel = new RegistrationHistoryRegistrationStatusLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_RegistrationHistoryRegistrationStatusLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_RegistrationHistories_Create, AppPermissions.Pages_RegistrationHistories_Edit)]
        public PartialViewResult RegistrationLookupTableModal(long? id, string displayName)
        {
            var viewModel = new RegistrationHistoryRegistrationLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_RegistrationHistoryRegistrationLookupTableModal", viewModel);
        }

    }
}