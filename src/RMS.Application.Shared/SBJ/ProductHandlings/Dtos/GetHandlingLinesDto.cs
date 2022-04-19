using System;
using System.Collections.Generic;
using System.Text;

namespace RMS.SBJ.ProductHandlings.Dtos
{
    public class GetHandlingLinesDto
    {
        public long ProductHandlingId { get; set; }
        public decimal? MinimumPurchaseAmount { get; set; }
        public decimal? MaximumPurchaseAmount { get; set; }
        public string CustomerCode { get; set; }
        public decimal? Amount { get; set; }
        public bool Fixed { get; set; }
        public bool ActivationCode { get; set; }

    }
}
