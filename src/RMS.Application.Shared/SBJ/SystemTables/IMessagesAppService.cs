using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.SystemTables.Dtos;
using RMS.Dto;
using System.Collections.Generic;

namespace RMS.SBJ.SystemTables
{
    public interface IMessagesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetMessageForViewDto>> GetAll(GetAllMessagesInput input);

        Task<GetMessageForViewDto> GetMessageForView(long id);

		Task<GetMessageForEditOutput> GetMessageForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditMessageDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetMessagesToExcel(GetAllMessagesForExcelInput input);

        Task<List<GetMessageForViewDto>> GetAllMessages();

        Task<PagedResultDto<MessageSystemLevelLookupTableDto>> GetAllSystemLevelForLookupTable(GetAllForLookupTableInput input);
    }
}