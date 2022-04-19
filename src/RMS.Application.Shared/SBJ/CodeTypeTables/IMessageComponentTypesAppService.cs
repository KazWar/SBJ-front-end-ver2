using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.Dto;
using System.Collections.Generic;

namespace RMS.SBJ.CodeTypeTables
{
    public interface IMessageComponentTypesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetMessageComponentTypeForViewDto>> GetAll(GetAllMessageComponentTypesInput input);

        Task<GetMessageComponentTypeForViewDto> GetMessageComponentTypeForView(long id);

		Task<GetMessageComponentTypeForEditOutput> GetMessageComponentTypeForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditMessageComponentTypeDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetMessageComponentTypesToExcel(GetAllMessageComponentTypesForExcelInput input);

        Task<List<GetMessageComponentTypeForViewDto>> GetAllMessageComponentTypes();
    }
}