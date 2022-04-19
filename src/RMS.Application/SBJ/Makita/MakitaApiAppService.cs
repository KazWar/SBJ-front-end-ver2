using Abp.Domain.Repositories;
using RMS.SBJ.CampaignProcesses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
//using IO.Swagger.Api;
//using IO.Swagger.Client;
//using IO.Swagger.Model;
using System.Net.Http;
using Abp.Runtime.Session;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using RMS.SBJ.Makita.Dtos;
using Microsoft.EntityFrameworkCore;
using RMS.SBJ.ProductHandlings;
using RMS.SBJ.HandlingLineProducts;
using RMS.SBJ.HandlingLines;
using RMS.SBJ.Products;
using RMS.SBJ.RetailerLocations;
using RMS.SBJ.CampaignRetailerLocations;
using RMS.SBJ.Retailers;
using System.Linq;
using RMS.SBJ.Registrations;
using RMS.SBJ.PurchaseRegistrations;
using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.PurchaseRegistrationFields;
using RMS.SBJ.PurchaseRegistrationFieldDatas;
using RMS.SBJ.RegistrationFields;
using RMS.SBJ.RegistrationFormFieldDatas;

namespace RMS.SBJ.Makita
{
    public class MakitaApiAppService : RMSAppServiceBase, IMakitaApiAppService
    {
        private readonly IRepository<Campaign, long> _campaignRepository;
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly ILogger<MakitaCampaignsAppService> _logger;
        private readonly IRepository<ProductHandling, long> _productHandlingRepository;
        private readonly IRepository<HandlingLineProduct, long> _handlingLineProductRepository;
        private readonly IRepository<HandlingLine, long> _handlingLineRepository;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IRepository<RetailerLocation, long> _lookup_retailerLocationRepository;
        private readonly IRepository<Retailer, long> _lookup_retailerRepository;
        private readonly IRepository<Registration, long> _lookup_registrationRepository;
        private readonly IRepository<PurchaseRegistration, long> _lookup_purchaseRegistrationRepository;
        private readonly IRepository<CampaignRetailerLocation, long> _lookup_campaignRetailerLocationRepository;
        private readonly ICampaignsAppService _campaignsAppService;
        private readonly IRegistrationStatusesAppService _registrationStatusesAppService;
        private readonly IRepository<RegistrationStatus, long> _lookup_registrationStatusRepository;
        private readonly IRepository<Country, long> _lookup_countyRepository;
        private readonly IRepository<PurchaseRegistrationField, long> _lookup_purchaseRegistrationFieldRepository;
        private readonly IRepository<PurchaseRegistrationFieldData, long> _lookup_purchaseRegistrationFieldDataRepository;
        private readonly IRepository<RegistrationField, long> _lookup_registrationFieldRepository;
        private readonly IRepository<RegistrationFieldData, long> _lookup_registrationFieldDataRepository;

