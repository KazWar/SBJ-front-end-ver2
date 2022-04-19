using System;
using System.Collections.Generic;
using System.Text;

namespace RMS.SBJ.Makita.Dtos
{
    public class MakitaCampaignsDto
    {
        public string id { get; set; }
        public string campiagnId { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string start_margin { get; set; }
        public string end_margin { get; set; }
        public string terms { get; set; }
        public string picture { get; set; }

    }
}
