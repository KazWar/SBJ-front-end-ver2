using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using RMS.SBJ.CampaignProcesses.Dtos;

namespace RMS.Web.Mvc.Services
{
    public interface IFrontEndAppService : IApplicationService
    {
        Task<IReadOnlyList<GetCampaignForViewDto>> GetAllCampaigns(DateTime? startDate = null, DateTime? endDate = null);

        Task<GetCampaignFormForViewDto> GetLatestCampaignFormForCampaign(long campaignId);
    }
}
