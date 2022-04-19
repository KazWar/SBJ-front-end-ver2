using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.RegistrationHistory.Dtos;
using RMS.Dto;

namespace RMS.SBJ.RegistrationHistory
{
    public interface IRegistrationHistoriesAppService : IApplicationService
    {
        Task<PagedResultDto<GetRegistrationHistoryForViewDto>> GetAll(GetAllRegistrationHistoriesInput input);

        Task<GetRegistrationHistoryForViewDto> GetRegistrationHistoryForView(long id);

        Task<GetRegistrationHistoryForEditOutput> GetRegistrationHistoryForEdit(EntityDto<long> input);

        Task CreateNew(CreateOrEditRegistrationHistoryDto input);

        Task CreateOrEdit(CreateOrEditRegistrationHistoryDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetRegistrationHistoriesToExcel(GetAllRegistrationHistoriesForExcelInput input);

        Task<PagedResultDto<RegistrationHistoryRegistrationStatusLookupTableDto>> GetAllRegistrationStatusForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<RegistrationHistoryRegistrationLookupTableDto>> GetAllRegistrationForLookupTable(GetAllForLookupTableInput input);

    }
}