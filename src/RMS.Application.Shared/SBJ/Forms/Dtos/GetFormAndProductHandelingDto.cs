using RMS.SBJ.ProductHandlings.Dtos;

namespace RMS.SBJ.Forms.Dtos
{
    public class GetFormAndProductHandelingDto
    {
        public FormExportJsonDto Formbuilder { get; set; }
        public GetProductHandlingForApiDto ProductHandling { get; set; }
    }
}