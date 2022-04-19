using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class GetMessageHistoryForEditOutput
    {
        public CreateOrEditMessageHistoryDto MessageHistory { get; set; }

    }
}