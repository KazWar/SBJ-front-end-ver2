using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RMS.SBJ.CampaignProcesses;
using RMS.SBJ.CampaignProcesses.Dtos;

namespace RMS.Web.Mvc.Services
{
    public class FrontEndAppService : RMSAppServiceBase, IFrontEndAppService
    {
        private readonly ICampaignsAppService _campaignsAppService;
        private readonly ICampaignFormsAppService _campaignFormsAppService;

        public FrontEndAppService(
            ICampaignsAppService campaignsAppService,
            ICampaignFormsAppService campaignFormsAppService)
        {
            _campaignsAppService = campaignsAppService;
            _campaignFormsAppService = campaignFormsAppService;
        }
        
        /// <summary>
        /// Gets all campaign records, with start- and/or end date as optional parameters.
        /// </summary>
        /// <param name="startDate">The start date of the campaign.</param>
        /// <param name="endDate">The end date of the campaign.</param>
        /// <returns>A <see cref="JsonResult"/> with all campaigns that satisfy the given conditions.</returns>
        public async Task<IReadOnlyList<GetCampaignForViewDto>> GetAllCampaigns(DateTime? startDate = null, DateTime? endDate = null)
        {
            var campaigns = await _campaignsAppService.GetAll(new GetAllCampaignsInput {
                MinStartDateFilter = startDate, 
                MaxEndDateFilter = endDate 
            });

            return campaigns.Items;
        }

        /// <summary>
        /// Gets the latest version of a <see cref="CampaignForm"/> for a specified <see cref="Campaign"/>.
        /// </summary>
        /// <param name="campaignId">The campaign ID to use for filtering.</param>
        /// <returns>A <see cref="JsonResult"/> with the latest <see cref="CampaignForm"/> that satisfies the given condition.</returns>
        public async Task<GetCampaignFormForViewDto> GetLatestCampaignFormForCampaign(long campaignId)
        {
            List<GetCampaignFormForViewDto> campaignForms = await _campaignFormsAppService.GetAllCampaignForms();
            GetCampaignFormForViewDto latestCampaignForm = campaignForms.LastOrDefault(x => x.CampaignForm.CampaignId == campaignId);

            return latestCampaignForm;
        }
    }
}
