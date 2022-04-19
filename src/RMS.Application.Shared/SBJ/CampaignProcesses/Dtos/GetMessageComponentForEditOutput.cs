using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class GetMessageComponentForEditOutput
    {
		public CreateOrEditMessageComponentDto MessageComponent { get; set; }

		public string MessageTypeName { get; set;}

		public string MessageComponentTypeName { get; set;}


    }
}