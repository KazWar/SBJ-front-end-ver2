using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.CampaignProcesses.Exporting
{
    public class CampaignMessagesExcelExporter : NpoiExcelExporterBase, ICampaignMessagesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CampaignMessagesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCampaignMessageForViewDto> campaignMessages)
        {
            return CreateExcelPackage(
                "CampaignMessages.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("CampaignMessages"));

                    AddHeader(
                        sheet,
                        L("IsActive"),
                        (L("Campaign")) + L("Name"),
                        (L("Message")) + L("Version")
                        );

                    AddObjects(
                        sheet, 2, campaignMessages,
                        _ => _.CampaignMessage.IsActive,
                        _ => _.CampaignName,
                        _ => _.MessageVersion
                        );

					
					
                });
        }
    }
}
