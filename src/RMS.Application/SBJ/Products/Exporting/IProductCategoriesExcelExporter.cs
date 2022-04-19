using System.Collections.Generic;
using RMS.SBJ.Products.Dtos;
using RMS.Dto;

namespace RMS.SBJ.Products.Exporting
{
    public interface IProductCategoriesExcelExporter
    {
        FileDto ExportToFile(List<GetProductCategoryForViewDto> productCategories);
    }
}