﻿using fAzureHelper;
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
            this.Trace($"Create container {PersonDB.personDBContainer}");
            this._containerManager.CreateContainer(PersonDB.personDBContainer);

            foreach (var p in Persons)
            {
                this.Trace($"Upload json metadata file {p.GetPropertiesJsonFile()}");
                var bm = GetBlobManager();
                bm.UploadJsonFileAsync(p.GetPropertiesJsonFile(), overide: true).GetAwaiter().GetResult();
                
                foreach (var image in p.Properties.Images)
                {
                    this.Trace($"Upload image {image.LocalFileName}");
                    bm.UploadJpegFileAsync(image.LocalFileName, overide: true).GetAwaiter().GetResult();
                }
                this.Trace($"");
            }
        }

        public void UploadImage(Person p, PersonImage image)
        {
            var bm = GetBlobManager();
            bm.UploadJpegFileAsync(image.LocalFileName, overide: true).GetAwaiter().GetResult();
        }

        public void DeleteImageInAzureStorage(PersonImage image)
        {
            var bm = GetBlobManager();
            bm.DeleteFileAsync(image.LocalFileName).GetAwaiter().GetResult();
        }

        public bool DeleteImage(Guid personGuid, string imageFileName)
        {
            var person = this.GetPersonByGuid(personGuid);
            var personImageToDelete = person.Properties.Images.FirstOrDefault(i => i.FileName == imageFileName);
            if (personImageToDelete != null)
            {
                this.DeleteImageInAzureStorage(personImageToDelete);
                person.Properties.Images.Remove(personImageToDelete);
                if (this.UpdatePerson(person.Properties))
                    return true;
            }
            return false;
        }

        public bool UpdatePerson(PersonProperties personProperties)
        {
            var person = this.GetPersonByGuid(personProperties.Guid);
            if (person == null)
                return false;

            // Save the person as a JSON local file
            var source = person.Source;
            var folder = person._folder;
            person.Source = PersonDBSource.LOCAL_FILE_SYSTEM;
            person._folder = Environment.GetEnvironmentVariable("TEMP");
            person.Properties = personProperties;
            person.SaveAsJsonToLocalFolder(); 
            // Upload the json local file to Azure storage
            GetBlobManager().UploadJsonFileAsync(person.GetPropertiesJsonFile(), overide: true).GetAwaiter().GetResult();
            File.Delete(person.GetPropertiesJsonFile());
            // Save the global json file in Azure storage
            person.Source = source;
            person._folder = folder;
            this.SaveJsonDBInAzure();

            return true;
        }

        public bool DeletePerson(Guid guid)
        {
            var person = this.GetPersonByGuid(guid);
            if (person == null)
                return false;

            // TODO: refactor in one function
            var bm = GetBlobManager();
            if (bm.FileExistAsync(person.GetPropertiesJsonFile()).GetAwaiter().GetResult())
            {
                bm.DeleteFileAsync(person.GetPropertiesJsonFile()).GetAwaiter().GetResult();
            }
            this.Persons.Remove(person);
            this.SaveJsonDBInAzure();
            return true;
        }

        public void UpdatePersonJsonFileInAzure(Person p)
        {
            var containerName = PersonDB.personDBContainer;
            this.Trace($"Upload json metadata file {p.Properties.Guid} {p.Properties.LastName} {p.Properties.FirstName}");
            var bm = GetBlobManager();
            p._folder = Environment.GetEnvironmentVariable("TEMP");
            var tmpJsonFile = p.GetPropertiesJsonFile();
            p.SaveAsJsonToLocalFolder(tmpJsonFile);
            bm.UploadJsonFileAsync(tmpJsonFile, overide: true).GetAwaiter().GetResult();
        }

        private List<string> LoadContainerList()
        {
            var containers = this._containerManager.GetContainerList().ToList();
            containers.Remove(personDBContainer);
            return containers;
        }

        private string GetPersonJsonDBFileName()
        {
            return $"{personDBContainer}.json";
        }

        private BlobManager GetBlobManager()
        {
            return new BlobManager(this._storageName, this._storageKey, PersonDB.personDBContainer);
        }

        public Person NewPerson()
        {
            var p = new Person(PersonDBSource.LOCAL_FILE_SYSTEM, Environment.GetEnvironmentVariable("TEMP"));
            p.Properties.LastName = $"New {Environment.TickCount}";
            p.Properties.FirstName = p.Properties.LastName;
            p.Properties.Guid = Guid.NewGuid();
            p.Properties.BirthDate = new PersonDate().SetToNow();
            p.Properties.DeathDate = new PersonDate().SetToNow();
            p.Properties.CreationDate = new PersonDate().SetToNow();
            p.Properties.CreationDate = new PersonDate().SetToNow();
            p.SaveAsJsonToLocalFolder(); // Save as JSON local file
            GetBlobManager().UploadJsonFileAsync(p.GetPropertiesJsonFile()).GetAwaiter().GetResult();

            this.Persons.Add(p);
            this.SaveJsonDBInAzure();
            p.Source = PersonDBSource.LOCAL_FILE_SYSTEM; // Force to delete on file system
            File.Delete(p.GetPropertiesJsonFile());

            return p;
        }

        public void SaveJsonDBInAzure()
        {
            var tmpPersonDBJsonFile = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), $"{this.GetPersonJsonDBFileName()}");
            var json = System.JSON.JSonObject.Serialize(this.Persons.Select(p => p.Properties).ToList());
            if (File.Exists(tmpPersonDBJsonFile))
                File.Delete(tmpPersonDBJsonFile);
            File.WriteAllText(tmpPersonDBJsonFile, json);

            var bm = GetBlobManager();
            bm.UploadJsonFileAsync(tmpPersonDBJsonFile, overide: true).GetAwaiter().GetResult();
        }

        public static PersonDB LoadSaveJsonDB(string storageName, string storageKey)
        {
            var personDB = new PersonDB(null, storageName, storageKey);
            personDB.Persons = new Persons(PersonDBSource.AZURE_STORAGE);
            var bm = personDB.GetBlobManager();
            var propertiesJson = bm.GetTextAsync($"{personDBContainer}.json").GetAwaiter().GetResult();
            var properties = System.JSON.JSonObject.Deserialize<List<PersonProperties>>(propertiesJson);
            foreach (var prop in properties)
            {
                var p = new Person(PersonDBSource.AZURE_STORAGE, null);
                p.Properties = prop;
                personDB.Persons.Add(p);
            }
            personDB.Persons.Sort();
            return personDB;
        }

        private void Trace(string m)
        {
            System.Console.WriteLine(m);
        }

        public void SaveToLocalFolder(string dbPath)
        {
            var bm = GetBlobManager();
            foreach (var p in this.Persons)
            {
                this.Trace($"");
                this.Trace($"Saving to local path p:{p.GetFullName()}");
                p.Source = PersonDBSource.LOCAL_FILE_SYSTEM;
                p._folder = dbPath;
                p.SaveAsJsonToLocalFolder(p.GetPropertiesJsonFile());
                if (p.Properties.Images != null)
                {
                    foreach (var i in p.Properties.Images)
                    {
                        this.Trace($"Saving to local path, image:{i.FileName}");
                        var localPath = Path.Combine(dbPath, i.FileName);
                        bm.DownloadFileAsync(i.FileName, localPath);
                    }
                }
            }
        }

        public void LoadFromAzureStorageDB()
        {
            this.Persons = new Persons(PersonDBSource.AZURE_STORAGE);

            var jsonFiles = this._containerManager.GetFiles(personDBContainer, ".json");
            jsonFiles.Remove(this.GetPersonJsonDBFileName());

            var bm = GetBlobManager();

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

        public Person GetPersonByLastNameFirstName(string lastName, string firstName)
        {
            return this.Persons.FirstOrDefault(p => p.Properties.LastName == lastName && p.Properties.FirstName == firstName);
        }

        public bool PersonExists(Guid guid)
        {
            return GetPersonByGuid(guid) != null;
        }

        public Person GetPersonByGuid(Guid guid)
        {
            return this.Persons.FirstOrDefault(p => p.Properties.Guid == guid);
        }

        public Person GetPersonByUsername(string username)
        {
            if (username == null)
                throw new ArgumentException($"username parameter cannot be null");
            return this.Persons.FirstOrDefault(p => p.Properties._Username == username);
        }
    }
}


