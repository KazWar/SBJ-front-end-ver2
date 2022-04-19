using System.Collections.Generic;
using RMS.PromoPlanner.Dtos;
using RMS.Dto;

namespace RMS.PromoPlanner.Exporting
{
    public interface IPromoScopesExcelExporter
    {
        FileDto ExportToFile(List<GetPromoScopeForViewDto> promoScopes);
    }
}