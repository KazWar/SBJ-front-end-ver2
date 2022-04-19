using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.HandlingLineLogics.Dtos;
using RMS.Dto;


namespace RMS.SBJ.HandlingLineLogics
{
    public interface IHandlingLineLogicsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetHandlingLineLogicForViewDto>> GetAll(GetAllHandlingLineLogicsInput input);

		Task<GetHandlingLineLogicForEditOutput> GetHandlingLineLogicForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditHandlingLineLogicDto input);

		Task Delete(EntityDto<long> input);

		
    }
}