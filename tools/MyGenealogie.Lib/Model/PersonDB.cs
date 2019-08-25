using fAzureHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGenealogie.Console
{
    public class PersonDB : IPersonDB
    {
        const string personDBContainer = "person-db";
        ContainerManager _containerManager;
        string _dbPath;
        public Persons Persons;
        string _storageName;
        string _storageKey;

        public PersonDB(string dbPath, string storageName, string storageKey)
        {
            this._storageKey = storageKey;
            this._storageName = storageName;
            this._containerManager = new ContainerManager(storageName, storageKey);
            this._dbPath = dbPath;
        }

        public void Upload()
        {
            foreach (var p in Persons)
            {
                var containerName = p.GetFolderName();
                this.Trace($"Create container {containerName}");
                this._containerManager.CreateContainer(containerName);

                this.Trace($"Upload json metadata file {p.GetPropertiesJsonFile()}");
                var bm = new BlobManager(this._storageName, this._storageKey, containerName);
                bm.UploadJsonFileAsync(p.GetPropertiesJsonFile(), overide: true).GetAwaiter().GetResult();

                p.LoadImages();
                foreach (var image in p.Properties.Images)
                {
                    this.Trace($"Upload image {image.LocalFileName}");
                    bm.UploadJpegFileAsync(image.LocalFileName, overide: true).GetAwaiter().GetResult();
                }
                this.Trace($"");
            }
        }

        private List<string> LoadContainerList()
        {
            var containers = this._containerManager.GetContainerList().ToList();
            containers.Remove(personDBContainer);
            return containers;
        }

        public void UpdatePersonDBJsonSummary()
        {
            var tmpPersonDBJsonFile = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), $"{personDBContainer}.json");
            var json = System.JSON.JSonObject.Serialize(this.Persons.Select(p => p.Properties).ToList());
            if (File.Exists(tmpPersonDBJsonFile))
                File.Delete(tmpPersonDBJsonFile);
            File.WriteAllText(tmpPersonDBJsonFile, json);
            
            this._containerManager.CreateContainer(personDBContainer);

            var bm = new BlobManager(this._storageName, this._storageKey, personDBContainer);
            bm.UploadJsonFileAsync(tmpPersonDBJsonFile, overide: true).GetAwaiter().GetResult();
        }
        
        public static PersonDB LoadPersonDBSummaryFromAzureStorageDB(string storageName, string storageKey)
        {
            var personDB = new PersonDB(null, storageName, storageKey);
            personDB.Persons = new Persons(PersonDBSource.AZURE_STORAGE);
            var bm = new BlobManager(storageName, storageKey, personDBContainer);
            var propertiesJson = bm.GetTextAsync($"{personDBContainer}.json").GetAwaiter().GetResult();
            var properties = System.JSON.JSonObject.Deserialize<List<PersonProperties>>(propertiesJson);
            foreach(var prop in properties)
            {
                var p = new Person(PersonDBSource.AZURE_STORAGE, null);
                p.Properties = prop;
                personDB.Persons.Add(p);
            }
            return personDB;
        }
        
        private void Trace(string m)
        {
            System.Console.WriteLine(m);
        }

        public void LoadFromAzureStorageDB()
        {
            this.Persons = new Persons(PersonDBSource.AZURE_STORAGE);
            var containers = LoadContainerList();

            foreach (var containerName in containers)
            {
                this.Trace($"Loading container {containerName}");
                var bm = new BlobManager(this._storageName, this._storageKey, containerName);
                var json = bm.GetTextAsync("p.json").GetAwaiter().GetResult();
                var p = Person.LoadFromJson(null, json, PersonDBSource.AZURE_STORAGE);
                this.Persons.Add(p);
                p.LoadImages();
                this.Trace($"");
            }
        }

        public void DeleteAzureStorageDB()
        {
            var containers = LoadContainerList();
            foreach(var c in containers)
            {
                System.Console.WriteLine($"Deleting container:{c}");
                this._containerManager.DeleteContainer(personDBContainer);
            }
        }

        public void LoadFromLocalDB()
        {
            var personFolders = System.IO.Directory.GetDirectories(this._dbPath);
            this.Persons = new Persons(PersonDBSource.LOCAL_FILE_SYSTEM);

            foreach (var personFolder in personFolders)
            {
                var p = Person.LoadFromFolder(personFolder);
                p.SaveAsJsonFile();
                this.Persons.Add(p);
            }
        }
    }
}
