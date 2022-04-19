using Abp.Authorization;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using RMS.Authorization;
using RMS.Dto;
using RMS.MultiTenancy;
using RMS.SBJ.CampaignProcesses;
using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.HandlingBatch;
using RMS.SBJ.HandlingBatch.Helpers;
using RMS.SBJ.HandlingLines;
using RMS.SBJ.Products;
using RMS.SBJ.PurchaseRegistrationFieldDatas;
using RMS.SBJ.PurchaseRegistrationFields;
using RMS.SBJ.PurchaseRegistrations;
using RMS.SBJ.RegistrationFields;
using RMS.SBJ.RegistrationFormFieldDatas;
using RMS.SBJ.Registrations;
using RMS.SBJ.Report.GeneralReports.Dtos;
using RMS.SBJ.Report.GeneralReports.Exporting;
using RMS.SBJ.RetailerLocations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMS.SBJ.Report.GeneralReports
{
    [AbpAuthorize(AppPermissions.Pages_GeneralReport)]
    public class GeneralReportAppService : RMSAppServiceBase, IGeneralReportAppService
    {
        private readonly IRepository<Registration, long> _registrationRepository;
        private readonly IRepository<PurchaseRegistration, long> _purchaseRegistrationRepository;
        private readonly IGeneralReportExcelExporter _generalReportExcelExporter;
        private readonly IRepository<Locale, long> _localeRepository;
        private readonly IRepository<RegistrationHistory.RegistrationHistory, long> _registrationHistoryRepository;
        private readonly IRepository<Country, long> _countryRepository;
        private readonly IRepository<RegistrationStatus, long> _registrationStatusRepository;
        private readonly IRepository<RejectionReason, long> _rejectionReasonRepository;
        private readonly IRepository<Product, long> _productRepository;
        private readonly IRepository<RetailerLocation, long> _retailerLocationRepository;
        private readonly IRepository<HandlingLine, long> _handlingLineRepository;
        private readonly IRepository<HandlingBatchLine, long> _handlingBatchLineRepository;
        private readonly IRepository<HandlingBatchHistory, long> _handlingBatchHistoryRepository;
        private readonly IRepository<HandlingBatchStatus, long> _handlingBatchStatusRepository;
        private readonly IRepository<Campaign, long> _campaignRepository;
        private readonly IRepository<RegistrationFieldData, long> _registrationFieldDataRepository;
        private readonly IRepository<RegistrationField, long> _registrationFieldRepository;
        private readonly IRepository<PurchaseRegistrationFieldData, long> _purchaseRegistrationFieldDataRepository;
        private readonly IRepository<PurchaseRegistrationField, long> _purchaseRegistrationFieldRepository;
        private readonly IRepository<Tenant> _tenantRepository;


        public GeneralReportAppService(
            IRepository<Registration, long> registrationRepository,
            IRepository<PurchaseRegistration, long> purchaseRegistrationRepository,
            IGeneralReportExcelExporter generalReportExcelExporter,
            IRepository<Locale, long> localeRepository,
            IRepository<RegistrationHistory.RegistrationHistory, long> registrationHistoryRepository,
            IRepository<Country, long> countryRepository,
            IRepository<RegistrationStatus, long> registrationStatusRepository,
            IRepository<RejectionReason, long> rejectionReasonRepository,
            IRepository<Product, long> productRepository,
            IRepository<RetailerLocation, long> retailerLocationRepository,
            IRepository<HandlingLine, long> handlingLineRepository,
            IRepository<HandlingBatchLine, long> handlingBatchLineRepository,
            IRepository<HandlingBatchHistory, long> handlingBatchHistoryRepository,
            IRepository<HandlingBatchStatus, long> handlingBatchStatusRepository,
            IRepository<Campaign, long> campaignRepository,
            IRepository<RegistrationFieldData, long> registrationFieldDataRepository,
            IRepository<RegistrationField, long> registrationFieldRepository,
            IRepository<PurchaseRegistrationFieldData, long> purchaseRegistrationFieldDataRepository,
            IRepository<PurchaseRegistrationField, long> purchaseRegistrationFieldRepository,
            IRepository<Tenant> tenantRepository)
        {
            _registrationRepository = registrationRepository;
            _purchaseRegistrationRepository = purchaseRegistrationRepository;
            _generalReportExcelExporter = generalReportExcelExporter;
            _localeRepository = localeRepository;
            _registrationHistoryRepository = registrationHistoryRepository;
            _countryRepository = countryRepository;
            _registrationStatusRepository = registrationStatusRepository;
            _rejectionReasonRepository = rejectionReasonRepository;
            _productRepository = productRepository;
            _retailerLocationRepository = retailerLocationRepository;
            _handlingLineRepository = handlingLineRepository;
            _handlingBatchLineRepository = handlingBatchLineRepository;
            _handlingBatchHistoryRepository = handlingBatchHistoryRepository;
            _handlingBatchStatusRepository = handlingBatchStatusRepository;
            _campaignRepository = campaignRepository;
            _registrationFieldDataRepository = registrationFieldDataRepository;
            _registrationFieldRepository = registrationFieldRepository;
            _purchaseRegistrationFieldDataRepository = purchaseRegistrationFieldDataRepository;
            _purchaseRegistrationFieldRepository = purchaseRegistrationFieldRepository;
            _tenantRepository = tenantRepository;
        }

        public async Task<FileDto> GetGeneralReportToExcel(GetAllGeneralReportForExcelInput input)
        {
            var filteredRegistrations = (from r in _registrationRepository.GetAll()
                                         join o1 in _localeRepository.GetAll() on r.LocaleId equals o1.Id into j1
                                         from l in j1.DefaultIfEmpty()
                                         join o2 in _registrationStatusRepository.GetAll() on r.RegistrationStatusId equals o2.Id into j2
                                         from rs in j2.DefaultIfEmpty()
                                         join o3 in _countryRepository.GetAll() on r.CountryId equals o3.Id into j3
                                         from c in j3.DefaultIfEmpty()
                                         join o4 in _rejectionReasonRepository.GetAll() on r.RejectionReasonId equals o4.Id into j4
                                         from rr in j4.DefaultIfEmpty()
                                         join o5 in _registrationHistoryRepository.GetAll() on r.Id equals o5.RegistrationId into j5
                                         from rhfirst in j5.DefaultIfEmpty()
                                         join o6 in _registrationHistoryRepository.GetAll() on r.Id equals o6.RegistrationId into j6
                                         from rhcurrent in j6.DefaultIfEmpty()
                                         join o7 in _campaignRepository.GetAll() on r.CampaignId equals o7.Id into j7
                                         from cp in j7.DefaultIfEmpty()
                                         where r.CampaignId == input.CampaignFilter &&
                                            rhfirst.Id == (from rhf in _registrationHistoryRepository.GetAll()
                                                           where r.Id == rhf.RegistrationId
                                                           select rhf.Id).FirstOrDefault() &&
                                            rhfirst.DateCreated >= input.StartDateFilter &&
                                            rhfirst.DateCreated < input.EndDateFilter.AddDays(1) &&
                                            rhcurrent.Id == (from rhc in _registrationHistoryRepository.GetAll()
                                                             where r.Id == rhc.RegistrationId &&
                                                             r.RegistrationStatusId == rhc.RegistrationStatusId
                                                             orderby rhc.DateCreated descending
                                                             select rhc.Id).FirstOrDefault()
                                         select new
                                         {
                                             CampaignName = cp.Name,
                                             RegistrationId = r.Id,
                                             Locale = l.Description,
                                             RegistrationTime = rhfirst.DateCreated,
                                             CompanyName = r.CompanyName,
                                             Name = r.FirstName + " " + r.LastName,
                                             Gender = r.Gender,
                                             Street = r.Street,
                                             HouseNumber = r.HouseNr,
                                             PostalCode = r.PostalCode,
                                             City = r.City,
                                             Country = c.CountryCode,
                                             PhoneNumber = r.PhoneNumber,
                                             Email = r.EmailAddress,
                                             BicIban = r.Bic + " + " + r.Iban,
                                             CurrentStatus = rs.Description,
                                             CurrentStatusTime = rhcurrent.DateCreated,
                                             Remarks = rhcurrent.Remarks,
                                             RejectionReason = !rr.IsIncompleteReason ? rr.Description : null,
                                             RejectedFields = !rr.IsIncompleteReason ? r.RejectedFields : null,
                                             IncompleteReason = rr.IsIncompleteReason ? rr.Description : null,
                                             IncompleteFields = rr.IsIncompleteReason ? r.IncompleteFields : null
                                         });

            var partialGeneralReports = (from pr in _purchaseRegistrationRepository.GetAll()
                                             join o1 in _productRepository.GetAll() on pr.ProductId equals o1.Id into j1
                                             from p in j1.DefaultIfEmpty()
                                             join o2 in _retailerLocationRepository.GetAll() on pr.RetailerLocationId equals o2.Id into j2
                                             from rl in j2.DefaultIfEmpty()
                                             join o3 in _handlingLineRepository.GetAll() on pr.HandlingLineId equals o3.Id into j3
                                             from hl in j3.DefaultIfEmpty()
                                             join o4 in filteredRegistrations on pr.RegistrationId equals o4.RegistrationId into j4
                                             from r in j4
                                             select new
                                             {
                                                 CampaignName = r.CampaignName,
                                                 RegistrationId = r.RegistrationId,
                                                 Locale = r.Locale,
                                                 RegistrationTime = r.RegistrationTime,
                                                 CompanyName = r.CompanyName,
                                                 Name = r.Name,
                                                 Gender = r.Gender,
                                                 Street = r.Street,
                                                 HouseNumber = r.HouseNumber,
                                                 PostalCode = r.PostalCode,
                                                 City = r.City,
                                                 Country = r.Country,
                                                 PhoneNumber = r.PhoneNumber,
                                                 Email = r.Email,
                                                 BicIban = r.BicIban,
                                                 CurrentStatus = r.CurrentStatus,
                                                 CurrentStatusTime = r.CurrentStatusTime,
                                                 Remarks = r.Remarks,
                                                 RejectionReason = r.RejectionReason,
                                                 RejectedFields = r.RejectedFields,
                                                 IncompleteReason = r.IncompleteReason,
                                                 IncompleteFields = r.IncompleteFields,

                                                 PurchaseRegistrationId = pr.Id,
                                                 ProductPurchased = p.ProductCode,
                                                 Quantity = pr.Quantity,
                                                 PurchaseTime = pr.PurchaseDate,
                                                 StorePurchased = rl.Name,
                                                 AddressPurchased = rl.Address,
                                                 PostalPurchased = rl.PostalCode,
                                                 CityPurchased = rl.City,
                                                 PremiumDescription = hl.PremiumDescription,
                                                 ActivationCode = hl.ActivationCode,
                                                 CashRefund = hl.Amount
                                             });

            var filteredRegistrationFields = await (from rfd in _registrationFieldDataRepository.GetAll()
                                      join o1 in _registrationFieldRepository.GetAll() on rfd.RegistrationFieldId equals o1.Id into j1
                                      from rf in j1.DefaultIfEmpty()
                                      join o2 in filteredRegistrations on rfd.RegistrationId equals o2.RegistrationId into j2
                                      from r in j2
                                      select new
                                      {
                                          RegistrationId = r.RegistrationId,
                                          Description = rf.Description,
                                          Value = rfd.Value
                                      }).ToListAsync();

            var filteredPurchaseRegistrationFields = await (from prfd in _purchaseRegistrationFieldDataRepository.GetAll()
                                      join o1 in _purchaseRegistrationFieldRepository.GetAll() on prfd.PurchaseRegistrationFieldId equals o1.Id into j1
                                      from prf in j1.DefaultIfEmpty()
                                      join o2 in partialGeneralReports on prfd.PurchaseRegistrationId equals o2.PurchaseRegistrationId into j2
                                      from r in j2
                                      select new
                                      {
                                          PurchaseRegistrationId = r.PurchaseRegistrationId,
                                          Description = prf.Description,
                                          Value = prfd.Value
                                      }).ToListAsync();

            var filteredHandlingBatchInfos = await (from hbl in _handlingBatchLineRepository.GetAll()
                                                 join o1 in _handlingBatchHistoryRepository.GetAll() on hbl.HandlingBatchId equals o1.Id into j1
                                                 from hbh in j1.DefaultIfEmpty()
                                                 join o2 in _handlingBatchStatusRepository.GetAll() on hbh.HandlingBatchStatusId equals o2.Id into j2
                                                 from hbs in j2.DefaultIfEmpty()
                                                 join o3 in partialGeneralReports on hbl.PurchaseRegistrationId equals o3.PurchaseRegistrationId into j3
                                                 from r in j3.DefaultIfEmpty()
                                                 where hbs.StatusCode == HandlingBatchLineStatusCodeHelper.Finished
                                                 select new
                                                 {
                                                     RegistrationId = r.RegistrationId,
                                                     HandlingBatchId = hbl.HandlingBatchId,
                                                     HandlingBatchFinishedTime = hbh.DateCreated
                                                 }).ToListAsync();

            var _getGeneralReportDtoList = new List<GeneralReportDto>();

            foreach (var partialGeneralReport in await partialGeneralReports.ToListAsync())
            {
                var generalReport = new GeneralReportDto
                {
                    Id = partialGeneralReport.RegistrationId,
                    CampaignName = partialGeneralReport.CampaignName,
                    Locale = partialGeneralReport.Locale,
                    RegistrationTime = partialGeneralReport.RegistrationTime,
                    CompanyName = partialGeneralReport.CompanyName,
                    Name = partialGeneralReport.Name,
                    Gender = partialGeneralReport.Gender,
                    Street = partialGeneralReport.Street,
                    HouseNumber = partialGeneralReport.HouseNumber,
                    PostalCode = partialGeneralReport.PostalCode,
                    City = partialGeneralReport.City,
                    Country = partialGeneralReport.Country,
                    PhoneNumber = partialGeneralReport.PhoneNumber,
                    Email = partialGeneralReport.Email,
                    BicIban = partialGeneralReport.BicIban,
                    CurrentStatus = partialGeneralReport.CurrentStatus,
                    CurrentStatusTime = partialGeneralReport.CurrentStatusTime,
                    Remarks = partialGeneralReport.Remarks,
                    RejectionReason = partialGeneralReport.RejectionReason,
                    RejectedFields = partialGeneralReport.RejectedFields,
                    IncompleteReason = partialGeneralReport.IncompleteReason,
                    IncompleteFields = partialGeneralReport.IncompleteFields,
                    ProductPurchased = partialGeneralReport.ProductPurchased,
                    Quantity = partialGeneralReport.Quantity,
                    PurchaseTime = partialGeneralReport.PurchaseTime,
                    StorePurchased = partialGeneralReport.StorePurchased,
                    AddressPurchased = partialGeneralReport.AddressPurchased,
                    PostalPurchased = partialGeneralReport.PostalPurchased,
                    CityPurchased = partialGeneralReport.CityPurchased,
                    ProductSelected = partialGeneralReport.PremiumDescription,
                    ActivationcodeSelected = partialGeneralReport.ActivationCode,
                    CashRefund = partialGeneralReport.CashRefund
                };

                var customFieldDescriptions = filteredRegistrationFields.Select(e => e.Description).Distinct();
                var customFields = new List<CustomField>();
                var currentRegistrationCustomFields = filteredRegistrationFields.Where(e => e.RegistrationId == partialGeneralReport.RegistrationId);
                foreach (var customFieldDescription in customFieldDescriptions)
                {
                    var currentRegistrationCustomField = currentRegistrationCustomFields.FirstOrDefault(e => e.Description == customFieldDescription);
                    if(currentRegistrationCustomField != null)
                    {
                        customFields.Add(new CustomField
                        {
                            Description = customFieldDescription,
                            Value = currentRegistrationCustomField.Value
                        });
                    }
                    else
                    {
                        customFields.Add(new CustomField
                        {
                            Description = customFieldDescription,
                            Value = null
                        });
                    }
                }
                generalReport.RegistrationFields = customFields;

                customFieldDescriptions = filteredPurchaseRegistrationFields.Select(e => e.Description).Distinct();
                customFields = new List<CustomField>();
                var currentPurchaseRegistrationCustomFields = filteredPurchaseRegistrationFields.Where(e => e.PurchaseRegistrationId == partialGeneralReport.PurchaseRegistrationId);
                foreach (var customFieldDescription in customFieldDescriptions)
                {
                    var currentPurchaseRegistrationCustomField = currentPurchaseRegistrationCustomFields.FirstOrDefault(e => e.Description == customFieldDescription);
                    if (currentPurchaseRegistrationCustomField != null)
                    {
                        customFields.Add(new CustomField
                        {
                            Description = customFieldDescription,
                            Value = currentPurchaseRegistrationCustomField.Value
                        });
                    }
                    else
                    {
                        customFields.Add(new CustomField
                        {
                            Description = customFieldDescription,
                            Value = null
                        });
                    }
                }
                generalReport.PurchaseRegistrationFields = customFields;

                if (filteredHandlingBatchInfos.Count() > 0)
                {
                    foreach (var handlingBatchInfo in filteredHandlingBatchInfos.Where(e => e.RegistrationId == generalReport.Id))
                    {
                        generalReport.HandlingBatchId = handlingBatchInfo.HandlingBatchId;
                        generalReport.HandlingBatchFinishedTime = handlingBatchInfo.HandlingBatchFinishedTime;
                    }
                }

                _getGeneralReportDtoList.Add(generalReport);
            }

            var tenant = AbpSession.TenantId != null ? _tenantRepository.Get((int)AbpSession.TenantId).Name : "";
            var campaign = _campaignRepository.GetAll().Where(e => e.Id == input.CampaignFilter).FirstOrDefault().CampaignCode;
            var name = tenant + "_" + campaign + '_' + input.StartDateFilter.ToShortDateString().Replace('/','-') + '_' + input.EndDateFilter.ToShortDateString().Replace('/', '-');

            return _generalReportExcelExporter.ExportToFile(name, _getGeneralReportDtoList);
        }
    }
}
