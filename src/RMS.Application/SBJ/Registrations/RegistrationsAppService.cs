using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Abp.Authorization;
using Abp.Linq.Extensions;
using Abp.Domain.Repositories;
using Abp.Application.Services.Dto;
using RMS.Dto;
using RMS.SBJ.Forms;
using RMS.Authorization;
using RMS.SBJ.CampaignProcesses;
using RMS.SBJ.PurchaseRegistrations;
using RMS.SBJ.RegistrationFormFieldDatas;
using RMS.SBJ.PurchaseRegistrationFields;
using RMS.SBJ.PurchaseRegistrationFieldDatas;
using RMS.SBJ.Registrations.Dtos.ProcessRegistration;
using RMS.SBJ.Registrations.Exporting;
using RMS.SBJ.Registrations.Dtos;
using RMS.SBJ.Products;
using RMS.SBJ.Retailers;
using RMS.SBJ.RetailerLocations;
using RMS.SBJ.Registrations.Helpers;
using RMS.SBJ.RegistrationHistory;
using RMS.SBJ.CodeTypeTables;
using RMS.Authorization.Users;
using RMS.SBJ.CampaignProcesses.Dtos;
using System.Text;
using RMS.SBJ.CodeTypeTables.Dtos;
using System.Reflection;
using RMS.SBJ.Messaging;
using RMS.SBJ.Messaging.Dtos;
using RMS.SBJ.Forms.Dtos;
using Newtonsoft.Json;
using Abp.Runtime.Session;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using System.IO;
using RMS.SBJ.HandlingLines;
using RMS.SBJ.ProductHandlings;
using RMS.SBJ.HandlingLineProducts;
using RMS.SBJ.RegistrationFields;
using RMS.SBJ.Messaging.Helpers;
using RMS.SBJ.Helpers;
using System.Security.Cryptography;
using RMS.SBJ.Makita;
using RMS.SBJ.ProductGifts;
using RMS.SBJ.CampaignProcesses.Helpers;

namespace RMS.SBJ.Registrations
{
    [AbpAuthorize(AppPermissions.Pages_Registrations)]
    public class RegistrationsAppService : RMSAppServiceBase, IRegistrationsAppService
    {
        private readonly ILogger<RegistrationsAppService> _logger;
        private readonly IRegistrationsExcelExporter _registrationsExcelExporter;
        private readonly IRepository<Registration, long> _registrationRepository;
        private readonly IRepository<Company.Company, long> _companyRepository;
        private readonly IRepository<CodeTypeTables.CampaignType, long> _campaignTypeRepository;
        private readonly IRepository<RegistrationStatus, long> _lookup_registrationStatusRepository;
        private readonly IRepository<PurchaseRegistration, long> _lookup_purchaseRegistrationRepository;
        private readonly IRepository<RegistrationField, long> _lookup_RegistrationFieldRepository;
        private readonly IRepository<RegistrationFieldData, long> _lookup_registrationFieldDataRepository;
        private readonly IRepository<PurchaseRegistrationField, long> _lookup_purchaseRegistrationFieldRepository;
        private readonly IRepository<PurchaseRegistrationFieldData, long> _lookup_purchaseRegistrationFieldDataRepository;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IRepository<ProductHandling, long> _lookup_productHandlingRepository;
        private readonly IRepository<ProductGift, long> _lookup_productGiftRepository;
        private readonly IRepository<Retailer, long> _lookup_retailerRepository;
        private readonly IRepository<RetailerLocation, long> _lookup_retailerLocationRepository;
        private readonly IRepository<Campaign, long> _lookup_campaignRepository;
        private readonly IRepository<CampaignCountry, long> _lookup_campaignCountryRepository;
        private readonly IRepository<CampaignForm, long> _lookup_campaignFormRepository;
        private readonly IRepository<CampaignTranslation, long> _lookup_campaignTranslationRepository;
        private readonly IRepository<FormLocale, long> _lookup_formLocaleRepository;
        private readonly IRepository<Forms.FormBlock, long> _lookup_formBlockRepository;
        private readonly IRepository<FormBlockField, long> _lookup_formBlockFieldRepository;
        private readonly IRepository<Forms.FormField, long> _lookup_formFieldRepository;
        private readonly IRepository<FormFieldTranslation, long> _lookup_formFieldTranslationRepository;
        private readonly IRepository<FieldType, long> _lookup_fieldTypeRepository;
        private readonly IRepository<FormFieldValueList, long> _lookup_formFieldValueListRepository;
        private readonly IRepository<ListValue, long> _lookup_listValueRepository;
        private readonly IRepository<ListValueTranslation, long> _lookup_listValueTranslationRepository;
        private readonly IRepository<RegistrationHistory.RegistrationHistory, long> _lookup_registrationHistoryRepository;
        private readonly IRepository<MakitaSerialNumber.MakitaSerialNumber, long> _lookup_makitaSerialNumberRepository;
        private readonly IRepository<CampaignCampaignType, long> _lookup_campaignCampaignTypeRepository;
        private readonly IRepository<CampaignTypeEvent, long> _lookup_campaignTypeEventRepository;
        private readonly IRepository<CampaignTypeEventRegistrationStatus, long> _lookup_campaignTypeEventRegistrationStatusRepository;
        private readonly IRepository<MessageComponentContent, long> _lookup_messageComponentContentRepository;
        private readonly IRepository<MessageContentTranslation, long> _lookup_messageContentTranslationRepository;
        private readonly IRepository<MessageHistory, long> _lookup_messageHistoryRepository;
        private readonly IRepository<MessageVariable, long> _lookup_messageVariableRepository;
        private readonly IRepository<MessageComponent, long> _lookup_messageComponentRepository;
        private readonly IRepository<ProductHandling, long> _productHandlingRepository;
        private readonly IRepository<HandlingLine, long> _lookup_handlingLineRepository;
        private readonly IRepository<HandlingLineProduct, long> _lookup_handlingLineProductRepository;
        private readonly IRepository<RejectionReasonTranslation, long> _lookup_rejectionReasonTranslationRepository;
        private readonly IRepository<CampaignMessage, long> _lookup_campaignMessageRepository;
        private readonly IRepository<MessageType, long> _lookup_messageTypeRepository;
        private readonly IRepository<Country, long> _lookup_countryRepository;
        private readonly IRepository<Locale, long> _lookup_localeRepository;
        private readonly IRepository<RegistrationJsonData.RegistrationJsonData, long> _lookup_registrationJsonDataRepository;
        private readonly IRepository<HandlingLineRetailers.HandlingLineRetailer, long> _lookup_handlingLineRetailersRepository;
        private readonly IRejectionReasonsAppService _rejectionReasonsAppService;
        private readonly IRegistrationStatusesAppService _registrationStatusesAppService;
        private readonly IRegistrationHistoriesAppService _registrationHistoriesAppService;
        private readonly IUserAppService _userAppService;
        private readonly IProductsAppService _productsAppService;
        private readonly ICountriesAppService _countriesAppService;
        private readonly IMessagingAppService _messagingAppService;
        private readonly IRetailersAppService _retailersAppService;
        private readonly IMakitaApiAppService _makitaApiAppService;

        //BAT Decryption
        private readonly string SecretKey = "c0unterf3itM*ch#";
        private readonly string InitialVector = "Th1ngsrem_vedlie";
        private readonly string FormatCode = "KBQF";

        public RegistrationsAppService(
            ILogger<RegistrationsAppService> logger,
            IRegistrationsExcelExporter registrationsExcelExporter,
            IRepository<Registration, long> registrationRepository,
            IRepository<Company.Company, long> companyRepository,
            IRepository<CodeTypeTables.CampaignType, long> campaignTypeRepository,
            IRepository<RegistrationStatus, long> lookup_registrationStatusRepository,
            IRepository<PurchaseRegistration, long> lookup_purchaseRegistrationRepository,
            IRepository<RegistrationField, long> lookup_RegistrationFieldRepository,
            IRepository<RegistrationFieldData, long> lookup_RegistrationFieldDataRepository,
            IRepository<PurchaseRegistrationField, long> lookup_purchaseRegistrationFieldRepository,
            IRepository<PurchaseRegistrationFieldData, long> lookup_purchaseRegistrationFieldDataRepository,
            IRepository<Product, long> lookup_productRepository,
            IRepository<ProductHandling, long> lookup_productHandlingRepository,
            IRepository<ProductGift, long> lookup_productGiftRepository,
            IRepository<Retailer, long> lookup_retailerRepository,
            IRepository<RetailerLocation, long> lookup_retailerLocationRepository,
            IRepository<Campaign, long> lookup_campaignRepository,
            IRepository<CampaignCountry, long> lookup_campaignCountryRepository,
            IRepository<CampaignForm, long> lookup_campaignFormRepository,
            IRepository<CampaignTranslation, long> lookup_campaignTranslationRepository,
            IRepository<FormLocale, long> lookup_formLocaleRepository,
            IRepository<Forms.FormBlock, long> lookup_formBlockRepository,
            IRepository<FormBlockField, long> lookup_formBlockFieldRepository,
            IRepository<Forms.FormField, long> lookup_formFieldRepository,
            IRepository<FormFieldTranslation, long> lookup_formFieldTranslationRepository,
            IRepository<FieldType, long> lookup_fieldTypeRepository,
            IRepository<FormFieldValueList, long> lookup_formFieldValueListRepository,
            IRepository<ListValue, long> lookup_listValueRepository,
            IRepository<ListValueTranslation, long> lookup_listValueTranslationRepository,
            IRepository<RegistrationHistory.RegistrationHistory, long> lookup_registrationHistoryRepository,
            IRepository<RejectionReasonTranslation, long> lookup_rejectionReasonTranslationRepository,
            IRepository<MakitaSerialNumber.MakitaSerialNumber, long> lookup_makitaSerialNumberRepository,
            IRepository<CampaignCampaignType, long> lookup_campaignCampaignTypeRepository,
            IRepository<CampaignTypeEvent, long> lookup_campaignTypeEventRepository,
            IRepository<CampaignTypeEventRegistrationStatus, long> lookup_campaignTypeEventRegistrationStatusRepository,
            IRepository<MessageComponentContent, long> lookup_messageComponentContentRepository,
            IRepository<MessageContentTranslation, long> lookup_messageContentTranslationRepository,
            IRepository<MessageHistory, long> lookup_messageHistoryRepository,
            IRepository<MessageVariable, long> lookup_messageVariableRepository,
            IRepository<MessageComponent, long> lookup_messageComponentRepository,
            IRepository<ProductHandling, long> productHandlingRepository,
            IRepository<HandlingLine, long> lookup_handlingLineRepository,
            IRepository<HandlingLineProduct, long> lookup_handlingLineProductRepository,
            IRepository<CampaignMessage, long> lookup_campaignMessageRepository,
            IRepository<MessageType, long> lookup_messageTypeRepository,
            IRepository<Country, long> lookup_countryRepository,
            IRepository<Locale, long> lookup_localeRepository,
            IRepository<RegistrationJsonData.RegistrationJsonData, long> lookup_registrationJsonDataRepository,
            IRepository<HandlingLineRetailers.HandlingLineRetailer, long> lookup_handlingLineRetailersRepository,
            IRejectionReasonsAppService rejectionReasonsAppService,
            IRegistrationStatusesAppService registrationStatusesAppService,
            IRegistrationHistoriesAppService registrationHistoriesAppService,
            IUserAppService userAppService,
            IProductsAppService productsAppService,
            ICountriesAppService countriesAppService,
            IMessagingAppService messagingAppService,
            IRetailersAppService retailersAppService,
            IMakitaApiAppService makitaApiAppService
            )
        {
            _logger = logger;
            _registrationsExcelExporter = registrationsExcelExporter;
            _registrationRepository = registrationRepository;
            _companyRepository = companyRepository;
            _campaignTypeRepository = campaignTypeRepository;
            _lookup_registrationStatusRepository = lookup_registrationStatusRepository;
            _lookup_purchaseRegistrationRepository = lookup_purchaseRegistrationRepository;
            _lookup_RegistrationFieldRepository = lookup_RegistrationFieldRepository;
            _lookup_registrationFieldDataRepository = lookup_RegistrationFieldDataRepository;
            _lookup_purchaseRegistrationFieldRepository = lookup_purchaseRegistrationFieldRepository;
            _lookup_purchaseRegistrationFieldDataRepository = lookup_purchaseRegistrationFieldDataRepository;
            _lookup_productRepository = lookup_productRepository;
            _lookup_productHandlingRepository = lookup_productHandlingRepository;
            _lookup_productGiftRepository = lookup_productGiftRepository;
            _lookup_retailerRepository = lookup_retailerRepository;
            _lookup_retailerLocationRepository = lookup_retailerLocationRepository;
            _lookup_campaignRepository = lookup_campaignRepository;
            _lookup_campaignCountryRepository = lookup_campaignCountryRepository;
            _lookup_campaignFormRepository = lookup_campaignFormRepository;
            _lookup_campaignTranslationRepository = lookup_campaignTranslationRepository;
            _lookup_formLocaleRepository = lookup_formLocaleRepository;
            _lookup_formBlockRepository = lookup_formBlockRepository;
            _lookup_formBlockFieldRepository = lookup_formBlockFieldRepository;
            _lookup_formFieldRepository = lookup_formFieldRepository;
            _lookup_formFieldTranslationRepository = lookup_formFieldTranslationRepository;
            _lookup_fieldTypeRepository = lookup_fieldTypeRepository;
            _lookup_formFieldValueListRepository = lookup_formFieldValueListRepository;
            _lookup_listValueRepository = lookup_listValueRepository;
            _lookup_listValueTranslationRepository = lookup_listValueTranslationRepository;
            _lookup_registrationHistoryRepository = lookup_registrationHistoryRepository;
            _lookup_makitaSerialNumberRepository = lookup_makitaSerialNumberRepository;
            _lookup_campaignCampaignTypeRepository = lookup_campaignCampaignTypeRepository;
            _lookup_campaignTypeEventRepository = lookup_campaignTypeEventRepository;
            _lookup_campaignTypeEventRegistrationStatusRepository = lookup_campaignTypeEventRegistrationStatusRepository;
            _lookup_messageComponentContentRepository = lookup_messageComponentContentRepository;
            _lookup_messageContentTranslationRepository = lookup_messageContentTranslationRepository;
            _lookup_messageHistoryRepository = lookup_messageHistoryRepository;
            _lookup_messageVariableRepository = lookup_messageVariableRepository;
            _lookup_messageComponentRepository = lookup_messageComponentRepository;
            _lookup_countryRepository = lookup_countryRepository;
            _lookup_registrationJsonDataRepository = lookup_registrationJsonDataRepository;
            _lookup_handlingLineRetailersRepository = lookup_handlingLineRetailersRepository;
            _productHandlingRepository = productHandlingRepository;
            _lookup_localeRepository = lookup_localeRepository;
            _lookup_handlingLineRepository = lookup_handlingLineRepository;
            _lookup_handlingLineProductRepository = lookup_handlingLineProductRepository;
            _lookup_rejectionReasonTranslationRepository = lookup_rejectionReasonTranslationRepository;
            _lookup_campaignMessageRepository = lookup_campaignMessageRepository;
            _lookup_messageTypeRepository = lookup_messageTypeRepository;
            _rejectionReasonsAppService = rejectionReasonsAppService;
            _registrationStatusesAppService = registrationStatusesAppService;
            _registrationHistoriesAppService = registrationHistoriesAppService;
            _userAppService = userAppService;
            _productsAppService = productsAppService;
            _countriesAppService = countriesAppService;
            _messagingAppService = messagingAppService;
            _retailersAppService = retailersAppService;
            _makitaApiAppService = makitaApiAppService;
        }

        public async Task<PagedResultDto<GetRegistrationForViewDto>> GetAll(GetAllRegistrationsInput input)
        {
            // SerialNumber search
            var registrationsList = new List<long>();
            if (!String.IsNullOrWhiteSpace(input.SerialNumberFilter))
            {
                var selectedRegistrations = from o in _lookup_purchaseRegistrationFieldDataRepository.GetAll()
                                            join o1 in _lookup_purchaseRegistrationRepository.GetAll() on o.PurchaseRegistrationId equals o1.Id
                                            where o.PurchaseRegistrationFieldId == 3 && o.Value.Contains(input.SerialNumberFilter)
                                            select o1.RegistrationId;

                if (selectedRegistrations.Count() > 0)
                {
                    registrationsList = await selectedRegistrations.Distinct().ToListAsync();
                }
            };

            var filteredRegistrations = _registrationRepository.GetAll()
                .Include(e => e.RegistrationStatusFk)
                .Include(e => e.CampaignFormFk)
                .Include(e => e.LocaleFk)
                .WhereIf(!String.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.FirstName.Contains(input.Filter) || e.LastName.Contains(input.Filter) || e.Id.ToString().StartsWith(input.Filter) ||
                         e.Street.Contains(input.Filter) || e.HouseNr.Contains(input.Filter) ||
                         e.PostalCode.Contains(input.Filter) || e.City.Contains(input.Filter) ||
                         e.EmailAddress.Contains(input.Filter) || e.PhoneNumber.Contains(input.Filter) ||
                         e.CompanyName.Contains(input.Filter) || e.Gender.Contains(input.Filter))
                .WhereIf(!String.IsNullOrWhiteSpace(input.FirstNameFilter), e => e.FirstName.Contains(input.FirstNameFilter))
                .WhereIf(!String.IsNullOrWhiteSpace(input.LastNameFilter), e => e.LastName.Contains(input.LastNameFilter))
                .WhereIf(!String.IsNullOrWhiteSpace(input.CityFilter), e => e.City.Contains(input.CityFilter))
                .WhereIf(!String.IsNullOrWhiteSpace(input.EmailAddressFilter),
                    e => e.EmailAddress.Contains(input.EmailAddressFilter))
                .WhereIf(Convert.ToInt64(input.RegistrationStatusFilter) != -1,
                    e => e.RegistrationStatusId == Convert.ToInt64(input.RegistrationStatusFilter))
                .WhereIf(!String.IsNullOrWhiteSpace(input.SerialNumberFilter),
                    e => registrationsList.Contains(e.Id))
                .WhereIf(Convert.ToInt64(input.CampaignDescriptionFilter) != -1,
                    e => e.CampaignFormFk.CampaignId == Convert.ToInt64(input.CampaignDescriptionFilter))
                .WhereIf(Convert.ToInt64(input.CampaignDescriptionFilter) == -1 && input.ActiveCampaignsOnlyFilter,
                    e => e.CampaignFormFk.CampaignFk.EndDate >= DateTime.Today);

            var pagedAndFilteredRegistrations = filteredRegistrations
                .OrderBy(r => r.Id)
                .PageBy(input);

            var registrationHistories = from o in _lookup_registrationHistoryRepository.GetAll()
                                        group o by o.RegistrationId into h
                                        select new
                                        {
                                            RegistrationId = h.Key,
                                            DateCreated = h.Min(x => x.DateCreated)
                                        };

            var registrations = from o in pagedAndFilteredRegistrations
                                join o1 in _lookup_registrationStatusRepository.GetAll() on o.RegistrationStatusId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()
                                join o2 in _lookup_campaignRepository.GetAll() on o.CampaignId equals o2.Id into j2
                                from s2 in j2.DefaultIfEmpty()
                                join o3 in registrationHistories on o.Id equals o3.RegistrationId into j3
                                from s3 in j3.DefaultIfEmpty()
                                where Convert.ToInt64(input.CampaignDescriptionFilter) == -1 || s2.Id == Convert.ToInt64(input.CampaignDescriptionFilter)
                                //where String.IsNullOrWhiteSpace(input.CampaignDescriptionFilter) || input.CampaignDescriptionFilter != null && input.CampaignDescriptionFilter.Length > 0 && s2.Description.Contains(input.CampaignDescriptionFilter)
                                select new GetRegistrationForViewDto()
                                {
                                    Registration = new RegistrationDto
                                    {
                                        Id = o.Id,
                                        CountryId = o.CountryId,
                                        CampaignId = o.CampaignId,
                                        FirstName = o.FirstName,
                                        LastName = o.LastName,
                                        Street = o.Street,
                                        HouseNr = o.HouseNr,
                                        PostalCode = o.PostalCode,
                                        City = o.City,
                                        EmailAddress = o.EmailAddress,
                                        PhoneNumber = o.PhoneNumber,
                                        CompanyName = o.CompanyName,
                                        Gender = o.Gender

                                    },
                                    ExternalCode = s2.ExternalCode,
                                    ProductCode = String.Empty,
                                    CampaignDescription = s2.Description,
                                    RegistrationStatusStatusCode = s1 == null || s1.StatusCode == null ? String.Empty : s1.StatusCode,
                                    DateCreated = s3 == null ? String.Empty : s3.DateCreated.ToString("dd-MM-yyyy HH:mm")
                                };

            var registrationsAsList = await registrations.ToListAsync();

            foreach (var registration in registrationsAsList)
            {
                var purchaseRegistration = await _lookup_purchaseRegistrationRepository.FirstOrDefaultAsync(record => record.RegistrationId == registration.Registration.Id);
                if (purchaseRegistration == null)
                {
                    continue;
                }

                var product = await _lookup_productRepository.FirstOrDefaultAsync(record => record.Id == purchaseRegistration.ProductId);
                if (product != null)
                {
                    registration.ProductCode = product.ProductCode;
                }
            }

            var totalCount = await filteredRegistrations.CountAsync();

            return new PagedResultDto<GetRegistrationForViewDto>(
                totalCount,
                registrationsAsList
            );
        }

