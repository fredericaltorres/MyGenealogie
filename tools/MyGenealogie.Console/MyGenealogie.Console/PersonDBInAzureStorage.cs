using fAzureHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGenealogie.Console
{
    class PersonDBInAzureStorage
    {
        ContainerManager _containerManager;
        public PersonDBInAzureStorage(string storageName, string storageKey)
        {
            this._containerManager = new ContainerManager(storageName, storageKey);
        }

        internal void Upload(string dbPath)
        {
            var containers = this._containerManager.GetContainerList();
        }
    }
}
