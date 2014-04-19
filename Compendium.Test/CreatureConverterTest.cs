using System.Collections.Generic;
using CompendiumImport.Data.Converters;
using CompendiumImport.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Masterplan.Data;
using HtmlAgilityPack;
using System.IO;
using System.Linq;

namespace Compendium.Test
{
    /// <summary>
    ///This is a test class for CreatureConverterTest and is intended
    ///to contain all CreatureConverterTest Unit Tests
    ///</summary>
    [TestClass()]
    public partial class CreatureConverterTest:BaseLibraryTest
    {
        private CreatureConverter _converter;    

        #region Additional test attributes
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            InitLibraries();
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
        public override void MyTestInitialize()
        {
            _converter = new CreatureConverter();
            base.MyTestInitialize();
        }
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        [TestMethod]
        [DeploymentItem("data\\ChaosMauler.html")]
        public void LoadChaosMauler()
        {
            Creature c = LoadCreature("ChaosMauler.html","Chaos Mauler");
            Assert.IsTrue(CreaturesEqual(c));
        }

        [TestMethod]
        [DeploymentItem(@"data\BoggleSightStealer.htm")]
        public void LoadBoggleSightStealer()
        {
           Creature c=LoadCreature("BoggleSightStealer.htm","Boggle Sight Stealer");
            Assert.IsTrue(CreaturesEqual(c));
        }

        [TestMethod]
        [DeploymentItem(@"data\GoblinSkullCleaver.html")]
        public void LoadGoblinSkullCleaver()
        {
            Creature c = LoadCreature("goblinskullcleaver.html","Goblin SkullCleaver");
            Assert.IsTrue(CreaturesEqual(c));
        }

        [TestMethod]
        [DeploymentItem("data\\CraudImpaler.htm")]
        public void LoadCraudImpaler()
        { //http://www.wizards.com/dndinsider/compendium/monster.aspx?id=4799
            Creature c = LoadCreature("craudimpaler.htm","Craud Impaler");
            Assert.IsTrue(CreaturesEqual(c));
        }

        [TestMethod]
        [DeploymentItem("data\\Tainted Wisp.htm")]
        public void TaintedWisp()
        { //http://www.wizards.com/dndinsider/compendium/monster.aspx?id=4456
            Creature c = LoadCreature("tainted wisp.htm","Tainted Wisp");
            Assert.IsTrue(CreaturesEqual(c));
        }

        [TestMethod]
        [DeploymentItem("data\\MageWight.htm")]
        public void MageWight()
        { //http://www.wizards.com/dndinsider/compendium/monster.aspx?id=4458
            Creature c = LoadCreature("magewight.htm","Mage Wight");
            Assert.IsTrue(CreaturesEqual(c));
        }

        [TestMethod]
        [DeploymentItem("data\\orc_chieftain.htm")]
        public void OrcChieftain()
        {// http://www.wizards.com/dndinsider/compendium/monster.aspx?id=703
            Creature c = LoadCreature("orc_chieftain.htm", "Orc Chieftain");
            Assert.IsTrue(CreaturesEqual(c));
        }

        [TestMethod]
        [DeploymentItem("data\\Magma Hurler.htm")]
        public void MagmaHurler()
        {// http://www.wizards.com/dndinsider/compendium/monster.aspx?id=329
            Creature c = LoadCreature("magma hurler.htm","magma hurler");
            Assert.IsTrue(CreaturesEqual(c));
        }

        [TestMethod]
        [DeploymentItem("data\\CharnelCinderhouse.htm")]
        public void CharnelCinderhouse()
        {// http://www.wizards.com/dndinsider/compendium/monster.aspx?id=1915

            Creature c = LoadCreature("CharnelCinderhouse.htm","charnel cinderhouse");
            Assert.IsTrue(CreaturesEqual(c));
        }

