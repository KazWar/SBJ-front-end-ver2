using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.Dto;

namespace RMS.SBJ.CampaignProcesses
{
    public interface IMessageHistoriesAppService : IApplicationService
    {
        Task<PagedResultDto<GetMessageHistoryForViewDto>> GetAll(GetAllMessageHistoriesInput input);

        Task<GetMessageHistoryForEditOutput> GetMessageHistoryForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditMessageHistoryDto input);

        Task Delete(EntityDto<long> input);

    }
}