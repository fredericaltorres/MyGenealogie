using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGenealogie
{
    public class ConvertPersonXmlToJson
    {
        public void Run(string dbPath)
        {
            System.Console.WriteLine("Convert Person Xml to Json file");
            var personFolders = System.IO.Directory.GetDirectories(dbPath);
            foreach(var personFolder in personFolders)
            {
                System.Console.WriteLine($"Processing {personFolder}");
                var p = Person.LoadFromFolder(personFolder);
                p.SaveAsJsonFile();
            }
        }
    }
}
