using System.Collections.Generic;
using RMS.SBJ.Registrations.Dtos;
using RMS.Dto;

namespace RMS.SBJ.Registrations.Exporting
{
    public interface IRegistrationsExcelExporter
    {
        FileDto ExportToFile(List<GetRegistrationForViewDto> registrations);
    }
}