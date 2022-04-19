using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.FormLocales;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.Forms;
using RMS.SBJ.Forms.Dtos;
using Abp.Application.Services.Dto;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using Abp.Web.Models;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_FormLocales)]
    public class FormLocalesController : RMSControllerBase
    {
        private readonly IFormLocalesAppService _formLocalesAppService;
        private readonly IFormsAppService _formsAppService;
        //soon to be deprecated (hopefully)
        private readonly IPhilipsFormLocalesAppService _philipsFormLocalesAppService;

        public FormLocalesController(IFormLocalesAppService formLocalesAppService, IFormsAppService formsAppService, IPhilipsFormLocalesAppService philipsFormLocalesAppService)
        {
            _formLocalesAppService = formLocalesAppService;
            _formsAppService = formsAppService;
            _philipsFormLocalesAppService = philipsFormLocalesAppService;
        }

        public ActionResult Index()
        {
            var model = new FormLocalesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_FormLocales_Create, AppPermissions.Pages_FormLocales_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id, string category)
        {
            GetFormLocaleForEditOutput getFormLocaleForEditOutput;

            if (id.HasValue)
            {
                getFormLocaleForEditOutput = await _formLocalesAppService.GetFormLocaleForEdit(new EntityDto<long> { Id = (long)id });
            }
            else
            {
                if (category == FormConsts.Company)
                {
                    var getAllFormEntities = await _formsAppService.GetAllForms();
                    var getLatestCompanyForm = getAllFormEntities.Where(item => item.Form.SystemLevelId == FormConsts.CompanySLId).LastOrDefault();
                    getFormLocaleForEditOutput = new GetFormLocaleForEditOutput
                    {
                        FormLocale = new CreateOrEditFormLocaleDto { FormId = getLatestCompanyForm.Form.Id },
                        FormVersion = getLatestCompanyForm.Form.Version
                    };
                }
                else
                {
                    getFormLocaleForEditOutput = new GetFormLocaleForEditOutput
                    {
                        FormLocale = new CreateOrEditFormLocaleDto(),
                    };
                }
            }


            var viewModel = new CreateOrEditFormLocaleModalViewModel()
            {
                FormLocale = getFormLocaleForEditOutput.FormLocale,
                FormVersion = getFormLocaleForEditOutput.FormVersion,
                LocaleDescription = getFormLocaleForEditOutput.LocaleDescription,
                FormLocaleLocaleList = await _formLocalesAppService.GetAllLocaleForTableDropdown(),
            };
            ViewBag.Category = category;

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewFormLocaleModal(long id)
        {
            var getFormLocaleForViewDto = await _formLocalesAppService.GetFormLocaleForView(id);

            var model = new FormLocaleViewModel()
            {
                FormLocale = getFormLocaleForViewDto.FormLocale
                ,
                FormVersion = getFormLocaleForViewDto.FormVersion

                ,
                LocaleDescription = getFormLocaleForViewDto.LocaleDescription

            };

            return PartialView("_ViewFormLocaleModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_FormLocales_Create, AppPermissions.Pages_FormLocales_Edit)]
        public PartialViewResult FormLookupTableModal(long? id, string displayName)
        {
            var viewModel = new FormLocaleFormLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_FormLocaleFormLookupTableModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_FormLocales_Create, AppPermissions.Pages_FormLocales_Edit)]
        public PartialViewResult LocaleLookupTableModal(long? id, string displayName)
        {
            var viewModel = new FormLocaleLocaleLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_FormLocaleLocaleLookupTableModal", viewModel);
        }

        [HttpGet]
        [DontWrapResult]
        public async Task<JsonResult> GetFormAndProductHandlingApi(long currentCampaignId, string currentLocale)
        {
            var formAndProductHandling = await _formLocalesAppService.GetFormAndProductHandeling(currentCampaignId, currentLocale);

            return Json(formAndProductHandling);
        }

        [HttpGet]
        [DontWrapResult]
        public async Task<JsonResult> GetFormAndProductHandlingApi_PhilipsCoffee(long currentCampaignId, string currentLocale)
        {
            var formAndProductHandling = await _philipsFormLocalesAppService.GetFormAndProductHandeling(currentCampaignId, currentLocale);

            return Json(formAndProductHandling);
        }
    }
}