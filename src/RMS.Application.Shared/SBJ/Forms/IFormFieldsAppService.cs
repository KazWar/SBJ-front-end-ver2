using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.Forms.Dtos;
using RMS.Dto;
using System.Collections.Generic;

namespace RMS.SBJ.Forms
{
	public interface IFormFieldsAppService : IApplicationService
	{
		Task<PagedResultDto<GetFormFieldForViewDto>> GetAll(GetAllFormFieldsInput input);

		Task<GetFormFieldForViewDto> GetFormFieldForView(long id);

		Task<GetFormFieldForEditOutput> GetFormFieldForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditFormFieldDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetFormFieldsToExcel(GetAllFormFieldsForExcelInput input);

		Task<PagedResultDto<FormFieldFieldTypeLookupTableDto>> GetAllFieldTypeForLookupTable(GetAllForLookupTableInput input);

		Task<List<GetFormFieldForViewDto>> GetAllFormFields();

		Task<bool> UpdateFormFields(UpdateFormFieldsDto input);
	}
}