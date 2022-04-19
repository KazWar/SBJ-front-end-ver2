﻿using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class GetAllCampaignCountriesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string CampaignNameFilter { get; set; }

        public string CountryDescriptionFilter { get; set; }

    }
}