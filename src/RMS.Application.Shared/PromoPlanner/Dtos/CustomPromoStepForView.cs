using System.Collections.Generic;

namespace RMS.PromoPlanner.Dtos
{
    public class CustomPromoStepForView
    {
        // PK PromoStepData
        public int? PromoStepId { get; set; }

        // PK PromoStep
        public int StepId { get; set; }

        public short Sequence { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }
        
        public IEnumerable<CustomPromoStepFieldForView> PromoStepFields { get; set; }
    }
}
