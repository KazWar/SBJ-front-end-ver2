using RMS.SBJ.CodeTypeTables;

using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Threading.Tasks;
using RMS.SBJ.Forms.Exporting;
using RMS.SBJ.Forms.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.Web.Models;
using RMS.SBJ.CampaignRetailerLocations;
using RMS.SBJ.RetailerLocations;
using RMS.SBJ.Retailers;
using RMS.SBJ.HandlingLineRetailers;
using RMS.SBJ.ProductHandlings;
using RMS.SBJ.HandlingLineProducts;
using RMS.SBJ.Products;
using RMS.SBJ.HandlingLines;
using RMS.SBJ.PurchaseRegistrationFormFields;
using RMS.SBJ.PurchaseRegistrationFieldDatas;
using RMS.SBJ.PurchaseRegistrations;
using RMS.SBJ.Registrations;
using RMS.SBJ.CampaignProcesses;
using RMS.SBJ.RegistrationFormFieldDatas;
using RMS.SBJ.Registrations.Helpers;
using RMS.SBJ.RegistrationFields;
using Abp.Runtime.Session;
using RMS.SBJ.PurchaseRegistrationFields;

namespace RMS.SBJ.Forms
{
    [AbpAuthorize(AppPermissions.Pages_FormLocales)]
    public class PhilipsFormLocalesAppService : RMSAppServiceBase, IPhilipsFormLocalesAppService
    {
        private readonly IRepository<FormLocale, long> _formLocaleRepository;
        private readonly IFormLocalesExcelExporter _formLocalesExcelExporter;
        private readonly IRepository<Form, long> _lookup_formRepository;
        private readonly IRepository<Locale, long> _lookup_localeRepository;
        private readonly IRepository<FormBlock, long> _formBlockRepository;
        private readonly IRepository<FormBlockField, long> _formBlockFieldRepository;
        private readonly IRepository<FormField, long> _formFieldRepository;
        private readonly IRepository<FieldType, long> _fieldTypeRepository;
        private readonly IRepository<Company.Company, long> _companyRepository;
        private readonly IRepository<FormFieldValueList, long> _formFieldValueListRepository;
        private readonly IRepository<ValueList, long> _valueListRepository;
        private readonly IRepository<ListValue, long> _listValueRepository;
        private readonly IRepository<ListValueTranslation, long> _listValueTranslationRepository;
        private readonly IRepository<CampaignRetailerLocation, long> _lookup_campaignRetailerLocationRepository;
        private readonly IRepository<RetailerLocation, long> _lookup_retailerLocationRepository;
        private readonly IRepository<Retailer, long> _lookup_retailerRepository;
        private readonly IRepository<HandlingLine, long> _lookup_handlingLineRepository;
        private readonly IRepository<HandlingLineRetailer, long> _lookup_handlingLineRetailerRepository;
        private readonly IRepository<HandlingLineProduct, long> _lookup_handlingLineProductRepository;
        private readonly IRepository<ProductHandling, long> _productHandlingRepository;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IRepository<Company.Address, long> _lookup_addressRepository;
        private readonly IRepository<FormFieldTranslation, long> _lookup_formFieldTranslationRepository;
        private readonly IRepository<PurchaseRegistrationFormField, long> _lookup_purchaseRegistrationFormFieldsRepository;
        private readonly IRepository<PurchaseRegistrationFieldData, long> _lookup_purchaseRegistrationFieldDataRepository;
        private readonly IRepository<PurchaseRegistration, long> _lookup_purchaseRegistrationRepository;
        private readonly IRepository<Registration, long> _lookup_registrationRepository;
        private readonly IRepository<RegistrationFieldData, long> _lookup_registrationFieldDataRepository;
        private readonly IRepository<CampaignForm, long> _lookup_campaignFormRepository;
        private readonly IRepository<Campaign, long> _lookup_campaignRepository;
        private readonly IRepository<Country, long> _lookup_countryRepository;
        private readonly IRepository<CampaignCountry, long> _lookup_CampaignCountryRepository;
        private readonly IRepository<Registration, long> _registrationRepository;
        private readonly IRepository<RegistrationField, long> _registrationFieldRepository;
        private readonly IRepository<PurchaseRegistrationField, long> _purchaseRegistrationFieldRepository;

        public PhilipsFormLocalesAppService(
            IRepository<FormLocale, long> formLocaleRepository,
            IFormLocalesExcelExporter formLocalesExcelExporter,
            IRepository<Registration, long> registrationRepository,
            IRepository<RegistrationFieldData, long> lookup_registrationFieldDataRepository,
            IRepository<Form, long> lookup_formRepository,
            IRepository<Locale, long> lookup_localeRepository,
            IRepository<Campaign, long> lookup_campaignRepository,
            IRepository<FormBlock, long> formBlockRepository,
            IRepository<FormBlockField, long> formBlockFieldRepository,
            IRepository<FormField, long> formFieldRepository,
            IRepository<FieldType, long> fieldTypeRepository,
            IRepository<Company.Company, long> companyRepository,
            IRepository<FormFieldValueList, long> formFieldValueListRepository,
            IRepository<ValueList, long> valueListRepository,
            IRepository<ListValue, long> listValueRepository,
            IRepository<ListValueTranslation, long> listValueTranslationRepository,
            IRepository<CampaignRetailerLocation, long> lookup_campaignRetailerLocationRepository,
            IRepository<RetailerLocation, long> lookup_retailerLocationRepository,
            IRepository<Retailer, long> lookup_retailerRepository,
            IRepository<HandlingLine, long> lookup_handlingLineRepository,
            IRepository<HandlingLineRetailer, long> lookup_handlingLineRetailerRepository,
            IRepository<HandlingLineProduct, long> lookup_handlingLineProductRepository,
            IRepository<ProductHandling, long> productHandlingRepository,
            IRepository<RegistrationField, long> registrationFieldRepository,
            IRepository<Product, long> lookup_productRepository,
            IRepository<Company.Address, long> lookup_addressRepository,
            IRepository<FormFieldTranslation, long> lookup_formFieldTranslationRepository,
            IRepository<PurchaseRegistrationFormField, long> lookup_purchaseRegistrationFormFieldsRepository,
            IRepository<PurchaseRegistrationFieldData, long> lookup_purchaseRegistrationFieldDataRepository,
            IRepository<PurchaseRegistration, long> lookup_purchaseRegistrationRepository,
            IRepository<Registration, long> lookup_registrationRepository,
            IRepository<CampaignForm, long> lookup_campaignFormRepository,
            IRepository<Country, long> lookup_countryRepository,
            IRepository<CampaignCountry, long> lookup_CampaignCountryRepository,
            IRepository<PurchaseRegistrationField, long> purchaseRegistrationFieldRepository)
        {
            _formLocaleRepository = formLocaleRepository;
            _formLocalesExcelExporter = formLocalesExcelExporter;
            _registrationRepository = registrationRepository;
            _lookup_formRepository = lookup_formRepository;
            _lookup_localeRepository = lookup_localeRepository;
            _formBlockRepository = formBlockRepository;
            _formBlockFieldRepository = formBlockFieldRepository;
            _formFieldRepository = formFieldRepository;
            _fieldTypeRepository = fieldTypeRepository;
            _companyRepository = companyRepository;
            _formFieldValueListRepository = formFieldValueListRepository;
            _valueListRepository = valueListRepository;
            _listValueRepository = listValueRepository;
            _listValueTranslationRepository = listValueTranslationRepository;
            _lookup_campaignRetailerLocationRepository = lookup_campaignRetailerLocationRepository;
            _lookup_retailerLocationRepository = lookup_retailerLocationRepository;
            _lookup_retailerRepository = lookup_retailerRepository;
            _lookup_handlingLineRepository = lookup_handlingLineRepository;
            _lookup_handlingLineRetailerRepository = lookup_handlingLineRetailerRepository;
            _lookup_handlingLineProductRepository = lookup_handlingLineProductRepository;
            _productHandlingRepository = productHandlingRepository;
            _lookup_productRepository = lookup_productRepository;
            _lookup_addressRepository = lookup_addressRepository;
            _lookup_formFieldTranslationRepository = lookup_formFieldTranslationRepository;
            _lookup_purchaseRegistrationFormFieldsRepository = lookup_purchaseRegistrationFormFieldsRepository;
            _lookup_purchaseRegistrationFieldDataRepository = lookup_purchaseRegistrationFieldDataRepository;
            _lookup_purchaseRegistrationRepository = lookup_purchaseRegistrationRepository;
            _lookup_registrationRepository = lookup_registrationRepository;
            _lookup_campaignFormRepository = lookup_campaignFormRepository;
            _lookup_registrationFieldDataRepository = lookup_registrationFieldDataRepository;
            _registrationFieldRepository = registrationFieldRepository;
            _lookup_campaignRepository = lookup_campaignRepository;
            _lookup_countryRepository = lookup_countryRepository;
            _lookup_CampaignCountryRepository = lookup_CampaignCountryRepository;
            _purchaseRegistrationFieldRepository = purchaseRegistrationFieldRepository;
        }

