using Abp.Application.Services.Dto;

namespace RMS.PromoPlanner.Dtos
{
    public class PromoScopeDto : EntityDto<long>
    {
		public string Description { get; set; }
    }
}