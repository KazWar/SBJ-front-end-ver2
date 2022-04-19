using RMS.SBJ.CampaignProcesses;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.ProductHandlings.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using RMS.SBJ.HandlingLineRetailers;
using RMS.SBJ.HandlingLineProducts;
using RMS.SBJ.HandlingLines;
using RMS.SBJ.CampaignRetailerLocations;
using RMS.SBJ.RetailerLocations;
using RMS.SBJ.Retailers;
using RMS.SBJ.Products;
using RMS.SBJ.Company;
using Microsoft.AspNetCore.Mvc;
using Abp.Web.Models;

namespace RMS.SBJ.ProductHandlings
{
    [AbpAuthorize(AppPermissions.Pages_ProductHandlings)]
    public class ProductHandlingsAppService : RMSAppServiceBase, IProductHandlingsAppService
    {

        private readonly IRepository<ProductHandling, long> _productHandlingRepository;
        private readonly IRepository<Campaign, long> _lookup_campaignRepository;
        private readonly IRepository<HandlingLineRetailer, long> _lookup_handlingLineRetailerRepository;
        private readonly IRepository<HandlingLineProduct, long> _lookup_handlingLineProductRepository;
        private readonly IRepository<HandlingLine, long> _lookup_handlingLineRepository;
        private readonly IRepository<CampaignRetailerLocation, long> _lookup_campaignRetailerLocationRepository;
        private readonly IRepository<RetailerLocation, long> _lookup_retailerLocationRepository;
        private readonly IRepository<Retailer, long> _lookup_retailerRepository;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IRepository<Address, long> _lookup_addressRepository;
        


        public ProductHandlingsAppService(IRepository<ProductHandling, long> productHandlingRepository, IRepository<Campaign, long> lookup_campaignRepository,
                                            IRepository<HandlingLineRetailer, long> lookup_handlingLineRetailerRepository,
                                            IRepository<HandlingLineProduct, long> lookup_handlingLineProductRepository,
                                            IRepository<HandlingLine, long> lookup_handlingLineRepository,
                                            IRepository<CampaignRetailerLocation, long> lookup_campaignRetailerLocationRepository,
                                            IRepository<RetailerLocation, long> lookup_retailerLocationRepository,
                                            IRepository<Retailer, long> lookup_retailerRepository, 
                                            IRepository<Product, long> lookup_productRepository,
                                            IRepository<Address, long> lookup_addressRepository)
        {
            _productHandlingRepository = productHandlingRepository;
            _lookup_campaignRepository = lookup_campaignRepository;
            _lookup_handlingLineRetailerRepository = lookup_handlingLineRetailerRepository;
            _lookup_handlingLineProductRepository = lookup_handlingLineProductRepository;
            _lookup_handlingLineRepository = lookup_handlingLineRepository;
            _lookup_campaignRetailerLocationRepository = lookup_campaignRetailerLocationRepository;
            _lookup_retailerLocationRepository = lookup_retailerLocationRepository;
            _lookup_retailerRepository = lookup_retailerRepository;
            _lookup_productRepository = lookup_productRepository;
            _lookup_addressRepository = lookup_addressRepository;
        }

        public async Task<PagedResultDto<GetProductHandlingForViewDto>> GetAll(GetAllProductHandlingsInput input)
        {

            var filteredProductHandlings = _productHandlingRepository.GetAll()
                        .Include(e => e.CampaignFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CampaignNameFilter), e => e.CampaignFk != null && e.CampaignFk.Name == input.CampaignNameFilter);

            var pagedAndFilteredProductHandlings = filteredProductHandlings
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productHandlings = from o in pagedAndFilteredProductHandlings
                                   join o1 in _lookup_campaignRepository.GetAll() on o.CampaignId equals o1.Id into j1
                                   from s1 in j1.DefaultIfEmpty()

                                   select new GetProductHandlingForViewDto()
                                   {
                                       ProductHandling = new ProductHandlingDto
                                       {
                                           Description = o.Description,
                                           Id = o.Id
                                       },
                                       CampaignName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                                   };

            var totalCount = await filteredProductHandlings.CountAsync();

            return new PagedResultDto<GetProductHandlingForViewDto>(
                totalCount,
                await productHandlings.ToListAsync()
            );
        }



        [AbpAuthorize(AppPermissions.Pages_ProductHandlings_Edit)]
        public async Task<GetProductHandlingForEditOutput> GetProductHandlingForEdit(EntityDto<long> input)
        {
            var productHandling = await _productHandlingRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductHandlingForEditOutput { ProductHandling = ObjectMapper.Map<CreateOrEditProductHandlingDto>(productHandling) };

            if (output.ProductHandling.CampaignId != null)
            {
                var _lookupCampaign = await _lookup_campaignRepository.FirstOrDefaultAsync((long)output.ProductHandling.CampaignId);
                output.CampaignName = _lookupCampaign?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductHandlingDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductHandlings_Create)]
        protected virtual async Task Create(CreateOrEditProductHandlingDto input)
        {
            var productHandling = ObjectMapper.Map<ProductHandling>(input);


            if (AbpSession.TenantId != null)
            {
                productHandling.TenantId = (int?)AbpSession.TenantId;
            }


            await _productHandlingRepository.InsertAsync(productHandling);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductHandlings_Edit)]
        protected virtual async Task Update(CreateOrEditProductHandlingDto input)
        {
            var productHandling = await _productHandlingRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productHandling);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductHandlings_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productHandlingRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductHandlings)]
        public async Task<PagedResultDto<ProductHandlingCampaignLookupTableDto>> GetAllCampaignForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_campaignRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var campaignList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductHandlingCampaignLookupTableDto>();
            foreach (var campaign in campaignList)
            {
                lookupTableDtoList.Add(new ProductHandlingCampaignLookupTableDto
                {
                    Id = campaign.Id,
                    DisplayName = campaign.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductHandlingCampaignLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}