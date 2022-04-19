using Abp.Authorization;
using RMS.Authorization;
using RMS.Dto;
using RMS.SBJ.Report.CostReports.Dtos;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.CampaignProcesses;
using RMS.SBJ.Registrations;
using RMS.SBJ.HandlingBatch.Helpers;
using RMS.SBJ.Registrations.Helpers;
using RMS.SBJ.CodeTypeTables;
using System.Collections.Generic;
using RMS.SBJ.CampaignProcesses.Helpers;
using RMS.SBJ.PurchaseRegistrations;
using RMS.SBJ.Report.CostReports;
using RMS.MultiTenancy;
using RMS.SBJ.Report.CostReports.Exporting;

namespace RMS.SBJ.CostReports
{
    [AbpAuthorize(AppPermissions.Pages_CostReport)]

    public class CostReportAppService : RMSAppServiceBase, ICostReportAppService
    {

        private readonly IRepository<Campaign, long> _lookup_campaignRepository;
        private readonly ICampaignTypesAppService _campaignTypesAppService;
        private readonly ICostReportExcelExporter _costReportExcelExporter;
        private readonly IRepository<HandlingBatch.HandlingBatch, long> _lookup_handlingBatchRepository;
        private readonly IRepository<HandlingBatch.HandlingBatchLine, long> _lookup_handlingBatchLineRepository;
        private readonly IRepository<HandlingBatch.HandlingBatchHistory, long> _lookup_handlingBatchHistoryRepository;
        private readonly IRepository<HandlingBatch.HandlingBatchStatus, long> _lookup_handlingBatchStatusRepository;
        private readonly IRepository<Registration, long> _lookup_registrationRepository;
        private readonly IRepository<RegistrationHistory.RegistrationHistory, long> _lookup_registrationHistoryRepository;
        private readonly IRepository<RegistrationStatus, long> _lookup_registrationStatusRepository;
        private readonly IRepository<PurchaseRegistration, long> _lookup_purchaseRegistrationRepository;
        private readonly IRepository<Tenant> _lookup_tenantRepository;

        public CostReportAppService(IRepository<Campaign, long> lookup_campaignRepository,
                                    ICampaignTypesAppService campaignTypesAppService,
                                    ICostReportExcelExporter costReportExcelExporter,
                                    IRepository<HandlingBatch.HandlingBatch, long> lookup_handlingBatchRepository,
                                    IRepository<HandlingBatch.HandlingBatchLine, long> lookup_handlingBatchLineRepository,
                                    IRepository<HandlingBatch.HandlingBatchHistory, long> lookup_handlingBatchHistoryRepository,
                                    IRepository<HandlingBatch.HandlingBatchStatus, long> lookup_handlingBatchStatusRepository,
                                    IRepository<Registration, long> lookup_registrationRepository,
                                    IRepository<RegistrationHistory.RegistrationHistory, long> lookup_registrationHistoryRepository,
                                    IRepository<RegistrationStatus, long> lookup_registrationStatusRepository,
                                    IRepository<PurchaseRegistration, long> lookup_purchaseRegistrationRepository,
                                    IRepository<Tenant> lookup_tenantRepository)
        {
            _lookup_campaignRepository = lookup_campaignRepository;
            _campaignTypesAppService = campaignTypesAppService;
            _costReportExcelExporter = costReportExcelExporter;
            _lookup_handlingBatchRepository = lookup_handlingBatchRepository;
            _lookup_handlingBatchLineRepository = lookup_handlingBatchLineRepository;
            _lookup_handlingBatchHistoryRepository = lookup_handlingBatchHistoryRepository;
            _lookup_handlingBatchStatusRepository = lookup_handlingBatchStatusRepository;
            _lookup_registrationRepository = lookup_registrationRepository;
            _lookup_registrationHistoryRepository = lookup_registrationHistoryRepository;
            _lookup_registrationStatusRepository = lookup_registrationStatusRepository;
            _lookup_purchaseRegistrationRepository = lookup_purchaseRegistrationRepository;
            _lookup_tenantRepository = lookup_tenantRepository;
        }

