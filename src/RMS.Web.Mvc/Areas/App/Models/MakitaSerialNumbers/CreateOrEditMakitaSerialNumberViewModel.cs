using RMS.SBJ.MakitaSerialNumber.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.MakitaSerialNumbers
{
    public class CreateOrEditMakitaSerialNumberModalViewModel
    {
        public CreateOrEditMakitaSerialNumberDto MakitaSerialNumber { get; set; }

        public bool IsEditMode => MakitaSerialNumber.Id.HasValue;
    }
}