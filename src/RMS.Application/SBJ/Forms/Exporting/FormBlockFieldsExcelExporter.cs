using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.Forms.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.Forms.Exporting
{
    public class FormBlockFieldsExcelExporter : NpoiExcelExporterBase, IFormBlockFieldsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public FormBlockFieldsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetFormBlockFieldForViewDto> formBlockFields)
        {
            return CreateExcelPackage(
                "FormBlockFields.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("FormBlockFields"));

                    AddHeader(
                        sheet,
                        L("SortOrder"),
                        (L("FormField")) + L("Description"),
                        (L("FormBlock")) + L("Description")
                        );

                    AddObjects(
                        sheet, 2, formBlockFields,
                        _ => _.FormBlockField.SortOrder,
                        _ => _.FormFieldDescription,
                        _ => _.FormBlockDescription
                        );

					
					
                });
        }
    }
}
