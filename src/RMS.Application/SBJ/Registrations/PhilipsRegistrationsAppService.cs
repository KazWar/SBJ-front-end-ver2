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
using RMS.SBJ.Messaging;
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

namespace RMS.SBJ.Registrations
{
    [AbpAuthorize(AppPermissions.Pages_Registrations)]
    public class PhilipsRegistrationsAppService : RMSAppServiceBase, IPhilipsRegistrationsAppService
    {
        private readonly ILogger<RegistrationsAppService> _logger;
        private readonly IRepository<Registration, long> _registrationRepository;
        private readonly IRepository<CodeTypeTables.CampaignType, long> _campaignTypeRepository;
        private readonly IRepository<RegistrationStatus, long> _lookup_registrationStatusRepository;
        private readonly IRepository<PurchaseRegistration, long> _lookup_purchaseRegistrationRepository;
        private readonly IRepository<RegistrationField, long> _lookup_RegistrationFieldRepository;
        private readonly IRepository<RegistrationFormFieldDatas.RegistrationFieldData, long> _lookup_registrationFieldDataRepository;
        private readonly IRepository<PurchaseRegistrationField, long> _lookup_purchaseRegistrationFieldRepository;
        private readonly IRepository<PurchaseRegistrationFieldData, long> _lookup_purchaseRegistrationFieldDataRepository;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IRepository<Retailer, long> _lookup_retailerRepository;
        private readonly IRepository<RetailerLocation, long> _lookup_retailerLocationRepository;
        private readonly IRepository<Campaign, long> _lookup_campaignRepository;
        private readonly IRepository<CampaignForm, long> _lookup_campaignFormRepository;
        private readonly IRepository<CampaignTranslation, long> _lookup_campaignTranslationRepository;
        private readonly IRepository<FormLocale, long> _lookup_formLocaleRepository;
        private readonly IRepository<Forms.FormBlock, long> _lookup_formBlockRepository;
        private readonly IRepository<FormBlockField, long> _lookup_formBlockFieldRepository;
        private readonly IRepository<Forms.FormField, long> _lookup_formFieldRepository;
        private readonly IRepository<Forms.FormFieldTranslation, long> _lookup_formFieldTranslationRepository;
        private readonly IRepository<FieldType, long> _lookup_fieldTypeRepository;
        private readonly IRepository<FormFieldValueList, long> _lookup_formFieldValueListRepository;
        private readonly IRepository<ListValue, long> _lookup_listValueRepository;
        private readonly IRepository<ListValueTranslation, long> _lookup_listValueTranslationRepository;
        private readonly IRepository<RegistrationHistory.RegistrationHistory, long> _lookup_registrationHistoryRepository;
        private readonly IRegistrationsExcelExporter _registrationsExcelExporter;
        private readonly IRepository<MakitaSerialNumber.MakitaSerialNumber, long> _lookup_makitaSerialNumberRepository;
        private readonly IRepository<CampaignCampaignType, long> _lookup_campaignCampaignTypeRepository;
        private readonly IRepository<CampaignTypeEvent, long> _lookup_campaignTypeEventRepository;
        private readonly IRepository<CampaignTypeEventRegistrationStatus, long> _lookup_campaignTypeEventRegistrationStatusRepository;
        private readonly IRepository<MessageComponentContent, long> _lookup_messageComponentContentRepository;
        private readonly IRepository<MessageContentTranslation, long> _lookup_messageContentTranslationRepository;
        private readonly IRepository<MessageHistory, long> _lookup_messageHistoryRepository;
        private readonly IRepository<MessageVariable, long> _lookup_messageVariableRepository;
        private readonly IRepository<MessageComponent, long> _lookup_messageComponentRepository;
        private readonly IRepository<Company.Company, long> _companyRepository;
        private readonly IRepository<Country, long> _lookup_countryRepositoryRepository;
        private readonly IRepository<RegistrationJsonData.RegistrationJsonData, long> _lookup_registrationJsonDataRepository;
        private readonly IRepository<RegistrationFormFieldDatas.RegistrationFieldData, long> _lookup_registrationFormFieldDataRepository;
        private readonly IRepository<HandlingLineRetailers.HandlingLineRetailer, long> _lookup_handlingLineRetailersRepository;
        private readonly IRepository<Locale, long> _lookup_localeRepository;
        private readonly IRepository<ProductHandling, long> _productHandlingRepository;
        private readonly IRepository<HandlingLine, long> _lookup_handlingLineRepository;
        private readonly IRepository<HandlingLineProduct, long> _lookup_handlingLineProductRepository;
        private readonly IRejectionReasonsAppService _rejectionReasonsAppService;
        private readonly IRepository<RejectionReasonTranslation, long> _lookup_rejectionReasonTranslationRepository;
        private readonly IRegistrationStatusesAppService _registrationStatusesAppService;
        private readonly IRegistrationHistoriesAppService _registrationHistoriesAppService;
        private readonly IUserAppService _userAppService;
        private readonly IProductsAppService _productsAppService;
        private readonly ICountriesAppService _countriesAppService;
        private readonly IMessagingAppService _messagingAppService;
        private readonly IRetailersAppService _retailersAppService;
        private readonly IRegistrationsAppService _registrationsAppService;

