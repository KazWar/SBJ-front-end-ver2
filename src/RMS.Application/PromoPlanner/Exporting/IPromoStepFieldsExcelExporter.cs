using System.Collections.Generic;
using RMS.PromoPlanner.Dtos;
using RMS.Dto;

namespace RMS.PromoPlanner.Exporting
{
    public interface IPromoStepFieldsExcelExporter
    {
        FileDto ExportToFile(List<GetPromoStepFieldForViewDto> promoStepFields);
    }
}