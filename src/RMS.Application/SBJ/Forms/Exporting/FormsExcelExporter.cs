using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.Forms.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.Forms.Exporting
{
    public class FormsExcelExporter : NpoiExcelExporterBase, IFormsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public FormsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetFormForViewDto> forms)
        {
            return CreateExcelPackage(
                "Forms.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("Forms"));

                    AddHeader(
                        sheet,
                        L("Version"),
                        (L("SystemLevel")) + L("Description")
                        );

                    AddObjects(
                        sheet, 2, forms,
                        _ => _.Form.Version,
                        _ => _.SystemLevelDescription
                        );

					
					
                });
        }
    }
}
