using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyGenealogie.Lib.Utility;
using System;
using System.Text;

namespace MyGenealogie.Lib.tests
{
    [TestClass]
    public class PasswordUtility_UnitTests
    {

        [TestMethod]
        public void HashPassword()
        {
            var originalPassword = "abcd1234!";
            var hash = PasswordUtility.HashPassword(originalPassword);
            Assert.IsTrue(PasswordUtility.VerifyHashedPassword(originalPassword, hash));

            var badHash = new StringBuilder(hash);
            badHash[8] = 'A';
            Assert.IsFalse(PasswordUtility.VerifyHashedPassword(originalPassword, badHash.ToString()));
        }
    }
}
