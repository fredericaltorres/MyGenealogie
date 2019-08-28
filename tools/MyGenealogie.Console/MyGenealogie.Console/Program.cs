using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGenealogie.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbPath = @"C:\DVT\MyGenealogie\person.db";
            var db2Path = @"C:\DVT\MyGenealogie\person.db2";
            var storageKey = File.ReadAllText(@".\storage.credentials");
            var storageName = "mygenealogie";

            var veryFirstConvertion = false;
            var reUploadAzureDatabase = false;
            var verifyAzurePersonDB = true;
            var deleteAzureDatabase = false;

            if (veryFirstConvertion)
            {
                new ConvertPersonXmlToJson().Run(dbPath, db2Path);
            }

            var db = new PersonDB(db2Path, storageName, storageKey);

            if (reUploadAzureDatabase)
            {
                db.LoadFromLocalDB();
                db.UploadFromLocalFolder();
            }

            if(verifyAzurePersonDB)
            {
                db.LoadFromAzureStorageDB();
                db.SaveJsonDBInAzure();
                var newDb = PersonDB.LoadSaveJsonDB(storageName, storageKey);
                var ok = db.Persons.Count == newDb.Persons.Count;
            }

            if(deleteAzureDatabase)
            {
                var dbDelete = new PersonDB(null, storageName, storageKey);
                dbDelete.DeleteAzureStorageDB();
            }
                       

            System.Console.WriteLine("Done");
            System.Console.ReadLine();
        }
    }
}
