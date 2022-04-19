using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using RMS.Authorization;
using RMS.Dto;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.SBJ.CampaignProcesses.Exporting;
using RMS.SBJ.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.SystemTables;
using Newtonsoft.Json;
using RMS.SBJ.ProductHandlings;
using RMS.SBJ.Company;
using RMS.SBJ.CampaignRetailerLocations;
using RMS.SBJ.RetailerLocations;
using RMS.SBJ.Retailers;
using RMS.SBJ.HandlingLines;
using RMS.SBJ.HandlingLineRetailers;
using RMS.SBJ.HandlingLineProducts;
using RMS.SBJ.Products;
using RMS.Web.Models.AzureBlobStorage;
using Microsoft.Extensions.Options;

namespace RMS.SBJ.CampaignProcesses
{
    [AbpAuthorize(AppPermissions.Pages_Campaigns)]
    public class CampaignsAppService : RMSAppServiceBase, ICampaignsAppService
    {
        private readonly IRepository<Campaign, long> _campaignRepository;
        private readonly IRepository<CampaignType, long> _campaignTypeRepository;
        private readonly IRepository<CampaignCampaignType, long> _campaignCampaignTypeRepository;
        private readonly IRepository<CampaignTypeEvent, long> _campaignTypeEventRepository;
        private readonly IRepository<CampaignTypeEventRegistrationStatus, long> _campaignTypeEventRegistrationStatusRepository;
        private readonly ICampaignsExcelExporter _campaignsExcelExporter;
        private readonly IRepository<Form, long> _lookup_formRepository;
        private readonly IRepository<CampaignForm, long> _lookup_campaignFormRepository;
        private readonly IRepository<CampaignTranslation, long> _lookup_campaignTranslationRepository;
        private readonly IRepository<CampaignMessage, long> _lookup_campaignMessageRepository;
        private readonly IRepository<Message, long> _lookup_messageRepository;
        private readonly IRepository<MessageType, long> _lookup_messageTypeRepository;
        private readonly IRepository<MessageComponent, long> _lookup_messageComponentRepository;
        private readonly IRepository<MessageComponentContent, long> _lookup_messageComponentContentRepository;
        private readonly IRepository<MessageContentTranslation, long> _lookup_messageContentTranslationRepository;
        private readonly IRepository<FormLocale, long> _formLocaleRepository;
        private readonly IRepository<FormBlock, long> _formBlockRepository;
        private readonly IRepository<FormBlockField, long> _formBlockFieldRepository;
        private readonly IRepository<Locale, long> _lookup_localeRepository;
        private readonly IRepository<Country, long> _lookup_countryRepository;
        private readonly IRepository<CampaignRetailerLocation, long> _lookup_campaignRetailerLocationRepository;
        private readonly IRepository<RetailerLocation, long> _lookup_retailerLocationRepository;
        private readonly IRepository<Retailer, long> _lookup_retailerRepository;
        private readonly IRepository<HandlingLine, long> _lookup_handlingLineRepository;
        private readonly IRepository<HandlingLineRetailer, long> _lookup_handlingLineRetailerRepository;
        private readonly IRepository<HandlingLineProduct, long> _lookup_handlingLineProductRepository;
        private readonly IRepository<ProductHandling, long> _productHandlingRepository;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IRepository<FormLocale, long> _lookup_formLocaleRepository;
        private readonly AzureBlobStorageSettingsModel _options;

