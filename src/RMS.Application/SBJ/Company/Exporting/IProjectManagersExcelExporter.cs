using System.Collections.Generic;
using RMS.SBJ.Company.Dtos;
using RMS.Dto;

namespace RMS.SBJ.Company.Exporting
{
    public interface IProjectManagersExcelExporter
    {
        FileDto ExportToFile(List<GetProjectManagerForViewDto> projectManagers);
    }
}