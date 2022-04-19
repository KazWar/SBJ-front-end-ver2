using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.Forms.Dtos;
using RMS.Dto;


namespace RMS.SBJ.Forms
{
    public interface IListValueTranslationsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetListValueTranslationForViewDto>> GetAll(GetAllListValueTranslationsInput input);

        Task<GetListValueTranslationForViewDto> GetListValueTranslationForView(long id);

		Task<GetListValueTranslationForEditOutput> GetListValueTranslationForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditListValueTranslationDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetListValueTranslationsToExcel(GetAllListValueTranslationsForExcelInput input);

		
		Task<PagedResultDto<ListValueTranslationListValueLookupTableDto>> GetAllListValueForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ListValueTranslationLocaleLookupTableDto>> GetAllLocaleForLookupTable(GetAllForLookupTableInput input);
		
    }
}