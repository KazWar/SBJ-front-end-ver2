
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.CodeTypeTables.Dtos
{
    public class CampaignCategoryDto : EntityDto<long>
    {
		public string Name { get; set; }

		public bool IsActive { get; set; }

		public int SortOrder { get; set; }



    }
}