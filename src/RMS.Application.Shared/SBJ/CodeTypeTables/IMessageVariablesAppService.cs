using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.Dto;
using System.Collections.Generic;

namespace RMS.SBJ.CodeTypeTables
{
    public interface IMessageVariablesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetMessageVariableForViewDto>> GetAll(GetAllMessageVariablesInput input);

        Task<GetMessageVariableForViewDto> GetMessageVariableForView(long id);

		Task<GetMessageVariableForEditOutput> GetMessageVariableForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditMessageVariableDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetMessageVariablesToExcel(GetAllMessageVariablesForExcelInput input);

        Task<List<GetMessageVariableForViewDto>> GetAllMessageVariables();
    }
}