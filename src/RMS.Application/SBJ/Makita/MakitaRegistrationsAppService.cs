using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Abp.Runtime.Session;
using Abp.Domain.Repositories;
using RMS.SBJ.CampaignProcesses;
using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.Makita.Dtos;
using RMS.SBJ.Products;
using RMS.SBJ.PurchaseRegistrationFieldDatas;
using RMS.SBJ.PurchaseRegistrationFields;
using RMS.SBJ.PurchaseRegistrations;
using RMS.SBJ.Registrations;
using RMS.SBJ.RetailerLocations;
using RMS.SBJ.Retailers;
using RMS.SBJ.HandlingLines;
using RMS.SBJ.ProductHandlings;
using RMS.SBJ.HandlingLineProducts;
using RMS.SBJ.RegistrationFields;
using RMS.SBJ.ProductGifts;
using RMS.SBJ.RegistrationFormFieldDatas;
using RMS.SBJ.MakitaBaseModelSerial;

namespace RMS.SBJ.Makita
{
    public class MakitaRegistrationsAppService : RMSAppServiceBase, IMakitaRegistrationsAppService
    {
        private const string MakitaEmail = MakitaConsts.MakitaEmail;
        private const string MakitaPassword = MakitaConsts.MakitaPassword;

        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly ILogger<MakitaRegistrationsAppService> _logger;
        private readonly IRepository<Registration, long> _lookup_registrationRepository;
        private readonly IRepository<PurchaseRegistration, long> _lookup_purchaseRegistrationRepository;
        private readonly IRepository<Country, long> _lookup_countryRepositoryRepository;
        private readonly IRepository<RetailerLocation, long> _lookup_retailerLocationRepository;
        private readonly IRepository<Retailer, long> _lookup_retailerRepository;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IRepository<RegistrationStatus, long> _lookup_registrationStatusRepository;
        private readonly IRepository<Campaign, long> _lookup_campaignRepository;
        private readonly IRepository<RegistrationJsonData.RegistrationJsonData, long> _lookup_registrationJsonDataRepository;
        private readonly IRepository<PurchaseRegistrationField, long> _lookup_purchaseRegistrationFieldRepository;
        private readonly IRepository<PurchaseRegistrationFieldData, long> _lookup_purchaseRegistrationFieldDataRepository;
        private readonly IRepository<RegistrationField, long> _lookup_registrationFormFieldRepository;
        private readonly IRepository<RegistrationFormFieldDatas.RegistrationFieldData, long> _lookup_registrationFormFieldDataRepository;
        private readonly IRepository<MakitaSerialNumber.MakitaSerialNumber, long> _lookup_makitaSerialNumberRepository;
        private readonly IRepository<RegistrationHistory.RegistrationHistory, long> _lookup_registrationHistoryRepository;
        private readonly IRepository<RejectionReason, long> _lookup_rejectionReasonRepository;
        private readonly IRepository<CampaignForm, long> _lookup_campaignFormRepository;
        private readonly IRepository<HandlingLine, long> _handlingLineRepository;
        private readonly IRepository<ProductHandling, long> _productHandlingRepository;
        private readonly IRepository<HandlingLineProduct, long> _handlingLineProductRepository;
        private readonly IRepository<ProductGift, long> _productGiftRepository;
        private readonly IRepository<MakitaBaseModelSerial.MakitaBaseModelSerial, long> _makitaBaseModelSerialRepository;