        [TestMethod]
        [DeploymentItem("data\\TwoHeadedTroll.htm")]
        public void LoadTwoHeadedTroll()
        { // http://www.wizards.com/dndinsider/compendium/monster.aspx?id=2069
            Creature c = LoadCreature("TwoHeadedTroll.htm","Two-headed troll");
            Assert.IsTrue(CreaturesEqual(c));
        }

        [TestMethod]
        [DeploymentItem("data\\Ogrewarhulk.htm")]
        public void OgreWarhulk()
        {//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=355
            Creature c = LoadCreature("OgreWarhulk.htm", "Ogre warhulk");
            Assert.IsTrue(CreaturesEqual(c));
        }

        [TestMethod]
        [DeploymentItem("data\\razorhydra.htm")]
        public void RazorHydra()
        {//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=3016
            Creature c = LoadCreature("razorhydra.htm","Razor hydra");
            Assert.IsTrue(CreaturesEqual(c));
        }

        [TestMethod]
        [DeploymentItem("data\\YounggoldDragon.htm")]
        public void YoungGoldDragon()
        {
//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=2899
            Creature c = LoadCreature("YounggoldDragon.htm", "Young Gold Dragon");
            Assert.IsTrue(CreaturesEqual(c));
        }

        [TestMethod]
        [DeploymentItem("data\\BullywugTwitcher.htm")]
        public void BullywugTwitcher()
        {//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=2822
            Creature c = LoadCreature("BullywugTwitcher.htm", "Bullywug Twitcher");
            Assert.IsTrue(CreaturesEqual(c));
        }

        [TestMethod]
        [DeploymentItem("data\\kuotoacutter.htm")]
        public void KuoToaCutter()
        {//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=2822
            Creature c = LoadCreature("kuotoacutter.htm", "Kuo-Toa Cutter");
            Assert.IsTrue(CreaturesEqual(c));
        }

        [TestMethod]
        [DeploymentItem("data\\battleworg.htm")]
        public void BattleWorg()
        {//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=5668
            Creature c = LoadCreature("battleworg.htm", "Battle Worg");
            Assert.IsTrue(CreaturesEqual(c));
        }

        [TestMethod]
        [DeploymentItem("data\\BloodSpearSavageThrong.htm")]
        public void BloodSpearSavageThrong()
        {//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=5667
            Creature c = LoadCreature("BloodSpearSavageThrong.htm", "Bloodspear Savage Throng");
            Assert.IsTrue(CreaturesEqual(c, "Swarm"));
        }

        //The test file has been corrected : aura was written as "Cold Spot (cold) aura [[x]]; each enemy that starts its turn within the aura takes [[x]] cold damage"
        [TestMethod]
        [DeploymentItem("data\\Griefmote Cloud.htm")]
        public void GriefmoteCloud()
        {//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=3191
            Creature c = LoadCreature("Griefmote Cloud.htm","Griefmote cloud");
            Assert.IsTrue(CreaturesEqual(c));
        }

        [TestMethod]
        [DeploymentItem("data\\BloodSpearGrenadier.htm")]
        public void BloodspearGrenadier()
        {//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=3191
           Creature c=LoadCreature("BloodSpearGrenadier.htm","Bloodspear grenadier");
            Assert.IsTrue(CreaturesEqual(c, "Caltrops"));
        }

        [TestMethod]
        [DeploymentItem("data\\Bloodspear Krull.htm")]
        public void BloodspearKrull()
        {//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=5662

            Creature c = LoadCreature("Bloodspear Krull.htm","bloodspear krull");
            Assert.IsTrue(CreaturesEqual(c));
        }
        [TestMethod]
        [DeploymentItem("data\\alesia gravelstoke.htm")]
        public void AlesiaGravelStoke()
        {//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=5770

            Creature c = LoadCreature("alesia gravelstoke.htm", "alesia gravelstoke");
            Assert.IsTrue(CreaturesEqual(c));
        }

