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
    partial class CreatureConverterTest
    {

        private Creature LoadCreature(string filename,string creatureName)
        {
            HtmlDocument doc = new HtmlDocument();
            Assert.IsTrue(File.Exists(filename), "Missing file " + filename);
            doc.Load(filename);
            Creature c = _converter.GetMasterPlanObjectFromDoc(doc,Warning);
            Assert.AreEqual(creatureName,c.Name,true);
            return c;
        }

        private bool CreaturesEqual(Creature fromDDI)
        {
            return CreaturesEqual(fromDDI, FindCreature(fromDDI.Name), null);
        }
        private bool CreaturesEqual(Creature fromDDI, string MpBugPowerName)
        {
            return CreaturesEqual(fromDDI, FindCreature(fromDDI.Name), MpBugPowerName);
        }


        private bool CreaturesEqual(Creature fromDDI, Creature fromMP,string MpBugPowerName)
        {
            Assert.IsNotNull(fromMP, fromDDI.Name + " not found in " + Libraries.Count + " test libraries");
            if (!String.IsNullOrEmpty(MpBugPowerName))
            { // Try to fix a masterplan bug
                CreaturePower correct = 
                    fromMP.CreaturePowers.Single(x => x.Name == MpBugPowerName);
                if (String.IsNullOrEmpty(correct.Details))
                {
                    correct.Details = correct.Range;
                    Assert.IsFalse(String.IsNullOrEmpty(correct.Range));
                    correct.Range = String.Empty;
                }
            }
            return CreaturesEqual(fromDDI, fromMP);
        }
        private bool CreaturesEqual(Creature fromDDI, Creature fromMP)
        {
            Assert.IsNotNull(fromMP,fromDDI.Name + " not found in " + Libraries.Count + " test libraries");
            if (string.IsNullOrEmpty(fromDDI.Tactics))
                fromDDI.Tactics = fromMP.Tactics;
            Assert.AreEqual(fromDDI.Name, fromMP.Name);
            Assert.AreEqual(fromDDI.AC, fromMP.AC);
            Assert.AreEqual(fromDDI.Alignment, fromDDI.Alignment);
            Assert.AreEqual(fromDDI.Will, fromMP.Will);
            Assert.AreEqual(fromDDI.Fortitude, fromMP.Fortitude);
            Assert.AreEqual(fromDDI.Reflex, fromMP.Reflex);
            //Assert.AreEqual(fromDDI.Category, fromMP.Category);
            if (!String.IsNullOrEmpty(fromMP.Details)) // If no details in MP, nocheckl
                Assert.AreEqual(fromMP.Details, fromDDI.Details);
            Assert.AreEqual(fromDDI.HP, fromMP.HP);
            Assert.AreEqual(fromDDI.Info, fromMP.Info);
            Assert.AreEqual(fromDDI.Initiative, fromMP.Initiative);
            Assert.AreEqual(CleanForCompare(fromMP.Keywords), CleanForCompare(fromDDI.Keywords));
            Assert.AreEqual(fromMP.Languages, fromDDI.Languages);
            Assert.AreEqual(fromDDI.Level, fromMP.Level);
            Assert.AreEqual(CleanForCompare(fromMP.Movement), CleanForCompare(fromDDI.Movement));
            Assert.AreEqual(fromMP.Origin, fromDDI.Origin);
            Assert.AreEqual(fromMP.Type, fromDDI.Type, "Type Invalid for " + fromMP.Name);
            Assert.AreEqual(fromMP.Phenotype, fromDDI.Phenotype);
            AreEqual(fromMP.Regeneration, fromDDI.Regeneration);
            Assert.AreEqual(fromMP.Resist, fromDDI.Resist);
            Assert.AreEqual(fromMP.Size, fromDDI.Size);
            Assert.AreEqual(fromMP.Tactics, fromDDI.Tactics);
            Assert.AreEqual(fromMP.Vulnerable, fromDDI.Vulnerable);
            Assert.AreEqual(fromDDI.URL, fromMP.URL);
            AreEqual(fromMP.Role, fromDDI.Role);
            AreAurasEqual(fromMP.Auras, fromDDI.Auras);
            AreDamageEqual(fromMP.DamageModifiers, fromDDI.DamageModifiers);
            TestSkillAndSenses(fromMP, fromDDI);

            Assert.AreEqual(CleanForCompare(fromMP.Keywords), CleanForCompare(fromDDI.Keywords));

            TestAbilities(fromDDI, fromMP);
            Assert.AreEqual(fromMP.CreaturePowers.Count, fromDDI.CreaturePowers.Count);
            foreach (CreaturePower mpcp in fromMP.CreaturePowers)
            {
                IEnumerable<CreaturePower> ddicps = fromDDI.CreaturePowers.Where(x => x.Name == mpcp.Name.Replace("’", "'").Trim());
                Assert.IsTrue(ddicps.Count() > 0);
                CreaturePower ddicp = ddicps.ElementAt(0);
                if (ddicps.Count() > 1)
                {
                    ddicp = ddicps.Single(x => x.Range == mpcp.Range);
                }
                AreEqual(mpcp.Action, ddicp.Action, ddicp.Name);
                Assert.AreEqual(mpcp.Category, ddicp.Category);
                Assert.AreEqual(mpcp.Condition, ddicp.Condition,true, "Invalid Condition for " + mpcp.Name);
                Assert.AreEqual(mpcp.Damage, ddicp.Damage);
                Assert.AreEqual(mpcp.Description, ddicp.Description);
                Assert.AreEqual(CleanForCompare(mpcp.Details), CleanForCompare(ddicp.Details),"Invalid Details for " + mpcp.Name);
                Assert.AreEqual(mpcp.Keywords, ddicp.Keywords, "Power Keywords");
                if (!(ddicp.Range == "Melee" && string.IsNullOrEmpty(mpcp.Range)))
                    Assert.AreEqual(mpcp.Range.Trim(), ddicp.Range);
                Assert.AreEqual(CleanForCompare(mpcp.Details), CleanForCompare(ddicp.Details));
                Assert.AreEqual(CleanForCompare(mpcp.ToString()), CleanForCompare(ddicp.ToString()));

                AreEqual(mpcp.Attack, ddicp.Attack, String.Format("{0} / {1}", mpcp.Name, fromMP.Name));
            }
            Assert.AreEqual(CleanForCompare(fromMP.Equipment), CleanForCompare(fromDDI.Equipment));
            Assert.AreEqual(CleanForCompare(fromDDI.Immune), CleanForCompare(fromMP.Immune));
            return true;
        }

        private void AreDamageEqual(List<DamageModifier> mp, List<DamageModifier> ddi)
        {
            Assert.AreEqual(mp.Count, ddi.Count);
            foreach (DamageModifier dMP in mp)
            {
                DamageModifier dDDI = ddi.SingleOrDefault(x => x.Type == dMP.Type);
                Assert.IsNotNull(dDDI);
                Assert.AreEqual(dMP.Value, dDDI.Value);
                Assert.AreEqual(dMP.Type, dDDI.Type);
                Assert.AreEqual(dMP.ToString(), dDDI.ToString());
            }
        }

        private void AreAurasEqual(List<Aura> mp, List<Aura> ddi)
        {
            Assert.AreEqual(mp.Count, ddi.Count);
            foreach (Aura aMP in mp)
            {
                Aura aDDI = ddi.SingleOrDefault(x => x.Name == aMP.Name.Trim());
                Assert.IsNotNull(aDDI);
                Assert.AreEqual(aMP.Keywords, aDDI.Keywords,String.Format("Different Keyword for aura {0}",aMP.Name));
                Assert.AreEqual(aMP.ToString(), aDDI.ToString());
            }
        }

        private void TestSkillAndSenses(Creature fromMP, Creature fromDDI)
        {
            List<string> skillsMP =
                fromMP.Skills.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<string> skillsDDI = fromDDI.Skills.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            for (int i = 0; i < skillsDDI.Count; i++)
            {
                skillsDDI[i] = skillsDDI[i].Trim();
            }
            skillsMP.ForEach(x => x.Trim());
            int nbMPs = skillsMP.Count();
            string senses = fromMP.Senses;
            if (senses.Contains("Perception +"))
            {
                nbMPs++;
                string[] sensA = senses.Split(new char[] { ';' }, 2);
                skillsMP.Add(sensA[0]);

                senses = (sensA.Count() == 1) ? "" : sensA[1].Trim();
                Assert.IsTrue(skillsMP[skillsMP.Count - 1].StartsWith("Perception +"));
            }
            Assert.AreEqual(nbMPs, skillsDDI.Count(), "Different skill numbers");
            foreach (string skill in skillsMP)
            {
                Assert.IsTrue(skillsDDI.Contains(skill.Trim()));
            }
            Assert.AreEqual(senses, fromDDI.Senses,true);
        }

        private void AreEqual(Regeneration ddi, Regeneration mp)
        {
            if (mp == null)
            {
                Assert.IsNull(ddi);
                return;
            }
            Assert.AreEqual(ddi.Details, mp.Details);
            Assert.AreEqual(ddi.Value, mp.Value);
            Assert.AreEqual(ddi.ToString(), mp.ToString());
        }

        private void TestAbilities(Creature fromDDI, Creature fromMP)
        {
            AreEqual(fromMP.Charisma, fromDDI.Charisma);
            AreEqual(fromMP.Strength, fromDDI.Strength);
            AreEqual(fromMP.Wisdom, fromDDI.Wisdom);
            AreEqual(fromMP.Intelligence, fromDDI.Intelligence);
            AreEqual(fromMP.Dexterity, fromDDI.Dexterity);
            AreEqual(fromMP.Constitution, fromDDI.Constitution);
        }

        private string CleanForCompare(string p)
        {
            if (p == null)
                return String.Empty;
            p = p.Replace("–", "-");
            return p.Replace(",", "").Replace(";", "").Replace(" ", "").Replace("’", "").Replace("'", "").Trim().ToLower();
        }

        private void AreEqual(Ability ddi, Ability mp)
        {
            if (mp == null)
            {
                Assert.IsNull(ddi);
                return;
            }
            Assert.AreEqual(ddi.Cost, mp.Cost);
            Assert.AreEqual(ddi.Modifier, mp.Modifier);
            Assert.AreEqual(ddi.Score, mp.Score);
        }
        
        protected void AreEqual(PowerAction mp, PowerAction ddi, string name)
        {
            if (mp == null)
            {
                Assert.IsNull(ddi, "Action not null " + name);
                return;
            }
            Assert.IsNotNull(ddi, "Missing action for " + name);
            Assert.AreEqual(mp.Action, ddi.Action, "Invalid action Type for " + name);
            Assert.AreEqual(mp.Recharge, ddi.Recharge);
            Assert.AreEqual(mp.SustainAction, ddi.SustainAction);
            Assert.AreEqual(mp.Trigger, ddi.Trigger);
            Assert.AreEqual(mp.Use, ddi.Use,"Invalid use for action " + name);
            Assert.AreEqual(mp.ToString(), ddi.ToString());
        }

        public void TestFirstBlock(Creature c, int HP, int init, int AC, int Fortitude, int Reflex, int Will, string speed)
        {
            Assert.AreEqual(HP, c.HP);
            Assert.AreEqual(init, c.Initiative);
            Assert.AreEqual(AC, c.AC);
            Assert.AreEqual(Fortitude, c.Fortitude);
            Assert.AreEqual(Reflex, c.Reflex);
            Assert.AreEqual(Will, c.Will);
            Assert.AreEqual(speed, c.Movement);
        }

        //private void TestComplexRole(IRole iRole, RoleType roleType, RoleFlag rf, bool isLeader)
        //{
        //    ComplexRole cr = iRole as ComplexRole;
        //    Assert.IsNotNull(cr);
        //    Assert.AreEqual(isLeader, cr.Leader);
        //    Assert.AreEqual(roleType, cr.Type);
        //    Assert.AreEqual(rf, cr.Flag);
        //}

        //private static void TestMinion(Creature c, bool HasRole, RoleType? roleType)
        //{
        //    Minion cr = (Minion)c.Role;
        //    Assert.AreEqual(HasRole, cr.HasRole);
        //    if (roleType != null)
        //    {
        //        Assert.IsTrue(cr.HasRole);
        //        Assert.AreEqual(roleType, cr.Type);
        //    }
        //    else
        //    {
        //        Assert.IsFalse(cr.HasRole);
        //    }
        //}

        private void TestSizeOriginType(Creature c, CreatureSize creatureSize, CreatureOrigin creatureOrigin, CreatureType creatureType, int level, string keyword)
        {
            Assert.AreEqual(creatureSize, c.Size);
            Assert.AreEqual(creatureOrigin, c.Origin);
            Assert.AreEqual(creatureType, c.Type);
            Assert.AreEqual(level, c.Level);
            Assert.AreEqual(CleanForCompare(keyword), CleanForCompare(c.Keywords));
        }

    }
}
