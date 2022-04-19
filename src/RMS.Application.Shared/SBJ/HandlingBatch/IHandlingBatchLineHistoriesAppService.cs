using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.HandlingBatch.Dtos;
using RMS.Dto;

namespace RMS.SBJ.HandlingBatch
{
    public interface IHandlingBatchLineHistoriesAppService : IApplicationService
    {
        Task<PagedResultDto<GetHandlingBatchLineHistoryForViewDto>> GetAll(GetAllHandlingBatchLineHistoriesInput input);

        Task<GetHandlingBatchLineHistoryForEditOutput> GetHandlingBatchLineHistoryForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditHandlingBatchLineHistoryDto input);

        Task Delete(EntityDto<long> input);

        Task<PagedResultDto<HandlingBatchLineHistoryHandlingBatchLineLookupTableDto>> GetAllHandlingBatchLineForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<HandlingBatchLineHistoryHandlingBatchLineStatusLookupTableDto>> GetAllHandlingBatchLineStatusForLookupTable(GetAllForLookupTableInput input);

    }
}