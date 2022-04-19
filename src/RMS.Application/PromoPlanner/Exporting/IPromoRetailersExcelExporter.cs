using System.Collections.Generic;
using RMS.PromoPlanner.Dtos;
using RMS.Dto;

namespace RMS.PromoPlanner.Exporting
{
    public interface IPromoRetailersExcelExporter
    {
        FileDto ExportToFile(List<GetPromoRetailerForViewDto> promoRetailers);
    }
}