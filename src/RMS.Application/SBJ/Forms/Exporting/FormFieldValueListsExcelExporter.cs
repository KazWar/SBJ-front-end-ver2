using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.Forms.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.Forms.Exporting
{
    public class FormFieldValueListsExcelExporter : NpoiExcelExporterBase, IFormFieldValueListsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public FormFieldValueListsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetFormFieldValueListForViewDto> formFieldValueLists)
        {
            return CreateExcelPackage(
                "FormFieldValueLists.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("FormFieldValueLists"));

                    AddHeader(
                        sheet,
                        L("PossibleListValues"),
                        (L("FormField")) + L("Description"),
                        (L("ValueList")) + L("Description")
                        );

                    AddObjects(
                        sheet, 2, formFieldValueLists,
                        _ => _.FormFieldValueList.PossibleListValues,
                        _ => _.FormFieldDescription,
                        _ => _.ValueListDescription
                        );

					
					
                });
        }
    }
}
