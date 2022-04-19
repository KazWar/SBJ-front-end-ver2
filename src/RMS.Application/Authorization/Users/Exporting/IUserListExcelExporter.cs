using System.Collections.Generic;
using RMS.Authorization.Users.Dto;
using RMS.Dto;

namespace RMS.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}