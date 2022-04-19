using System;
using System.Collections.Generic;
using System.Text;

namespace RMS.SBJ.Makita.Dtos
{
    public class MakitaRetailersDataDto
    {
        public string id { get; set; }
        public string debiteur_nr { get; set; }
        public string bedrijfsnaam { get; set; }
        public string adres { get; set; }
        public string huisnummer { get; set; }
        public string postcode { get; set; }
        public string plaats { get; set; }
        public string volgnr { get; set; }
        public string deelnemer_eindgebruiker_actie { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string cond_brief_verzenden { get; set; }
        public string participating { get; set; }
        public MakitaRetailerAdressesDropOffDto drop_off { get; set; }
    }
}
