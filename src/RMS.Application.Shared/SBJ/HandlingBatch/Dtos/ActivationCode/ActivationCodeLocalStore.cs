using System.Collections.Generic;

namespace RMS.SBJ.HandlingBatch.Dtos.ActivationCode
{
    public sealed class ActivationCodeLocalStore
    {
        public long LocaleId { get; set; }

        public string Locale { get; set; }

        public List<long> ActivationCodeIds { get; set; }
    }
}
