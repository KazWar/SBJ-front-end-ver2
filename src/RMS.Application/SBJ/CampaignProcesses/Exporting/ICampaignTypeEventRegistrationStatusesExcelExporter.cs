﻿using System.Collections.Generic;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.Dto;

namespace RMS.SBJ.CampaignProcesses.Exporting
{
    public interface ICampaignTypeEventRegistrationStatusesExcelExporter
    {
        FileDto ExportToFile(List<GetCampaignTypeEventRegistrationStatusForViewDto> campaignTypeEventRegistrationStatuses);
    }
}