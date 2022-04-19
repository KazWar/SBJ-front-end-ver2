using System;
using System.Collections.Generic;
using System.Text;

namespace RMS.SBJ.ProductHandlings.Dtos
{
    public class GetProductHandelingDto
    {

        public long ProductId { get; set; }
        public long? ProductCategoryId { get; set; }
        public string Name { get; set; }
        public string CTN { get; set; }
        public string EAN { get; set; }
        public List<long> HandlingLineIds { get; set; }


    }
}
