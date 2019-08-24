using fAzureHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGenealogie.Console
{
    public class PersonDBInAzureStorage
    {
        ContainerManager _containerManager;
        string _dbPath;
        public Persons Persons;
        string _storageName;
            string _storageKey;

        public PersonDBInAzureStorage(string dbPath, string storageName, string storageKey)
        {
            this._storageKey = storageKey;
            this._storageName = storageName;
            this._containerManager = new ContainerManager(storageName, storageKey);
            this._dbPath = dbPath;
            
        }

        public void Upload()
        {
            var containers = this._containerManager.GetContainerList();
            foreach(var p in Persons)
            {
                var containerName = p.GetFolderName();
                this.Trace($"Create container {containerName}");
                this._containerManager.CreateContainer(containerName);

                this.Trace($"Upload json metadata file {p.GetPropertiesJsonFile()}");
                var bm = new BlobManager(this._storageName, this._storageKey, containerName);
                bm.UploadJsonFileAsync(p.GetPropertiesJsonFile(), overide:true).GetAwaiter().GetResult();

                p.LoadImages();
                foreach (var image in p.Properties.Images)
                {
                    this.Trace($"Upload image {image.LocalFileName}");
                    bm.UploadJpegFileAsync(image.LocalFileName, overide: true).GetAwaiter().GetResult();
                }

                this.Trace($"");
            }
        }

        private void Trace(string m)
        {
            System.Console.WriteLine(m);
        }

        public void LoadPersonsFromAzureStorageDB()
        {
            this.Persons = new Persons(PersonDBSource.AZURE_STORAGE);
            var containers = this._containerManager.GetContainerList();
            foreach (var containerName in containers)
            {
                this.Trace($"Loading container {containerName}");
                var bm = new BlobManager(this._storageName, this._storageKey, containerName);
                var tmpJsonFile = Path.GetTempFileName();
                File.Delete(tmpJsonFile);
                bm.DownloadFileAsync("p.json", tmpJsonFile).GetAwaiter().GetResult();
                var p = Person.LoadFromJsonFile(null, tmpJsonFile, PersonDBSource.AZURE_STORAGE);
                this.Persons.Add(p);
                p.LoadImages();
                this.Trace($"");
            }
        }

        public void LoadPersonsFromLocalDB()
        {
            var personFolders = System.IO.Directory.GetDirectories(this._dbPath);
            this.Persons = new Persons(PersonDBSource.LOCAL_FILE_SYSTEM);

            foreach (var personFolder in personFolders)
            {
                var p = Person.LoadFromFolder(personFolder);
                this.Persons.Add(p);
            }
        }
    }
}
