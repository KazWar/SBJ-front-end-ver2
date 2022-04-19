using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.Dto;
using RMS.SBJ.CampaignProcesses.Dtos;

namespace RMS.SBJ.Makita
{
    public interface IMakitaCampaignsAppService : IApplicationService 
    {
        Task<object> UpdateMakitaCampaigns();

        Task<object> UpdateMakitaRetailers();

        Task<object> UpdateMakitaCampaignProducts();

        Task<PagedResultDto<GetCampaignForViewDto>> GetAll(GetAllCampaignsInput input);

    }
}
