using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.CodeTypeTables.Exporting
{
    public class ProcessEventsExcelExporter : NpoiExcelExporterBase, IProcessEventsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProcessEventsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProcessEventForViewDto> processEvents)
        {
            return CreateExcelPackage(
                "ProcessEvents.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("ProcessEvents"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, 2, processEvents,
                        _ => _.ProcessEvent.Name,
                        _ => _.ProcessEvent.IsActive
                        );

					
					
                });
        }
    }
}
