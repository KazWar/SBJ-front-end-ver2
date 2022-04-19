using System;
using System.Collections.Generic;
using System.Text;

namespace RMS.SBJ.Makita.Dtos
{
    public class MakitaApiProductsDto
    {
        public string product_id { get; set; }
        public string serial_number { get; set; }
        public int choice_id { get; set; }
        public DateTime purchase_date{get; set;}
    }
}