        public CampaignsAppService(
            IRepository<Campaign, long> campaignRepository,
            IRepository<CampaignType, long> campaignTypeRepository,
            IRepository<CampaignCampaignType, long> campaignCampaignTypeRepository,
            IRepository<CampaignTypeEvent, long> campaignTypeEventRepository,
            IRepository<CampaignTypeEventRegistrationStatus, long> campaignTypeEventRegistrationStatusRepository,
            ICampaignsExcelExporter campaignsExcelExporter,
            IRepository<Form, long> lookup_formRepository,
            IRepository<FormLocale, long> formLocaleRepository,
            IRepository<FormBlock, long> formBlockRepository,
            IRepository<FormBlockField, long> formBlockFieldRepository,
            IRepository<CampaignForm, long> lookup_campaignFormRepository,
            IRepository<CampaignTranslation, long> lookup_campaignTranslationRepository,
            IRepository<CampaignMessage, long> lookup_campaignMessageRepository,
            IRepository<Message, long> lookup_messageRepository,
            IRepository<MessageType, long> lookup_messageTypeRepository,
            IRepository<MessageComponent, long> lookup_messageComponentRepository,
            IRepository<MessageComponentContent, long> lookup_messageComponentContentRepository,
            IRepository<MessageContentTranslation, long> lookup_messageContentTranslationRepository,
            IRepository<Locale, long> lookup_localeRepository,
            IRepository<Company.Company, long> companyRepository,
            IRepository<Address, long> lookup_addressRepository,
            IRepository<Country, long> lookup_countryRepository,
            IRepository<CampaignRetailerLocation, long> lookup_campaignRetailerLocationRepository,
            IRepository<RetailerLocation, long> lookup_retailerLocationRepository,
            IRepository<Retailer, long> lookup_retailerRepository,
            IRepository<HandlingLine, long> lookup_handlingLineRepository,
            IRepository<HandlingLineRetailer, long> lookup_handlingLineRetailerRepository,
            IRepository<HandlingLineProduct, long> lookup_handlingLineProductRepository,
            IRepository<ProductHandling, long> productHandlingRepository,
            IRepository<Product, long> lookup_productRepository,
            IRepository<FormLocale, long> lookup_formLocaleRepository,
            IOptions<AzureBlobStorageSettingsModel> options
            )
        {
            _campaignRepository = campaignRepository;
            _campaignTypeRepository = campaignTypeRepository;
            _campaignCampaignTypeRepository = campaignCampaignTypeRepository;
            _campaignTypeEventRepository = campaignTypeEventRepository;
            _campaignTypeEventRegistrationStatusRepository = campaignTypeEventRegistrationStatusRepository;
            _campaignsExcelExporter = campaignsExcelExporter;
            _lookup_formRepository = lookup_formRepository;
            _lookup_campaignFormRepository = lookup_campaignFormRepository;
            _lookup_campaignTranslationRepository = lookup_campaignTranslationRepository;
            _lookup_campaignMessageRepository = lookup_campaignMessageRepository;
            _lookup_messageRepository = lookup_messageRepository;
            _lookup_messageTypeRepository = lookup_messageTypeRepository;
            _lookup_messageComponentRepository = lookup_messageComponentRepository;
            _lookup_messageComponentContentRepository = lookup_messageComponentContentRepository;
            _lookup_messageContentTranslationRepository = lookup_messageContentTranslationRepository;
            _formLocaleRepository = formLocaleRepository;
            _formBlockRepository = formBlockRepository;
            _formBlockFieldRepository = formBlockFieldRepository;
            _lookup_localeRepository = lookup_localeRepository;
            _lookup_countryRepository = lookup_countryRepository;
            _lookup_campaignRetailerLocationRepository = lookup_campaignRetailerLocationRepository;
            _lookup_retailerLocationRepository = lookup_retailerLocationRepository;
            _lookup_retailerRepository = lookup_retailerRepository;
            _lookup_handlingLineRepository = lookup_handlingLineRepository;
            _lookup_handlingLineRetailerRepository = lookup_handlingLineRetailerRepository;
            _lookup_handlingLineProductRepository = lookup_handlingLineProductRepository;
            _productHandlingRepository = productHandlingRepository;
            _lookup_productRepository = lookup_productRepository;
            _lookup_formLocaleRepository = lookup_formLocaleRepository;
            _options = options.Value;

        }

        public async Task<PagedResultDto<GetCampaignForViewDto>> GetAll(GetAllCampaignsInput input)
        {
            var filteredCampaigns = _campaignRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(input.MinCampaignCodeFilter != null, e => e.CampaignCode >= input.MinCampaignCodeFilter)
                        .WhereIf(input.MaxCampaignCodeFilter != null, e => e.CampaignCode <= input.MaxCampaignCodeFilter);

            var pagedAndFilteredCampaigns = filteredCampaigns
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var campaigns = from o in pagedAndFilteredCampaigns
                            select new GetCampaignForViewDto()
                            {
                                Campaign = new CampaignDto
                                {
                                    Name = o.Name,
                                    Description = o.Description,
                                    StartDate = o.StartDate,
                                    EndDate = o.EndDate,
                                    CampaignCode = o.CampaignCode,
                                    ExternalCode = o.ExternalCode,
                                    Id = o.Id
                                }
                            };

            var totalCount = await filteredCampaigns.CountAsync();

            return new PagedResultDto<GetCampaignForViewDto>(
                totalCount,
                await campaigns.ToListAsync()
            );
        }

        public async Task<int> GetSuggestedNewCampaignCode()
        {
            var lastTwoCampaigns = new List<Campaign>();
            var campaignCount = await _campaignRepository.GetAll().Where(c => c.CampaignCode.HasValue).CountAsync();
            var takeCount = campaignCount >= 2 ? 2 : campaignCount;
            var newCampaignCode = AbpSession.TenantId.Value * 10010;

            lastTwoCampaigns = await _campaignRepository.GetAll().Where(c => c.CampaignCode.HasValue).OrderByDescending(c => c.CampaignCode).Take(takeCount).ToListAsync();

            switch (takeCount)
            {
                case 1:
                    newCampaignCode = lastTwoCampaigns[0].CampaignCode.Value + 10;
                    break;
                case 2:
                    var seed = lastTwoCampaigns[0].CampaignCode.Value - lastTwoCampaigns[1].CampaignCode.Value;
                    newCampaignCode = seed > 0 ? lastTwoCampaigns[0].CampaignCode.Value + seed : lastTwoCampaigns[0].CampaignCode.Value + 10;
                    break;
            }

            return newCampaignCode;
        }