        public MakitaRegistrationsAppService(
            ILogger<MakitaRegistrationsAppService> logger,
            IRepository<Registration, long> lookup_registrationRepository,
            IRepository<PurchaseRegistration, long> lookup_purchaseRegistrationRepository,
            IRepository<Country, long> lookup_countryRepositoryRepository,
            IRepository<RetailerLocation, long> lookup_retailerLocationRepository,
            IRepository<Retailer, long> lookup_retailerRepository,
            IRepository<Product, long> lookup_productRepository,
            IRepository<RegistrationStatus, long> lookup_registrationStatusRepository,
            IRepository<Campaign, long> lookup_campaignRepository,
            IRepository<RegistrationJsonData.RegistrationJsonData, long> lookup_registrationJsonDataRepository,
            IRepository<PurchaseRegistrationField, long> lookup_purchaseRegistrationFieldRepository,
            IRepository<PurchaseRegistrationFieldData, long> lookup_purchaseRegistrationFieldDataRepository,
            IRepository<RegistrationField, long> lookup_registrationFormFieldRepository,
            IRepository<RegistrationFormFieldDatas.RegistrationFieldData, long> lookup_registrationFormFieldDataRepository,
            IRepository<MakitaSerialNumber.MakitaSerialNumber, long> lookup_makitaSerialNumberRepository,
            IRepository<RegistrationHistory.RegistrationHistory, long> lookup_registrationHistoryRepository,
            IRepository<RejectionReason, long> lookup_rejectionReasonRepository,
            IRepository<CampaignForm, long> lookup_campaignFormRepository,
            IRepository<HandlingLine, long> handlingLineRepository,
            IRepository<ProductHandling, long> productHandlingRepository,
            IRepository<HandlingLineProduct, long> handlingLineProductRepository,
            IRepository<ProductGift, long> productGiftRepository,
            IRepository<MakitaBaseModelSerial.MakitaBaseModelSerial, long> makitaBaseModelSerialRepository
            )
        {
            _logger = logger;
            _lookup_registrationRepository = lookup_registrationRepository;
            _lookup_purchaseRegistrationRepository = lookup_purchaseRegistrationRepository;
            _lookup_countryRepositoryRepository = lookup_countryRepositoryRepository;
            _lookup_retailerLocationRepository = lookup_retailerLocationRepository;
            _lookup_retailerRepository = lookup_retailerRepository;
            _lookup_productRepository = lookup_productRepository;
            _lookup_registrationStatusRepository = lookup_registrationStatusRepository;
            _lookup_campaignRepository = lookup_campaignRepository;
            _lookup_registrationJsonDataRepository = lookup_registrationJsonDataRepository;
            _lookup_purchaseRegistrationFieldRepository = lookup_purchaseRegistrationFieldRepository;
            _lookup_purchaseRegistrationFieldDataRepository = lookup_purchaseRegistrationFieldDataRepository;
            _lookup_registrationFormFieldRepository = lookup_registrationFormFieldRepository;
            _lookup_registrationFormFieldDataRepository = lookup_registrationFormFieldDataRepository;
            _lookup_makitaSerialNumberRepository = lookup_makitaSerialNumberRepository;
            _lookup_registrationHistoryRepository = lookup_registrationHistoryRepository;
            _lookup_rejectionReasonRepository = lookup_rejectionReasonRepository;
            _lookup_campaignFormRepository = lookup_campaignFormRepository;
            _handlingLineRepository = handlingLineRepository;
            _productHandlingRepository = productHandlingRepository;
            _handlingLineProductRepository = handlingLineProductRepository;
            _productGiftRepository = productGiftRepository;
            _makitaBaseModelSerialRepository = makitaBaseModelSerialRepository;
        }

