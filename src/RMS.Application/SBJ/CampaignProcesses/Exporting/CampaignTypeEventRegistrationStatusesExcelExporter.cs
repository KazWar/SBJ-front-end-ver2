using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.CampaignProcesses.Exporting
{
    public class CampaignTypeEventRegistrationStatusesExcelExporter : NpoiExcelExporterBase, ICampaignTypeEventRegistrationStatusesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CampaignTypeEventRegistrationStatusesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCampaignTypeEventRegistrationStatusForViewDto> campaignTypeEventRegistrationStatuses)
        {
            return CreateExcelPackage(
                "CampaignTypeEventRegistrationStatuses.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("CampaignTypeEventRegistrationStatuses"));

                    AddHeader(
                        sheet,
                        L("SortOrder"),
                        (L("CampaignTypeEvent")) + L("SortOrder"),
                        (L("RegistrationStatus")) + L("Description")
                        );

                    AddObjects(
                        sheet, 2, campaignTypeEventRegistrationStatuses,
                        _ => _.CampaignTypeEventRegistrationStatus.SortOrder,
                        _ => _.CampaignTypeEventSortOrder,
                        _ => _.RegistrationStatusDescription
                        );

					
					
                });
        }
    }
}
