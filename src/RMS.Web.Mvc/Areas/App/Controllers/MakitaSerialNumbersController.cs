using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.MakitaSerialNumbers;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.MakitaSerialNumber;
using RMS.SBJ.MakitaSerialNumber.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_MakitaSerialNumbers)]
    public class MakitaSerialNumbersController : RMSControllerBase
    {
        private readonly IMakitaSerialNumbersAppService _makitaSerialNumbersAppService;

        public MakitaSerialNumbersController(IMakitaSerialNumbersAppService makitaSerialNumbersAppService)
        {
            _makitaSerialNumbersAppService = makitaSerialNumbersAppService;
        }

        public ActionResult Index()
        {
            var model = new MakitaSerialNumbersViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_MakitaSerialNumbers_Create, AppPermissions.Pages_MakitaSerialNumbers_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
            GetMakitaSerialNumberForEditOutput getMakitaSerialNumberForEditOutput;

            if (id.HasValue)
            {
                getMakitaSerialNumberForEditOutput = await _makitaSerialNumbersAppService.GetMakitaSerialNumberForEdit(new EntityDto<long> { Id = (long)id });
            }
            else
            {
                getMakitaSerialNumberForEditOutput = new GetMakitaSerialNumberForEditOutput
                {
                    MakitaSerialNumber = new CreateOrEditMakitaSerialNumberDto()
                };
            }

            var viewModel = new CreateOrEditMakitaSerialNumberModalViewModel()
            {
                MakitaSerialNumber = getMakitaSerialNumberForEditOutput.MakitaSerialNumber,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewMakitaSerialNumberModal(long id)
        {
            var getMakitaSerialNumberForViewDto = await _makitaSerialNumbersAppService.GetMakitaSerialNumberForView(id);

            var model = new MakitaSerialNumberViewModel()
            {
                MakitaSerialNumber = getMakitaSerialNumberForViewDto.MakitaSerialNumber
            };

            return PartialView("_ViewMakitaSerialNumberModal", model);
        }

    }
}