        public async Task<List<GetCampaignLocalesDto>> GetCampaignLocales()
        {
            var jsonLocaleFormDto = new List<GetCampaignLocalesDto>();
            var activeCampaignForms = await _lookup_campaignFormRepository.GetAll().Where(fl => fl.IsActive == true).ToListAsync();
            foreach (var activeCampaignForm in activeCampaignForms)
            {
                var campaignTable = await _campaignRepository.GetAsync(activeCampaignForm.CampaignId);
                var formTable = await _lookup_formRepository.GetAll().Where(fl => fl.Id == activeCampaignForm.FormId).FirstOrDefaultAsync();

                var GetFormLocaleDto = new List<GetFormLocalesDto>();
                var formLocaleTable = await _formLocaleRepository.GetAll().Where(fl => fl.FormId == activeCampaignForm.FormId).OrderBy(fl => fl.Description).ThenBy(fl => fl.Id).ToListAsync();
                foreach (var SelectLocales in formLocaleTable)
                {
                    var campaignLocales = new GetCampaignLocalesDto();
                    var localeRecord = await _lookup_localeRepository.GetAsync(SelectLocales.LocaleId);
                    var countryRecord = await _lookup_countryRepository.GetAll().Where(fl => fl.Id == localeRecord.CountryId).FirstOrDefaultAsync();

                    campaignLocales.CampaignName = campaignTable.Name;
                    campaignLocales.CampaignId = activeCampaignForm.CampaignId;
                    campaignLocales.CampaignCode = (long)campaignTable.CampaignCode;
                    campaignLocales.CampaignDescription = campaignTable.Description;
                    campaignLocales.Version = formTable.Version;
                    campaignLocales.LocaleDescription = $"{countryRecord.CountryCode.ToLower()}_{localeRecord.LanguageCode}";
                    campaignLocales.LocaleId = SelectLocales.LocaleId;
                    campaignLocales.StartDate = campaignTable.StartDate;
                    campaignLocales.EndDate = campaignTable.EndDate;
                    
                    var localeName = string.Empty;
                    switch (campaignLocales.LocaleDescription)
                    {
                        case "nl_nl":
                            localeName = "Nederland - Nederlands";
                            break;

                        case "be_nl":
                            localeName = "België - Nederlands";
                            break;

                        case "be_fr":
                            localeName = "Belgique - Francais";
                            break;
                        
                        case "fr_fr":
                            localeName = "France - Francais";
                            break;

                        case "de_de":
                            localeName = "Deutschland - Deutsch";
                            break;

                        case "gb_en":
                            localeName = "United Kingdom - English";
                            break;

                        case "at_de":
                            localeName = "Österreich - Deutsch";
                            break;

                        case "ch_de":
                            localeName = "Schweiz - Deutsch";
                            break;

                        case "ch_fr":
                            localeName = "Suisse - Francais";
                            break;

                        case "es_es":
                            localeName = "España - Español";
                            break;

                        case "it_it":
                            localeName = "Italia - Italiano";
                            break;

                        case "pt_pt":
                            localeName = "Portugal - Portugues";
                            break;
                    }
                    campaignLocales.LocaleName = localeName;

                    jsonLocaleFormDto.Add(campaignLocales);
                }
            }
            return jsonLocaleFormDto;
        }

        public async Task<List<GetCampaignLocalesDto>> GetCampaignOverview(string currentLocale)
        {
            var jsonLocaleFormDto = new List<GetCampaignLocalesDto>();
            var formLocaleRepository = await _lookup_formLocaleRepository.GetAll().Where(fl => fl.Description == currentLocale).ToListAsync();

            foreach (var activeCampaignForm in formLocaleRepository)
            {
                var activeCampaignForms = await _lookup_campaignFormRepository.GetAll().Where(fl => fl.IsActive == true && fl.FormId == activeCampaignForm.FormId).FirstOrDefaultAsync();
                if (activeCampaignForms == null) { continue; }
                var campaignTable = await _campaignRepository.GetAsync(activeCampaignForms.CampaignId);
                var formTable = await _lookup_formRepository.GetAll().Where(fl => fl.Id == activeCampaignForm.FormId).FirstOrDefaultAsync();

                var localeRecord = await _lookup_localeRepository.GetAll().Where(l => l.Description.ToLower().Trim() == currentLocale.ToLower().Trim()).FirstOrDefaultAsync();
                var countryRecord = await _lookup_countryRepository.GetAll().Where(fl => fl.Id == localeRecord.CountryId).FirstOrDefaultAsync();

                var campaignTranslationsForLocale = await _lookup_campaignTranslationRepository.GetAll().Where(t => t.CampaignId == activeCampaignForms.CampaignId && t.LocaleId == localeRecord.Id).ToListAsync();
                var campaignLocales = new GetCampaignLocalesDto();

                var campaignName = campaignTable.Name;
                var campaignDescription = campaignTable.Description;

                if (campaignTranslationsForLocale.Any(t => t.Name.ToLower().Trim() == "campaignname"))
                {
                    campaignName = campaignTranslationsForLocale.Where(t => t.Name.ToLower().Trim() == "campaignname").First().Description;
                }

                if (campaignTranslationsForLocale.Any(t => t.Name.ToLower().Trim() == "campaigndescription"))
                {
                    campaignDescription = campaignTranslationsForLocale.Where(t => t.Name.ToLower().Trim() == "campaigndescription").First().Description;
                }

                var myTenantId = AbpSession.TenantId ?? 0;

                campaignLocales.CampaignName = campaignName;
                campaignLocales.CampaignDescription = campaignDescription;
                campaignLocales.CampaignId = activeCampaignForms.CampaignId;
                campaignLocales.CampaignCode = (long)campaignTable.CampaignCode;
                campaignLocales.Version = formTable.Version;
                campaignLocales.LocaleDescription = localeRecord.Description;
                campaignLocales.LocaleId = localeRecord.Id;
                campaignLocales.StartDate = campaignTable.StartDate;
                campaignLocales.EndDate = campaignTable.EndDate;
                campaignLocales.CampaignThumbnail = campaignTable.ThumbnailImagePath;

                jsonLocaleFormDto.Add(campaignLocales);
                
            }
            var jsonLocaleFormDtoSorted = jsonLocaleFormDto.OrderBy(d => d.StartDate).ToList();
            return jsonLocaleFormDtoSorted;
        }

