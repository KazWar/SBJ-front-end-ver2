using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.Dto;


namespace RMS.SBJ.CampaignProcesses
{
    public interface IMessageComponentsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetMessageComponentForViewDto>> GetAll(GetAllMessageComponentsInput input);

        Task<PagedResultDto<GetMessageComponentForViewDto>> GetMessageComponentsByMessageTypeId(long messageTypeId, string messageTypeName, long messageId);

        Task<GetMessageComponentForViewDto> GetMessageComponentForView(long id);

		Task<GetMessageComponentForEditOutput> GetMessageComponentForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditMessageComponentDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetMessageComponentsToExcel(GetAllMessageComponentsForExcelInput input);

        Task<PagedResultDto<GetMessageComponentForViewDto>> GetAllMessageComponents();
    }
}