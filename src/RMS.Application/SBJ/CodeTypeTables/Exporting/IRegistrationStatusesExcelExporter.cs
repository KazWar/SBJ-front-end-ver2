using System.Collections.Generic;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.Dto;

namespace RMS.SBJ.CodeTypeTables.Exporting
{
    public interface IRegistrationStatusesExcelExporter
    {
        FileDto ExportToFile(List<GetRegistrationStatusForViewDto> registrationStatuses);
    }
}