using Abp.Domain.Repositories;
using RMS.SBJ.CampaignProcesses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
//using IO.Swagger.Api;
//using IO.Swagger.Client;
//using IO.Swagger.Model;
using System.Net.Http;
using Abp.Runtime.Session;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RMS.SBJ.Makita.Dtos;
using Microsoft.EntityFrameworkCore;
using RMS.SBJ.ProductHandlings;
using RMS.SBJ.HandlingLineProducts;
using RMS.SBJ.HandlingLines;
using RMS.SBJ.Products;
using RMS.SBJ.CampaignProcesses.Dtos;
using Abp.Application.Services.Dto;
using RMS.SBJ.RetailerLocations;
using RMS.SBJ.CampaignRetailerLocations;
using RMS.SBJ.Retailers;
using System.Linq;
using Abp.Authorization;
using RMS.Authorization;
using RMS.SBJ.Forms;
using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.ProductGifts;
using RMS.SBJ.PurchaseRegistrationFields;
using System.Globalization;
using RMS.SBJ.Helpers;

namespace RMS.SBJ.Makita
{
    [AbpAuthorize(AppPermissions.Pages_MakitaCampaigns)]
    public class MakitaCampaignsAppService : RMSAppServiceBase, IMakitaCampaignsAppService
    {
        private readonly IRepository<Campaign, long> _campaignRepository;
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly ILogger<MakitaCampaignsAppService> _logger;
        private readonly IRepository<ProductHandling, long> _productHandlingRepository;
        private readonly IRepository<HandlingLineProduct, long> _handlingLineProductRepository;
        private readonly IRepository<HandlingLine, long> _handlingLineRepository;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IRepository<RetailerLocation, long> _lookup_retailerLocationRepository;
        private readonly IRepository<Retailer, long> _lookup_retailerRepository;
        private readonly IRepository<CampaignRetailerLocation, long> _lookup_campaignRetailerLocationRepository;
        private readonly ICampaignsAppService _campaignsAppService;
        private readonly IRepository<Form, long> _lookup_formRepository;
        private readonly IRepository<FormLocale, long> _formLocaleRepository;
        private readonly IRepository<CampaignForm, long> _lookup_campaignFormRepository; 
        private readonly IRepository<Locale, long> _lookup_localeRepository;
        private readonly IRepository<FormBlock, long> _formBlockRepository;
        private readonly IRepository<FormBlockField, long> _formBlockFieldRepository;
        private readonly IRepository<ProductGift, long> _productGiftRepository;
        private readonly IRepository<PurchaseRegistrationField, long> _purchaseRegistrationFieldRepository;

        public MakitaCampaignsAppService(
            IRepository<Campaign, long> campaignRepository,
            ILogger<MakitaCampaignsAppService> logger,
            IRepository<ProductHandling, long> productHandlingRepository,
            IRepository<HandlingLineProduct, long> handlingLineProductRepository,
            IRepository<HandlingLine, long> handlingLineRepository,
            IRepository<Product, long> lookup_productRepository,
            ICampaignsAppService campaignsAppService,
            IRepository<RetailerLocation, long> lookup_retailerLocationRepository,
            IRepository<Retailer, long> lookup_retailerRepository,
            IRepository<Form, long> lookup_formRepository,
            IRepository<CampaignRetailerLocation, long> lookup_campaignRetailerLocationRepository,
            IRepository<FormLocale, long> formLocaleRepository,
            IRepository<CampaignForm, long> lookup_campaignFormRepository,
            IRepository<Locale, long> lookup_localeRepository,
            IRepository<FormBlock, long> formBlockRepository,
            IRepository<FormBlockField, long> formBlockFieldRepository,
            IRepository<ProductGift, long> productGiftRepository,
            IRepository<PurchaseRegistrationField, long> purchaseRegistrationFieldRepository
        )
        {
            _campaignRepository = campaignRepository;
            _logger = logger;
            _productHandlingRepository = productHandlingRepository;
            _handlingLineProductRepository = handlingLineProductRepository;
            _handlingLineRepository = handlingLineRepository;
            _lookup_productRepository = lookup_productRepository;
            _campaignsAppService = campaignsAppService;
            _lookup_retailerLocationRepository = lookup_retailerLocationRepository;
            _lookup_retailerRepository = lookup_retailerRepository;
            _lookup_campaignRetailerLocationRepository = lookup_campaignRetailerLocationRepository;
            _lookup_formRepository = lookup_formRepository;
            _formLocaleRepository = formLocaleRepository;
            _lookup_campaignFormRepository = lookup_campaignFormRepository;
            _lookup_localeRepository = lookup_localeRepository;
            _formBlockRepository = formBlockRepository;
            _formBlockFieldRepository = formBlockFieldRepository;
            _productGiftRepository = productGiftRepository;
            _purchaseRegistrationFieldRepository = purchaseRegistrationFieldRepository;
    }

