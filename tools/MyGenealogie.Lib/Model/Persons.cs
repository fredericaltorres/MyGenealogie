using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;

namespace MyGenealogie
{
    public class Persons : List<Person>
    {
        public string ToJSON()
        {
            return System.JSON.JSonObject.Serialize(this);
        }
    }
}


