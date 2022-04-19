using Abp.IO.Extensions;
using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RMS.AzureBlobStorage
{
    public class AzureBlobStorageAppService : IAzureBlobStorageAppService
    {
       

        public async Task<string> UploadBinaryAsync(string azureStorageConnection, string containerName, byte[] bytes, string fileName)
        {
            var blobContainerClient = new BlobContainerClient(azureStorageConnection, containerName);

       
            var extension = fileName.Substring(fileName.LastIndexOf('.'));

            var searchName = fileName.Substring(0, fileName.LastIndexOf('_'));

            var newSequenceNumber = 1;
            var sequencePositions = 3;

            var listBlobItems = blobContainerClient.GetBlobs(Azure.Storage.Blobs.Models.BlobTraits.None, Azure.Storage.Blobs.Models.BlobStates.None, searchName).ToList();
            if (listBlobItems.Count() != 0)
            {
                var lastBlobItem = listBlobItems.Last();
                var lastFileName = lastBlobItem.Name;

                var positionLastSeperator = lastFileName.LastIndexOf('_');
                if (positionLastSeperator !=0)
                {
                    var positionLastPeriod = lastFileName.LastIndexOf('.');
                    var sequenceNumberString = lastFileName.Substring(positionLastSeperator + 1, (positionLastPeriod - positionLastSeperator) - 1);
                    sequencePositions = sequenceNumberString.Length;

                    if (int.TryParse(sequenceNumberString, out int sequenceNumber))
                    {
                        newSequenceNumber = sequenceNumber + 1;
                    }
                }

            }

            var adjustedFileName = $"{searchName}_{newSequenceNumber.ToString(new string('0', sequencePositions))}{extension}";

            await blobContainerClient.UploadBlobAsync(adjustedFileName, new MemoryStream(bytes));
            return blobContainerClient.Uri.ToString();
        }

        public async Task<byte[]> DownloadBinaryAsync(string azureStorageConnection, string containerName, string fileName)
        {
            var blobContainerClient = new BlobContainerClient(azureStorageConnection, containerName);
            var blob =  blobContainerClient.GetBlobClient(fileName);
            var memStream = new MemoryStream();

            await blob.DownloadToAsync(memStream);

            return memStream.GetAllBytes();
        }

        public async Task<string> DownloadBase64StringAsync(string azureStorageConnection, string containerName, string fileName)
        {
            var blobContainerClient = new BlobContainerClient(azureStorageConnection, containerName);
            var blob = blobContainerClient.GetBlobClient(fileName);
            var memStream = new MemoryStream();

            await blob.DownloadToAsync(memStream);

            memStream.Position = 0;

            var resultString = string.Empty;
            var contentString = Convert.ToBase64String(memStream.GetAllBytes());           
            var fileExtension = fileName.Split('.').Last().ToLower().Trim();

            switch (fileExtension)
            {
                case "pdf":
                    resultString = $"data:application/pdf;base64,{contentString}";
                    break;
                case "svg":
                    resultString = $"data:image/svg+xml;base64,{contentString}";
                    break;
                default:
                    resultString = $"data:image/{fileExtension};base64,{contentString}";
                    break;
            }

            return resultString;
        }
    }
}
