using System;
using System.Collections.Generic;
using System.Text;

namespace RMS.SBJ.Makita.Dtos
{
    public class MakitaApiDto
    {
        public int campaign_id { get; set; }
        public int dealer_id { get; set; }
        public int contact_id { get; set; }
        public string debtor_number { get; set; }        
        public string status { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string gender { get; set; }
        public string email { get; set; }
        public string phone_number { get; set; }
        public string streetname { get; set; }
        public string house_number { get; set; }
        public string city { get; set; }
        public string zip_code { get; set; }
        public string country { get; set; }
        public string coc_number { get; set; }
        public string vat_number { get; set; }
        public string company { get; set; }
        public List<MakitaApiProductsDto> products { get; set; }

    }
}
