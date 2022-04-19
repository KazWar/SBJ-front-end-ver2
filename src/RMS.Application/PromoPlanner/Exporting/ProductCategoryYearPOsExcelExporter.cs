using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.Dto;
using RMS.Storage;
using RMS.DataExporting.Excel.NPOI;
using RMS.PromoPlanner.Dtos;

namespace RMS.PromoPlanner.Exporting
{
    public class ProductCategoryYearPosExcelExporter : NpoiExcelExporterBase, IProductCategoryYearPosExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductCategoryYearPosExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductCategoryYearPoForViewDto> productCategoryYearPOs)
        {
            return CreateExcelPackage(
                "ProductCategoryYearPos.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("ProductCategoryYearPos"));

                    AddHeader(
                        sheet,
                        L("Year"),
                        L("PoNumberHandling"),
                        L("PoNumberCashback"),
                        L("ProductCategory") + L("Code")
                        );

                    AddObjects(
                        sheet, 2, productCategoryYearPOs,
                        _ => _.ProductCategoryYearPo.Year,
                        _ => _.ProductCategoryYearPo.PoNumberHandling,
                        _ => _.ProductCategoryYearPo.PoNumberCashback,
                        _ => _.ProductCategoryCode
                        );
                });
        }
    }
}
