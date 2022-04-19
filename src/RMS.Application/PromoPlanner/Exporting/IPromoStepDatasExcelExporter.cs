using System.Collections.Generic;
using RMS.PromoPlanner.Dtos;
using RMS.Dto;

namespace RMS.PromoPlanner.Exporting
{
    public interface IPromoStepDatasExcelExporter
    {
        FileDto ExportToFile(List<GetPromoStepDataForViewDto> promoStepDatas);
    }
}