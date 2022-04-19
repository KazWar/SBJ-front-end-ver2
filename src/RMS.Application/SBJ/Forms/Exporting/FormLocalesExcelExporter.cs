using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.Forms.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.Forms.Exporting
{
    public class FormLocalesExcelExporter : NpoiExcelExporterBase, IFormLocalesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public FormLocalesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetFormLocaleForViewDto> formLocales)
        {
            return CreateExcelPackage(
                "FormLocales.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("FormLocales"));

                    AddHeader(
                        sheet,
                        L("Description"),
                        (L("Form")) + L("Version"),
                        (L("Locale")) + L("Description")
                        );

                    AddObjects(
                        sheet, 2, formLocales,
                        _ => _.FormLocale.Description,
                        _ => _.FormVersion,
                        _ => _.LocaleDescription
                        );

					
					
                });
        }
    }
}
