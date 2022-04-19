using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.Forms.Dtos;
using RMS.Dto;


namespace RMS.SBJ.Forms
{
    public interface IFieldTypesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetFieldTypeForViewDto>> GetAll(GetAllFieldTypesInput input);

        Task<GetFieldTypeForViewDto> GetFieldTypeForView(long id);

		Task<GetFieldTypeForEditOutput> GetFieldTypeForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditFieldTypeDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetFieldTypesToExcel(GetAllFieldTypesForExcelInput input);

		
    }
}