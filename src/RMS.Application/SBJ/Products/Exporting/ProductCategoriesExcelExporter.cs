using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.Products.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.Products.Exporting
{
    public class ProductCategoriesExcelExporter : NpoiExcelExporterBase, IProductCategoriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductCategoriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductCategoryForViewDto> productCategories)
        {
            return CreateExcelPackage(
                "ProductCategories.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("ProductCategories"));

                    AddHeader(
                        sheet,
                        L("Code"),
                        L("Description"),
                        L("PoHandling"),
                        L("PoCashBack"),
                        L("Color")
                        );

                    AddObjects(
                        sheet, 2, productCategories,
                        _ => _.ProductCategory.Code,
                        _ => _.ProductCategory.Description,
                        _ => _.ProductCategory.POHandling,
                        _ => _.ProductCategory.POCashback,
                        _ => _.ProductCategory.Color
                        );

					
					
                });
        }
    }
}
