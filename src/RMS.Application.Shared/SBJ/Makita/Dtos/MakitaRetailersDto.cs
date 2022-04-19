using System;
using System.Collections.Generic;
using System.Text;

namespace RMS.SBJ.Makita.Dtos
{
    public class MakitaRetailersDto
    {
        public string current_page { get; set; }
        public List<MakitaRetailersDataDto> data { get; set; }
        public string from { get; set; }
        public string last_page { get; set; }
    }
}
