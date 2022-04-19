using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.SystemTables.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.SystemTables.Exporting
{
    public class MessagesExcelExporter : NpoiExcelExporterBase, IMessagesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MessagesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetMessageForViewDto> messages)
        {
            return CreateExcelPackage(
                "Messages.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("Messages"));

                    AddHeader(
                        sheet,
                        L("Version"),
                        (L("SystemLevel")) + L("Description")
                        );

                    AddObjects(
                        sheet, 2, messages,
                        _ => _.Message.Version,
                        _ => _.SystemLevelDescription
                        );

					
					
                });
        }
    }
}
