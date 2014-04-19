using System;
using System.Collections.Generic;
using System.IO;
using CompendiumImport.Data.Converters;
using CompendiumImport.Tools;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Masterplan.Data;

namespace Compendium.Test
{
    /// <summary>
    ///This is a test class for HtmlDataTest and is intended
    ///to contain all HtmlDataTest Unit Tests
    ///</summary>
    [TestClass()]
    public class HtmlDataTest
    {
        public List<String> Warning { get; set; }

        public HtmlDataTest()
            : base()
        {

        }

        #region Additional test attributes

        //
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
        }

        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
        }

        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //

        #endregion Additional test attributes

        private  HtmlData GetTarget(string password,string importType)
        {
            string login;
            string passw;
            string file = System.Configuration.ConfigurationManager.AppSettings["ID-File"];
            if (!File.Exists(file))
            {
                Assert.Fail("Can't find login/password file " + file);
            }
            using (TextReader tr = new StreamReader(file))
            {
                login = tr.ReadLine();
                passw = tr.ReadLine();
            }
            if (!string.IsNullOrEmpty(password))
                passw = password;
            return new HtmlData(login, passw,importType);
        }

        [Ignore]
        [TestMethod]
        public void GetHtmlTest_Creatures()
        {
            HtmlData target = GetTarget(null,"monster");
            HtmlDocument actual;
            actual = target.GetHtmlDocument(4799);
            Assert.IsTrue(actual.DocumentNode.InnerText.Contains("Craud Impaler"));
            actual = target.GetHtmlDocument(2544);
            Assert.IsFalse(actual.DocumentNode.InnerText.Contains("Subscribe Now"));
            Assert.IsTrue(actual.DocumentNode.InnerText.Contains("Chaos Mauler"));
            actual = target.GetHtmlDocument(5869);
            Assert.IsTrue(actual.DocumentNode.InnerText.Contains("Vampiric Mist"));
        }

        [Ignore]
        [TestMethod]
        public void GetHtmlTest_Trap()
        {
            HtmlData target = GetTarget(null,"trap");
            HtmlDocument actual;
            actual = target.GetHtmlDocument(31);
            Assert.IsTrue(actual.DocumentNode.InnerText.Contains("Falling Iron Portcullis"));
          
        }

        [TestMethod]
        [ExpectedException(typeof(CompendiumImport.Tools.FailToLogInException))]
        public void InvalidLoginPassword()
        {
            HtmlData target = GetTarget("toto","monster");
            HtmlDocument actual = target.GetHtmlDocument(4747);
        }

        [Ignore]
        [TestMethod]
        public void GetAndLoad()
        {
            HtmlData target = GetTarget(null,"monster");
            HtmlDocument actual;
            actual = target.GetHtmlDocument(5373);
            string creaturename = "Anakore Render";
            Assert.IsTrue(actual.DocumentNode.InnerText.Contains(creaturename));
            CreatureConverter cc=new CreatureConverter();
            Creature c = cc.GetMasterPlanObjectFromDoc(actual,Warning);
            Assert.AreEqual(c.Name,creaturename);
        }
    }
}