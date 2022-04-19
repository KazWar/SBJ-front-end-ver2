using System.Threading.Tasks;
using System.Collections.Generic;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.PromoPlanner.Dtos;
using RMS.Dto;

namespace RMS.PromoPlanner
{
    public interface IPromoScopesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPromoScopeForViewDto>> GetAll(GetAllPromoScopesInput input);

        Task<IEnumerable<GetPromoScopeForViewDto>> GetAllWithoutPaging();

        Task<GetPromoScopeForViewDto> GetPromoScopeForView(long id);

		Task<GetPromoScopeForEditOutput> GetPromoScopeForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditPromoScopeDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetPromoScopesToExcel(GetAllPromoScopesForExcelInput input);

		
    }
}