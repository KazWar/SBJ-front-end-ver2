using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.Company.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.Company.Exporting
{
    public class ProjectManagersExcelExporter : NpoiExcelExporterBase, IProjectManagersExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProjectManagersExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProjectManagerForViewDto> projectManagers)
        {
            return CreateExcelPackage(
                "ProjectManagers.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("ProjectManagers"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("PhoneNumber"),
                        L("EmailAddress"),
                        (L("Address")) + L("PostalCode")
                        );

                    AddObjects(
                        sheet, 2, projectManagers,
                        _ => _.ProjectManager.Name,
                        _ => _.ProjectManager.PhoneNumber,
                        _ => _.ProjectManager.EmailAddress,
                        _ => _.AddressPostalCode
                        );

					
					
                });
        }
    }
}
