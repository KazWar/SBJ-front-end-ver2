using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.Dto;


namespace RMS.SBJ.CodeTypeTables
{
    public interface IProcessEventsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetProcessEventForViewDto>> GetAll(GetAllProcessEventsInput input);

        Task<GetProcessEventForViewDto> GetProcessEventForView(long id);

		Task<GetProcessEventForEditOutput> GetProcessEventForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditProcessEventDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetProcessEventsToExcel(GetAllProcessEventsForExcelInput input);

		
    }
}