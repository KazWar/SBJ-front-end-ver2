using RMS.SBJ.CampaignProcesses;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.ProductGifts.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using RMS.Storage;

namespace RMS.SBJ.ProductGifts
{
    [AbpAuthorize(AppPermissions.Pages_ProductGifts)]
    public class ProductGiftsAppService : RMSAppServiceBase, IProductGiftsAppService
    {
        private readonly IRepository<ProductGift, long> _productGiftRepository;
        private readonly IRepository<Campaign, long> _lookup_campaignRepository;

        public ProductGiftsAppService(IRepository<ProductGift, long> productGiftRepository, IRepository<Campaign, long> lookup_campaignRepository)
        {
            _productGiftRepository = productGiftRepository;
            _lookup_campaignRepository = lookup_campaignRepository;

        }

        public async Task<PagedResultDto<GetProductGiftForViewDto>> GetAll(GetAllProductGiftsInput input)
        {

            var filteredProductGifts = _productGiftRepository.GetAll()
                        .Include(e => e.CampaignFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ProductCode.Contains(input.Filter) || e.GiftName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CampaignNameFilter), e => e.CampaignFk != null && e.CampaignFk.Name == input.CampaignNameFilter);

            var pagedAndFilteredProductGifts = filteredProductGifts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productGifts = from o in pagedAndFilteredProductGifts
                               join o1 in _lookup_campaignRepository.GetAll() on o.CampaignId equals o1.Id into j1
                               from s1 in j1.DefaultIfEmpty()

                               select new
                               {

                                   Id = o.Id,
                                   CampaignName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                               };

            var totalCount = await filteredProductGifts.CountAsync();

            var dbList = await productGifts.ToListAsync();
            var results = new List<GetProductGiftForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductGiftForViewDto()
                {
                    ProductGift = new ProductGiftDto
                    {

                        Id = o.Id,
                    },
                    CampaignName = o.CampaignName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductGiftForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProductGiftForViewDto> GetProductGiftForView(long id)
        {
            var productGift = await _productGiftRepository.GetAsync(id);

            var output = new GetProductGiftForViewDto { ProductGift = ObjectMapper.Map<ProductGiftDto>(productGift) };

            if (output.ProductGift.CampaignId != null)
            {
                var _lookupCampaign = await _lookup_campaignRepository.FirstOrDefaultAsync((long)output.ProductGift.CampaignId);
                output.CampaignName = _lookupCampaign?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProductGifts_Edit)]
        public async Task<GetProductGiftForEditOutput> GetProductGiftForEdit(EntityDto<long> input)
        {
            var productGift = await _productGiftRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductGiftForEditOutput { ProductGift = ObjectMapper.Map<CreateOrEditProductGiftDto>(productGift) };

            if (output.ProductGift.CampaignId != null)
            {
                var _lookupCampaign = await _lookup_campaignRepository.FirstOrDefaultAsync((long)output.ProductGift.CampaignId);
                output.CampaignName = _lookupCampaign?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductGiftDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductGifts_Create)]
        protected virtual async Task Create(CreateOrEditProductGiftDto input)
        {
            var productGift = ObjectMapper.Map<ProductGift>(input);

            if (AbpSession.TenantId != null)
            {
                productGift.TenantId = (int?)AbpSession.TenantId;
            }

            await _productGiftRepository.InsertAsync(productGift);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductGifts_Edit)]
        protected virtual async Task Update(CreateOrEditProductGiftDto input)
        {
            var productGift = await _productGiftRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productGift);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductGifts_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productGiftRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductGifts)]
        public async Task<PagedResultDto<ProductGiftCampaignLookupTableDto>> GetAllCampaignForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_campaignRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var campaignList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductGiftCampaignLookupTableDto>();
            foreach (var campaign in campaignList)
            {
                lookupTableDtoList.Add(new ProductGiftCampaignLookupTableDto
                {
                    Id = campaign.Id,
                    DisplayName = campaign.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductGiftCampaignLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}