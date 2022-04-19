using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.ProductHandlings;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.HandlingLines.Exporting;
using RMS.SBJ.HandlingLines.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.SBJ.HandlingLines
{
    [AbpAuthorize(AppPermissions.Pages_HandlingLines)]
    public class HandlingLinesAppService : RMSAppServiceBase, IHandlingLinesAppService
    {
        private readonly IRepository<HandlingLine, long> _handlingLineRepository;
        private readonly IHandlingLinesExcelExporter _handlingLinesExcelExporter;
        private readonly IRepository<CampaignType, long> _lookup_campaignTypeRepository;
        private readonly IRepository<ProductHandling, long> _lookup_productHandlingRepository;

        public HandlingLinesAppService(IRepository<HandlingLine, long> handlingLineRepository, IHandlingLinesExcelExporter handlingLinesExcelExporter, IRepository<CampaignType, long> lookup_campaignTypeRepository, IRepository<ProductHandling, long> lookup_productHandlingRepository)
        {
            _handlingLineRepository = handlingLineRepository;
            _handlingLinesExcelExporter = handlingLinesExcelExporter;
            _lookup_campaignTypeRepository = lookup_campaignTypeRepository;
            _lookup_productHandlingRepository = lookup_productHandlingRepository;

        }

        public async Task<PagedResultDto<GetHandlingLineForViewDto>> GetAll(GetAllHandlingLinesInput input)
        {

            var filteredHandlingLines = _handlingLineRepository.GetAll()
                        .Include(e => e.CampaignTypeFk)
                        .Include(e => e.ProductHandlingFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomerCode.Contains(input.Filter))
                        .WhereIf(input.MinMinimumPurchaseAmountFilter != null, e => e.MinimumPurchaseAmount >= input.MinMinimumPurchaseAmountFilter)
                        .WhereIf(input.MaxMinimumPurchaseAmountFilter != null, e => e.MinimumPurchaseAmount <= input.MaxMinimumPurchaseAmountFilter)
                        .WhereIf(input.MinMaximumPurchaseAmountFilter != null, e => e.MaximumPurchaseAmount >= input.MinMaximumPurchaseAmountFilter)
                        .WhereIf(input.MaxMaximumPurchaseAmountFilter != null, e => e.MaximumPurchaseAmount <= input.MaxMaximumPurchaseAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerCodeFilter), e => e.CustomerCode == input.CustomerCodeFilter)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(input.FixedFilter.HasValue && input.FixedFilter > -1, e => (input.FixedFilter == 1 && e.Fixed) || (input.FixedFilter == 0 && !e.Fixed))
                        .WhereIf(input.ActivationCodeFilter.HasValue && input.ActivationCodeFilter > -1, e => (input.ActivationCodeFilter == 1 && e.ActivationCode) || (input.ActivationCodeFilter == 0 && !e.ActivationCode))
                        .WhereIf(input.MinQuantityFilter != null, e => e.Quantity >= input.MinQuantityFilter)
                        .WhereIf(input.MaxQuantityFilter != null, e => e.Quantity <= input.MaxQuantityFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CampaignTypeNameFilter), e => e.CampaignTypeFk != null && e.CampaignTypeFk.Name == input.CampaignTypeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductHandlingDescriptionFilter), e => e.ProductHandlingFk != null && e.ProductHandlingFk.Description == input.ProductHandlingDescriptionFilter);

            var pagedAndFilteredHandlingLines = filteredHandlingLines
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var handlingLines = from o in pagedAndFilteredHandlingLines
                                join o1 in _lookup_campaignTypeRepository.GetAll() on o.CampaignTypeId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()

                                join o2 in _lookup_productHandlingRepository.GetAll() on o.ProductHandlingId equals o2.Id into j2
                                from s2 in j2.DefaultIfEmpty()

                                select new GetHandlingLineForViewDto()
                                {
                                    HandlingLine = new HandlingLineDto
                                    {
                                        MinimumPurchaseAmount = o.MinimumPurchaseAmount,
                                        MaximumPurchaseAmount = o.MaximumPurchaseAmount,
                                        CustomerCode = o.CustomerCode,
                                        Amount = o.Amount,
                                        Fixed = o.Fixed,
                                        ActivationCode = o.ActivationCode,
                                        Quantity = o.Quantity,
                                        Id = o.Id
                                    },
                                    CampaignTypeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                    ProductHandlingDescription = s2 == null || s2.Description == null ? "" : s2.Description.ToString()
                                };

            var totalCount = await filteredHandlingLines.CountAsync();

            return new PagedResultDto<GetHandlingLineForViewDto>(
                totalCount,
                await handlingLines.ToListAsync()
            );
        }

        public async Task<GetHandlingLineForViewDto> GetHandlingLineForView(long id)
        {
            var handlingLine = await _handlingLineRepository.GetAsync(id);

            var output = new GetHandlingLineForViewDto { HandlingLine = ObjectMapper.Map<HandlingLineDto>(handlingLine) };

            if (output.HandlingLine.CampaignTypeId != null)
            {
                var _lookupCampaignType = await _lookup_campaignTypeRepository.FirstOrDefaultAsync((long)output.HandlingLine.CampaignTypeId);
                output.CampaignTypeName = _lookupCampaignType?.Name?.ToString();
            }

            if (output.HandlingLine.ProductHandlingId != null)
            {
                var _lookupProductHandling = await _lookup_productHandlingRepository.FirstOrDefaultAsync((long)output.HandlingLine.ProductHandlingId);
                output.ProductHandlingDescription = _lookupProductHandling?.Description?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingLines_Edit)]
        public async Task<GetHandlingLineForEditOutput> GetHandlingLineForEdit(EntityDto<long> input)
        {
            var handlingLine = await _handlingLineRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetHandlingLineForEditOutput { HandlingLine = ObjectMapper.Map<CreateOrEditHandlingLineDto>(handlingLine) };

            if (output.HandlingLine.CampaignTypeId != null)
            {
                var _lookupCampaignType = await _lookup_campaignTypeRepository.FirstOrDefaultAsync((long)output.HandlingLine.CampaignTypeId);
                output.CampaignTypeName = _lookupCampaignType?.Name?.ToString();
            }

            if (output.HandlingLine.ProductHandlingId != null)
            {
                var _lookupProductHandling = await _lookup_productHandlingRepository.FirstOrDefaultAsync((long)output.HandlingLine.ProductHandlingId);
                output.ProductHandlingDescription = _lookupProductHandling?.Description?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditHandlingLineDto input)
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

        [AbpAuthorize(AppPermissions.Pages_HandlingLines_Create)]
        protected virtual async Task Create(CreateOrEditHandlingLineDto input)
        {
            var handlingLine = ObjectMapper.Map<HandlingLine>(input);

            if (AbpSession.TenantId != null)
            {
                handlingLine.TenantId = (int?)AbpSession.TenantId;
            }

            await _handlingLineRepository.InsertAsync(handlingLine);
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingLines_Edit)]
        protected virtual async Task Update(CreateOrEditHandlingLineDto input)
        {
            var handlingLine = await _handlingLineRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, handlingLine);
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingLines_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _handlingLineRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetHandlingLinesToExcel(GetAllHandlingLinesForExcelInput input)
        {

            var filteredHandlingLines = _handlingLineRepository.GetAll()
                        .Include(e => e.CampaignTypeFk)
                        .Include(e => e.ProductHandlingFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomerCode.Contains(input.Filter))
                        .WhereIf(input.MinMinimumPurchaseAmountFilter != null, e => e.MinimumPurchaseAmount >= input.MinMinimumPurchaseAmountFilter)
                        .WhereIf(input.MaxMinimumPurchaseAmountFilter != null, e => e.MinimumPurchaseAmount <= input.MaxMinimumPurchaseAmountFilter)
                        .WhereIf(input.MinMaximumPurchaseAmountFilter != null, e => e.MaximumPurchaseAmount >= input.MinMaximumPurchaseAmountFilter)
                        .WhereIf(input.MaxMaximumPurchaseAmountFilter != null, e => e.MaximumPurchaseAmount <= input.MaxMaximumPurchaseAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerCodeFilter), e => e.CustomerCode == input.CustomerCodeFilter)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(input.FixedFilter.HasValue && input.FixedFilter > -1, e => (input.FixedFilter == 1 && e.Fixed) || (input.FixedFilter == 0 && !e.Fixed))
                        .WhereIf(input.ActivationCodeFilter.HasValue && input.ActivationCodeFilter > -1, e => (input.ActivationCodeFilter == 1 && e.ActivationCode) || (input.ActivationCodeFilter == 0 && !e.ActivationCode))
                        .WhereIf(input.MinQuantityFilter != null, e => e.Quantity >= input.MinQuantityFilter)
                        .WhereIf(input.MaxQuantityFilter != null, e => e.Quantity <= input.MaxQuantityFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CampaignTypeNameFilter), e => e.CampaignTypeFk != null && e.CampaignTypeFk.Name == input.CampaignTypeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductHandlingDescriptionFilter), e => e.ProductHandlingFk != null && e.ProductHandlingFk.Description == input.ProductHandlingDescriptionFilter);

            var query = (from o in filteredHandlingLines
                         join o1 in _lookup_campaignTypeRepository.GetAll() on o.CampaignTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_productHandlingRepository.GetAll() on o.ProductHandlingId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetHandlingLineForViewDto()
                         {
                             HandlingLine = new HandlingLineDto
                             {
                                 MinimumPurchaseAmount = o.MinimumPurchaseAmount,
                                 MaximumPurchaseAmount = o.MaximumPurchaseAmount,
                                 CustomerCode = o.CustomerCode,
                                 Amount = o.Amount,
                                 Fixed = o.Fixed,
                                 ActivationCode = o.ActivationCode,
                                 Quantity = o.Quantity,
                                 Id = o.Id
                             },
                             CampaignTypeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             ProductHandlingDescription = s2 == null || s2.Description == null ? "" : s2.Description.ToString()
                         });

            var handlingLineListDtos = await query.ToListAsync();

            return _handlingLinesExcelExporter.ExportToFile(handlingLineListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingLines)]
        public async Task<List<HandlingLineCampaignTypeLookupTableDto>> GetAllCampaignTypeForTableDropdown()
        {
            return await _lookup_campaignTypeRepository.GetAll()
                .Select(campaignType => new HandlingLineCampaignTypeLookupTableDto
                {
                    Id = campaignType.Id,
                    DisplayName = campaignType == null || campaignType.Name == null ? "" : campaignType.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingLines)]
        public async Task<PagedResultDto<HandlingLineProductHandlingLookupTableDto>> GetAllProductHandlingForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productHandlingRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Description != null && e.Description.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productHandlingList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HandlingLineProductHandlingLookupTableDto>();
            foreach (var productHandling in productHandlingList)
            {
                lookupTableDtoList.Add(new HandlingLineProductHandlingLookupTableDto
                {
                    Id = productHandling.Id,
                    DisplayName = productHandling.Description?.ToString()
                });
            }

            return new PagedResultDto<HandlingLineProductHandlingLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}