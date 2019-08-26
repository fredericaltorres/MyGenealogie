using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGenealogie
{
    public class ConvertPersonXmlToJson
    {
        public void Run(string dbPath, string dbPath2)
        {
            System.Console.WriteLine("Convert Person Xml to Json file");
            var personFolders = System.IO.Directory.GetDirectories(dbPath);
            var personDB = new Persons(PersonDBSource.LOCAL_FILE_SYSTEM);
            var renameCounterError = 0;
            foreach(var personFolder in personFolders)
            {
                System.Console.WriteLine($"Processing {personFolder}");
                var p = Person.LoadFromFolder(personFolder, true);
                p.SaveAsJsonFile();
                p.SaveAsJsonFile(Path.Combine(dbPath2,  $"{p.Properties.Guid}.json"));
                p.CopyImagesWithGuidPrefix(dbPath2);

                if (!p.RenamePersonFolderToSanitizedName())
                    renameCounterError += 1;
                personDB.Add(p);
            }
            System.Console.WriteLine($"renameCounterError {renameCounterError}");
            File.WriteAllText(Path.Combine(dbPath, "PersonDB.json"), personDB.ToJSON());
        }
    }
}
