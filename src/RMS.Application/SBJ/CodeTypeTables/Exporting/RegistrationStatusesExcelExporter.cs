using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.CodeTypeTables.Exporting
{
    public class RegistrationStatusesExcelExporter : NpoiExcelExporterBase, IRegistrationStatusesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RegistrationStatusesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRegistrationStatusForViewDto> registrationStatuses)
        {
            return CreateExcelPackage(
                "RegistrationStatuses.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("RegistrationStatuses"));

                    AddHeader(
                        sheet,
                        L("StatusCode"),
                        L("Description"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, 2, registrationStatuses,
                        _ => _.RegistrationStatus.StatusCode,
                        _ => _.RegistrationStatus.Description,
                        _ => _.RegistrationStatus.IsActive
                        );

					
					
                });
        }
    }
}
