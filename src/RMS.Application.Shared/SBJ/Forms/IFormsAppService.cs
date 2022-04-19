using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.Forms.Dtos;
using RMS.Dto;
using System.Collections.Generic;

namespace RMS.SBJ.Forms
{
    public interface IFormsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetFormForViewDto>> GetAll(GetAllFormsInput input);

        Task<GetFormForViewDto> GetFormForView(long id);

		Task<GetFormForEditOutput> GetFormForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditFormDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetFormsToExcel(GetAllFormsForExcelInput input);

		
		Task<PagedResultDto<FormSystemLevelLookupTableDto>> GetAllSystemLevelForLookupTable(GetAllForLookupTableInput input);

		Task<List<GetFormForViewDto>> GetAllForms();
	}
}