using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.HandlingBatches;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.HandlingBatch;
using RMS.SBJ.HandlingBatch.Dtos.Premium;
using RMS.SBJ.HandlingBatch.Dtos.CashRefund;
using RMS.Web.Models.TenantSetup;
using RMS.SBJ.HandlingBatch.Dtos.ActivationCode;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_HandlingBatches)]
    public class HandlingBatchesController : RMSControllerBase
    {
        private readonly IHandlingBatchesAppService _handlingBatchesAppService;
        private readonly TenantSetupModel _tenantSetupModel;

        public HandlingBatchesController(IHandlingBatchesAppService handlingBatchesAppService, TenantSetupModel tenantSetupModel)
        {
            _handlingBatchesAppService = handlingBatchesAppService;
            _tenantSetupModel = tenantSetupModel;
        }

        public ActionResult Index()
        {
            var myTenantId = AbpSession.TenantId ?? 0;
            var tenantSettings = _tenantSetupModel.TenantSettings.Where(s => s.TenantId == myTenantId).FirstOrDefault();

            var model = new HandlingBatchesViewModel
            {
                FilterText = String.Empty,
                WarehouseId = tenantSettings.WarehouseId,
                OrderUserId = tenantSettings.OrderUserId,
                Password = tenantSettings.Password,
                SepaInitiator = tenantSettings.SepaInitiator 
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_HandlingBatches_Create, AppPermissions.Pages_HandlingBatches_Edit, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        public async Task<ActionResult> CreatePremium()
        {
            var myTenantId = AbpSession.TenantId ?? 0;
            var tenantSettings = _tenantSetupModel.TenantSettings.Where(s => s.TenantId == myTenantId).FirstOrDefault();
           
            var input = new GetInformationForNewPremiumBatchInput()
            {
                CampaignBatchables = String.Empty,
                WarehouseId = tenantSettings.WarehouseId,
                OrderUserId = tenantSettings.OrderUserId,
                Password = tenantSettings.Password
            };

            var output = await _handlingBatchesAppService.GetInformationForNewPremiumBatch(input);

            var model = new NewHandlingBatchViewModel()
            {
                InformationForNewPremiumBatch = output
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_HandlingBatches_Create, AppPermissions.Pages_HandlingBatches_Edit, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        public async Task<ActionResult> CreateCashRefund()
        {           
            var input = new GetInformationForNewCashRefundBatchInput()
            {
                CampaignBatchables = String.Empty
            };

            var output = await _handlingBatchesAppService.GetInformationForNewCashRefundBatch(input);

            var model = new NewHandlingBatchViewModel()
            {
                InformationForNewCashRefundBatch = output
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_HandlingBatches_Create, AppPermissions.Pages_HandlingBatches_Edit, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        public async Task<ActionResult> CreateActivationCode()
        {
            var input = new GetInformationForNewActivationCodeBatchInput()
            {
                CampaignBatchables = String.Empty
            };

            var output = await _handlingBatchesAppService.GetInformationForNewActivationCodeBatch(input);

            var model = new NewHandlingBatchViewModel()
            {
                InformationForNewActivationCodeBatch = output
            };

            return View(model);
        }

        public async Task<ActionResult> ViewPremium(long id)
        {
            var myTenantId = AbpSession.TenantId ?? 0;
            var tenantSettings = _tenantSetupModel.TenantSettings.Where(s => s.TenantId == myTenantId).FirstOrDefault();

            var input = new GetPremiumBatchForData()
            {
                Id = id,
                WarehouseId = tenantSettings.WarehouseId,
                OrderUserId = tenantSettings.OrderUserId,
                Password = tenantSettings.Password
            };

            var output = await _handlingBatchesAppService.GetPremiumBatchForView(input);

            var model = new ViewHandlingBatchViewModel()
            {
                 PremiumBatchForView = output
            };

            return View(model);
        }

        public async Task<ActionResult> ViewCashRefund(long id)
        {
            var myTenantId = AbpSession.TenantId ?? 0;
            var tenantSettings = _tenantSetupModel.TenantSettings.Where(s => s.TenantId == myTenantId).FirstOrDefault();

            var output = await _handlingBatchesAppService.GetCashRefundBatchForView(id);

            var model = new ViewHandlingBatchViewModel()
            {
                CashRefundBatchForView = output
            };

            model.CashRefundBatchForView.SepaInitiator = tenantSettings.SepaInitiator;

            return View(model);
        }

        public async Task<ActionResult> ViewActivationCode(long id)
        {
            var myTenantId = AbpSession.TenantId ?? 0;
            var tenantSettings = _tenantSetupModel.TenantSettings.Where(s => s.TenantId == myTenantId).FirstOrDefault();

            var output = await _handlingBatchesAppService.GetActivationCodeBatchForView(id);

            var model = new ViewHandlingBatchViewModel()
            {
                ActivationCodeBatchForView = output
            };

            return View(model);
        }

        public async Task<ActionResult> ViewHandlingBatch(long id)
        {
            var getHandlingBatchForViewDto = await _handlingBatchesAppService.GetHandlingBatchForView(id);

            var model = new HandlingBatchViewModel()
            {
                HandlingBatch = getHandlingBatchForViewDto.HandlingBatch
                ,
                CampaignTypeName = getHandlingBatchForViewDto.CampaignTypeName

                ,
                HandlingBatchStatusStatusDescription = getHandlingBatchForViewDto.HandlingBatchStatusStatusDescription

            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_HandlingBatches_Create, AppPermissions.Pages_HandlingBatches_Edit, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        public PartialViewResult CampaignTypeLookupTableModal(long? id, string displayName)
        {
            var viewModel = new HandlingBatchCampaignTypeLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_HandlingBatchCampaignTypeLookupTableModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_HandlingBatches_Create, AppPermissions.Pages_HandlingBatches_Edit, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        public PartialViewResult HandlingBatchStatusLookupTableModal(long? id, string displayName)
        {
            var viewModel = new HandlingBatchHandlingBatchStatusLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_HandlingBatchHandlingBatchStatusLookupTableModal", viewModel);
        }

    }
}