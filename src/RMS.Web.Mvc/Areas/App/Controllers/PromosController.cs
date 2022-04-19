using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.Promos;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.PromoPlanner;
using RMS.PromoPlanner.Dtos;
using Abp.Application.Services.Dto;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.Products;
using RMS.SBJ.Retailers;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Promos)]
    public class PromosController : RMSControllerBase
    {
        private readonly IPromosAppService _promosAppService;
        private readonly ICountriesAppService _countriesAppService;
        private readonly IPromoCountriesAppService _promoCountriesAppService;
        private readonly IPromoStepsAppService _promoStepsAppService;
        private readonly IPromoScopesAppService _promoScopesAppService;
        private readonly ICampaignTypesAppService _campaignTypesAppService;
        private readonly IProductCategoriesAppService _productCategoriesAppService;
        private readonly IRetailersAppService _retailersAppService;
        private readonly IPromoStepDatasAppService _promoStepDatasService;
        private readonly IPromoStepFieldDatasAppService _promoStepFieldDatasAppService;

        public PromosController(
            IPromosAppService promosAppService, 
            IPromoStepDatasAppService promoStepDatasAppService, IPromoStepFieldDatasAppService promoStepFieldDatasAppService,
            ICountriesAppService countriesAppService, IPromoCountriesAppService promoCountriesAppService,
            IPromoStepsAppService promoStepsAppService, IPromoScopesAppService promoScopesAppService, 
            ICampaignTypesAppService campaignTypesAppService, IProductCategoriesAppService productCategoriesAppService, IRetailersAppService retailersAppService)
        {
            _promosAppService = promosAppService;
            _promoStepDatasService = promoStepDatasAppService;
            _promoStepFieldDatasAppService = promoStepFieldDatasAppService;
            _countriesAppService = countriesAppService;
            _promoCountriesAppService = promoCountriesAppService;
            _promoStepsAppService = promoStepsAppService;
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
            var promoScopes = await _promoScopesAppService.GetAllWithoutPaging();
            var promoScopeList = from promoScope in promoScopes
                        select new SelectListItem { Value=promoScope.PromoScope.Id.ToString(), Text = promoScope.PromoScope.Description };
            ViewBag.PromoScopes = promoScopeList.ToList();

            //CampaignTypes select
            var campaignTypes = await _campaignTypesAppService.GetAllWithoutPaging();
            var campaignTypeList = from campaignType in campaignTypes
                                 select new SelectListItem { Value = campaignType.CampaignType.Id.ToString(), Text = campaignType.CampaignType.Name };
            ViewBag.CampaignTypes = campaignTypeList.ToList();

            //ProductCategories select
            var productCategories = await _productCategoriesAppService.GetAllWithoutPaging();
            var productCategoryList = from productCategory in productCategories
                                   select new SelectListItem { Value = productCategory.ProductCategory.Id.ToString(), Text = productCategory.ProductCategory.Description };
            ViewBag.ProductCategories = productCategoryList.ToList();

            //Retailers select
            var retailers = await _retailersAppService.GetAllWithoutPaging();
            var retailerList = from retailer in retailers
                                      select new SelectListItem { Value = retailer.Retailer.Id.ToString(), Text = retailer.Retailer.Name };
            ViewBag.Retailers = retailerList.ToList();

            return View(model);
        }

        //[AbpMvcAuthorize(AppPermissions.Pages_Promos_Create, AppPermissions.Pages_Promos_Edit)]
        //public async Task<PartialViewResult> CreateOrEditModal(long? id)
        //{
        //    GetPromoForEditOutput getPromoForEditOutput;

        //    if (id.HasValue)
        //    {
        //        getPromoForEditOutput = await _promosAppService.GetPromoForEdit(new EntityDto<long> { Id = (long)id });
        //    }
        //    else
        //    {
        //        getPromoForEditOutput = new GetPromoForEditOutput
        //        {
        //            Promo = new CreateOrEditPromoDto()
        //        };
        //        getPromoForEditOutput.Promo.PromoStart = DateTime.Now;
        //        getPromoForEditOutput.Promo.PromoEnd = DateTime.Now;
        //    }

        //    var viewModel = new CreateOrEditPromoViewModel()
        //    {
        //        Promo = getPromoForEditOutput.Promo,
        //        PromoScopeDescription = getPromoForEditOutput.PromoScopeDescription,
        //        CampaignTypeName = getPromoForEditOutput.CampaignTypeName,
        //        ProductCategoryDescription = getPromoForEditOutput.ProductCategoryDescription,
        //    };

        //    return PartialView("_CreateOrEditModal", viewModel);
        //}

        //CreateOrEdit non-modal...
        [AbpMvcAuthorize(AppPermissions.Pages_Promos_Create, AppPermissions.Pages_Promos_Edit)]
        public async Task<ActionResult> CreateOrEdit(long? id)
        {
            GetPromoForEditOutput getPromoForEditOutput;

            if (id.HasValue)
            {
                getPromoForEditOutput = await _promosAppService.GetPromoForEdit(new EntityDto<long> { Id = (long)id });
                getPromoForEditOutput.Promo.PromoSteps = getPromoForEditOutput.PromoSteps.ToList();
            }
            else
            {
                getPromoForEditOutput = new GetPromoForEditOutput
                {
                    Promo = new CreateOrEditPromoDto()
                };
                getPromoForEditOutput.Promo.PromoStart = DateTime.Now;
                getPromoForEditOutput.Promo.PromoEnd = DateTime.Now;
                getPromoForEditOutput.Promo.PromoSteps = await _promoStepsAppService.GetAllAsReadOnlyList(new GetAllPromoStepsInput { Filter = "" });
            }

            var viewModel = new CreateOrEditPromoViewModel()
            {
                Promo = getPromoForEditOutput.Promo,
                PromoScopeDescription = getPromoForEditOutput.PromoScopeDescription,
                CampaignTypeName = getPromoForEditOutput.CampaignTypeName,
                ProductCategoryDescription = getPromoForEditOutput.ProductCategoryDescription,
                PromoProducts = getPromoForEditOutput.PromoProducts,
                PromoRetailers = getPromoForEditOutput.PromoRetailers,
                PromoCountries = getPromoForEditOutput.PromoCountries,
                SelectedCountryIds = getPromoForEditOutput.SelectedCountryIds,
                PromoSteps = getPromoForEditOutput.Promo.PromoSteps
            };

            var availableCountries = await _countriesAppService.GetAllWithoutPaging();
            var availableCountriesList = from country in availableCountries
                                         select new SelectListItem { Value = country.CountryId.ToString(), Text = $"{country.CountryName} ({country.CountryCode})" };

            ViewBag.AvailableCountries = availableCountriesList.ToList();

            var availablePromoScopes = await _promoScopesAppService.GetAllWithoutPaging();
            var availablePromoScopesList = from promoScope in availablePromoScopes
                                           select new SelectListItem { Value = promoScope.PromoScope.Id.ToString(), Text = promoScope.PromoScope.Description };

            ViewBag.AvailablePromoScopes = availablePromoScopesList.ToList();

            var availableCampaignTypes = await _campaignTypesAppService.GetAllWithoutPaging();
            var availableCampaignTypesList = from CampaignType in availableCampaignTypes
                                             select new SelectListItem { Value = CampaignType.CampaignType.Id.ToString(), Text = CampaignType.CampaignType.Name };

            ViewBag.AvailableCampaignTypes = availableCampaignTypesList.ToList();

            var availableProductCategories = await _productCategoriesAppService.GetAllWithoutPaging();
            var availableProductCategoriesList = from ProductCategory in availableProductCategories
                                                 select new SelectListItem { Value = ProductCategory.ProductCategory.Id.ToString(), Text = ProductCategory.ProductCategory.Description };

            ViewBag.AvailableProductCategories = availableProductCategoriesList.ToList();

            return View(viewModel);
        }

        //public async Task<PartialViewResult> ViewPromoModal(long id)
        //{
        //    var getPromoForViewDto = await _promosAppService.GetPromoForView(id);

        //    var viewmodel = new PromoViewModel()
        //    {
        //        Promo = getPromoForViewDto.Promo,
        //        PromoScopeDescription = getPromoForViewDto.PromoScopeDescription,
        //        CampaignTypeName = getPromoForViewDto.CampaignTypeName,
        //        ProductCategoryDescription = getPromoForViewDto.ProductCategoryDescription,
        //        PromoSteps = getPromoForViewDto.PromoSteps
        //    };

        //    return PartialView("_ViewPromoModal", viewmodel);
        //}

        //View non-modal...
        public async Task<ActionResult> ViewPromo(long id)
        {
            var getPromoForViewDto = await _promosAppService.GetPromoForView(id);

            var viewmodel = new PromoViewModel()
            {
                Promo = getPromoForViewDto.Promo,
                PromoScopeDescription = getPromoForViewDto.PromoScopeDescription,
                CampaignTypeName = getPromoForViewDto.CampaignTypeName,
                ProductCategoryDescription = getPromoForViewDto.ProductCategoryDescription,
                PromoProducts = getPromoForViewDto.PromoProducts,
                PromoRetailers = getPromoForViewDto.PromoRetailers,
                PromoCountries = getPromoForViewDto.PromoCountries,
                PromoSteps = getPromoForViewDto.PromoSteps,
            };

            return View(viewmodel);
        }

        [HttpPost]
        public async Task<ActionResult> SavePromo([FromBody] CustomCreateOrEditPromoDto objPromo)
        {
            var promoId = await _promosAppService.CreateOrEdit(objPromo.Promo);

            if (objPromo.SelectedCountryIds.Any())
            {
                if (objPromo.Promo.Id != null)
                {
                    var oldPromoCountries = await _promoCountriesAppService.GetAllCountriesForPromo(new GetAllCountriesForPromoInput { PromoId = promoId });

                    foreach (var oldPromoCountry in oldPromoCountries.Items)
                    {
                        await _promoCountriesAppService.Delete(new EntityDto<long> { Id = oldPromoCountry.Id });
                    }
                }

                foreach (var id in objPromo.SelectedCountryIds)
                {
                    await _promoCountriesAppService.CreateOrEdit(new CreateOrEditPromoCountryDto { 
                        CountryId = id, 
                        PromoId = promoId 
                    });
                }
            }

            foreach (var promoStep in objPromo.Promo.PromoSteps)
            {
                var existingPromoStepDataId = await _promoStepDatasService.GetPromoStepDataIdForFks(promoId, promoStep.StepId);

                long promoStepDataId = await _promoStepDatasService.CreateOrEdit(new CreateOrEditPromoStepDataDto { 
                    Id = (int?)existingPromoStepDataId,
                    PromoId = promoId, 
                    PromoStepId = promoStep.StepId, 
                    Description = promoStep.Description 
                });

                if (promoStep.PromoStepFields == null)
                {
                    continue;
                }

                DateTime? confirmationDate = null;

                foreach (var promoStepField in promoStep.PromoStepFields)
                {
                    var existingPromoStepFieldId = await _promoStepFieldDatasAppService.GetPromoStepDataFieldIdForFks(promoStepField.FieldId, promoStepDataId);

                    await _promoStepFieldDatasAppService.CreateOrEdit(new CreateOrEditPromoStepFieldDataDto
                    {
                        Id = (int?)existingPromoStepFieldId,
                        PromoStepDataId = (int?)promoStepDataId,
                        PromoStepFieldId = promoStepField.FieldId,
                        Value = promoStepField.FieldValue
                    });

                    if (!string.IsNullOrWhiteSpace(promoStepField.FieldValue))
                    {
                        confirmationDate = DateTime.Now;
                    }
                }

                if (confirmationDate.HasValue)
                {
                    var promoStepData = await _promoStepDatasService.GetPromoStepDataForView((int)promoStepDataId);

                    promoStepData.PromoStepData.ConfirmationDate = confirmationDate;

                    await _promoStepDatasService.CreateOrEdit(new CreateOrEditPromoStepDataDto { 
                        Id = promoStepData.PromoStepData.Id,
                        PromoId = promoStepData.PromoStepData.PromoId,
                        PromoStepId = promoStepData.PromoStepData.PromoStepId,
                        Description = promoStepData.PromoStepData.Description,
                        ConfirmationDate = promoStepData.PromoStepData.ConfirmationDate 
                    });
                }
            }

            if (objPromo.Promo.Id == null)
            {
                objPromo.Promo.Id = promoId;
                //generate the definitive promo code...
                objPromo.Promo.Promocode = String.Format("PROMO{0}", String.Format("{0:000000}", promoId));
                //save immediately...
                await _promosAppService.CreateOrEdit(objPromo.Promo);
            }

            return View("Promos");
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Promos_Create, AppPermissions.Pages_Promos_Edit)]
        public PartialViewResult PromoScopeLookupTableModal(long? id, string displayName)
        {
            var viewModel = new PromoPromoScopeLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_PromoPromoScopeLookupTableModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Promos_Create, AppPermissions.Pages_Promos_Edit)]
        public PartialViewResult CampaignTypeLookupTableModal(long? id, string displayName)
        {
            var viewModel = new PromoCampaignTypeLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_PromoCampaignTypeLookupTableModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Promos_Create, AppPermissions.Pages_Promos_Edit)]
        public PartialViewResult ProductCategoryLookupTableModal(long? id, string displayName)
        {
            var viewModel = new PromoProductCategoryLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_PromoProductCategoryLookupTableModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Promos_Create, AppPermissions.Pages_Promos_Edit)]
        public PartialViewResult ProductLookupTableModal(long? id, string displayName)
        {
            var viewModel = new PromoProductLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_PromoProductLookupTableModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Promos_Create, AppPermissions.Pages_Promos_Edit)]
        public PartialViewResult RetailerLookupTableModal(long? id, string displayName)
        {
            var viewModel = new PromoRetailerLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_PromoRetailerLookupTableModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Promos_Create, AppPermissions.Pages_Promos_Edit)]
        public PartialViewResult CountryLookupTableModal(long? id, string displayName)
        {
            var viewModel = new PromoCountryLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_PromoCountryLookupTableModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Promos_Create, AppPermissions.Pages_Promos_Edit)]
        public PartialViewResult PromoStepFieldLookupTableModal(long? id, string displayName)
        {
            var viewModel = new PromoStepFieldLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_PromoStepFieldLookupTableModal", viewModel);
        }
    }
}