        public async Task<GetRegistrationForViewDto> GetRegistrationForView(long id)
        {
            var registration = await _registrationRepository.GetAsync(id);

            var output = new GetRegistrationForViewDto { Registration = ObjectMapper.Map<RegistrationDto>(registration) };

            var lookupRegistrationStatus = await _lookup_registrationStatusRepository.FirstOrDefaultAsync(output.Registration.RegistrationStatusId);
            output.RegistrationStatusStatusCode = lookupRegistrationStatus?.StatusCode;

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Registrations_Edit)]
        public async Task<GetRegistrationForEditOutput> GetRegistrationForEdit(EntityDto<long> input)
        {
            var registration = await _registrationRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetRegistrationForEditOutput { Registration = ObjectMapper.Map<CreateOrEditRegistrationDto>(registration) };

            var lookupRegistrationStatus = await _lookup_registrationStatusRepository.FirstOrDefaultAsync(output.Registration.RegistrationStatusId);
            output.RegistrationStatusStatusCode = lookupRegistrationStatus?.StatusCode;

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Registrations_Edit)]
        public async Task<bool> SaveRegistrationForProcessing(SaveRegistrationForProcessingInput input)
        {
            int tenantId = AbpSession.GetTenantId();
            if (input == null || input.RegistrationId <= 0)
            {
                return false;
            }

            var registration = await _registrationRepository.GetAsync(input.RegistrationId);
            if (registration == null)
            {
                return false;
            }
            var originalRegistrationStatusId = registration.RegistrationStatusId;

            var purchaseRegistrations = await _lookup_purchaseRegistrationRepository.GetAll()
                .Where(p => p.RegistrationId == input.RegistrationId)
                .ToListAsync();

            var rejectedFields = new List<string>();
            foreach (var field in input.FormFields)
            {
                if (field.FieldId <= 0)
                {
                    //we are dealing with a sub-field (Quantity/TotalAmount/PurchaseDate/...) of a special Product field (ProductPremiumQuantity/PurchaseRegistration/PurchaseRegistrationLite/PurchaseRegistrationSerial)
                    field.FieldId = input.FormFields.First(f => f.PurchaseRegistrationField == "ProductId").FieldId;
                }

                if (field.IsRejected)
                {
                    var formFieldRecord = await _lookup_formFieldRepository.GetAsync(field.FieldId);
                    if (formFieldRecord == null)
                    {
                        break;
                    }

                    var fieldName = formFieldRecord.FieldName;
                    if (!rejectedFields.Contains(fieldName))
                    {
                        rejectedFields.Add(fieldName);
                    }
                }

                switch (field.FieldSource)
                {
                    case FieldSourceHelper.Registration:
                        var registrationField = typeof(Registration).GetProperty(field.RegistrationField);
                        var registrationFieldType = registrationField.PropertyType;

                        object fieldValue = FormFieldHelper.FormatDBFormField(field.FieldValue, registrationFieldType);

                        if (field.FieldType != FieldTypeHelper.FileUploader &&
                            field.FieldType != FieldTypeHelper.IbanChecker) // For now, FileUploader and IBAN/BIC remain read-only.
                        {
                            registrationField.SetValue(registration, fieldValue);
                        }

                        break;
                    case FieldSourceHelper.PurchaseRegistration:
                        var purchaseRegistration = purchaseRegistrations.FirstOrDefault(x => x.Id == field.FieldLineId);
                        if (purchaseRegistration == null)
                        {
                            break;
                        }

                        var purchaseRegistrationField = typeof(PurchaseRegistration).GetProperty(field.PurchaseRegistrationField);
                        var purchaseRegistrationFieldType = purchaseRegistrationField.PropertyType;

                        object purchaseFieldValue = FormFieldHelper.FormatDBFormField(field.FieldValue, purchaseRegistrationFieldType);

                        //@MAKITA ONLY: any possible CHANGE of PurchasedProduct by ServiceCenter must be IGNORED!!!
                        if (tenantId != TenantHelper.MakitaLive && tenantId != TenantHelper.MakitaTest && field.PurchaseRegistrationField == "ProductId" && purchaseRegistration.ProductId != Convert.ToInt64(purchaseFieldValue))
                        {
                            //PurchasedProduct has been changed by ServiceCenter -> re-relate HandlingLine accordingly
                            var productHandling = await _productHandlingRepository.GetAll().FirstOrDefaultAsync(x => x.CampaignId == registration.CampaignId);
                            var handlingLines = await _lookup_handlingLineRepository.GetAllListAsync(x => x.ProductHandlingId == productHandling.Id);
                            var handlingLineIds = handlingLines.Select(x => x.Id).ToList();
                            var handlingLineProduct = await _lookup_handlingLineProductRepository.GetAll().Where(x => handlingLineIds.Contains(x.HandlingLineId) && x.ProductId == Convert.ToInt64(purchaseFieldValue)).FirstOrDefaultAsync();

                            if (handlingLineProduct != null)
                            {
                                purchaseRegistration.HandlingLineId = handlingLineProduct.HandlingLineId;
                            }
                            else
                            {
                                if (handlingLines.Where(l => l.CustomerCode.ToUpper().Trim() != "UNKNOWN").Count() == 1)
                                {
                                    //only 1 Premium is linked to this campaign, so take that one
                                    purchaseRegistration.HandlingLineId = handlingLines.Where(l => l.CustomerCode.ToUpper().Trim() != "UNKNOWN").First().Id;
                                }
                                else
                                {
                                    //more than 1 Premiums are linked to this campaign, so link it to the UNKNOWN
                                    purchaseRegistration.HandlingLineId = handlingLines.Where(l => l.CustomerCode.ToUpper().Trim() == "UNKNOWN").First().Id;
                                }
                            }
                        }

                        if (field.FieldType != FieldTypeHelper.FileUploader)
                        {
                            typeof(PurchaseRegistration).GetProperty(field.PurchaseRegistrationField).SetValue(purchaseRegistration, purchaseFieldValue);
                        }

                        break;
                    case FieldSourceHelper.CustomRegistration:
                        var RegistrationFieldData = _lookup_registrationFieldDataRepository.GetAll().Where(f => f.RegistrationId == registration.Id);
                        var customRegistrationFieldValue = await (from o in RegistrationFieldData
                                                                  join o1 in _lookup_RegistrationFieldRepository.GetAll() on o.RegistrationFieldId equals o1.Id
                                                                  where o1.FormFieldId == field.FieldId
                                                                  select o).FirstOrDefaultAsync();

                        if (customRegistrationFieldValue == null)
                        {
                            customRegistrationFieldValue = await (from o in RegistrationFieldData
                                                                  join o1 in _lookup_RegistrationFieldRepository.GetAll() on o.RegistrationFieldId equals o1.Id
                                                                  where o1.FormFieldId == field.FallbackFieldId
                                                                  select o).FirstOrDefaultAsync();
                        }

                        if (field.FieldType != FieldTypeHelper.FileUploader)
                        {
                            customRegistrationFieldValue.Value = field.FieldValue;
                        }

                        await _lookup_registrationFieldDataRepository.UpdateAsync(customRegistrationFieldValue);

                        break;
                    case FieldSourceHelper.CustomPurchaseRegistration:
                        var purchaseRegistrationFieldData = _lookup_purchaseRegistrationFieldDataRepository.GetAll().Where(p => p.PurchaseRegistrationId == field.FieldLineId);
                        var customPurchaseRegistrationFieldValue = await (from o in purchaseRegistrationFieldData
                                                                          join o1 in _lookup_purchaseRegistrationFieldRepository.GetAll()
                                                                          on o.PurchaseRegistrationFieldId equals o1.Id
                                                                          where o1.FormFieldId == field.FieldId
                                                                          select o).FirstOrDefaultAsync();

                        if (customPurchaseRegistrationFieldValue == null)
                        {
                            customPurchaseRegistrationFieldValue = await (from o in purchaseRegistrationFieldData
                                                                          join o1 in _lookup_purchaseRegistrationFieldRepository.GetAll()
                                                                          on o.PurchaseRegistrationFieldId equals o1.Id
                                                                          where o1.FormFieldId == field.FallbackFieldId
                                                                          select o).FirstOrDefaultAsync();
                        }

                        if (field.FieldType != FieldTypeHelper.FileUploader)
                        {
                            customPurchaseRegistrationFieldValue.Value = field.FieldValue;
                        }

                        await _lookup_purchaseRegistrationFieldDataRepository.UpdateAsync(customPurchaseRegistrationFieldValue);

                        break;
                    default:
                        continue;
                }
            }

            foreach (var purchaseRegistration in purchaseRegistrations)
            {
                await _lookup_purchaseRegistrationRepository.UpdateAsync(purchaseRegistration);
            }

            var registrationStatusPending = await _registrationStatusesAppService.GetByStatusCode(RegistrationStatusCodeHelper.Pending);
            var registrationStatusAccepted = await _registrationStatusesAppService.GetByStatusCode(RegistrationStatusCodeHelper.Accepted);
            var registrationStatusSent = await _registrationStatusesAppService.GetByStatusCode(RegistrationStatusCodeHelper.Send);

            registration.IncompleteFields = String.Empty;
            registration.RejectedFields = String.Empty;
            registration.Password = null;

            if (!rejectedFields.Any())
            {
                if (input.IsApproved)
                {
                    registration.RegistrationStatusId = registrationStatusAccepted.RegistrationStatus.Id;

                    //if there is a POINTS handling line AND this registration is fully linked to it, then put the registration on SENT immediately
                    var productHandling = await _lookup_productHandlingRepository.GetAll().Where(h => h.CampaignId == registration.CampaignId).FirstAsync();
                    var pointsHandlingLine = await _lookup_handlingLineRepository.GetAll().Where(h => h.ProductHandlingId == productHandling.Id && h.CustomerCode.ToUpper().Trim() == "POINTS").FirstOrDefaultAsync();

                    if (pointsHandlingLine != null)
                    {
                        if (purchaseRegistrations.All(p => p.HandlingLineId == pointsHandlingLine.Id))
                        {
                            registration.RegistrationStatusId = registrationStatusSent.RegistrationStatus.Id;
                        }
                    }
                }
                else
                {
                    var currentRegistrationStatus = await _lookup_registrationStatusRepository.GetAsync(registration.RegistrationStatusId);
                    var currentStatusCode = currentRegistrationStatus?.StatusCode;

                    if (currentStatusCode == RegistrationStatusCodeHelper.Accepted ||
                        currentStatusCode == RegistrationStatusCodeHelper.Rejected ||
                        currentStatusCode == RegistrationStatusCodeHelper.Incomplete)
                    {
                        // Reset to pending
                        registration.RegistrationStatusId = registrationStatusPending.RegistrationStatus.Id;
                    }
                }
            }
            else
            {
                var registrationStatus =
                   (input.SelectedIncompleteReasonId != -1) ?
                       await _registrationStatusesAppService.GetByStatusCode(RegistrationStatusCodeHelper.Incomplete) :
                       await _registrationStatusesAppService.GetByStatusCode(RegistrationStatusCodeHelper.Rejected);

                registration.RegistrationStatusId = registrationStatus.RegistrationStatus.Id;

                var rejectionFields = string.Join(",", rejectedFields);

                switch (registrationStatus.RegistrationStatus.StatusCode)
                {
                    case RegistrationStatusCodeHelper.Incomplete:
                        registration.IncompleteFields = rejectionFields;
                        // Add password
                        registration.Password = PasswordGenerator.GeneratePassword(8, new Random());
                        break;
                    case RegistrationStatusCodeHelper.Rejected:
                        registration.RejectedFields = rejectionFields;
                        break;
                }
            }

            registration.RejectionReasonId =
                (input.SelectedIncompleteReasonId != -1) ? input.SelectedIncompleteReasonId : (input.SelectedRejectionReasonId != -1) ? input.SelectedRejectionReasonId : (long?)null;

            await _registrationRepository.UpdateAsync(registration);

            DateTime nowUtc = DateTime.UtcNow;
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            DateTime westTime = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, cstZone);

            // If we have gone from Approved to Sent immediately (POINTS registration), make sure that both stati are logged in the history
            if (registration.RegistrationStatusId == registrationStatusSent.RegistrationStatus.Id)
            {
                await _registrationHistoriesAppService.CreateNew(new RegistrationHistory.Dtos.CreateOrEditRegistrationHistoryDto
                {
                    RegistrationId = registration.Id,
                    RegistrationStatusId = registrationStatusAccepted.RegistrationStatus.Id,
                    DateCreated = westTime,
                    Remarks = String.Empty,
                    AbpUserId = AbpSession.UserId ?? 1
                });
            }

            // Create RegistrationHistory record
            await _registrationHistoriesAppService.CreateNew(new RegistrationHistory.Dtos.CreateOrEditRegistrationHistoryDto
            {
                RegistrationId = registration.Id,
                RegistrationStatusId = registration.RegistrationStatusId,
                DateCreated = westTime,
                Remarks = String.Empty,
                AbpUserId = AbpSession.UserId ?? 1
            });

            //MAKITA API CALL
            if ((tenantId == TenantHelper.MakitaLive || tenantId == TenantHelper.MakitaTest) && (registration.RegistrationStatusId == registrationStatusAccepted.RegistrationStatus.Id || registration.RegistrationStatusId == registrationStatusSent.RegistrationStatus.Id) && originalRegistrationStatusId != registrationStatusSent.RegistrationStatus.Id)
            {
                try
                {
                    await _makitaApiAppService.MakitaRegistrationApproved(registration.Id);
                }
                catch (Exception ex)
                {

                    return true;
                }
            };

            return true;
        }

        public async Task<GetRegistrationForProcessingOutput> GetRegistrationForProcessing(EntityDto<long> input)
        {
            var userPermissions = await _userAppService.GetUserPermissionsForEdit(new EntityDto<long> { Id = (long)AbpSession.UserId });

            var registration = (from o in _registrationRepository.GetAll()
                                join o1 in _lookup_campaignRepository.GetAll() on o.CampaignId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()
                                join o2 in _lookup_campaignFormRepository.GetAll() on o.CampaignFormId equals o2.Id into j2
                                from s2 in j2.DefaultIfEmpty()
                                join o3 in _lookup_registrationStatusRepository.GetAll() on o.RegistrationStatusId equals o3.Id into j3
                                from s3 in j3.DefaultIfEmpty()
                                join o4 in _lookup_countryRepository.GetAll() on o.CountryId equals o4.Id into j4
                                from s4 in j4.DefaultIfEmpty()
                                where o.Id == input.Id
                                select new
                                {
                                    Id = o.Id,
                                    LocaleId = o.LocaleId,
                                    CountryId = o.CountryId,
                                    CampaignId = o.CampaignId,
                                    FormId = s2.FormId,
                                    CampaignFormId = o.CampaignFormId,
                                    RejectionReasonId = o.RejectionReasonId,
                                    FirstName = o.FirstName,
                                    LastName = o.LastName,
                                    EmailAddress = o.EmailAddress,
                                    Country = s4.Description,
                                    IncompleteFields = o.IncompleteFields,
                                    RejectedFields = o.RejectedFields,
                                    Bic = o.Bic,
                                    Iban = o.Iban,
                                    StatusCode = s3.StatusCode,
                                    CampaignName = s1.Name,
                                    CampaignStartDate = s1.StartDate,
                                    CampaignEndDate = s1.EndDate
                                }).First();

            var purchaseRegistrations = (from o in _lookup_purchaseRegistrationRepository.GetAll()
                                         join o1 in _lookup_handlingLineRepository.GetAll() on o.HandlingLineId equals o1.Id into j1
                                         from s1 in j1.DefaultIfEmpty()
                                         join o2 in _campaignTypeRepository.GetAll() on s1.CampaignTypeId equals o2.Id into j2
                                         from s2 in j2.DefaultIfEmpty()
                                         where o.RegistrationId == registration.Id
                                         select new
                                         {
                                             Id = o.Id,
                                             ProductId = o.ProductId,
                                             RetailerLocationId = o.RetailerLocationId,
                                             Quantity = o.Quantity,
                                             TotalAmount = o.TotalAmount,
                                             PurchaseDate = o.PurchaseDate,
                                             InvoiceImagePath = o.InvoiceImagePath,
                                             CampaignType = s2.Code,
                                             HLQuantity = s1.Quantity,
                                             HLAmount = s1.Amount,
                                             HLFixed = s1.Fixed,
                                             HLPercentage = s1.Percentage,
                                             HLActivationCode = s1.ActivationCode,
                                             CustomerCode = s1.CustomerCode,
                                             PremiumDescription = s1.PremiumDescription
                                         }).ToList();

            var products = await _productsAppService.GetAllProductsForCampaign(registration.CampaignId);
            var retailerLocations = await _retailersAppService.GetAllRetailersForCampaign(registration.CampaignId);
            var campaignCountries = await _lookup_campaignCountryRepository.GetAllListAsync(c => c.CampaignId == registration.CampaignId);

            var formLocale = await _lookup_formLocaleRepository.GetAll().FirstOrDefaultAsync(f => f.FormId == registration.FormId && f.LocaleId == registration.LocaleId);
            var formBlocks = await _lookup_formBlockRepository.GetAll().Where(f => f.FormLocaleId == formLocale.Id).OrderBy(f => f.SortOrder).ToListAsync();
            var uiFormBlocks = new List<Dtos.ProcessRegistration.FormBlock>();

            var registrationObject = await _registrationRepository.GetAsync(registration.Id); //exclusively needed for mapping purposes
            var messageHistory = await _lookup_messageHistoryRepository.GetAll().Where(h => h.RegistrationId == registration.Id).OrderBy(h => h.TimeStamp).ThenBy(h => h.Id).ToListAsync();
            var messageStatus = _messagingAppService.getMessageStatusList(messageHistory.Select(h => h.MessageId).ToList());

            var availableIncompleteReasons = await _rejectionReasonsAppService.GetAllForIncomplete();
            var availableRejectionReasons = await _rejectionReasonsAppService.GetAllForRejection();

            bool canOnlyEditRemarks = false;
            bool countryIsPresentInForm = false;

            bool isPremium = false;
            bool isCashRefund = false;
            bool isActivationCode = false;

            switch (purchaseRegistrations[0].CampaignType)
            {
                case CampaignTypeHelper.Premium:
                    isPremium = true;
                    break;
                case CampaignTypeHelper.CashRefund:
                    isCashRefund = true;
                    break;
                case CampaignTypeHelper.ActivationCode:
                    isActivationCode = true;
                    break;
            }

            if (registration.StatusCode == RegistrationStatusCodeHelper.Accepted ||
                registration.StatusCode == RegistrationStatusCodeHelper.Rejected)
            {
                canOnlyEditRemarks = !userPermissions.GrantedPermissionNames.Contains(AppPermissions.Pages_Registrations_EditAll);
            }
            else if (registration.StatusCode == RegistrationStatusCodeHelper.InProgress ||
                     registration.StatusCode == RegistrationStatusCodeHelper.Send)
            {
                canOnlyEditRemarks = true;
            }

            var rejectionFields = new List<string>();

            if (!String.IsNullOrWhiteSpace(registration.IncompleteFields))
            {
                rejectionFields.AddRange(registration.IncompleteFields.Split(','));
            }
            else if (!String.IsNullOrWhiteSpace(registration.RejectedFields))
            {
                rejectionFields.AddRange(registration.RejectedFields.Split(','));
            }

            foreach (var formBlock in formBlocks)
            {
                var formBlockFields = _lookup_formBlockFieldRepository.GetAll().Where(f => f.FormBlockId == formBlock.Id);
                var formBlockFieldsWithDetails = await (from o in formBlockFields
                                                        join o1 in _lookup_formFieldRepository.GetAll() on o.FormFieldId equals o1.Id into j1
                                                        from s1 in j1.DefaultIfEmpty()
                                                        join o2 in _lookup_fieldTypeRepository.GetAll() on s1.FieldTypeId equals o2.Id into j2
                                                        from s2 in j2.DefaultIfEmpty()
                                                        orderby o.SortOrder
                                                        select new Dtos.ProcessRegistration.FormField
                                                        {
                                                            FieldId = s1.Id,
                                                            FieldTypeId = s1.FieldTypeId,
                                                            FieldType = s2.Description,
                                                            FieldLabel = s1.Label,
                                                            FieldName = s1.FieldName,
                                                            RegistrationField = s1.RegistrationField,
                                                            PurchaseRegistrationField = s1.PurchaseRegistrationField,
                                                            IsRejected = rejectionFields.Contains(s1.FieldName)
                                                        }).ToListAsync();

                //its type is unknown as of yet
                var uiFormFields = new List<Dtos.ProcessRegistration.FormField>();
                var uiFormFieldCollectionLines = new List<FormFieldCollectionLine>();

                //initialize the uiFormFieldCollectionLines
                foreach (var purchaseRegistration in purchaseRegistrations)
                {
                    var chosenProduct = products.Items.FirstOrDefault(x => x.Product.Id == purchaseRegistration.ProductId);

                    uiFormFieldCollectionLines.Add(new FormFieldCollectionLine
                    {
                        PurchaseRegId = purchaseRegistration.Id,
                        Title = chosenProduct?.Product?.Description,
                        SubTitle = chosenProduct?.Product?.ProductCode,
                        FormFields = new List<Dtos.ProcessRegistration.FormField>()
                    });
                }

                foreach (var formBlockField in formBlockFieldsWithDetails)
                {
                    //are there ListValues for this FormField?
                    var formFieldListValuesCollection = new List<FormFieldListValue>();
                    var formFieldValueList = await _lookup_formFieldValueListRepository.GetAll().Where(f => f.FormFieldId == formBlockField.FieldId).FirstOrDefaultAsync();

                    if (formFieldValueList != null)
                    {
                        var formFieldListValues = _lookup_listValueRepository.GetAll().Where(f => f.ValueListId == formFieldValueList.ValueListId);
                        var formFieldListValuesWithTranslations = await (from o in formFieldListValues
                                                                         join o1 in _lookup_listValueTranslationRepository.GetAll() on o.Id equals o1.ListValueId into j1
                                                                         from s1 in j1.DefaultIfEmpty()
                                                                         where s1.LocaleId == registration.LocaleId
                                                                         select new FormFieldListValue
                                                                         {
                                                                             KeyValue = o.KeyValue,
                                                                             Description = s1.Description
                                                                         }).ToListAsync();

                        foreach (var listValue in formFieldListValues)
                        {
                            var translatedListValue = formFieldListValuesWithTranslations.FirstOrDefault(f => f.KeyValue == listValue.KeyValue);

                            if (translatedListValue != null)
                            {
                                formFieldListValuesCollection.Add(translatedListValue);
                            }
                            else
                            {
                                formFieldListValuesCollection.Add(new FormFieldListValue() { KeyValue = listValue.KeyValue, Description = listValue.Description });
                            }
                        }
                    }

                    formBlockField.FieldListValues = formFieldListValuesCollection;
                    formBlockField.IsReadOnly = formBlockField.FieldType != FieldTypeHelper.Remark && canOnlyEditRemarks;

                    //Go...
                    if (!String.IsNullOrWhiteSpace(formBlockField.RegistrationField))
                    {
                        var fieldValue = typeof(Registration).GetProperty(formBlockField.RegistrationField).GetValue(registrationObject, null);
                        var fieldValueFormatted = String.Empty;
                        if (fieldValue != null)
                        {
                            fieldValueFormatted = FormFieldHelper.FormatUIFormField(fieldValue, formBlockField.FieldType);
                        }

                        formBlockField.FieldSource = FieldSourceHelper.Registration;
                        formBlockField.FieldLineId = null;
                        formBlockField.FieldValue = fieldValueFormatted;

                        uiFormFields.Add(formBlockField);
                    }
                    else if (!String.IsNullOrWhiteSpace(formBlockField.PurchaseRegistrationField))
                    {
                        foreach (var purchaseRegistration in purchaseRegistrations)
                        {
                            var purchaseRegObject = await _lookup_purchaseRegistrationRepository.GetAsync(purchaseRegistration.Id);
                            var fieldValue = typeof(PurchaseRegistration).GetProperty(formBlockField.PurchaseRegistrationField).GetValue(purchaseRegObject, null);
                            var fieldValueFormatted = String.Empty;
                            if (fieldValue != null)
                            {
                                fieldValueFormatted = FormFieldHelper.FormatUIFormField(fieldValue, formBlockField.FieldType);
                            }

                            var formBlockFieldClone = FormFieldHelper.CloneUIFormField(formBlockField);

                            //restore FieldListValues
                            formBlockFieldClone.FieldListValues = formBlockField.FieldListValues;

                            formBlockFieldClone.FieldSource = FieldSourceHelper.PurchaseRegistration;
                            formBlockFieldClone.FieldLineId = purchaseRegistration.Id;
                            formBlockFieldClone.FieldValue = fieldValueFormatted;

                            //uiFormFieldCollectionLines[purchaseRegistrations.IndexOf(purchaseRegistration)].FormFields.Add(formBlockFieldClone); DOES NOT WORK RIGHT!!!
                            uiFormFieldCollectionLines.Where(x => x.PurchaseRegId == purchaseRegistration.Id).First().FormFields.Add(formBlockFieldClone);
                        }
                    }
                    else if ((formBlockField.FieldType == FieldTypeHelper.DropdownMenu && formBlockField.FieldName == FieldNameHelper.ProductPremium)
                           || formBlockField.FieldType == FieldSourceHelper.Product
                           || formBlockField.FieldType == FieldSourceHelper.ProductPremiumLite
                           || formBlockField.FieldType == FieldSourceHelper.ProductPremiumQuantity 
                           || formBlockField.FieldType == FieldSourceHelper.PurchaseRegistration 
                           || formBlockField.FieldType == FieldSourceHelper.PurchaseRegistrationLite 
                           || formBlockField.FieldType == FieldSourceHelper.PurchaseRegistrationSerial)
                    {
                        var formFieldListValuesCollectionProduct = new List<FormFieldListValue>();

                        foreach (var product in products?.Items)
                        {
                            formFieldListValuesCollectionProduct.Add(new FormFieldListValue()
                            {
                                KeyValue = product.Product?.Id.ToString(),
                                Description = $"{product.Product?.ProductCode} ({product.Product?.Description})"
                            });
                        }

                        var formFieldListValuesCollectionRetailer = retailerLocations.Items.Select(q => new FormFieldListValue
                        {
                            KeyValue = q.RetailerLocation.Id.ToString(),
                            Description = String.IsNullOrWhiteSpace(q.RetailerLocation.PostalCode) ? q.RetailerLocation.Name : $"{q.RetailerLocation.Name} ({q.RetailerLocation.PostalCode})"
                        }).OrderBy(q => q.Description).ToList();

                        foreach (var purchaseRegistration in purchaseRegistrations)
                        {
                            var formBlockFieldClone = FormFieldHelper.CloneUIFormField(formBlockField);
                            formBlockFieldClone.FieldType = FieldTypeHelper.DropdownMenu;
                            formBlockFieldClone.FieldListValues = formFieldListValuesCollectionProduct;
                            formBlockFieldClone.FieldSource = FieldSourceHelper.PurchaseRegistration;
                            formBlockFieldClone.FieldLineId = purchaseRegistration.Id;
                            formBlockFieldClone.RegistrationField = String.Empty;
                            formBlockFieldClone.PurchaseRegistrationField = "ProductId";
                            formBlockFieldClone.FieldValue = purchaseRegistration.ProductId.ToString();

                            uiFormFieldCollectionLines.Where(x => x.PurchaseRegId == purchaseRegistration.Id).First().FormFields.Add(formBlockFieldClone);

                            if (formBlockField.FieldName == FieldNameHelper.ProductPremiumQuantity
                             || formBlockField.FieldName == FieldNameHelper.PurchaseRegistration
                             || formBlockField.FieldName == FieldNameHelper.PurchaseRegistrationLite
                             || formBlockField.FieldName == FieldNameHelper.PurchaseRegistrationSerial)
                            {
                                var formBlockField_Quantity = FormFieldHelper.CloneUIFormField(formBlockFieldClone);

                                formBlockField_Quantity.FieldId = 0;
                                formBlockField_Quantity.FieldLabel = "Quantity";
                                formBlockField_Quantity.FieldType = FieldTypeHelper.InputNumber;
                                formBlockField_Quantity.PurchaseRegistrationField = "Quantity";
                                formBlockField_Quantity.FieldValue = purchaseRegistration.Quantity.ToString();

                                uiFormFieldCollectionLines.Where(x => x.PurchaseRegId == purchaseRegistration.Id).First().FormFields.Add(formBlockField_Quantity);

                                if (formBlockField.FieldName == FieldNameHelper.PurchaseRegistration
                                 || formBlockField.FieldName == FieldNameHelper.PurchaseRegistrationLite
                                 || formBlockField.FieldName == FieldNameHelper.PurchaseRegistrationSerial)
                                {
                                    if (formBlockField.FieldName == FieldNameHelper.PurchaseRegistration)
                                    {
                                        var formBlockField_TotalAmount = FormFieldHelper.CloneUIFormField(formBlockFieldClone);

                                        formBlockField_TotalAmount.FieldId = -1;
                                        formBlockField_TotalAmount.FieldLabel = "Total amount";
                                        formBlockField_TotalAmount.FieldType = FieldTypeHelper.InputNumber;
                                        formBlockField_TotalAmount.PurchaseRegistrationField = "TotalAmount";
                                        formBlockField_TotalAmount.FieldValue = purchaseRegistration.TotalAmount.ToString().Replace(',', '.');

                                        uiFormFieldCollectionLines.Where(x => x.PurchaseRegId == purchaseRegistration.Id).First().FormFields.Add(formBlockField_TotalAmount);
                                    }

                                    var formBlockField_PurchaseDate = FormFieldHelper.CloneUIFormField(formBlockFieldClone);

                                    formBlockField_PurchaseDate.FieldId = -2;
                                    formBlockField_PurchaseDate.FieldLabel = "Purchase date";
                                    formBlockField_PurchaseDate.FieldType = FieldTypeHelper.DatePicker;
                                    formBlockField_PurchaseDate.PurchaseRegistrationField = "PurchaseDate";
                                    formBlockField_PurchaseDate.FieldValue = purchaseRegistration.PurchaseDate.ToString("yyyy-MM-dd");

                                    uiFormFieldCollectionLines.Where(x => x.PurchaseRegId == purchaseRegistration.Id).First().FormFields.Add(formBlockField_PurchaseDate);

                                    var formBlockField_RetailerLocation = FormFieldHelper.CloneUIFormField(formBlockFieldClone);

                                    formBlockField_RetailerLocation.FieldId = -3;
                                    formBlockField_RetailerLocation.FieldLabel = "Retailer";
                                    formBlockField_RetailerLocation.FieldType = FieldTypeHelper.DropdownMenu;
                                    formBlockField_RetailerLocation.FieldListValues = formFieldListValuesCollectionRetailer;
                                    formBlockField_RetailerLocation.PurchaseRegistrationField = "RetailerLocationId";
                                    formBlockField_RetailerLocation.FieldValue = purchaseRegistration.RetailerLocationId.ToString();

                                    uiFormFieldCollectionLines.Where(x => x.PurchaseRegId == purchaseRegistration.Id).First().FormFields.Add(formBlockField_RetailerLocation);

                                    var formBlockField_InvoiceImage = FormFieldHelper.CloneUIFormField(formBlockFieldClone);

                                    formBlockField_InvoiceImage.FieldId = -4;
                                    formBlockField_InvoiceImage.FieldLabel = "Invoice";
                                    formBlockField_InvoiceImage.FieldType = FieldTypeHelper.FileUploader;
                                    formBlockField_InvoiceImage.PurchaseRegistrationField = "InvoiceImagePath";
                                    formBlockField_InvoiceImage.FieldValue = purchaseRegistration.InvoiceImagePath.ToString();

                                    uiFormFieldCollectionLines.Where(x => x.PurchaseRegId == purchaseRegistration.Id).First().FormFields.Add(formBlockField_InvoiceImage);

                                    if (formBlockField.FieldName == FieldNameHelper.PurchaseRegistrationSerial)
                                    {
                                        var purchaseRegistrationFieldData = _lookup_purchaseRegistrationFieldDataRepository.GetAll().Where(f => f.PurchaseRegistrationId == purchaseRegistration.Id);
                                        var customSerialRelatedFields = (from o in purchaseRegistrationFieldData
                                                                         join o1 in _lookup_purchaseRegistrationFieldRepository.GetAll() on o.PurchaseRegistrationFieldId equals o1.Id
                                                                         join o2 in _lookup_formFieldRepository.GetAll() on o1.FormFieldId equals o2.Id
                                                                         where o2.FieldName == FieldNameHelper.SerialNumber || o2.FieldName == FieldNameHelper.SerialCodeImage
                                                                         select new
                                                                         {
                                                                             FieldId = o1.FormFieldId,
                                                                             FieldName = o2.FieldName,
                                                                             FieldValue = o.Value
                                                                         }).ToList();

                                        var customSerialNumberField = customSerialRelatedFields.Where(x => x.FieldName == FieldNameHelper.SerialNumber).First();
                                        var customSerialImageField = customSerialRelatedFields.Where(x => x.FieldName == FieldNameHelper.SerialCodeImage).First();

                                        var formBlockField_SerialNumber = FormFieldHelper.CloneUIFormField(formBlockFieldClone);

                                        formBlockField_SerialNumber.FieldId = -5;
                                        formBlockField_SerialNumber.FieldLabel = "Serial number";
                                        formBlockField_SerialNumber.FieldType = FieldTypeHelper.InputText;
                                        formBlockField_SerialNumber.PurchaseRegistrationField = String.Empty;
                                        formBlockField_SerialNumber.FieldSource = FieldSourceHelper.CustomPurchaseRegistration;
                                        formBlockField_SerialNumber.FallbackFieldId = customSerialNumberField.FieldId;
                                        formBlockField_SerialNumber.FieldValue = customSerialNumberField.FieldValue;

                                        uiFormFieldCollectionLines.Where(x => x.PurchaseRegId == purchaseRegistration.Id).First().FormFields.Add(formBlockField_SerialNumber);

                                        var formBlockField_SerialImage = FormFieldHelper.CloneUIFormField(formBlockFieldClone);

                                        formBlockField_SerialImage.FieldId = -6;
                                        formBlockField_SerialImage.FieldLabel = "Serial image";
                                        formBlockField_SerialImage.FieldType = FieldTypeHelper.FileUploader;
                                        formBlockField_SerialImage.PurchaseRegistrationField = String.Empty;
                                        formBlockField_SerialImage.FieldSource = FieldSourceHelper.CustomPurchaseRegistration;
                                        formBlockField_SerialImage.FallbackFieldId = customSerialImageField.FieldId;
                                        formBlockField_SerialImage.FieldValue = customSerialImageField.FieldValue;

                                        uiFormFieldCollectionLines.Where(x => x.PurchaseRegId == purchaseRegistration.Id).First().FormFields.Add(formBlockField_SerialImage);
                                    }
                                }
                            }

                            //show what this customer will receive in a Literal field...
                            var customer = !String.IsNullOrWhiteSpace(registration.FirstName) || !String.IsNullOrWhiteSpace(registration.LastName) ? $"{registration.FirstName} {registration.LastName}" : "deze klant";
                            var receiveInfo = $"Indien goedgekeurd ontvangt {customer} ";
                            int quantity = 1;

                            //are we dealing with a Gift...?
                            ProductGift gift = null;
                            var formFieldGift = await _lookup_formFieldRepository.GetAll().Where(f => f.FieldName == FieldNameHelper.GiftId).FirstOrDefaultAsync();
                            if (formFieldGift != null)
                            {
                                var purchaseRegistrationFieldData = _lookup_purchaseRegistrationFieldDataRepository.GetAll().Where(f => f.PurchaseRegistrationId == purchaseRegistration.Id);
                                var customPurchaseRegistrationField = (from o in purchaseRegistrationFieldData
                                                                       join o1 in _lookup_purchaseRegistrationFieldRepository.GetAll() on o.PurchaseRegistrationFieldId equals o1.Id
                                                                       where o1.FormFieldId == formFieldGift.Id
                                                                       select o).FirstOrDefault();

                                long giftId;
                                if (customPurchaseRegistrationField != null && long.TryParse(customPurchaseRegistrationField.Value, out giftId))
                                {
                                    var product = await _lookup_productRepository.GetAsync(purchaseRegistration.ProductId);

                                    gift = await _lookup_productGiftRepository.GetAll().Where(g => g.GiftId == giftId
                                                                                                && g.CampaignId == registration.CampaignId
                                                                                                && g.ProductCode.ToUpper().Trim() == product.ProductCode.ToUpper().Trim()).FirstOrDefaultAsync();
                                }
                            }
                            if (gift != null)
                            {
                                //we are dealing with a Gift, so display it...
                                receiveInfo += gift.TotalPoints > 0 ? $"{gift.TotalPoints} CADEAUPUNTEN" : $"een {gift.GiftName.ToUpper().Trim()}";
                            }
                            else
                            {
                                //we are not dealing with a Gift, apply the regular display...
                                if ((!String.IsNullOrWhiteSpace(purchaseRegistration.CustomerCode) && purchaseRegistration.CustomerCode.ToUpper().Trim() != "POINTS" && purchaseRegistration.CustomerCode.ToUpper().Trim() != "UNKNOWN")
                                   || purchaseRegistration.HLAmount.HasValue
                                   || purchaseRegistration.HLActivationCode)
                                {
                                    if (isPremium)
                                    {
                                        quantity = purchaseRegistration.HLFixed ? purchaseRegistration.HLQuantity.Value : purchaseRegistration.HLQuantity.Value * purchaseRegistration.Quantity;
                                        var premium = !String.IsNullOrWhiteSpace(purchaseRegistration.PremiumDescription) ? purchaseRegistration.PremiumDescription.ToUpper() : purchaseRegistration.CustomerCode;
                                        var tag = quantity == 1 ? "stuk" : "stuks";
                                        receiveInfo += $"{quantity} {tag} van de premium {premium}";
                                    }
                                    else if (isCashRefund)
                                    {
                                        var refund = purchaseRegistration.HLPercentage ? Math.Round(purchaseRegistration.TotalAmount * (purchaseRegistration.HLAmount.Value / 100), 2) : purchaseRegistration.HLFixed ? Math.Round(purchaseRegistration.HLAmount.Value, 2) : Math.Round(purchaseRegistration.HLAmount.Value * purchaseRegistration.Quantity, 2);
                                        receiveInfo += $"€ {refund}";
                                    }
                                    else if (isActivationCode)
                                    {
                                        quantity = purchaseRegistration.HLFixed ? 1 : purchaseRegistrations.Count();
                                        var tag = quantity == 1 ? "activatiecode" : "activatiecodes";
                                        receiveInfo += $"{quantity} {tag.ToUpper()}";
                                    }
                                }
                                else
                                {
                                    receiveInfo += "niets. Afhandeling onbekend.";
                                }
                            }

                            var formBlockField_ReceiveInfo = FormFieldHelper.CloneUIFormField(formBlockFieldClone);

                            formBlockField_ReceiveInfo.FieldId = -666;
                            formBlockField_ReceiveInfo.FieldLabel = String.Empty;
                            formBlockField_ReceiveInfo.FieldType = FieldTypeHelper.Literal;
                            formBlockField_ReceiveInfo.PurchaseRegistrationField = String.Empty;
                            formBlockField_ReceiveInfo.FieldValue = receiveInfo;

                            uiFormFieldCollectionLines.Where(x => x.PurchaseRegId == purchaseRegistration.Id).First().FormFields.Add(formBlockField_ReceiveInfo);
                        }
                    }
                    else if (formBlockField.FieldType == FieldSourceHelper.RetailerLocation
                          || formBlockField.FieldType == FieldSourceHelper.RetailerRadioButton)
                    {
                        formFieldListValuesCollection.Clear();
                        formFieldListValuesCollection = retailerLocations.Items.Select(q => new FormFieldListValue
                        {
                            KeyValue = q.RetailerLocation.Id.ToString(),
                            Description = String.IsNullOrWhiteSpace(q.RetailerLocation.PostalCode) ? q.RetailerLocation.Name : $"{q.RetailerLocation.Name} ({q.RetailerLocation.PostalCode})"
                        }).OrderBy(q => q.Description).ToList();

                        foreach (var purchaseRegistration in purchaseRegistrations)
                        {
                            var formBlockFieldClone = FormFieldHelper.CloneUIFormField(formBlockField);
                            formBlockFieldClone.FieldType = FieldTypeHelper.DropdownMenu;
                            formBlockFieldClone.FieldListValues = formFieldListValuesCollection;
                            formBlockFieldClone.FieldSource = FieldSourceHelper.PurchaseRegistration;
                            formBlockFieldClone.FieldLineId = purchaseRegistration.Id;
                            formBlockFieldClone.RegistrationField = String.Empty;
                            formBlockFieldClone.PurchaseRegistrationField = "RetailerLocationId";
                            formBlockFieldClone.FieldValue = purchaseRegistration.RetailerLocationId.ToString();

                            uiFormFieldCollectionLines.Where(x => x.PurchaseRegId == purchaseRegistration.Id).First().FormFields.Add(formBlockFieldClone);
                        }
                    }
                    else if ((formBlockField.FieldType == FieldTypeHelper.DropdownMenu && formBlockField.FieldName == FieldNameHelper.Country) || formBlockField.FieldType == FieldSourceHelper.Country)
                    {
                        if (campaignCountries.Any())
                        {
                            formFieldListValuesCollection.Clear();

                            foreach (var campaignCountry in campaignCountries)
                            {
                                var country = await _lookup_countryRepository.FirstOrDefaultAsync(x => x.Id == campaignCountry.CountryId);
                                if (country == null)
                                {
                                    continue;
                                }

                                formFieldListValuesCollection.Add(new FormFieldListValue
                                {
                                    KeyValue = country.Id.ToString(),
                                    Description = country.Description
                                });
                            }
                        }
                        else
                        {
                            // Fall back to normal country repository if campaign countries isn't set up 
                            var countries = await _countriesAppService.GetAll();

                            formFieldListValuesCollection.Clear();

                            foreach (var country in countries)
                            {
                                formFieldListValuesCollection.Add(new FormFieldListValue
                                {
                                    KeyValue = country.Country.Id.ToString(),
                                    Description = country.Country.Description
                                });
                            }
                        }

                        countryIsPresentInForm = true;

                        formBlockField.FieldType = FieldTypeHelper.DropdownMenu;
                        formBlockField.FieldListValues = formFieldListValuesCollection;
                        formBlockField.FieldSource = FieldSourceHelper.Registration;
                        formBlockField.FieldLineId = null;
                        formBlockField.RegistrationField = "CountryId";
                        formBlockField.PurchaseRegistrationField = String.Empty;
                        formBlockField.FieldValue = registration.CountryId.ToString();

                        uiFormFields.Add(formBlockField);
                    }
                    else if (formBlockField.FieldType == FieldSourceHelper.IbanChecker)
                    {
                        formBlockField.FieldLabel = "IBAN & BIC";
                        formBlockField.FieldSource = FieldSourceHelper.Registration;
                        formBlockField.FieldLineId = null;
                        formBlockField.RegistrationField = "Iban";
                        formBlockField.PurchaseRegistrationField = String.Empty;
                        formBlockField.FieldValue = $"{registration.Iban}|{registration.Bic}";

                        uiFormFields.Add(formBlockField);
                    }
                    else // Custom field, either on Registration level or PurchaseRegistration level...
                    {
                        var registrationFieldData = _lookup_registrationFieldDataRepository.GetAll().Where(f => f.RegistrationId == registration.Id);
                        var customRegistrationField = (from o in registrationFieldData
                                                       join o1 in _lookup_RegistrationFieldRepository.GetAll() on o.RegistrationFieldId equals o1.Id
                                                       where o1.FormFieldId == formBlockField.FieldId
                                                       select o).FirstOrDefault();

                        if (customRegistrationField == null)
                        {
                            //deviation rule (initially caused by UniqueCodeByCampaign variants @CaroCroc):
                            //find out if the value is linked to a related FormField based on FieldType & FieldName
                            //if so, change the FormFieldId to the related FormFieldId and continue from there
                            var relatedFormFields = _lookup_formFieldRepository.GetAll().Where(f => f.FieldTypeId == formBlockField.FieldTypeId && f.FieldName == formBlockField.FieldName);
                            var relatedRegistrationField = (from o in registrationFieldData
                                                            join o1 in _lookup_RegistrationFieldRepository.GetAll() on o.RegistrationFieldId equals o1.Id
                                                            where relatedFormFields.Any(f => f.Id == o1.FormFieldId)
                                                            select o1).FirstOrDefault();

                            if (relatedRegistrationField != null)
                            {
                                //changing the FormFieldId to the related FormFieldId
                                formBlockField.FieldId = relatedRegistrationField.FormFieldId;

                                customRegistrationField = (from o in registrationFieldData
                                                           join o1 in _lookup_RegistrationFieldRepository.GetAll() on o.RegistrationFieldId equals o1.Id
                                                           where o1.FormFieldId == formBlockField.FieldId
                                                           select o).FirstOrDefault();
                            }
                        }

                        if (customRegistrationField != null)
                        {
                            var fieldValueFormatted = String.Empty;
                            if (!String.IsNullOrWhiteSpace(customRegistrationField.Value))
                            {
                                fieldValueFormatted = FormFieldHelper.FormatUIFormField(customRegistrationField.Value, formBlockField.FieldType);
                            }

                            formBlockField.FieldSource = FieldSourceHelper.CustomRegistration;
                            formBlockField.FieldLineId = null;
                            formBlockField.FieldValue = fieldValueFormatted;

                            uiFormFields.Add(formBlockField);
                        }
                        else
                        {
                            foreach (var purchaseRegistration in purchaseRegistrations)
                            {
                                var purchaseRegistrationFieldData = _lookup_purchaseRegistrationFieldDataRepository.GetAll().Where(f => f.PurchaseRegistrationId == purchaseRegistration.Id);
                                var customPurchaseRegistrationField = (from o in purchaseRegistrationFieldData
                                                                       join o1 in _lookup_purchaseRegistrationFieldRepository.GetAll() on o.PurchaseRegistrationFieldId equals o1.Id
                                                                       where o1.FormFieldId == formBlockField.FieldId
                                                                       select o).FirstOrDefault();

                                if (customPurchaseRegistrationField != null && formBlockField.FieldName != FieldNameHelper.GiftId)
                                {
                                    var fieldValueFormatted = String.Empty;
                                    if (!String.IsNullOrWhiteSpace(customPurchaseRegistrationField.Value))
                                    {
                                        fieldValueFormatted = FormFieldHelper.FormatUIFormField(customPurchaseRegistrationField.Value, formBlockField.FieldType);
                                    }

                                    var formBlockFieldClone = FormFieldHelper.CloneUIFormField(formBlockField);

                                    //restore FieldListValues
                                    formBlockFieldClone.FieldListValues = formBlockField.FieldListValues;

                                    formBlockFieldClone.FieldSource = FieldSourceHelper.CustomPurchaseRegistration;
                                    formBlockFieldClone.FieldLineId = purchaseRegistration.Id;
                                    formBlockFieldClone.FieldValue = fieldValueFormatted;

                                    uiFormFieldCollectionLines.Where(x => x.PurchaseRegId == purchaseRegistration.Id).First().FormFields.Add(formBlockFieldClone);
                                }
                            }
                        }
                    }
                }

                var uiFormBlock = new Dtos.ProcessRegistration.FormBlock
                {
                    BlockTitle = formBlock.Description,
                    FormFields = uiFormFields,
                    FormFieldsCollectionLines = uiFormFieldCollectionLines.Any(x => x.FormFields.Count > 0) ? uiFormFieldCollectionLines : new List<FormFieldCollectionLine>()
                };

                uiFormBlocks.Add(uiFormBlock);
            }

            var registrationHistories = await _lookup_registrationHistoryRepository.GetAll()
                                                                                   .Where(h => h.RegistrationId == registration.Id)
                                                                                   .OrderBy(h => h.RegistrationId).ThenBy(r => r.Id)
                                                                                   .Include(h => h.RegistrationStatusFk).ToListAsync();

            var registrationHistoryList = new List<GetRegistrationHistoryEntryForViewOutput>();

            for (var i = 0; i < registrationHistories.Count; i++)
            {
                var user = await _userAppService.GetUserById(new EntityDto<long> { Id = registrationHistories[i].AbpUserId });
                var typeOfChange = TypeOfChange.Status;

                if (i > 0 && registrationHistories[i].RegistrationStatusId == registrationHistories[i - 1].RegistrationStatusId)
                {
                    typeOfChange = TypeOfChange.Data;
                }

                registrationHistoryList.Add(new GetRegistrationHistoryEntryForViewOutput
                {
                    FirstName = user?.Name,
                    LastName = user?.Surname,
                    DateCreated = registrationHistories[i].DateCreated,
                    RegistrationStatusCode = registrationHistories[i].RegistrationStatusFk.StatusCode,
                    RegistrationStatusDescription = registrationHistories[i].RegistrationStatusFk.Description,
                    TypeOfChange = typeOfChange
                });
            }

            var registrationMessageHistoryList = new List<GetRegistrationMessageHistoryEntryForViewOutput>();

            foreach (var historyRecord in messageHistory)
            {
                //var content = historyRecord.Content.Trim().Substring(0, 6) == "<html>" ? historyRecord.Content.Trim() : $"<html><body>{historyRecord.Content.Trim()}</body></html>";
                //var previewContent = $"data:text/html,{HttpUtility.UrlEncode(content)}";

                registrationMessageHistoryList.Add(new GetRegistrationMessageHistoryEntryForViewOutput
                {
                    MessageName = historyRecord.MessageName,
                    Subject = historyRecord.Subject,
                    To = historyRecord.To,
                    Content = historyRecord.Content,
                    TimeStamp = historyRecord.TimeStamp,
                    StatusId = messageStatus.Any(s => s.MessageId == historyRecord.MessageId) ? messageStatus.Where(s => s.MessageId == historyRecord.MessageId).First().StatusId : MessageStatusHelper.Unknown
                });
            }

            var uiForm = new GetRegistrationForProcessingOutput
            {
                CampaignTitle = registration.CampaignName,
                CampaignType = isPremium ? Dtos.CampaignType.Premium : isCashRefund ? Dtos.CampaignType.CashRefund : isActivationCode ? Dtos.CampaignType.ActivationCode : Dtos.CampaignType.Other,
                CampaignStartDate = registration.CampaignStartDate,
                CampaignEndDate = registration.CampaignEndDate,
                DateCreated = registrationHistories[0].DateCreated.ToString("dd-MM-yyyy HH:mm"),
                FormBlocks = uiFormBlocks,
                Registration = ObjectMapper.Map<CreateOrEditRegistrationDto>(registrationObject),
                RelatedRegistrationsByEmail = await GetRelatedRegistrationsByEmailAddress(registration.Id, registration.EmailAddress),
                RelatedRegistrationsBySerialNumber = await GetRelatedRegistrationsBySerialNumber(registration.Id),
                RegistrationHistoryEntries = registrationHistoryList,
                RegistrationMessageHistoryEntries = registrationMessageHistoryList,
                IncompleteReasons = availableIncompleteReasons,
                RejectionReasons = availableRejectionReasons,
                SelectedIncompleteReasonId = availableIncompleteReasons.Any(r => r.RejectionReason.Id == registration.RejectionReasonId) ? registration.RejectionReasonId : null,
                SelectedRejectionReasonId = availableRejectionReasons.Any(r => r.RejectionReason.Id == registration.RejectionReasonId) ? registration.RejectionReasonId : null,
                PostalCountry = !countryIsPresentInForm ? registration.Country : String.Empty,
                StatusCode = registration.StatusCode,
                StatusIsChangeable = !canOnlyEditRemarks
            };

            return uiForm;
        }

        public async Task<GetEditForRegistrationDto> GetEditForRegistration(GetFormLayoutAndDataInput input)
        {
            var registration = await _registrationRepository.GetAsync(input.RegistrationId);

            return new GetEditForRegistrationDto
            {
                Registration = ObjectMapper.Map<RegistrationDto>(registration)
            };
        }

        public async Task CreateOrEdit(CreateOrEditRegistrationDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Registrations_Create)]
        protected virtual async Task Create(CreateOrEditRegistrationDto input)
        {
            var registration = ObjectMapper.Map<Registration>(input);

            if (AbpSession.TenantId != null)
            {
                registration.TenantId = AbpSession.TenantId;
            }

            await _registrationRepository.InsertAsync(registration);
        }

        public async Task<bool> SendFormData(string blobStorage, string blobContainer, FormRegistrationHandlingDto vueJsToRmsModel)
        {
            var mappedData = JsonConvert.DeserializeObject<FormRegistrationHandlingDto>(vueJsToRmsModel.Data);

            int tenantId = AbpSession.GetTenantId();
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");
            DateTime nowUtc = DateTime.UtcNow;
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            DateTime westTime = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, cstZone);

            //VALIDATIONS
            var campaign = await _lookup_campaignRepository.GetAll().FirstOrDefaultAsync(fl => fl.CampaignCode == Convert.ToInt32(mappedData.CampaignCode));
            if (campaign == null)
            {
                _logger.LogError($"Error in {nameof(RegistrationsAppService)}: returned {nameof(campaign)} variable is invalid.");
                return false;
            }

            var campaignForm = await _lookup_campaignFormRepository.GetAll().FirstOrDefaultAsync(fl => fl.CampaignId == campaign.Id);
            if (campaignForm == null)
            {
                _logger.LogError($"Error in {nameof(RegistrationsAppService)}: returned {nameof(campaignForm)} variable is invalid.");
                return false;
            }

            var locale = await _lookup_localeRepository.GetAll().FirstOrDefaultAsync(fl => fl.Description == mappedData.Locale);
            if (locale == null)
            {
                _logger.LogError($"Error in {nameof(RegistrationsAppService)}: returned {nameof(locale)} variable is invalid.");
                return false;
            }

            long? countryId;
            if (mappedData.Country != null && !String.IsNullOrWhiteSpace(mappedData.Country.ListValueTranslationKeyValue))
            {
                var country = await _lookup_countryRepository.GetAll().FirstOrDefaultAsync(fl => fl.CountryCode == mappedData.Country.ListValueTranslationKeyValue.ToUpper().Trim());
                if (country == null)
                {
                    _logger.LogError($"Error in {nameof(RegistrationsAppService)}: returned {nameof(country)} variable is invalid.");
                    return false;
                }

                countryId = country.Id;
            }
            else
            {
                //fallback: take the country from the locale
                var country = await _lookup_countryRepository.GetAsync(locale.CountryId);
                if (country == null)
                {
                    _logger.LogError($"Error in {nameof(RegistrationsAppService)}: returned {nameof(country)} variable is invalid.");
                    return false;
                }

                countryId = country.Id;
            }

            //Added new component with retailer in it? add it to the retailerLocationId check.
            long? retailerLocationId = !String.IsNullOrWhiteSpace(mappedData.StorePurchased) ? (long?)Convert.ToInt64(mappedData.StorePurchased) : (mappedData.StorePicker != null && !String.IsNullOrWhiteSpace(mappedData.StorePicker.RetailerLocationId)) ? (long?)Convert.ToInt64(mappedData.StorePicker.RetailerLocationId) : mappedData.PurchaseRegistration != null ? (long?)0 : mappedData.PurchaseRegistrationLite != null ? (long?)0 : mappedData.PurchaseRegistrationSerial != null ? (long?)0 : null;
            var retailerLocations = await _retailersAppService.GetAllRetailersForCampaign(campaign.Id);
            if (retailerLocationId.HasValue)
            {
                if (retailerLocationId != 0 && !retailerLocations.Items.Any(rl => rl.RetailerLocation.Id == retailerLocationId.Value))
                {
                    _logger.LogError($"Error in {nameof(RegistrationsAppService)}: returned {nameof(retailerLocationId)} variable is invalid.");
                    return false;
                }
            }
            else
            {
                //fallback: look for the "not in list" record (Id = 999)
                var retailerLocation_999 = retailerLocations.Items.Where(rl => rl.RetailerLocation.Id == 999).FirstOrDefault();

                if (retailerLocation_999 != null)
                {
                    retailerLocationId = retailerLocation_999.RetailerLocation.Id;
                }
                else
                {
                    _logger.LogError($"Error in {nameof(RegistrationsAppService)}: returned {nameof(retailerLocationId)} variable is invalid.");
                    return false;
                }
            }

            bool productIsMapped = false;
            if (!String.IsNullOrWhiteSpace(mappedData.Product) || 
                (mappedData.ProductPremium != null && mappedData.ProductPremium.Count > 0) ||
                (mappedData.ProductPremiumLite != null && mappedData.ProductPremiumLite.Count > 0) ||
                (mappedData.ProductPremiumQuantity != null && mappedData.ProductPremiumQuantity.Count > 0) || 
                (mappedData.PurchaseRegistration != null && mappedData.PurchaseRegistration.Count > 0) || 
                (mappedData.PurchaseRegistrationLite != null && mappedData.PurchaseRegistrationLite.Count > 0) || 
                (mappedData.PurchaseRegistrationSerial != null && mappedData.PurchaseRegistrationSerial.Count > 0)) 
            {
                productIsMapped = true;
            }

            if (!productIsMapped)
            {
                _logger.LogError($"Error in {nameof(RegistrationsAppService)}: purchased product is missing.");
                return false;
            }

            //INSERT REGISTRATION
            var registrationStatusPending = await _registrationStatusesAppService.GetByStatusCode(RegistrationStatusCodeHelper.Pending);

            var registrationMapper = new Registration
            {
                CompanyName = mappedData.CompanyName,
                Gender = mappedData.Gender?.ListValueTranslationKeyValue,
                FirstName = mappedData.FirstName,
                LastName = mappedData.LastName,
                PostalCode = mappedData.ZipCode,
                Street = mappedData.StreetName,
                HouseNr = mappedData.HouseNumber,
                City = mappedData.Residence,
                CountryId = countryId.Value,
                EmailAddress = mappedData.EmailAddress,
                PhoneNumber = mappedData.PhoneNumber,
                Bic = mappedData.IbanChecker?.Bic,
                Iban = mappedData.IbanChecker?.Iban,
                CampaignId = campaign.Id,
                CampaignFormId = campaignForm.Id,
                LocaleId = locale.Id,
                RegistrationStatusId = registrationStatusPending.RegistrationStatus.Id,
                TenantId = tenantId,
            };

            var registrationId = await _registrationRepository.InsertAndGetIdAsync(registrationMapper);
            if (registrationId <= 0)
            {
                _logger.LogError($"Error in {nameof(RegistrationsAppService)}: returned {nameof(registrationId)} variable is less than or equal to 0.");
                return false;
            }

            //UPLOAD VARIOUS IMAGES (if available)
            string invoiceImage = string.Empty;
            if (mappedData.InvoiceImagePath != null && mappedData.InvoiceImagePath.Count > 0)
            {
                var image = mappedData.InvoiceImagePath.ToList()[0];
                var blobServiceClient = new BlobServiceClient(blobStorage);
                BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainer);

                var fileExtension = string.Empty;
                int index = image.IndexOf('/') + 1;
                while (image.Substring(index, 1) != ";")
                {
                    fileExtension += image.Substring(index, 1);
                    index += 1;
                }
                fileExtension = fileExtension.Replace("+xml", "");
                var fileContent = image.Split("base64,")[1];
                invoiceImage = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{registrationId}-{Guid.NewGuid()}.{fileExtension}";
                blobContainerClient.UploadBlob(invoiceImage, new MemoryStream(Convert.FromBase64String(fileContent)));
            }

            string serialCodeImage = string.Empty;
            if (mappedData.SerialCodeImage != null && mappedData.SerialCodeImage.Count > 0)
            {
                var image = mappedData.SerialCodeImage.ToList()[0];
                var blobServiceClient = new BlobServiceClient(blobStorage);
                BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainer);

                var fileExtension = string.Empty;
                int index = image.IndexOf('/') + 1;
                while (image.Substring(index, 1) != ";")
                {
                    fileExtension += image.Substring(index, 1);
                    index += 1;
                }
                fileExtension = fileExtension.Replace("+xml", "");
                var fileContent = image.Split("base64,")[1];
                serialCodeImage = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{registrationId}-{Guid.NewGuid()}.{fileExtension}";
                blobContainerClient.UploadBlob(serialCodeImage, new MemoryStream(Convert.FromBase64String(fileContent)));
            }

            string eanCodeImage = string.Empty;
            if (mappedData.EanCodeImage != null && mappedData.EanCodeImage.Count > 0)
            {
                var image = mappedData.EanCodeImage.ToList()[0];
                var blobServiceClient = new BlobServiceClient(blobStorage);
                BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainer);

                var fileExtension = string.Empty;
                int index = image.IndexOf('/') + 1;
                while (image.Substring(index, 1) != ";")
                {
                    fileExtension += image.Substring(index, 1);
                    index += 1;
                }
                fileExtension = fileExtension.Replace("+xml", "");
                var fileContent = image.Split("base64,")[1];
                eanCodeImage = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{registrationId}-{Guid.NewGuid()}.{fileExtension}";
                blobContainerClient.UploadBlob(eanCodeImage, new MemoryStream(Convert.FromBase64String(fileContent)));
            }

            string retourStorageImage = string.Empty;
            if (mappedData.RetourStorageImage != null && mappedData.RetourStorageImage.Count > 0)
            {
                var image = mappedData.RetourStorageImage.ToList()[0];
                var blobServiceClient = new BlobServiceClient(blobStorage);
                BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainer);

                var fileExtension = string.Empty;
                int index = image.IndexOf('/') + 1;
                while (image.Substring(index, 1) != ";")
                {
                    fileExtension += image.Substring(index, 1);
                    index += 1;
                }
                fileExtension = fileExtension.Replace("+xml", "");
                var fileContent = image.Split("base64,")[1];
                retourStorageImage = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{registrationId}-{Guid.NewGuid()}.{fileExtension}";
                blobContainerClient.UploadBlob(retourStorageImage, new MemoryStream(Convert.FromBase64String(fileContent)));
            }

            //CUSTOM REGISTRATION FIELDS
            var registrationFieldUniqueCode = await _lookup_RegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.UniqueCode).FirstOrDefaultAsync();
            if (registrationFieldUniqueCode != null)
            {
                if (mappedData.UniqueCode != null)
                {
                    await InsertRegistrationFieldData(tenantId, mappedData.UniqueCode, registrationFieldUniqueCode.Id, registrationId);
                }
                else
                {
                    await InsertRegistrationFieldData(tenantId, String.Empty, registrationFieldUniqueCode.Id, registrationId);
                }
            }

            var registrationFieldUniqueCodeByCampaign = await _lookup_RegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.UniqueCodeByCampaign).FirstOrDefaultAsync();
            if (registrationFieldUniqueCodeByCampaign != null)
            {
                if (mappedData.UniqueCodeByCampaign != null)
                {
                    await InsertRegistrationFieldData(tenantId, mappedData.UniqueCodeByCampaign, registrationFieldUniqueCodeByCampaign.Id, registrationId);
                }
                else
                {
                    await InsertRegistrationFieldData(tenantId, String.Empty, registrationFieldUniqueCodeByCampaign.Id, registrationId);
                }
            }

            var registrationFieldLegalForm = await _lookup_RegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.LegalForm).FirstOrDefaultAsync();
            if (registrationFieldLegalForm != null)
            {
                if (mappedData.LegalForm != null)
                {
                    await InsertRegistrationFieldData(tenantId, mappedData.LegalForm, registrationFieldLegalForm.Id, registrationId);
                }
                else
                {
                    await InsertRegistrationFieldData(tenantId, String.Empty, registrationFieldLegalForm.Id, registrationId);
                }
            }

            var registrationFieldBusinessNumber = await _lookup_RegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.BusinessNumber).FirstOrDefaultAsync();
            if (registrationFieldBusinessNumber != null)
            {
                if (mappedData.BusinessNumber != null)
                {
                    await InsertRegistrationFieldData(tenantId, mappedData.BusinessNumber, registrationFieldBusinessNumber.Id, registrationId);
                }
                else
                {
                    await InsertRegistrationFieldData(tenantId, String.Empty, registrationFieldBusinessNumber.Id, registrationId);
                }
            }

            var registrationFieldVatNumber = await _lookup_RegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.VatNumber).FirstOrDefaultAsync();
            if (registrationFieldVatNumber != null)
            {
                if (mappedData.VatNumber != null)
                {
                    await InsertRegistrationFieldData(tenantId, mappedData.VatNumber, registrationFieldVatNumber.Id, registrationId);
                }
                else
                {
                    await InsertRegistrationFieldData(tenantId, String.Empty, registrationFieldVatNumber.Id, registrationId);
                }
            }

            var registrationFieldStoreNumber = await _lookup_RegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.StoreNumber).FirstOrDefaultAsync();
            if (registrationFieldStoreNumber != null)
            {
                if (mappedData.StoreNumber != null)
                {
                    await InsertRegistrationFieldData(tenantId, mappedData.StoreNumber, registrationFieldStoreNumber.Id, registrationId);
                }
                else
                {
                    await InsertRegistrationFieldData(tenantId, String.Empty, registrationFieldStoreNumber.Id, registrationId);
                }
            }

            var registrationFieldStoreName = await _lookup_RegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.StoreName).FirstOrDefaultAsync();
            if (registrationFieldStoreName != null)
            {
                if (mappedData.StoreName != null)
                {
                    await InsertRegistrationFieldData(tenantId, mappedData.StoreName, registrationFieldStoreName.Id, registrationId);
                }
                else
                {
                    await InsertRegistrationFieldData(tenantId, String.Empty, registrationFieldStoreName.Id, registrationId);
                }
            }

            var registrationFieldDeclareAuthority = await _lookup_RegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.DeclareAuthority).FirstOrDefaultAsync();
            if (registrationFieldDeclareAuthority != null)
            {
                if (mappedData.DeclareAuthority != null)
                {
                    await InsertRegistrationFieldData(tenantId, mappedData.DeclareAuthority.ListValueTranslationKeyValue, registrationFieldDeclareAuthority.Id, registrationId);
                }
                else
                {
                    await InsertRegistrationFieldData(tenantId, String.Empty, registrationFieldDeclareAuthority.Id, registrationId);
                }
            }

            var registrationFieldDeclareAuthorityRetour = await _lookup_RegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.DeclareAuthorityRetour).FirstOrDefaultAsync();
            if (registrationFieldDeclareAuthorityRetour != null)
            {
                if (mappedData.DeclareAuthorityRetour != null)
                {
                    await InsertRegistrationFieldData(tenantId, mappedData.DeclareAuthorityRetour.ListValueTranslationKeyValue, registrationFieldDeclareAuthorityRetour.Id, registrationId);
                }
                else
                {
                    await InsertRegistrationFieldData(tenantId, String.Empty, registrationFieldDeclareAuthorityRetour.Id, registrationId);
                }
            }

            var registrationFieldNewsletter = await _lookup_RegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.Newsletter).FirstOrDefaultAsync();
            if (registrationFieldNewsletter != null)
            {
                if (mappedData.Newsletter != null)
                {
                    await InsertRegistrationFieldData(tenantId, mappedData.Newsletter, registrationFieldNewsletter.Id, registrationId);
                }
                else
                {
                    await InsertRegistrationFieldData(tenantId, "false", registrationFieldNewsletter.Id, registrationId);
                }
            }

            var registrationFieldPrivacy = await _lookup_RegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.Privacy).FirstOrDefaultAsync();
            if (registrationFieldPrivacy != null)
            {
                if (mappedData.Privacy != null)
                {
                    await InsertRegistrationFieldData(tenantId, mappedData.Privacy, registrationFieldPrivacy.Id, registrationId);
                }
                else
                {
                    await InsertRegistrationFieldData(tenantId, "false", registrationFieldPrivacy.Id, registrationId);
                }
            }

            var registrationFieldPolicy = await _lookup_RegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.Policy).FirstOrDefaultAsync();
            if (registrationFieldPolicy != null)
            {
                if (mappedData.Policy != null)
                {
                    await InsertRegistrationFieldData(tenantId, mappedData.Policy, registrationFieldPolicy.Id, registrationId);
                }
                else
                {
                    await InsertRegistrationFieldData(tenantId, "false", registrationFieldPolicy.Id, registrationId);
                }
            }

            var registrationFieldPolicyDreft = await _lookup_RegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.PolicyDreft).FirstOrDefaultAsync();
            if (registrationFieldPolicyDreft != null)
            {
                if (mappedData.PolicyDreft != null)
                {
                    await InsertRegistrationFieldData(tenantId, mappedData.PolicyDreft, registrationFieldPolicyDreft.Id, registrationId);
                }
                else
                {
                    await InsertRegistrationFieldData(tenantId, "false", registrationFieldPolicyDreft.Id, registrationId);
                }
            }

            var registrationFieldPolicyAriel = await _lookup_RegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.PolicyAriel).FirstOrDefaultAsync();
            if (registrationFieldPolicyAriel != null)
            {
                if (mappedData.PolicyAriel != null)
                {
                    await InsertRegistrationFieldData(tenantId, mappedData.PolicyAriel, registrationFieldPolicyAriel.Id, registrationId);
                }
                else
                {
                    await InsertRegistrationFieldData(tenantId, "false", registrationFieldPolicyAriel.Id, registrationId);
                }
            }

            var registrationFieldPolicyNoFrost = await _lookup_RegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.PolicyNoFrost).FirstOrDefaultAsync();
            if (registrationFieldPolicyNoFrost != null)
            {
                if (mappedData.PolicyNoFrost != null)
                {
                    await InsertRegistrationFieldData(tenantId, mappedData.PolicyNoFrost, registrationFieldPolicyNoFrost.Id, registrationId);
                }
                else
                {
                    await InsertRegistrationFieldData(tenantId, "false", registrationFieldPolicyNoFrost.Id, registrationId);
                }
            }

            //PROCESS PURCHASE REGISTRATION(S)
            int purchaseLineCount = !String.IsNullOrWhiteSpace(mappedData.Product) ? 1 : 
                                    mappedData.ProductPremium != null ? mappedData.ProductPremium.Count :
                                    mappedData.ProductPremiumLite != null ? mappedData.ProductPremiumLite.Count :
                                    mappedData.ProductPremiumQuantity != null ? mappedData.ProductPremiumQuantity.Count : 
                                    mappedData.PurchaseRegistration != null ? mappedData.PurchaseRegistration.Count : 
                                    mappedData.PurchaseRegistrationLite != null ? mappedData.PurchaseRegistrationLite.Count :
                                    mappedData.PurchaseRegistrationSerial.Count;

            for (int purchaseLineIndex = 0; purchaseLineIndex < purchaseLineCount; purchaseLineIndex++)
            {
                //RETRIEVE PURCHASED PRODUCT & HANDLING LINE
                long? productId;
                long? handlinglineId;
                int quantity = 1;
                decimal totalAmount = 0;
                string serialNumber = mappedData.SerialNumber;

                if (!String.IsNullOrWhiteSpace(mappedData.Product))
                {
                    var product = await _lookup_productRepository.GetAsync(Convert.ToInt64(mappedData.Product));
                    var productHandling = await _productHandlingRepository.GetAll().FirstOrDefaultAsync(x => x.CampaignId == campaign.Id);
                    var handlingLines = await _lookup_handlingLineRepository.GetAllListAsync(x => x.ProductHandlingId == productHandling.Id);
                    var handlingLineIds = handlingLines.Select(x => x.Id).ToList();
                    var handlingLineProduct = await _lookup_handlingLineProductRepository.GetAll().Where(x => handlingLineIds.Contains(x.HandlingLineId) && x.ProductId == product.Id).FirstOrDefaultAsync();

                    if (handlingLineProduct != null)
                    {
                        handlinglineId = handlingLineProduct.HandlingLineId;
                    }
                    else
                    {
                        if (handlingLines.Where(l => l.CustomerCode.ToUpper().Trim() != "UNKNOWN").Count() == 1)
                        {
                            //only 1 Premium is linked to this campaign, so take that one
                            handlinglineId = handlingLines.Where(l => l.CustomerCode.ToUpper().Trim() != "UNKNOWN").First().Id;
                        }
                        else
                        {
                            //more than 1 Premiums are linked to this campaign, so link it to the UNKNOWN
                            handlinglineId = handlingLines.Where(l => l.CustomerCode.ToUpper().Trim() == "UNKNOWN").First().Id;
                        }
                    }

                    productId = product.Id;
                    quantity = !String.IsNullOrWhiteSpace(mappedData.Quantity) ? Convert.ToInt32(mappedData.Quantity) : 1;
                    totalAmount = !String.IsNullOrWhiteSpace(mappedData.TotalAmount) ? Convert.ToDecimal(mappedData.TotalAmount) : 0.00M;
                }
                else if (mappedData.ProductPremium != null)
                {
                    var product = await _lookup_productRepository.GetAsync(Convert.ToInt64(mappedData.ProductPremium[purchaseLineIndex].ChosenItemId));

                    productId = product.Id;
                    handlinglineId = Convert.ToInt64(mappedData.ProductPremium[purchaseLineIndex].HandlingLineId);
                    quantity = !String.IsNullOrWhiteSpace(mappedData.Quantity) ? Convert.ToInt32(mappedData.Quantity) : 1;
                    totalAmount = !String.IsNullOrWhiteSpace(mappedData.TotalAmount) ? Convert.ToDecimal(mappedData.TotalAmount) : 0.00M;
                }
                else if (mappedData.ProductPremiumLite != null)
                {
                    var product = await _lookup_productRepository.GetAsync(Convert.ToInt64(mappedData.ProductPremiumLite[purchaseLineIndex].ChosenItemId));

                    productId = product.Id;
                    handlinglineId = Convert.ToInt64(mappedData.ProductPremiumLite[purchaseLineIndex].HandlingLineId);
                    quantity = !String.IsNullOrWhiteSpace(mappedData.Quantity) ? Convert.ToInt32(mappedData.Quantity) : 1;
                    totalAmount = !String.IsNullOrWhiteSpace(mappedData.TotalAmount) ? Convert.ToDecimal(mappedData.TotalAmount) : 0.00M;
                }
                else if (mappedData.ProductPremiumQuantity != null)
                {
                    var product = await _lookup_productRepository.GetAsync(Convert.ToInt64(mappedData.ProductPremiumQuantity[purchaseLineIndex].ProductPremium[0].ChosenItemId));

                    productId = product.Id;
                    handlinglineId = Convert.ToInt64(mappedData.ProductPremiumQuantity[purchaseLineIndex].ProductPremium[0].HandlingLineId);
                    quantity = !String.IsNullOrWhiteSpace(mappedData.ProductPremiumQuantity[purchaseLineIndex].HandlingLineQuantity) ? Convert.ToInt32(mappedData.ProductPremiumQuantity[purchaseLineIndex].HandlingLineQuantity) : 1;
                    totalAmount = !String.IsNullOrWhiteSpace(mappedData.TotalAmount) ? Convert.ToDecimal(mappedData.TotalAmount) : 0.00M;
                }
                else if (mappedData.PurchaseRegistration != null)
                {
                    //mappedData.PurchaseRegistration is applicable
                    var product = await _lookup_productRepository.GetAsync(Convert.ToInt64(mappedData.PurchaseRegistration[purchaseLineIndex].ProductPremium[0].ChosenItemId));

                    productId = product.Id;
                    handlinglineId = Convert.ToInt64(mappedData.PurchaseRegistration[purchaseLineIndex].ProductPremium[0].HandlingLineId);
                    quantity = !String.IsNullOrWhiteSpace(mappedData.PurchaseRegistration[purchaseLineIndex].HandlingLineQuantity) ? Convert.ToInt32(mappedData.PurchaseRegistration[purchaseLineIndex].HandlingLineQuantity) : 1;
                    totalAmount = !String.IsNullOrWhiteSpace(mappedData.PurchaseRegistration[purchaseLineIndex].TotalAmount) ? Convert.ToDecimal(mappedData.PurchaseRegistration[purchaseLineIndex].TotalAmount) : 0.00M;
                    retailerLocationId = !String.IsNullOrWhiteSpace(mappedData.PurchaseRegistration[purchaseLineIndex].StorePurchased) ? Convert.ToInt64(mappedData.PurchaseRegistration[purchaseLineIndex].StorePurchased) : 0;
                    if (retailerLocationId.Value == 0)
                    {
                        _logger.LogError($"Error in {nameof(RegistrationsAppService)}: returned {nameof(retailerLocationId)} variable is invalid.");
                        return false;
                    }

                    if (mappedData.PurchaseRegistration[purchaseLineIndex].InvoiceImage != null)
                    {
                        var image = mappedData.PurchaseRegistration[purchaseLineIndex].InvoiceImage.ToList()[0];
                        var blobServiceClient = new BlobServiceClient(blobStorage);
                        BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainer);

                        var fileExtension = string.Empty;
                        int index = image.IndexOf('/') + 1;
                        while (image.Substring(index, 1) != ";")
                        {
                            fileExtension += image.Substring(index, 1);
                            index += 1;
                        }
                        fileExtension = fileExtension.Replace("+xml", "");
                        var fileContent = image.Split("base64,")[1];
                        invoiceImage = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{registrationId}-{Guid.NewGuid()}.{fileExtension}";
                        blobContainerClient.UploadBlob(invoiceImage, new MemoryStream(Convert.FromBase64String(fileContent)));
                    }
                }
                else if (mappedData.PurchaseRegistrationLite != null)
                {
                    //mappedData.PurchaseRegistration is applicable
                    var product = await _lookup_productRepository.GetAsync(Convert.ToInt64(mappedData.PurchaseRegistrationLite[purchaseLineIndex].ProductPremium[0].ChosenItemId));

                    productId = product.Id;
                    handlinglineId = Convert.ToInt64(mappedData.PurchaseRegistrationLite[purchaseLineIndex].ProductPremium[0].HandlingLineId);
                    quantity = !String.IsNullOrWhiteSpace(mappedData.PurchaseRegistrationLite[purchaseLineIndex].HandlingLineQuantity) ? Convert.ToInt32(mappedData.PurchaseRegistrationLite[purchaseLineIndex].HandlingLineQuantity) : 1;
                    totalAmount = !String.IsNullOrWhiteSpace(mappedData.PurchaseRegistrationLite[purchaseLineIndex].TotalAmount) ? Convert.ToDecimal(mappedData.PurchaseRegistrationLite[purchaseLineIndex].TotalAmount) : 0.00M;
                    retailerLocationId = !String.IsNullOrWhiteSpace(mappedData.PurchaseRegistrationLite[purchaseLineIndex].StorePurchased) ? Convert.ToInt64(mappedData.PurchaseRegistrationLite[purchaseLineIndex].StorePurchased) : 0;
                    if (retailerLocationId.Value == 0)
                    {
                        _logger.LogError($"Error in {nameof(RegistrationsAppService)}: returned {nameof(retailerLocationId)} variable is invalid.");
                        return false;
                    }

                    if (mappedData.PurchaseRegistrationLite[purchaseLineIndex].InvoiceImage != null)
                    {
                        var image = mappedData.PurchaseRegistrationLite[purchaseLineIndex].InvoiceImage.ToList()[0];
                        var blobServiceClient = new BlobServiceClient(blobStorage);
                        BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainer);

                        var fileExtension = string.Empty;
                        int index = image.IndexOf('/') + 1;
                        while (image.Substring(index, 1) != ";")
                        {
                            fileExtension += image.Substring(index, 1);
                            index += 1;
                        }
                        fileExtension = fileExtension.Replace("+xml", "");
                        var fileContent = image.Split("base64,")[1];
                        invoiceImage = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{registrationId}-{Guid.NewGuid()}.{fileExtension}";
                        blobContainerClient.UploadBlob(invoiceImage, new MemoryStream(Convert.FromBase64String(fileContent)));
                    }
                }
                else
                {
                    //mappedData.PurchaseRegistrationSerial is applicable
                    var product = await _lookup_productRepository.GetAsync(Convert.ToInt64(mappedData.PurchaseRegistrationSerial[purchaseLineIndex].ProductPremium[0].ChosenItemId));

                    productId = product.Id;
                    handlinglineId = Convert.ToInt64(mappedData.PurchaseRegistrationSerial[purchaseLineIndex].ProductPremium[0].HandlingLineId);
                    quantity = !String.IsNullOrWhiteSpace(mappedData.PurchaseRegistrationSerial[purchaseLineIndex].HandlingLineQuantity) ? Convert.ToInt32(mappedData.PurchaseRegistrationSerial[purchaseLineIndex].HandlingLineQuantity) : 1;
                    totalAmount = !String.IsNullOrWhiteSpace(mappedData.PurchaseRegistrationSerial[purchaseLineIndex].TotalAmount) ? Convert.ToDecimal(mappedData.PurchaseRegistrationSerial[purchaseLineIndex].TotalAmount) : 0.00M;
                    retailerLocationId = !String.IsNullOrWhiteSpace(mappedData.PurchaseRegistrationSerial[purchaseLineIndex].StorePurchased) ? Convert.ToInt64(mappedData.PurchaseRegistrationSerial[purchaseLineIndex].StorePurchased) : 0;
                    if (retailerLocationId.Value == 0)
                    {
                        _logger.LogError($"Error in {nameof(RegistrationsAppService)}: returned {nameof(retailerLocationId)} variable is invalid.");
                        return false;
                    }

                    if (mappedData.PurchaseRegistrationSerial[purchaseLineIndex].InvoiceImage != null)
                    {
                        var image = mappedData.PurchaseRegistrationSerial[purchaseLineIndex].InvoiceImage.ToList()[0];
                        var blobServiceClient = new BlobServiceClient(blobStorage);
                        BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainer);

                        var fileExtension = string.Empty;
                        int index = image.IndexOf('/') + 1;
                        while (image.Substring(index, 1) != ";")
                        {
                            fileExtension += image.Substring(index, 1);
                            index += 1;
                        }
                        fileExtension = fileExtension.Replace("+xml", "");
                        var fileContent = image.Split("base64,")[1];
                        invoiceImage = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{registrationId}-{Guid.NewGuid()}.{fileExtension}";
                        blobContainerClient.UploadBlob(invoiceImage, new MemoryStream(Convert.FromBase64String(fileContent)));
                    }

                    if (mappedData.PurchaseRegistrationSerial[purchaseLineIndex].SerialImage != null)
                    {
                        var image = mappedData.PurchaseRegistrationSerial[purchaseLineIndex].SerialImage.ToList()[0];
                        var blobServiceClient = new BlobServiceClient(blobStorage);
                        BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainer);

                        var fileExtension = string.Empty;
                        int index = image.IndexOf('/') + 1;
                        while (image.Substring(index, 1) != ";")
                        {
                            fileExtension += image.Substring(index, 1);
                            index += 1;
                        }
                        fileExtension = fileExtension.Replace("+xml", "");
                        var fileContent = image.Split("base64,")[1];
                        serialCodeImage = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{registrationId}-{Guid.NewGuid()}.{fileExtension}";
                        blobContainerClient.UploadBlob(serialCodeImage, new MemoryStream(Convert.FromBase64String(fileContent)));
                    }

                    if (mappedData.PurchaseRegistrationSerial[purchaseLineIndex].SerialNumber != null)
                    {
                        serialNumber = mappedData.PurchaseRegistrationSerial[purchaseLineIndex].SerialNumber;
                    }
                }

                //INSERT PURCHASE REGISTRATION
                var purchaseRegistrationMapper = new PurchaseRegistration
                {
                    PurchaseDate = !String.IsNullOrWhiteSpace(mappedData.PurchaseDate) ? Convert.ToDateTime(mappedData.PurchaseDate) : westTime,
                    RegistrationId = registrationId,
                    RetailerLocationId = retailerLocationId.Value,
                    ProductId = productId.Value,
                    Quantity = quantity,
                    InvoiceImagePath = invoiceImage,
                    TotalAmount = totalAmount,
                    HandlingLineId = handlinglineId.Value,
                    TenantId = tenantId,
                };

                var purchaseRegistrationId = await _lookup_purchaseRegistrationRepository.InsertAndGetIdAsync(purchaseRegistrationMapper);
                if (purchaseRegistrationId <= 0)
                {
                    _logger.LogError($"Error in {nameof(RegistrationsAppService)}: returned {nameof(purchaseRegistrationId)} variable is less than or equal to 0.");
                    return false;
                };

                //CUSTOM PURCHASE REGISTRATION FIELDS
                var purchaseRegistrationFieldAlreadyCustomer = await _lookup_purchaseRegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.AlreadyCustomer).FirstOrDefaultAsync();
                if (purchaseRegistrationFieldAlreadyCustomer != null)
                {
                    if (mappedData.AlreadyCustomer != null)
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, mappedData.AlreadyCustomer.ListValueTranslationKeyValue.ToString(), purchaseRegistrationId, purchaseRegistrationFieldAlreadyCustomer.Id);
                    }
                    else
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, String.Empty, purchaseRegistrationId, purchaseRegistrationFieldAlreadyCustomer.Id);
                    }
                }

                var purchaseRegistrationFieldCustomerReturnReason = await _lookup_purchaseRegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.CustomerReturnReason).FirstOrDefaultAsync();
                if (purchaseRegistrationFieldCustomerReturnReason != null)
                {
                    if (mappedData.CustomorReturnReason != null)
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, mappedData.CustomorReturnReason.ListValueTranslationKeyValue.ToString(), purchaseRegistrationId, purchaseRegistrationFieldCustomerReturnReason.Id);
                    }
                    else
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, String.Empty, purchaseRegistrationId, purchaseRegistrationFieldCustomerReturnReason.Id);
                    }
                }

                var purchaseRegistrationFieldDeviceReturnReason = await _lookup_purchaseRegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.DeviceReturnReason).FirstOrDefaultAsync();
                if (purchaseRegistrationFieldDeviceReturnReason != null)
                {
                    if (mappedData.DeviceReturnReason != null)
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, mappedData.DeviceReturnReason.ListValueTranslationKeyValue.ToString(), purchaseRegistrationId, purchaseRegistrationFieldDeviceReturnReason.Id);
                    }
                    else
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, String.Empty, purchaseRegistrationId, purchaseRegistrationFieldDeviceReturnReason.Id);
                    }
                }

                var purchaseRegistrationFieldEpenReturnReason = await _lookup_purchaseRegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.EpenReturnReason).FirstOrDefaultAsync();
                if (purchaseRegistrationFieldEpenReturnReason != null)
                {
                    if (mappedData.EpenReturnReason != null)
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, mappedData.EpenReturnReason.ListValueTranslationKeyValue.ToString(), purchaseRegistrationId, purchaseRegistrationFieldEpenReturnReason.Id);
                    }
                    else
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, String.Empty, purchaseRegistrationId, purchaseRegistrationFieldEpenReturnReason.Id);
                    }
                }

                var purchaseRegistrationFieldEpodReturnReason = await _lookup_purchaseRegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.EpodReturnReason).FirstOrDefaultAsync();
                if (purchaseRegistrationFieldEpodReturnReason != null)
                {
                    if (mappedData.EpodReturnReason != null)
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, mappedData.EpodReturnReason.ListValueTranslationKeyValue.ToString(), purchaseRegistrationId, purchaseRegistrationFieldEpodReturnReason.Id);
                    }
                    else
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, String.Empty, purchaseRegistrationId, purchaseRegistrationFieldEpodReturnReason.Id);
                    }
                }

                var purchaseRegistrationFieldOtherReason = await _lookup_purchaseRegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.OtherReason).FirstOrDefaultAsync();
                if (purchaseRegistrationFieldOtherReason != null)
                {
                    if (mappedData.OtherReason != null)
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, mappedData.OtherReason, purchaseRegistrationId, purchaseRegistrationFieldOtherReason.Id);
                    }
                    else
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, String.Empty, purchaseRegistrationId, purchaseRegistrationFieldOtherReason.Id);
                    }
                }

                var purchaseRegistrationFieldStoreNumber = await _lookup_purchaseRegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.StoreNumber).FirstOrDefaultAsync();
                if (purchaseRegistrationFieldStoreNumber != null)
                {
                    if (mappedData.StoreNumber != null)
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, mappedData.StoreNumber, purchaseRegistrationId, purchaseRegistrationFieldStoreNumber.Id);
                    }
                    else
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, String.Empty, purchaseRegistrationId, purchaseRegistrationFieldStoreNumber.Id);
                    }
                }

                var purchaseRegistrationFieldStoreName = await _lookup_purchaseRegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.StoreName).FirstOrDefaultAsync();
                if (purchaseRegistrationFieldStoreName != null)
                {
                    if (mappedData.StoreName != null)
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, mappedData.StoreName, purchaseRegistrationId, purchaseRegistrationFieldStoreName.Id);
                    }
                    else
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, String.Empty, purchaseRegistrationId, purchaseRegistrationFieldStoreName.Id);
                    }
                }

                var purchaseRegistrationFieldPlaceNameStore = await _lookup_purchaseRegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.PlaceNameStore).FirstOrDefaultAsync();
                if (purchaseRegistrationFieldPlaceNameStore != null)
                {
                    if (mappedData.PlaceNameStore != null)
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, mappedData.PlaceNameStore, purchaseRegistrationId, purchaseRegistrationFieldPlaceNameStore.Id);
                    }
                    else
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, String.Empty, purchaseRegistrationId, purchaseRegistrationFieldPlaceNameStore.Id);
                    }
                }

                var purchaseRegistrationFieldStoreNotAvailable = await _lookup_purchaseRegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.StoreNotAvailable).FirstOrDefaultAsync();
                if (purchaseRegistrationFieldStoreNotAvailable != null)
                {
                    if (mappedData.StoreNotAvalible != null)
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, mappedData.StoreNotAvalible, purchaseRegistrationId, purchaseRegistrationFieldStoreNotAvailable.Id);
                    }
                    else
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, String.Empty, purchaseRegistrationId, purchaseRegistrationFieldStoreNotAvailable.Id);
                    }
                }

                var purchaseRegistrationFieldDogProduct = await _lookup_purchaseRegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.DogProduct).FirstOrDefaultAsync();
                if (purchaseRegistrationFieldDogProduct != null)
                {
                    if (mappedData.DogProduct != null)
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, mappedData.DogProduct.ListValueTranslationKeyValue.ToString(), purchaseRegistrationId, purchaseRegistrationFieldDogProduct.Id);
                    }
                    else
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, String.Empty, purchaseRegistrationId, purchaseRegistrationFieldDogProduct.Id);
                    }
                }

                var purchaseRegistrationFieldCatProduct = await _lookup_purchaseRegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.CatProduct).FirstOrDefaultAsync();
                if (purchaseRegistrationFieldCatProduct != null)
                {
                    if (mappedData.CatProduct != null)
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, mappedData.CatProduct.ListValueTranslationKeyValue.ToString(), purchaseRegistrationId, purchaseRegistrationFieldCatProduct.Id);
                    }
                    else
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, String.Empty, purchaseRegistrationId, purchaseRegistrationFieldCatProduct.Id);
                    }
                }

                var purchaseRegistrationFieldDeclareAuthority = await _lookup_purchaseRegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.DeclareAuthority).FirstOrDefaultAsync();
                if (purchaseRegistrationFieldDeclareAuthority != null)
                {
                    if (mappedData.DeclareAuthority != null)
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, mappedData.DeclareAuthority.ListValueTranslationKeyValue.ToString(), purchaseRegistrationId, purchaseRegistrationFieldDeclareAuthority.Id);
                    }
                    else
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, String.Empty, purchaseRegistrationId, purchaseRegistrationFieldDeclareAuthority.Id);
                    }
                }

                var purchaseRegistrationFieldDeclareAuthorityRetour = await _lookup_purchaseRegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.DeclareAuthorityRetour).FirstOrDefaultAsync();
                if (purchaseRegistrationFieldDeclareAuthorityRetour != null)
                {
                    if (mappedData.DeclareAuthorityRetour != null)
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, mappedData.DeclareAuthorityRetour.ListValueTranslationKeyValue.ToString(), purchaseRegistrationId, purchaseRegistrationFieldDeclareAuthorityRetour.Id);
                    }
                    else
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, String.Empty, purchaseRegistrationId, purchaseRegistrationFieldDeclareAuthorityRetour.Id);
                    }
                }

                var purchaseRegistrationFieldSerialCodeImage = await _lookup_purchaseRegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.SerialCodeImage).FirstOrDefaultAsync();
                if (purchaseRegistrationFieldSerialCodeImage != null)
                {
                    if (mappedData.SerialCodeImage != null || serialCodeImage != null)
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, serialCodeImage, purchaseRegistrationId, purchaseRegistrationFieldSerialCodeImage.Id);
                    }
                    else
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, String.Empty, purchaseRegistrationId, purchaseRegistrationFieldSerialCodeImage.Id);
                    }
                }

                var purchaseRegistrationFieldEanCodeImage = await _lookup_purchaseRegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.EanCodeImage).FirstOrDefaultAsync();
                if (purchaseRegistrationFieldEanCodeImage != null)
                {
                    if (mappedData.EanCodeImage != null)
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, eanCodeImage, purchaseRegistrationId, purchaseRegistrationFieldEanCodeImage.Id);
                    }
                    else
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, String.Empty, purchaseRegistrationId, purchaseRegistrationFieldEanCodeImage.Id);
                    }
                }

                var purchaseRegistrationFieldRetourStorageImage = await _lookup_purchaseRegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.RetourStorageImage).FirstOrDefaultAsync();
                if (purchaseRegistrationFieldRetourStorageImage != null)
                {
                    if (mappedData.RetourStorageImage != null)
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, retourStorageImage, purchaseRegistrationId, purchaseRegistrationFieldRetourStorageImage.Id);
                    }
                    else
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, String.Empty, purchaseRegistrationId, purchaseRegistrationFieldRetourStorageImage.Id);
                    }
                }

                var purchaseRegistrationFieldCashBackProduct = await _lookup_purchaseRegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.CashBackProduct).FirstOrDefaultAsync();
                if (purchaseRegistrationFieldCashBackProduct != null)
                {
                    if (mappedData.CashBackProduct != null)
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, mappedData.CashBackProduct.ListValueTranslationKeyValue.ToString(), purchaseRegistrationId, purchaseRegistrationFieldCashBackProduct.Id);
                    }
                    else
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, String.Empty, purchaseRegistrationId, purchaseRegistrationFieldCashBackProduct.Id);
                    }
                }

                var purchaseRegistrationFieldCashBackPuppyKittenProduct = await _lookup_purchaseRegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.CashBackPuppyKittenProduct).FirstOrDefaultAsync();
                if (purchaseRegistrationFieldCashBackPuppyKittenProduct != null)
                {
                    if (mappedData.CashBackPuppyKittenProduct != null)
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, mappedData.CashBackPuppyKittenProduct.ListValueTranslationKeyValue.ToString(), purchaseRegistrationId, purchaseRegistrationFieldCashBackPuppyKittenProduct.Id);
                    }
                    else
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, String.Empty, purchaseRegistrationId, purchaseRegistrationFieldCashBackPuppyKittenProduct.Id);
                    }
                }

                var purchaseRegistrationFieldSerialNumber = await _lookup_purchaseRegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.SerialNumber).FirstOrDefaultAsync();
                if (purchaseRegistrationFieldSerialNumber != null)
                {
                    if (serialNumber != null)
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, serialNumber, purchaseRegistrationId, purchaseRegistrationFieldSerialNumber.Id);
                    }
                    else
                    {
                        await InsertPurchaseRegistrationFieldData(tenantId, String.Empty, purchaseRegistrationId, purchaseRegistrationFieldSerialNumber.Id);
                    }
                }
            }

            //HISTORY
            var remarks = "Pending";
            await InsertRegistrationHistory(tenantId, registrationId, registrationMapper.RegistrationStatusId, westTime, 1, remarks);

            var registrationJsonData = new RegistrationJsonData.RegistrationJsonData
            {
                Data = vueJsToRmsModel.Data,
                DateCreated = westTime,
                TenantId = tenantId,
                RegistrationId = registrationId
            };

            var registrationJsonDataId = await _lookup_registrationJsonDataRepository.InsertAndGetIdAsync(registrationJsonData);

            //MESSAGING
            try
            {
                await ComposeRegistrationStatusMessaging(registrationId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        public async Task<bool> UpdateFormData(string blobStorage, string blobContainer, UpdateRegistrationDataDto updateModel)
        {
            var tenantId = AbpSession.GetTenantId();
            var mappedData = JsonConvert.DeserializeObject<UpdateRegistrationDataDto>(updateModel.Data);

            DateTime nowUtc = DateTime.UtcNow;
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            DateTime westTime = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, cstZone);

            var registration = await _registrationRepository.GetAsync(mappedData.RegistrationId);
            var purchaseRegistrations = await _lookup_purchaseRegistrationRepository.GetAll().Where(fl => fl.RegistrationId == mappedData.RegistrationId).ToListAsync();
            var campaign = await _lookup_campaignRepository.GetAsync(registration.CampaignId);

            //UPLOAD VARIOUS IMAGES (if available)
            string invoiceImage = string.Empty;
            if (mappedData.InvoiceImagePath != null && mappedData.InvoiceImagePath.Count > 0)
            {
                var image = mappedData.InvoiceImagePath.ToList()[0];
                var blobServiceClient = new BlobServiceClient(blobStorage);
                BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainer);

                var fileExtension = string.Empty;
                int index = image.IndexOf('/') + 1;
                while (image.Substring(index, 1) != ";")
                {
                    fileExtension += image.Substring(index, 1);
                    index += 1;
                }
                fileExtension = fileExtension.Replace("+xml", "");
                var fileContent = image.Split("base64,")[1];
                invoiceImage = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{registration.Id}-{Guid.NewGuid()}.{fileExtension}";
                blobContainerClient.UploadBlob(invoiceImage, new MemoryStream(Convert.FromBase64String(fileContent)));
            }

            string serialCodeImage = string.Empty;
            if (mappedData.SerialCodeImage != null && mappedData.SerialCodeImage.Count > 0)
            {
                var image = mappedData.SerialCodeImage.ToList()[0];
                var blobServiceClient = new BlobServiceClient(blobStorage);
                BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainer);

                var fileExtension = string.Empty;
                int index = image.IndexOf('/') + 1;
                while (image.Substring(index, 1) != ";")
                {
                    fileExtension += image.Substring(index, 1);
                    index += 1;
                }
                fileExtension = fileExtension.Replace("+xml", "");
                var fileContent = image.Split("base64,")[1];
                serialCodeImage = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{registration.Id}-{Guid.NewGuid()}.{fileExtension}";
                blobContainerClient.UploadBlob(serialCodeImage, new MemoryStream(Convert.FromBase64String(fileContent)));
            }

            string eanCodeImage = string.Empty;
            if (mappedData.EanCodeImage != null && mappedData.EanCodeImage.Count > 0)
            {
                var image = mappedData.EanCodeImage.ToList()[0];
                var blobServiceClient = new BlobServiceClient(blobStorage);
                BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainer);

                var fileExtension = string.Empty;
                int index = image.IndexOf('/') + 1;
                while (image.Substring(index, 1) != ";")
                {
                    fileExtension += image.Substring(index, 1);
                    index += 1;
                }
                fileExtension = fileExtension.Replace("+xml", "");
                var fileContent = image.Split("base64,")[1];
                eanCodeImage = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{registration.Id}-{Guid.NewGuid()}.{fileExtension}";
                blobContainerClient.UploadBlob(eanCodeImage, new MemoryStream(Convert.FromBase64String(fileContent)));
            }

            string retourStorageImage = string.Empty;
            if (mappedData.RetourStorageImage != null && mappedData.RetourStorageImage.Count > 0)
            {
                var image = mappedData.RetourStorageImage.ToList()[0];
                var blobServiceClient = new BlobServiceClient(blobStorage);
                BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainer);

                var fileExtension = string.Empty;
                int index = image.IndexOf('/') + 1;
                while (image.Substring(index, 1) != ";")
                {
                    fileExtension += image.Substring(index, 1);
                    index += 1;
                }
                fileExtension = fileExtension.Replace("+xml", "");
                var fileContent = image.Split("base64,")[1];
                retourStorageImage = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{registration.Id}-{Guid.NewGuid()}.{fileExtension}";
                blobContainerClient.UploadBlob(retourStorageImage, new MemoryStream(Convert.FromBase64String(fileContent)));
            }

            //PROCESS INCOMPLETE FIELDS
            var incompleteFields = registration.IncompleteFields;
            if (incompleteFields == null)
            {
                return false;
            }

            var incompleteList = incompleteFields.Split(',').ToList();

            foreach (var incompleteField in incompleteList)
            {
                var formField = await _lookup_formFieldRepository.GetAll().Where(ff => ff.FieldName == incompleteField).FirstOrDefaultAsync();
                var fieldType = await _lookup_fieldTypeRepository.GetAll().Where(ft => ft.Id == formField.FieldTypeId).FirstOrDefaultAsync();
                var formBlockField = await _lookup_formBlockFieldRepository.GetAll().Where(fbf => fbf.FormFieldId == formField.Id).FirstOrDefaultAsync();
                var formBlock = await _lookup_formBlockRepository.GetAll().Where(fb => fb.Id == formBlockField.FormBlockId).FirstOrDefaultAsync();

                var customRegistrationField = await _lookup_RegistrationFieldRepository.FirstOrDefaultAsync(rf => rf.FormFieldId == formField.Id);
                var customPurchaseRegistrationField = await _lookup_purchaseRegistrationFieldRepository.FirstOrDefaultAsync(prf => prf.FormFieldId == formField.Id);

                if (customRegistrationField != null)
                {
                    var customRegistrationFieldData = await _lookup_registrationFieldDataRepository.FirstOrDefaultAsync(rfd => rfd.RegistrationFieldId == customRegistrationField.Id && rfd.RegistrationId == registration.Id);

                    if (customRegistrationFieldData == null)
                    {
                        await InsertRegistrationFieldData(tenantId, String.Empty, customRegistrationField.Id, registration.Id);
                        customRegistrationFieldData = await _lookup_registrationFieldDataRepository.FirstOrDefaultAsync(rfd => rfd.RegistrationFieldId == customRegistrationField.Id && rfd.RegistrationId == registration.Id);
                    }

                    if (formField.FieldName == FieldNameHelper.UniqueCode && !String.IsNullOrWhiteSpace(mappedData.UniqueCode))
                    {
                        customRegistrationFieldData.Value = mappedData.UniqueCode;
                    }
                    else if (formField.FieldName == FieldNameHelper.UniqueCodeByCampaign && !String.IsNullOrWhiteSpace(mappedData.UniqueCodeByCampaign))
                    {
                        customRegistrationFieldData.Value = mappedData.UniqueCodeByCampaign;
                    }
                    else if (formField.FieldName == FieldNameHelper.LegalForm && !String.IsNullOrWhiteSpace(mappedData.LegalForm))
                    {
                        customRegistrationFieldData.Value = mappedData.LegalForm;
                    }
                    else if (formField.FieldName == FieldNameHelper.BusinessNumber && !String.IsNullOrWhiteSpace(mappedData.BusinessNumber))
                    {
                        customRegistrationFieldData.Value = mappedData.BusinessNumber;
                    }
                    else if (formField.FieldName == FieldNameHelper.VatNumber && !String.IsNullOrWhiteSpace(mappedData.VatNumber))
                    {
                        customRegistrationFieldData.Value = mappedData.VatNumber;
                    }
                    else if (formField.FieldName == FieldNameHelper.StoreNumber && !String.IsNullOrWhiteSpace(mappedData.StoreNumber))
                    {
                        customRegistrationFieldData.Value = mappedData.StoreNumber;
                    }
                    else if (formField.FieldName == FieldNameHelper.StoreName && !String.IsNullOrWhiteSpace(mappedData.StoreName))
                    {
                        customRegistrationFieldData.Value = mappedData.StoreName;
                    }
                    else if (formField.FieldName == FieldNameHelper.DeclareAuthority && mappedData.DeclareAuthority != null)
                    {
                        customRegistrationFieldData.Value = mappedData.DeclareAuthority.ListValueTranslationKeyValue;
                    }
                    else if (formField.FieldName == FieldNameHelper.DeclareAuthorityRetour && mappedData.DeclareAuthorityRetour != null)
                    {
                        customRegistrationFieldData.Value = mappedData.DeclareAuthorityRetour.ListValueTranslationKeyValue;
                    }
                    else if (formField.FieldName == FieldNameHelper.Newsletter && !String.IsNullOrWhiteSpace(mappedData.Newsletter))
                    {
                        customRegistrationFieldData.Value = mappedData.Newsletter;
                    }
                    else if (formField.FieldName == FieldNameHelper.Privacy && !String.IsNullOrWhiteSpace(mappedData.Privacy))
                    {
                        customRegistrationFieldData.Value = mappedData.Privacy;
                    }
                    else if (formField.FieldName == FieldNameHelper.Policy && !String.IsNullOrWhiteSpace(mappedData.Policy))
                    {
                        customRegistrationFieldData.Value = mappedData.Policy;
                    }
                    else if (formField.FieldName == FieldNameHelper.PolicyDreft && !String.IsNullOrWhiteSpace(mappedData.PolicyDreft))
                    {
                        customRegistrationFieldData.Value = mappedData.PolicyDreft;
                    }
                    else if (formField.FieldName == FieldNameHelper.PolicyAriel && !String.IsNullOrWhiteSpace(mappedData.PolicyAriel))
                    {
                        customRegistrationFieldData.Value = mappedData.PolicyAriel;
                    }
                    else if (formField.FieldName == FieldNameHelper.PolicyNoFrost && !String.IsNullOrWhiteSpace(mappedData.PolicyNoFrost))
                    {
                        customRegistrationFieldData.Value = mappedData.PolicyNoFrost;
                    }
                }
                else if (customPurchaseRegistrationField != null)
                {
                    foreach (var purchaseRegistration in purchaseRegistrations)
                    {
                        var customPurchaseRegistrationFieldData = await _lookup_purchaseRegistrationFieldDataRepository.FirstOrDefaultAsync(prfd => prfd.PurchaseRegistrationFieldId == customPurchaseRegistrationField.Id && prfd.PurchaseRegistrationId == purchaseRegistration.Id);

                        if (customPurchaseRegistrationFieldData == null)
                        {
                            await InsertPurchaseRegistrationFieldData(tenantId, String.Empty, purchaseRegistration.Id, customPurchaseRegistrationField.Id);
                            customPurchaseRegistrationFieldData = await _lookup_purchaseRegistrationFieldDataRepository.FirstOrDefaultAsync(prfd => prfd.PurchaseRegistrationFieldId == customPurchaseRegistrationField.Id && prfd.PurchaseRegistrationId == purchaseRegistration.Id);
                        }

                        if (formField.FieldName == FieldNameHelper.AlreadyCustomer && mappedData.AlreadyCustomer != null)
                        {
                            customPurchaseRegistrationFieldData.Value = mappedData.AlreadyCustomer.ListValueTranslationKeyValue;
                        }
                        else if (formField.FieldName == FieldNameHelper.CustomerReturnReason && mappedData.CustomorReturnReason != null)
                        {
                            customPurchaseRegistrationFieldData.Value = mappedData.CustomorReturnReason.ListValueTranslationKeyValue;
                        }
                        else if (formField.FieldName == FieldNameHelper.DeviceReturnReason && mappedData.DeviceReturnReason != null)
                        {
                            customPurchaseRegistrationFieldData.Value = mappedData.DeviceReturnReason.ListValueTranslationKeyValue;
                        }
                        else if (formField.FieldName == FieldNameHelper.EpenReturnReason && mappedData.EpenReturnReason != null)
                        {
                            customPurchaseRegistrationFieldData.Value = mappedData.EpenReturnReason.ListValueTranslationKeyValue;
                        }
                        else if (formField.FieldName == FieldNameHelper.EpodReturnReason && mappedData.EpodReturnReason != null)
                        {
                            customPurchaseRegistrationFieldData.Value = mappedData.EpodReturnReason.ListValueTranslationKeyValue;
                        }
                        else if (formField.FieldName == FieldNameHelper.OtherReason && !String.IsNullOrWhiteSpace(mappedData.OtherReason))
                        {
                            customPurchaseRegistrationFieldData.Value = mappedData.OtherReason;
                        }
                        else if (formField.FieldName == FieldNameHelper.StoreNumber && !String.IsNullOrWhiteSpace(mappedData.StoreNumber))
                        {
                            customPurchaseRegistrationFieldData.Value = mappedData.StoreNumber;
                        }
                        else if (formField.FieldName == FieldNameHelper.StoreName && !String.IsNullOrWhiteSpace(mappedData.StoreName))
                        {
                            customPurchaseRegistrationFieldData.Value = mappedData.StoreName;
                        }
                        else if (formField.FieldName == FieldNameHelper.PlaceNameStore && !String.IsNullOrWhiteSpace(mappedData.PlaceNameStore))
                        {
                            customPurchaseRegistrationFieldData.Value = mappedData.PlaceNameStore;
                        }
                        else if (formField.FieldName == FieldNameHelper.StoreNotAvailable && !String.IsNullOrWhiteSpace(mappedData.StoreNotAvalible))
                        {
                            customPurchaseRegistrationFieldData.Value = mappedData.StoreNotAvalible;
                        }
                        else if (formField.FieldName == FieldNameHelper.DogProduct && mappedData.DogProduct != null)
                        {
                            customPurchaseRegistrationFieldData.Value = mappedData.DogProduct.ListValueTranslationKeyValue;
                        }
                        else if (formField.FieldName == FieldNameHelper.CatProduct && mappedData.CatProduct != null)
                        {
                            customPurchaseRegistrationFieldData.Value = mappedData.CatProduct.ListValueTranslationKeyValue;
                        }
                        else if (formField.FieldName == FieldNameHelper.DeclareAuthority && mappedData.DeclareAuthority != null)
                        {
                            customPurchaseRegistrationFieldData.Value = mappedData.DeclareAuthority.ListValueTranslationKeyValue;
                        }
                        else if (formField.FieldName == FieldNameHelper.DeclareAuthorityRetour && mappedData.DeclareAuthorityRetour != null)
                        {
                            customPurchaseRegistrationFieldData.Value = mappedData.DeclareAuthorityRetour.ListValueTranslationKeyValue;
                        }
                        else if (formField.FieldName == FieldNameHelper.SerialNumber && !String.IsNullOrWhiteSpace(mappedData.SerialNumber))
                        {
                            customPurchaseRegistrationFieldData.Value = mappedData.SerialNumber;
                        }
                        else if (formField.FieldName == FieldNameHelper.SerialCodeImage && !String.IsNullOrWhiteSpace(serialCodeImage))
                        {
                            customPurchaseRegistrationFieldData.Value = serialCodeImage;
                        }
                        else if (formField.FieldName == FieldNameHelper.EanCodeImage && !String.IsNullOrWhiteSpace(eanCodeImage))
                        {
                            customPurchaseRegistrationFieldData.Value = eanCodeImage;
                        }
                        else if (formField.FieldName == FieldNameHelper.RetourStorageImage && !String.IsNullOrWhiteSpace(retourStorageImage))
                        {
                            customPurchaseRegistrationFieldData.Value = retourStorageImage;
                        }
                    }
                }
                else
                {
                    if (formField.FieldName == FieldNameHelper.CompanyName && !String.IsNullOrWhiteSpace(mappedData.CompanyName)) { registration.CompanyName = mappedData.CompanyName; }
                    else if (formField.FieldName == FieldNameHelper.Gender && !String.IsNullOrWhiteSpace(mappedData.Gender?.ListValueTranslationKeyValue)) { registration.Gender = mappedData.Gender?.ListValueTranslationKeyValue; }
                    else if (formField.FieldName == FieldNameHelper.FirstName && !String.IsNullOrWhiteSpace(mappedData.FirstName)) { registration.FirstName = mappedData.FirstName; }
                    else if (formField.FieldName == FieldNameHelper.LastName && !String.IsNullOrWhiteSpace(mappedData.LastName)) { registration.LastName = mappedData.LastName; }
                    else if (formField.FieldName == FieldNameHelper.ZipCode && !String.IsNullOrWhiteSpace(mappedData.ZipCode)) { registration.PostalCode = mappedData.ZipCode; }
                    else if (formField.FieldName == FieldNameHelper.StreetName && !String.IsNullOrWhiteSpace(mappedData.StreetName)) { registration.Street = mappedData.StreetName; }
                    else if (formField.FieldName == FieldNameHelper.HouseNumber && !String.IsNullOrWhiteSpace(mappedData.HouseNumber)) { registration.HouseNr = mappedData.HouseNumber; }
                    else if (formField.FieldName == FieldNameHelper.Residence && !String.IsNullOrWhiteSpace(mappedData.Residence)) { registration.City = mappedData.Residence; }
                    else if (formField.FieldName == FieldNameHelper.EmailAddress && !String.IsNullOrWhiteSpace(mappedData.EmailAddress)) { registration.EmailAddress = mappedData.EmailAddress; }
                    else if (formField.FieldName == FieldNameHelper.PhoneNumber && !String.IsNullOrWhiteSpace(mappedData.PhoneNumber)) { registration.PhoneNumber = mappedData.PhoneNumber; }
                    else if (formField.FieldName == FieldNameHelper.IbanChecker && mappedData.IbanChecker != null) { registration.Iban = mappedData.IbanChecker.Iban; registration.Bic = mappedData.IbanChecker.Bic; }
                    else if (formField.FieldName == FieldNameHelper.Country && !String.IsNullOrWhiteSpace(mappedData.CountryId))
                    {
                        var country = await _lookup_countryRepository.GetAsync(Convert.ToInt64(mappedData.CountryId));

                        if (country != null)
                        {
                            registration.CountryId = country.Id;
                        }
                    }

                    int purchaseLineIndex = 0;

                    foreach (var purchaseRegistration in purchaseRegistrations)
                    {
                        if (formField.FieldName == FieldNameHelper.Quantity && !String.IsNullOrWhiteSpace(mappedData.Quantity))
                        {
                            purchaseRegistration.Quantity = Convert.ToInt32(mappedData.Quantity);
                        }
                        else if (formField.FieldName == FieldNameHelper.TotalAmount && !String.IsNullOrWhiteSpace(mappedData.TotalAmount))
                        {
                            purchaseRegistration.TotalAmount = Convert.ToDecimal(mappedData.TotalAmount);
                        }
                        else if (formField.FieldName == FieldNameHelper.PurchaseDate && !String.IsNullOrWhiteSpace(mappedData.PurchaseDate))
                        {
                            purchaseRegistration.PurchaseDate = Convert.ToDateTime(mappedData.PurchaseDate,
                                        System.Globalization.CultureInfo.GetCultureInfo("nl-NL").DateTimeFormat);
                        }
                        else if (formField.FieldName == FieldNameHelper.InvoiceImagePath && !String.IsNullOrWhiteSpace(invoiceImage))
                        {
                            purchaseRegistration.InvoiceImagePath = invoiceImage;
                        }
                        else if ((formField.FieldName == FieldNameHelper.Product && !String.IsNullOrWhiteSpace(mappedData.Product))
                              || (formField.FieldName == FieldNameHelper.ProductPremium && mappedData.ProductPremium != null && mappedData.ProductPremium.Count > 0)
                              || (formField.FieldName == FieldNameHelper.ProductPremiumLite && mappedData.ProductPremiumLite != null && mappedData.ProductPremiumLite.Count > 0)
                              || (formField.FieldName == FieldNameHelper.ProductPremiumQuantity && mappedData.ProductPremiumQuantity != null && mappedData.ProductPremiumQuantity.Count > 0))
                        {
                            long? productId;
                            long? handlinglineId;

                            if (!String.IsNullOrWhiteSpace(mappedData.Product))
                            {
                                var product = await _lookup_productRepository.GetAsync(Convert.ToInt64(mappedData.Product));
                                var productHandling = await _productHandlingRepository.GetAll().FirstOrDefaultAsync(x => x.CampaignId == campaign.Id);
                                var handlingLines = await _lookup_handlingLineRepository.GetAllListAsync(x => x.ProductHandlingId == productHandling.Id);
                                var handlingLineIds = handlingLines.Select(x => x.Id).ToList();
                                var handlingLineProduct = await _lookup_handlingLineProductRepository.GetAll().Where(x => handlingLineIds.Contains(x.HandlingLineId) && x.ProductId == product.Id).FirstOrDefaultAsync();

                                if (handlingLineProduct != null)
                                {
                                    handlinglineId = handlingLineProduct.HandlingLineId;
                                }
                                else
                                {
                                    if (handlingLines.Where(l => l.CustomerCode.ToUpper().Trim() != "UNKNOWN").Count() == 1)
                                    {
                                        //only 1 Premium is linked to this campaign, so take that one
                                        handlinglineId = handlingLines.Where(l => l.CustomerCode.ToUpper().Trim() != "UNKNOWN").First().Id;
                                    }
                                    else
                                    {
                                        //more than 1 Premiums are linked to this campaign, so link it to the UNKNOWN
                                        handlinglineId = handlingLines.Where(l => l.CustomerCode.ToUpper().Trim() == "UNKNOWN").First().Id;
                                    }
                                }

                                productId = product.Id;
                            }
                            else if (mappedData.ProductPremium != null)
                            {
                                var index = mappedData.ProductPremium.Count > purchaseLineIndex ? purchaseLineIndex : mappedData.ProductPremium.Count - 1;
                                var product = await _lookup_productRepository.GetAsync(Convert.ToInt64(mappedData.ProductPremium[index].ChosenItemId));

                                productId = product.Id;
                                handlinglineId = Convert.ToInt64(mappedData.ProductPremium[index].HandlingLineId);
                            }
                            else if (mappedData.ProductPremiumLite != null)
                            {
                                var index = mappedData.ProductPremiumLite.Count > purchaseLineIndex ? purchaseLineIndex : mappedData.ProductPremiumLite.Count - 1;
                                var product = await _lookup_productRepository.GetAsync(Convert.ToInt64(mappedData.ProductPremiumLite[index].ChosenItemId));

                                productId = product.Id;
                                handlinglineId = Convert.ToInt64(mappedData.ProductPremiumLite[index].HandlingLineId);
                            }
                            else 
                            {
                                var index = mappedData.ProductPremiumQuantity.Count > purchaseLineIndex ? purchaseLineIndex : mappedData.ProductPremiumQuantity.Count - 1;
                                var product = await _lookup_productRepository.GetAsync(Convert.ToInt64(mappedData.ProductPremiumQuantity[index].ProductPremium[0].ChosenItemId));

                                productId = product.Id;
                                handlinglineId = Convert.ToInt64(mappedData.ProductPremiumQuantity[index].ProductPremium[0].HandlingLineId);

                                if (!String.IsNullOrWhiteSpace(mappedData.ProductPremiumQuantity[index].HandlingLineQuantity))
                                {
                                    purchaseRegistration.Quantity = Convert.ToInt32(mappedData.ProductPremiumQuantity[index].HandlingLineQuantity);
                                }
                            }

                            if (productId.HasValue)
                            {
                                purchaseRegistration.ProductId = productId.Value;
                            }

                            if (handlinglineId.HasValue)
                            {
                                purchaseRegistration.HandlingLineId = handlinglineId.Value;
                            }
                        }
                        else if (formField.FieldName == FieldNameHelper.StorePurchased)
                        {
                            long? retailerLocationId = !String.IsNullOrWhiteSpace(mappedData.StorePurchased) ? (long?)Convert.ToInt64(mappedData.StorePurchased) : (mappedData.StorePicker != null && !String.IsNullOrWhiteSpace(mappedData.StorePicker.RetailerLocationId)) ? (long?)Convert.ToInt64(mappedData.StorePicker.RetailerLocationId) : null;
                            if (retailerLocationId.HasValue)
                            {
                                var retailerLocations = await _retailersAppService.GetAllRetailersForCampaign(campaign.Id);
                                if (retailerLocations.Items.Any(rl => rl.RetailerLocation.Id == retailerLocationId.Value))
                                {
                                    purchaseRegistration.RetailerLocationId = retailerLocationId.Value;
                                }
                            }
                        }
                        else if (formField.FieldName == FieldNameHelper.PurchaseRegistration)
                        {
                            long? retailerLocationId = !String.IsNullOrWhiteSpace(mappedData.PurchaseRegistration[purchaseLineIndex].StorePurchased) ? (long?)Convert.ToInt64(mappedData.PurchaseRegistration[purchaseLineIndex].StorePurchased) : null;
                            if (retailerLocationId.HasValue)
                            {
                                var retailerLocations = await _retailersAppService.GetAllRetailersForCampaign(campaign.Id);
                                if (retailerLocations.Items.Any(rl => rl.RetailerLocation.Id == retailerLocationId.Value))
                                {
                                    purchaseRegistration.RetailerLocationId = retailerLocationId.Value;
                                }
                            }

                            long? productId;
                            long? handlinglineId;
                            if (mappedData.PurchaseRegistration[purchaseLineIndex].ProductPremium != null && mappedData.PurchaseRegistration[purchaseLineIndex].ProductPremium.Count > 0)
                            {
                                var index = mappedData.PurchaseRegistration[purchaseLineIndex].ProductPremium.Count > purchaseLineIndex ? purchaseLineIndex : mappedData.PurchaseRegistration[purchaseLineIndex].ProductPremium.Count - 1;
                                var product = await _lookup_productRepository.GetAsync(Convert.ToInt64(mappedData.PurchaseRegistration[purchaseLineIndex].ProductPremium[index].ChosenItemId));

                                productId = product.Id;
                                handlinglineId = Convert.ToInt64(mappedData.PurchaseRegistration[purchaseLineIndex].ProductPremium[index].HandlingLineId);


                                if (productId.HasValue)
                                {
                                    purchaseRegistration.ProductId = productId.Value;
                                }

                                if (handlinglineId.HasValue)
                                {
                                    purchaseRegistration.HandlingLineId = handlinglineId.Value;
                                }
                            }

                            if (mappedData.PurchaseRegistration[purchaseLineIndex].InvoiceImage != null && mappedData.PurchaseRegistration[purchaseLineIndex].InvoiceImage.Count > 0)
                            {
                                var image = mappedData.PurchaseRegistration[purchaseLineIndex].InvoiceImage.ToList()[0];
                                var blobServiceClient = new BlobServiceClient(blobStorage);
                                BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainer);

                                var fileExtension = string.Empty;
                                int index = image.IndexOf('/') + 1;
                                while (image.Substring(index, 1) != ";")
                                {
                                    fileExtension += image.Substring(index, 1);
                                    index += 1;
                                }
                                fileExtension = fileExtension.Replace("+xml", "");
                                var fileContent = image.Split("base64,")[1];
                                invoiceImage = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{registration.Id}-{Guid.NewGuid()}.{fileExtension}";
                                blobContainerClient.UploadBlob(invoiceImage, new MemoryStream(Convert.FromBase64String(fileContent)));

                                purchaseRegistration.InvoiceImagePath = invoiceImage;
                            }

                            if (mappedData.PurchaseRegistration[purchaseLineIndex].PurchaseDate != null)
                            {
                                purchaseRegistration.PurchaseDate = Convert.ToDateTime(mappedData.PurchaseRegistration[purchaseLineIndex].PurchaseDate,
                                        System.Globalization.CultureInfo.GetCultureInfo("nl-NL").DateTimeFormat);
                            }

                            if (mappedData.PurchaseRegistration[purchaseLineIndex].HandlingLineQuantity != null)
                            {
                                purchaseRegistration.Quantity = Convert.ToInt32(mappedData.PurchaseRegistration[purchaseLineIndex].HandlingLineQuantity);
                            }

                            if (mappedData.PurchaseRegistration[purchaseLineIndex].TotalAmount != null)
                            {
                                purchaseRegistration.TotalAmount = Convert.ToDecimal(mappedData.PurchaseRegistration[purchaseLineIndex].TotalAmount);
                            }
                        }
                        else if (formField.FieldName == FieldNameHelper.PurchaseRegistrationLite)
                        {
                            long? retailerLocationId = !String.IsNullOrWhiteSpace(mappedData.PurchaseRegistrationLite[purchaseLineIndex].StorePurchased) ? (long?)Convert.ToInt64(mappedData.PurchaseRegistrationLite[purchaseLineIndex].StorePurchased) : null;
                            if (retailerLocationId.HasValue)
                            {
                                var retailerLocations = await _retailersAppService.GetAllRetailersForCampaign(campaign.Id);
                                if (retailerLocations.Items.Any(rl => rl.RetailerLocation.Id == retailerLocationId.Value))
                                {
                                    purchaseRegistration.RetailerLocationId = retailerLocationId.Value;
                                }
                            }

                            long? productId;
                            long? handlinglineId;
                            if (mappedData.PurchaseRegistrationLite[purchaseLineIndex].ProductPremium != null && mappedData.PurchaseRegistrationLite[purchaseLineIndex].ProductPremium.Count > 0)
                            {
                                var index = mappedData.PurchaseRegistrationLite[purchaseLineIndex].ProductPremium.Count > purchaseLineIndex ? purchaseLineIndex : mappedData.PurchaseRegistrationLite[purchaseLineIndex].ProductPremium.Count - 1;
                                var product = await _lookup_productRepository.GetAsync(Convert.ToInt64(mappedData.PurchaseRegistrationLite[purchaseLineIndex].ProductPremium[index].ChosenItemId));

                                productId = product.Id;
                                handlinglineId = Convert.ToInt64(mappedData.PurchaseRegistrationLite[purchaseLineIndex].ProductPremium[index].HandlingLineId);


                                if (productId.HasValue)
                                {
                                    purchaseRegistration.ProductId = productId.Value;
                                }

                                if (handlinglineId.HasValue)
                                {
                                    purchaseRegistration.HandlingLineId = handlinglineId.Value;
                                }
                            }

                            if (mappedData.PurchaseRegistrationLite[purchaseLineIndex].InvoiceImage != null && mappedData.PurchaseRegistrationLite[purchaseLineIndex].InvoiceImage.Count > 0)
                            {
                                var image = mappedData.PurchaseRegistrationLite[purchaseLineIndex].InvoiceImage.ToList()[0];
                                var blobServiceClient = new BlobServiceClient(blobStorage);
                                BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainer);

                                var fileExtension = string.Empty;
                                int index = image.IndexOf('/') + 1;
                                while (image.Substring(index, 1) != ";")
                                {
                                    fileExtension += image.Substring(index, 1);
                                    index += 1;
                                }
                                fileExtension = fileExtension.Replace("+xml", "");
                                var fileContent = image.Split("base64,")[1];
                                invoiceImage = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{registration.Id}-{Guid.NewGuid()}.{fileExtension}";
                                blobContainerClient.UploadBlob(invoiceImage, new MemoryStream(Convert.FromBase64String(fileContent)));

                                purchaseRegistration.InvoiceImagePath = invoiceImage;
                            }

                            if (mappedData.PurchaseRegistrationLite[purchaseLineIndex].PurchaseDate != null)
                            {
                                purchaseRegistration.PurchaseDate = Convert.ToDateTime(mappedData.PurchaseRegistrationLite[purchaseLineIndex].PurchaseDate,
                                        System.Globalization.CultureInfo.GetCultureInfo("nl-NL").DateTimeFormat);
                            }

                            if (mappedData.PurchaseRegistrationLite[purchaseLineIndex].HandlingLineQuantity != null)
                            {
                                purchaseRegistration.Quantity = Convert.ToInt32(mappedData.PurchaseRegistrationLite[purchaseLineIndex].HandlingLineQuantity);
                            }
                        }
                        else if (formField.FieldName == FieldNameHelper.PurchaseRegistrationSerial)
                        {
                            long? retailerLocationId = !String.IsNullOrWhiteSpace(mappedData.PurchaseRegistrationSerial[purchaseLineIndex].StorePurchased) ? (long?)Convert.ToInt64(mappedData.PurchaseRegistrationSerial[purchaseLineIndex].StorePurchased) : null;
                            if (retailerLocationId.HasValue)
                            {
                                var retailerLocations = await _retailersAppService.GetAllRetailersForCampaign(campaign.Id);
                                if (retailerLocations.Items.Any(rl => rl.RetailerLocation.Id == retailerLocationId.Value))
                                {
                                    purchaseRegistration.RetailerLocationId = retailerLocationId.Value;
                                }
                            }

                            long? productId;
                            long? handlinglineId;
                            if (mappedData.PurchaseRegistrationSerial[purchaseLineIndex].ProductPremium != null && mappedData.PurchaseRegistrationSerial[purchaseLineIndex].ProductPremium.Count > 0)
                            {
                                var index = mappedData.PurchaseRegistrationSerial[purchaseLineIndex].ProductPremium.Count > purchaseLineIndex ? purchaseLineIndex : mappedData.PurchaseRegistrationSerial[purchaseLineIndex].ProductPremium.Count - 1;
                                var product = await _lookup_productRepository.GetAsync(Convert.ToInt64(mappedData.PurchaseRegistrationSerial[purchaseLineIndex].ProductPremium[index].ChosenItemId));

                                productId = product.Id;
                                handlinglineId = Convert.ToInt64(mappedData.PurchaseRegistrationSerial[purchaseLineIndex].ProductPremium[index].HandlingLineId);


                                if (productId.HasValue)
                                {
                                    purchaseRegistration.ProductId = productId.Value;
                                }

                                if (handlinglineId.HasValue)
                                {
                                    purchaseRegistration.HandlingLineId = handlinglineId.Value;
                                }
                            }

                            if (mappedData.PurchaseRegistrationSerial[purchaseLineIndex].InvoiceImage != null && mappedData.PurchaseRegistrationSerial[purchaseLineIndex].InvoiceImage.Count > 0)
                            {
                                var image = mappedData.PurchaseRegistrationSerial[purchaseLineIndex].InvoiceImage.ToList()[0];
                                var blobServiceClient = new BlobServiceClient(blobStorage);
                                BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainer);

                                var fileExtension = string.Empty;
                                int index = image.IndexOf('/') + 1;
                                while (image.Substring(index, 1) != ";")
                                {
                                    fileExtension += image.Substring(index, 1);
                                    index += 1;
                                }
                                fileExtension = fileExtension.Replace("+xml", "");
                                var fileContent = image.Split("base64,")[1];
                                invoiceImage = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{registration.Id}-{Guid.NewGuid()}.{fileExtension}";
                                blobContainerClient.UploadBlob(invoiceImage, new MemoryStream(Convert.FromBase64String(fileContent)));

                                purchaseRegistration.InvoiceImagePath = invoiceImage;
                            }

                            if (mappedData.PurchaseRegistrationSerial[purchaseLineIndex].PurchaseDate != null)
                            {
                                purchaseRegistration.PurchaseDate = Convert.ToDateTime(mappedData.PurchaseRegistrationSerial[purchaseLineIndex].PurchaseDate,
                                        System.Globalization.CultureInfo.GetCultureInfo("nl-NL").DateTimeFormat);
                            }

                            if (mappedData.PurchaseRegistrationSerial[purchaseLineIndex].HandlingLineQuantity != null)
                            {
                                purchaseRegistration.Quantity = Convert.ToInt32(mappedData.PurchaseRegistrationSerial[purchaseLineIndex].HandlingLineQuantity);
                            }

                            var purchaseRegistrationFieldSerialNumber = await _lookup_purchaseRegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.SerialNumber).FirstOrDefaultAsync();
                            if (purchaseRegistrationFieldSerialNumber != null)
                            {
                                var purchaseRegistrationFieldSerialNumberData = await _lookup_purchaseRegistrationFieldDataRepository.GetAll().Where(rf => rf.PurchaseRegistrationId == purchaseRegistration.Id && rf.PurchaseRegistrationFieldId == purchaseRegistrationFieldSerialNumber.Id).FirstOrDefaultAsync();
                                if (purchaseRegistrationFieldSerialNumberData != null)
                                {
                                    purchaseRegistrationFieldSerialNumberData.Value = mappedData.PurchaseRegistrationSerial[purchaseLineIndex].SerialNumber;
                                }
                            }

                            var purchaseRegistrationFieldSerialCodeImage = await _lookup_purchaseRegistrationFieldRepository.GetAll().Where(rf => rf.Description == FieldNameHelper.SerialCodeImage).FirstOrDefaultAsync();
                            if (purchaseRegistrationFieldSerialCodeImage != null)
                            {
                                var purchaseRegistrationFieldSerialCodeImageData = await _lookup_purchaseRegistrationFieldDataRepository.GetAll().Where(rf => rf.PurchaseRegistrationId == purchaseRegistration.Id && rf.PurchaseRegistrationFieldId == purchaseRegistrationFieldSerialCodeImage.Id).FirstOrDefaultAsync();
                                if (purchaseRegistrationFieldSerialCodeImageData != null && mappedData.PurchaseRegistrationSerial[purchaseLineIndex].SerialImage.Count > 0)
                                {
                                    var image = mappedData.PurchaseRegistrationSerial[purchaseLineIndex].SerialImage.ToList()[0];
                                    var blobServiceClient = new BlobServiceClient(blobStorage);
                                    BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainer);

                                    var fileExtension = string.Empty;
                                    int index = image.IndexOf('/') + 1;
                                    while (image.Substring(index, 1) != ";")
                                    {
                                        fileExtension += image.Substring(index, 1);
                                        index += 1;
                                    }
                                    fileExtension = fileExtension.Replace("+xml", "");
                                    var fileContent = image.Split("base64,")[1];
                                    serialCodeImage = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{registration.Id}-{Guid.NewGuid()}.{fileExtension}";
                                    blobContainerClient.UploadBlob(serialCodeImage, new MemoryStream(Convert.FromBase64String(fileContent)));


                                    purchaseRegistrationFieldSerialCodeImageData.Value = serialCodeImage;
                                }
                            }

                        }

                        purchaseLineIndex += 1;
                    }
                }
            }

            var registrationStatusPending = await _registrationStatusesAppService.GetByStatusCode(RegistrationStatusCodeHelper.Pending);
            var remarks = "Updated";

            registration.RegistrationStatusId = registrationStatusPending.RegistrationStatus.Id;
            registration.IncompleteFields = null;
            registration.Password = null;
            registration.RejectionReasonId = null;

            await InsertRegistrationHistory(tenantId, mappedData.RegistrationId, registrationStatusPending.RegistrationStatus.Id, westTime, 1, remarks);

            return true;
        }

        private async Task<bool> InsertRegistrationFieldData(int tenantId, string value, long registrationFieldId, long registrationId)
        {
            try
            {
                await _lookup_registrationFieldDataRepository.InsertAsync(new RegistrationFieldData
                {
                    TenantId = tenantId,
                    Value = value,
                    RegistrationFieldId = registrationFieldId,
                    RegistrationId = registrationId
                });

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(InsertRegistrationFieldData)}: {ex.Message}. StackTrace: {ex.StackTrace}");

                return false;
            }
        }

        private async Task<bool> InsertPurchaseRegistrationFieldData(int tenantId, string value, long purchaseRegistrationId, long purchaseRegistrationFieldId)
        {
            try
            {
                await _lookup_purchaseRegistrationFieldDataRepository.InsertAsync(new PurchaseRegistrationFieldData
                {
                    TenantId = tenantId,
                    Value = value,
                    PurchaseRegistrationId = purchaseRegistrationId,
                    PurchaseRegistrationFieldId = purchaseRegistrationFieldId,
                });

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(InsertPurchaseRegistrationFieldData)}: {ex.Message}. StackTrace: {ex.StackTrace}");

                return false;
            }
        }

        public static string MoveImageToCloud(int year, int month, int day, long registrationId, string fileDbContent)
        {
            string blobPath;
            try
            {
                var fileExtension = String.Empty;

                int index = fileDbContent.IndexOf('/') + 1;
                while (fileDbContent.Substring(index, 1) != ";")
                {
                    fileExtension += fileDbContent.Substring(index, 1);
                    index += 1;
                }
                fileExtension = fileExtension.Replace("+xml", "");
                var fileContent = fileDbContent.Split("base64,")[1];
                blobPath = $"{year}/{month}/{day}/{registrationId}-{Guid.NewGuid()}.{fileExtension}";

                //BlobServiceClient blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=storsbjrms1;AccountKey=gE77biXf4071zkS4U3KNV/HJ2G3b0ldLu/v+nGeY/ZpB/8NSZ+HJ5Rk2Hm1iEGU7GLOmqJ6w3ICilw51MMfEYQ==;EndpointSuffix=core.windows.net;");
                BlobServiceClient blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=storsbjrms2;AccountKey=tMQ6E8fHvAStOJtW6/YyNDCJHXfKzgwQJy2PJnvM6nYrcOIZvpRZgwr4VH8VNR5k7nu3RARcEZHqu7XqGND71w==;EndpointSuffix=core.windows.net");
                BlobContainerClient blobContainer = blobServiceClient.GetBlobContainerClient("philips");

                blobContainer.UploadBlob(blobPath, new MemoryStream(Convert.FromBase64String(fileContent)));
            }
            catch
            {
                blobPath = String.Empty;
            }

            return blobPath;
        }

        private async Task<bool> InsertRegistrationHistory(int tenantId, long registrationId, long registrationStatusId, DateTime dateCreated, long abpUserId, string remarks)
        {
            try
            {
                await _lookup_registrationHistoryRepository.InsertAsync(new RegistrationHistory.RegistrationHistory
                {
                    TenantId = tenantId,
                    RegistrationId = registrationId,
                    RegistrationStatusId = registrationStatusId,
                    DateCreated = dateCreated,
                    AbpUserId = abpUserId,
                    Remarks = remarks,
                });

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(InsertRegistrationHistory)}: {ex.Message}. StackTrace: {ex.StackTrace}");

                return false;
            }
        }

        private async Task<List<GetRelatedRegistrationsForViewOutput>> GetRelatedRegistrationsBySerialNumber(long registrationId)
        {
            var relatedSerialNumbers = (from prf in _lookup_purchaseRegistrationFieldRepository.GetAll()
                                        join o1 in _lookup_purchaseRegistrationFieldDataRepository.GetAll() on prf.Id equals o1.PurchaseRegistrationFieldId into j1
                                        from prfd in j1
                                        join o2 in _lookup_purchaseRegistrationRepository.GetAll() on prfd.PurchaseRegistrationId equals o2.Id into j2
                                        from pr in j2
                                        join o3 in _registrationRepository.GetAll() on pr.RegistrationId equals o3.Id into j3
                                        from r in j3
                                        where prf.Description == "SerialNumber"
                                        && r.Id == registrationId
                                        && prfd.Value != null
                                        && prfd.Value != String.Empty
                                        select prfd.Value);

            var relatedRegistrationsBySerialNumber = await (from prfd in _lookup_purchaseRegistrationFieldDataRepository.GetAll()
                                                            join o1 in _lookup_purchaseRegistrationRepository.GetAll() on prfd.PurchaseRegistrationId equals o1.Id into j1
                                                            from pr in j1
                                                            join o2 in _registrationRepository.GetAll() on pr.RegistrationId equals o2.Id into j2
                                                            from r in j2
                                                            where relatedSerialNumbers.Contains(prfd.Value)
                                                            && r.Id != registrationId
                                                            select new
                                                            {
                                                                RegistrationStatusId = (ulong)r.RegistrationStatusId,
                                                                RegistrationId = (ulong)r.Id,
                                                                SerialNumber = prfd.Value,
                                                                ProductId = pr.ProductId
                                                            }).ToListAsync();

            var projectedRelatedRegistrationsBySerialNumber = new List<GetRelatedRegistrationsForViewOutput>();

            foreach (var relatedRegistration in relatedRegistrationsBySerialNumber)
            {
                var projectedRelatedRegistrationBySerialNumber = new GetRelatedRegistrationsForViewOutput()
                {
                    StatusId = (long)relatedRegistration.RegistrationStatusId,
                    RegistrationId = relatedRegistration.RegistrationId,
                    SerialNumber = relatedRegistration.SerialNumber
                };

                var relatedRegistrationHistory = await _lookup_registrationHistoryRepository.FirstOrDefaultAsync(x => (ulong)x.RegistrationId == relatedRegistration.RegistrationId);
                if (relatedRegistrationHistory != null)
                {
                    projectedRelatedRegistrationBySerialNumber.DateCreated = relatedRegistrationHistory.DateCreated;
                }

                var relatedProduct = await _lookup_productRepository.FirstOrDefaultAsync(product => product.Id == relatedRegistration.ProductId);
                if (relatedProduct != null)
                {
                    projectedRelatedRegistrationBySerialNumber.ProductCode = relatedProduct.ProductCode;
                    projectedRelatedRegistrationBySerialNumber.ProductDescription = relatedProduct.Description;
                }

                var relatedStatus = await _lookup_registrationStatusRepository.FirstOrDefaultAsync(x => (long)x.Id == (long)relatedRegistration.RegistrationStatusId);
                if (relatedStatus != null)
                {
                    projectedRelatedRegistrationBySerialNumber.StatusCode = relatedStatus.StatusCode;
                    projectedRelatedRegistrationBySerialNumber.StatusDescription = relatedStatus.Description;
                }

                    projectedRelatedRegistrationsBySerialNumber.Add(projectedRelatedRegistrationBySerialNumber);
            }

            return projectedRelatedRegistrationsBySerialNumber;
        }

        private async Task<List<GetRelatedRegistrationsForViewOutput>> GetRelatedRegistrationsByEmailAddress(long registrationId, string registrationEmailAddress)
        {
            //this combination of information, because there could be a family who use the same e-mail address and postal code, but don't share the same names
            //should therefore be a separate registration if the names are different, no?
            var relatedRegistrationsByEmail = await _registrationRepository.GetAllListAsync(x => x.Id != registrationId
                                                       && !String.IsNullOrWhiteSpace(x.EmailAddress) && !String.IsNullOrWhiteSpace(registrationEmailAddress)
                                                       && x.EmailAddress.ToLower().Trim() == registrationEmailAddress.ToLower().Trim());

            var projectedRelatedRegistrationsByEmail = relatedRegistrationsByEmail.Select(x => new GetRelatedRegistrationsForViewOutput
            {   StatusId = x.RegistrationStatusId,
                RegistrationId = (ulong)x.Id,
                DateCreated = null
            }).ToList();

            foreach (var relatedRegistration in projectedRelatedRegistrationsByEmail)
            {
                var relatedRegistrationHistory = await _lookup_registrationHistoryRepository.FirstOrDefaultAsync(x => (ulong)x.RegistrationId == relatedRegistration.RegistrationId);
                if (relatedRegistrationHistory != null)
                {
                    relatedRegistration.DateCreated = relatedRegistrationHistory.DateCreated;
                }

                var relatedProductRegistration = await _lookup_purchaseRegistrationRepository.GetAll().Include(e => e.ProductFk).FirstOrDefaultAsync(x => (ulong)x.RegistrationId == relatedRegistration.RegistrationId);
                if (relatedProductRegistration == null)
                {
                    continue;
                }

                var relatedProduct = await _lookup_productRepository.FirstOrDefaultAsync(product => product.Id == relatedProductRegistration.ProductId);
                if (relatedProduct != null)
                {
                    relatedRegistration.ProductCode = relatedProduct.ProductCode;
                    relatedRegistration.ProductDescription = relatedProduct.Description;
                }

                var relatedStatus = await _lookup_registrationStatusRepository.FirstOrDefaultAsync(x => (long)x.Id == (long)relatedRegistration.StatusId);
                if (relatedStatus != null)
                {
                    relatedRegistration.StatusCode = relatedStatus.StatusCode;
                    relatedRegistration.StatusDescription = relatedStatus.Description;
                }

            }

            return projectedRelatedRegistrationsByEmail;
        }

        [AbpAuthorize(AppPermissions.Pages_Registrations_Edit)]
        protected virtual async Task Update(CreateOrEditRegistrationDto input)
        {
            var registration = await _registrationRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, registration);
        }

        [AbpAuthorize(AppPermissions.Pages_Registrations_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _registrationRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetRegistrationsToExcel(GetAllRegistrationsForExcelInput input)
        {
            var filteredRegistrations = _registrationRepository.GetAll()
                .Include(e => e.RegistrationStatusFk)
                .Include(e => e.CampaignFormFk)
                .Include(e => e.LocaleFk)
                .WhereIf(!String.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.FirstName.Contains(input.Filter) || e.LastName.Contains(input.Filter) ||
                         e.Street.Contains(input.Filter) || e.HouseNr.Contains(input.Filter) ||
                         e.PostalCode.Contains(input.Filter) || e.City.Contains(input.Filter) ||
                         e.EmailAddress.Contains(input.Filter) || e.PhoneNumber.Contains(input.Filter) ||
                         e.CompanyName.Contains(input.Filter) || e.Gender.Contains(input.Filter))
                .WhereIf(!String.IsNullOrWhiteSpace(input.FirstNameFilter), e => e.FirstName == input.FirstNameFilter)
                .WhereIf(!String.IsNullOrWhiteSpace(input.LastNameFilter), e => e.LastName == input.LastNameFilter)
                .WhereIf(!String.IsNullOrWhiteSpace(input.StreetFilter), e => e.Street == input.StreetFilter)
                .WhereIf(!String.IsNullOrWhiteSpace(input.HouseNrFilter), e => e.HouseNr == input.HouseNrFilter)
                .WhereIf(!String.IsNullOrWhiteSpace(input.PostalCodeFilter),
                    e => e.PostalCode == input.PostalCodeFilter)
                .WhereIf(!String.IsNullOrWhiteSpace(input.CityFilter), e => e.City == input.CityFilter)
                .WhereIf(!String.IsNullOrWhiteSpace(input.EmailAddressFilter),
                    e => e.EmailAddress == input.EmailAddressFilter)
                .WhereIf(!String.IsNullOrWhiteSpace(input.PhoneNumberFilter),
                    e => e.PhoneNumber == input.PhoneNumberFilter)
                .WhereIf(!String.IsNullOrWhiteSpace(input.CompanyNameFilter),
                    e => e.CompanyName == input.CompanyNameFilter)
                .WhereIf(!String.IsNullOrWhiteSpace(input.GenderFilter), e => e.Gender == input.GenderFilter)
                .WhereIf(input.MinCountryIdFilter != null, e => e.CountryId >= input.MinCountryIdFilter)
                .WhereIf(input.MaxCountryIdFilter != null, e => e.CountryId <= input.MaxCountryIdFilter)
                .WhereIf(input.MinCampaignIdFilter != null, e => e.CampaignId >= input.MinCampaignIdFilter)
                .WhereIf(input.MaxCampaignIdFilter != null, e => e.CampaignId <= input.MaxCampaignIdFilter)
                .WhereIf(!String.IsNullOrWhiteSpace(input.RegistrationStatusStatusCodeFilter),
                    e => e.RegistrationStatusFk != null &&
                         e.RegistrationStatusFk.StatusCode == input.RegistrationStatusStatusCodeFilter);

            var query = from o in filteredRegistrations
                        join o1 in _lookup_registrationStatusRepository.GetAll() on o.RegistrationStatusId equals o1.Id into j1
                        from s1 in j1.DefaultIfEmpty()
                        select new GetRegistrationForViewDto
                        {
                            Registration = new RegistrationDto
                            {
                                FirstName = o.FirstName,
                                LastName = o.LastName,
                                Street = o.Street,
                                HouseNr = o.HouseNr,
                                PostalCode = o.PostalCode,
                                City = o.City,
                                EmailAddress = o.EmailAddress,
                                PhoneNumber = o.PhoneNumber,
                                CompanyName = o.CompanyName,
                                Gender = o.Gender,
                                CountryId = o.CountryId,
                                CampaignId = o.CampaignId,
                                Id = o.Id
                            },
                            RegistrationStatusStatusCode = s1 == null || s1.StatusCode == null ? "" : s1.StatusCode.ToString()
                        };

            var registrationListDto = await query.ToListAsync();

            return _registrationsExcelExporter.ExportToFile(registrationListDto);
        }

        [AbpAuthorize(AppPermissions.Pages_Registrations)]
        public async Task<PagedResultDto<RegistrationRegistrationStatusLookupTableDto>> GetAllRegistrationStatusForLookupTable(RMS.SBJ.Registrations.Dtos.GetAllForLookupTableInput input)
        {
            var query = _lookup_registrationStatusRepository.GetAll().WhereIf(
                   !String.IsNullOrWhiteSpace(input.Filter),
                  e => e.StatusCode != null && e.StatusCode.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var registrationStatusList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<RegistrationRegistrationStatusLookupTableDto>();
            foreach (var registrationStatus in registrationStatusList)
            {
                lookupTableDtoList.Add(new RegistrationRegistrationStatusLookupTableDto
                {
                    Id = registrationStatus.Id,
                    DisplayName = registrationStatus.StatusCode?.ToString()
                });
            }

            return new PagedResultDto<RegistrationRegistrationStatusLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Registrations)]
        public async Task<List<RegistrationFormLocaleLookupTableDto>> GetAllFormLocaleForTableDropdown()
        {
            return await _lookup_formLocaleRepository.GetAll()
                .Select(formLocale => new RegistrationFormLocaleLookupTableDto
                {
                    Id = formLocale.Id,
                    DisplayName = formLocale == null || formLocale.Description == null ? "" : formLocale.Description.ToString()
                }).ToListAsync();
        }

        public async Task ComposeRegistrationStatusMessaging(long registrationId)
        {
            try
            {
                var companyName = _companyRepository.GetAllList().Select(company => company.Name).LastOrDefault();

                var messageVariables = await _lookup_messageVariableRepository.GetAllListAsync();

                var contents = new StringBuilder();
                var messageSubject = String.Empty;
                var registrationEntity = await _registrationRepository.GetAsync(registrationId);
                var registration = ObjectMapper.Map<RegistrationDto>(registrationEntity);

                var registrationStatusEntity = await _lookup_registrationStatusRepository.FirstOrDefaultAsync(registration.RegistrationStatusId);
                var registrationStatus = ObjectMapper.Map<RegistrationStatusDto>(registrationStatusEntity);

                var campaignForm = await _lookup_campaignFormRepository.GetAsync(registration.CampaignFormId.Value);
                var campaign = await _lookup_campaignRepository.GetAsync(campaignForm.CampaignId);

                DateTime nowUtc = DateTime.UtcNow;
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
                DateTime westTime = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, cstZone);

                #region Campaign Campaign Type
                var selectedCampaignCampaignTypesEntity = await _lookup_campaignCampaignTypeRepository.GetAll().Where(campaignType => campaignType.CampaignId == registration.CampaignId).ToListAsync();
                var selectedCampaignCampaignTypesModel = ObjectMapper.Map<List<CampaignCampaignTypeDto>>(selectedCampaignCampaignTypesEntity);
                #endregion

                var campaignMessage = await _lookup_campaignMessageRepository.GetAll().Where(campaignMessage => campaignMessage.CampaignId == registration.CampaignId).FirstOrDefaultAsync();
                MessageType messageType = null;

                if (campaignMessage != null)
                {
                    messageType = await _lookup_messageTypeRepository.GetAll().Where(messageType => messageType.MessageId == campaignMessage.MessageId).FirstOrDefaultAsync();
                }


                #region Campaign Type Event
                var campaignTypeEvents = new List<CampaignTypeEventDto>();
                foreach (var selectedCampaignCampaignTypeItem in selectedCampaignCampaignTypesModel)
                {
                    var campaignTypeEventsEntities = await _lookup_campaignTypeEventRepository.GetAllListAsync();
                    var campaignTypeEventsEntity = campaignTypeEventsEntities.Where(item => item.CampaignTypeId == selectedCampaignCampaignTypeItem.CampaignTypeId)
                                                        .OrderBy(item => item.CampaignTypeId).ThenBy(item => item.SortOrder).ToList();
                    var campaignTypeEventsModel = ObjectMapper.Map<List<CampaignTypeEventDto>>(campaignTypeEventsEntity);

                    campaignTypeEvents.AddRange(campaignTypeEventsModel);
                }
                #endregion

                #region Campaign Type Event Registration Status
                var campaignTypeEventRegistrationStatuses = new List<CampaignTypeEventRegistrationStatusDto>();
                foreach (var campaignTypeEvent in campaignTypeEvents)
                {
                    var campaignTypeEventRegistrationStatusEntity = await _lookup_campaignTypeEventRegistrationStatusRepository.GetAll().Where(item => item.CampaignTypeEventId == campaignTypeEvent.Id && item.RegistrationStatusId == registration.RegistrationStatusId).ToListAsync();
                    var campaignTypeEventRegistrationStatusModel = ObjectMapper.Map<List<CampaignTypeEventRegistrationStatusDto>>(campaignTypeEventRegistrationStatusEntity);
                    campaignTypeEventRegistrationStatuses.AddRange(campaignTypeEventRegistrationStatusModel);
                }
                #endregion

                #region MessageComponentContent
                var messageComponentContents = new List<MessageComponentContentDto>();
                var emailMessageComponentContents = new List<MessageComponentContentDto>();
                foreach (var campaignTypeEventRegistrationStatus in campaignTypeEventRegistrationStatuses)
                {
                    if (messageType != null)
                    {
                        var getMessageComponents = _lookup_messageComponentRepository.GetAll().Where(component => component.MessageTypeId == messageType.Id).ToList();

                        foreach (var messageComponentItem in getMessageComponents)
                        {
                            var messageComponentContentEntity = await _lookup_messageComponentContentRepository.GetAll().Where(item => item.CampaignTypeEventRegistrationStatusId == campaignTypeEventRegistrationStatus.Id && item.MessageComponentId == messageComponentItem.Id).ToListAsync();
                            var messageComponentContentModel = ObjectMapper.Map<List<MessageComponentContentDto>>(messageComponentContentEntity);

                            messageComponentContents.AddRange(messageComponentContentModel);
                        }
                    }
                    else
                    {
                        var messageComponentContentEntity = await _lookup_messageComponentContentRepository.GetAll().Where(item => item.CampaignTypeEventRegistrationStatusId == campaignTypeEventRegistrationStatus.Id).ToListAsync();
                        var messageComponentContentModel = ObjectMapper.Map<List<MessageComponentContentDto>>(messageComponentContentEntity);

                        messageComponentContents.AddRange(messageComponentContentModel);
                    }
                }

                foreach (var messageComponentContent in messageComponentContents)
                {
                    var linkedMessageComponent = _lookup_messageComponentRepository.GetAll()
                        .Include(e => e.MessageTypeFk)
                        .Include(e => e.MessageComponentTypeFk)
                        .ToList()
                        .Where(e => e.Id == messageComponentContent.MessageComponentId)
                        .LastOrDefault();
                    messageComponentContent.MessageType = linkedMessageComponent?.MessageTypeFk.Name;
                    messageComponentContent.MessageComponentType = linkedMessageComponent?.MessageComponentTypeFk.Name;

                    if (messageComponentContent.MessageType.ToLower() == "mail")
                    {
                        emailMessageComponentContents.Add(messageComponentContent);
                    }
                }
                #endregion

                #region Compose Email Content
                if (registrationStatus.StatusCode == RegistrationStatusCodeHelper.Accepted || registrationStatus.StatusCode == RegistrationStatusCodeHelper.Rejected ||
                    registrationStatus.StatusCode == RegistrationStatusCodeHelper.InProgress || registrationStatus.StatusCode == RegistrationStatusCodeHelper.Pending ||
                    registrationStatus.StatusCode == RegistrationStatusCodeHelper.Incomplete || registrationStatus.StatusCode == RegistrationStatusCodeHelper.Send)
                {
                    foreach (var emailMessageContent in emailMessageComponentContents)
                    {
                        var messageContentTranslationEntity = _lookup_messageContentTranslationRepository.GetAllList().Where(item => item.MessageComponentContentId == emailMessageContent.Id && item.LocaleId == registration.LocaleId).LastOrDefault();
                        var messageContentTranslationModel = ObjectMapper.Map<MessageContentTranslationDto>(messageContentTranslationEntity);
                        if (messageContentTranslationModel?.Content != null)//Populate translated contents
                        {
                            foreach (var mvd in messageVariables)
                            {
                                if (messageContentTranslationModel.Content.Contains(mvd.Description))
                                {
                                    switch (mvd.RmsTable)
                                    {
                                        case nameof(Registration):
                                            {
                                                if (mvd.Description.Contains("Incomplete"))
                                                {
                                                    var formFieldList = new List<FormFieldDto>();
                                                    var formFieldTranslation = new StringBuilder();
                                                    var registrationIncompleteFields = registration.IncompleteFields.Split(",");
                                                    foreach (var registrationIncompleteField in registrationIncompleteFields)
                                                    {
                                                        var allFormFields = _lookup_formFieldRepository.GetAllList()
                                                            .Where(x => x.FieldName.ToLower() == registrationIncompleteField.ToLower())
                                                            .Select(y => new FormFieldDto { Id = y.Id }).LastOrDefault();
                                                        formFieldList.Add(allFormFields);
                                                    }
                                                    foreach (var formFieldItem in formFieldList)
                                                    {
                                                        var formFieldTranslations = _lookup_formFieldTranslationRepository.GetAllList()
                                                            .Where(x => x.FormFieldId == formFieldItem.Id && x.LocaleId == registration.LocaleId)
                                                            .Select(y => y.Label).LastOrDefault();
                                                        if (formFieldTranslations != null)
                                                        {
                                                            formFieldTranslation.Append("<br />" + formFieldTranslations);
                                                        }
                                                        else
                                                        {
                                                            formFieldTranslation.Append("<br />" + formFieldItem.Label);
                                                        }
                                                    }
                                                    messageContentTranslationModel.Content = messageContentTranslationModel.Content
                                                        .Replace($"#{{{mvd.Description}}}", formFieldTranslation?.ToString());
                                                }
                                                else if (mvd.Description.Contains("Reject"))
                                                {
                                                    var rejectionReason = _rejectionReasonsAppService.GetRejectionReason(registration.RejectionReasonId.Value);
                                                    var rejectionReasonTranslation = _lookup_rejectionReasonTranslationRepository.GetAllList()
                                                        .Where(rrt => rrt.LocaleId == registration.LocaleId && rrt.RejectionReasonId == registration.RejectionReasonId)
                                                        .Select(rrt => rrt.Description).LastOrDefault();
                                                    messageContentTranslationModel.Content = rejectionReasonTranslation == null ?
                                                                                                messageContentTranslationModel.Content.Replace($"#{{{mvd.Description}}}", rejectionReason?.RejectionReason?.Description)
                                                                                                  : messageContentTranslationModel.Content.Replace($"#{{{mvd.Description}}}", rejectionReasonTranslation);
                                                }
                                                else if (mvd.Description.Contains("CashbackAmount"))
                                                {

                                                    var cashbackAmount = (from o in _lookup_purchaseRegistrationRepository.GetAll()
                                                                          join o1 in _registrationRepository.GetAll() on o.RegistrationId equals o1.Id into j1
                                                                          from s1 in j1.DefaultIfEmpty()
                                                                          join o2 in _lookup_handlingLineRepository.GetAll() on o.HandlingLineId equals o2.Id into j2
                                                                          from s2 in j2.DefaultIfEmpty()
                                                                          where o.RegistrationId == registrationId
                                                                          orderby o.RegistrationId, o.Id
                                                                          select new
                                                                          {
                                                                              PurchaseRegistrationId = o.Id,
                                                                              RefundAmount = s2.Percentage ? Math.Round(o.TotalAmount * (s2.Amount.Value / 100), 2) : s2.Fixed ? Math.Round(s2.Amount.Value, 2) : Math.Round(s2.Amount.Value * o.Quantity, 2)
                                                                          }).ToList();
                                                    var totalCashback = cashbackAmount.Sum(x => x.RefundAmount);
                                                    messageContentTranslationModel.Content = messageContentTranslationModel.Content.Replace($"#{{{mvd.Description}}}", String.Format("{0:.##}", totalCashback));

                                                }
                                                else
                                                {
                                                    messageContentTranslationModel.Content = messageContentTranslationModel.Content
                                                  .Replace($"#{{{mvd.Description}}}", registration
                                                  .GetType()
                                                  .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
                                                  .Where(property => mvd.TableField.Contains(property.Name))
                                                  .Select(property => property.GetValue(registration)).FirstOrDefault().ToString());
                                                }
                                            }
                                            break;
                                        case nameof(Campaign):
                                            {
                                                //get translated value out of the CampaignTranslations table, if available there...
                                                var translatedVariable = _lookup_campaignTranslationRepository.GetAll().Where(t => t.CampaignId == registration.CampaignId && t.LocaleId == registration.LocaleId && t.Name.ToLower().Trim() == mvd.Description.ToLower().Trim()).FirstOrDefault();
                                                if (translatedVariable != null)
                                                    messageContentTranslationModel.Content = messageContentTranslationModel.Content
                                                    .Replace($"#{{{mvd.Description}}}", translatedVariable.Description);
                                                else
                                                    messageContentTranslationModel.Content = messageContentTranslationModel.Content
                                                    .Replace($"#{{{mvd.Description}}}", campaign
                                                    .GetType()
                                                    .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
                                                    .Where(property => mvd.TableField.Contains(property.Name))
                                                    .Select(property => property.GetValue(campaign)).FirstOrDefault().ToString());
                                            }
                                            break;
                                    }
                                }
                            }
                            if (emailMessageContent.MessageComponentType.ToLower() == "subject") //Filtered Subject MessageComponentType as it has to be inserted into a separate column
                            {
                                messageSubject = messageContentTranslationModel.Content;
                            }
                            else
                                contents.Append(messageContentTranslationModel.Content);
                        }
                        else
                        {
                            foreach (var mvd in messageVariables)
                            {
                                if (emailMessageContent.Content.Contains(mvd.Description))
                                {
                                    switch (mvd.RmsTable)
                                    {
                                        case nameof(Registration):
                                            {
                                                if (mvd.Description.Contains("Incomplete"))
                                                {
                                                    var formFieldList = new List<FormFieldDto>();
                                                    var formFieldTranslation = new StringBuilder();
                                                    var registrationIncompleteFields = registration.IncompleteFields.Split(",");
                                                    foreach (var registrationIncompleteField in registrationIncompleteFields)
                                                    {
                                                        var allFormFields = _lookup_formFieldRepository.GetAllList()
                                                            .Where(x => x.FieldName.ToLower() == registrationIncompleteField.ToLower())
                                                            .Select(y => new FormFieldDto { Id = y.Id }).LastOrDefault();
                                                        formFieldList.Add(allFormFields);
                                                    }
                                                    foreach (var formFieldItem in formFieldList)
                                                    {
                                                        var formFieldTranslations = _lookup_formFieldTranslationRepository.GetAllList()
                                                            .Where(x => x.FormFieldId == formFieldItem.Id && x.LocaleId == registration.LocaleId)
                                                            .Select(y => y.Label).LastOrDefault();
                                                        if (formFieldTranslations != null)
                                                        {
                                                            formFieldTranslation.Append("<br />" + formFieldTranslations);
                                                        }
                                                        else
                                                        {
                                                            formFieldTranslation.Append("<br />" + formFieldItem.Label);
                                                        }
                                                    }
                                                    emailMessageContent.Content = emailMessageContent.Content
                                                        .Replace($"#{{{mvd.Description}}}", formFieldTranslation?.ToString());
                                                }
                                                else if (mvd.Description.Contains("Reject"))
                                                {
                                                    var rejectionReason = _rejectionReasonsAppService.GetRejectionReason(registration.RejectionReasonId.Value);
                                                    var rejectionReasonTranslation = _lookup_rejectionReasonTranslationRepository.GetAllList()
                                                        .Where(rrt => rrt.LocaleId == registration.LocaleId && rrt.RejectionReasonId == registration.RejectionReasonId)
                                                        .Select(rrt => rrt.Description).LastOrDefault();
                                                    emailMessageContent.Content = rejectionReasonTranslation == null ?
                                                                                                emailMessageContent.Content.Replace($"#{{{mvd.Description}}}", rejectionReason?.RejectionReason?.Description)
                                                                                                  : emailMessageContent.Content.Replace($"#{{{mvd.Description}}}", rejectionReasonTranslation);
                                                }
                                                else if (mvd.Description.Contains("CashbackAmount"))
                                                {
                                                    var cashbackAmount = (from o in _lookup_purchaseRegistrationRepository.GetAll()
                                                                          join o1 in _registrationRepository.GetAll() on o.RegistrationId equals o1.Id into j1
                                                                          from s1 in j1.DefaultIfEmpty()
                                                                          join o2 in _lookup_handlingLineRepository.GetAll() on o.HandlingLineId equals o2.Id into j2
                                                                          from s2 in j2.DefaultIfEmpty()
                                                                          where o.RegistrationId == registrationId
                                                                          orderby o.RegistrationId, o.Id
                                                                          select new
                                                                          {
                                                                              PurchaseRegistrationId = o.Id,
                                                                              RefundAmount = s2.Percentage ? Math.Round(o.TotalAmount * (s2.Amount.Value / 100), 2) : s2.Fixed ? Math.Round(s2.Amount.Value, 2) : Math.Round(s2.Amount.Value * o.Quantity, 2)
                                                                          }).ToList();

                                                    var totalCashback = cashbackAmount.Sum(x => x.RefundAmount);
                                                    messageContentTranslationModel.Content = messageContentTranslationModel.Content.Replace($"#{{{mvd.Description}}}", String.Format("{0:.##}", totalCashback));
                                                }
                                                else
                                                {
                                                    emailMessageContent.Content = emailMessageContent.Content
                                                    .Replace($"#{{{mvd.Description}}}", registration
                                                    .GetType()
                                                    .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
                                                    .Where(property => mvd.TableField.Contains(property.Name))
                                                    .Select(property => property.GetValue(registration)).FirstOrDefault().ToString());
                                                }
                                            }
                                            break;
                                        case nameof(Campaign):
                                            //get translated value out of the CampaignTranslations table, if available there...
                                            var translatedVariable = _lookup_campaignTranslationRepository.GetAll().Where(t => t.CampaignId == registration.CampaignId && t.LocaleId == registration.LocaleId && t.Name.ToLower().Trim() == mvd.Description.ToLower().Trim()).FirstOrDefault();

                                            if (translatedVariable != null)
                                            {
                                                emailMessageContent.Content = emailMessageContent.Content
                                                .Replace($"#{{{mvd.Description}}}", translatedVariable.Description);
                                            }
                                            else
                                            {
                                                emailMessageContent.Content = emailMessageContent.Content
                                                .Replace($"#{{{mvd.Description}}}", campaign
                                                .GetType()
                                                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
                                                .Where(property => mvd.TableField.Contains(property.Name))
                                                .Select(property => property.GetValue(campaign)).FirstOrDefault().ToString());
                                            }

                                            break;
                                    }
                                }
                            }
                            if (emailMessageContent.MessageComponentType.ToLower() == "subject")
                            {
                                messageSubject = emailMessageContent.Content;
                            }
                            else
                                contents.Append(emailMessageContent.Content);
                        }
                    }
                }
                #endregion

                #region Messaging DB and MessageHistory
                if (contents != null && contents.Length > 0)
                {
                    var messagingEntity = new MessagesDto
                    {
                        Source = RegistrationConsts.Source,
                        Reference = registration.Id.ToString(),
                        InitiatorReference = RegistrationConsts.InitiatorReference,
                        MessageInfo = RegistrationConsts.MessageInfo,
                        MessageCollectionId = RegistrationConsts.MessageCollectionId,
                        CurrentStepId = RegistrationConsts.CurrentStepId,
                        TemplateId = RegistrationConsts.TemplateId,
                        Subject = messageSubject,
                        DisplayName = companyName,
                        From = RegistrationConsts.From,
                        To = registration.EmailAddress.Trim(),
                        Body = contents.ToString(),
                        ExpirationTime = westTime,
                        Priority = RegistrationConsts.Priority,
                        AwaitingSend = false,
                        SendError = false,
                        Finished = false,
                        CreatedAt = westTime,
                    };
                    var messageId = _messagingAppService.CreateOrEditAndGetId(messagingEntity);
                    await _lookup_messageHistoryRepository.InsertOrUpdateAsync(new MessageHistory
                    {
                        TenantId = AbpSession.TenantId != null ? AbpSession.TenantId : null,
                        RegistrationId = registrationId,
                        AbpUserId = AbpSession.UserId.Value,
                        Content = contents.ToString(),
                        TimeStamp = westTime,
                        MessageName = registrationStatus.Description,
                        MessageId = messageId,
                        Subject = messageSubject,
                        To = registration.EmailAddress
                    });
                }
                #endregion
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //e8IXULKBdS1OfYoSl6PnL3NupJtHokvJGppLEx4L3qc
        [AbpAuthorize(AppPermissions.Pages_Registrations)]
        public async Task<bool> BatAuthentication(FormRegistrationHandlingDto vueJsToRmsModel)
        {
            var token = vueJsToRmsModel.Data;

            try
            {
                //BASE^4 URL Decode
                token = token.Replace("-", "+").Replace("_", "/");
                var base64 = Encoding.ASCII.GetBytes(token);
                var padding = base64.Length * 3 % 4;//(base64.Length*6 % 8)/2
                if (padding != 0)
                {
                    token = token.PadRight(token.Length + padding, '=');
                }

                var encryptedBytes = Convert.FromBase64String(token);
                var plainText = String.Empty;
                var secretKey = Encoding.UTF8.GetBytes(SecretKey);
                var IV = Encoding.UTF8.GetBytes(InitialVector);

                try
                {
                    using (Aes aesAlg = Aes.Create())
                    {
                        aesAlg.BlockSize = 128;
                        aesAlg.Mode = CipherMode.CBC;

                        aesAlg.Key = secretKey;
                        aesAlg.IV = IV;

                        // Create a decryptor to perform the stream transform.
                        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                        // Create the streams used for decryption.
                        using (MemoryStream msDecrypt = new MemoryStream(encryptedBytes))
                        {
                            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                            {
                                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                                {

                                    // Read the decrypted bytes from the decrypting stream
                                    // and place them in a string.
                                    plainText = srDecrypt.ReadToEnd();
                                }
                            }
                        }
                    }

                    //Correct length?
                    if (plainText.Length != 23) return false;

                    //Format code correct?
                    var code = $"{plainText[0]}{plainText[5]}{plainText[10]}{plainText[17]}";
                    if (code != FormatCode) return false;

                    //Time difference nit greater then 2 minutes?
                    var day = int.Parse(plainText.Substring(2, 2));
                    var month = int.Parse(plainText.Substring(7, 2));
                    var year = int.Parse(plainText.Substring(12, 4));
                    var hour = int.Parse(plainText.Substring(19, 2));
                    var minutes = int.Parse(plainText.Substring(21, 2));

                    var momentSent = new DateTime(year, month, day, hour, minutes, 0);
                    var currentMoment = DateTime.UtcNow;

                    var timeDifference = currentMoment - momentSent;
                    if (timeDifference.TotalMinutes > 2) return false;

                    //in all other cases return true
                    return true;

                }
                catch (Exception)
                {
                    //If any exception; decryption, parsing dates etc. always return false
                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}