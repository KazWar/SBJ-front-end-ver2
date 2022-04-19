using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.PromoPlanner.Dtos;
using RMS.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using RMS.SBJ.Products;
using Abp.Linq.Extensions;

namespace RMS.PromoPlanner
{
    [AbpAuthorize(AppPermissions.Pages_PromoCalendar)]
    public class PromoCalendarAppService : RMSAppServiceBase, IPromoCalendarAppService
    {
        private readonly IRepository<Promo, long> _promoRepository;
        private readonly IRepository<ProductCategory, long> _productCategoryRepository;
        private readonly IRepository<PromoRetailer, long> _lookup_promoRetailerRepository;


        public PromoCalendarAppService(IRepository<Promo, long> promoRepository, IRepository<ProductCategory, long> productCategoryRepository, IRepository<PromoRetailer, long> lookup_promoRetailerRepository)
        {
            _promoRepository = promoRepository;
            _productCategoryRepository = productCategoryRepository;
            _lookup_promoRetailerRepository = lookup_promoRetailerRepository;
        }

        public async Task<List<GetPromoCalendarEventsDto>> GetAllEvents(DateTime Start, DateTime End, GetAllPromosInput input)
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

            var activePromos = await filteredPromos.Where(p => p.PromoStart.Date < End && p.PromoEnd.Date > Start).ToListAsync();
            var listPromoEvents = (from promo in activePromos
                                   select new GetPromoCalendarEventsDto
                                   {
                                       id = promo.Id,
                                       title = promo.Description,
                                       start = promo.PromoStart.Date.ToString("yyyy-MM-dd"),
                                       end = promo.PromoEnd.Date.ToString("yyyy-MM-dd"),
                                       allDay = true,
                                       url = $"/App/Promos/ViewPromo?id={promo.Id}",
                                       backgroundColor = _productCategoryRepository.Get((long)promo.ProductCategoryId).Color
                                   }).ToList();

            return listPromoEvents;
        }

    }
}
