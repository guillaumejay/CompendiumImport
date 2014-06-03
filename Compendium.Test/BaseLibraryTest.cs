using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Masterplan.Data;
using System.IO;
using Utils;

namespace Compendium.Test
{
    /// <summary>
    /// Summary description for BaseLibraryTest
    /// </summary>
    [TestClass]
    public abstract class BaseLibraryTest
    {

        public List<String> Warning { get; set; }
        public BaseLibraryTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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
        protected static List<Library> Libraries = new List<Library>();

        protected static void InitLibraries()
        {
            Libraries.Clear();
            foreach (string fi in Directory.GetFiles(".", "*.library"))
            {
                AddLibrary(fi);
            }
            if (!Libraries.Any())
            {
                throw new Exception("No library found in " + Directory.GetCurrentDirectory());
            }
        }
        protected static void AddLibrary(string fileName)
        {
            Library library;
            library = Serialisation<Library>.Load(fileName, SerialisationMode.Binary);
            Assert.IsNotNull(library, "Missing library : " + fileName);
            Libraries.Add(library);
        }
        protected Creature FindCreature(string name)
        {
            Creature c = null;
            for (int i = 0; i < Libraries.Count; i++)
            {
                Creature c2 = Libraries[i].FindCreature(name);
                if (c != null)
                {
                    if (name == "TestImport.Library") // has Priority
                        c = c2;
                }
                if (c2 != null)
                    c = c2;
            }
            return c;
        }
        protected void AreEqual(IRole fromMP, IRole fromDDI)
        {
            Assert.AreEqual(fromMP.GetType().FullName, fromDDI.GetType().FullName);
            if (fromMP.GetType().ToString() == typeof(Minion).ToString())
            {
                Minion mp = fromMP as Minion;
                Minion ddi = fromDDI as Minion;
                Assert.AreEqual(mp.HasRole, ddi.HasRole);
                Assert.AreEqual(mp.Type, ddi.Type);
            }
            else
            {
                ComplexRole mp = fromMP as ComplexRole;
                ComplexRole ddi = fromDDI as ComplexRole;
                Assert.AreEqual(mp.Flag, ddi.Flag);
                Assert.AreEqual(mp.Leader, ddi.Leader);
                Assert.AreEqual(mp.Type, ddi.Type);
            }

        }
        protected Trap FindTrap(string name)
        {
            Trap c = null;
            for (int i = 0; i < Libraries.Count; i++)
            {
                Trap c2 = Libraries[i].FindTrap(name);
                if (c != null)
                {
                    if (name == "TestImport.Library") // has Priority
                        c = c2;
                }
                if (c2 != null)
                    c = c2;
            }
            return c;
        }

        protected MagicItem FindMagicItem(string name)
        {
            MagicItem c = null;
            for (int i = 0; i < Libraries.Count; i++)
            {
                MagicItem c2 = Libraries[i].FindMagicItem(name);
                if (c != null)
                {
                    if (name == "TestImport.Library") // has Priority
                        c = c2;
                }
                if (c2 != null)
                    c = c2;
            }
            return c;
        }

        protected void AreEqual(PowerAttack mp, PowerAttack ddi, string Name)
        {
            if (mp == null)
            {
                Assert.IsNull(ddi, "Attack not null " + Name);
                return;
            }
            else
            {
                Assert.IsNotNull(ddi, "Attack null for " + Name);
            }
            Assert.AreEqual(mp.Bonus, ddi.Bonus);
            Assert.AreEqual(mp.Defence, ddi.Defence);
            Assert.AreEqual(mp.ToString(), ddi.ToString());
        }
        #region Additional test attributes

        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        public virtual void MyTestInitialize()
        {
            Warning = new List<string>();
        }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion
    }
}
