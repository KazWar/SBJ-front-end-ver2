using RMS.SBJ.ProductHandlings.Dtos;

namespace RMS.SBJ.Forms.Dtos
{
    public class GetFormLayoutAndDataDto
    {
        public FormExportJsonDto Formbuilder { get; set; }

        public GetProductHandlingForApiDto ProductHandling { get; set; }
    }
}
