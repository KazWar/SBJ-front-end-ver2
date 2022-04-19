using Abp.Application.Services.Dto;

namespace RMS.SBJ.Retailers.Dtos
{
    public class RetailerDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public long? CountryId { get; set; }
    }
}