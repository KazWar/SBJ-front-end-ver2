using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RMS.Authorization;
using RMS.PromoPlanner;
using RMS.PromoPlanner.Dtos;
using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.Products;
using RMS.SBJ.Retailers;
using RMS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.SBJ.Products.Dtos;
using RMS.SBJ.Retailers.Dtos;
using RMS.Web.Areas.App.Models.Promos;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_PromoCalendar)]
    public class PromoCalendar : RMSControllerBase
    {
        private readonly IPromoCalendarAppService _promoCalendarAppService;
        private readonly IPromoScopesAppService _promoScopesAppService;
        private readonly ICampaignTypesAppService _campaignTypesAppService;
        private readonly IProductCategoriesAppService _productCategoriesAppService;
        private readonly IRetailersAppService _retailersAppService;

        public PromoCalendar(IPromoCalendarAppService promoCalendarAppService, IPromoScopesAppService promoScopesAppService, ICampaignTypesAppService campaignTypesAppService,
                                IProductCategoriesAppService productCategoriesAppService, IRetailersAppService retailersAppService)
        {
            _promoCalendarAppService = promoCalendarAppService;
            _promoScopesAppService = promoScopesAppService;
            _campaignTypesAppService = campaignTypesAppService;
            _productCategoriesAppService = productCategoriesAppService;
            _retailersAppService = retailersAppService;

        }
        public async Task<ActionResult> Index()
        {
            var model = new PromosViewModel
            {
                FilterText = ""
            };

            //Promoscope select
            var promoScopes = await _promoScopesAppService.GetAll(new GetAllPromoScopesInput());
            var promoScopeList = from promoScope in promoScopes.Items
                        select new SelectListItem { Value=promoScope.PromoScope.Id.ToString(), Text = promoScope.PromoScope.Description };
            ViewBag.PromoScopes = promoScopeList.ToList();

            //CampaignTypes select
            var campaignTypes = await _campaignTypesAppService.GetAll(new GetAllCampaignTypesInput() { IsActiveFilter = 1 });
            var campaignTypeList = from campaignType in campaignTypes.Items
                                 select new SelectListItem { Value = campaignType.CampaignType.Id.ToString(), Text = campaignType.CampaignType.Name };
            ViewBag.CampaignTypes = campaignTypeList.ToList();

            //ProductCategories select
            var productCategories = await _productCategoriesAppService.GetAll(new GetAllProductCategoriesInput());
            var productCategoryList = from productCategory in productCategories.Items
                                   select new SelectListItem { Value = productCategory.ProductCategory.Id.ToString(), Text = productCategory.ProductCategory.Description };
            ViewBag.ProductCategories = productCategoryList.ToList();

            //Retailers select
            var retailers = await _retailersAppService.GetAll(new GetAllRetailersInput());
            var retailerList = from retailer in retailers.Items
                                      select new SelectListItem { Value = retailer.Retailer.Id.ToString(), Text = retailer.Retailer.Name };
            ViewBag.Retailers = retailerList.ToList();

            return View(model);
        }

        [HttpGet]
        [DontWrapResult]
        public string GetAllEvents(DateTime start, DateTime end, string inputFilter)
        {
            var promosFilter = JsonConvert.DeserializeObject<GetAllPromosInput>(inputFilter);

            List<GetPromoCalendarEventsDto> calendarEvents = _promoCalendarAppService.GetAllEvents(start, end, promosFilter).Result;
            var result = JsonConvert.SerializeObject(calendarEvents); 

            return result;
        }
    }
}
