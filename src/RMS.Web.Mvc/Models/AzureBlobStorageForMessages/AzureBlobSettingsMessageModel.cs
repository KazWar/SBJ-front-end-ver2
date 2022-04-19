using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMS.Web.Models.AzureBlobStorageForMessages
{
    public class AzureBlobSettingsMessageModel
    {
        public string RMSBlobStorage { get; set; }

        public string DefaultBlobContainer { get; set; }
    }
}
