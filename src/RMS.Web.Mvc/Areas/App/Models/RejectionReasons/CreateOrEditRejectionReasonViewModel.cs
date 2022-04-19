using RMS.SBJ.CodeTypeTables.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.RejectionReasons
{
    public class CreateOrEditRejectionReasonModalViewModel
    {
        public CreateOrEditRejectionReasonDto RejectionReason { get; set; }

        public bool IsEditMode => RejectionReason.Id.HasValue;
    }
}