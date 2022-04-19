using System.Collections.Generic;
using RMS.SBJ.SystemTables.Dtos;
using RMS.Dto;

namespace RMS.SBJ.SystemTables.Exporting
{
    public interface IMessagesExcelExporter
    {
        FileDto ExportToFile(List<GetMessageForViewDto> messages);
    }
}