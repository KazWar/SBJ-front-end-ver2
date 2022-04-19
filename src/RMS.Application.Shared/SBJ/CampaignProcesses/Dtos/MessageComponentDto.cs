
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class MessageComponentDto : EntityDto<long>
    {
		public bool IsActive { get; set; }


		 public long MessageTypeId { get; set; }

		 		 public long MessageComponentTypeId { get; set; }

		 
    }
}