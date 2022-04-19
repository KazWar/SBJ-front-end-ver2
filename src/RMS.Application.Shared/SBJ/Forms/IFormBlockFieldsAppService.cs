using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.Forms.Dtos;
using RMS.Dto;
using System.Collections.Generic;

namespace RMS.SBJ.Forms
{
    public interface IFormBlockFieldsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetFormBlockFieldForViewDto>> GetAll(GetAllFormBlockFieldsInput input);

        Task<GetFormBlockFieldForViewDto> GetFormBlockFieldForView(long id);

		Task<GetFormBlockFieldForEditOutput> GetFormBlockFieldForEdit(EntityDto<long> input);

		Task<GetFormBlockFieldForEditDto> GetFormBlockFieldForEdit(long fieldId, long blockId, long localeId);

		Task<bool> UpdateSortOrder(UpdateSortOrderDto input);

		Task<bool> AddFormFieldToFormBlock(long fieldId, long blockId);

		Task<bool> RemoveFormFieldFromFormBlock(long fieldId, long blockId);

		Task CreateOrEdit(CreateOrEditFormBlockFieldDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetFormBlockFieldsToExcel(GetAllFormBlockFieldsForExcelInput input);
	
		Task<PagedResultDto<FormBlockFieldFormFieldLookupTableDto>> GetAllFormFieldForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<FormBlockFieldFormBlockLookupTableDto>> GetAllFormBlockForLookupTable(GetAllForLookupTableInput input);

		Task<List<GetFormBlockFieldForViewDto>> GetAllFormBlockFields(long? localeId);
	}
}