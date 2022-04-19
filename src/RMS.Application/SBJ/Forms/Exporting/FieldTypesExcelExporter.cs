using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.Forms.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.Forms.Exporting
{
    public class FieldTypesExcelExporter : NpoiExcelExporterBase, IFieldTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public FieldTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetFieldTypeForViewDto> fieldTypes)
        {
            return CreateExcelPackage(
                "FieldTypes.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("FieldTypes"));

                    AddHeader(
                        sheet,
                        L("Description")
                        );

                    AddObjects(
                        sheet, 2, fieldTypes,
                        _ => _.FieldType.Description
                        );

					
					
                });
        }
    }
}
