using Abp.Application.Services.Dto;

namespace RMS.SBJ.Forms.Dtos
{
    public class GetFormIbanBicDto
    {
        public string Iban { get; set; }

        public string Bic { get; set; }
    }
}

