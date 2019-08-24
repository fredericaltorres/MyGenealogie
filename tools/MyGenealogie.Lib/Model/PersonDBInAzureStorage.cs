using fAzureHelper;
using System;
using System.Collections.Generic;
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

        public PersonDBInAzureStorage(string dbPath, string storageName, string storageKey)
        {
            this._containerManager = new ContainerManager(storageName, storageKey);
            this._dbPath = dbPath;
            this.LoadPersons();
        }

        public void Upload()
        {
            var containers = this._containerManager.GetContainerList();
        }

        private void LoadPersons()
        {
            var personFolders = System.IO.Directory.GetDirectories(this._dbPath);
            this.Persons = new Persons();

            foreach (var personFolder in personFolders)
            {
                var p = Person.LoadFromFolder(personFolder);
                this.Persons.Add(p);
            }
        }
    }
}
