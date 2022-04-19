using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.CodeTypeTables.Exporting
{
    public class LocalesExcelExporter : NpoiExcelExporterBase, ILocalesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public LocalesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetLocaleForViewDto> locales)
        {
            return CreateExcelPackage(
                "Locales.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("Locales"));

                    AddHeader(
                        sheet,
                        L("LanguageCode"),
                        L("Description"),
                        L("IsActive"),
                        (L("Country")) + L("CountryCode")
                        );

                    AddObjects(
                        sheet, 2, locales,
                        _ => _.Locale.LanguageCode,
                        _ => _.Locale.Description,
                        _ => _.Locale.IsActive,
                        _ => _.CountryCountryCode
                        );

					
					
                });
        }
    }
}
