using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using RMS.Authorization;
using RMS.SBJ.Makita;
using RMS.SBJ.Makita.Dtos;
using RMS.Web.Areas.App.Models.Makita;
using RMS.Web.Controllers;
using Abp.Web.Models;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPutAttribute = Microsoft.AspNetCore.Mvc.HttpPutAttribute;
using FromBodyAttribute = Microsoft.AspNetCore.Mvc.FromBodyAttribute;
using JsonResult = Microsoft.AspNetCore.Mvc.JsonResult;
using RMS.Web.Models.TenantSetup;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Registrations)]
    [DontWrapResult]
    public class MakitaRegistrationsController : RMSControllerBase
    {
        private readonly IMakitaRegistrationsAppService _makitaRegistrationsAppService;
        private readonly TenantSetupModel _tenantSetupModel;

        public MakitaRegistrationsController(IMakitaRegistrationsAppService makitaRegistrationsAppService, TenantSetupModel tenantSetupModel)
        {
            _makitaRegistrationsAppService = makitaRegistrationsAppService;
            _tenantSetupModel = tenantSetupModel;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<JsonResult> Create([FromBody] MakitaRegistrationsModel makitaRegistrationsModel)
        {

            var myTenantId = AbpSession.TenantId ?? 0;
            var tenantSettings = _tenantSetupModel.TenantSettings.Where(s => s.TenantId == myTenantId).FirstOrDefault();

            var registrationMapper = new MakitaRegistrationsDto
            {
                ActionCode = makitaRegistrationsModel.ActionCode,
                CompanyName = makitaRegistrationsModel.CompanyName,
                Gender = makitaRegistrationsModel.Gender,
                FirstName = makitaRegistrationsModel.FirstName,
                LastName = makitaRegistrationsModel.LastName,
                LegalForm = makitaRegistrationsModel.LegalForm,
                BusinessNumber = makitaRegistrationsModel.BusinessNumber,
                VatNumber = makitaRegistrationsModel.VatNumber,
                ZipCode = makitaRegistrationsModel.ZipCode,
                StreetName = makitaRegistrationsModel.StreetName,
                HouseNumber = makitaRegistrationsModel.HouseNumber,
                Residence = makitaRegistrationsModel.Residence,
                Country = makitaRegistrationsModel.Country,
                EmailAddress = makitaRegistrationsModel.EmailAddress,
                PhoneNumber = makitaRegistrationsModel.PhoneNumber,
                Debtor_number = makitaRegistrationsModel.Debtor_number,
                CampaignId = makitaRegistrationsModel.CampaignId,
                Contact_id = makitaRegistrationsModel.Contact_id
            };
            var productRegistrationsDto = new List<MakitaProductRegistrationsDto>();
            var productRegistrationList = makitaRegistrationsModel.ProductRegistrations.ToList();
            foreach (var product in productRegistrationList)
            {
                var productRegistrationDto = new MakitaProductRegistrationsDto
                {
                    Model = product.Model,
                    SerialNumber = product.SerialNumber,
                    PurchaseDate = product.PurchaseDate,
                    StorePurchased = product.StorePurchased,
                    InvoiceImagePath = product.InvoiceImagePath,
                    SerialNumberImagePath = product.SerialNumberImagePath,
                    Gift_id = product.Gift_id
                };
                productRegistrationsDto.Add(productRegistrationDto);
            }
            registrationMapper.ProductRegistrations = productRegistrationsDto;
            var registrationId = await _makitaRegistrationsAppService.Create(tenantSettings.RMSBlobStorage, tenantSettings.RMSBlobContainer, registrationMapper);

            if (registrationId == "campaignError")
            {
                var error = new
                {
                    RegistrationId = 0,
                    ErrorMessage = $" ActionCode: {makitaRegistrationsModel.ActionCode} is unknown",
                };
                return Json(error);
            }
            else if (registrationId == "countryError")
            {
                var error = new
                {
                    RegistrationId = 0,
                    ErrorMessage = $" Country: {makitaRegistrationsModel.Country} is unknown",
                };
                return Json(error);
            }
            else if (registrationId == "registrationInsertError")
            {
                var error = new
                {
                    RegistrationId = 0,
                    ErrorMessage = $"Registration insert has failed",
                };
                return Json(error);
            }
            else if (registrationId == "productError")
            {
                var error = new
                {
                    RegistrationId = 0,
                    ErrorMessage = $"Model is unknown",
                };
                return Json(error);
            }
            else if (registrationId == "retailerLocationtError")
            {
                var error = new
                {
                    RegistrationId = 0,
                    ErrorMessage = $"Retailer location is unknown",
                };
                return Json(error);
            }
            else if (registrationId == "purchaseRegistrationError")
            {
                var error = new
                {
                    RegistrationId = 0,
                    ErrorMessage = $"Purchase registration insert has failed.",
                };
                return Json(error);
            };

            var serializedJson = new { RegistrationId = registrationId };
            return Json(serializedJson);
        }

        [HttpGet]
        public async Task<object> GetRegistrationStatus(long registrationId)
        {
            var registrationStatusId = await _makitaRegistrationsAppService.GetRegistrationStatus(registrationId);
            return registrationStatusId;
        }

        [HttpPut]
        public async Task<object> Update([FromBody] MakitaRegistrationsModel makitaRegistrationsModel)
        {
            var myTenantId = AbpSession.TenantId ?? 0;
            var tenantSettings = _tenantSetupModel.TenantSettings.Where(s => s.TenantId == myTenantId).FirstOrDefault();

            var registrationMapper = new MakitaRegistrationsDto
            {
                RegistrationId = makitaRegistrationsModel.RegistrationId,
                ActionCode = makitaRegistrationsModel.ActionCode,
                CompanyName = makitaRegistrationsModel.CompanyName,
                Gender = makitaRegistrationsModel.Gender,
                FirstName = makitaRegistrationsModel.FirstName,
                LastName = makitaRegistrationsModel.LastName,
                LegalForm = makitaRegistrationsModel.LegalForm,
                BusinessNumber = makitaRegistrationsModel.BusinessNumber,
                VatNumber = makitaRegistrationsModel.VatNumber,
                ZipCode = makitaRegistrationsModel.ZipCode,
                StreetName = makitaRegistrationsModel.StreetName,
                HouseNumber = makitaRegistrationsModel.HouseNumber,
                Residence = makitaRegistrationsModel.Residence,
                Country = makitaRegistrationsModel.Country,
                EmailAddress = makitaRegistrationsModel.EmailAddress,
                PhoneNumber = makitaRegistrationsModel.PhoneNumber,
                Debtor_number = makitaRegistrationsModel.Debtor_number,
                CampaignId = makitaRegistrationsModel.CampaignId,
                Contact_id = makitaRegistrationsModel.Contact_id
            };
            var productRegistrationsDto = new List<MakitaProductRegistrationsDto>();
            var productRegistrationList = makitaRegistrationsModel.ProductRegistrations.ToList();
            foreach (var product in productRegistrationList)
            {
                var productRegistrationDto = new MakitaProductRegistrationsDto
                {
                    Model = product.Model,
                    SerialNumber = product.SerialNumber,
                    PurchaseDate = product.PurchaseDate,
                    StorePurchased = product.StorePurchased,
                    InvoiceImagePath = product.InvoiceImagePath,
                    SerialNumberImagePath = product.SerialNumberImagePath,
                    Gift_id = product.Gift_id
                };
                productRegistrationsDto.Add(productRegistrationDto);
            }
            registrationMapper.ProductRegistrations = productRegistrationsDto;
            var update = await _makitaRegistrationsAppService.Update(tenantSettings.RMSBlobStorage, tenantSettings.RMSBlobContainer, registrationMapper);
            var successMessage = new { registrationUpdate = "Success" };
            return Json(successMessage);
        }
    }
}
