using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.HandlingBatch.Dtos;
using RMS.Dto;

namespace RMS.SBJ.HandlingBatch
{
    public interface IHandlingBatchHistoriesAppService : IApplicationService
    {
        Task<PagedResultDto<GetHandlingBatchHistoryForViewDto>> GetAll(GetAllHandlingBatchHistoriesInput input);

        Task<GetHandlingBatchHistoryForEditOutput> GetHandlingBatchHistoryForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditHandlingBatchHistoryDto input);

        Task Delete(EntityDto<long> input);

        Task<PagedResultDto<HandlingBatchHistoryHandlingBatchLookupTableDto>> GetAllHandlingBatchForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<HandlingBatchHistoryHandlingBatchStatusLookupTableDto>> GetAllHandlingBatchStatusForLookupTable(GetAllForLookupTableInput input);

    }
}