using System.Collections.Generic;
using RMS.SBJ.Products.Dtos;
using RMS.Dto;

namespace RMS.SBJ.Products.Exporting
{
    public interface IProductsExcelExporter
    {
        FileDto ExportToFile(List<GetProductForViewDto> products);
    }
}