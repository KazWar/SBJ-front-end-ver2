using System.Collections.Generic;

namespace RMS.SBJ.Forms.Dtos
{
    public class UpdateSortOrderDto
    {
        public IEnumerable<UpdateSortOrderDetailsDto> EditedSortOrder { get; set; }
    }
}
