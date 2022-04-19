using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.Dto;

namespace RMS.SBJ.CodeTypeTables
{
    public interface IRegistrationStatusesAppService : IApplicationService
    {
	    Task<GetRegistrationStatusForViewDto> GetByStatusCode(string statusCode);

        Task<PagedResultDto<GetRegistrationStatusForViewDto>> GetAll(GetAllRegistrationStatusesInput input);

        Task<GetRegistrationStatusForViewDto> GetRegistrationStatusForView(long id);

		Task<GetRegistrationStatusForEditOutput> GetRegistrationStatusForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditRegistrationStatusDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetRegistrationStatusesToExcel(GetAllRegistrationStatusesForExcelInput input);		
    }
}