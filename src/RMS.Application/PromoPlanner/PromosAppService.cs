using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.Products;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.PromoPlanner.Exporting;
using RMS.PromoPlanner.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using RMS.SBJ.Retailers;

namespace RMS.PromoPlanner
{
    [AbpAuthorize(AppPermissions.Pages_Promos)]
    public class PromosAppService : RMSAppServiceBase, IPromosAppService
    {
        private readonly IRepository<Promo, long> _promoRepository;
        private readonly IPromosExcelExporter _promosExcelExporter;
        private readonly IRepository<PromoScope, long> _lookup_promoScopeRepository;
        private readonly IRepository<CampaignType, long> _lookup_campaignTypeRepository;
        private readonly IRepository<ProductCategory, long> _lookup_productCategoryRepository;
        private readonly IRepository<PromoProduct, long> _lookup_promoProductRepository;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IRepository<PromoRetailer, long> _lookup_promoRetailerRepository;
        private readonly IRepository<Retailer, long> _lookup_retailerRepository;
        private readonly IRepository<PromoCountry, long> _lookup_promoCountryRepository;
        private readonly IRepository<Country, long> _lookup_countryRepository;
        private readonly IRepository<PromoStepData, int> _lookup_promoStepDataRepository;
        private readonly IRepository<PromoStep, int> _lookup_promoStepRepository;
        private readonly IRepository<PromoStepFieldData, int> _lookup_promoStepFieldDataRepository;
        private readonly IRepository<PromoStepField, int> _lookup_promoStepFieldRepository;

        public PromosAppService(IRepository<Promo, long> promoRepository, IPromosExcelExporter promosExcelExporter, IRepository<PromoScope, long> lookup_promoScopeRepository, IRepository<CampaignType, long> lookup_campaignTypeRepository, IRepository<ProductCategory, long> lookup_productCategoryRepository, IRepository<PromoProduct, long> lookup_promoProductRepository, IRepository<Product, long> lookup_productRepository, IRepository<PromoRetailer, long> lookup_promoRetailerRepository, IRepository<Retailer, long> lookup_retailerRepository, IRepository<PromoCountry, long> lookup_promoCountryRepository, IRepository<Country, long> lookup_countryRepository, IRepository<PromoStepData, int> lookup_promoStepDataRepository, IRepository<PromoStep, int> lookup_promoStepRepository, IRepository<PromoStepFieldData, int> lookup_promoStepFieldDataRepository, IRepository<PromoStepField, int> lookup_promoStepFieldRepository)
        {
            _promoRepository = promoRepository;
            _promosExcelExporter = promosExcelExporter;
            _lookup_promoScopeRepository = lookup_promoScopeRepository;
            _lookup_campaignTypeRepository = lookup_campaignTypeRepository;
            _lookup_productCategoryRepository = lookup_productCategoryRepository;
            _lookup_promoProductRepository = lookup_promoProductRepository;
            _lookup_productRepository = lookup_productRepository;
            _lookup_promoRetailerRepository = lookup_promoRetailerRepository;
            _lookup_retailerRepository = lookup_retailerRepository;
            _lookup_promoCountryRepository = lookup_promoCountryRepository;
            _lookup_countryRepository = lookup_countryRepository;
            _lookup_promoStepDataRepository = lookup_promoStepDataRepository;
            _lookup_promoStepRepository = lookup_promoStepRepository;
            _lookup_promoStepFieldDataRepository = lookup_promoStepFieldDataRepository;
            _lookup_promoStepFieldRepository = lookup_promoStepFieldRepository;
        }

        private class PromoStepDetails
        {
            public int StepId { get; set; }
            public short Sequence { get; set; }
            public DateTime? ConfirmationDate { get; set; }
        }