        public async Task<PagedResultDto<GetFormLocaleForViewDto>> GetAll(GetAllFormLocalesInput input)
        {
            var filteredFormLocales = _formLocaleRepository.GetAll()
                        .Include(e => e.FormFk)
                        .Include(e => e.LocaleFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FormVersionFilter), e => e.FormFk != null && e.FormFk.Version == input.FormVersionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LocaleDescriptionFilter), e => e.LocaleFk != null && e.LocaleFk.LanguageCode == input.LocaleDescriptionFilter);

            var pagedAndFilteredFormLocales = filteredFormLocales
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var formLocales = from o in pagedAndFilteredFormLocales
                              join o1 in _lookup_formRepository.GetAll() on o.FormId equals o1.Id into j1
                              from s1 in j1.DefaultIfEmpty()

                              join o2 in _lookup_localeRepository.GetAll() on o.LocaleId equals o2.Id into j2
                              from s2 in j2.DefaultIfEmpty()

                              select new GetFormLocaleForViewDto()
                              {
                                  FormLocale = new FormLocaleDto
                                  {
                                      Description = o.Description,
                                      Id = o.Id
                                  },
                                  FormVersion = s1 == null || s1.Version == null ? "" : s1.Version.ToString(),
                                  LocaleDescription = s2 == null || s2.Description == null ? "" : s2.Description.ToString()
                              };

            var totalCount = await filteredFormLocales.CountAsync();