        public PhilipsRegistrationsAppService(
            ILogger<RegistrationsAppService> logger,
            IRepository<Registration, long> registrationRepository,
            IRepository<RegistrationStatus, long> lookup_registrationStatusRepository,
            IRepository<PurchaseRegistration, long> lookup_purchaseRegistrationRepository,
            IRepository<RegistrationField, long> lookup_RegistrationFieldRepository,
            IRepository<RegistrationFormFieldDatas.RegistrationFieldData, long> lookup_RegistrationFieldDataRepository,
            IRepository<PurchaseRegistrationField, long> lookup_purchaseRegistrationFieldRepository,
            IRepository<PurchaseRegistrationFieldData, long> lookup_purchaseRegistrationFieldDataRepository,
            IRepository<Product, long> lookup_productRepository,
            IRepository<Retailer, long> lookup_retailerRepository,
            IRepository<RetailerLocation, long> lookup_retailerLocationRepository,
            IRepository<Campaign, long> lookup_campaignRepository,
            IRepository<CampaignForm, long> lookup_campaignFormRepository,
            IRepository<CampaignTranslation, long> lookup_campaignTranslationRepository,
            IRepository<FormLocale, long> lookup_formLocaleRepository,
            IRepository<Forms.FormBlock, long> lookup_formBlockRepository,
            IRepository<FormBlockField, long> lookup_formBlockFieldRepository,
            IRepository<Forms.FormField, long> lookup_formFieldRepository,
            IRepository<Forms.FormFieldTranslation, long> lookup_formFieldTranslationRepository,
            IRepository<FieldType, long> lookup_fieldTypeRepository,
            IRepository<FormFieldValueList, long> lookup_formFieldValueListRepository,
            IRepository<ListValue, long> lookup_listValueRepository,
            IRepository<ListValueTranslation, long> lookup_listValueTranslationRepository,
            IRepository<RegistrationHistory.RegistrationHistory, long> lookup_registrationHistoryRepository,
            IRejectionReasonsAppService rejectionReasonsAppService,
            IRepository<RejectionReasonTranslation, long> lookup_rejectionReasonTranslationRepository,
            IRegistrationStatusesAppService registrationStatusesAppService,
            IRegistrationHistoriesAppService registrationHistoriesAppService,
            IRegistrationsExcelExporter registrationsExcelExporter,
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
            IUserAppService userAppService,
            IProductsAppService productsAppService,
            ICountriesAppService countriesAppService,
            IRepository<Country, long> lookup_countryRepositoryRepository,
            IRepository<RegistrationJsonData.RegistrationJsonData, long> lookup_registrationJsonDataRepository,
            IRepository<HandlingLineRetailers.HandlingLineRetailer, long> lookup_handlingLineRetailersRepository,
            IRepository<RegistrationFieldData, long> lookup_registrationFormFieldDataRepository,
            IRepository<Locale, long> lookup_localeRepository,
            IMessagingAppService messagingAppService,
            IRepository<Company.Company, long> companyRepository,
            IRetailersAppService retailersAppService,
            IRegistrationsAppService registrationsAppService,
        IRepository<CodeTypeTables.CampaignType, long> campaignTypeRepository)
        {
            _logger = logger;
            _registrationsExcelExporter = registrationsExcelExporter;
            _registrationRepository = registrationRepository;
            _lookup_registrationStatusRepository = lookup_registrationStatusRepository;
            _lookup_purchaseRegistrationRepository = lookup_purchaseRegistrationRepository;
            _lookup_RegistrationFieldRepository = lookup_RegistrationFieldRepository;
            _lookup_registrationFieldDataRepository = lookup_RegistrationFieldDataRepository;
            _lookup_purchaseRegistrationFieldRepository = lookup_purchaseRegistrationFieldRepository;
            _lookup_purchaseRegistrationFieldDataRepository = lookup_purchaseRegistrationFieldDataRepository;
            _lookup_productRepository = lookup_productRepository;
            _lookup_retailerRepository = lookup_retailerRepository;
            _lookup_retailerLocationRepository = lookup_retailerLocationRepository;
            _lookup_campaignRepository = lookup_campaignRepository;
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
            _lookup_countryRepositoryRepository = lookup_countryRepositoryRepository;
            _lookup_registrationJsonDataRepository = lookup_registrationJsonDataRepository;
            _lookup_registrationFormFieldDataRepository = lookup_registrationFormFieldDataRepository;
            _lookup_handlingLineRetailersRepository = lookup_handlingLineRetailersRepository;
            _productHandlingRepository = productHandlingRepository;
            _lookup_localeRepository = lookup_localeRepository;
            _registrationsAppService = registrationsAppService;
            _lookup_handlingLineRepository = lookup_handlingLineRepository;
            _lookup_handlingLineProductRepository = lookup_handlingLineProductRepository;
            _lookup_rejectionReasonTranslationRepository = lookup_rejectionReasonTranslationRepository;
            _rejectionReasonsAppService = rejectionReasonsAppService;
            _registrationStatusesAppService = registrationStatusesAppService;
            _registrationHistoriesAppService = registrationHistoriesAppService;
            _userAppService = userAppService;
            _productsAppService = productsAppService;
            _countriesAppService = countriesAppService;
            _messagingAppService = messagingAppService;
            _companyRepository = companyRepository;
            _retailersAppService = retailersAppService;
            _campaignTypeRepository = campaignTypeRepository;
        }