        [Ignore] // bug in masterplan (save mix details and Range for the collapse power) makes this untestable
        [TestMethod]
        [DeploymentItem("data\\Arkhosian Siege Tower.htm")]
        public void ArkhosianSiegeTower()
        {//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=5741

            Creature c = LoadCreature("Arkhosian Siege Tower.htm","Arkhosian Siege Tower");
            Assert.IsTrue(CreaturesEqual(c));
        }

        [TestMethod]
        [DeploymentItem("data\\cassian.htm")]
        public void CassianChena()
        {//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=5620

            Creature c = LoadCreature("cassian.htm", "Cassian d'Cherevan");
            Assert.IsTrue(CreaturesEqual(c, "Bolstering Presence"));
        }

        [TestMethod]
        [DeploymentItem("data\\KoboldSlyBlade.htm")]
        public void KoboldSlyblade()
        {//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=3191
            Creature c = LoadCreature("KoboldSlyBlade.htm", "Kobold Slyblade");
            Assert.IsTrue(CreaturesEqual(c,"Combat Advantage"));
        }

        [TestMethod]
        [DeploymentItem("data\\ChampionOfTiamat.htm")]
        public void ChampionOfTiamat()
        {//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=1417
            Creature c = LoadCreature("ChampionOfTiamat.htm", "Champion of Tiamat, Dragonguard Dragonborn Champion");
            Assert.IsTrue(CreaturesEqual(c,"No Remorse"));
        }

        [TestMethod]
        [DeploymentItem("data\\harpyscreecher.htm")]
        public void HarpyScreecher()
        {//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=2066
            Creature c = LoadCreature("HarpyScreecher.htm","Harpy screecher");
            Assert.IsTrue(CreaturesEqual(c));
        }

        [TestMethod]
        [DeploymentItem("data\\Erzoun.htm")]
        public void Erzoun()
        {//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=5833
            Creature c = LoadCreature("erzoun.htm","erzoun");
            Assert.IsTrue(CreaturesEqual(c,"Deep Shadow"));
        }

        [TestMethod]
        [DeploymentItem("data\\PlagueChaosBeast.htm")]
        public void PlagueChaosBeast()
        {//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=5610
            Creature c = LoadCreature("PlagueChaosBeast.htm","Plague Demon Chaos Beast");
            Assert.IsTrue(CreaturesEqual(c));
        }

        [TestMethod]
        [DeploymentItem("data\\Xander Gravelstoke.htm")]
        public void XanderGravelstoke()
        {//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=5610
            Creature c = LoadCreature("Xander Gravelstoke.htm", "Xander Gravelstoke");
            Assert.IsTrue(CreaturesEqual(c,"Quick Rally"));
        }

        [TestMethod]
        [DeploymentItem("data\\PortalHound.htm")]
        public void PortalHound()
        {//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=5610
            Creature c = LoadCreature("PortalHound.htm", "Portal Hound");
            Assert.IsTrue(CreaturesEqual(c));
        }

        [TestMethod]
        [DeploymentItem("data\\BloodstoneSpider.htm")]
        [ExpectedException(typeof(InvalidStructureException))]
        public void BloodstoneSpider()
        {//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=1455
            Creature c = LoadCreature("BloodstoneSpider.htm", "Bloodstone Spider");
            //no test possible, no level
        }

         [TestMethod]
        [DeploymentItem("data\\SirOakley.htm")]
        [Ignore] //no role can not be imported
        public void SirOakley()
        {//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=5970
            Creature c = LoadCreature("SirOakley.htm", "Sir Oakley (Companion)");
            Assert.IsTrue(CreaturesEqual(c));
        }

