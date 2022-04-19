using System.Collections.Generic;
using RMS.SBJ.HandlingLines.Dtos;
using RMS.Dto;

namespace RMS.SBJ.HandlingLines.Exporting
{
    public interface IHandlingLinesExcelExporter
    {
        FileDto ExportToFile(List<GetHandlingLineForViewDto> handlingLines);
    }
}