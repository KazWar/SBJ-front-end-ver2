using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.HandlingBatch.Dtos;
using RMS.Dto;

namespace RMS.SBJ.HandlingBatch
{
    public interface IHandlingBatchLinesAppService : IApplicationService
    {
        Task<PagedResultDto<GetHandlingBatchLineForViewDto>> GetAll(GetAllHandlingBatchLinesInput input);

        Task<GetHandlingBatchLineForViewDto> GetHandlingBatchLineForView(long id);

        Task<GetHandlingBatchLineForEditOutput> GetHandlingBatchLineForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditHandlingBatchLineDto input);

        Task Delete(EntityDto<long> input);

        Task<PagedResultDto<HandlingBatchLineHandlingBatchLookupTableDto>> GetAllHandlingBatchForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<HandlingBatchLinePurchaseRegistrationLookupTableDto>> GetAllPurchaseRegistrationForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<HandlingBatchLineHandlingBatchLineStatusLookupTableDto>> GetAllHandlingBatchLineStatusForLookupTable(GetAllForLookupTableInput input);

    }
}