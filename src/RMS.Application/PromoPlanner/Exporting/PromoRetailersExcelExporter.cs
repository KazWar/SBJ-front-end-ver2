using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.PromoPlanner.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.PromoPlanner.Exporting
{
    public class PromoRetailersExcelExporter : NpoiExcelExporterBase, IPromoRetailersExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PromoRetailersExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPromoRetailerForViewDto> promoRetailers)
        {
            return CreateExcelPackage(
                "PromoRetailers.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("PromoRetailers"));

                    AddHeader(
                        sheet,
                        (L("Promo")) + L("Promocode"),
                        (L("Retailer")) + L("Code")
                        );

                    AddObjects(
                        sheet, 2, promoRetailers,
                        _ => _.PromoPromocode,
                        _ => _.RetailerCode
                        );

					
					
                });
        }
    }
}
