using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace fAzureHelper
{
    public class ContainerManager : AzureStorageBaseClass
    {
        private CloudBlobClient _cloudBlobClient = null;

        public ContainerManager(string storageAccountName, string storageAccessKey) : base(storageAccountName, storageAccessKey)
        {
            this._cloudBlobClient = _storageAccount.CreateCloudBlobClient();
        }
        public IEnumerable<string> GetContainerList()
        {
            var containers = _cloudBlobClient.ListContainers().ToList();
            return containers.Select(c => c.Name);
        }
        public void CreateContainer(string name)
        {
            var container = _cloudBlobClient.GetContainerReference(name);
            //BlobContainerPublicAccessType accessType, BlobRequestOptions requestOptions = null, OperationContext operationContext = null
            container.CreateIfNotExists(BlobContainerPublicAccessType.Container);
        }
    }
}