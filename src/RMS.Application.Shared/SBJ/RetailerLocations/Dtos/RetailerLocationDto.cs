using Abp.Application.Services.Dto;

namespace RMS.SBJ.RetailerLocations.Dtos
{
    public class RetailerLocationDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }

        public string ExternalCode { get; set; }

        public string ExternalId { get; set; }

        public long RetailerId { get; set; }
    }
}