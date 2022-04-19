namespace RMS.SBJ.Forms.Dtos
{
    public class GetFormBlockFieldForViewDto
    {
		public FormBlockFieldDto FormBlockField { get; set; }

        public GetFormBlockFieldForEditDto FormBlockFieldForEdit { get; set; }

        public string FormFieldDescription { get; set;}

		public string FormBlockDescription { get; set;}

        public long FormFieldTranslationId { get; set; }

        public int FormBlockSortOrder { get; set; }
    }
}