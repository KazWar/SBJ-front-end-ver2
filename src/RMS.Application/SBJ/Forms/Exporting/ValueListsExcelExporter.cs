using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.Forms.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.Forms.Exporting
{
    public class ValueListsExcelExporter : NpoiExcelExporterBase, IValueListsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ValueListsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetValueListForViewDto> valueLists)
        {
            return CreateExcelPackage(
                "ValueLists.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("ValueLists"));

                    AddHeader(
                        sheet,
                        L("Description"),
                        L("ListValueApiCall")
                        );

                    AddObjects(
                        sheet, 2, valueLists,
                        _ => _.ValueList.Description,
                        _ => _.ValueList.ListValueApiCall
                        );

					
					
                });
        }
    }
}
