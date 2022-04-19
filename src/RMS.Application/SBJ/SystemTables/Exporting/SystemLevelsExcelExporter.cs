using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.SystemTables.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.SystemTables.Exporting
{
    public class SystemLevelsExcelExporter : NpoiExcelExporterBase, ISystemLevelsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public SystemLevelsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetSystemLevelForViewDto> systemLevels)
        {
            return CreateExcelPackage(
                "SystemLevels.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("SystemLevels"));

                    AddHeader(
                        sheet,
                        L("Description")
                        );

                    AddObjects(
                        sheet, 2, systemLevels,
                        _ => _.SystemLevel.Description
                        );

					
					
                });
        }
    }
}
