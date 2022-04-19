using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.RegistrationJsonData.Dtos;
using RMS.Dto;

namespace RMS.SBJ.RegistrationJsonData
{
    public interface IRegistrationJsonDatasAppService : IApplicationService
    {
        Task<PagedResultDto<GetRegistrationJsonDataForViewDto>> GetAll(GetAllRegistrationJsonDatasInput input);

        Task<GetRegistrationJsonDataForViewDto> GetRegistrationJsonDataForView(long id);

        Task<GetRegistrationJsonDataForEditOutput> GetRegistrationJsonDataForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditRegistrationJsonDataDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetRegistrationJsonDatasToExcel(GetAllRegistrationJsonDatasForExcelInput input);

        Task<PagedResultDto<RegistrationJsonDataRegistrationLookupTableDto>> GetAllRegistrationForLookupTable(GetAllForLookupTableInput input);

    }
}