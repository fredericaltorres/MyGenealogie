using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;

namespace MyGenealogie
{
    public enum PersonDBSource
    {
        LOCAL_FILE_SYSTEM,
        AZURE_STORAGE
    };

    public class Persons : List<Person>
    {
        public PersonDBSource Source { get; }

        public string ToJSON()
        {
            return System.JSON.JSonObject.Serialize(this);
        }
        public Persons()
        {

        }
        public Persons(PersonDBSource source)
        {
            Source = source;
        }

        public void Sort()
        {
            var sorted = this.OrderBy(p => p.GetFullName()).ToList();
            this.Set(sorted);
        }
        
        private void Set(List<Person> persons)
        {
            this.Clear();
            foreach (var p in persons)
                this.Add(p);
        }
        public Person Add(Person p)
        {
            p.Source = this.Source;
            base.Add(p);
            return p; 
        }
    }
}