        public async Task<object> Create(string blobStorage, string blobContainer, MakitaRegistrationsDto makitaRegistrationsModel)
        {
            int tenantId = AbpSession.GetTenantId();

            var values = new Dictionary<string, string>
            {
                { "email", MakitaEmail },
                { "password", MakitaPassword }
            };
            var content = new FormUrlEncodedContent(values);

            HttpResponseMessage response = await _httpClient.PostAsync($"https://actie.makita.nl/api/login", content);
            if (response == null)
            {
                _logger.LogError($"Error in {nameof(MakitaRegistrationsAppService)}: response of type {nameof(HttpResponseMessage)} is null.");
            }

            string responseString = await response.Content.ReadAsStringAsync();
            dynamic decodedToken = JObject.Parse(responseString);
            var makitaToken = decodedToken.token;

            var campaign = await _lookup_campaignRepository.GetAll().FirstOrDefaultAsync(fl => fl.ExternalCode == makitaRegistrationsModel.ActionCode);
            if (campaign == null)
            {
                var error = "campaignError";

                _logger.LogError($"Error in {nameof(MakitaRegistrationsAppService)}: returned {nameof(campaign)} variable is of value null.");

                return error;
            };

            var campaignHandlingLines =
                from o in _handlingLineRepository.GetAll()
                join o1 in _productHandlingRepository.GetAll() on o.ProductHandlingId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                where s1.CampaignId == campaign.Id
                select o;

            var campaignForm = await _lookup_campaignFormRepository.GetAll().FirstOrDefaultAsync(fl => fl.CampaignId == campaign.Id);
            if (campaignForm == null)
            {
                var error = "campaignFormError";

                _logger.LogError($"Error in {nameof(MakitaRegistrationsAppService)}: returned {nameof(campaignForm)} variable is of value null.");

                return error;
            }

            var country = await _lookup_countryRepositoryRepository.GetAll().FirstOrDefaultAsync(fl => fl.CountryCode == makitaRegistrationsModel.Country.Trim().ToUpper());
            if (country == null)
            {
                var error = "countryError";

                _logger.LogError($"Error in {nameof(MakitaRegistrationsAppService)}: returned {nameof(country)} variable is of value null.");

                return error;
            };

            var productRegistrationList = makitaRegistrationsModel.ProductRegistrations.ToList();

            foreach (var productRegistration in productRegistrationList)
            {
                var products = await _lookup_productRepository.GetAll().FirstOrDefaultAsync(fl => fl.ProductCode.Trim().ToUpper() == productRegistration.Model.Trim().ToUpper());
                if (products == null)
                {
                    var error = "productError";

                    _logger.LogError($"Error in {nameof(MakitaRegistrationsAppService)}: returned {nameof(products)} variable is of value null.");

                    return error;
                };

                var retailerLocation = await _lookup_retailerLocationRepository.GetAll().FirstOrDefaultAsync(fl => fl.ExternalCode == productRegistration.StorePurchased);
                if (retailerLocation == null)
                {
                    var error = "retailerLocationError";

                    _logger.LogError($"Error in {nameof(MakitaRegistrationsAppService)}: returned {nameof(retailerLocation)} variable is of value null.");

                    return error;
                }
            }

            var registrationMapper = new Registration
            {
                CompanyName = makitaRegistrationsModel.CompanyName,
                Gender = makitaRegistrationsModel.Gender,
                FirstName = makitaRegistrationsModel.FirstName,
                LastName = makitaRegistrationsModel.LastName,
                PostalCode = makitaRegistrationsModel.ZipCode,
                Street = makitaRegistrationsModel.StreetName,
                HouseNr = makitaRegistrationsModel.HouseNumber,
                City = makitaRegistrationsModel.Residence,
                CountryId = country.Id,
                EmailAddress = makitaRegistrationsModel.EmailAddress,
                PhoneNumber = makitaRegistrationsModel.PhoneNumber,
                CampaignId = campaign.Id,
                CampaignFormId = campaignForm.Id,
                LocaleId = 1,
                RegistrationStatusId = 1,
                TenantId = tenantId,
            };

            var registrationId = await _lookup_registrationRepository.InsertAndGetIdAsync(registrationMapper);
            if (registrationId <= 0)
            {
                var error = "registrationInsertError";

                _logger.LogError($"Error in {nameof(MakitaRegistrationsAppService)}: returned {nameof(registrationId)} variable is less than or equal to 0.");

                return error;
            };

            await InsertRegistrationFieldData(tenantId, makitaRegistrationsModel.LegalForm, 2, registrationId);
            await InsertRegistrationFieldData(tenantId, makitaRegistrationsModel.BusinessNumber, 3, registrationId);
            await InsertRegistrationFieldData(tenantId, makitaRegistrationsModel.VatNumber, 4, registrationId);
            await InsertRegistrationFieldData(tenantId, string.Empty, 5, registrationId);
            await InsertRegistrationFieldData(tenantId, makitaRegistrationsModel.Contact_id, 6, registrationId);
            await InsertRegistrationFieldData(tenantId, makitaRegistrationsModel.Debtor_number, 7, registrationId);
            await InsertRegistrationFieldData(tenantId, makitaRegistrationsModel.CampaignId, 8, registrationId);

            var productRegistrationsDto = new List<MakitaProductRegistrationsDto>();
            var productRegistrationCount = productRegistrationList.Count();
            var productRegistrationAccepted = 0;

            foreach (var product in productRegistrationList)
            {
                var productChosen = await _lookup_productRepository.GetAll().FirstOrDefaultAsync(fl => fl.ProductCode.Trim() == product.Model.Trim());
                var retailerLocation = await _lookup_retailerLocationRepository.GetAll().FirstOrDefaultAsync(fl => fl.ExternalCode == product.StorePurchased);
                var productGift = await _productGiftRepository.GetAll().Where(pg => pg.CampaignId == campaign.Id && pg.ProductCode.Trim() == productChosen.ProductCode.Trim() && pg.GiftId == Convert.ToInt64(product.Gift_id)).FirstOrDefaultAsync();
                long? handlingLineId;
                if (productGift == null || productGift.TotalPoints == 0)
                {
                    handlingLineId = (from o in _handlingLineProductRepository.GetAll()
                                      join o1 in campaignHandlingLines on o.HandlingLineId equals o1.Id into j1
                                      from s1 in j1
                                      where o.ProductId == productChosen.Id
                                      select s1.Id).FirstOrDefault();

                    if (handlingLineId == null || handlingLineId == 0)
                    {
                        if (campaignHandlingLines.Where(l => l.CustomerCode.ToUpper().Trim() != "UNKNOWN" && l.CustomerCode.ToUpper().Trim() != "POINTS").Count() == 1)
                        {
                            //only 1 Premium is linked to this campaign, so take that one
                            handlingLineId = campaignHandlingLines.Where(l => l.CustomerCode.ToUpper().Trim() != "UNKNOWN" && l.CustomerCode.ToUpper().Trim() != "POINTS").First().Id;
                        }
                        else
                        {
                            //more than 1 Premiums are linked to this campaign, so link it to the UNKNOWN
                            handlingLineId = campaignHandlingLines.Where(l => l.CustomerCode.ToUpper().Trim() == "UNKNOWN" && l.CustomerCode.ToUpper().Trim() != "POINTS").First().Id;
                        }
                    }
                }
                else
                {
                    handlingLineId = campaignHandlingLines.Where(hl => hl.CustomerCode.ToUpper().Trim() == "POINTS").First().Id;
                }

                var purchaseRegistrationMapper = new PurchaseRegistration
                {
                    PurchaseDate = Convert.ToDateTime(product.PurchaseDate),
                    RegistrationId = registrationId,
                    RetailerLocationId = retailerLocation?.Id ?? 11,
                    ProductId = productChosen.Id,
                    Quantity = 1,
                    TotalAmount = 0.00M,
                    HandlingLineId = handlingLineId.Value,
                    TenantId = tenantId,
                };

                var purchaseRegistrationId = await _lookup_purchaseRegistrationRepository.InsertAndGetIdAsync(purchaseRegistrationMapper);
                if (purchaseRegistrationId <= 0)
                {
                    var error = "purchaseRegistrationError";

                    _logger.LogError($"Error in {nameof(MakitaRegistrationsAppService)}: returned {nameof(purchaseRegistrationId)} variable is less than or equal to 0.");

                    return error;
                };

                await InsertPurchaseRegistrationFieldData(tenantId, product.SerialNumber, purchaseRegistrationId, 3);
                await InsertPurchaseRegistrationFieldData(tenantId, product.Gift_id, purchaseRegistrationId, 5);

                var blobServiceClient = new BlobServiceClient(blobStorage);
                BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainer);
                var invoiceImageDownload = string.Empty;
                var serialNumberImageDownload = string.Empty;
                var webClient = new WebClient();

                if (string.IsNullOrEmpty(product.InvoiceImagePath))
                {
                    purchaseRegistrationMapper.InvoiceImage = string.Empty;
                    purchaseRegistrationMapper.InvoiceImagePath = string.Empty;
                }
                else
                {
                    invoiceImageDownload = $"{product.InvoiceImagePath}?token={makitaToken}";

                    byte[] invoiceImageByte = webClient.DownloadData(invoiceImageDownload);
                    string contentType = webClient.ResponseHeaders["Content-Type"];
                    string contentExtension = contentType.Split("/")[1];
                    var invoiceBlobPath = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{registrationId}-{Guid.NewGuid()}.{contentExtension}";

                    blobContainerClient.UploadBlob(invoiceBlobPath, new MemoryStream(invoiceImageByte));
                    purchaseRegistrationMapper.InvoiceImagePath = invoiceBlobPath;
                }

                if (!string.IsNullOrEmpty(product.SerialNumberImagePath))
                {
                    serialNumberImageDownload = $"{product.SerialNumberImagePath}?token={makitaToken}";

                    byte[] serialNumberImageByte = webClient.DownloadData(serialNumberImageDownload);
                    string contentType = webClient.ResponseHeaders["Content-Type"];
                    string contentExtension = contentType.Split("/")[1];
                    var serialBlobPath = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{registrationId}-{Guid.NewGuid()}.{contentExtension}";

                    blobContainerClient.UploadBlob(serialBlobPath, new MemoryStream(serialNumberImageByte));
                    await InsertPurchaseRegistrationFieldData(tenantId, serialBlobPath, purchaseRegistrationId, 4);
                }
                else
                {
                    await InsertPurchaseRegistrationFieldData(tenantId, string.Empty, purchaseRegistrationId, 4);
                }

                var makitaSerialNumber = Convert.ToInt64(product.SerialNumber);
                var makitaProductModel = product.Model;
                var makitaBaseModelSerials = _makitaBaseModelSerialRepository.GetAll().FirstOrDefault(sn => makitaProductModel.Trim().Contains(sn.BasisModel) && makitaSerialNumber >= sn.SerialNumberFrom && makitaSerialNumber <= sn.SerialNumberTo);          

                if (makitaBaseModelSerials != null)
                {
                    productRegistrationAccepted += 1;
                }
            }

