using Abp.Application.Services.Dto;

namespace RMS.SBJ.CodeTypeTables.Dtos
{
    public class RegistrationStatusDto : EntityDto<long>
    {
        public string StatusCode { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}