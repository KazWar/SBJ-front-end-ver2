using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.Forms.Dtos;
using RMS.Dto;


namespace RMS.SBJ.Forms
{
    public interface IFormFieldValueListsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetFormFieldValueListForViewDto>> GetAll(GetAllFormFieldValueListsInput input);

        Task<GetFormFieldValueListForViewDto> GetFormFieldValueListForView(long id);

		Task<GetFormFieldValueListForEditOutput> GetFormFieldValueListForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditFormFieldValueListDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetFormFieldValueListsToExcel(GetAllFormFieldValueListsForExcelInput input);

		
		Task<PagedResultDto<FormFieldValueListFormFieldLookupTableDto>> GetAllFormFieldForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<FormFieldValueListValueListLookupTableDto>> GetAllValueListForLookupTable(GetAllForLookupTableInput input);
		
    }
}