         [TestMethod]
         [DeploymentItem("data\\anakoreRender.htm")]
         public void AnakoreRender()
         {//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=5610
             Creature c = LoadCreature("anakoreRender.htm", "Anakore Render");
             Assert.IsTrue(CreaturesEqual(c));
         }
         [TestMethod]
         [DeploymentItem("data\\StormAbishai.htm")]
         public void StormAbishai()
         {//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=1482
             Creature c = LoadCreature("StormAbishai.htm", "Storm Abishai");
             Assert.IsTrue(CreaturesEqual(c));
         }
         [TestMethod]
         [DeploymentItem("data\\SwarmTongueHydra.htm")]
         public void SwarmTongueHydra()
         {//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=
             Creature c = LoadCreature("SwarmTongueHydra.htm", "Swarmtongue Hydra");
             Assert.IsTrue(CreaturesEqual(c));
         }

        [TestMethod]
         [DeploymentItem("data\\rimefireGriffon.htm")]
         public void RimeFireGriffon()
         {//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=389
             Creature c = LoadCreature("rimefireGriffon.htm", "Rimefire Griffon");
             Assert.IsTrue(CreaturesEqual(c));
         }
        [TestMethod]
        [DeploymentItem("data\\AncientAbyssalWurm.htm")]
        public void AncientAbyssalWurm()
        {//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=5116
            Creature c = LoadCreature("AncientAbyssalWurm.htm", "Ancient Abyssal Wurm");
            Assert.IsTrue(CreaturesEqual(c));
        }

        [Ignore] //Masterplan bug
        [TestMethod]
        [DeploymentItem("data\\YoungVolcanicDragon.htm")]
        public void YoungVolcanicDragon()
        {//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=5116
            Creature c = LoadCreature("YoungVolcanicDragon.htm", "Young Volcanic Dragon");
            Assert.IsTrue(CreaturesEqual(c,"Growing Heat"));
        }

        [TestMethod]
        [DeploymentItem("data\\CoilOfZehir.htm")]
        public void CoilOfZehir()
        {//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=5035
            Creature c = LoadCreature("CoilOfZehir.htm", "Coil of Zehir");
            Assert.IsTrue(CreaturesEqual(c));
        }

        [TestMethod]
        [DeploymentItem("data\\Kuo-Toa Lash.htm")]
        public void KuotoaLash()
        {//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=4932
            Creature c = LoadCreature("Kuo-Toa Lash.htm", "Kuo-Toa Lash");
            Assert.IsTrue(CreaturesEqual(c));
        }

        [TestMethod]
        [DeploymentItem("data\\Palebloodfiend.htm")]
        public void Palebloodfiend()
        {//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=4949
            Creature c = LoadCreature("Palebloodfiend.htm", "Pale Bloodfiend");
            Assert.IsTrue(CreaturesEqual(c));
        }

        [Ignore]//Masterplan bug
        [TestMethod]
        [DeploymentItem("data\\Querelian.htm")]
        public void Querelian()
        {//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=5300
            Creature c = LoadCreature("Querelian.htm", "Querelian");
            Assert.IsTrue(CreaturesEqual(c, "Feywild Challenge"));
        }


        [TestMethod]
        [DeploymentItem("data\\Blood Worm.htm")]
        public void BloodWorm()
        {//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=5097
            Creature c = LoadCreature("Blood Worm.htm", "Blood Worm");
            Assert.IsTrue(CreaturesEqual(c));
        }
        [Ignore] //Masterplan bug
        [TestMethod]
        [DeploymentItem("data\\Ultrodemon schemer.htm")]
        public void UltrodemonSchemer()
        {//http://www.wizards.com/dndinsider/compendium/monster.aspx?id=4816
            Creature c = LoadCreature("Ultrodemon schemer.htm", "Ultrodemon schemer");
            Assert.AreEqual(1,Warning.Count);
            Assert.AreEqual("Ultrodemon Schemer : No speed found (Import Not Stopped)", Warning.First());
            c.Movement = "7 phasing";
            Assert.IsTrue(CreaturesEqual(c,"Demonic Authority"));
        }
    }
}