        public async Task<PagedResultDto<GetPromoForViewDto>> GetAll(GetAllPromosInput input)
        {

            var filteredPromos = _promoRepository.GetAll()
                        .Include(e => e.PromoScopeFk)
                        .Include(e => e.CampaignTypeFk)
                        .Include(e => e.ProductCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Promocode.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PromocodeFilter), e => e.Promocode == input.PromocodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(input.MinPromoStartFilter != null, e => e.PromoStart >= input.MinPromoStartFilter)
                        .WhereIf(input.MaxPromoStartFilter != null, e => e.PromoStart <= input.MaxPromoStartFilter)
                        .WhereIf(input.MinPromoEndFilter != null, e => e.PromoEnd >= input.MinPromoEndFilter)
                        .WhereIf(input.MaxPromoEndFilter != null, e => e.PromoEnd <= input.MaxPromoEndFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PromoScopeFilter), e => e.PromoScopeFk != null && e.PromoScopeFk.Id.ToString() == input.PromoScopeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CampaignTypeFilter), e => e.CampaignTypeFk != null && e.CampaignTypeFk.Id.ToString() == input.CampaignTypeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Id.ToString() == input.ProductCategoryFilter);

            // Retailer selection
            if (!string.IsNullOrWhiteSpace(input.RetailerFilter))
            {
                var retailerList = input.RetailerFilter.Split(',').Select(long.Parse).ToList();
                var selectedPromoRetailers = _lookup_promoRetailerRepository.GetAll().Where(r => retailerList.Contains(r.RetailerId));
                var retailerSelectedPromos = from f in filteredPromos
                                             join r in selectedPromoRetailers on f.Id equals r.PromoId
                                             select f;
                filteredPromos = retailerSelectedPromos;
            }

            var pagedAndFilteredPromos = filteredPromos
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var promos = from o in pagedAndFilteredPromos
                         join o1 in _lookup_promoScopeRepository.GetAll() on o.PromoScopeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_campaignTypeRepository.GetAll() on o.CampaignTypeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetPromoForViewDto()
                         {
                             Promo = new PromoDto
                             {
                                 Promocode = o.Promocode,
                                 Description = o.Description,
                                 PromoStart = o.PromoStart,
                                 PromoEnd = o.PromoEnd,
                                 CloseDate = o.CloseDate,
                                 Comments = o.Comments,
                                 Id = o.Id
                             },
                             PromoScopeDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString(),
                             CampaignTypeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             ProductCategoryDescription = s3 == null || s3.Description == null ? "" : s3.Description.ToString(),
                             Status = DateTime.Today < o.PromoStart ? "Pending" : DateTime.Today > o.PromoEnd ? "Finished" : "Running"
                         };

            var promoList = promos.ToList();

            foreach (var promo in promoList)
            {
                var promoStepsData = _lookup_promoStepDataRepository.GetAll().Where(e => e.PromoId == promo.Promo.Id);
                var promoStepsDataWithDetails = from o in promoStepsData
                                                join o1 in _lookup_promoStepRepository.GetAll() on o.PromoStepId equals o1.Id into j1
                                                from s1 in j1.DefaultIfEmpty()

                                                select new PromoStepDetails()
                                                {
                                                    StepId = (int)o.PromoStepId,
                                                    Sequence = s1.Sequence,
                                                    ConfirmationDate = o.ConfirmationDate
                                                };

                var promoProgress = new List<StepStatus>();

                var promoSteps = _lookup_promoStepRepository.GetAll().OrderBy(p => p.Sequence);
                foreach (var promoStep in promoSteps)
                {
                    var stepStatus = new StepStatus();
                    var existingStepData = promoStepsDataWithDetails.FirstOrDefault(d => d.Sequence == promoStep.Sequence);
                    stepStatus.Confirmed = (existingStepData != null && existingStepData.ConfirmationDate.HasValue);
                    stepStatus.Description = promoStep.Description;
                    promoProgress.Add(stepStatus);
                }
                promo.PromoProgress = promoProgress;
            }

            var totalCount = await filteredPromos.CountAsync();