        public MakitaApiAppService(
            IRepository<Campaign, long> campaignRepository,
            ILogger<MakitaCampaignsAppService> logger,
            IRepository<ProductHandling, long> productHandlingRepository,
            IRepository<HandlingLineProduct, long> handlingLineProductRepository,
            IRepository<HandlingLine, long> handlingLineRepository,
            IRepository<Product, long> lookup_productRepository,
            ICampaignsAppService campaignsAppService,
            IRepository<RetailerLocation, long> lookup_retailerLocationRepository,
            IRepository<Retailer, long> lookup_retailerRepository,
            IRepository<Registration, long> lookup_registrationRepository,
            IRepository<PurchaseRegistration, long> lookup_purchaseRegistrationRepository,
            IRegistrationStatusesAppService registrationStatusesAppService,
            IRepository<CampaignRetailerLocation, long> lookup_campaignRetailerLocationRepository,
            IRepository<RegistrationStatus, long> lookup_registrationStatusRepository,
            IRepository<Country, long> lookup_countyRepository,
            IRepository<PurchaseRegistrationField, long> lookup_purchaseRegistrationFieldRepository,
            IRepository<PurchaseRegistrationFieldData, long> lookup_purchaseRegistrationFieldDataRepository,
            IRepository<RegistrationField, long> lookup_registrationFieldRepository,
            IRepository<RegistrationFieldData, long> lookup_registrationFieldDataRepository

        )
        {
            _campaignRepository = campaignRepository;
            _logger = logger;
            _productHandlingRepository = productHandlingRepository;
            _handlingLineProductRepository = handlingLineProductRepository;
            _handlingLineRepository = handlingLineRepository;
            _lookup_productRepository = lookup_productRepository;
            _campaignsAppService = campaignsAppService;
            _lookup_retailerLocationRepository = lookup_retailerLocationRepository;
            _lookup_retailerRepository = lookup_retailerRepository;
            _lookup_registrationRepository = lookup_registrationRepository;
            _lookup_campaignRetailerLocationRepository = lookup_campaignRetailerLocationRepository;
            _lookup_purchaseRegistrationRepository = lookup_purchaseRegistrationRepository;
            _registrationStatusesAppService = registrationStatusesAppService;
            _lookup_registrationStatusRepository = lookup_registrationStatusRepository;
            _lookup_countyRepository = lookup_countyRepository;
            _lookup_purchaseRegistrationFieldRepository = lookup_purchaseRegistrationFieldRepository;
            _lookup_purchaseRegistrationFieldDataRepository = lookup_purchaseRegistrationFieldDataRepository;
            _lookup_registrationFieldRepository = lookup_registrationFieldRepository;
            _lookup_registrationFieldDataRepository = lookup_registrationFieldDataRepository;
        }


        public async Task<object> MakitaRegistrationApproved(long registrationId)
        {
            int tenantId = AbpSession.GetTenantId();

            if (tenantId != 8 && tenantId != 32)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }

            var registration = await _lookup_registrationRepository.GetAll().Where(r => r.Id == registrationId).FirstOrDefaultAsync();
            var campaign = await _campaignRepository.GetAsync(registration.CampaignId);
            var registrationStatus = await _lookup_registrationStatusRepository.GetAll().Where(rs => rs.Id == registration.RegistrationStatusId).FirstOrDefaultAsync();
            var country = await _lookup_countyRepository.GetAll().Where(c => c.Id == registration.CountryId).FirstOrDefaultAsync();
            var legalFormField = await _lookup_registrationFieldRepository.GetAll().Where(sn => sn.Description == "LegalForm").FirstOrDefaultAsync();
            var businessNumberField = await _lookup_registrationFieldRepository.GetAll().Where(sn => sn.Description == "BusinessNumber").FirstOrDefaultAsync();
            var vatNumberField = await _lookup_registrationFieldRepository.GetAll().Where(sn => sn.Description == "VatNumber").FirstOrDefaultAsync();

            var legalForm = await _lookup_registrationFieldDataRepository.GetAll().Where(sn => sn.RegistrationFieldId == legalFormField.Id && sn.RegistrationId == registrationId).FirstOrDefaultAsync();
            var businessNumber = await _lookup_registrationFieldDataRepository.GetAll().Where(sn => sn.RegistrationFieldId == businessNumberField.Id && sn.RegistrationId == registrationId).FirstOrDefaultAsync();
            var vatNumber = await _lookup_registrationFieldDataRepository.GetAll().Where(sn => sn.RegistrationFieldId == vatNumberField.Id && sn.RegistrationId == registrationId).FirstOrDefaultAsync();

            var Contact_IdField = await _lookup_registrationFieldRepository.GetAll().Where(sn => sn.Description == "Contact_Id").FirstOrDefaultAsync();
            var DebtorNumberField = await _lookup_registrationFieldRepository.GetAll().Where(sn => sn.Description == "Debtor_number").FirstOrDefaultAsync();

            var contact_Id = await _lookup_registrationFieldDataRepository.GetAll().Where(sn => sn.RegistrationFieldId == Contact_IdField.Id && sn.RegistrationId == registrationId).FirstOrDefaultAsync();
            if (contact_Id == null)
            {
                return null;
            }

