using System.Collections.Generic;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.Dto;

namespace RMS.SBJ.CodeTypeTables.Exporting
{
    public interface IRejectionReasonsExcelExporter
    {
        FileDto ExportToFile(List<GetRejectionReasonForViewDto> rejectionReasons);
    }
}