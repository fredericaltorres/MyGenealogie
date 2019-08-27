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
            container.CreateIfNotExists(BlobContainerPublicAccessType.Container);
        }
        public void DeleteContainer(string name)
        {
            var container = _cloudBlobClient.GetContainerReference(name);
            if (container.Properties.LeaseState == LeaseState.Leased)
            {
                container.BreakLease(null);
            }
            var e = container.Exists();
            container.Delete();
        }
        public List<string> GetFiles(string name, string extension)
        {
            extension = extension.ToLowerInvariant();
            var container = _cloudBlobClient.GetContainerReference(name);
            var list = container.ListBlobs();
            var files = list.OfType<CloudBlockBlob>().Where(f => f.Name.ToLowerInvariant().EndsWith(extension)).Select(b => b.Name).ToList();
            return files;
        }
    }
}