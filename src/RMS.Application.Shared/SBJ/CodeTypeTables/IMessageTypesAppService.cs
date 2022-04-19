using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.Dto;
using System.Collections.Generic;

namespace RMS.SBJ.CodeTypeTables
{
    public interface IMessageTypesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetMessageTypeForViewDto>> GetAll(GetAllMessageTypesInput input);

        Task<GetMessageTypeForViewDto> GetMessageTypeForView(long id);

		Task<GetMessageTypeForEditOutput> GetMessageTypeForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditMessageTypeDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetMessageTypesToExcel(GetAllMessageTypesForExcelInput input);

        Task<List<GetMessageTypeForViewDto>> GetAllMessageTypes();
    }
}