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
             ///// new ConvertPersonXmlToJson().Run(dbPath);

            var storageKey = File.ReadAllText(@".\storage.credentials");
            var storageName = "mygenealogie";
            var db = new PersonDB(dbPath, storageName, storageKey);

            var reUploadDatabase = false;
            var verifyAzurePersonDB = true;

            if (reUploadDatabase && !verifyAzurePersonDB)
            {
                db.LoadFromLocalDB();
                db.Upload();
            }

            if(verifyAzurePersonDB && !reUploadDatabase)
            {
                //db.LoadFromAzureStorageDB();
                //db.UpdatePersonDBJsonSummary();
                var newDb = PersonDB.LoadPersonDBSummaryFromAzureStorageDB(storageName, storageKey);
            }

            

            System.Console.WriteLine("Done");
            System.Console.ReadLine();
        }
    }
}
