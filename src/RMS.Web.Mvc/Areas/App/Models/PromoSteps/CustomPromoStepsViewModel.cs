using RMS.PromoPlanner.Dtos;
using System.Collections.Generic;

namespace RMS.Web.Areas.App.Models.PromoSteps
{
    public class CustomPromoStepsViewModel
    {
        public bool IsEditMode { get; set; }
        public IEnumerable<CustomPromoStepForView> PromoSteps { get; set; }
    }
}
