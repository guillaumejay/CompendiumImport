using CompendiumImport.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Compendium.Test
{
    
    
    /// <summary>
    ///This is a test class for CompendiumSourceTest and is intended
    ///to contain all CompendiumSourceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CompendiumSourceTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for FromUrl
        ///</summary>
        [TestMethod()]
        public void FromUrlTest()
        {
            IEnumerable<CompendiumSource> actual;
            actual = CompendiumSource.FromUrl();
            Assert.IsTrue(actual.Count()>5,"No sources found !");
            Assert.IsNotNull(actual.SingleOrDefault(x => x.Name == "Dragon Magazines"));
            Assert.IsTrue(actual.Any(x => x.SourceType == "MonsterSourceOther"));
            Assert.IsTrue(actual.Any(x => x.SourceType == "MonsterSourceBook"));
        }
    }
}
