using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.Forms.Dtos;
using RMS.Dto;


namespace RMS.SBJ.Forms
{
    public interface IValueListsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetValueListForViewDto>> GetAll(GetAllValueListsInput input);

        Task<GetValueListForViewDto> GetValueListForView(long id);

		Task<GetValueListForEditOutput> GetValueListForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditValueListDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetValueListsToExcel(GetAllValueListsForExcelInput input);

		
    }
}