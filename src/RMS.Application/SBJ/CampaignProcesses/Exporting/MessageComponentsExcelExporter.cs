using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.CampaignProcesses.Exporting
{
    public class MessageComponentsExcelExporter : NpoiExcelExporterBase, IMessageComponentsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MessageComponentsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetMessageComponentForViewDto> messageComponents)
        {
            return CreateExcelPackage(
                "MessageComponents.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("MessageComponents"));

                    AddHeader(
                        sheet,
                        L("IsActive"),
                        (L("MessageType")) + L("Name"),
                        (L("MessageComponentType")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, messageComponents,
                        _ => _.MessageComponent.IsActive,
                        _ => _.MessageTypeName,
                        _ => _.MessageComponentTypeName
                        );

					
					
                });
        }
    }
}
