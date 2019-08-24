using System;
using System.Collections.Generic;
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
            new ConvertPersonXmlToJson().Run(dbPath);
            System.Console.WriteLine("Done");
            System.Console.ReadLine();
        }
    }
}
