using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class CampaignDto : EntityDto<long>
    {
		public string Name { get; set; }

		public string Description { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

		public int? CampaignCode { get; set; }

		public string ExternalCode { get; set; }

		public string ExternalId { get; set; }

		public string ThumbnailImagePath { get; set; }

		public string BannerImagePath { get; set; }
	}
}