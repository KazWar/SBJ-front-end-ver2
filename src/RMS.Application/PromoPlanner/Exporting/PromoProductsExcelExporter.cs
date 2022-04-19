using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.PromoPlanner.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.PromoPlanner.Exporting
{
    public class PromoProductsExcelExporter : NpoiExcelExporterBase, IPromoProductsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PromoProductsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPromoProductForViewDto> promoProducts)
        {
            return CreateExcelPackage(
                "PromoProducts.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("PromoProducts"));

                    AddHeader(
                        sheet,
                        (L("Promo")) + L("Promocode"),
                        (L("Product")) + L("Ctn")
                        );

                    AddObjects(
                        sheet, 2, promoProducts,
                        _ => _.PromoPromocode,
                        _ => _.ProductCtn
                        );

					
					
                });
        }
    }
}