        public async Task<PagedResultDto<GetCampaignForViewDto>> GetAll(GetAllCampaignsInput input)
        {
            var allCampaigns = await _campaignsAppService.GetAll(input);

            return allCampaigns;
        }


        public async Task<object> UpdateMakitaCampaigns()
        {
            int tenantId = AbpSession.GetTenantId();

            _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", MakitaConsts.MakitaBearerToken);

            HttpResponseMessage response = await _httpClient.GetAsync($"https://api.makita.nl/actioncampaign/v1/campaigns");
            if (response == null)
            {
                var error = "HttpResponseError";
                _logger.LogError($"Error in {nameof(MakitaCampaignsAppService)}: response of type {nameof(HttpResponseMessage)} is null.");
                return error;
            }

            string responseString = await response.Content.ReadAsStringAsync();
            var allCampaigns = JsonConvert.DeserializeObject<List<MakitaCampaignsDto>>(responseString);

            foreach (var campaign in allCampaigns)
            {
                if (string.IsNullOrWhiteSpace(campaign.code)) continue;
                if (campaign.code == "test" || campaign.code == "asdsad" || campaign.code == "makita") continue;

                var makitaCampaigns = _campaignRepository.GetAll().FirstOrDefaultAsync(fl => fl.ExternalCode == campaign.code);
                if (makitaCampaigns.Result != null) continue;
                
                var newCampaign = new Campaign
                {
                    TenantId = tenantId,
                    Name = campaign.name,
                    Description = campaign.name,
                    EndDate = Convert.ToDateTime(campaign.end_date),
                    StartDate = Convert.ToDateTime(campaign.start_date),
                    ExternalCode = campaign.code,
                    ThumbnailImagePath = campaign.picture,
                    ExternalId = campaign.id
                };

                var insertCampaign = await _campaignRepository.InsertAndGetIdAsync(newCampaign);
                if (insertCampaign <= 0)
                {
                    var error = "campaignInsertError";
                    _logger.LogError($"Error in {nameof(MakitaCampaignsAppService)}: returned {nameof(insertCampaign)} variable is less than or equal to 0.");
                    return error;
                };

                var newProductHandling = new ProductHandling
                {
                    CampaignId = insertCampaign,
                    TenantId = tenantId,
                    Description = campaign.code
                };
                var insertProductHandling = await _productHandlingRepository.InsertAndGetIdAsync(newProductHandling);
                if (insertProductHandling <= 0)
                {
                    var error = "insertProductHandlingError";
                    _logger.LogError($"Error in {nameof(MakitaCampaignsAppService)}: returned {nameof(insertProductHandling)} variable is less than or equal to 0.");
                    return error;
                };

                var newHandlingLine = new HandlingLine
                {
                    TenantId = tenantId,
                    CustomerCode = string.Empty,
                    Fixed = true,
                    ActivationCode = false,
                    CampaignTypeId = 2,
                    ProductHandlingId = insertProductHandling,
                    Quantity = 1,
                    Percentage = false
                };
                var insertHandlingLine = await _handlingLineRepository.InsertAndGetIdAsync(newHandlingLine);
                if (insertHandlingLine <= 0)
                {
                    var error = "insertHandlingLineError";
                    _logger.LogError($"Error in {nameof(MakitaCampaignsAppService)}: returned {nameof(insertHandlingLine)} variable is less than or equal to 0.");
                    return error;
                };

                var newHandlingLineUnknown = new HandlingLine
                {
                    TenantId = tenantId,
                    CustomerCode = "UNKNOWN",
                    Fixed = false,
                    ActivationCode = false,
                    CampaignTypeId = 2,
                    ProductHandlingId = insertProductHandling,
                    Percentage = false
                };
                var insertHandlingLineUnknown = await _handlingLineRepository.InsertAndGetIdAsync(newHandlingLineUnknown);
                if (insertHandlingLineUnknown <= 0)
                {
                    var error = "insertHandlingLineUnknownError";
                    _logger.LogError($"Error in {nameof(MakitaCampaignsAppService)}: returned {nameof(insertHandlingLineUnknown)} variable is less than or equal to 0.");
                    return error;
                };

                var newHandlingLinePoints = new HandlingLine
                {
                    TenantId = tenantId,
                    CustomerCode = "POINTS",
                    Fixed = false,
                    ActivationCode = false,
                    CampaignTypeId = 2,
                    ProductHandlingId = insertProductHandling,
                    Percentage = false
                };
                var insertHandlingLinePoints = await _handlingLineRepository.InsertAndGetIdAsync(newHandlingLinePoints);
                if (insertHandlingLinePoints <= 0)
                {
                    var error = "insertHandlingLineUnknownError";
                    _logger.LogError($"Error in {nameof(MakitaCampaignsAppService)}: returned {nameof(insertHandlingLineUnknown)} variable is less than or equal to 0.");
                    return error;
                };

                //Forms start...

                var companyForm = await _lookup_formRepository.GetAsync(2);
                var companyFormLocales = await _formLocaleRepository.GetAll().Where(f => f.FormId == companyForm.Id).ToListAsync();

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
                    CampaignId = insertCampaign,
                    FormId = formId,
                    IsActive = true
                });