        public async Task<CostReportDto> GetCostReport(GetAllCostReportInput input)
        {
            var costReport = new CostReportDto();
            var campaignId = input.CampaignId;

            var campaign = await _lookup_campaignRepository.GetAsync(campaignId);
            if (campaign == null) return costReport;

            costReport.CampaignCode = campaign.CampaignCode.ToString();
            costReport.CampaignName = campaign.Name;
            costReport.CampaignStart = campaign.StartDate;
            costReport.CampaignEnd = campaign.EndDate;

            var minDate = input.StartDateFilter;
            var maxDate = input.EndDateFilter;

            var registrationsWithHistory = (from r in _lookup_registrationRepository.GetAll()
                                            join rh in _lookup_registrationHistoryRepository.GetAll() on r.Id equals rh.RegistrationId
                                            join rs in _lookup_registrationStatusRepository.GetAll() on rh.RegistrationStatusId equals rs.Id
                                            where r.CampaignId == campaignId && rh.DateCreated > minDate && rh.DateCreated < maxDate.AddDays(1)
                                            select new
                                            {
                                                Year = rh.DateCreated.Year,
                                                Month = rh.DateCreated.Month,
                                                RegistrationId = r.Id,
                                                HistoryStatus = rs.StatusCode,
                                                HistoryDate = rh.DateCreated
                                            }).ToList();

            var registrationsGrouped = (from rwh in registrationsWithHistory
                                        group new { rwh.Year, rwh.Month, rwh.RegistrationId, rwh.HistoryStatus, rwh.HistoryDate }
                                        by new { rwh.Year, rwh.Month, rwh.RegistrationId, rwh.HistoryStatus } into GroupedResult
                                        select new
                                        {
                                            Year = GroupedResult.Key.Year,
                                            Month = GroupedResult.Key.Month,
                                            RegistrationId = GroupedResult.Key.RegistrationId,
                                            HistoryStatus = GroupedResult.Key.HistoryStatus,
                                            LastDateHistoryStatus = GroupedResult.OrderByDescending(hd => hd.HistoryDate).Select(hd => hd.HistoryDate)
                                                               .FirstOrDefault()
                                        }).ToList();


            var finishedHandlingBatch = HandlingBatchStatusCodeHelper.Finished;

            var typePremium = await _campaignTypesAppService.GetByCode(CampaignTypeHelper.Premium);
            var typeCashRefund = await _campaignTypesAppService.GetByCode(CampaignTypeHelper.CashRefund);
            var typeActivationCode = await _campaignTypesAppService.GetByCode(CampaignTypeHelper.ActivationCode);

            var startYear = minDate.Year;
            var startMonth = minDate.Month;
            var endYear = maxDate.Year;
            var endMonth = maxDate.Month;

            var totalsMonth = new List<MonthlyTotal>();

            for (int year = startYear; year <= endYear; year++)
            {
                for (int month = (year == startYear ? startMonth : 1); month <= (year == endYear ? endMonth : 12); month++)
                {
                    var totalMonth = new MonthlyTotal();
                    totalMonth.Year = year;
                    totalMonth.Month = month;

                    totalMonth.NewRegistrations = registrationsGrouped.Where(rg => rg.Year == year && rg.Month == month && rg.HistoryStatus == RegistrationStatusCodeHelper.Pending).ToList().Count;
                    totalMonth.CompleteRegistrations = registrationsGrouped.Where(rg => rg.Year == year && rg.Month == month && rg.HistoryStatus == RegistrationStatusCodeHelper.Accepted).ToList().Count;
                    totalMonth.ExtraHandlingRegistrations = registrationsGrouped.Where(rg => rg.Year == year && rg.Month == month && (rg.HistoryStatus == RegistrationStatusCodeHelper.Incomplete || rg.HistoryStatus == RegistrationStatusCodeHelper.Rejected)).ToList().Count;

                    //All finished handlingbatches this month
                    var finishedhandlingBatches = (from hb in _lookup_handlingBatchRepository.GetAll()
                                                   join hbl in _lookup_handlingBatchLineRepository.GetAll() on hb.Id equals hbl.HandlingBatchId
                                                   join pr in _lookup_purchaseRegistrationRepository.GetAll() on hbl.PurchaseRegistrationId equals pr.Id
                                                   join r in _lookup_registrationRepository.GetAll() on pr.RegistrationId equals r.Id
                                                   join hbh in _lookup_handlingBatchHistoryRepository.GetAll() on hb.Id equals hbh.HandlingBatchId
                                                   join hbs in _lookup_handlingBatchStatusRepository.GetAll() on hbh.HandlingBatchStatusId equals hbs.Id
                                                   where hbs.StatusCode == finishedHandlingBatch && (hbh.DateCreated.Year == year && hbh.DateCreated.Month == month)
                                                   select new
                                                   {
                                                       HandlingBatchId = hb.Id,
                                                       CampaignTypeId = hb.CampaignTypeId,
                                                       OrderId = hbl.ExternalOrderId,
                                                       RegistrationId = pr.RegistrationId
                                                   }).ToList();

                    var paymentBatches = (from fhb in finishedhandlingBatches
                                          where fhb.CampaignTypeId == typeCashRefund.CampaignType.Id
                                          select new
                                          {
                                              HandlingBatchId = fhb.HandlingBatchId,
                                              RegistrationId = fhb.RegistrationId
                                          }).ToList();
                    totalMonth.PaymentBatches = paymentBatches.Select(p => p.HandlingBatchId).Distinct().Count();
                    totalMonth.PaymentsSent = paymentBatches.Select(p => p.RegistrationId).Distinct().Count();

                    var activationCodeBatches = (from fhb in finishedhandlingBatches
                                                 where fhb.CampaignTypeId == typeActivationCode.CampaignType.Id
                                                 select new
                                                 {
                                                     HandlingBatchId = fhb.HandlingBatchId,
                                                     RegistrationId = fhb.RegistrationId
                                                 }).ToList();
                    totalMonth.ActivationCodeBatches = activationCodeBatches.Select(p => p.HandlingBatchId).Distinct().Count();
                    totalMonth.ActivationCodesSent = activationCodeBatches.Select(p => p.RegistrationId).Distinct().Count();

                    var premiumBatches = (from fhb in finishedhandlingBatches
                                          where fhb.CampaignTypeId == typePremium.CampaignType.Id
                                          select new
                                          {
                                              HandlingBatchId = fhb.HandlingBatchId,
                                              OrderId = fhb.OrderId
                                          }).ToList();
                    totalMonth.PremiumBatches = premiumBatches.Select(p => p.HandlingBatchId).Distinct().Count();
                    totalMonth.PremiumsSent = premiumBatches.Select(p => p.OrderId).Distinct().Count();

                    totalsMonth.Add(totalMonth);

                }
            }

            costReport.MonthlyTotals = totalsMonth;

            return costReport;

        }

        public async Task<FileDto> GetCostReportToExcel(GetAllCostReportInput input)
        {
            var tenant = AbpSession.TenantId != null ? _lookup_tenantRepository.Get((int)AbpSession.TenantId).Name : "";
            var campaign = _lookup_campaignRepository.GetAll().Where(e => e.Id == input.CampaignId).FirstOrDefault().CampaignCode;
            var name = tenant + "_" + campaign + "_" + input.StartDateFilter.ToShortDateString().Replace('/', '-') + '_' + input.EndDateFilter.ToShortDateString().Replace('/', '-');

            return _costReportExcelExporter.ExportToFile(name, await GetCostReport(input));
        }

    }
}
