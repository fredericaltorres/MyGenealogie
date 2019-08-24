using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;

namespace MyGenealogie
{
    public class Person
    {
        public PersonProperties Properties = new PersonProperties();
        internal string _folder;
        public Person()
        {

        }

        public Person(string folder)
        {
            this._folder = folder;
        }

        //torres0frederic-antoine-leon
        //torres1mccowan0karin-gene
        //torres-frederic,date ~1964,comment ~avis-de-naissance.jpg
        //https://mygenealogie.blob.core.windows.net/torres0frederic-antoine-leon/torres-frederic,date~1964,comment~avis-de-naissance.jpg

        //Assert.AreEqual("torres0frederic2antoine-leon",
        //        Person.SanitizeNameForAzureContainerName("TORRES, Frederic, Antoine Leon"));
        //    Assert.AreEqual("torres0mcowan1karin2gene",
        //        Person.SanitizeNameForAzureContainerName("TORRES [McCowan], Karin, Gene"));

        public static string SanitizeNameForAzureContainerName(string n)
        {
            if (string.IsNullOrEmpty(n)) throw new ArgumentException("Parameter cannot be null or empty");

            n = n.Replace("  ", " ");
            n = n.Replace("[ ", "[");
            n = n.Replace(" [", "[");
            n = n.Replace("] ", "]");
            n = n.Replace(" ]", "]");
            n = n.Replace(", ", ",");
            n = n.Replace(" ,", ",");

            var sb = new StringBuilder(n);
            var comaCounter = 0;
            for(var i=0; i<sb.Length; i++)
            {
                sb[i] = sb[i].ToString().ToLowerInvariant()[0];
                if (sb[i] == ' ') sb[i] = '-';
                if (sb[i] == '[') sb[i] = '9';
                if (sb[i] == ']') sb[i] = '`'; // to be removed
                if (sb[i] == ',') {
                    sb[i] = comaCounter.ToString()[0];
                    comaCounter += 1;
                }
            }
            return sb.ToString().Replace("`","");
        }

        private string GetPropertiesFile(string file)
        {
            return Path.Combine(this._folder, file);
        }

        private string GetPropertiesXmlFile ()
        {
            return GetPropertiesFile("p.xml");
        }
        private string GetPropertiesJsonFile()
        {
            return GetPropertiesFile("p.json");
        }

        public bool RenamePersonFolderToSanitizedName()
        {
            try
            {
                var newPersonFolderName = this.GetNewFullPathSanitized();
                if (this._folder != newPersonFolderName)
                {
                    System.Console.WriteLine($"Rename person from:{this._folder}, to:{this.GetNewFullPathSanitized()}");
                    Directory.Move(this._folder, this.GetNewFullPathSanitized());
                }
                return true;
            }
            catch(Exception ex)
            {
                System.Console.WriteLine($"Rename person folder error:{ex.Message}");
                return false;
            }
        }


        public string GetNewFullPathSanitized()
        {
            var folderName = Path.GetFileName(this._folder);
            var newFolderName = SanitizeNameForAzureContainerName(folderName);
            var parentFolder = Path.GetDirectoryName(this._folder);
            return Path.Combine(parentFolder, newFolderName);
        }

        public List<PersonImage> LoadImages()
        {
            var l = new List<PersonImage>();
            var images = Directory.GetFiles(this._folder, "*.jpg").ToList();
            foreach(var i in images)
            {
                var pi = new PersonImage {
                    ImageName = Path.GetFileNameWithoutExtension(i),
                    FileName = Path.GetFileName(i),
                    LocalFileName = i,
                    Url = null,
                };
                l.Add(pi);
            }
            this.Properties.Images = l;
            return l;
        }

        public static void LoadNamesInfoFromFolderSyntaxNumberAsSeparator(Person p)
        {
            // beaudun9semeac0marie-louise1josette-annette
            // torres0frederic1antoine-leon
            var name = new DirectoryInfo(p._folder).Name;
            var parts = name.Split(new char[] { '0' , '1' , '9' });
            if(name.Contains("9"))
            {
                p.Properties.LastName = parts[0];
                p.Properties.MaidenName = parts[1];
                if (name.Contains("0"))
                    p.Properties.FirstName = parts[2];
                if (name.Contains("1"))
                    p.Properties.MiddleName = parts[3];
            }
            else
            {
                p.Properties.LastName = parts[0];
                if (name.Contains("0"))
                    p.Properties.FirstName = parts[1];
                if (name.Contains("1"))
                    p.Properties.MiddleName = parts[2];
            }
        }

