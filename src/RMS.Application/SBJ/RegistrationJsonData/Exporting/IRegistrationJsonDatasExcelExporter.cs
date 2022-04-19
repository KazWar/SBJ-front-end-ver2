using System.Collections.Generic;
using RMS.SBJ.RegistrationJsonData.Dtos;
using RMS.Dto;

namespace RMS.SBJ.RegistrationJsonData.Exporting
{
    public interface IRegistrationJsonDatasExcelExporter
    {
        FileDto ExportToFile(List<GetRegistrationJsonDataForViewDto> registrationJsonDatas);
    }
}