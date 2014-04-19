using System.Linq;
using CompendiumImport.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Compendium.Test
{
    
    
    /// <summary>
    ///This is a test class for SearchResultMonsterTest and is intended
    ///to contain all SearchResultMonsterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SearchResultMonsterTest
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
        ///A test for BySource
        ///</summary>
        [TestMethod()]
        public void MonsterBySourceTest()
        {
            CompendiumSource source = new CompendiumSource {Value = 203};
            
            IEnumerable<SearchResult> actual;
            actual = SearchResultMonster.BySource(source);
            Assert.AreEqual(5,actual.Count());
            Assert.IsTrue(actual.Any(x=>x.Name=="Erdlu"));
        }

        /// <summary>
        ///A test for BySource
        ///</summary>
        [TestMethod()]
        public void ItemBySourceTest()
        {
            CompendiumSource source = new CompendiumSource { Value = 14,Name="Arcane Power" };

            IEnumerable<SearchResult> actual;
            actual = SearchResultMagicItem.BySource(source);
            Assert.AreEqual(9, actual.Count());
            Assert.IsTrue(actual.Any(x => x.Name == "Magic Tome"));
        }

        [TestMethod()]
        public void TrapBySourceTest()
        {
            CompendiumSource source = new CompendiumSource { Value = 202 };

            IEnumerable<SearchResult> actual;
            actual = SearchResultTrap.BySource(source);
            Assert.AreEqual(16, actual.Count());
            Assert.IsTrue(actual.Any(x => x.Name == "Dust Funnel"));
        }
    }
}
