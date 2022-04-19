using System.Collections.Generic;
using RMS.PromoPlanner.Dtos;
using RMS.Dto;

namespace RMS.PromoPlanner.Exporting
{
    public interface IPromoProductsExcelExporter
    {
        FileDto ExportToFile(List<GetPromoProductForViewDto> promoProducts);
    }
}