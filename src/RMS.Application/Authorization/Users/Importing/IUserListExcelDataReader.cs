using System.Collections.Generic;
using RMS.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace RMS.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}
