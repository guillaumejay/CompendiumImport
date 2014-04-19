using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using CompendiumImport.Data.Converters;
using Masterplan.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Compendium.Test
{
    /// <summary>
    /// Summary description for TrapConvertTest
    /// </summary>
    [TestClass]
    public partial class MagicItemConverterTest:BaseLibraryTest
    {
        private MagicItemConverter _converter;
      

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
         [ClassInitialize()]
         public static void MyClassInitialize(TestContext testContext)
         {
             InitLibraries();
         }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
         [TestInitialize()]
         public override void MyTestInitialize()
         {
             _converter=new MagicItemConverter();
             base.MyTestInitialize();
         }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        [DeploymentItem(@"data\FreezingArrow.htm")]
        public void FreezingArrow()
         { //http://www.wizards.com/dndinsider/compendium/trap.aspx?id=97
             MagicItem ddi = LoadMagicItem("FreezingArrow.htm", "Freezing Arrow");
            Assert.IsTrue(MagicItemsEqual(ddi));
        }

    }
}