            return new PagedResultDto<GetPromoForViewDto>(
                totalCount,
                promoList
            );
        }

        public async Task<GetPromoForViewDto> GetPromoForView(long id)
        {
            var promo = await _promoRepository.GetAsync(id);

            var output = new GetPromoForViewDto { Promo = ObjectMapper.Map<PromoDto>(promo) };

            //if (output.Promo.PromoScopeId != null)
            //{
            var _lookupPromoScope = await _lookup_promoScopeRepository.FirstOrDefaultAsync((long)output.Promo.PromoScopeId);
            output.PromoScopeDescription = _lookupPromoScope?.Description?.ToString();
            //}

            //if (output.Promo.CampaignTypeId != null)
            //{
            var _lookupCampaignType = await _lookup_campaignTypeRepository.FirstOrDefaultAsync((long)output.Promo.CampaignTypeId);
            output.CampaignTypeName = _lookupCampaignType?.Name?.ToString();
            //}

            //if (output.Promo.ProductCategoryId != null)
            //{
            var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync((long)output.Promo.ProductCategoryId);
            output.ProductCategoryDescription = _lookupProductCategory?.Description?.ToString();
            //}

            //products...
            var promoProducts = _lookup_promoProductRepository.GetAll().Where(e => e.PromoId == id);
            var promoProductsWithDetails = from o in promoProducts
                                           join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                                           from s1 in j1.DefaultIfEmpty()

                                           select new CustomProductForView()
                                           {
                                               ProductId = s1.Id,
                                               CtnCode = s1.ProductCode,
                                               EanCode = s1.Ean,
                                               Description = s1.Description
                                           };

            var promoProductsWithDetailsList = promoProductsWithDetails.OrderBy(s => s.Description).ToList();

            //retailers...
            var promoRetailers = _lookup_promoRetailerRepository.GetAll().Where(e => e.PromoId == id);
            var promoRetailersWithDetails = from o in promoRetailers
                                            join o1 in _lookup_retailerRepository.GetAll() on o.RetailerId equals o1.Id into j1
                                            from s1 in j1.DefaultIfEmpty()

                                            join o2 in _lookup_countryRepository.GetAll() on s1.CountryId equals o2.Id into j2
                                            from s2 in j2.DefaultIfEmpty()

                                            select new CustomPromoRetailerForView()
                                            {
                                                RetailerId = s1.Id,
                                                RetailerCode = s1.Code,
                                                RetailerName = s1.Name,
                                                RetailerCountry = s2.CountryCode
                                            };

            var promoRetailersWithDetailsList = promoRetailersWithDetails.OrderBy(s => s.RetailerName).ToList();

            //countries...
            var promoCountries = _lookup_promoCountryRepository.GetAll().Where(e => e.PromoId == id);
            var promoCountriesWithDetails = from o in promoCountries
                                            join o1 in _lookup_countryRepository.GetAll() on o.CountryId equals o1.Id into j1
                                            from s1 in j1.DefaultIfEmpty()

                                            select new CustomPromoCountryForView()
                                            {
                                                CountryId = s1.Id,
                                                CountryCode = s1.CountryCode,
                                                CountryName = s1.Description
                                            };

            var promoCountriesWithDetailsList = promoCountriesWithDetails.OrderBy(s => s.CountryName).ToList();

            //steps...
            var promoStepsData = _lookup_promoStepDataRepository.GetAll().Where(e => e.PromoId == id);
            var promoStepsDataWithDetails = from o in promoStepsData
                                            join o1 in _lookup_promoStepRepository.GetAll() on o.PromoStepId equals o1.Id into j1
                                            from s1 in j1.DefaultIfEmpty()

                                            select new CustomPromoStepForView()
                                            {
                                                PromoStepId = o.Id,
                                                StepId = s1.Id,
                                                Sequence = s1.Sequence,
                                                Description = s1.Description,
                                                Status = o.ConfirmationDate.HasValue ? String.Format("finished on {0}", o.ConfirmationDate.Value.ToString("dd/MM/yyyy")) : "in progress"
                                            };

            var promoStepsDataWithDetailsList = promoStepsDataWithDetails.OrderBy(s => s.Sequence).ToList();

            foreach (var promoStepData in promoStepsDataWithDetailsList)
            {
                var promoStepFieldsData = _lookup_promoStepFieldDataRepository.GetAll().Where(e => e.PromoStepDataId == promoStepData.PromoStepId);
                var promoStepFieldsDataWithDetails = from o in promoStepFieldsData
                                                     join o1 in _lookup_promoStepFieldRepository.GetAll() on o.PromoStepFieldId equals o1.Id into j1
                                                     from s1 in j1.DefaultIfEmpty()

                                                     select new CustomPromoStepFieldForView()
                                                     {
                                                         FieldId = s1.Id,
                                                         Sequence = s1.Sequence,
                                                         FieldName = s1.Description,
                                                         FieldValue = o.Value
                                                     };

                var promoStepFieldsDataWithDetailsList = promoStepFieldsDataWithDetails.OrderBy(s => s.Sequence).ToList();

                promoStepData.PromoStepFields = promoStepFieldsDataWithDetailsList;
            }

            output.PromoProducts = promoProductsWithDetailsList;
            output.PromoRetailers = promoRetailersWithDetailsList;
            output.PromoCountries = promoCountriesWithDetailsList;
            output.PromoSteps = promoStepsDataWithDetailsList;

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Promos_Edit)]
        public async Task<GetPromoForEditOutput> GetPromoForEdit(EntityDto<long> input)
        {
            var promo = await _promoRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPromoForEditOutput { Promo = ObjectMapper.Map<CreateOrEditPromoDto>(promo) };

            var _lookupPromoScope = await _lookup_promoScopeRepository.FirstOrDefaultAsync((long)output.Promo.PromoScopeId);
            output.PromoScopeDescription = _lookupPromoScope?.Description?.ToString();

            var _lookupCampaignType = await _lookup_campaignTypeRepository.FirstOrDefaultAsync((long)output.Promo.CampaignTypeId);
            output.CampaignTypeName = _lookupCampaignType?.Name?.ToString();

            var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync((long)output.Promo.ProductCategoryId);
            output.ProductCategoryDescription = _lookupProductCategory?.Description?.ToString();

            //products...
            var promoProducts = _lookup_promoProductRepository.GetAll().Where(e => e.PromoId == input.Id);
            var promoProductsWithDetails = from o in promoProducts
                                           join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                                           from s1 in j1.DefaultIfEmpty()

                                           select new CustomProductForView()
                                           {
                                               ProductId = s1.Id,
                                               CtnCode = s1.ProductCode,
                                               EanCode = s1.Ean,
                                               Description = s1.Description
                                           };

            var promoProductsWithDetailsList = promoProductsWithDetails.OrderBy(s => s.Description).ToList();

            //retailers...
            var promoRetailers = _lookup_promoRetailerRepository.GetAll().Where(e => e.PromoId == input.Id);
            var promoRetailersWithDetails = from o in promoRetailers
                                            join o1 in _lookup_retailerRepository.GetAll() on o.RetailerId equals o1.Id into j1
                                            from s1 in j1.DefaultIfEmpty()

                                            join o2 in _lookup_countryRepository.GetAll() on s1.CountryId equals o2.Id into j2
                                            from s2 in j2.DefaultIfEmpty()

                                            select new CustomPromoRetailerForView()
                                            {
                                                RetailerId = s1.Id,
                                                RetailerCode = s1.Code,
                                                RetailerName = s1.Name,
                                                RetailerCountry = s2.CountryCode
                                            };

            var promoRetailersWithDetailsList = promoRetailersWithDetails.OrderBy(s => s.RetailerName).ToList();

            //countries...
            var promoCountries = _lookup_promoCountryRepository.GetAll().Where(e => e.PromoId == input.Id);
            var promoCountriesWithDetails = from o in promoCountries
                                            join o1 in _lookup_countryRepository.GetAll() on o.CountryId equals o1.Id into j1
                                            from s1 in j1.DefaultIfEmpty()

                                            select new CustomPromoCountryForView()
                                            {
                                                CountryId = s1.Id,
                                                CountryCode = s1.CountryCode,
                                                CountryName = s1.Description
                                            };

            var promoCountriesWithDetailsList = promoCountriesWithDetails.OrderBy(s => s.CountryName).ToList();
            var promoCountryIds = String.Empty;

            if (promoCountriesWithDetailsList.Count > 0)
            {
                promoCountryIds = String.Join(",", promoCountriesWithDetailsList.Select(c => c.CountryId));
            }

            var promoSteps = _lookup_promoStepRepository.GetAll();
            var promoStepsFiltered = new List<CustomPromoStepForView>();

            foreach (var ps in promoSteps)
            {
                var record = _lookup_promoStepDataRepository.GetAll().FirstOrDefault(psd => psd.PromoId == input.Id && psd.PromoStepId == ps.Id);

                promoStepsFiltered.Add(new CustomPromoStepForView
                {
                    PromoStepId = record?.Id,
                    Description = ps.Description,
                    Sequence = ps.Sequence,
                    StepId = ps.Id,
                    Status = (record != null) ? (record.ConfirmationDate.HasValue ? $"finished on {record.ConfirmationDate.Value:dd/MM/yyyy}" : "in progress") : string.Empty
                });
            }

            var promoStepsDataWithDetailsList = promoStepsFiltered.OrderBy(s => s.Sequence).ToList();

            foreach (var promoStepData in promoStepsDataWithDetailsList)
            {
                //update: get all the possible fields that belong to the PromoStep by default, and then left join to get the values of the fields that have values at the moment...
                var promoStepFields = _lookup_promoStepFieldRepository.GetAll().Where(e => e.PromoStepId == promoStepData.StepId);
                var promoStepFieldsWithDetails = from o in promoStepFields
                                                 join o1 in _lookup_promoStepFieldDataRepository.GetAll().Where(e => e.PromoStepDataId == promoStepData.PromoStepId) on o.Id equals o1.PromoStepFieldId into j1
                                                 from s1 in j1.DefaultIfEmpty()

                                                 select new CustomPromoStepFieldForView()
                                                 {
                                                     FieldId = o.Id,
                                                     Sequence = o.Sequence,
                                                     FieldName = o.Description,
                                                     FieldValue = !string.IsNullOrEmpty(s1.Value) ? s1.Value : ""
                                                 };

                var promoStepFieldsWithDetailsList = promoStepFieldsWithDetails.OrderBy(s => s.Sequence).ToList();

                promoStepData.PromoStepFields = promoStepFieldsWithDetailsList;
            }

            output.PromoProducts = promoProductsWithDetailsList;
            output.PromoRetailers = promoRetailersWithDetailsList;
            output.PromoCountries = promoCountriesWithDetailsList;
            output.SelectedCountryIds = promoCountryIds;
            output.PromoSteps = promoStepsDataWithDetailsList;

            return output;
        }

        public async Task<long> CreateOrEdit(CreateOrEditPromoDto input)
        {
            if (input.Id == null)
            {
                return await Create(input);
            }
            else
            {
                await Update(input);
                return (long)input.Id;
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Promos_Create)]
        protected virtual async Task<long> Create(CreateOrEditPromoDto input)
        {
            var promo = ObjectMapper.Map<Promo>(input);

            if (AbpSession.TenantId != null)
            {
                promo.TenantId = AbpSession.TenantId;
            }

            var id = await _promoRepository.InsertAndGetIdAsync(promo);
            return id;
        }

        [AbpAuthorize(AppPermissions.Pages_Promos_Edit)]
        protected virtual async Task Update(CreateOrEditPromoDto input)
        {
            Promo promo = await _promoRepository.FirstOrDefaultAsync((long)input.Id);

            ObjectMapper.Map(input, promo);
        }

        [AbpAuthorize(AppPermissions.Pages_Promos_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _promoRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetPromosToExcel(GetAllPromosForExcelInput input)
        {
            var filteredPromos = _promoRepository.GetAll()
                        .Include(e => e.PromoScopeFk)
                        .Include(e => e.CampaignTypeFk)
                        .Include(e => e.ProductCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Promocode.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PromocodeFilter), e => e.Promocode == input.PromocodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(input.MinPromoStartFilter != null, e => e.PromoStart >= input.MinPromoStartFilter)
                        .WhereIf(input.MaxPromoStartFilter != null, e => e.PromoStart <= input.MaxPromoStartFilter)
                        .WhereIf(input.MinPromoEndFilter != null, e => e.PromoEnd >= input.MinPromoEndFilter)
                        .WhereIf(input.MaxPromoEndFilter != null, e => e.PromoEnd <= input.MaxPromoEndFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PromoScopeDescriptionFilter), e => e.PromoScopeFk != null && e.PromoScopeFk.Description == input.PromoScopeDescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CampaignTypeNameFilter), e => e.CampaignTypeFk != null && e.CampaignTypeFk.Name == input.CampaignTypeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryDescriptionFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Description == input.ProductCategoryDescriptionFilter);

            var query = (from o in filteredPromos
                         join o1 in _lookup_promoScopeRepository.GetAll() on o.PromoScopeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_campaignTypeRepository.GetAll() on o.CampaignTypeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetPromoForViewDto()
                         {
                             Promo = new PromoDto
                             {
                                 Promocode = o.Promocode,
                                 Description = o.Description,
                                 PromoStart = o.PromoStart,
                                 PromoEnd = o.PromoEnd,
                                 CloseDate = o.CloseDate,
                                 Comments = o.Comments,
                                 Id = o.Id
                             },
                             PromoScopeDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString(),
                             CampaignTypeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             ProductCategoryDescription = s3 == null || s3.Description == null ? "" : s3.Description.ToString()
                         });


            var promoListDtos = await query.ToListAsync();

            return _promosExcelExporter.ExportToFile(promoListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Promos)]
        public async Task<PagedResultDto<PromoPromoScopeLookupTableDto>> GetAllPromoScopeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_promoScopeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Description != null && e.Description.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var promoScopeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<PromoPromoScopeLookupTableDto>();
            foreach (var promoScope in promoScopeList)
            {
                lookupTableDtoList.Add(new PromoPromoScopeLookupTableDto
                {
                    Id = promoScope.Id,
                    DisplayName = promoScope.Description?.ToString()
                });
            }

            return new PagedResultDto<PromoPromoScopeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Promos)]
        public async Task<PagedResultDto<PromoCampaignTypeLookupTableDto>> GetAllCampaignTypeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_campaignTypeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var campaignTypeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<PromoCampaignTypeLookupTableDto>();
            foreach (var campaignType in campaignTypeList)
            {
                lookupTableDtoList.Add(new PromoCampaignTypeLookupTableDto
                {
                    Id = campaignType.Id,
                    DisplayName = campaignType.Name?.ToString()
                });
            }

            return new PagedResultDto<PromoCampaignTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Promos)]
        public async Task<PagedResultDto<PromoProductCategoryLookupTableDto>> GetAllProductCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Description != null && e.Description.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<PromoProductCategoryLookupTableDto>();
            foreach (var productCategory in productCategoryList)
            {
                lookupTableDtoList.Add(new PromoProductCategoryLookupTableDto
                {
                    Id = productCategory.Id,
                    DisplayName = productCategory.Description?.ToString()
                });
            }

            return new PagedResultDto<PromoProductCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}