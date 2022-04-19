using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.Forms.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.Forms.Exporting
{
    public class ListValuesExcelExporter : NpoiExcelExporterBase, IListValuesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListValuesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetListValueForViewDto> listValues)
        {
            return CreateExcelPackage(
                "ListValues.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("ListValues"));

                    AddHeader(
                        sheet,
                        L("KeyValue"),
                        L("Description"),
                        L("SortOrder"),
                        (L("ValueList")) + L("Description")
                        );

                    AddObjects(
                        sheet, 2, listValues,
                        _ => _.ListValue.KeyValue,
                        _ => _.ListValue.Description,
                        _ => _.ListValue.SortOrder,
                        _ => _.ValueListDescription
                        );

					
					
                });
        }
    }
}
