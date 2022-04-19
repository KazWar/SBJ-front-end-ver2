using System.Collections.Generic;
using Abp;
using RMS.Chat.Dto;
using RMS.Dto;

namespace RMS.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