        public static void LoadNamesInfoFromFolderSyntaxWithBrakets(Person p)
        {
            var name = new DirectoryInfo(p._folder).Name;
            var parts = name.Split(',');
            if (parts.Length >= 1)
            {
                if(parts[0].Contains("["))
                {
                    var ln = parts[0];
                    var index = ln.IndexOf("[");
                    p.Properties.MaidenName = ln.Substring(index+1).Replace("]", "").Trim();
                    p.Properties.LastName = ln.Substring(0, index).Trim();
                }
                else
                {
                    p.Properties.LastName = parts[0].Trim();
                }
            }
            if (parts.Length >= 2)
                p.Properties.FirstName = parts[1].Trim();
        }

        public static Person LoadFromFolder(string folder)
        {
            var p = new Person(folder);
            if (File.Exists(p.GetPropertiesJsonFile()))
            {
                var json = File.ReadAllText(p.GetPropertiesJsonFile());
                p = System.JSON.JSonObject.Deserialize<Person>(json);
            }
            else if (File.Exists(p.GetPropertiesXmlFile()))
            {
                LoadFromXmlFile(p);
                LoadNamesInfoFromFolderSyntaxWithBrakets(p);
                p.LoadImages();
            }
            return p;
        }

        public void SaveAsJsonFile()
        {
            var json = System.JSON.JSonObject.Serialize(this.Properties);
            if (File.Exists(this.GetPropertiesJsonFile()))
                File.Delete(this.GetPropertiesJsonFile());
            File.WriteAllText(this.GetPropertiesJsonFile(), json);
        }

        private static void LoadFromXmlFile(Person p)
        {
            var xml = File.ReadAllText(p.GetPropertiesXmlFile());
            var xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.LoadXml(xml);
            var personXmlNode = xmlDoc.SelectSingleNode("/Person");

            p.Properties.Guid = ParseFromGuid(personXmlNode, "Guid").Value;
            p.Properties.FatherGuid = ParseFromGuid(personXmlNode, "FatherGuid");
            p.Properties.MotherGuid = ParseFromGuid(personXmlNode, "MotherGuid");
            p.Properties.CreationDate = ParseFromDate(personXmlNode, "CreationDate");
            p.Properties.BirthDate = ParseFromDate(personXmlNode, "BirthDate");
            p.Properties.DeathDate = ParseFromDate(personXmlNode, "DeathDate");
            p.Properties.Sexe = ParseFromString(personXmlNode, "Sexe");
            p.Properties.BirthCity = ParseFromString(personXmlNode, "BirthCity");
            p.Properties.BirthCountry = ParseFromString(personXmlNode, "BirthCountry");
            p.Properties.DeathCity = ParseFromString(personXmlNode, "DeathCity");
            p.Properties.DeathCountry = ParseFromString(personXmlNode, "DeathCountry");
            p.Properties.Comment = ParseFromString(personXmlNode, "Comment");
        }

        private static string ParseFromString(XmlNode xmlNode, string property)
        {
            var node = xmlNode.SelectSingleNode(property);
            if (!string.IsNullOrEmpty(node.InnerText))
                return node.InnerText;

            return null;
        }
        private static Guid? ParseFromGuid(XmlNode xmlNode, string property)
        {
            var node = xmlNode.SelectSingleNode(property);
            if(!string.IsNullOrEmpty(node.InnerText))
            {
                var g = Guid.Parse(node.InnerText);
                return g;
            }
            return null;
        }
        private static PersonDate ParseFromDate(XmlNode xmlNode, string property)
        {
            var node = xmlNode.SelectSingleNode(property);
            if (node.InnerText == null)
                return null;

            if (!string.IsNullOrEmpty(node.InnerText.Trim()))
            {
                var parts = node.InnerText.Split('-');
                var year = 0;
                var month = 0;
                var day = 0;
                if (parts.Length >= 1)
                    year = int.Parse(parts[0]);
                if (parts.Length >= 2)
                    month = int.Parse(parts[1]);
                if (parts.Length >= 3)
                    day = int.Parse(parts[2]);

                if (month > 12)
                    month = 1; // Some bug in the legacy database

                var d = new PersonDate { Year = year, Month = month, Day = day };
                return d;
            }
            return null;
        }
    }
}


