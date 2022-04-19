using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.CampaignProcesses.Exporting
{
    public class CampaignTypeEventsExcelExporter : NpoiExcelExporterBase, ICampaignTypeEventsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CampaignTypeEventsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCampaignTypeEventForViewDto> campaignTypeEvents)
        {
            return CreateExcelPackage(
                "CampaignTypeEvents.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("CampaignTypeEvents"));

                    AddHeader(
                        sheet,
                        L("SortOrder"),
                        (L("CampaignType")) + L("Name"),
                        (L("ProcessEvent")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, campaignTypeEvents,
                        _ => _.CampaignTypeEvent.SortOrder,
                        _ => _.CampaignTypeName,
                        _ => _.ProcessEventName
                        );

					
					
                });
        }
    }
}