        public async Task<PagedResultDto<GetRegistrationForViewDto>> GetAll(GetAllRegistrationsInput input)
        {
            // SerialNumber search
            var registrationsList = new List<long>();
            if (!string.IsNullOrWhiteSpace(input.SerialNumberFilter))
            {

                var selectedSerialNumberList = await _lookup_purchaseRegistrationFieldDataRepository.GetAll()
                    .Where(s => s.PurchaseRegistrationFieldId == 3 && s.Value.Contains(input.SerialNumberFilter)).ToListAsync();
                foreach (var serialNumber in selectedSerialNumberList)
                {
                    var selectedRegistrations = _lookup_purchaseRegistrationRepository.GetAll()
                        .Where(pr => pr.Id == serialNumber.PurchaseRegistrationId).Select(s => s.RegistrationId).Distinct();
                    registrationsList.AddRange(selectedRegistrations);
                }
            };

            var filteredRegistrations = _registrationRepository.GetAll()
                .Include(e => e.RegistrationStatusFk)
                .Include(e => e.CampaignFormFk)
                .Include(e => e.LocaleFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.FirstName.Contains(input.Filter) || e.LastName.Contains(input.Filter) || e.Id.ToString().StartsWith(input.Filter) ||
                         e.Street.Contains(input.Filter) || e.HouseNr.Contains(input.Filter) ||
                         e.PostalCode.Contains(input.Filter) || e.City.Contains(input.Filter) ||
                         e.EmailAddress.Contains(input.Filter) || e.PhoneNumber.Contains(input.Filter) ||
                         e.CompanyName.Contains(input.Filter) || e.Gender.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.FirstNameFilter), e => e.FirstName == input.FirstNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.LastNameFilter), e => e.LastName == input.LastNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter), e => e.City == input.CityFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.EmailAddressFilter),
                    e => e.EmailAddress == input.EmailAddressFilter)
                .WhereIf(Convert.ToInt64(input.RegistrationStatusFilter) != -1,
                    e => e.RegistrationStatusId == Convert.ToInt64(input.RegistrationStatusFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SerialNumberFilter),
                    e => registrationsList.Contains(e.Id))
                .WhereIf(!string.IsNullOrWhiteSpace(input.CampaignDescriptionFilter),
                    e => e.CampaignFormFk.CampaignFk.Description.Contains(input.CampaignDescriptionFilter));

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
                                where string.IsNullOrWhiteSpace(input.CampaignDescriptionFilter) || input.CampaignDescriptionFilter != null && input.CampaignDescriptionFilter.Length > 0 && s2.Description.Contains(input.CampaignDescriptionFilter)
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
                                    ProductCode = string.Empty,
                                    CampaignDescription = s2.Description,
                                    RegistrationStatusStatusCode = s1 == null || s1.StatusCode == null ? string.Empty : s1.StatusCode,
                                    DateCreated = s3 == null ? string.Empty : s3.DateCreated.ToString("dd-MM-yyyy HH:mm")
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
            if (input == null || input.RegistrationId <= 0)
            {
                return false;
            }

            var registration = await _registrationRepository.GetAsync(input.RegistrationId);
            if (registration == null)
            {
                return false;
            }

            var purchaseRegistrations = await _lookup_purchaseRegistrationRepository.GetAll()
                .Where(pr => pr.RegistrationId == input.RegistrationId)
                .ToListAsync();

            var rejectedFields = new List<string>();

            foreach (var field in input.FormFields)
            {
                if (field.IsRejected)
                {
                    var formFieldRecord = await _lookup_formFieldRepository.GetAsync(field.FieldId);
                    if (formFieldRecord == null)
                    {
                        break;
                    }

                    var fieldName = formFieldRecord.FieldName;

                    rejectedFields.Add(fieldName);
                }

                switch (field.FieldSource)
                {
                    case FieldSourceHelper.Registration:
                        var registrationField = typeof(Registration).GetProperty(field.RegistrationField);
                        var registrationFieldType = registrationField.PropertyType;

                        object fieldValue;

                        if (registrationFieldType == typeof(int))
                        {
                            fieldValue = Convert.ToInt32(field.FieldValue);
                        }
                        else if (registrationFieldType == typeof(long))
                        {
                            fieldValue = Convert.ToInt64(field.FieldValue);
                        }
                        else if (registrationFieldType == typeof(decimal))
                        {
                            fieldValue = decimal.Parse(field.FieldValue, CultureInfo.InvariantCulture);
                        }
                        else if (registrationFieldType == typeof(DateTime))
                        {
                            fieldValue = Convert.ToDateTime(field.FieldValue);
                        }
                        else
                        {
                            fieldValue = field.FieldValue;
                        }

                        if (field.FieldType != FieldTypeHelper.FileUploader &&
                            field.FieldType != FieldTypeHelper.IbanChecker) // For now, IBAN/BIC remains read-only.
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

                        object purchaseFieldValue;

                        if (purchaseRegistrationFieldType == typeof(int))
                        {
                            purchaseFieldValue = Convert.ToInt32(field.FieldValue);
                        }
                        else if (purchaseRegistrationFieldType == typeof(long))
                        {
                            purchaseFieldValue = Convert.ToInt64(field.FieldValue);
                        }
                        else if (purchaseRegistrationFieldType == typeof(decimal))
                        {
                            purchaseFieldValue = decimal.Parse(field.FieldValue, CultureInfo.InvariantCulture);
                        }
                        else if (purchaseRegistrationFieldType == typeof(DateTime))
                        {
                            purchaseFieldValue = Convert.ToDateTime(field.FieldValue);
                        }
                        else
                        {
                            purchaseFieldValue = field.FieldValue;
                        }

                        if (field.PurchaseRegistrationField == "ProductId" && purchaseRegistration.ProductId != Convert.ToInt64(purchaseFieldValue))
                        {
                            var productHandling = await _productHandlingRepository.GetAll().FirstOrDefaultAsync(x => x.CampaignId == registration.CampaignId);
                            var handlingLines = await _lookup_handlingLineRepository.GetAllListAsync(x => x.ProductHandlingId == productHandling.Id);
                            var handlingLineProducts = await _lookup_handlingLineProductRepository.GetAllListAsync(x => x.ProductId == Convert.ToInt64(purchaseFieldValue));
                            var handlingLinesByHandlingLineProduct = handlingLines.FirstOrDefault(x => handlingLineProducts.Any(y => y.HandlingLineId == x.Id));

                            if (handlingLinesByHandlingLineProduct != null)
                            {
                                purchaseRegistration.HandlingLineId = handlingLinesByHandlingLineProduct.Id;
                            }
                            else
                            {
                                // Link to "unknown" record
                                var unknownHandlingLine = await _lookup_handlingLineRepository.FirstOrDefaultAsync(x => x.ProductHandlingId == productHandling.Id && x.CustomerCode.Trim().ToUpper() == "UNKNOWN");

                                purchaseRegistration.HandlingLineId = unknownHandlingLine.Id;
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

                        if (field.FieldType != FieldTypeHelper.FileUploader)
                        {
                            customRegistrationFieldValue.Value = field.FieldValue;
                        }

                        await _lookup_registrationFieldDataRepository.UpdateAsync(customRegistrationFieldValue);

                        break;
                    case FieldSourceHelper.CustomPurchaseRegistration:
                        var purchaseRegistrationFieldData = _lookup_purchaseRegistrationFieldDataRepository.GetAll()
                            .Where(prfd => prfd.PurchaseRegistrationId == field.FieldLineId);
                        var customPurchaseRegistrationFieldValue = await (from o in purchaseRegistrationFieldData
                                                                            join o1 in _lookup_purchaseRegistrationFieldRepository.GetAll() 
                                                                            on o.PurchaseRegistrationFieldId equals o1.Id
                                                                          where o1.FormFieldId == field.FieldId
                                                                          select o).FirstOrDefaultAsync();

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

            registration.IncompleteFields = string.Empty;
            registration.RejectedFields = string.Empty;

            if (!rejectedFields.Any())
            {
                if (input.IsApproved)
                {
                    var registrationStatusAccepted =
                        await _registrationStatusesAppService.GetByStatusCode(RegistrationStatusCodeHelper.Accepted);

                    registration.RegistrationStatusId = registrationStatusAccepted.RegistrationStatus.Id;
                    registration.Password = null;
                }
                else
                {
                    var currentRegistrationStatus = await _lookup_registrationStatusRepository.GetAsync(registration.RegistrationStatusId);
                    var currentStatusCode = currentRegistrationStatus?.StatusCode;

                    if (currentStatusCode == RegistrationStatusCodeHelper.Accepted ||
                        currentStatusCode == RegistrationStatusCodeHelper.Rejected ||
                        currentStatusCode == RegistrationStatusCodeHelper.Incomplete)
                    {
                        // Otherwise set to pending
                        var registrationStatusPending =
                            await _registrationStatusesAppService.GetByStatusCode(RegistrationStatusCodeHelper.Pending);

                        registration.RegistrationStatusId = registrationStatusPending.RegistrationStatus.Id;
                        registration.Password = null;
                    }
                }
            }
            else
            {
                var registrationStatus =
                   (input.SelectedRejectionReasonId == -1) ?
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
                        registration.Password = null;
                        break;
                }
            }

            registration.RejectionReasonId =
                (input.SelectedRejectionReasonId != -1) ? input.SelectedRejectionReasonId : (long?)null;

            await _registrationRepository.UpdateAsync(registration);

            DateTime nowUtc = DateTime.UtcNow;
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            DateTime westTime = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, cstZone);

            // Create RegistrationHistory record
            await _registrationHistoriesAppService.CreateNew(new RegistrationHistory.Dtos.CreateOrEditRegistrationHistoryDto
            {
                RegistrationId = registration.Id,
                RegistrationStatusId = registration.RegistrationStatusId,
                DateCreated = westTime,
                Remarks = string.Empty,
                AbpUserId = AbpSession.UserId ?? -1
            });

            return true;
        }

        public async Task<GetRegistrationForProcessingOutput> GetRegistrationForProcessing(EntityDto<long> input)
        {
            var userPermissions = await _userAppService.GetUserPermissionsForEdit(new EntityDto<long> { Id = (long)AbpSession.UserId });

            var registration = await _registrationRepository.GetAsync(input.Id);
            var purchaseRegistrations = await _lookup_purchaseRegistrationRepository.GetAll().Where(p => p.RegistrationId == input.Id).ToListAsync();
            var registrationStatus = await _lookup_registrationStatusRepository.FirstOrDefaultAsync(registration.RegistrationStatusId);
            var registrationHistory = await _lookup_registrationHistoryRepository.GetAll().OrderBy(record => record.RegistrationId).FirstOrDefaultAsync(record => record.RegistrationId == input.Id);
            var messageHistory = await _lookup_messageHistoryRepository.GetAll().Where(h => h.RegistrationId == input.Id).OrderBy(h => h.TimeStamp).ThenBy(h => h.Id).ToListAsync();
            var campaignForm = await _lookup_campaignFormRepository.GetAsync(registration.CampaignFormId);
            var campaign = await _lookup_campaignRepository.GetAsync(campaignForm.CampaignId);
            var formLocale = await _lookup_formLocaleRepository.GetAll().FirstOrDefaultAsync(f => f.FormId == campaignForm.FormId && f.LocaleId == registration.LocaleId);
            var formBlocks = await _lookup_formBlockRepository.GetAll().Where(f => f.FormLocaleId == formLocale.Id).OrderBy(f => f.SortOrder).ToListAsync();
            var uiFormBlocks = new List<Dtos.ProcessRegistration.FormBlock>();
            var availableRejectionReasons = await _rejectionReasonsAppService.GetAll();

            var messageStatus = _messagingAppService.getMessageStatusList(messageHistory.Select(h => h.MessageId).ToList());

            var canOnlyEditRemarks = false;

            //if (registrationStatus.StatusCode == RegistrationStatusCodeHelper.Accepted ||
            //    registrationStatus.StatusCode == RegistrationStatusCodeHelper.Rejected ||
            //    registrationStatus.StatusCode == RegistrationStatusCodeHelper.InProgress ||
            //    registrationStatus.StatusCode == RegistrationStatusCodeHelper.Send)
            //{
            //    canOnlyEditRemarks = !userPermissions.GrantedPermissionNames.Contains(AppPermissions.Pages_Registrations_EditAll);
            //}

            if (registrationStatus.StatusCode == RegistrationStatusCodeHelper.Accepted ||
                registrationStatus.StatusCode == RegistrationStatusCodeHelper.Rejected)
            {
                canOnlyEditRemarks = !userPermissions.GrantedPermissionNames.Contains(AppPermissions.Pages_Registrations_EditAll);
            }else if (registrationStatus.StatusCode == RegistrationStatusCodeHelper.InProgress ||
                      registrationStatus.StatusCode == RegistrationStatusCodeHelper.Send)
            {
                canOnlyEditRemarks = true;
            }

            var rejectionFields = new List<string>();

            if (!string.IsNullOrWhiteSpace(registration.IncompleteFields))
            {
                rejectionFields.AddRange(registration.IncompleteFields.Split(','));
            }
            else if (!string.IsNullOrWhiteSpace(registration.RejectedFields))
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
                                                            FieldType = s2.Description,
                                                            FieldLabel = s1.Label,
                                                            RegistrationField = s1.RegistrationField,
                                                            PurchaseRegistrationField = s1.PurchaseRegistrationField,
                                                            IsRejected = rejectionFields.Contains(s1.FieldName)
                                                        }).ToListAsync();

                // Its type is unknown as of yet
                var uiFormFields = new List<Dtos.ProcessRegistration.FormField>();
                var uiFormFieldCollectionLines = new List<FormFieldCollectionLine>();

                foreach (var formBlockField in formBlockFieldsWithDetails)
                {
                    // Are there ListValues for this FormField?
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

                    //Go...
                    if (!string.IsNullOrEmpty(formBlockField.RegistrationField))
                    {
                        var fieldValue = typeof(Registration).GetProperty(formBlockField.RegistrationField).GetValue(registration, null);
                        var fieldValueFormatted = string.Empty;

                        if (fieldValue != null)
                        {
                            fieldValueFormatted = formBlockField.FieldType switch
                            {
                                FieldTypeHelper.InputNumber => fieldValue.ToString().Replace(',', '.'),
                                FieldTypeHelper.DatePicker => ((DateTime)fieldValue).ToString("yyyy-MM-dd"),
                                _ => fieldValue.ToString(),
                            };
                        }

                        uiFormFields.Add(new Dtos.ProcessRegistration.FormField
                        {
                            FieldId = formBlockField.FieldId,
                            FieldType = formBlockField.FieldType,
                            FieldLabel = formBlockField.FieldLabel,
                            FieldValue = fieldValueFormatted,
                            FieldListValues = formFieldListValuesCollection,
                            FieldSource = FieldSourceHelper.Registration,
                            RegistrationField = formBlockField.RegistrationField,
                            PurchaseRegistrationField = formBlockField.PurchaseRegistrationField,
                            IsRejected = formBlockField.IsRejected,
                            IsReadOnly = formBlockField.FieldType != FieldTypeHelper.Remark && canOnlyEditRemarks
                        });
                    }
                    else if (!string.IsNullOrEmpty(formBlockField.PurchaseRegistrationField))
                    {
                        if (!uiFormFieldCollectionLines.Any())
                        {
                            foreach (var purchaseRegistration in purchaseRegistrations)
                            {
                                var fieldValue = typeof(PurchaseRegistration).GetProperty(formBlockField.PurchaseRegistrationField).GetValue(purchaseRegistration, null);
                                var fieldValueFormatted = string.Empty;

                                if (fieldValue != null)
                                {
                                    fieldValueFormatted = formBlockField.FieldType switch
                                    {
                                        FieldTypeHelper.InputNumber => fieldValue.ToString().Replace(',', '.'),
                                        FieldTypeHelper.DatePicker => ((DateTime)fieldValue).ToString("yyyy-MM-dd"),
                                        _ => fieldValue.ToString(),
                                    };
                                }

                                var formFieldsList = new List<Dtos.ProcessRegistration.FormField>
                                {
                                    new Dtos.ProcessRegistration.FormField
                                    {
                                        FieldId = formBlockField.FieldId,
                                        FieldType = formBlockField.FieldType,
                                        FieldLabel = formBlockField.FieldLabel,
                                        FieldValue = fieldValueFormatted,
                                        FieldListValues = formFieldListValuesCollection,
                                        FieldSource = FieldSourceHelper.PurchaseRegistration,
                                        RegistrationField = formBlockField.RegistrationField,
                                        PurchaseRegistrationField = formBlockField.PurchaseRegistrationField,
                                        FieldLineId = purchaseRegistration.Id,
                                        IsRejected = formBlockField.IsRejected,
                                        IsReadOnly = formBlockField.FieldType != FieldTypeHelper.Remark && canOnlyEditRemarks
                                    }
                                };

                                // var products = await _productsAppService.GetAll();
                                var products = await _productsAppService.GetAllProductsForCampaign(campaign.Id);
                                var chosenProductDto = products.Items.FirstOrDefault(x => x.Product.Id == purchaseRegistration.ProductId);
                                uiFormFieldCollectionLines.Add(new FormFieldCollectionLine
                                {
                                    FormFields = formFieldsList,
                                    Title = chosenProductDto?.Product?.Description,
                                    SubTitle = chosenProductDto?.Product?.ProductCode,
                                });
                            }
                        }
                        else
                        {
                            foreach (var purchaseRegistration in purchaseRegistrations)
                            {
                                var fieldValue = typeof(PurchaseRegistration).GetProperty(formBlockField.PurchaseRegistrationField).GetValue(purchaseRegistration, null);
                                var fieldValueFormatted = string.Empty;

                                if (fieldValue != null)
                                {
                                    fieldValueFormatted = formBlockField.FieldType switch
                                    {
                                        FieldTypeHelper.InputNumber => fieldValue.ToString().Replace(',', '.'),
                                        FieldTypeHelper.DatePicker => ((DateTime)fieldValue).ToString("yyyy-MM-dd"),
                                        _ => fieldValue.ToString(),
                                    };
                                }

                                uiFormFieldCollectionLines[purchaseRegistrations.IndexOf(purchaseRegistration)].FormFields.Add(new Dtos.ProcessRegistration.FormField
                                {
                                    FieldId = formBlockField.FieldId,
                                    FieldType = formBlockField.FieldType,
                                    FieldLabel = formBlockField.FieldLabel,
                                    FieldValue = fieldValueFormatted,
                                    FieldListValues = formFieldListValuesCollection,
                                    FieldSource = FieldSourceHelper.PurchaseRegistration,
                                    RegistrationField = formBlockField.RegistrationField,
                                    PurchaseRegistrationField = formBlockField.PurchaseRegistrationField,
                                    FieldLineId = purchaseRegistration.Id,
                                    IsRejected = formBlockField.IsRejected,
                                    IsReadOnly = formBlockField.FieldType != FieldTypeHelper.Remark && canOnlyEditRemarks
                                });
                            }
                        }
                    }
                    else if (formBlockField.FieldType == FieldSourceHelper.Product)
                    {
                        var products = await _productsAppService.GetAllProductsForCampaign(campaign.Id); // 5616 items -> 576 items

                        formFieldListValuesCollection.Clear();

                        foreach (var product in products?.Items)
                        {
                            formFieldListValuesCollection.Add(new FormFieldListValue()
                            {
                                KeyValue = product.Product?.Id.ToString(),
                                Description = $"{product.Product?.ProductCode} ({product.Product?.Description})"
                            });
                        }

                        if (!uiFormFieldCollectionLines.Any())
                        {
                            foreach (var purchaseRegistration in purchaseRegistrations)
                            {
                                var formFieldsList = new List<Dtos.ProcessRegistration.FormField>
                                {
                                    new Dtos.ProcessRegistration.FormField
                                    {
                                        FieldId = formBlockField.FieldId,
                                        FieldType = FieldTypeHelper.DropdownMenu,
                                        FieldLabel = formBlockField.FieldLabel,
                                        FieldValue = purchaseRegistration.ProductId.ToString(),
                                        FieldListValues = formFieldListValuesCollection,
                                        FieldSource = FieldSourceHelper.PurchaseRegistration,
                                        RegistrationField = string.Empty,
                                        PurchaseRegistrationField = "ProductId",
                                        FieldLineId = purchaseRegistration.Id,
                                        IsRejected = formBlockField.IsRejected,
                                        IsReadOnly = formBlockField.FieldType != FieldTypeHelper.Remark && canOnlyEditRemarks
                                    }
                                };

                                var chosenProductDto = products?.Items.FirstOrDefault(x => x.Product.Id == purchaseRegistration.ProductId);

                                uiFormFieldCollectionLines.Add(new FormFieldCollectionLine
                                {
                                    Title = chosenProductDto?.Product?.Description,
                                    SubTitle = chosenProductDto?.Product?.ProductCode,
                                    FormFields = formFieldsList
                                });
                            }
                        }
                        else
                        {
                            foreach (var purchaseRegistration in purchaseRegistrations)
                            {
                                uiFormFieldCollectionLines[purchaseRegistrations.IndexOf(purchaseRegistration)].FormFields.Add(new Dtos.ProcessRegistration.FormField
                                {
                                    FieldId = formBlockField.FieldId,
                                    FieldType = FieldTypeHelper.DropdownMenu,
                                    FieldLabel = formBlockField.FieldLabel,
                                    FieldValue = purchaseRegistration.ProductId.ToString(),
                                    FieldListValues = formFieldListValuesCollection,
                                    FieldSource = FieldSourceHelper.PurchaseRegistration,
                                    RegistrationField = string.Empty,
                                    PurchaseRegistrationField = "ProductId",
                                    FieldLineId = purchaseRegistration.Id,
                                    IsRejected = formBlockField.IsRejected,
                                    IsReadOnly = formBlockField.FieldType != FieldTypeHelper.Remark && canOnlyEditRemarks
                                });
                            }
                        }
                    }
                    else if (formBlockField.FieldType == FieldSourceHelper.RetailerLocation)
                    {
                        var retailerLocations = await _retailersAppService.GetAllRetailersForCampaign(campaign.Id);

                        formFieldListValuesCollection.Clear();
                        formFieldListValuesCollection = retailerLocations.Items.Select(q => new FormFieldListValue
                        {
                            KeyValue = q.RetailerLocation.Id.ToString(),
                            Description = string.IsNullOrWhiteSpace(q.RetailerLocation.PostalCode) ? q.RetailerLocation.Name : $"{q.RetailerLocation.Name} ({q.RetailerLocation.PostalCode})"
                        }).OrderBy(q => q.Description).ToList();

                        /*formFieldListValuesCollection.Clear();
                        formFieldListValuesCollection = await (from o in retailerLocations
                                                               join o1 in _lookup_retailerRepository.GetAll() on o.RetailerId equals o1.Id into j1
                                                               from s1 in j1.DefaultIfEmpty()
                                                               orderby o.Name, s1.Name
                                                               select new FormFieldListValue
                                                               {
                                                                   KeyValue = o.Id.ToString(),
                                                                   Description = $"{o.Name} ({o.PostalCode})"
                                                               }).ToListAsync();*/

                        if (!uiFormFieldCollectionLines.Any())
                        {
                            foreach (var purchaseRegistration in purchaseRegistrations)
                            {
                                var formFieldsList = new List<Dtos.ProcessRegistration.FormField>
                                {
                                    new Dtos.ProcessRegistration.FormField
                                    {
                                        FieldId = formBlockField.FieldId,
                                        FieldType = FieldTypeHelper.DropdownMenu,
                                        FieldLabel = formBlockField.FieldLabel,
                                        FieldValue = purchaseRegistration.RetailerLocationId.ToString(),
                                        FieldListValues = formFieldListValuesCollection,
                                        FieldSource = FieldSourceHelper.PurchaseRegistration,
                                        RegistrationField = string.Empty,
                                        PurchaseRegistrationField = $"{FieldSourceHelper.RetailerLocation}Id",
                                        FieldLineId = purchaseRegistration.Id,
                                        IsRejected = formBlockField.IsRejected,
                                        IsReadOnly = formBlockField.FieldType != FieldTypeHelper.Remark && canOnlyEditRemarks
                                    }
                                };

                                uiFormFieldCollectionLines.Add(new FormFieldCollectionLine
                                {
                                    FormFields = formFieldsList
                                });
                            }
                        }
                        else
                        {
                            foreach (var purchaseRegistration in purchaseRegistrations)
                            {
                                uiFormFieldCollectionLines[purchaseRegistrations.IndexOf(purchaseRegistration)].FormFields.Add(new Dtos.ProcessRegistration.FormField
                                {
                                    FieldId = formBlockField.FieldId,
                                    FieldType = FieldTypeHelper.DropdownMenu,
                                    FieldLabel = formBlockField.FieldLabel,
                                    FieldValue = purchaseRegistration.RetailerLocationId.ToString(),
                                    FieldListValues = formFieldListValuesCollection,
                                    FieldSource = FieldSourceHelper.PurchaseRegistration,
                                    RegistrationField = string.Empty,
                                    PurchaseRegistrationField = $"{FieldSourceHelper.RetailerLocation}Id",
                                    FieldLineId = purchaseRegistration.Id,
                                    IsRejected = formBlockField.IsRejected,
                                    IsReadOnly = formBlockField.FieldType != FieldTypeHelper.Remark && canOnlyEditRemarks
                                });
                            }
                        }
                    }
                    else if (formBlockField.FieldType == FieldSourceHelper.Country)
                    {
                        var countries = await _countriesAppService.GetAll();

                        formFieldListValuesCollection.Clear();

                        foreach (var country in countries)
                        {
                            formFieldListValuesCollection.Add(new FormFieldListValue { KeyValue = country.Country.Id.ToString(), Description = country.Country.Description });
                        }

                        uiFormFields.Add(new Dtos.ProcessRegistration.FormField()
                        {
                            FieldId = formBlockField.FieldId,
                            FieldType = FieldTypeHelper.DropdownMenu,
                            FieldLabel = formBlockField.FieldLabel,
                            FieldValue = registration.CountryId.ToString(),
                            FieldListValues = formFieldListValuesCollection,
                            FieldSource = FieldSourceHelper.Registration,
                            RegistrationField = "CountryId",
                            PurchaseRegistrationField = string.Empty,
                            IsRejected = formBlockField.IsRejected,
                            IsReadOnly = formBlockField.FieldType != FieldTypeHelper.Remark && canOnlyEditRemarks
                        });
                    }
                    else if (formBlockField.FieldType == FieldSourceHelper.IbanChecker)
                    {
                        uiFormFields.Add(new Dtos.ProcessRegistration.FormField()
                        {
                            FieldId = formBlockField.FieldId,
                            FieldType = FieldTypeHelper.IbanChecker,
                            FieldLabel = "IBAN & BIC",
                            FieldValue = $"{registration.Iban}|{registration.Bic}",
                            FieldListValues = formFieldListValuesCollection,
                            FieldSource = FieldSourceHelper.Registration,
                            RegistrationField = "Iban",
                            PurchaseRegistrationField = string.Empty,
                            IsRejected = formBlockField.IsRejected,
                            IsReadOnly = formBlockField.FieldType != FieldTypeHelper.Remark && canOnlyEditRemarks
                        });
                    }
                    else // Custom field, either on Registration level or PurchaseRegistration level...
                    {
                        var registrationFieldData = _lookup_registrationFieldDataRepository.GetAll().Where(f => f.RegistrationId == registration.Id);
                        var customRegistrationField = (from o in registrationFieldData
                                                       join o1 in _lookup_RegistrationFieldRepository.GetAll() on o.RegistrationFieldId equals o1.Id
                                                       where o1.FormFieldId == formBlockField.FieldId
                                                       select o).FirstOrDefault();

                        if (customRegistrationField != null)
                        {
                            var fieldValueFormatted = string.Empty;

                            if (!string.IsNullOrWhiteSpace(customRegistrationField.Value))
                            {
                                fieldValueFormatted = formBlockField.FieldType switch
                                {
                                    FieldTypeHelper.InputNumber => customRegistrationField.Value.Replace(',', '.'),
                                    FieldTypeHelper.DatePicker => Convert.ToDateTime(customRegistrationField.Value).ToString("yyyy-MM-dd"),
                                    _ => customRegistrationField.Value,
                                };
                            }

                            uiFormFields.Add(new Dtos.ProcessRegistration.FormField
                            {
                                FieldId = formBlockField.FieldId,
                                FieldType = formBlockField.FieldType,
                                FieldLabel = formBlockField.FieldLabel,
                                FieldValue = fieldValueFormatted,
                                FieldListValues = formFieldListValuesCollection,
                                FieldSource = FieldSourceHelper.CustomRegistration,
                                RegistrationField = formBlockField.RegistrationField,
                                PurchaseRegistrationField = formBlockField.PurchaseRegistrationField,
                                IsRejected = formBlockField.IsRejected,
                                IsReadOnly = formBlockField.FieldType != FieldTypeHelper.Remark && canOnlyEditRemarks
                            });
                        }
                        else
                        {
                            if (!uiFormFieldCollectionLines.Any())
                            {
                                foreach (var purchaseRegistration in purchaseRegistrations)
                                {
                                    var purchaseRegistrationFieldData = _lookup_purchaseRegistrationFieldDataRepository.GetAll().Where(f => f.PurchaseRegistrationId == purchaseRegistration.Id);
                                    var customPurchaseRegistrationField = (from o in purchaseRegistrationFieldData
                                                                           join o1 in _lookup_purchaseRegistrationFieldRepository.GetAll() on o.PurchaseRegistrationFieldId equals o1.Id
                                                                           where o1.FormFieldId == formBlockField.FieldId
                                                                           select o).FirstOrDefault();

                                    if (customPurchaseRegistrationField != null)
                                    {
                                        var formFieldsList = new List<Dtos.ProcessRegistration.FormField>();
                                        var fieldValueFormatted = string.Empty;

                                        if (!string.IsNullOrWhiteSpace(customPurchaseRegistrationField.Value))
                                        {
                                            fieldValueFormatted = formBlockField.FieldType switch
                                            {
                                                FieldTypeHelper.InputNumber => customPurchaseRegistrationField.Value.Replace(',', '.'),
                                                FieldTypeHelper.DatePicker => Convert.ToDateTime(customPurchaseRegistrationField.Value).ToString("yyyy-MM-dd"),
                                                _ => customPurchaseRegistrationField.Value,
                                            };
                                        }

                                        formFieldsList.Add(new Dtos.ProcessRegistration.FormField
                                        {
                                            FieldId = formBlockField.FieldId,
                                            FieldType = formBlockField.FieldType,
                                            FieldLabel = formBlockField.FieldLabel,
                                            FieldValue = fieldValueFormatted,
                                            FieldListValues = formFieldListValuesCollection,
                                            FieldSource = FieldSourceHelper.CustomPurchaseRegistration,
                                            RegistrationField = formBlockField.RegistrationField,
                                            PurchaseRegistrationField = formBlockField.PurchaseRegistrationField,
                                            FieldLineId = purchaseRegistration.Id,
                                            IsRejected = formBlockField.IsRejected,
                                            IsReadOnly = formBlockField.FieldType != FieldTypeHelper.Remark && canOnlyEditRemarks
                                        });

                                        uiFormFieldCollectionLines.Add(new FormFieldCollectionLine
                                        {
                                            FormFields = formFieldsList
                                        });
                                    }
                                }
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

                                    if (customPurchaseRegistrationField != null)
                                    {
                                        var fieldValueFormatted = string.Empty;

                                        if (!string.IsNullOrWhiteSpace(customPurchaseRegistrationField.Value))
                                        {
                                            fieldValueFormatted = formBlockField.FieldType switch
                                            {
                                                FieldTypeHelper.InputNumber => customPurchaseRegistrationField.Value.Replace(',', '.'),
                                                FieldTypeHelper.DatePicker => Convert.ToDateTime(customPurchaseRegistrationField.Value).ToString("yyyy-MM-dd"),
                                                _ => customPurchaseRegistrationField.Value,
                                            };
                                        }

                                        uiFormFieldCollectionLines[purchaseRegistrations.IndexOf(purchaseRegistration)].FormFields.Add(new Dtos.ProcessRegistration.FormField
                                        {
                                            FieldId = formBlockField.FieldId,
                                            FieldType = formBlockField.FieldType,
                                            FieldLabel = formBlockField.FieldLabel,
                                            FieldValue = fieldValueFormatted,
                                            FieldListValues = formFieldListValuesCollection,
                                            FieldSource = FieldSourceHelper.CustomPurchaseRegistration,
                                            RegistrationField = formBlockField.RegistrationField,
                                            PurchaseRegistrationField = formBlockField.PurchaseRegistrationField,
                                            FieldLineId = purchaseRegistration.Id,
                                            IsRejected = formBlockField.IsRejected,
                                            IsReadOnly = formBlockField.FieldType != FieldTypeHelper.Remark && canOnlyEditRemarks
                                        });

                                    }


                                }
                            }
                        }
                    }
                }

                var uiFormBlock = new Dtos.ProcessRegistration.FormBlock
                {
                    BlockTitle = formBlock.Description,
                    FormFields = uiFormFields,
                    FormFieldsCollectionLines = uiFormFieldCollectionLines
                };

                uiFormBlocks.Add(uiFormBlock);
            }

            // This combination of information, because there could be a family who use the same e-mail address and postal code, but don't share the same names
            // Should therefore be a separate registration if the names are different, no?
            var relatedRegistrations = await _registrationRepository.GetAll()
                .Where(record => record.Id != registration.Id &&
                                 record.EmailAddress.Trim().ToLower() == registration.EmailAddress.Trim().ToLower() &&
                                 record.PostalCode.Trim().ToLower() == registration.PostalCode.Trim().ToLower() &&
                                 record.FirstName.Trim().ToLower() == registration.FirstName.Trim().ToLower() &&
                                 record.LastName.Trim().ToLower() == registration.LastName.Trim().ToLower())
                .Select(x => new GetRelatedRegistrationsForViewOutput
                {
                    RegistrationId = (ulong)x.Id,
                    DateCreated = null
                }).ToListAsync();

            foreach (var relatedRegistration in relatedRegistrations)
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
            }

            var registrationHistories = await _lookup_registrationHistoryRepository.GetAll()
                .Where(r => r.RegistrationId == registration.Id)
                .Include(record => record.RegistrationStatusFk).ToListAsync();
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

            bool isCashRefund = false;
            bool isActivationCode = false;
            var purchaseRegistrationsForRegistration = await _lookup_purchaseRegistrationRepository.GetAllListAsync(x => x.RegistrationId == registration.Id);
            if (purchaseRegistrationsForRegistration.Any())
            {
                foreach(var purchaseRegistration in purchaseRegistrationsForRegistration)
                {
                    var handlingLinesForPurchaseRegistration = await _lookup_handlingLineRepository.GetAll().
                        FirstAsync(x => x.Id == purchaseRegistration.HandlingLineId);
                    var campaignTypeForHandlingLine = await _campaignTypeRepository.GetAsync(handlingLinesForPurchaseRegistration.CampaignTypeId);

                    switch(campaignTypeForHandlingLine.Id)
                    {
                        case 1: // CR
                            isCashRefund = true;
                            break;
                        case 3: // AC
                            isActivationCode = true;
                            break;
                    }
                }
            }

            var uiForm = new GetRegistrationForProcessingOutput
            {
                CampaignTitle = campaign.Description,
                CampaignType = isCashRefund ? Dtos.CampaignType.CashRefund : (isActivationCode ? Dtos.CampaignType.ActivationCode : Dtos.CampaignType.Other),
                CampaignStartDate = campaign.StartDate,
                CampaignEndDate = campaign.EndDate,
                DateCreated = registrationHistory?.DateCreated.ToString("dd-MM-yyyy HH:mm"),
                FormBlocks = uiFormBlocks,
                Registration = ObjectMapper.Map<CreateOrEditRegistrationDto>(registration),
                RelatedRegistrationsByEmail = relatedRegistrations,
                RelatedRegistrationsBySerialNumber = new List<GetRelatedRegistrationsForViewOutput>(),
                RegistrationHistoryEntries = registrationHistoryList,
                RegistrationMessageHistoryEntries = registrationMessageHistoryList,
                RejectionReasons = availableRejectionReasons,
                SelectedRejectionReasonId = registration.RejectionReasonId,
                StatusCode = registrationStatus.StatusCode,
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

        public async Task<bool> SendFormData(string blobStorage, string blobContainer, PhilipsFormRegistrationHandlingDto vueJsToRmsModel)
        {
            var mappedData = JsonConvert.DeserializeObject<PhilipsFormRegistrationHandlingDto>(vueJsToRmsModel.Data);

            int tenantId = AbpSession.GetTenantId();
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");
            DateTime nowUtc = DateTime.UtcNow;
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            DateTime westTime = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, cstZone);

            var campaign = await _lookup_campaignRepository.GetAll().FirstOrDefaultAsync(fl => fl.CampaignCode == Convert.ToInt32(mappedData.CampaignCode));
            if (campaign == null)
            {
                _logger.LogError($"Error in {nameof(PhilipsRegistrationsAppService)}: returned {nameof(campaign)} variable is of value null.");

                return false;
            }

            var campaignForm = await _lookup_campaignFormRepository.GetAll().FirstOrDefaultAsync(fl => fl.CampaignId == campaign.Id);
            if (campaignForm == null)
            {
                _logger.LogError($"Error in {nameof(PhilipsRegistrationsAppService)}: returned {nameof(campaignForm)} variable is of value null.");

                return false;
            }

            long? countryId = null;
            if (mappedData.Country != null)
            {
                var country = await _lookup_countryRepositoryRepository.GetAll().FirstOrDefaultAsync(fl => fl.CountryCode == mappedData.Country.ListValueTranslationKeyValue.ToUpper());
                if (country == null)
                {
                    _logger.LogError($"Error in {nameof(PhilipsRegistrationsAppService)}: returned {nameof(country)} variable is of value null.");

                    return false;
                }

                countryId = country.Id;
            }
            else
            {
                var localeLookup = await _lookup_localeRepository.GetAll().FirstOrDefaultAsync(fl => fl.Description == mappedData.Locale);
                countryId = localeLookup.CountryId;
            }

            var locale = await _lookup_localeRepository.GetAll().FirstOrDefaultAsync(fl => fl.Description == mappedData.Locale);
            if (locale == null)
            {
                _logger.LogError($"Error in {nameof(PhilipsRegistrationsAppService)}: returned {nameof(locale)} variable is of value null.");

                return false;
            }

            RetailerLocation retailerLocation;
            if (mappedData.StorePurchased != null)
            {
                retailerLocation =
                await _lookup_retailerLocationRepository.GetAll().FirstOrDefaultAsync(fl => fl.Id == Convert.ToInt64(mappedData.StorePurchased)) ??
                await _lookup_retailerLocationRepository.GetAll().FirstAsync();
            }
            else
            {
                retailerLocation = await _lookup_retailerLocationRepository.GetAll().FirstAsync();
            }

            if (retailerLocation != null)
            {
                var retailer = await _lookup_retailerRepository.GetAll().FirstOrDefaultAsync(rt => rt.Id == retailerLocation.RetailerId);
                if (retailer == null)
                {
                    _logger.LogError($"Error in {nameof(PhilipsRegistrationsAppService)}: returned {nameof(retailer)} variable is of value null.");

                    return false;
                }
            }

            int? retailerLocationId = null;
            if (mappedData.StorePicker != null)
            {
                retailerLocationId = Convert.ToInt32(mappedData.StorePicker.RetailerLocationId);
            }

            var registrationStatus = !string.IsNullOrWhiteSpace(mappedData.UniqueCode) ?
                await _registrationStatusesAppService.GetByStatusCode(RegistrationStatusCodeHelper.Accepted) :
                await _registrationStatusesAppService.GetByStatusCode(RegistrationStatusCodeHelper.Pending);

            var uniqueCodeRecord = await _lookup_RegistrationFieldRepository.GetAll().FirstOrDefaultAsync(x => x.Description == "UniqueCode");
            if (!string.IsNullOrWhiteSpace(mappedData.UniqueCode) && uniqueCodeRecord == null)
            { 
                _logger.LogError("UniqueCode record not found.");
                return false;
            }

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
                CountryId = (long)countryId,
                EmailAddress = mappedData.EmailAddress,
                PhoneNumber = mappedData.PhoneNumber,
                Bic = mappedData.IbanChecker?.Bic,
                Iban = mappedData.IbanChecker?.Iban,
                CampaignId = campaign.Id,
                CampaignFormId = campaignForm.Id,
                LocaleId = locale.Id,
                RegistrationStatusId = registrationStatus.RegistrationStatus.Id,
                TenantId = tenantId,
            };

            var registrationId = await _registrationRepository.InsertAndGetIdAsync(registrationMapper);
            if (registrationId <= 0)
            {
                _logger.LogError($"Error in {nameof(RegistrationsAppService)}: returned {nameof(registrationId)} variable is less than or equal to 0.");

                return false;
            }

            var blobPath = string.Empty;
            var fileExtension = string.Empty;

            if (mappedData.InvoiceImagePath != null)
            {
                var invoiceImageList = mappedData.InvoiceImagePath.ToList();
                var blobServiceClient = new BlobServiceClient(blobStorage);
                BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainer);

                foreach (var image in invoiceImageList)
                {
                    int index = image.IndexOf('/') + 1;
                    while (image.Substring(index, 1) != ";")
                    {
                        fileExtension += image.Substring(index, 1);
                        index += 1;
                    }
                    fileExtension = fileExtension.Replace("+xml", "");
                    var fileContent = image.Split("base64,")[1];
                    blobPath = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{registrationId}-{Guid.NewGuid()}.{fileExtension}";
                    blobContainerClient.UploadBlob(blobPath, new MemoryStream(Convert.FromBase64String(fileContent)));
                }
            }

            long? productId = null;
            long? handlingLineId = null;
            var campaignHandlingLines =
                from o in _lookup_handlingLineRepository.GetAll()
                join o1 in _productHandlingRepository.GetAll() on o.ProductHandlingId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                where s1.CampaignId == campaign.Id
                select o;

            var product = !string.IsNullOrWhiteSpace(mappedData.Product) ?
                await _lookup_productRepository.GetAsync(Convert.ToInt64(mappedData.Product)) :
                await _lookup_productRepository.GetAll().FirstAsync();

            productId = product != null ? product.Id : Convert.ToInt64(mappedData.Product);
            
            if (mappedData.ProductPremium != null)
            {
                handlingLineId = Convert.ToInt64(mappedData.ProductPremium.ListValueTranslationKeyValue);
            }
            else
            {
                var handlingLines = from o in _lookup_handlingLineProductRepository.GetAll()
                                    join o1 in campaignHandlingLines on o.HandlingLineId equals o1.Id into j1
                                    from s1 in j1
                                    where o.ProductId == productId
                                    select s1;

              

                if (handlingLines.Count() == 1)
                {
                    // Take the first record of handlingLines if there is only 1 present
                    handlingLineId = handlingLines.First().Id;
                }
                else
                {
                    // There are more than 1 present (or none), so take the UNKNOWN record
                    handlingLineId = campaignHandlingLines.Where(l => l.CustomerCode.ToUpper().Trim() == "UNKNOWN").First().Id;
                }
            }

            var productQuantity = 1;
            if (mappedData.Quantity != null)
            {
                productQuantity = Convert.ToInt32(mappedData.Quantity);
            }

            var purchaseRegistrationMapper = new PurchaseRegistration
            {
                PurchaseDate = mappedData.PurchaseDate != null ? Convert.ToDateTime(mappedData.PurchaseDate) : DateTime.Today,
                RegistrationId = registrationId,
                RetailerLocationId = retailerLocation?.Id ?? retailerLocationId ?? 999,
                ProductId = (long)productId,
                Quantity = productQuantity,
                InvoiceImagePath = blobPath,
                TotalAmount = 0.00M,
                HandlingLineId = (long)handlingLineId,
                TenantId = tenantId,
            };

            var purchaseRegistrationId = await _lookup_purchaseRegistrationRepository.InsertAndGetIdAsync(purchaseRegistrationMapper);
            if (purchaseRegistrationId <= 0)
            {
                _logger.LogError($"Error in {nameof(RegistrationsAppService)}: returned {nameof(purchaseRegistrationId)} variable is less than or equal to 0.");

                return false;
            };

            if (mappedData.StoreNotAvalible != null)
            {
                await _lookup_purchaseRegistrationFieldDataRepository.InsertAsync(new PurchaseRegistrationFieldData
                {
                    TenantId = tenantId,
                    Value = mappedData.StoreNotAvalible,
                    PurchaseRegistrationId = purchaseRegistrationId,
                    PurchaseRegistrationFieldId = 1,
                });
            }

            if (mappedData.PlaceNameStore != null)
            {
                await _lookup_purchaseRegistrationFieldDataRepository.InsertAsync(new PurchaseRegistrationFieldData
                {
                    TenantId = tenantId,
                    Value = mappedData.PlaceNameStore,
                    PurchaseRegistrationId = purchaseRegistrationId,
                    PurchaseRegistrationFieldId = 3,
                });
            }

            if (uniqueCodeRecord != null)
            {
                await _lookup_registrationFieldDataRepository.InsertAsync(new RegistrationFieldData 
                {
                    TenantId = tenantId,
                    RegistrationId = registrationId,
                    RegistrationFieldId = uniqueCodeRecord.Id,
                    Value = mappedData.UniqueCode
                });
            }
            
            await InsertRegistrationHistory(tenantId, registrationId, registrationMapper.RegistrationStatusId, westTime, 1, registrationStatus.RegistrationStatus.Description);

            var registrationJsonData = new RegistrationJsonData.RegistrationJsonData
            {
                Data = vueJsToRmsModel.Data,
                DateCreated = westTime,
                TenantId = tenantId,
                RegistrationId = registrationId
            };

            var registrationJsonDataId = await _lookup_registrationJsonDataRepository.InsertAndGetIdAsync(registrationJsonData);

            if (registrationId != default || registrationId <= 0)
            {
                try
                {
                    await _registrationsAppService.ComposeRegistrationStatusMessaging(registrationId);
                    return registrationId >= 1;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return registrationId >= 1;
        }

        private async Task<bool> InsertRegistrationFieldData(int tenantId, string value, long registrationFieldId, long registrationId)
        {
            try
            {
                await _lookup_registrationFormFieldDataRepository.InsertAsync(new RegistrationFieldData
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

        public static string MoveImageToCloud(int year, int month, int day, long registrationId, string fileDbContent)
        {
            string blobPath;
            try
            {
                var fileExtension = string.Empty;

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
                blobPath = string.Empty;
            }

            return blobPath;
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
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.FirstName.Contains(input.Filter) || e.LastName.Contains(input.Filter) ||
                         e.Street.Contains(input.Filter) || e.HouseNr.Contains(input.Filter) ||
                         e.PostalCode.Contains(input.Filter) || e.City.Contains(input.Filter) ||
                         e.EmailAddress.Contains(input.Filter) || e.PhoneNumber.Contains(input.Filter) ||
                         e.CompanyName.Contains(input.Filter) || e.Gender.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.FirstNameFilter), e => e.FirstName == input.FirstNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.LastNameFilter), e => e.LastName == input.LastNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.StreetFilter), e => e.Street == input.StreetFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.HouseNrFilter), e => e.HouseNr == input.HouseNrFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.PostalCodeFilter),
                    e => e.PostalCode == input.PostalCodeFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter), e => e.City == input.CityFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.EmailAddressFilter),
                    e => e.EmailAddress == input.EmailAddressFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.PhoneNumberFilter),
                    e => e.PhoneNumber == input.PhoneNumberFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.CompanyNameFilter),
                    e => e.CompanyName == input.CompanyNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.GenderFilter), e => e.Gender == input.GenderFilter)
                .WhereIf(input.MinCountryIdFilter != null, e => e.CountryId >= input.MinCountryIdFilter)
                .WhereIf(input.MaxCountryIdFilter != null, e => e.CountryId <= input.MaxCountryIdFilter)
                .WhereIf(input.MinCampaignIdFilter != null, e => e.CampaignId >= input.MinCampaignIdFilter)
                .WhereIf(input.MaxCampaignIdFilter != null, e => e.CampaignId <= input.MaxCampaignIdFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.RegistrationStatusStatusCodeFilter),
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
                   !string.IsNullOrWhiteSpace(input.Filter),
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
    }
}