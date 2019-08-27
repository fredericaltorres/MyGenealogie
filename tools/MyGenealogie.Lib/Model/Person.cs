using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using MyGenealogie.Console;

namespace MyGenealogie
{
    public class Person
    {
        public PersonProperties Properties = new PersonProperties();
        public PersonDBSource Source;
        internal string _folder;

        public Person()
        {

        }
        
        public Person(PersonDBSource source, string folder)
        {
            this._folder = folder;
            this.Source = source;
        }

        //torres0frederic-antoine-leon
        //torres1mccowan0karin-gene
        //torres-frederic,date ~1964,comment ~avis-de-naissance.jpg
        //https://mygenealogie.blob.core.windows.net/torres0frederic-antoine-leon/torres-frederic,date~1964,comment~avis-de-naissance.jpg

        //Assert.AreEqual("torres0frederic2antoine-leon",
        //        Person.SanitizeNameForAzureContainerName("TORRES, Frederic, Antoine Leon"));
        //    Assert.AreEqual("torres0mcowan1karin2gene",
        //        Person.SanitizeNameForAzureContainerName("TORRES [McCowan], Karin, Gene"));


        private static string BuildImageUrl(string fileName, string folderName)
        {
            return $"https://mygenealogie.blob.core.windows.net/{folderName}/{fileName}";
        }

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
        public string GetPropertiesJsonFile()
        {
            return GetPropertiesFile($"{this.Properties.Guid}.json");
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
        
        public string GetFolderName()
        {
            var folderName = Path.GetFileName(this._folder);
            return folderName;
        }

        public string GetNewFullPathSanitized()
        {
            var folderName = Path.GetFileName(this._folder);
            var newFolderName = SanitizeNameForAzureContainerName(folderName);
            var parentPathFolder = Path.GetDirectoryName(this._folder);
            return Path.Combine(parentPathFolder, newFolderName);
        }

        public void LoadImages(string folder = null, string fileMask = "*.jpg")
        {
            if (folder == null)
                folder = this._folder;
            if (this.Source == PersonDBSource.LOCAL_FILE_SYSTEM)
            {
                var l = new List<PersonImage>();
                var images = Directory.GetFiles(folder, fileMask).ToList();
                foreach (var i in images)
                {
                    var pi = new PersonImage
                    {
                        ImageName = Path.GetFileNameWithoutExtension(i),
                        FileName = Path.GetFileName(i),
                        LocalFileName = i,
                        Url = BuildImageUrl(Path.GetFileName(i), PersonDB.personDBContainer),
                    };
                    l.Add(pi);
                }
                this.Properties.Images = l;
            }
            else if (this.Source == PersonDBSource.AZURE_STORAGE)
            {
                //foreach(var im in this.Properties.Images)
                //    im.Url = BuildImageUrl(im.FileName, this.GetFolderName());
            }
        }

        public void CopyImagesWithGuidPrefix(string destination)
        {
            if (this.Source == PersonDBSource.LOCAL_FILE_SYSTEM)
            {
                foreach(var im in this.Properties.Images)
                {
                    System.Console.WriteLine($"Copying image {Path.GetFileName(im.LocalFileName)}");
                    var finalFileName = $"{this.Properties.Guid}.{Path.GetFileName(im.LocalFileName)}";
                    File.Copy(im.LocalFileName, Path.Combine(destination, finalFileName));
                }
            }
            else if (this.Source == PersonDBSource.AZURE_STORAGE)
            {
                throw new NotImplementedException();
            }
        }

        public static void LoadNamesInfoFromFolderSyntaxNumberAsSeparator(Person p)
        {
            // beaudun9semeac0marie-louise1josette-annette
            // torres0frederic1antoine-leon
            var name = new DirectoryInfo(p._folder).Name;
            var parts = name.Split(new char[] { '0' , '1' , '9' });
            if(name.Contains("9"))
            {
                p.Properties.MaidenName = parts[0];
                p.Properties.LastName = parts[1];
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

        public static Person LoadFromFolder(string folder, bool loadXml = false)
        {
            System.Console.WriteLine($"LoadFromFolder:{folder}");
            var p = new Person( PersonDBSource.LOCAL_FILE_SYSTEM, folder);
            if (!loadXml && File.Exists(p.GetPropertiesJsonFile()))
            {
                p = LoadFromJsonFile(folder, p.GetPropertiesJsonFile(), PersonDBSource.LOCAL_FILE_SYSTEM);
                LoadNamesInfoFromFolderSyntaxNumberAsSeparator(p);
                p.LoadImages();

            }
            else if (File.Exists(p.GetPropertiesXmlFile()))
            {
                // System.Diagnostics.Debugger.Break();
                LoadFromXmlFile(p);
                LoadNamesInfoFromFolderSyntaxNumberAsSeparator(p);
                p.LoadImages();
            }
            return p;
        }

        public static Person LoadFromJsonFile(string folder, string jsonFile, PersonDBSource source)
        {
            return LoadFromJson(folder, File.ReadAllText(jsonFile), source);
        }

        public static Person LoadFromJson(string folder, string json, PersonDBSource source)
        {
            var p = new Person();
            p.Properties = System.JSON.JSonObject.Deserialize<PersonProperties>(json);
            p._folder = folder;
            p.Source = source;
            return p;
        }

        public void SaveAsJsonFile(string outputJsonFileName = null)
        {
            if (outputJsonFileName == null)
                outputJsonFileName = this.GetPropertiesJsonFile();
            var json = System.JSON.JSonObject.Serialize(this.Properties);
            if (File.Exists(outputJsonFileName))
                File.Delete(outputJsonFileName);
            File.WriteAllText(outputJsonFileName, json);
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
            p.Properties.SpouseGuid = ParseFromGuid(personXmlNode, "SpouseGuid");
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
            if (node != null)
            {
                if (!string.IsNullOrEmpty(node.InnerText))
                {
                    var g = Guid.Parse(node.InnerText);
                    return g;
                }
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