            var remarks = string.Empty;
            if (productRegistrationAccepted == productRegistrationCount)
            {
                registrationMapper.RegistrationStatusId = 9;
                remarks = "Requested Invoice Check";
            };

            await InsertRegistrationHistory(tenantId, registrationId, registrationMapper.RegistrationStatusId, DateTime.Now, 1, remarks);

            string registrationJsonDataString = JsonConvert.SerializeObject(makitaRegistrationsModel);
            var registrationJsonData = new RegistrationJsonData.RegistrationJsonData
            {
                Data = registrationJsonDataString,
                DateCreated = DateTime.Now,
                TenantId = tenantId,
                RegistrationId = registrationId
            };
            var registrationJsonDataId = _lookup_registrationJsonDataRepository.InsertAndGetId(registrationJsonData);

            return registrationId;
        }

        public async Task<object> Update(string blobStorage, string blobContainer, MakitaRegistrationsDto makitaRegistrationsModel)
        {
            var tenantId = AbpSession.GetTenantId();
            var country = await _lookup_countryRepositoryRepository.GetAll()
                .Where(fl => fl.CountryCode == makitaRegistrationsModel.Country.ToUpper().Trim()).FirstOrDefaultAsync();
            var campaign = await _lookup_campaignRepository.GetAll()
                .Where(fl => fl.ExternalCode == makitaRegistrationsModel.ActionCode).FirstOrDefaultAsync();
            var registration = await _lookup_registrationRepository.GetAll()
                .Where(fl => fl.Id == makitaRegistrationsModel.RegistrationId).FirstOrDefaultAsync();
            var currentLegalFormFieldData = await _lookup_registrationFormFieldDataRepository.GetAll()
                .Where(fl => fl.RegistrationId == makitaRegistrationsModel.RegistrationId && fl.RegistrationFieldId == 2).FirstOrDefaultAsync();
            var currentBusinessNumberFieldData = await _lookup_registrationFormFieldDataRepository.GetAll()
                .Where(fl => fl.RegistrationId == makitaRegistrationsModel.RegistrationId && fl.RegistrationFieldId == 3).FirstOrDefaultAsync();
            var currentVatNumberFieldData = await _lookup_registrationFormFieldDataRepository.GetAll()
                .Where(fl => fl.RegistrationId == makitaRegistrationsModel.RegistrationId && fl.RegistrationFieldId == 4).FirstOrDefaultAsync();

            var currentContact_IdFieldData = await _lookup_registrationFormFieldDataRepository.GetAll()
                .Where(fl => fl.RegistrationId == makitaRegistrationsModel.RegistrationId && fl.RegistrationFieldId == 6).FirstOrDefaultAsync();

            var registrationStatus_Incomplete = await _lookup_registrationStatusRepository.GetAll()
                .Where(fl => fl.Description == "Incomplete").FirstOrDefaultAsync();

            if(registration.RegistrationStatusId != registrationStatus_Incomplete.Id) {
                return null;
            }

            if (currentContact_IdFieldData == null)
            {
                await _lookup_registrationFormFieldDataRepository.InsertAndGetIdAsync(new RegistrationFieldData
                {
                    TenantId = tenantId,
                    RegistrationId = registration.Id,
                    RegistrationFieldId = 6,
                    Value = makitaRegistrationsModel.Contact_id,
                });
            }

            var currentDebtor_numberFieldData = await _lookup_registrationFormFieldDataRepository.GetAll()
                .Where(fl => fl.RegistrationId == makitaRegistrationsModel.RegistrationId && fl.RegistrationFieldId == 7).FirstOrDefaultAsync();
            if (currentDebtor_numberFieldData == null)
            {
                await _lookup_registrationFormFieldDataRepository.InsertAsync(new RegistrationFieldData
                {
                    TenantId = tenantId,
                    RegistrationId = registration.Id,
                    RegistrationFieldId = 7,
                    Value = makitaRegistrationsModel.Debtor_number,
                });
            }

            var currentCampaign_Id_MakitaFieldData = await _lookup_registrationFormFieldDataRepository.GetAll()
                .Where(fl => fl.RegistrationId == makitaRegistrationsModel.RegistrationId && fl.RegistrationFieldId == 8).FirstOrDefaultAsync();
            if (currentCampaign_Id_MakitaFieldData == null)
            {
                await _lookup_registrationFormFieldDataRepository.InsertAsync(new RegistrationFieldData
                {
                    TenantId = tenantId,
                    RegistrationId = registration.Id,
                    RegistrationFieldId = 8,
                    Value = makitaRegistrationsModel.CampaignId,
                });
            }

            var values = new Dictionary<string, string>
            {
                { "email", MakitaEmail },
                { "password", MakitaPassword }
            };
            var content = new FormUrlEncodedContent(values);
            var response = await _httpClient.PostAsync($"https://actie.makita.nl/api/login", content);
            var responseString = await response.Content.ReadAsStringAsync();
            dynamic decodedToken = JObject.Parse(responseString);
            var makitaToken = decodedToken.token;

            registration.CompanyName = makitaRegistrationsModel.CompanyName;
            registration.Gender = makitaRegistrationsModel.Gender;
            registration.FirstName = makitaRegistrationsModel.FirstName;
            registration.LastName = makitaRegistrationsModel.LastName;
            registration.PostalCode = makitaRegistrationsModel.ZipCode;
            registration.Street = makitaRegistrationsModel.StreetName;
            registration.HouseNr = makitaRegistrationsModel.HouseNumber;
            registration.City = makitaRegistrationsModel.Residence;
            registration.CountryId = country.Id;
            registration.EmailAddress = makitaRegistrationsModel.EmailAddress;
            registration.PhoneNumber = makitaRegistrationsModel.PhoneNumber;
            registration.CampaignId = campaign.Id;
            registration.IncompleteFields = string.Empty;
            currentLegalFormFieldData.Value = makitaRegistrationsModel.LegalForm;
            currentBusinessNumberFieldData.Value = makitaRegistrationsModel.BusinessNumber;
            currentVatNumberFieldData.Value = makitaRegistrationsModel.VatNumber;

            if (currentContact_IdFieldData != null)
            {
                currentContact_IdFieldData.Value = makitaRegistrationsModel.Contact_id;
            };

            if (currentDebtor_numberFieldData != null)
            {
                currentDebtor_numberFieldData.Value = makitaRegistrationsModel.Debtor_number;
            }

            if (currentCampaign_Id_MakitaFieldData != null)
            {
                currentCampaign_Id_MakitaFieldData.Value = makitaRegistrationsModel.CampaignId;
            }

            var purchaseRegistrationList = await _lookup_purchaseRegistrationRepository.GetAll()
                .Where(fl => fl.RegistrationId == makitaRegistrationsModel.RegistrationId).ToListAsync();
            var IncomingproductRegistrationList = makitaRegistrationsModel.ProductRegistrations.ToList();

            var productRegistrationCount = IncomingproductRegistrationList.Count();
            var productRegistrationAccepted = 0;

            for (int i = 0; i < purchaseRegistrationList.Count; ++i)
            {
                var currentPurchase = purchaseRegistrationList[i];
                var incomingProductRegistration = IncomingproductRegistrationList[i];
                var registrationId = registration.Id;

                var currentSerialImagePath = await _lookup_purchaseRegistrationFieldDataRepository.GetAll()
                    .Where(fl => fl.PurchaseRegistrationId == currentPurchase.Id && fl.PurchaseRegistrationFieldId == 4).FirstOrDefaultAsync();

                BlobServiceClient blobServiceClient = new BlobServiceClient(blobStorage);
                BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainer);

                var invoiceImageDownload = string.Empty;
                var serialNumberImageDownload = string.Empty;
                var webClient = new WebClient();

                if (!string.IsNullOrWhiteSpace(incomingProductRegistration.InvoiceImagePath))
                {
                    invoiceImageDownload = incomingProductRegistration.InvoiceImagePath + "?token=" + makitaToken;
                    byte[] invoiceImageByte = webClient.DownloadData(invoiceImageDownload);
                    var contentType = webClient.ResponseHeaders["Content-Type"];
                    var contentExtension = contentType.Split("/")[1];
                    var invoiceBlobPath = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{registrationId}-{Guid.NewGuid()}.{contentExtension}";
                    blobContainerClient.UploadBlob(invoiceBlobPath, new MemoryStream(invoiceImageByte));
                    currentPurchase.InvoiceImagePath = invoiceBlobPath;
                };

                if (!string.IsNullOrWhiteSpace(incomingProductRegistration.SerialNumberImagePath))
                {
                    serialNumberImageDownload = incomingProductRegistration.SerialNumberImagePath + "?token=" + makitaToken;
                    byte[] serialNumberImageByte = webClient.DownloadData(serialNumberImageDownload);
                    var contentType = webClient.ResponseHeaders["Content-Type"];
                    var contentExtension = contentType.Split("/")[1];
                    var serialBlobPath = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{registrationId}-{Guid.NewGuid()}.{contentExtension}";
                    blobContainerClient.UploadBlob(serialBlobPath, new MemoryStream(serialNumberImageByte));
                    currentSerialImagePath.Value = serialBlobPath;
                };

                var retailerLocation = await _lookup_retailerLocationRepository.GetAll()
                    .Where(fl => fl.ExternalCode == incomingProductRegistration.StorePurchased).FirstOrDefaultAsync();
                var retailerLocationId = 11;
                if (retailerLocation != null)
                {
                    retailerLocationId = (int)retailerLocation.Id;

                };

                currentPurchase.RetailerLocationId = retailerLocationId;
                currentPurchase.PurchaseDate = Convert.ToDateTime(incomingProductRegistration.PurchaseDate);

                var serialNumbers = await _lookup_purchaseRegistrationFieldDataRepository.GetAll()
                   .Where(fl => fl.PurchaseRegistrationId == currentPurchase.Id && fl.PurchaseRegistrationFieldId == 3).FirstOrDefaultAsync();

                if (incomingProductRegistration.SerialNumber != null)
                {
                    serialNumbers.Value = incomingProductRegistration.SerialNumber;
                    var serialNumberCheck = await _makitaBaseModelSerialRepository.GetAll().Where(sn => incomingProductRegistration.Model.Trim().Contains(sn.BasisModel) && Convert.ToInt32(incomingProductRegistration.SerialNumber.Trim()) >= sn.SerialNumberFrom && Convert.ToInt32(incomingProductRegistration.SerialNumber.Trim()) <= sn.SerialNumberTo).FirstOrDefaultAsync();
                    if (serialNumberCheck != null) productRegistrationAccepted += 1;
                }

                bool reviseHandlingLineId = false;

                var incomingProduct = await _lookup_productRepository.GetAll()
                    .Where(fl => fl.ProductCode.Trim().ToUpper() == incomingProductRegistration.Model.Trim().ToUpper()).FirstOrDefaultAsync();

                if (currentPurchase.ProductId != incomingProduct.Id)
                {
                    currentPurchase.ProductId = incomingProduct.Id;
                    reviseHandlingLineId = true;
                }

                var currentGiftId = await _lookup_purchaseRegistrationFieldDataRepository.GetAll()
                    .Where(fl => fl.PurchaseRegistrationId == currentPurchase.Id && fl.PurchaseRegistrationFieldId == 5).FirstOrDefaultAsync();

                if (currentGiftId == null)
                {
                    await _lookup_purchaseRegistrationFieldDataRepository.InsertAndGetIdAsync(new PurchaseRegistrationFieldData
                    {
                        TenantId = tenantId,
                        PurchaseRegistrationId = currentPurchase.Id,
                        PurchaseRegistrationFieldId = 5,
                        Value = incomingProductRegistration.Gift_id,
                    });
                    reviseHandlingLineId = true;
                }
                else
                {
                    if (currentGiftId.Value != incomingProductRegistration.Gift_id)
                    {
                        currentGiftId.Value = incomingProductRegistration.Gift_id;
                        reviseHandlingLineId = true;
                    }
                }

                if (reviseHandlingLineId)
                {
                    var campaignHandlingLines =
                        from o in _handlingLineRepository.GetAll()
                        join o1 in _productHandlingRepository.GetAll() on o.ProductHandlingId equals o1.Id into j1
                        from s1 in j1.DefaultIfEmpty()
                        where s1.CampaignId == campaign.Id && o.CustomerCode != "UNKNOWN"
                        select o;

                    var productGift = await _productGiftRepository.GetAll().Where(pg => pg.CampaignId == campaign.Id && pg.ProductCode.Trim() == incomingProduct.ProductCode.Trim() && pg.GiftId == Convert.ToInt64(incomingProductRegistration.Gift_id)).FirstOrDefaultAsync();
                    long? handlingLineId;
                    if (productGift == null || (productGift != null && productGift.TotalPoints == 0))
                    {
                        handlingLineId = (from o in _handlingLineProductRepository.GetAll()
                                          join o1 in campaignHandlingLines on o.HandlingLineId equals o1.Id into j1
                                          from s1 in j1
                                          where o.ProductId == incomingProduct.Id
                                          select s1.Id).FirstOrDefault();
                    }
                    else
                    {
                        handlingLineId = campaignHandlingLines.Where(hl => hl.CustomerCode.ToUpper().Trim() == "POINTS").First().Id;
                    }

                    if (handlingLineId.HasValue) //...and if not, just leave the currentPurchase.handlingLineId as it currently is in the database
                    {
                        currentPurchase.HandlingLineId = handlingLineId.Value;
                    }
                }
            }