            var debtor_number = await _lookup_registrationFieldDataRepository.GetAll().Where(sn => sn.RegistrationFieldId == DebtorNumberField.Id && sn.RegistrationId == registrationId).FirstOrDefaultAsync();
            if (debtor_number == null)
            {
                return null;
            }

            var companyName = "false";
            if (!string.IsNullOrWhiteSpace(registration.CompanyName))
            {
                companyName = "true";
            }

            var purchaseRegistrationsList = await _lookup_purchaseRegistrationRepository.GetAll().Where(pr => pr.RegistrationId == registrationId).ToListAsync();
            var registrationProductsList = new List<MakitaApiProductsDto>();
            var dealer_id = string.Empty;
            foreach (var purchaseRegistration in purchaseRegistrationsList)
            {
                var purchaseDate = purchaseRegistration.PurchaseDate;
                var linkedProduct = await _lookup_productRepository.GetAll().Where(lp => lp.Id == purchaseRegistration.ProductId).FirstOrDefaultAsync();
                var serialNumberField = await _lookup_purchaseRegistrationFieldRepository.GetAll().Where(sn => sn.Description == "SerialNumber").FirstOrDefaultAsync();
                var serialNumber = await _lookup_purchaseRegistrationFieldDataRepository.GetAll().Where(sn => sn.PurchaseRegistrationFieldId == serialNumberField.Id && sn.PurchaseRegistrationId == purchaseRegistration.Id).FirstOrDefaultAsync();
                var giftIdField = await _lookup_purchaseRegistrationFieldRepository.GetAll().Where(sn => sn.Description == "Gift_Id").FirstOrDefaultAsync();
                var giftId = await _lookup_purchaseRegistrationFieldDataRepository.GetAll().Where(sn => sn.PurchaseRegistrationFieldId == giftIdField.Id && sn.PurchaseRegistrationId == purchaseRegistration.Id).FirstOrDefaultAsync();
                var retailerLocation = await _lookup_retailerLocationRepository.GetAll().Where(rl => rl.Id == purchaseRegistration.RetailerLocationId).FirstOrDefaultAsync();
                dealer_id = retailerLocation.ExternalId;
                var product = new MakitaApiProductsDto
                {
                    product_id = linkedProduct.ProductCode,
                    serial_number = (serialNumber != null && !String.IsNullOrWhiteSpace(serialNumber.Value)) ? serialNumber.Value : "x",
                    choice_id = Convert.ToInt32(giftId.Value),
                    purchase_date = purchaseDate
                };
                registrationProductsList.Add(product);
            }

            var gender = registration.Gender;
            switch (gender)
            {
                case "0":
                case "X":
                    gender = "x";
                    break;
                case "M":
                    gender = "male";
                    break;
                case "F":
                    gender = "female";
                    break;
            }

            var approvedRegistration = new MakitaApiDto
            {
                campaign_id = Convert.ToInt32(campaign.ExternalId),
                dealer_id = Convert.ToInt32(dealer_id),
                debtor_number = debtor_number?.Value,
                contact_id = Convert.ToInt32(contact_Id?.Value),
                status = "archief",
                first_name = registration.FirstName,
                last_name = registration.LastName,
                gender = gender,
                email = registration.EmailAddress,
                phone_number = registration.PhoneNumber,
                streetname = registration.Street,
                house_number = registration.HouseNr,
                city = registration.City,
                zip_code = registration.PostalCode,
                country = country.CountryCode,
                coc_number = businessNumber?.Value,
                vat_number = vatNumber?.Value,
                company = companyName,
                products = registrationProductsList,
            };
           
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MakitaConsts.MakitaBearerToken);
               var url = "https://api.makita.nl/actioncampaign/v1/registrations";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            var sendContent = JsonConvert.SerializeObject(approvedRegistration);
            var json = JsonConvert.DeserializeObject(sendContent);

            var content = new StringContent(sendContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }
    }
}
