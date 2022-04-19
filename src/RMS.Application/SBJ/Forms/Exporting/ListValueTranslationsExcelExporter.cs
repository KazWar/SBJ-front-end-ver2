using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.Forms.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.Forms.Exporting
{
    public class ListValueTranslationsExcelExporter : NpoiExcelExporterBase, IListValueTranslationsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListValueTranslationsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetListValueTranslationForViewDto> listValueTranslations)
        {
            return CreateExcelPackage(
                "ListValueTranslations.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("ListValueTranslations"));

                    AddHeader(
                        sheet,
                        L("KeyValue"),
                        L("Description"),
                        (L("ListValue")) + L("KeyValue"),
                        (L("Locale")) + L("LanguageCode")
                        );

                    AddObjects(
                        sheet, 2, listValueTranslations,
                        _ => _.ListValueTranslation.KeyValue,
                        _ => _.ListValueTranslation.Description,
                        _ => _.ListValueKeyValue,
                        _ => _.LocaleLanguageCode
                        );

					
					
                });
        }
    }
}
