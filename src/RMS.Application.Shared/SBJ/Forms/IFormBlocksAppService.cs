using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.Forms.Dtos;
using RMS.Dto;
using System.Collections.Generic;

namespace RMS.SBJ.Forms
{
    public interface IFormBlocksAppService : IApplicationService 
    {
        Task<PagedResultDto<GetFormBlockForViewDto>> GetAll(GetAllFormBlocksInput input);

        Task<GetFormBlockForViewDto> GetFormBlockForView(long id);

		Task<GetFormBlockForEditOutput> GetFormBlockForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditFormBlockDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetFormBlocksToExcel(GetAllFormBlocksForExcelInput input);

		
		Task<PagedResultDto<FormBlockFormLocaleLookupTableDto>> GetAllFormLocaleForLookupTable(GetAllForLookupTableInput input);

		Task<List<GetFormBlockForViewDto>> GetAllFormBlocks();

		Task<long> CreateOrEditAndGetId(CreateOrEditFormBlockDto input);
	}
}