            var registrationStatusId = 1;
            var remarks = string.Empty;

            if (productRegistrationAccepted == productRegistrationCount)
            {
                registrationStatusId = 9;
                remarks = "Requested Invoice Check";
            };

            registration.RegistrationStatusId = registrationStatusId;

            await InsertRegistrationHistory(tenantId, makitaRegistrationsModel.RegistrationId, registrationStatusId, DateTime.Now, 1, remarks);

            return null;
        }

        private async Task<bool> InsertRegistrationFieldData(int tenantId, string value, long registrationFieldId, long registrationId)
        {
            try
            {
                await _lookup_registrationFormFieldDataRepository.InsertAsync(new RegistrationFormFieldDatas.RegistrationFieldData
                {
                    TenantId = tenantId,
                    Value = value,
                    RegistrationFieldId = registrationFieldId,
                    RegistrationId = registrationId
                });

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(InsertRegistrationFieldData)}: {ex.Message}. StackTrace: {ex.StackTrace}");

                return false;
            }
        }

        private async Task<bool> InsertPurchaseRegistrationFieldData(int tenantId, string value, long purchaseRegistrationId, long purchaseRegistrationFieldId)
        {
            try
            {
                await _lookup_purchaseRegistrationFieldDataRepository.InsertAsync(new PurchaseRegistrationFieldData
                {
                    TenantId = tenantId,
                    Value = value,
                    PurchaseRegistrationId = purchaseRegistrationId,
                    PurchaseRegistrationFieldId = purchaseRegistrationFieldId,
                });

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(InsertPurchaseRegistrationFieldData)}: {ex.Message}. StackTrace: {ex.StackTrace}");

                return false;
            }
        }

        private async Task<bool> InsertRegistrationHistory(int tenantId, long registrationId, long registrationStatusId, DateTime dateCreated, long abpUserId, string remarks)
        {
            try
            {
                await _lookup_registrationHistoryRepository.InsertAsync(new RegistrationHistory.RegistrationHistory
                {
                    TenantId = tenantId,
                    RegistrationId = registrationId,
                    RegistrationStatusId = registrationStatusId,
                    DateCreated = dateCreated,
                    AbpUserId = abpUserId,
                    Remarks = remarks,
                });

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(InsertRegistrationHistory)}: {ex.Message}. StackTrace: {ex.StackTrace}");

                return false;
            }
        }

        public async Task<object> GetRegistrationStatus(long registrationId)
        {
            var registrationRecord = await _lookup_registrationRepository.GetAll().FirstOrDefaultAsync(fl => fl.Id == registrationId);
            var registrationStatusRecord = await _lookup_registrationStatusRepository.GetAll().FirstOrDefaultAsync(fl => fl.Id == registrationRecord.RegistrationStatusId);

            if (registrationStatusRecord == null)
            {
                _logger.LogError($"Error in {nameof(GetRegistrationStatus)}: could not retrieve RegistrationStatus record.");
            }

            var registrationStatusCode = registrationStatusRecord.StatusCode;
            var registrationStatusDescription = registrationStatusRecord.Description;
            var rejectionReason = await _lookup_rejectionReasonRepository.GetAll().FirstOrDefaultAsync(fl => fl.Id == registrationRecord.RejectionReasonId);

            if (!String.IsNullOrWhiteSpace(registrationRecord.IncompleteFields))
            {
                if (rejectionReason != null)
                {
                    var serializedJson = new
                    {
                        registrationStatusCode = registrationStatusCode,
                        registrationStatusDescription = registrationStatusDescription,
                        incompleteFields = registrationRecord.IncompleteFields,
                        extraInfo = rejectionReason.Description
                    };

                    return serializedJson;
                }
                else
                {
                    var serializedJson = new
                    {
                        registrationStatusCode = registrationStatusCode,
                        registrationStatusDescription = registrationStatusDescription,
                        incompleteFields = registrationRecord.IncompleteFields
                    };

                    return serializedJson;
                }
            }
            else if (!String.IsNullOrWhiteSpace(registrationRecord.RejectedFields))
            {
                if (rejectionReason != null)
                {
                    var serializedJson = new
                    {
                        registrationStatusCode = registrationStatusCode,
                        registrationStatusDescription = registrationStatusDescription,
                        rejectedFields = registrationRecord.RejectedFields,
                        extraInfo = rejectionReason.Description
                    };

                    return serializedJson;
                }
                else
                {
                    var serializedJson = new
                    {
                        registrationStatusCode = registrationStatusCode,
                        registrationStatusDescription = registrationStatusDescription,
                        rejectedFields = registrationRecord.RejectedFields
                    };

                    return serializedJson;
                }
            }
            else
            {
                var serializedJson = new
                {
                    registrationStatusCode = registrationStatusCode,
                    registrationStatusDescription = registrationStatusDescription
                };

                return serializedJson;
            }


        }
    }
}