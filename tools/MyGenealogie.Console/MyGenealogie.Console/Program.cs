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
            var db = new PersonDBInAzureStorage(dbPath, "mygenealogie", storageKey);
            db.Upload();

            System.Console.WriteLine("Done");
            System.Console.ReadLine();
        }
    }
}
