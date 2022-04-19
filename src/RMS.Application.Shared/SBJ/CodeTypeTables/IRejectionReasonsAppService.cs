using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.Dto;
using System.Collections.Generic;

namespace RMS.SBJ.CodeTypeTables
{
    public interface IRejectionReasonsAppService : IApplicationService
    {
        Task<IEnumerable<GetRejectionReasonForViewDto>> GetAll();

        Task<IEnumerable<GetRejectionReasonForViewDto>> GetAllForIncomplete();

        Task<IEnumerable<GetRejectionReasonForViewDto>> GetAllForRejection();

        Task<PagedResultDto<GetRejectionReasonForViewDto>> GetAll(GetAllRejectionReasonsInput input);

        Task<PagedResultDto<GetRejectionReasonForViewDto>> GetAllForIncomplete(GetAllRejectionReasonsInput input);

        Task<PagedResultDto<GetRejectionReasonForViewDto>> GetAllForRejection(GetAllRejectionReasonsInput input);

        Task<GetRejectionReasonForViewDto> GetRejectionReasonForView(long id);

        Task<GetRejectionReasonForEditOutput> GetRejectionReasonForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditRejectionReasonDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetRejectionReasonsToExcel(GetAllRejectionReasonsForExcelInput input);

        GetRejectionReasonForViewDto GetRejectionReason(long id);
    }
}