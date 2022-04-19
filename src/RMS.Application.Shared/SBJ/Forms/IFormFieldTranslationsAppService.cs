using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.Forms.Dtos;
using RMS.Dto;
using System.Collections.Generic;

namespace RMS.SBJ.Forms
{
    public interface IFormFieldTranslationsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetFormFieldTranslationForViewDto>> GetAll(GetAllFormFieldTranslationsInput input);

        Task<GetFormFieldTranslationForViewDto> GetFormFieldTranslationForView(long id);

		Task<GetFormFieldTranslationForEditOutput> GetFormFieldTranslationForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditFormFieldTranslationDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetFormFieldTranslationsToExcel(GetAllFormFieldTranslationsForExcelInput input);

		
		Task<PagedResultDto<FormFieldTranslationFormFieldLookupTableDto>> GetAllFormFieldForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<FormFieldTranslationLocaleLookupTableDto>> GetAllLocaleForLookupTable(GetAllForLookupTableInput input);

		Task<List<GetFormFieldTranslationForViewDto>> GetAllFormFieldTranslations();
	}
}