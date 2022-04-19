using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.SystemTables.Dtos;
using RMS.Dto;


namespace RMS.SBJ.SystemTables
{
    public interface ISystemLevelsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetSystemLevelForViewDto>> GetAll(GetAllSystemLevelsInput input);

        Task<GetSystemLevelForViewDto> GetSystemLevelForView(long id);

		Task<GetSystemLevelForEditOutput> GetSystemLevelForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditSystemLevelDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetSystemLevelsToExcel(GetAllSystemLevelsForExcelInput input);

		
    }
}