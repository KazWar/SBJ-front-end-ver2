using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.HandlingBatch.Dtos;

namespace RMS.SBJ.HandlingBatch
{
    public interface IHandlingBatchLineStatusesAppService : IApplicationService
    {
        Task<GetHandlingBatchLineStatusForViewDto> GetById(long id);

        Task<GetHandlingBatchLineStatusForViewDto> GetByStatusCode(string statusCode);

        Task<PagedResultDto<GetHandlingBatchLineStatusForViewDto>> GetAll(GetAllHandlingBatchLineStatusesInput input);

        Task<GetHandlingBatchLineStatusForEditOutput> GetHandlingBatchLineStatusForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditHandlingBatchLineStatusDto input);

        Task Delete(EntityDto<long> input);

    }
}