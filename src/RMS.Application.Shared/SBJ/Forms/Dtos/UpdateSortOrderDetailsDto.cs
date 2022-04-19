namespace RMS.SBJ.Forms.Dtos
{
    public class UpdateSortOrderDetailsDto
    {
        public long BlockId { get; set; }
        public long FieldId { get; set; }

        public int SortOrderBlock { get; set; }
        public int SortOrderField { get; set; }
    }
}
