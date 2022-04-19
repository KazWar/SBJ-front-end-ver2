using System.Collections.Generic;
using RMS.Authorization.Users.Importing.Dto;
using RMS.Dto;

namespace RMS.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
