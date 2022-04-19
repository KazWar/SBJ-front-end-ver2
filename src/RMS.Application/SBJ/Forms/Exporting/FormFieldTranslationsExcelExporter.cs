using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.Forms.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.Forms.Exporting
{
    public class FormFieldTranslationsExcelExporter : NpoiExcelExporterBase, IFormFieldTranslationsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public FormFieldTranslationsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetFormFieldTranslationForViewDto> formFieldTranslations)
        {
            return CreateExcelPackage(
                "FormFieldTranslations.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("FormFieldTranslations"));

                    AddHeader(
                        sheet,
                        L("Label"),
                        L("DefaultValue"),
                        L("RegularExpression"),
                        (L("FormField")) + L("Description"),
                        (L("Locale")) + L("LanguageCode")
                        );

                    AddObjects(
                        sheet, 2, formFieldTranslations,
                        _ => _.FormFieldTranslation.Label,
                        _ => _.FormFieldTranslation.DefaultValue,
                        _ => _.FormFieldTranslation.RegularExpression,
                        _ => _.FormFieldDescription,
                        _ => _.LocaleLanguageCode
                        );

					
					
                });
        }
    }
}