        public async Task<GetCampaignForFormDto> GetCampaignForForm(string currentLocale, long currentCampaignCode)
        {
            var jsonCampaignData = new GetCampaignForFormDto();

            var activeampaignForms = _lookup_campaignFormRepository.GetAll().Where(fl => fl.IsActive == true);
            var listActiveForms = await activeampaignForms.ToListAsync();
            var campaignTable = await _campaignRepository.GetAll().Where(fl => fl.CampaignCode == currentCampaignCode).FirstOrDefaultAsync();
            if (campaignTable == null) return null;
            var campaignFormTable = await _lookup_campaignFormRepository.GetAll().Where(fl => fl.CampaignId == campaignTable.Id).FirstOrDefaultAsync();
            var formTable = await _lookup_formRepository.GetAll().Where(fl => fl.Id == campaignFormTable.FormId).FirstOrDefaultAsync();
            var formLocaleTable = await _formLocaleRepository.GetAll().Where(fl => fl.FormId == campaignFormTable.FormId & fl.Description == currentLocale).FirstOrDefaultAsync();
            var localeTable = await _lookup_localeRepository.GetAll().Where(fl => fl.Id == formLocaleTable.LocaleId).FirstOrDefaultAsync();
            var isActive = campaignFormTable.IsActive;
            if (isActive == false) return null;

            jsonCampaignData.CampaignId = campaignTable.Id;
            jsonCampaignData.CampaignCode = currentCampaignCode;
            jsonCampaignData.FormId = campaignFormTable.FormId;
            jsonCampaignData.Version = formTable.Version;
            jsonCampaignData.LocaleId = formLocaleTable.LocaleId;
            jsonCampaignData.LocaleDescription = localeTable.Description;
            jsonCampaignData.FormLocaleId = formLocaleTable.Id;
            jsonCampaignData.CampaignEndDate = campaignTable.EndDate;
            jsonCampaignData.BannerImagePath = campaignTable.BannerImagePath;
            return jsonCampaignData;
        }

        public async Task<GetCampaignForViewDto> GetCampaignForView(long id)
        {
            var campaign = await _campaignRepository.GetAsync(id);

            var output = new GetCampaignForViewDto { Campaign = ObjectMapper.Map<CampaignDto>(campaign) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Campaigns_Edit)]
        public async Task<GetCampaignForEditOutput> GetCampaignForEdit(EntityDto<long> input)
        {
            var campaign = await _campaignRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCampaignForEditOutput { Campaign = ObjectMapper.Map<CreateOrEditCampaignDto>(campaign) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCampaignDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Campaigns_Create)]
        protected virtual async Task Create(CreateOrEditCampaignDto input)
        {
            var campaign = ObjectMapper.Map<Campaign>(input);


            if (AbpSession.TenantId != null)
            {
                campaign.TenantId = (int?)AbpSession.TenantId;
            }


            await _campaignRepository.InsertAsync(campaign);
        }

        [AbpAuthorize(AppPermissions.Pages_Campaigns_Edit)]
        protected virtual async Task Update(CreateOrEditCampaignDto input)
        {
            var campaign = await _campaignRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, campaign);
        }

