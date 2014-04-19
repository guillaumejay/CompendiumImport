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
    public partial class TrapConverterTest
    {
        private Trap LoadTrap(string filename, string trapName)
        {
            HtmlDocument doc = new HtmlDocument();
            Assert.IsTrue(File.Exists(filename), "Missing file " + filename);
            doc.Load(filename);
            Trap t = _converter.GetMasterPlanObjectFromDoc(doc,Warning);
            Assert.AreEqual(trapName, t.Name,true);
            return t;
        }

        private bool TrapsEqual(Trap fromDDI)
        {
            return TrapsEqual(fromDDI, FindTrap(fromDDI.Name));
        }

        private bool TrapsEqual(Trap fromDDI, Trap fromMP)
        {
            Assert.IsNotNull(fromMP, fromDDI.Name + " not found in " + Libraries.Count + " test libraries");
            Assert.AreEqual(fromMP.Name, fromDDI.Name);
            Assert.AreEqual(fromMP.Type, fromDDI.Type);
            AreEqual(fromMP.Role, fromDDI.Role);
            Assert.AreEqual(fromMP.Level,fromDDI.Level);
            Assert.AreEqual(fromMP.Details, fromDDI.Details, "Different details");
            Assert.AreEqual(fromMP.ReadAloud,fromDDI.ReadAloud);
            foreach (var tsdMP in fromMP.Skills)
            {
                TrapSkillData tsdDDi =
                    fromDDI.Skills.SingleOrDefault(x => x.DC == tsdMP.DC && x.SkillName == tsdMP.SkillName);
                Assert.IsNotNull(tsdDDi,tsdMP.ToString() + " not found");
                Assert.AreEqual(tsdMP.Details,tsdDDi.Details);
                Assert.AreEqual(tsdMP.ToString(),tsdDDi.ToString());
            }
            AreEqual(fromMP.Attack,fromDDI.Attack, fromDDI.Name);
            CollectionAssert.AreEqual(fromMP.Countermeasures,fromDDI.Countermeasures);
            return true;
        }

        private void AreEqual(TrapAttack fromMP, TrapAttack fromDDI,string name)
        {
            if (fromMP.HasInitiative)
            {
                Assert.IsTrue(fromDDI.HasInitiative);
                Assert.AreEqual(fromMP.Initiative,fromDDI.Initiative);
            }
            else
            {
                Assert.IsFalse(fromDDI.HasInitiative);
                Assert.AreEqual(0,fromDDI.Initiative);
            }
            Assert.AreEqual(fromMP.Trigger,fromDDI.Trigger);
            Assert.AreEqual(fromMP.Action,fromDDI.Action);
            Assert.AreEqual(fromMP.Range,fromDDI.Range);
            Assert.AreEqual(fromMP.Target,fromDDI.Target);
            AreEqual(fromMP.Attack,fromDDI.Attack,name);
            Assert.AreEqual(fromMP.OnHit,fromDDI.OnHit);
            Assert.AreEqual(fromMP.OnMiss, fromDDI.OnMiss,true);
            Assert.AreEqual(fromMP.Effect,fromDDI.Effect);
            Assert.AreEqual(fromMP.ToString(),fromDDI.ToString());

        }
    }
}
