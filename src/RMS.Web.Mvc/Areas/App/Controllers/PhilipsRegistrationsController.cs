using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.Registrations;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.Registrations;
using RMS.SBJ.Registrations.Dtos;
using Abp.Application.Services.Dto;
using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.Web.Models.AzureBlobStorage;
using RMS.AzureBlobStorage;
using RMS.Web.Models.TenantSetup;
using System.Linq;
using RMS.SBJ.Forms.Dtos;
using RMS.SBJ.Forms;
using System.Net.Http;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Registrations)]
    public class PhilipsRegistrationsController : RMSControllerBase
    {
        private readonly IPhilipsRegistrationsAppService _registrationsAppService;
        private readonly IRegistrationStatusesAppService _registrationStatusesAppService;
        private readonly IAzureBlobStorageAppService _azureBlobStorageAppService;
        private readonly IPhilipsFormLocalesAppService _formLocalesAppService;
        private readonly TenantSetupModel _tenantSetupModel;


        public PhilipsRegistrationsController(
            IPhilipsRegistrationsAppService registrationsAppService, 
            IRegistrationStatusesAppService registrationStatusesAppService, 
            IAzureBlobStorageAppService azureBlobStorageAppService, 
            IPhilipsFormLocalesAppService formLocalesAppService,
            TenantSetupModel tenantSetupModel)
        {
            _registrationsAppService = registrationsAppService;
            _registrationStatusesAppService = registrationStatusesAppService;
            _azureBlobStorageAppService = azureBlobStorageAppService;
            _formLocalesAppService = formLocalesAppService;
            _tenantSetupModel = tenantSetupModel;
        }

        public async Task<ActionResult> Index()
        {
            var model = new RegistrationsViewModel
            {
                FilterText = "",
                RegistrationStatuses = await _registrationStatusesAppService.GetAll(new GetAllRegistrationStatusesInput { IsActiveFilter = 1 })
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Registrations_Create, AppPermissions.Pages_Registrations_Edit)]
        public async Task<ActionResult> CreateOrEdit(long? id)
        {
            GetRegistrationForProcessingOutput getRegistrationForProcessingOutput;
            
            if (id.HasValue)
            {
                getRegistrationForProcessingOutput = await _registrationsAppService.GetRegistrationForProcessing(new EntityDto<long> { Id = (long)id });
            }
            else
            {
                getRegistrationForProcessingOutput = new GetRegistrationForProcessingOutput
                {
                    Registration = new CreateOrEditRegistrationDto()
                };
            }

            var viewModel = new ProcessingRegistrationViewModel
            {
                CampaignTitle = getRegistrationForProcessingOutput.CampaignTitle,
                CampaignType = getRegistrationForProcessingOutput.CampaignType,
                CampaignStartDate = getRegistrationForProcessingOutput.CampaignStartDate.ToString("dd-MM-yyyy"),
                CampaignEndDate = getRegistrationForProcessingOutput.CampaignEndDate.ToString("dd-MM-yyyy"),
                DateCreated = getRegistrationForProcessingOutput.DateCreated,
                Registration = getRegistrationForProcessingOutput.Registration,
                RelatedRegistrationsByEmail = getRegistrationForProcessingOutput.RelatedRegistrationsByEmail,
                RelatedRegistrationsBySerialNumber = getRegistrationForProcessingOutput.RelatedRegistrationsBySerialNumber,
                RegistrationHistoryEntries = getRegistrationForProcessingOutput.RegistrationHistoryEntries,
                RegistrationMessageHistoryEntries = getRegistrationForProcessingOutput.RegistrationMessageHistoryEntries,
                FormBlocks = getRegistrationForProcessingOutput.FormBlocks,
                RejectionReasons = getRegistrationForProcessingOutput.RejectionReasons,
                SelectedRejectionReasonId = getRegistrationForProcessingOutput.SelectedRejectionReasonId,
                StatusCode = getRegistrationForProcessingOutput.StatusCode,
                StatusIsChangeable = getRegistrationForProcessingOutput.StatusIsChangeable,
                TypeOfChange = getRegistrationForProcessingOutput.TypeOfChange,
            };

            return View(viewModel);
        }

        //Added parameter - chatConversationId so as to load ViewRegistration when new RMSChat notification link is clicked
        public async Task<ActionResult> ViewRegistration(ulong id)
        {
            if (id == 0)
            {
                throw new Exception("Parameter 'id' cannot be of value 0.");
            }

            GetRegistrationForProcessingOutput getRegistrationForProcessingOutput = await _registrationsAppService.GetRegistrationForProcessing(new EntityDto<long> { Id = (long)id });

            var viewModel = new ProcessingRegistrationViewModel
            {
                CampaignTitle = getRegistrationForProcessingOutput.CampaignTitle,
                CampaignType = getRegistrationForProcessingOutput.CampaignType,
                CampaignStartDate = getRegistrationForProcessingOutput.CampaignStartDate.ToString("dd-MM-yyyy"),
                CampaignEndDate = getRegistrationForProcessingOutput.CampaignEndDate.ToString("dd-MM-yyyy"),
                DateCreated = getRegistrationForProcessingOutput.DateCreated,
                Registration = getRegistrationForProcessingOutput.Registration,
                RelatedRegistrationsByEmail = getRegistrationForProcessingOutput.RelatedRegistrationsByEmail,
                RegistrationHistoryEntries = getRegistrationForProcessingOutput.RegistrationHistoryEntries,
                RegistrationMessageHistoryEntries = getRegistrationForProcessingOutput.RegistrationMessageHistoryEntries,
                FormBlocks = getRegistrationForProcessingOutput.FormBlocks,
                RejectionReasons = getRegistrationForProcessingOutput.RejectionReasons,
                SelectedRejectionReasonId = getRegistrationForProcessingOutput.SelectedRejectionReasonId,
                StatusCode = getRegistrationForProcessingOutput.StatusCode,
                StatusIsChangeable = false, // because this is a view-/readonly-mode
                TypeOfChange = getRegistrationForProcessingOutput.TypeOfChange,
            };

            return View(viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Registrations_Create, AppPermissions.Pages_Registrations_Edit)]
        public PartialViewResult RegistrationStatusLookupTableModal(long? id, string displayName)
        {
            var viewModel = new RegistrationRegistrationStatusLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_RegistrationRegistrationStatusLookupTableModal", viewModel);
        }

        [HttpGet]
        public async Task<JsonResult> LoadCloudImage(string blobPath, [FromServices] TenantSetupModel tenantSetup)
        {            
            var myTenantId = AbpSession.TenantId ?? 0;
            var tenantSettings = tenantSetup.TenantSettings.Where(s => s.TenantId == myTenantId).FirstOrDefault();

            var imageBase64String = await _azureBlobStorageAppService.DownloadBase64StringAsync(tenantSettings.RMSBlobStorage, tenantSettings.RMSBlobContainer, blobPath);
            
            var model = new CloudImageModel
            {
                ImageBase64String = imageBase64String
            };

            return Json(model);
        }

        [HttpPost]
        public async Task<JsonResult> GetEditForRegistration([FromBody] GetFormLayoutAndDataInput input)
        {
            var result = await _formLocalesAppService.GetFormLayoutAndDataForRegistration(input);

            return Json(result ?? null);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> SendFormData([FromBody] PhilipsFormRegistrationHandlingDto vueJsToRmsModel)
        {
            var myTenantId = AbpSession.TenantId ?? 0;
            var tenantSettings = _tenantSetupModel.TenantSettings.Where(s => s.TenantId == myTenantId).FirstOrDefault();

            var registrationMapper = new PhilipsFormRegistrationHandlingDto
            {
                Data = vueJsToRmsModel.Data
            };

            var formsService = await _registrationsAppService.SendFormData(tenantSettings.RMSBlobStorage, tenantSettings.RMSBlobContainer, registrationMapper);

            if (formsService == true)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            }

            return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
        }
    }
}