        [AbpAuthorize(AppPermissions.Pages_Campaigns_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _campaignRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCampaignsToExcel(GetAllCampaignsForExcelInput input)
        {

            var filteredCampaigns = _campaignRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(input.MinCampaignCodeFilter != null, e => e.CampaignCode >= input.MinCampaignCodeFilter)
                        .WhereIf(input.MaxCampaignCodeFilter != null, e => e.CampaignCode <= input.MaxCampaignCodeFilter);

            var query = (from o in filteredCampaigns
                         select new GetCampaignForViewDto()
                         {
                             Campaign = new CampaignDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 StartDate = o.StartDate,
                                 EndDate = o.EndDate,
                                 CampaignCode = o.CampaignCode,
                                 Id = o.Id
                             }
                         });


            var campaignListDtos = await query.ToListAsync();

            return _campaignsExcelExporter.ExportToFile(campaignListDtos);
        }

        public async Task<PagedResultDto<CampaignFormCompanyLookupTableDto>> GetAllCampaignFormFromCompanyForLookupTable(RMS.SBJ.CampaignProcesses.Dtos.GetAllForLookupTableInput input)
        {
            var campaignFormListFromCompanyFormLocale = new List<FormLocale>();
            var formsQuery = _lookup_formRepository.GetAll();
            var formLocalesQuery = _formLocaleRepository.GetAll();

            var companyFormLocaleQuery = formLocalesQuery.Where(item => item.FormId == FormConsts.CompanySLId);
            var formList = await formsQuery
                .ToListAsync();

            var formLocaleList = await formLocalesQuery
                .PageBy(input)
                .ToListAsync();

            #region company
            var latestCompanyFormVersion = formList.Where(item => item.SystemLevelId == FormConsts.CompanySLId).LastOrDefault();
            var companyFormsList = formLocaleList.Where(item => item.FormId == latestCompanyFormVersion.Id).ToList();
            var companyFormLocaleLocaleIds = companyFormsList.Select(item => item.LocaleId);
            #endregion

            #region campaign
            var latestCampaignFormVersion = formList.Where(item => item.SystemLevelId == FormConsts.CampaignSLId).LastOrDefault();
            var campaignFormsList = formLocaleList.Where(item => item.FormId == latestCampaignFormVersion.Id).ToList();
            var campaignFormLocaleLocaleIds = campaignFormsList.Select(item => item.LocaleId);
            #endregion

            var otherCompanyFormLocaleLocaleIds = companyFormLocaleLocaleIds.Except(campaignFormLocaleLocaleIds);

            foreach (var formLocaleId in otherCompanyFormLocaleLocaleIds)
            {
                var campaignFormFromCompanyFormLocale = companyFormsList.Where(item => item.LocaleId == formLocaleId).FirstOrDefault();
                campaignFormListFromCompanyFormLocale.Add(campaignFormFromCompanyFormLocale);
            }

            var lookupTableDtoList = new List<CampaignFormCompanyLookupTableDto>();

            foreach (var form in campaignFormListFromCompanyFormLocale)
            {
                lookupTableDtoList.Add(new CampaignFormCompanyLookupTableDto
                {
                    Id = form.Id,
                    DisplayName = form.Description,
                    FormId = latestCampaignFormVersion.Id,
                    LocaleId = form.LocaleId
                });
            }
            return new PagedResultDto<CampaignFormCompanyLookupTableDto>(
                lookupTableDtoList.Count,
                lookupTableDtoList
            );
        }

        public async Task<long> CreateOrEditAndGetId(CreateOrEditCampaignDto input)
        {
            var campaign = ObjectMapper.Map<Campaign>(input);
            if (input.Id == null)
            {
                var campaignId = await _campaignRepository.InsertAndGetIdAsync(campaign);
                return campaignId;
            }
            else
            {
                var campaignId = await _campaignRepository.InsertOrUpdateAndGetIdAsync(campaign);
                return campaignId;
            }
        }

        public async Task<long> CreateOrEditCustomized(CreateOrEditCampaignDto input, string selectedCampaignTypeIds, string selectedLocaleIds, long? sourceCampaignId)
        {
            //how bizarre: changing parameter "string selectedCampaignTypeIds" into a List or Array completely screws up the RMS...
            //although campaigns with multiple campaignTypes are not yet in practice, it may still be so in the future, so this process already takes the possibility into account
            //if sourceCampaignId is empty, then we copy from Company level, otherwise we copy from Campaign level
            //selectedCampaignTypeIds and selectedLocaleIds are only relevant when copying from Company level

            var campaignMap = ObjectMapper.Map<Campaign>(input);

            campaignMap.TenantId = AbpSession.TenantId;
            campaignMap.CreatorUserId = AbpSession.UserId ?? 1;
            campaignMap.CreationTime = DateTime.Now;
            campaignMap.StartDate = campaignMap.StartDate.Date;
            campaignMap.EndDate = campaignMap.EndDate.Date.AddHours(23).AddMinutes(59);

            long campaignId;

            if (!input.Id.HasValue)
            {
                //gather all necessary source information...
                List<long> sourceCampaignTypeIds = null;
                List<long> sourceCampaignTypeEventRegistrationStatusIds = null;
                List<FormLocale> sourceFormLocales = null;
                List<MessageType> sourceMessageTypes = null;

                if (!sourceCampaignId.HasValue)
                {
                    sourceCampaignTypeIds = !String.IsNullOrWhiteSpace(selectedCampaignTypeIds) ? JsonConvert.DeserializeObject<List<long>>(selectedCampaignTypeIds) : new List<long>();
                    var sourceLocaleIds = !String.IsNullOrWhiteSpace(selectedLocaleIds) ? JsonConvert.DeserializeObject<List<long>>(selectedLocaleIds) : new List<long>();

                    sourceFormLocales = (from o in _formLocaleRepository.GetAll()
                                         join o1 in _lookup_formRepository.GetAll() on o.FormId equals o1.Id into j1
                                         from s1 in j1
                                         where s1.SystemLevelId == 1
                                            && sourceLocaleIds.Contains(o.LocaleId)
                                         select o).ToList();

                    sourceMessageTypes = (from o in _lookup_messageTypeRepository.GetAll()
                                          join o1 in _lookup_messageRepository.GetAll() on o.MessageId equals o1.Id into j1
                                          from s1 in j1
                                          where s1.SystemLevelId == 1
                                          select o).ToList();
                }
                else
                {
                    //crap protection: retrieve the source campaignTypeId(s) via HandlingLine instead of CampaignCampaignType
                    sourceCampaignTypeIds = (from o in _lookup_handlingLineRepository.GetAll()
                                             join o1 in _productHandlingRepository.GetAll() on o.ProductHandlingId equals o1.Id into j1
                                             from s1 in j1
                                             where s1.CampaignId == sourceCampaignId.Value
                                                 && o.CustomerCode.ToUpper().Trim() != "UNKNOWN"
                                             select o.CampaignTypeId).Distinct().ToList();

                    sourceFormLocales = (from o in _formLocaleRepository.GetAll()
                                         join o1 in _lookup_campaignFormRepository.GetAll() on o.FormId equals o1.FormId into j1
                                         from s1 in j1
                                         where s1.CampaignId == sourceCampaignId.Value
                                         select o).ToList();

                    sourceMessageTypes = (from o in _lookup_messageTypeRepository.GetAll()
                                          join o1 in _lookup_campaignMessageRepository.GetAll() on o.MessageId equals o1.MessageId into j1
                                          from s1 in j1
                                          where s1.CampaignId == sourceCampaignId.Value
                                          select o).ToList();
                }

                sourceCampaignTypeEventRegistrationStatusIds = (from o in _campaignTypeEventRegistrationStatusRepository.GetAll()
                                                                join o1 in _campaignTypeEventRepository.GetAll() on o.CampaignTypeEventId equals o1.Id into j1
                                                                from s1 in j1
                                                                where sourceCampaignTypeIds.Contains(s1.CampaignTypeId)
                                                                select o.Id).ToList();

                campaignId = await _campaignRepository.InsertAndGetIdAsync(campaignMap);

                foreach (var campaignTypeId in sourceCampaignTypeIds)
                {
                    var campaignType = await _campaignTypeRepository.GetAsync(campaignTypeId);

                    await _campaignCampaignTypeRepository.InsertAsync(new CampaignCampaignType()
                    {
                        CampaignId = campaignId,
                        CampaignTypeId = campaignTypeId,
                        TenantId = AbpSession.TenantId,
                        CreatorUserId = AbpSession.UserId ?? 1,
                        CreationTime = DateTime.Now,
                        Description = campaignType.Name
                    });
                }

                //forms - general...
                var formId = await _lookup_formRepository.InsertAndGetIdAsync(new Form
                {
                    TenantId = AbpSession.TenantId,
                    CreatorUserId = AbpSession.UserId ?? 1,
                    CreationTime = DateTime.Now,
                    SystemLevelId = 2,
                    Version = "1.0"
                });

                var campaignFormId = await _lookup_campaignFormRepository.InsertAndGetIdAsync(new CampaignForm
                {
                    TenantId = AbpSession.TenantId,
                    CreatorUserId = AbpSession.UserId ?? 1,
                    CreationTime = DateTime.Now,
                    CampaignId = campaignId,
                    FormId = formId,
                    IsActive = true
                });

                //messaging - general...
                var sourceAndDestinationMessageComponentContentIds = new Dictionary<long, long>();

                var messageId = await _lookup_messageRepository.InsertAndGetIdAsync(new Message
                {
                    TenantId = AbpSession.TenantId,
                    CreatorUserId = AbpSession.UserId ?? 1,
                    CreationTime = DateTime.Now,
                    SystemLevelId = 2,
                    Version = "1.0"
                });

                var campaignMessageId = await _lookup_campaignMessageRepository.InsertAndGetIdAsync(new CampaignMessage
                {
                    TenantId = AbpSession.TenantId,
                    CreatorUserId = AbpSession.UserId ?? 1,
                    CreationTime = DateTime.Now,
                    CampaignId = campaignId,
                    MessageId = messageId,
                    IsActive = true
                });

                foreach (var sourceMessageType in sourceMessageTypes)
                {
                    var campaignMessageTypeId = await _lookup_messageTypeRepository.InsertAndGetIdAsync(new MessageType
                    {
                        TenantId = AbpSession.TenantId,
                        CreatorUserId = AbpSession.UserId ?? 1,
                        CreationTime = DateTime.Now,
                        MessageId = messageId,
                        Name = sourceMessageType.Name,
                        Source = sourceMessageType.Source,
                        IsActive = true
                    });

                    var sourceMessageComponents = await _lookup_messageComponentRepository.GetAll().Where(c => c.MessageTypeId == sourceMessageType.Id).ToListAsync();

                    foreach (var sourceMessageComponent in sourceMessageComponents)
                    {
                        var campaignMessageComponentId = await _lookup_messageComponentRepository.InsertAndGetIdAsync(new MessageComponent
                        {
                            TenantId = AbpSession.TenantId,
                            CreatorUserId = AbpSession.UserId ?? 1,
                            CreationTime = DateTime.Now,
                            MessageTypeId = campaignMessageTypeId,
                            MessageComponentTypeId = sourceMessageComponent.MessageComponentTypeId,
                            IsActive = true
                        });

                        var sourceMessageComponentContents = await _lookup_messageComponentContentRepository.GetAll().Where(c => c.MessageComponentId == sourceMessageComponent.Id && sourceCampaignTypeEventRegistrationStatusIds.Contains(c.CampaignTypeEventRegistrationStatusId)).ToListAsync();

                        foreach (var sourceMessageComponentContent in sourceMessageComponentContents)
                        {
                            var campaignMessageComponentContentId = await _lookup_messageComponentContentRepository.InsertAndGetIdAsync(new MessageComponentContent
                            {
                                TenantId = AbpSession.TenantId,
                                CreatorUserId = AbpSession.UserId ?? 1,
                                CreationTime = DateTime.Now,
                                MessageComponentId = campaignMessageComponentId,
                                CampaignTypeEventRegistrationStatusId = sourceMessageComponentContent.CampaignTypeEventRegistrationStatusId,
                                Content = sourceMessageComponentContent.Content
                            });

                            //Key = source MessageComponentContentId & Value = destination MessageComponentContentId
                            sourceAndDestinationMessageComponentContentIds.Add(sourceMessageComponentContent.Id, campaignMessageComponentContentId);
                        }
                    }
                }

                foreach (var sourceFormLocale in sourceFormLocales)
                {
                    //forms - localized...
                    var sourceFormLocaleInfo = await _lookup_localeRepository.GetAsync(sourceFormLocale.LocaleId);
                    var sourceFormBlocks = await _formBlockRepository.GetAll().Where(b => b.FormLocaleId == sourceFormLocale.Id).ToListAsync();

                    var campaignFormLocaleId = await _formLocaleRepository.InsertAndGetIdAsync(new FormLocale
                    {
                        TenantId = AbpSession.TenantId,
                        CreatorUserId = AbpSession.UserId ?? 1,
                        CreationTime = DateTime.Now,
                        FormId = formId,
                        LocaleId = sourceFormLocale.LocaleId,
                        Description = sourceFormLocaleInfo.Description
                    });

                    foreach (var sourceFormBlock in sourceFormBlocks)
                    {
                        var campaignFormBlockId = await _formBlockRepository.InsertAndGetIdAsync(new FormBlock
                        {
                            TenantId = AbpSession.TenantId,
                            CreatorUserId = AbpSession.UserId ?? 1,
                            CreationTime = DateTime.Now,
                            FormLocaleId = campaignFormLocaleId,
                            Description = sourceFormBlock.Description,
                            SortOrder = sourceFormBlock.SortOrder
                        });

                        var sourceFormBlockFields = await _formBlockFieldRepository.GetAll().Where(f => f.FormBlockId == sourceFormBlock.Id).ToListAsync();

                        foreach (var sourceFormBlockField in sourceFormBlockFields)
                        {
                            var campaignFormBlockFieldId = await _formBlockFieldRepository.InsertAndGetIdAsync(new FormBlockField
                            {
                                TenantId = AbpSession.TenantId,
                                CreatorUserId = AbpSession.UserId ?? 1,
                                CreationTime = DateTime.Now,
                                FormBlockId = campaignFormBlockId,
                                FormFieldId = sourceFormBlockField.FormFieldId,
                                SortOrder = sourceFormBlockField.SortOrder
                            });
                        }
                    }

                    //messaging - localized...
                    foreach (var sourceAndDestinationComponentContentId in sourceAndDestinationMessageComponentContentIds)
                    {
                        //Key = source MessageComponentContentId & Value = destination MessageComponentContentId
                        var sourceMessageComponentContentTranslation = await _lookup_messageContentTranslationRepository.GetAll().Where(t => t.MessageComponentContentId == sourceAndDestinationComponentContentId.Key && t.LocaleId == sourceFormLocale.LocaleId).FirstAsync();

                        var campaignMessageComponentContentTranslationId = await _lookup_messageContentTranslationRepository.InsertAndGetIdAsync(new MessageContentTranslation
                        {
                            TenantId = AbpSession.TenantId,
                            CreatorUserId = AbpSession.UserId ?? 1,
                            CreationTime = DateTime.Now,
                            MessageComponentContentId = sourceAndDestinationComponentContentId.Value,
                            LocaleId = sourceFormLocale.LocaleId,
                            Content = sourceMessageComponentContentTranslation.Content
                        });
                    }
                }
            }
            else
            {
                var campaign = await _campaignRepository.UpdateAsync(campaignMap);

                campaignId = campaign.Id;
            }

            return campaignId;
        }

        public async Task<bool> CheckCompanySetup()
        {
            // check if company setup is complete enough to initiate a new campaign...
            var companyForm = await _lookup_formRepository.GetAll().Where(f => f.SystemLevelId == 1).FirstOrDefaultAsync();

            if (companyForm == null) { return false; }

            //FormLocale companyFormLocale = null;

            //companyFormLocale = await _formLocaleRepository.GetAll().Where(f => f.FormId == companyForm.Id && f.Description.ToLower().Trim() == "nl_nl").FirstOrDefaultAsync();

            //if (companyFormLocale == null)
            //{
            //    companyFormLocale = await _formLocaleRepository.GetAll().Where(f => f.FormId == companyForm.Id && f.Description.ToLower().Trim() == "be_nl").FirstOrDefaultAsync();

            //    if (companyFormLocale == null)
            //    {
            //        companyFormLocale = await _formLocaleRepository.GetAll().Where(f => f.FormId == companyForm.Id && f.Description.ToLower().Trim().EndsWith("_en")).OrderBy(f => f.LocaleId).FirstOrDefaultAsync();

            //        if (companyFormLocale == null)
            //        {
            //            companyFormLocale = await _formLocaleRepository.GetAll().Where(f => f.FormId == companyForm.Id).OrderBy(f => f.LocaleId).FirstOrDefaultAsync();
            //        }
            //    }
            //}

            var companyFormLocales = await _formLocaleRepository.GetAll().Where(f => f.FormId == companyForm.Id).ToListAsync();

            bool verdict = true;

            foreach (var companyFormLocale in companyFormLocales)
            {
                var companyFormBlocks = await _formBlockRepository.GetAll().Where(b => b.FormLocaleId == companyFormLocale.Id).ToListAsync();
                var companyFormBlockIds = companyFormBlocks.Select(b => b.Id).ToList();

                var companyFormBlockFields = await _formBlockFieldRepository.GetAll().Where(f => companyFormBlockIds.Contains(f.FormBlockId.Value)).ToListAsync();
                var companyFormBlockFieldBlockIds = companyFormBlockFields.Select(f => f.FormBlockId.Value).Distinct().ToList();

                //each companyFormBlock (there must be at least 1) must contain at least 1 formfield
                if (companyFormBlockIds.Count == 0 ||
                    companyFormBlockFieldBlockIds.Count == 0 ||
                    !Enumerable.SequenceEqual(companyFormBlockIds.OrderBy(s => s), companyFormBlockFieldBlockIds.OrderBy(s => s)))
                { verdict = false; break; }

                //check messaging setup...
                var companyMessage = await _lookup_messageRepository.GetAll().Where(m => m.SystemLevelId == 1).FirstOrDefaultAsync();

                if (companyMessage == null) { verdict = false; break; }

                var companyMessageTypes = await _lookup_messageTypeRepository.GetAll().Where(t => t.MessageId == companyMessage.Id).ToListAsync();
                var companyMessageTypeIds = companyMessageTypes.Select(t => t.Id).ToList();

                var companyMessageComponents = await _lookup_messageComponentRepository.GetAll().Where(c => companyMessageTypeIds.Contains(c.MessageTypeId)).ToListAsync();
                var companyMessageComponentTypeIds = companyMessageComponents.Select(c => c.MessageTypeId).Distinct().ToList();

                //each companyMessageType (there must be at least 1) must contain at least 1 component
                if (companyMessageTypeIds.Count == 0 ||
                    companyMessageComponentTypeIds.Count == 0 ||
                    !Enumerable.SequenceEqual(companyMessageTypeIds.OrderBy(s => s), companyMessageComponentTypeIds.OrderBy(s => s)))
                { verdict = false; break; }

                var companyMessageComponentIds = companyMessageComponents.Select(c => c.Id).ToList();
                var companyMessageComponentContents = await _lookup_messageComponentContentRepository.GetAll().Where(c => companyMessageComponentIds.Contains(c.MessageComponentId)).ToListAsync();
                var companyMessageComponentContentComponentIds = companyMessageComponentContents.Select(c => c.MessageComponentId).Distinct().ToList();

                //each companyMessageComponent (there must be at least 1) must contain at least 1 content record
                if (companyMessageComponentIds.Count == 0 ||
                    companyMessageComponentContentComponentIds.Count == 0 ||
                    !Enumerable.SequenceEqual(companyMessageComponentIds.OrderBy(s => s), companyMessageComponentContentComponentIds.OrderBy(s => s)))
                { verdict = false; break; }

                var companyMessageComponentContentIds = companyMessageComponentContents.Select(c => c.Id).ToList();
                var companyMessageComponentContentTranslations = await _lookup_messageContentTranslationRepository.GetAll().Where(t => companyMessageComponentContentIds.Contains(t.MessageComponentContentId) && t.LocaleId == companyFormLocale.LocaleId).ToListAsync();
                var companyMessageComponentContentTranslationContentIds = companyMessageComponentContentTranslations.Select(t => t.MessageComponentContentId).Distinct().ToList();

                //each companyMessageComponentContent (there must be at least 1) must contain at least 1 translation record
                if (companyMessageComponentContentIds.Count == 0 ||
                    companyMessageComponentContentTranslationContentIds.Count == 0 ||
                    !Enumerable.SequenceEqual(companyMessageComponentContentIds.OrderBy(s => s), companyMessageComponentContentTranslationContentIds.OrderBy(s => s)))
                { verdict = false; break; }
            }

            return verdict;
        }

        public async Task<long> GetLatestFormIdForCampaign()
        {
            var allForms = await _lookup_formRepository.GetAllListAsync();
            var getLatestFormIdForCampaign = allForms.Where(item => item.SystemLevelId == FormConsts.CampaignSLId).LastOrDefault().Id;
            return getLatestFormIdForCampaign;
        }

        public async Task<long> GetLatestMessageIdForCampaign()
        {
            var allMessages = await _lookup_messageRepository.GetAllListAsync();
            var getLatestMessageIdForCampaign = allMessages.Where(item => item.SystemLevelId == MessageConsts.CampaignSLId).LastOrDefault().Id;
            return getLatestMessageIdForCampaign;
        }

        public async Task<List<GetCampaignForViewDto>> GetAllCampaignsForDropdown(bool? activeCampaignsOnly)
        {
            var filteredCampaigns = _campaignRepository.GetAll()
                                    .Where(e => e.CampaignCode != null)
                                    .WhereIf(activeCampaignsOnly.Equals(true), e => e.EndDate >= System.DateTime.Today)
                                    .OrderBy("id asc");

            var campaigns = from o in filteredCampaigns
                            select new GetCampaignForViewDto()
                            {
                                Campaign = new CampaignDto
                                {
                                    Name = o.Name,
                                    Description = o.Description,
                                    StartDate = o.StartDate,
                                    EndDate = o.EndDate,
                                    CampaignCode = o.CampaignCode,
                                    ExternalCode = o.ExternalCode,
                                    Id = o.Id
                                }
                            };

            return await campaigns.ToListAsync();
        }
    }
}