            return new PagedResultDto<GetFormLocaleForViewDto>(
                totalCount,
                await formLocales.ToListAsync()
            );
        }

        [WrapResult(WrapOnSuccess = false, WrapOnError = false)]
        public async Task<GetFormLayoutAndDataDto> GetFormLayoutAndDataForRegistration(GetFormLayoutAndDataInput input)
        {
            if (input == null)
            {
                Logger.Error($"Input parameter is null. Cannot execute {nameof(GetFormLayoutAndDataForRegistration)}.");
                return null;
            }

            var registration = await _registrationRepository.GetAsync(input.RegistrationId);
            if (input.Password != registration.Password)
            {
                Logger.Error($"Given password does not match the one assigned to the registration. Given value: {input.Password}");
                return null;
            }

            var formObject = await GetFormAndProductHandeling(registration.CampaignId, input.Locale);
            var result = ObjectMapper.Map<GetFormLayoutAndDataDto>(formObject);
            if (result?.Formbuilder == null)
            {
                Logger.Error($"FormBuilder is null. Aborting method {nameof(GetFormLayoutAndDataForRegistration)}.");
                return null;
            }

            foreach (var block in result.Formbuilder.ExportBlocks)
            {
                foreach (var field in block.ExportFields)
                {
                    var incompleteFields = registration.IncompleteFields;
                    var incompleteList = incompleteFields.Split(',');
                    if (incompleteList.Contains(field.Description))
                    {
                        field.ReadOnly = false;
                    }
                    else
                    {
                        field.ReadOnly = true;
                    }

                    if (field.Description == FieldTypeHelper.PageSeparator)
                    {
                        continue;
                    }

                    var fieldValue = string.Empty;

                    // If it is a RegistrationField
                    if (!string.IsNullOrWhiteSpace(field.RegistrationField))
                    {
                        var getValue = typeof(Registration).GetProperty(field.RegistrationField).GetValue(registration);
                        if (getValue != null)
                        {
                            fieldValue = getValue.ToString();
                        }

                        if (!incompleteList.Contains(field.Description))
                        {
                            field.ReadOnly = true;
                        }

                        // In case we need this specifically, but don't think we will
                        //if (string.IsNullOrWhiteSpace(fieldValue))
                        //{ 
                        //    switch(field.FieldType)
                        //    {
                        //        // Country?
                        //        //case FieldTypeHelper.DatePicker:
                        //        //    break;
                        //        //case FieldTypeHelper.InputNumber:
                        //        //    break;
                        //        case FieldTypeHelper.IbanChecker:
                        //            break;
                        //    }
                        //}
                    }
                    else if (field.FieldType == "IbanChecker")
                    {
                        var getRegistration = await _registrationRepository.GetAsync(input.RegistrationId);
                        if (getRegistration.Iban != null)
                        {
                            fieldValue = getRegistration.Iban.ToString();
                        }
                        else
                        {
                            fieldValue = "";
                        }
                    }
                    // If it is a PurchaseRegistrationField
                    else if (field.IsPurchaseRegistration)
                    {
                        // Get the purchase registration record so that GetValue has a target object
                        // Note: registration.PurchaseRegistrations does not have a link at this stage, so it will always be null
                        var relatedPurchaseRegistration = await _lookup_purchaseRegistrationRepository.FirstOrDefaultAsync(pr => pr.RegistrationId == registration.Id);
                        if (relatedPurchaseRegistration != null)
                        {
                            switch (field.FieldType)
                            {
                                case FieldTypeHelper.RetailerLocation:
                                    var retailerLocationId = typeof(PurchaseRegistration).GetProperty($"{FieldTypeHelper.RetailerLocation}Id")?.GetValue(relatedPurchaseRegistration);
                                    fieldValue = retailerLocationId?.ToString();
                                    break;
                                default:
                                    fieldValue = typeof(PurchaseRegistration).GetProperty(field.PurchaseRegistrationField)?.GetValue(relatedPurchaseRegistration)?.ToString();
                                    break;
                            }
                        }
                    }

                    // Then it is a custom field
                    if (string.IsNullOrWhiteSpace(fieldValue))
                    {
                        // Custom

                        // If custom Registration -> RegistrationField
                        var customRegistrationField = await _registrationFieldRepository.FirstOrDefaultAsync(rf => rf.FormFieldId == field.FormFieldId);
                        if (customRegistrationField != null)
                        {
                            var customRegistrationFieldData = await _lookup_registrationFieldDataRepository.FirstOrDefaultAsync(rfd => rfd.RegistrationFieldId == customRegistrationField.Id && rfd.RegistrationId == input.RegistrationId);
                            fieldValue = customRegistrationFieldData?.Value;
                        }
                        else
                        {
                            // If custom PurchaseRegistration -> PurchaseRegistrationField
                            var customPurchaseRegistrationField = await _purchaseRegistrationFieldRepository.FirstOrDefaultAsync(prf => prf.FormFieldId == field.FormFieldId);
                            var relatedPurchaseRegistration = await _lookup_purchaseRegistrationRepository.FirstOrDefaultAsync(pr => pr.RegistrationId == input.RegistrationId);
                            if (customPurchaseRegistrationField != null)
                            {
                                var customPurchaseRegistration = await _lookup_purchaseRegistrationFieldDataRepository.FirstOrDefaultAsync(prfd => prfd.PurchaseRegistrationFieldId == customPurchaseRegistrationField.Id && prfd.PurchaseRegistrationId == relatedPurchaseRegistration.Id);
                                fieldValue = customPurchaseRegistration?.Value;
                            }
                        }
                    }

                    field.DefaultValue = fieldValue;
                }
            }

            return result;
        }

        [WrapResult(WrapOnSuccess = false, WrapOnError = false)]
        public async Task<GetFormAndProductHandelingDto> GetFormAndProductHandeling(long currentCampaignId, string currentLocale)
        {
            int tenantId = AbpSession.GetTenantId();
            var wrapDto = new GetFormAndProductHandelingDto();
            var campaignForm = await _lookup_campaignFormRepository.GetAll().FirstOrDefaultAsync(cf => cf.CampaignId == currentCampaignId);
            var campaign = await _lookup_campaignRepository.GetAll().FirstOrDefaultAsync(c => c.Id == currentCampaignId);

            var formLocale = await _formLocaleRepository.GetAll().FirstOrDefaultAsync(fl => fl.Description == currentLocale && fl.FormId == campaignForm.FormId);
            if (formLocale == null)
            {
                return null;
            }

            var formTable = await _lookup_formRepository.GetAll().FirstOrDefaultAsync(f => f.Id == formLocale.FormId);
            var locale = await _lookup_localeRepository.GetAsync(formLocale.LocaleId);
            var companyTable = await _companyRepository.GetAll().FirstOrDefaultAsync();

            var jsonDataLocale = new FormExportJsonDto
            {
                FormLocaleId = formLocale.Id,
                FormId = formLocale.FormId,
                CompanyName = companyTable.Name,
                Version = formTable.Version,
                CountryCode = locale.CountryId,
                LanguageCode = locale.LanguageCode
            };

            var jsonFormBlocks = new List<FormBlocksExportJsonDto>();

            var formBlocks = await _formBlockRepository.GetAll().Where(flb => flb.FormLocaleId == formLocale.Id).OrderBy(flb => flb.SortOrder).ToListAsync();
            foreach (var formBlock in formBlocks)
            {
                var jsonDataBlock = new FormBlocksExportJsonDto
                {
                    Description = formBlock.Description,
                    PurchaseRegistration = formBlock.IsPurchaseRegistration
                };

                var jsonFormfields = new List<FormFieldsExportJsonDto>();

                var formBlockFields = await _formBlockFieldRepository.GetAll().Where(fbf => fbf.FormBlockId == formBlock.Id).OrderBy(fbf => fbf.SortOrder).ToListAsync();
                foreach (var formBlockField in formBlockFields)
                {
                    var formField = await _formFieldRepository.GetAsync((long)formBlockField.FormFieldId);
                    var fieldType = await _fieldTypeRepository.GetAll().FirstOrDefaultAsync(fl => fl.Id == formField.FieldTypeId);
                    var formFieldTranslation = await _lookup_formFieldTranslationRepository.GetAll().FirstOrDefaultAsync(fft => fft.FormFieldId == formBlockField.FormFieldId & fft.LocaleId == formLocale.LocaleId);

                    var jsonDataField = new FormFieldsExportJsonDto
                    {
                        Description = formField.FieldName,
                        Label = formFieldTranslation != null ? formFieldTranslation.Label : formField.Label,
                        MaxLength = formField.MaxLength,
                        DefaultValue = formField.DefaultValue,
                        Required = formField.Required,
                        ReadOnly = formField.ReadOnly,
                        InputMask = formField.InputMask,
                        FieldType = fieldType.Description,
                        RegistrationField = formField.RegistrationField,
                        IsPurchaseRegistration =
                            !string.IsNullOrWhiteSpace(formField.PurchaseRegistrationField) ||
                            fieldType.Description == FieldTypeHelper.RetailerLocation ||
                            formField.FieldName == FieldNameHelper.StorePurchased,
                        PurchaseRegistrationField = formField.PurchaseRegistrationField,
                        FormFieldId = formField.Id,
                        RegularExpression = formFieldTranslation != null ? formFieldTranslation.RegularExpression : formField.RegularExpression
                    };

                    if (tenantId == 104 && fieldType.Description == FieldTypeHelper.DatePicker)
                    {
                        jsonDataField.StartDate = campaign.StartDate.ToString();
                        jsonDataField.EndDate = campaign.EndDate.ToString();
                    }

                    var jsonFormfieldValueList = new List<GetFormFieldValueListDto>();
                    var dropdownListValues = new List<DropdownListDto>();

                    if (fieldType.Description == FieldTypeHelper.RetailerRadioButton)
                    {
                        var campaignRetailerLocationList = await _lookup_campaignRetailerLocationRepository.GetAll().Where(cr => cr.CampaignId == currentCampaignId).ToListAsync();
                        foreach (var retailer in campaignRetailerLocationList)
                        {
                            var retailerRadio = new DropdownListDto();
                            var retailerLocation = await _lookup_retailerLocationRepository.GetAll().FirstOrDefaultAsync(rl => rl.Id == retailer.RetailerLocationId);

                            retailerRadio.RetailerAddress = retailerLocation.Name;
                            retailerRadio.RetailerLocationId = retailerLocation.Id.ToString();

                            dropdownListValues.Add(retailerRadio);
                        }

                        jsonDataField.DropdownList = dropdownListValues;
                    }

                    if (fieldType.Description == FieldTypeHelper.RetailerLocation ||
                        fieldType.Description == FieldTypeHelper.Product)
                    {
                        if (formField.FieldName == FieldNameHelper.StorePurchased)
                        {
                            var campaignRetailerLocationList = await _lookup_campaignRetailerLocationRepository.GetAll().Where(cr => cr.CampaignId == currentCampaignId).ToListAsync();
                            foreach (var retailer in campaignRetailerLocationList)
                            {
                                var retailerDropdown = new DropdownListDto();
                                var retailerLocation = await _lookup_retailerLocationRepository.GetAll().Where(rl => rl.Id == retailer.RetailerLocationId).FirstOrDefaultAsync();
                                var retailerAddress = retailerLocation.Name + ", " + retailerLocation.Address + ", " + retailerLocation.PostalCode + ", " + retailerLocation.City;
                                //productHandlingLine.HandlingLineId = handlingLine.Id.ToString();
                                retailerDropdown.RetailerAddress = retailerAddress;
                                retailerDropdown.RetailerLocationId = retailerLocation.Id.ToString();

                                dropdownListValues.Add(retailerDropdown);
                            }
                            jsonDataField.DropdownList = dropdownListValues;
                        }

                        if (formField.FieldName == FieldNameHelper.Product)
                        {
                            var productHandling = await _productHandlingRepository.GetAll().Where(ph => ph.CampaignId == currentCampaignId).FirstOrDefaultAsync();
                            var handlingLine = await _lookup_handlingLineRepository.GetAll().Where(hl => hl.ProductHandlingId == productHandling.Id).FirstOrDefaultAsync();
                            var handlingLineProductsList = await _lookup_handlingLineProductRepository.GetAll().Where(hlp => hlp.HandlingLineId == handlingLine.Id).ToListAsync();

                            foreach (var product in handlingLineProductsList)
                            {
                                var productLookup = await _lookup_productRepository.GetAll().Where(p => p.Id == product.ProductId).FirstOrDefaultAsync();

                                var productDropdown = new DropdownListDto
                                {
                                    ProductId = productLookup.Id.ToString(),
                                    ProductDescription = productLookup.Description
                                };

                                dropdownListValues.Add(productDropdown);
                            }
                            jsonDataField.DropdownList = dropdownListValues;
                        }
                    }
                    else if (
                        fieldType.Description == FieldTypeHelper.DropdownMenu ||
                        fieldType.Description == FieldTypeHelper.CheckBox ||
                        fieldType.Description == FieldTypeHelper.RadioButton)
                    {
                        if (formField.FieldName == FieldNameHelper.ProductPremium)
                        {
                            var campaignHandlingLines =
                                from o in _lookup_handlingLineRepository.GetAll()
                                join o1 in _productHandlingRepository.GetAll() on o.ProductHandlingId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()
                                where s1.CampaignId == campaign.Id
                                select o;

                            var allProducts = await _lookup_productRepository.GetAllListAsync();
                            var premiums = new List<GetFormFieldValueListDto>();

                            foreach (var product in allProducts)
                            {
                                var handlingLines =
                                    from o in _lookup_handlingLineProductRepository.GetAll()
                                    join o1 in campaignHandlingLines on o.HandlingLineId equals o1.Id into j1
                                    from s1 in j1.DefaultIfEmpty()
                                    where o.ProductId == product.Id
                                    select s1;

                                foreach (var handlingLine in handlingLines)
                                {
                                    if (handlingLine == null || premiums.Any(x => x.ListValueTranslationKeyValue == handlingLine.Id.ToString()))
                                    {
                                        continue;
                                    }

                                    premiums.Add(new GetFormFieldValueListDto
                                    {
                                        ListValueTranslationKeyValue = handlingLine.Id.ToString(),
                                        ListValueTranslationDescription = handlingLine.PremiumDescription
                                    });
                                }
                            }

                            jsonFormfieldValueList.AddRange(premiums);
                        }
                        else if (formField.FieldName == FieldNameHelper.Country)
                        {
                            var campaignCountries =
                                from o in _lookup_CampaignCountryRepository.GetAll()
                                join o1 in _lookup_countryRepository.GetAll() on o.CountryId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()
                                where o.CampaignId == campaign.Id
                                select s1;

                            var countries = new List<GetFormFieldValueListDto>();

                            foreach (var campaignCountry in campaignCountries)
                            {
                                countries.Add(new GetFormFieldValueListDto
                                {
                                    ListValueTranslationKeyValue = campaignCountry.CountryCode,
                                    ListValueTranslationDescription = campaignCountry.Description 
                                });
                            }

                            jsonFormfieldValueList.AddRange(countries);
                        }

                        var formFieldValueLists = await _formFieldValueListRepository.GetAll().Where(ffv => ffv.FormFieldId == formBlockField.FormFieldId).ToListAsync();

                        foreach (var formFieldValueList in formFieldValueLists)
                        {
                            var valueList = await _valueListRepository.GetAll().Where(fbf => fbf.Id == formFieldValueList.ValueListId).FirstOrDefaultAsync();
                            var listValue = await _listValueRepository.GetAll().Where(fbf => fbf.ValueListId == formFieldValueList.ValueListId).OrderBy(fbf => fbf.SortOrder).FirstOrDefaultAsync();
                            var listValueTranslation = await _listValueTranslationRepository.GetAll().Where(fbf => fbf.ListValueId == listValue.Id).FirstOrDefaultAsync();
                            var listValues = await _listValueRepository.GetAll().Where(lv => lv.ValueListId == formFieldValueList.ValueListId).OrderBy(lv => lv.SortOrder).ToListAsync();

                            foreach (var item in listValues)
                            {
                                var formfieldValueListDto = new GetFormFieldValueListDto();
                                var jsonHandlingLine = new List<GetFormHandlingLineDto>();

                                var listTranslation = string.Empty;
                                var listKeyValue = string.Empty;

                                var translation = await _listValueTranslationRepository.GetAll().FirstOrDefaultAsync(x => x.ListValueId == item.Id && x.LocaleId == formLocale.LocaleId);
                                listTranslation = translation.Description;
                                var keyValue = await _listValueTranslationRepository.GetAll().FirstOrDefaultAsync(x => x.ListValueId == item.Id && x.LocaleId == formLocale.LocaleId);
                                listKeyValue = keyValue.KeyValue;

                                formfieldValueListDto.ListValueTranslationKeyValue = listKeyValue;
                                formfieldValueListDto.ListValueTranslationDescription = listTranslation;

                                var productLookup = await _lookup_productRepository.GetAll().Where(p => p.ProductCode == listKeyValue).FirstOrDefaultAsync();
                                if (productLookup != null)
                                {
                                    var handlingLineProduct = await _lookup_handlingLineProductRepository.GetAll().Where(hlp => hlp.ProductId == Convert.ToInt32(productLookup.Id)).ToListAsync();
                                    foreach (var handling in handlingLineProduct)
                                    {
                                        var handlingLine = await _lookup_handlingLineRepository.GetAll().Where(fl => fl.Id == handling.HandlingLineId).FirstOrDefaultAsync();

                                        var productHandlingLine = new GetFormHandlingLineDto
                                        {
                                            HandlingLineId = handlingLine.Id.ToString(),
                                            HandlingLineDescription = handlingLine.PremiumDescription,
                                            ChosenItemId = handling.ProductId.ToString()
                                        };

                                        jsonHandlingLine.Add(productHandlingLine);
                                    }
                                }

                                var retailer = await _lookup_retailerRepository.GetAll().Where(r => r.Code == listKeyValue).FirstOrDefaultAsync();
                                if (retailer != null)
                                {
                                    var retailerLocations = await _lookup_retailerLocationRepository.GetAll().Where(r => r.RetailerId == retailer.Id).ToListAsync();

                                    foreach (var retailerLocation in retailerLocations)
                                    {
                                        var handlingLineRetailerLocation = await _lookup_handlingLineRetailerRepository.GetAll().Where(fl => fl.RetailerId == retailerLocation.RetailerId).FirstOrDefaultAsync();
                                        var retailerHandlingLine = new GetFormHandlingLineDto
                                        {
                                            HandlingLineId = handlingLineRetailerLocation.HandlingLineId.ToString()
                                        };
                                        var address = retailerLocation.Address + ", " + retailerLocation.PostalCode + ", " + retailerLocation.City;

                                        retailerHandlingLine.HandlingLineDescription = address;
                                        retailerHandlingLine.ChosenItemId = retailerLocation.RetailerId.ToString();

                                        jsonHandlingLine.Add(retailerHandlingLine);
                                    }
                                }

                                formfieldValueListDto.HandlingLine = jsonHandlingLine;
                                jsonFormfieldValueList.Add(formfieldValueListDto);
                            }
                        }
                    }

                    jsonDataField.FormFieldValueList = jsonFormfieldValueList;
                    jsonFormfields.Add(jsonDataField);
                }

                jsonDataBlock.ExportFields = jsonFormfields;
                jsonFormBlocks.Add(jsonDataBlock);
            }

            jsonDataLocale.ExportBlocks = jsonFormBlocks;
            wrapDto.Formbuilder = jsonDataLocale;

            return wrapDto;
        }

        [WrapResult(WrapOnSuccess = false, WrapOnError = false)]
        public async Task<GetFormAndProductHandelingDto> GetEditFormHandling(long currentRegistrationId)
        {
            var myTenantId = AbpSession.TenantId ?? 0;
            var wrapDto = new GetFormAndProductHandelingDto();
            var jsonDataLocale = new FormExportJsonDto();
            var getRegistration = await _registrationRepository.GetAsync(currentRegistrationId);
            var getRegistrationFieldData = await _lookup_registrationFieldDataRepository.GetAll().Where(cr => cr.RegistrationId == currentRegistrationId).ToListAsync();
            var getPurchaseRegistrations = await _lookup_purchaseRegistrationRepository.GetAll().Where(pr => pr.RegistrationId == currentRegistrationId).ToListAsync();
            //var getPurchaseRegistrationFieldData = await _lookup_purchaseRegistrationFieldDataRepository.GetAll().Where(cr => cr.PurchaseRegistrationId == currentRegistrationId).ToListAsync();

            var campaignForm = await _lookup_campaignFormRepository.GetAll().FirstOrDefaultAsync(cf => cf.CampaignId == getRegistration.CampaignId);

            var formLocale = await _formLocaleRepository.GetAll().FirstOrDefaultAsync(fl => fl.FormId == campaignForm.FormId);
            if (formLocale == null)
            {
                return null;
            }


            var jsonFormBlocks = new List<FormBlocksExportJsonDto>();
            var formBlocks = await _formBlockRepository.GetAll().Where(flb => flb.FormLocaleId == formLocale.Id).OrderBy(flb => flb.SortOrder).ToListAsync();
            foreach (var formBlock in formBlocks)
            {
                var jsonDataBlock = new FormBlocksExportJsonDto
                {
                    Description = formBlock.Description,
                    PurchaseRegistration = formBlock.IsPurchaseRegistration
                };

                var jsonFormfields = new List<FormFieldsExportJsonDto>();
                var formBlockFields = await _formBlockFieldRepository.GetAll().Where(fbf => fbf.FormBlockId == formBlock.Id).OrderBy(fbf => fbf.SortOrder).ToListAsync();

                var formBlockFieldsWithDetails = (from o in formBlockFields
                                                  join o1 in _formFieldRepository.GetAll() on o.FormFieldId equals o1.Id into j1
                                                  from s1 in j1.DefaultIfEmpty()
                                                  join o2 in _fieldTypeRepository.GetAll() on s1.FieldTypeId equals o2.Id into j2
                                                  from s2 in j2.DefaultIfEmpty()
                                                  orderby o.SortOrder
                                                  select new
                                                  {
                                                      FieldId = s1.Id,
                                                      FieldType = s2.Description,
                                                      FieldLabel = s1.Label,
                                                      RegistrationField = s1.RegistrationField,
                                                      PurchaseRegistrationField = s1.PurchaseRegistrationField

                                                  }).ToList();

                var jsonFormfieldValueList = new List<GetFormFieldValueListDto>();
                var dropdownListValues = new List<DropdownListDto>();

                foreach (var formBlockField in formBlockFieldsWithDetails)
                {
                    var jsonDataField = new FormFieldsExportJsonDto();
                    var formField = await _formFieldRepository.GetAsync(formBlockField.FieldId);

                    var fieldType = await _fieldTypeRepository.GetAll().FirstOrDefaultAsync(fl => fl.Id == formField.FieldTypeId);
                    var formFieldTranslation = await _lookup_formFieldTranslationRepository.GetAll().FirstOrDefaultAsync(fft => fft.FormFieldId == formBlockField.FieldId & fft.LocaleId == formLocale.LocaleId);

                    if (!string.IsNullOrEmpty(formBlockField.RegistrationField))
                    {
                        var fieldValue = typeof(Registration).GetProperty(formField.RegistrationField).GetValue(getRegistration, null);

                        jsonDataField.Description = formField.FieldName;
                        jsonDataField.Label = formFieldTranslation.Label;
                        jsonDataField.MaxLength = formField.MaxLength;
                        jsonDataField.DefaultValue = fieldValue.ToString();
                        jsonDataField.Required = formField.Required;
                        jsonDataField.InputMask = formField.InputMask;
                        jsonDataField.FieldType = fieldType.Description;
                        jsonDataField.PurchaseRegistrationField = formField.PurchaseRegistrationField;
                        jsonDataField.RegularExpression = formField.RegularExpression;

                        jsonFormfields.Add(jsonDataField);
                    }
                    else if (!string.IsNullOrEmpty(formBlockField.PurchaseRegistrationField))
                    {
                        foreach (var purchaseRegistration in getPurchaseRegistrations)
                        {
                            var fieldValue = typeof(PurchaseRegistration).GetProperty(formField.PurchaseRegistrationField).GetValue(purchaseRegistration, null);

                            //var getPurchaseRegistrationFieldData = await _lookup_purchaseRegistrationFieldDataRepository.GetAll().Where(pr => pr.PurchaseRegistrationId == purchaseRegistration.Id).ToListAsync();

                            jsonDataField.Description = formField.FieldName;
                            jsonDataField.Label = formFieldTranslation.Label;
                            jsonDataField.MaxLength = formField.MaxLength;
                            jsonDataField.DefaultValue = formField.DefaultValue;
                            jsonDataField.Required = formField.Required;
                            jsonDataField.InputMask = formField.InputMask;
                            jsonDataField.FieldType = fieldType.Description;
                            jsonDataField.RegularExpression = formField.RegularExpression;
                            jsonDataField.PurchaseRegistrationField = formField.PurchaseRegistrationField;

                            if (fieldType.Description == "RetailerRadioButton")
                            {
                                var campaignRetailerLocationList = await _lookup_campaignRetailerLocationRepository.GetAll()
                                    .Where(cr => cr.CampaignId == getRegistration.CampaignId).ToListAsync();

                                foreach (var retailer in campaignRetailerLocationList)
                                {
                                    var retailerRadio = new DropdownListDto();
                                    var retailerLocation = await _lookup_retailerLocationRepository.GetAll().Where(rl => rl.Id == retailer.RetailerLocationId).FirstOrDefaultAsync();

                                    retailerRadio.RetailerAddress = retailerLocation.Name;
                                    retailerRadio.RetailerLocationId = retailerLocation.Id.ToString();

                                    dropdownListValues.Add(retailerRadio);
                                }

                                jsonDataField.DropdownList = dropdownListValues;
                            }

                            jsonFormfields.Add(jsonDataField);
                        }
                    }
                    else if (
                        formBlockField.FieldType == FieldSourceHelper.Product ||
                        formBlockField.FieldType == FieldSourceHelper.RetailerLocation)
                    {

                        if (formField.FieldName == "StorePurchased")
                        {
                            var campaignRetailerLocationList = await _lookup_campaignRetailerLocationRepository.GetAll()
                                .Where(cr => cr.CampaignId == getRegistration.CampaignId).ToListAsync();

                            foreach (var retailer in campaignRetailerLocationList)
                            {
                                var retailerDropdown = new DropdownListDto();
                                var retailerLocation = await _lookup_retailerLocationRepository.GetAll().Where(rl => rl.Id == retailer.RetailerLocationId).FirstOrDefaultAsync();
                                var retailerAddress = retailerLocation.Name + ", " + retailerLocation.Address + ", " + retailerLocation.PostalCode + ", " + retailerLocation.City;
                                //productHandlingLine.HandlingLineId = handlingLine.Id.ToString();
                                retailerDropdown.RetailerAddress = retailerAddress;
                                retailerDropdown.RetailerLocationId = retailerLocation.Id.ToString();

                                dropdownListValues.Add(retailerDropdown);
                            }

                            jsonDataField.DropdownList = dropdownListValues;
                        }

                        if (formField.FieldName == "Product")
                        {
                            var productHandling = await _productHandlingRepository.GetAll().Where(ph => ph.CampaignId == getRegistration.CampaignId).FirstOrDefaultAsync();
                            var handlingLine = await _lookup_handlingLineRepository.GetAll().Where(hl => hl.ProductHandlingId == productHandling.Id).FirstOrDefaultAsync();
                            var handlingLineProductsList = await _lookup_handlingLineProductRepository.GetAll().Where(hlp => hlp.HandlingLineId == handlingLine.Id).ToListAsync();

                            foreach (var product in handlingLineProductsList)
                            {
                                var productLookup = await _lookup_productRepository.GetAll().Where(p => p.Id == product.Id).FirstOrDefaultAsync();

                                var productDropdown = new DropdownListDto
                                {
                                    ProductId = productLookup.Id.ToString(),
                                    ProductDescription = productLookup.Description
                                };

                                dropdownListValues.Add(productDropdown);
                            }

                            jsonDataField.DropdownList = dropdownListValues;
                        }

                    }
                    else if (formBlockField.FieldType == FieldSourceHelper.RetailerRadioButton)
                    {

                    }
                    else if (
                        formBlockField.FieldType == FieldTypeHelper.DropdownMenu ||
                        formBlockField.FieldType == FieldTypeHelper.CheckBox ||
                        formBlockField.FieldType == FieldTypeHelper.RadioButton)
                    {
                        var formFieldValueLists = await _formFieldValueListRepository.GetAll().Where(ffv => ffv.FormFieldId == formBlockField.FieldId).ToListAsync();
                        foreach (var formFieldValueList in formFieldValueLists)
                        {
                            var valueList = await _valueListRepository.GetAll().Where(fbf => fbf.Id == formFieldValueList.ValueListId).FirstOrDefaultAsync();
                            var listValue = await _listValueRepository.GetAll().Where(fbf => fbf.ValueListId == formFieldValueList.ValueListId).OrderBy(fbf => fbf.SortOrder).FirstOrDefaultAsync();
                            var listValueTranslation = await _listValueTranslationRepository.GetAll().Where(fbf => fbf.ListValueId == listValue.Id).FirstOrDefaultAsync();
                            var listValues = await _listValueRepository.GetAll().Where(lv => lv.ValueListId == formFieldValueList.ValueListId).OrderBy(lv => lv.SortOrder).ToListAsync();

                            foreach (var item in listValues)
                            {
                                var formfieldValueListDto = new GetFormFieldValueListDto();
                                var jsonHandlingLine = new List<GetFormHandlingLineDto>();

                                var listTranslation = string.Empty;
                                var listKeyValue = string.Empty;

                                var translation = await _listValueTranslationRepository.GetAll().FirstOrDefaultAsync(x => x.ListValueId == item.Id && x.LocaleId == formLocale.LocaleId);
                                listTranslation = translation.Description;
                                var keyValue = await _listValueTranslationRepository.GetAll().FirstOrDefaultAsync(x => x.ListValueId == item.Id && x.LocaleId == formLocale.LocaleId);
                                listKeyValue = keyValue.KeyValue;

                                formfieldValueListDto.ListValueTranslationKeyValue = listKeyValue;
                                formfieldValueListDto.ListValueTranslationDescription = listTranslation;

                                var productLookup = await _lookup_productRepository.GetAll().FirstOrDefaultAsync(p => p.ProductCode == listKeyValue);
                                if (productLookup != null)
                                {
                                    var handlingLineProduct = await _lookup_handlingLineProductRepository.GetAll()
                                        .Where(hlp => hlp.ProductId == Convert.ToInt32(productLookup.Id)).ToListAsync();

                                    foreach (var handling in handlingLineProduct)
                                    {
                                        var productHandlingLine = new GetFormHandlingLineDto();

                                        var handlingLine = await _lookup_handlingLineRepository.GetAll().FirstOrDefaultAsync(fl => fl.Id == handling.HandlingLineId);
                                        productHandlingLine.HandlingLineId = handlingLine.Id.ToString();
                                        productHandlingLine.HandlingLineDescription = handlingLine.PremiumDescription;
                                        productHandlingLine.ChosenItemId = handling.ProductId.ToString();

                                        jsonHandlingLine.Add(productHandlingLine);
                                    }
                                }

                                var retailer = await _lookup_retailerRepository.GetAll().FirstOrDefaultAsync(r => r.Code == listKeyValue);
                                if (retailer != null)
                                {
                                    var retailerLocations = await _lookup_retailerLocationRepository.GetAll().Where(r => r.RetailerId == retailer.Id).ToListAsync();


                                    foreach (var retailerLocation in retailerLocations)
                                    {
                                        var retailerHandlingLine = new GetFormHandlingLineDto();
                                        var handlingLineRetailerLocation = await _lookup_handlingLineRetailerRepository.GetAll().FirstOrDefaultAsync(fl => fl.RetailerId == retailerLocation.RetailerId);
                                        retailerHandlingLine.HandlingLineId = handlingLineRetailerLocation.HandlingLineId.ToString();

                                        var address = retailerLocation.Address + ", " + retailerLocation.PostalCode + ", " + retailerLocation.City;
                                        retailerHandlingLine.HandlingLineDescription = address;
                                        retailerHandlingLine.ChosenItemId = retailerLocation.RetailerId.ToString();

                                        jsonHandlingLine.Add(retailerHandlingLine);
                                    }
                                }

                                formfieldValueListDto.HandlingLine = jsonHandlingLine;
                                jsonFormfieldValueList.Add(formfieldValueListDto);
                            }
                        }
                    }
                    else // Custom field, either on Registration level or PurchaseRegistration level...
                    {
                        var customRegistrationField = (from o in getRegistrationFieldData
                                                       join o1 in _registrationFieldRepository.GetAll() on o.RegistrationFieldId equals o1.Id
                                                       where o1.FormFieldId == formBlockField.FieldId
                                                       select o).FirstOrDefault();

                        //var customPurchaseRegistrationField = (from o in getPurchaseRegistrationFieldData
                        //                               join o1 in _registrationFieldRepository.GetAll() on o.RegistrationFieldId equals o1.Id
                        //                               where o1.FormFieldId == formBlockField.FieldId
                        //                               select o).FirstOrDefault();
                    }
                }

                jsonDataBlock.ExportFields = jsonFormfields;
                jsonFormBlocks.Add(jsonDataBlock);
            }

            jsonDataLocale.ExportBlocks = jsonFormBlocks;
            wrapDto.Formbuilder = jsonDataLocale;

            return wrapDto;
        }

        public async Task<GetFormLocaleForViewDto> GetFormLocaleForView(long id)
        {
            var formLocale = await _formLocaleRepository.GetAsync(id);

            var output = new GetFormLocaleForViewDto { FormLocale = ObjectMapper.Map<FormLocaleDto>(formLocale) };

            var _lookupForm = await _lookup_formRepository.FirstOrDefaultAsync((long)output.FormLocale.FormId);
            output.FormVersion = _lookupForm?.Version?.ToString();

            if (output.FormLocale.LocaleId != null)
            {
                var _lookupLocale = await _lookup_localeRepository.FirstOrDefaultAsync((long)output.FormLocale.LocaleId);
                output.LocaleDescription = _lookupLocale?.Description?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_FormLocales_Edit)]
        public async Task<GetFormLocaleForEditOutput> GetFormLocaleForEdit(EntityDto<long> input)
        {
            var formLocale = await _formLocaleRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetFormLocaleForEditOutput { FormLocale = ObjectMapper.Map<CreateOrEditFormLocaleDto>(formLocale) };

            if (output.FormLocale.FormId != null)
            {
                var _lookupForm = await _lookup_formRepository.FirstOrDefaultAsync((long)output.FormLocale.FormId);
                output.FormVersion = _lookupForm?.Version?.ToString();
            }

            if (output.FormLocale.LocaleId != null)
            {
                var _lookupLocale = await _lookup_localeRepository.FirstOrDefaultAsync((long)output.FormLocale.LocaleId);
                output.LocaleDescription = _lookupLocale?.Description?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditFormLocaleDto input)
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

        [AbpAuthorize(AppPermissions.Pages_FormLocales_Create)]
        protected virtual async Task Create(CreateOrEditFormLocaleDto input)
        {
            var formLocale = ObjectMapper.Map<FormLocale>(input);


            if (AbpSession.TenantId != null)
            {
                formLocale.TenantId = (int?)AbpSession.TenantId;
            }


            await _formLocaleRepository.InsertAsync(formLocale);
        }

        [AbpAuthorize(AppPermissions.Pages_FormLocales_Edit)]
        protected virtual async Task Update(CreateOrEditFormLocaleDto input)
        {
            var formLocale = await _formLocaleRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, formLocale);
        }

        [AbpAuthorize(AppPermissions.Pages_FormLocales_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _formLocaleRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetFormLocalesToExcel(GetAllFormLocalesForExcelInput input)
        {

            var filteredFormLocales = _formLocaleRepository.GetAll()
                        .Include(e => e.FormFk)
                        .Include(e => e.LocaleFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FormVersionFilter), e => e.FormFk != null && e.FormFk.Version == input.FormVersionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LocaleDescriptionFilter), e => e.LocaleFk != null && e.LocaleFk.Description == input.LocaleDescriptionFilter);

            var query = (from o in filteredFormLocales
                         join o1 in _lookup_formRepository.GetAll() on o.FormId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_localeRepository.GetAll() on o.LocaleId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetFormLocaleForViewDto()
                         {
                             FormLocale = new FormLocaleDto
                             {
                                 Description = o.Description,
                                 Id = o.Id
                             },
                             FormVersion = s1 == null || s1.Version == null ? "" : s1.Version.ToString(),
                             LocaleDescription = s2 == null || s2.Description == null ? "" : s2.Description.ToString()
                         });


            var formLocaleListDtos = await query.ToListAsync();

            return _formLocalesExcelExporter.ExportToFile(formLocaleListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_FormLocales)]
        public async Task<PagedResultDto<FormLocaleFormLookupTableDto>> GetAllFormForLookupTable(RMS.SBJ.Forms.Dtos.GetAllForLookupTableInput input)
        {
            var query = _lookup_formRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Version != null && e.Version.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var formLookupList = await query
                .PageBy(input)
                .ToListAsync();

            var formsList = new List<Form>();
            if (input.CurrentFormPage == 0)
            {
                formsList = formLookupList;
            }
            else
            {
                formsList = formLookupList.Where(item => item.SystemLevelId == input.CurrentFormPage).ToList();
            }

            var lookupTableDtoList = new List<FormLocaleFormLookupTableDto>();
            foreach (var form in formsList)
            {
                lookupTableDtoList.Add(new FormLocaleFormLookupTableDto
                {
                    Id = form.Id,
                    DisplayName = form.Version?.ToString()
                });
            }


            return new PagedResultDto<FormLocaleFormLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_FormLocales)]
        public async Task<PagedResultDto<FormLocaleLocaleLookupTableDto>> GetAllLocaleForLookupTable(RMS.SBJ.Forms.Dtos.GetAllForLookupTableInput input)
        {
            var query = _lookup_localeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Description != null && e.Description.Contains(input.Filter)
               );

            var localeList = await query.Where(item => item.IsActive)
                .PageBy(input)
                .ToListAsync();


            var lookupTableDtoList = new List<FormLocaleLocaleLookupTableDto>();

            #region FormLocale Locale lookup for Company
            if (input.CurrentFormPage == FormConsts.CompanySLId)
            {
                var localeIdList = localeList.Select(item => item.Id);
                var latestCompanyFormVersion = _lookup_formRepository.GetAllList().Where(item => item.SystemLevelId == FormConsts.CompanySLId).LastOrDefault();
                var formLocale = _formLocaleRepository.GetAllList();
                var companyLevelFormLocales = formLocale.Where(item => item.FormId == latestCompanyFormVersion.Id);
                var localeIdsOfCompanyLevelFormLocales = companyLevelFormLocales.Select(item => item.LocaleId).ToList();
                var unmappedCompanyLevelLocales = localeIdList.Except(localeIdsOfCompanyLevelFormLocales);

                foreach (var locale in unmappedCompanyLevelLocales)
                {
                    lookupTableDtoList.Add(new FormLocaleLocaleLookupTableDto
                    {
                        Id = locale,
                        DisplayName = localeList.Where(item => item.Id == locale).LastOrDefault().Description
                    });
                }
            }
            #endregion
            else
            {
                foreach (var locale in localeList)
                {
                    lookupTableDtoList.Add(new FormLocaleLocaleLookupTableDto
                    {
                        Id = locale.Id,
                        DisplayName = locale.Description?.ToString()
                    });
                }
            }

            var totalCount = lookupTableDtoList.Count();

            return new PagedResultDto<FormLocaleLocaleLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        public async Task<List<GetFormLocaleForViewDto>> GetAllFormLocales()
        {
            var allFormLocales = _formLocaleRepository.GetAll()
                        .Include(e => e.FormFk)
                        .Include(e => e.LocaleFk);

            var formLocales = from o in allFormLocales
                              join o1 in _lookup_formRepository.GetAll() on o.FormId equals o1.Id into j1
                              from s1 in j1.DefaultIfEmpty()

                              join o2 in _lookup_localeRepository.GetAll() on o.LocaleId equals o2.Id into j2
                              from s2 in j2.DefaultIfEmpty()

                              select new GetFormLocaleForViewDto()
                              {
                                  FormLocale = new FormLocaleDto
                                  {
                                      Description = o.Description,
                                      Id = o.Id,
                                      FormId = o.FormId,
                                      LocaleId = o.LocaleId
                                  },
                                  FormVersion = s1 == null || s1.Version == null ? "" : s1.Version.ToString(),
                                  LocaleDescription = s2 == null || s2.Description == null ? "" : s2.Description.ToString()
                              };

            var totalCount = await allFormLocales.CountAsync();

            return formLocales.ToList();
        }

        [AbpAuthorize(AppPermissions.Pages_FormLocales)]
        public async Task<List<FormLocaleLocaleLookupTableDto>> GetAllLocaleForTableDropdown()
        {
            return await _lookup_localeRepository.GetAll()
                .Select(locale => new FormLocaleLocaleLookupTableDto
                {
                    Id = locale.Id,
                    DisplayName = locale == null || locale.LanguageCode == null ? "" : locale.LanguageCode.ToString()
                }).ToListAsync();
        }

        public async Task<long> CreateOrEditAndGetId(CreateOrEditFormLocaleDto input)
        {
            if (input.Id == null)
            {
                var formLocale = ObjectMapper.Map<FormLocale>(input);
                if (AbpSession.TenantId != null)
                {
                    formLocale.TenantId = (int?)AbpSession.TenantId;
                }
                return await _formLocaleRepository.InsertAndGetIdAsync(formLocale);
            }
            else
            {
                var formLocale = await _formLocaleRepository.FirstOrDefaultAsync((long)input.Id);
                ObjectMapper.Map(input, formLocale);
                return formLocale.Id;
            }
        }
    }
}

