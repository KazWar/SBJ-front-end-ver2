using System;
using System.Collections.Generic;
using System.Text;

namespace RMS.SBJ.Makita.Dtos
{
    public class MakitaCampaignRetailersDto
    {
        public string id { get; set; }
        public string debtor_number { get; set; }
        public string company_name { get; set; }
        public MakitaRetailerAdressesDto addresses { get; set; }
    }
}
