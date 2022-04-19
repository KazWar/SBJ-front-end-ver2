using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.CampaignProcesses.Exporting
{
    public class MessageComponentContentsExcelExporter : NpoiExcelExporterBase, IMessageComponentContentsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MessageComponentContentsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetMessageComponentContentForViewDto> messageComponentContents)
        {
            return CreateExcelPackage(
                "MessageComponentContents.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("MessageComponentContents"));

                    AddHeader(
                        sheet,
                        L("Content"),
                        (L("MessageComponent")) + L("IsActive"),
                        (L("CampaignTypeEventRegistrationStatus")) + L("SortOrder")
                        );

                    AddObjects(
                        sheet, 2, messageComponentContents,
                        _ => _.MessageComponentContent.Content,
                        _ => _.MessageComponentIsActive,
                        _ => _.CampaignTypeEventRegistrationStatusSortOrder
                        );

					
					
                });
        }
    }
}
