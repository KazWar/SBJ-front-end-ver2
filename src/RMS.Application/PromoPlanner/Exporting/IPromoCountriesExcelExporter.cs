using System.Collections.Generic;
using RMS.PromoPlanner.Dtos;
using RMS.Dto;

namespace RMS.PromoPlanner.Exporting
{
    public interface IPromoCountriesExcelExporter
    {
        FileDto ExportToFile(List<GetPromoCountryForViewDto> promoCountries);
    }
}