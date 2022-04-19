using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.Forms.Dtos;
using RMS.Dto;


namespace RMS.SBJ.Forms
{
    public interface IListValuesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetListValueForViewDto>> GetAll(GetAllListValuesInput input);

        Task<GetListValueForViewDto> GetListValueForView(long id);

		Task<GetListValueForEditOutput> GetListValueForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditListValueDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetListValuesToExcel(GetAllListValuesForExcelInput input);

		
		Task<PagedResultDto<ListValueValueListLookupTableDto>> GetAllValueListForLookupTable(GetAllForLookupTableInput input);
		
    }
}