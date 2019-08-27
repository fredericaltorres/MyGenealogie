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
        public const string personDBContainer = "person-db";
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

        public void UploadFromLocalFolder()
        {
            var containerName = PersonDB.personDBContainer;
            this.Trace($"Create container {containerName}");
            this._containerManager.CreateContainer(containerName);

            foreach (var p in Persons)
            {
                this.Trace($"Upload json metadata file {p.GetPropertiesJsonFile()}");
                var bm = new BlobManager(this._storageName, this._storageKey, containerName);
                bm.UploadJsonFileAsync(p.GetPropertiesJsonFile(), overide: true).GetAwaiter().GetResult();
                
                foreach (var image in p.Properties.Images)
                {
                    this.Trace($"Upload image {image.LocalFileName}");
                    bm.UploadJpegFileAsync(image.LocalFileName, overide: true).GetAwaiter().GetResult();
                }
                this.Trace($"");
            }
        }

        public void UpdatePersonJsonFileInAzure(Person p)
        {
            var containerName = PersonDB.personDBContainer;
            this.Trace($"Upload json metadata file {p.Properties.Guid} {p.Properties.LastName} {p.Properties.FirstName}");
            var bm = new BlobManager(this._storageName, this._storageKey, containerName);
            p._folder = Environment.GetEnvironmentVariable("TEMP");
            var tmpJsonFile = p.GetPropertiesJsonFile();
            p.SaveAsJsonFile(tmpJsonFile);
            bm.UploadJsonFileAsync(tmpJsonFile, overide: true).GetAwaiter().GetResult();
        }

        private List<string> LoadContainerList()
        {
            var containers = this._containerManager.GetContainerList().ToList();
            containers.Remove(personDBContainer);
            return containers;
        }

        //public void UpdatePersonDBJsonSummary()
        //{
        //    var tmpPersonDBJsonFile = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), $"{personDBContainer}.json");
        //    var json = System.JSON.JSonObject.Serialize(this.Persons.Select(p => p.Properties).ToList());
        //    if (File.Exists(tmpPersonDBJsonFile))
        //        File.Delete(tmpPersonDBJsonFile);
        //    File.WriteAllText(tmpPersonDBJsonFile, json);
            
        //    this._containerManager.CreateContainer(personDBContainer);

        //    var bm = new BlobManager(this._storageName, this._storageKey, personDBContainer);
        //    bm.UploadJsonFileAsync(tmpPersonDBJsonFile, overide: true).GetAwaiter().GetResult();
        //}
        
        //public static PersonDB LoadPersonDBSummaryFromAzureStorageDB(string storageName, string storageKey)
        //{
        //    var personDB = new PersonDB(null, storageName, storageKey);
        //    personDB.Persons = new Persons(PersonDBSource.AZURE_STORAGE);
        //    var bm = new BlobManager(storageName, storageKey, personDBContainer);
        //    var propertiesJson = bm.GetTextAsync($"{personDBContainer}.json").GetAwaiter().GetResult();
        //    var properties = System.JSON.JSonObject.Deserialize<List<PersonProperties>>(propertiesJson);
        //    foreach(var prop in properties)
        //    {
        //        var p = new Person(PersonDBSource.AZURE_STORAGE, null);
        //        p.Properties = prop;
        //        personDB.Persons.Add(p);
        //    }
        //    return personDB;
        //}
        
        private void Trace(string m)
        {
            System.Console.WriteLine(m);
        }

        public void LoadFromAzureStorageDB()
        {
            this.Persons = new Persons(PersonDBSource.AZURE_STORAGE);

            var jsonFiles = this._containerManager.GetFiles(personDBContainer, ".json");

            var bm = new BlobManager(this._storageName, this._storageKey, personDBContainer, create: false);

            foreach (var jsonFile in jsonFiles)
            {
                this.Trace($"Loading file {jsonFile}");
                var json = bm.GetTextAsync(jsonFile).GetAwaiter().GetResult();
                var p = Person.LoadFromJson(null, json, PersonDBSource.AZURE_STORAGE);
                this.Persons.Add(p);
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
            var jsonFiles = System.IO.Directory.GetFiles(this._dbPath, "*.json");
            this.Persons = new Persons(PersonDBSource.LOCAL_FILE_SYSTEM);

            foreach (var jsonFile in jsonFiles)
            {
                var p = Person.LoadFromJsonFile(this._dbPath, jsonFile, PersonDBSource.LOCAL_FILE_SYSTEM);
                p.LoadImages(this._dbPath, $"{p.Properties.Guid}.*.jpg");                
                this.Persons.Add(p);
            }
        }

        public Person GetPersonByGuid(Guid guid)
        {
            return this.Persons.FirstOrDefault(p => p.Properties.Guid == guid);
        }
    }
}
