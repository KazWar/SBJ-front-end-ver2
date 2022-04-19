using System.Collections.Generic;
using RMS.SBJ.RegistrationHistory.Dtos;
using RMS.Dto;

namespace RMS.SBJ.RegistrationHistory.Exporting
{
    public interface IRegistrationHistoriesExcelExporter
    {
        FileDto ExportToFile(List<GetRegistrationHistoryForViewDto> registrationHistories);
    }
}