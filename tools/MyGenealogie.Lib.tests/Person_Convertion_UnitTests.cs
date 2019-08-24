using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MyGenealogie.Lib.tests
{
    [TestClass]
    public class Person_Convertion_UnitTests
    {
        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void SanitizeNameForAzureContainerName_ParameterNull()
        {
            Person.SanitizeNameForAzureContainerName(null);
        }
        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void SanitizeNameForAzureContainerName_ParameterEmptyString()
        {
            Person.SanitizeNameForAzureContainerName("");
        }
        [TestMethod]
        public void SanitizeNameForAzureContainerName_basic()
        {
            Assert.AreEqual("a", Person.SanitizeNameForAzureContainerName("a"));
            Assert.AreEqual("a-b", Person.SanitizeNameForAzureContainerName("a b"));
            Assert.AreEqual("lastname-firstname", 
                Person.SanitizeNameForAzureContainerName("LastName FirstName"));
        }
        [TestMethod]
        public void SanitizeNameForAzureContainerName_RealNames()
        {
            Assert.AreEqual("torres0frederic1antoine-leon",
                Person.SanitizeNameForAzureContainerName("TORRES , Frederic, Antoine Leon"));
            Assert.AreEqual("torres9mccowan0karin1gene",
                Person.SanitizeNameForAzureContainerName("TORRES[McCowan] , Karin , Gene"));
        }
        [TestMethod]
        public void GetNewFullPathSanitized()
        {
            var p = new Person(PersonDBSource.LOCAL_FILE_SYSTEM,  @"C:\DVT\MyGenealogie\person.db\BEAUDUN [SEMEAC], Marie Louise");
            var expected = @"C:\DVT\MyGenealogie\person.db\beaudun9semeac0marie-louise";
            Assert.AreEqual(expected, p.GetNewFullPathSanitized());

            p = new Person(PersonDBSource.LOCAL_FILE_SYSTEM, @"C:\DVT\MyGenealogie\person.db\BEAUDUN [SEMEAC], Marie Louise, Josette  Annette");
            expected = @"C:\DVT\MyGenealogie\person.db\beaudun9semeac0marie-louise1josette-annette";
            Assert.AreEqual(expected, p.GetNewFullPathSanitized());
        }
        [TestMethod]
        public void LoadNamesInfoFromFolderSyntaxNumberAsSeparator_WithMaidenName()
        {
            var p = new Person(PersonDBSource.LOCAL_FILE_SYSTEM, @"C:\DVT\MyGenealogie\person.db\beaudun9semeac0marie-louise");
            Person.LoadNamesInfoFromFolderSyntaxNumberAsSeparator(p);
            Assert.AreEqual("beaudun", p.Properties.LastName);
            Assert.AreEqual("semeac", p.Properties.MaidenName);
            Assert.AreEqual("marie-louise", p.Properties.FirstName);
            Assert.AreEqual(null, p.Properties.MiddleName);

            p = new Person(PersonDBSource.LOCAL_FILE_SYSTEM, @"C:\DVT\MyGenealogie\person.db\beaudun9semeac0marie-louise1josette-annette");
            Person.LoadNamesInfoFromFolderSyntaxNumberAsSeparator(p);
            Assert.AreEqual("beaudun", p.Properties.LastName);
            Assert.AreEqual("semeac", p.Properties.MaidenName);
            Assert.AreEqual("marie-louise", p.Properties.FirstName);
            Assert.AreEqual("josette-annette", p.Properties.MiddleName);
        }

        [TestMethod]
        public void LoadNamesInfoFromFolderSyntaxNumberAsSeparator_NoMaidenName()
        {
            var p = new Person(PersonDBSource.LOCAL_FILE_SYSTEM, @"C:\DVT\MyGenealogie\person.db\torres0frederic");
            Person.LoadNamesInfoFromFolderSyntaxNumberAsSeparator(p);
            Assert.AreEqual("torres", p.Properties.LastName);
            Assert.AreEqual(null, p.Properties.MaidenName);
            Assert.AreEqual("frederic", p.Properties.FirstName);
            Assert.AreEqual(null, p.Properties.MiddleName);

            p = new Person(PersonDBSource.LOCAL_FILE_SYSTEM, @"C:\DVT\MyGenealogie\person.db\torres0frederic1antoine-leon");
            Person.LoadNamesInfoFromFolderSyntaxNumberAsSeparator(p);
            Assert.AreEqual("torres", p.Properties.LastName);
            Assert.AreEqual(null, p.Properties.MaidenName);
            Assert.AreEqual("frederic", p.Properties.FirstName);
            Assert.AreEqual("antoine-leon", p.Properties.MiddleName);

        }
    }
}