                foreach (var companyFormLocale in companyFormLocales)
                {
                    //forms - localized...
                    var companyFormLocaleInfo = await _lookup_localeRepository.GetAsync(companyFormLocale.LocaleId);
                    var companyFormBlocks = await _formBlockRepository.GetAll().Where(b => b.FormLocaleId == companyFormLocale.Id).ToListAsync();

                    var campaignFormLocaleId = await _formLocaleRepository.InsertAndGetIdAsync(new FormLocale
                    {
                        TenantId = AbpSession.TenantId,
                        CreatorUserId = AbpSession.UserId ?? 1,
                        CreationTime = DateTime.Now,
                        FormId = formId,
                        LocaleId = companyFormLocale.LocaleId,
                        Description = companyFormLocaleInfo.Description
                    });

                    foreach (var companyFormBlock in companyFormBlocks)
                    {
                        var campaignFormBlockId = await _formBlockRepository.InsertAndGetIdAsync(new FormBlock
                        {
                            TenantId = AbpSession.TenantId,
                            CreatorUserId = AbpSession.UserId ?? 1,
                            CreationTime = DateTime.Now,
                            FormLocaleId = campaignFormLocaleId,
                            Description = companyFormBlock.Description,
                            SortOrder = companyFormBlock.SortOrder
                        });

                        var companyFormBlockFields = await _formBlockFieldRepository.GetAll().Where(f => f.FormBlockId == companyFormBlock.Id).ToListAsync();
                        foreach (var companyFormBlockField in companyFormBlockFields)
                        {
                            var campaignFormBlockFieldId = await _formBlockFieldRepository.InsertAndGetIdAsync(new FormBlockField
                            {
                                TenantId = AbpSession.TenantId,
                                CreatorUserId = AbpSession.UserId ?? 1,
                                CreationTime = DateTime.Now,
                                FormBlockId = campaignFormBlockId,
                                FormFieldId = companyFormBlockField.FormFieldId,
                                SortOrder = companyFormBlockField.SortOrder
                            });
                        }


                        if (companyFormBlock.Description == "Productregistratie")
                        {
                            var purchaseRegistrationGiftField = _purchaseRegistrationFieldRepository.GetAll().Where(pr => pr.Description == "Gift_Id").FirstOrDefault();
                            var campaignFormBlockFieldId = await _formBlockFieldRepository.InsertAndGetIdAsync(new FormBlockField
                            {
                                TenantId = AbpSession.TenantId,
                                CreatorUserId = AbpSession.UserId ?? 1,
                                CreationTime = DateTime.Now,
                                FormBlockId = campaignFormBlockId,
                                FormFieldId = purchaseRegistrationGiftField.FormFieldId,
                                SortOrder = 100
                            });
                        }
                    }
                    //Forms end
                }

            }
            return true;
        }

        public async Task<object> UpdateMakitaRetailers()
        {
            int tenantId = AbpSession.GetTenantId();

            _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", MakitaConsts.MakitaBearerToken);

            HttpResponseMessage response = await _httpClient.GetAsync($"https://api.makita.nl/actioncampaign/v2/dealers");
            if (response == null)
            {
                var error = "HttpResponseError";

                _logger.LogError($"Error in {nameof(MakitaCampaignsAppService)}: response of type {nameof(HttpResponseMessage)} is null.");

                return error;
            }

            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");
            DateTime nowUtc = DateTime.UtcNow;
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            DateTime westTime = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, cstZone);

            string responseString = await response.Content.ReadAsStringAsync();
            var allRetailers = JsonConvert.DeserializeObject<MakitaRetailersDto>(responseString);
            var mainRetailer = _lookup_retailerRepository.GetAll().FirstOrDefault();
            var allCampaigns = await _campaignRepository.GetAll().Where(c => c.CampaignCode != null && c.EndDate > westTime).ToListAsync();
            var pagesCount = Convert.ToInt32(allRetailers.last_page);

            for (int i = 1; i <= pagesCount; i++)
            {
                HttpResponseMessage getDealers = await _httpClient.GetAsync($"https://api.makita.nl//actioncampaign/v2/dealers?&page=" + i);
                if (getDealers == null)
                {
                    var error = "HttpResponseError";

                    _logger.LogError($"Error in {nameof(MakitaCampaignsAppService)}: response of type {nameof(HttpResponseMessage)} is null.");

                    return error;
                }

                string responseDealers = await getDealers.Content.ReadAsStringAsync();
                var unpackedDealers = JsonConvert.DeserializeObject<MakitaRetailersDto>(responseDealers);

                foreach (var retailer in unpackedDealers.data)
                {
                    var makitaRetailers = await _lookup_retailerLocationRepository.GetAll().Where(rl => rl.ExternalCode == retailer.debiteur_nr).FirstOrDefaultAsync();
                    if (makitaRetailers == null)
                    {
                        var newRetailer = new RetailerLocation
                        {
                            TenantId = tenantId,
                            Name = retailer.bedrijfsnaam,
                            RetailerId = mainRetailer.Id,
                            PostalCode = retailer.postcode,
                            City = retailer.plaats,
                            ExternalCode = retailer.debiteur_nr,
                            Address = retailer.adres
                        };

                        var insertRetailer_GetId = await _lookup_retailerLocationRepository.InsertAndGetIdAsync(newRetailer);

                        foreach (var campaign in allCampaigns)
                        {
                            var newCampaignRetailerLocation = new CampaignRetailerLocation
                            {
                                TenantId = tenantId,
                                CampaignId = campaign.Id,
                                RetailerLocationId = insertRetailer_GetId
                            };

                            var insertCampaignRetailerLocation = await _lookup_campaignRetailerLocationRepository.InsertAndGetIdAsync(newCampaignRetailerLocation);
                            if (insertCampaignRetailerLocation <= 0)
                            {
                                var error = "campaignRetailerLocationInsertError";

                                _logger.LogError($"Error in {nameof(MakitaCampaignsAppService)}: returned {nameof(insertCampaignRetailerLocation)} variable is less than or equal to 0.");

                                return error;
                            };
                        }
                    }
                    else
                    {
                        var makitaRetailerExist = _lookup_retailerLocationRepository.GetAll().FirstOrDefault(rl => rl.ExternalCode == retailer.debiteur_nr);
                        makitaRetailerExist.Address = retailer.adres;
                        makitaRetailerExist.City = retailer.plaats;
                        makitaRetailerExist.Name = retailer.bedrijfsnaam;
                        makitaRetailerExist.PostalCode = retailer.postcode;
                        makitaRetailerExist.ExternalId = retailer.id.ToString();
                        await _lookup_retailerLocationRepository.UpdateAsync(makitaRetailerExist);

                        foreach (var campaign in allCampaigns)
                        {
                            var campaignRetailerLocationRepo = await _lookup_campaignRetailerLocationRepository.GetAll().Where(cr => cr.RetailerLocationId == makitaRetailerExist.Id && cr.CampaignId == campaign.Id).FirstOrDefaultAsync();
                            if (campaignRetailerLocationRepo == null)
                            {
                                var newCampaignRetailerLocation = new CampaignRetailerLocation
                                {
                                    TenantId = tenantId,
                                    CampaignId = campaign.Id,
                                    RetailerLocationId = makitaRetailerExist.Id
                                };

                                var insertCampaignRetailerLocation = await _lookup_campaignRetailerLocationRepository.InsertAndGetIdAsync(newCampaignRetailerLocation);
                                if (insertCampaignRetailerLocation <= 0)
                                {
                                    var error = "campaignRetailerLocationInsertError";

                                    _logger.LogError($"Error in {nameof(MakitaCampaignsAppService)}: returned {nameof(insertCampaignRetailerLocation)} variable is less than or equal to 0.");

                                    return error;
                                };
                            }
                        }
                    }


                }
            }




            return true;
        }

        public async Task<object> UpdateMakitaCampaignProducts()
        {
            int tenantId = AbpSession.GetTenantId();
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");
            DateTime nowUtc = DateTime.UtcNow;
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            DateTime westTime = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, cstZone);

            var makitaCampaigns = _campaignRepository.GetAll().Where(cp => cp.EndDate > westTime);
            //var campaignIds = new List<long>() { 23, 24, 25 };
            //var makitaCampaigns = _campaignRepository.GetAll().Where(cp => campaignIds.Contains(cp.Id) );

            _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", MakitaConsts.MakitaBearerToken);

            foreach (var campaign in makitaCampaigns)
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"https://api.makita.nl/actioncampaign/v1/campaigns/" + campaign.ExternalId + "/products");
                if (response == null)
                {
                    _logger.LogError($"Error in {nameof(MakitaRegistrationsAppService)}: response of type {nameof(HttpResponseMessage)} is null.");
                }

                string responseString = await response.Content.ReadAsStringAsync();
                var allProducts = JsonConvert.DeserializeObject<List<MakitaCampaignProductsDto>>(responseString);

                foreach (var product in allProducts)
                {          
                    var productRepository = await _lookup_productRepository.GetAll().FirstOrDefaultAsync(fl => fl.ProductCode == product.Product_id);
                    if (productRepository == null)
                    {
                        var newProduct = new Product
                        {
                            TenantId = tenantId,
                            ProductCode = product.Product_id,
                            Description = product.Title,
                            ProductCategoryId = 1,
                        };                        
                        var insertProduct_getId = await _lookup_productRepository.InsertAndGetIdAsync(newProduct);

                        var productGiftsRepository = await _productGiftRepository.GetAll().Where(pg => pg.CampaignId == campaign.Id && pg.ProductCode == product.Product_id).FirstOrDefaultAsync();
                        if (productGiftsRepository == null)
                        {
                            foreach (var gift in product.Gift_choices)
                            {
                                var newProductGift = new ProductGift
                                {
                                    CampaignId = campaign.Id,
                                    GiftId = Convert.ToInt32(gift.Id),
                                    GiftName = gift.Name,
                                    ProductCode = product.Product_id,
                                    TenantId = tenantId,
                                    TotalPoints = Convert.ToInt32(gift.Total_points)
                                };
                                var insertProductGift = await _productGiftRepository.InsertAndGetIdAsync(newProductGift);
                            }

                        }

                        var productHandling = await _productHandlingRepository.GetAll().FirstOrDefaultAsync(fl => fl.CampaignId == Convert.ToInt32(campaign.Id));
                        var handlingLine = await _handlingLineRepository.GetAll().Where(hl => hl.ProductHandlingId == productHandling.Id && hl.CustomerCode != "UNKNOWN" && hl.CustomerCode != "POINTS").FirstOrDefaultAsync();
                        var newHandlingLineProduct = new HandlingLineProduct
                        {
                            TenantId = tenantId,
                            HandlingLineId = handlingLine.Id,
                            ProductId = insertProduct_getId,
                        };

                        var insertHandlingLineProduct_getId = await _handlingLineProductRepository.InsertAndGetIdAsync(newHandlingLineProduct);
                        if (insertHandlingLineProduct_getId <= 0)
                        {
                            var error = "insertHandlingLineProductError";
                            _logger.LogError($"Error in {nameof(MakitaCampaignsAppService)}: returned {nameof(insertHandlingLineProduct_getId)} variable is less than or equal to 0.");
                            return error;
                        };
                    }
                    else if (productRepository != null)
                    {
                        var productGiftsRepository = await _productGiftRepository.GetAll().Where(pg => pg.CampaignId == campaign.Id && pg.ProductCode == product.Product_id).ToListAsync();
                        if (productGiftsRepository.Count == 0)
                        {
                            foreach (var gift in product.Gift_choices)
                            {
                                var newProductGift = new ProductGift
                                {
                                    CampaignId = campaign.Id,
                                    GiftId = Convert.ToInt32(gift.Id),
                                    GiftName = gift.Name,
                                    ProductCode = product.Product_id,
                                    TenantId = tenantId,
                                    TotalPoints = Convert.ToInt32(gift.Total_points)
                                };
                                try
                                {
                                    var insertProductGift = await _productGiftRepository.InsertAndGetIdAsync(newProductGift);
                                }
                                catch (Exception ex)
                                {

                                    throw ex;
                                }
                                
                            }
                        }
                        else
                        {
                            foreach (var gift in product.Gift_choices)
                            {
                                var giftItem = productGiftsRepository.Where(pg => pg.GiftId == Convert.ToInt32(gift.Id)).FirstOrDefault();
                                if (giftItem == null)
                                {
                                    var newProductGift = new ProductGift
                                    {
                                        CampaignId = campaign.Id,
                                        GiftId = Convert.ToInt32(gift.Id),
                                        GiftName = gift.Name,
                                        ProductCode = product.Product_id,
                                        TenantId = tenantId,
                                        TotalPoints = Convert.ToInt32(gift.Total_points)
                                    };
                                    var insertProductGift = await _productGiftRepository.InsertAndGetIdAsync(newProductGift);
                                }
                                else
                                {
                                    giftItem.GiftName = gift.Name;
                                    giftItem.ProductCode = product.Product_id;
                                    giftItem.TenantId = tenantId;
                                    giftItem.TotalPoints = Convert.ToInt32(gift.Total_points);
                                    await _productGiftRepository.UpdateAsync(giftItem);
                                }                                
                            }

                            var productHandling = await _productHandlingRepository.GetAll().FirstOrDefaultAsync(fl => fl.CampaignId == Convert.ToInt32(campaign.Id));
                            var handlingLine = await _handlingLineRepository.GetAll().Where(hl => hl.ProductHandlingId == productHandling.Id && hl.CustomerCode != "UNKNOWN" && hl.CustomerCode != "POINTS").FirstOrDefaultAsync();

                            var handlingLineProduct = await _handlingLineProductRepository.GetAll().Where(hlp => hlp.ProductId == productRepository.Id && hlp.HandlingLineId == handlingLine.Id).FirstOrDefaultAsync();
                            if (handlingLineProduct == null)
                            {
                                var newHandlingLineProduct = new HandlingLineProduct
                                {
                                    TenantId = tenantId,
                                    HandlingLineId = handlingLine.Id,
                                    ProductId = productRepository.Id,
                                };

                                var insertHandlingLineProduct_getId = await _handlingLineProductRepository.InsertAndGetIdAsync(newHandlingLineProduct);
                                if (insertHandlingLineProduct_getId <= 0)
                                {
                                    var error = "insertHandlingLineProductError";
                                    _logger.LogError($"Error in {nameof(MakitaCampaignsAppService)}: returned {nameof(insertHandlingLineProduct_getId)} variable is less than or equal to 0.");
                                    return error;
                                };
                            }
                        }
                    }
                }
            }
            return true;
        }

        public bool IsTenantMakita()
        {
            var isTenantMakita = AbpSession.TenantId;

            return (isTenantMakita == TenantHelper.MakitaLive || isTenantMakita == TenantHelper.MakitaTest);
        }
    }
}
