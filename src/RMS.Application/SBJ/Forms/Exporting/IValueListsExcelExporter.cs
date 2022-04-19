using System.Collections.Generic;
using RMS.SBJ.Forms.Dtos;
using RMS.Dto;

namespace RMS.SBJ.Forms.Exporting
{
    public interface IValueListsExcelExporter
    {
        FileDto ExportToFile(List<GetValueListForViewDto> valueLists);
    }
}