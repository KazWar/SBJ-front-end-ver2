using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.HandlingBatch.Dtos;

namespace RMS.SBJ.HandlingBatch
{
    public interface IHandlingBatchStatusesAppService : IApplicationService
    {
        Task<GetHandlingBatchStatusForViewDto> GetById(long id);

        Task<GetHandlingBatchStatusForViewDto> GetByStatusCode(string statusCode);

        Task<PagedResultDto<GetHandlingBatchStatusForViewDto>> GetAll(GetAllHandlingBatchStatusesInput input);

        Task<GetHandlingBatchStatusForEditOutput> GetHandlingBatchStatusForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditHandlingBatchStatusDto input);

        Task Delete(EntityDto<long> input);

    }
}