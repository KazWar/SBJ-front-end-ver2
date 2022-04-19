using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.Company.Dtos;
using RMS.Dto;


namespace RMS.SBJ.Company
{
    public interface IProjectManagersAppService : IApplicationService 
    {
        Task<PagedResultDto<GetProjectManagerForViewDto>> GetAll(GetAllProjectManagersInput input);

        Task<GetProjectManagerForViewDto> GetProjectManagerForView(long id);

		Task<GetProjectManagerForEditOutput> GetProjectManagerForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditProjectManagerDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetProjectManagersToExcel(GetAllProjectManagersForExcelInput input);

		
    }
}