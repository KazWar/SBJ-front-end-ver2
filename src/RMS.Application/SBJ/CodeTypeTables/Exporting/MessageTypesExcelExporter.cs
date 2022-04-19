using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.CodeTypeTables.Exporting
{
    public class MessageTypesExcelExporter : NpoiExcelExporterBase, IMessageTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MessageTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetMessageTypeForViewDto> messageTypes)
        {
            return CreateExcelPackage(
                "MessageTypes.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("MessageTypes"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Source"),
                        L("IsActive"),
                        (L("Message")) + L("Version")
                        );

                    AddObjects(
                        sheet, 2, messageTypes,
                        _ => _.MessageType.Name,
                        _ => _.MessageType.Source,
                        _ => _.MessageType.IsActive,
                        _ => _.MessageVersion
                        );

					
					
                });
        }
    }
}
