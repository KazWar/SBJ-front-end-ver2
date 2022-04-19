using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.PromoPlanner.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.PromoPlanner.Exporting
{
    public class PromoCountriesExcelExporter : NpoiExcelExporterBase, IPromoCountriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PromoCountriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPromoCountryForViewDto> promoCountries)
        {
            return CreateExcelPackage(
                "PromoCountries.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("PromoCountries"));

                    AddHeader(
                        sheet,
                        (L("Promo")) + L("Promocode"),
                        (L("Country")) + L("CountryCode")
                        );

                    AddObjects(
                        sheet, 2, promoCountries,
                        _ => _.PromoPromocode,
                        _ => _.CountryCountryCode
                        );

					
					
                });
        }
    }
}
