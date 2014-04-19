using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using CompendiumImport.Data.Converters;
using CompendiumImport.Tools;
using Masterplan.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Compendium.Test
{
    /// <summary>
    /// Summary description for TrapConvertTest
    /// </summary>
    [TestClass]
    public partial class TrapConverterTest:BaseLibraryTest
    {
        private TrapConverter _converter;
        public TrapConverterTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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
             _converter=new TrapConverter();
             base.MyTestInitialize();
         }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        [DeploymentItem(@"data\ClawsOfSand.htm")]
        public void ClawsOfSand()
         { //http://www.wizards.com/dndinsider/compendium/trap.aspx?id=97
             Trap ddi = LoadTrap("ClawsOfSand.htm", "Claws of Sand");
            Assert.IsTrue(TrapsEqual(ddi));
        }

        [TestMethod]
        [DeploymentItem(@"data\WaterSerpent.htm")]
        public void WaterSerpent()
        { //http://www.wizards.com/dndinsider/compendium/trap.aspx?id=485
            Trap ddi = LoadTrap("WaterSerpent.htm", "Water Serpent");
            Assert.IsTrue(TrapsEqual(ddi));
        }

        [TestMethod]
        [DeploymentItem(@"data\FallingIronPortcullis.htm")]
        public void FallingIronPortcullis()
        { //http://www.wizards.com/dndinsider/compendium/trap.aspx?id=31
            Trap ddi = LoadTrap("FallingIronPortcullis.htm", "Falling Iron Portcullis");
            //Trap mp = FindTrap(ddi.Name);
            //Assert.IsNotNull(mp);
            //if (String.IsNullOrEmpty(mp.Attack.Trigger))
            //    mp.Attack.Trigger =
            //        "A portcullis falls and blocks the passage when a creature steps on a pressure plate.";
            Assert.IsTrue(TrapsEqual(ddi));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidStructureException))]
        [DeploymentItem(@"data\Water Pit.htm")]
        public void WaterPit()
        { //http://www.wizards.com/dndinsider/compendium/trap.aspx?id=913
            Trap ddi = LoadTrap("Water Pit.htm", "Water Pit");
            Assert.IsTrue(TrapsEqual(ddi));
        }

        [Ignore]
        [TestMethod]
        [DeploymentItem(@"data\TheCrowd.htm")]
        public void TheCrowd()
        { //http://www.wizards.com/dndinsider/compendium/trap.aspx?id=941
            Trap ddi = LoadTrap("TheCrowd.htm", "TheCrowd");
            Assert.IsTrue(TrapsEqual(ddi));
        }
    }
}
