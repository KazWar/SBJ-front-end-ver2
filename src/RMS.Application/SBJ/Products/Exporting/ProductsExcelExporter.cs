using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.Products.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.Products.Exporting
{
    public class ProductsExcelExporter : NpoiExcelExporterBase, IProductsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductForViewDto> products)
        {
            return CreateExcelPackage(
                "Products.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("Products"));

                    AddHeader(
                        sheet,
                        L("ProductCode"),
                        L("Description"),
                        L("Ean"),
                        (L("ProductCategory")) + L("Description")
                        );

                    AddObjects(
                        sheet, 2, products,
                        _ => _.Product.ProductCode,
                        _ => _.Product.Description,
                        _ => _.Product.Ean,
                        _ => _.ProductCategoryDescription
                        );

					
					
                });
        }
    }
}
