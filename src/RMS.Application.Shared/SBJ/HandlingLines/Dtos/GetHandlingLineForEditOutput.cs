using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.HandlingLines.Dtos
{
    public class GetHandlingLineForEditOutput
    {
        public CreateOrEditHandlingLineDto HandlingLine { get; set; }

        public string CampaignTypeName { get; set; }

        public string ProductHandlingDescription { get; set; }

    }
}