using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMS.Web.Models.AzureBlobStorage
{
    public class AzureBlobStorageSettingsModel
    {
        public string RMSBlobStorage { get; set; }

        public string DefaultBlobContainer { get; set; }

    }
}
