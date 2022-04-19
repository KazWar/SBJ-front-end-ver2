using System.Collections.Generic;
using RMS.SBJ.Forms.Dtos;
using RMS.Dto;

namespace RMS.SBJ.Forms.Exporting
{
    public interface IFormBlocksExcelExporter
    {
        FileDto ExportToFile(List<GetFormBlockForViewDto> formBlocks);
    }
}