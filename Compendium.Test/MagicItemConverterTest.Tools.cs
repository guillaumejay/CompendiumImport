using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using Masterplan.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Compendium.Test
{
    public partial class MagicItemConverterTest
    {
        private MagicItem LoadMagicItem(string filename, string trapName)
        {
            HtmlDocument doc = new HtmlDocument();
            Assert.IsTrue(File.Exists(filename), "Missing file " + filename);
            doc.Load(filename);
            MagicItem t = _converter.GetMasterPlanObjectFromDoc(doc,Warning);
            Assert.AreEqual(trapName.ToLower(), t.Name.ToLower());
            return t;
        }

        private bool MagicItemsEqual(MagicItem fromDDI)
        {
            return MagicItemsEqual(fromDDI, FindMagicItem(fromDDI.Name));
        }

        private bool MagicItemsEqual(MagicItem fromDDI, MagicItem fromMP)
        {
            Assert.IsNotNull(fromMP, fromDDI.Name + " not found in " + Libraries.Count + " test libraries");
            Assert.AreEqual(fromMP.Name, fromDDI.Name);
            Assert.AreEqual(fromMP.Type, fromDDI.Type);
            Assert.AreEqual(fromMP.Level,fromDDI.Level);
            return true;
        }
    }
}
