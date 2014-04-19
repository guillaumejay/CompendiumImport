using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CompendiumImport.Tools;
using HtmlAgilityPack;
using Masterplan.Data;

namespace CompendiumImport.Data.Converters
{
    internal class TrapConverter : Converter<Trap>
    {
        #region Regular Expressions for search
        public static Regex _regSkillLine = new Regex(@"DC (\d+): (.+)", RegexOptions.IgnoreCase);
        public static Regex _regInitiative = new Regex(@"<b>Initiative</b> \+(\d)+", RegexOptions.IgnoreCase);
        public static Regex _regActionType = new Regex(@"(Standard|Free|Reaction|Minor|Move|Interrupt|Opportunity)", RegexOptions.IgnoreCase);
        private static Regex _regSkillName = new Regex(@"Perception|Arcana", RegexOptions.IgnoreCase);
        private static Regex _regRange = new Regex(@"(Ranged|Area|Close Blast|Melee) (\d+)(.*)", RegexOptions.IgnoreCase);
        #endregion
        
        public override Trap GetMasterPlanObjectFromDoc(HtmlAgilityPack.HtmlDocument doc, List<string> warnings)
        {
            Trap t = new Trap();
            HtmlNode detailNode = doc.DocumentNode.SelectSingleNode("//div[attribute::id='detail']");
            HtmlNode trapNode = doc.DocumentNode.SelectSingleNode("//h1[attribute::class='trap']");
            if (trapNode == null)
            {
                trapNode = doc.DocumentNode.SelectSingleNode("//h1[attribute::class='thHead']");
                t.Name = GetTextUntilTag(trapNode.InnerHtml, "");
                throw new InvalidStructureException("Data format not yet supported",t.Name);
            }
            t.Name = GetTextUntilTag(trapNode.InnerHtml, "");
            HtmlNode current = trapNode.SelectSingleNode("//span[attribute::class='type']");
            t.Type = (TrapType)Enum.Parse(typeof(TrapType), current.InnerText.Trim());
            current = trapNode.SelectSingleNode("//span[@class='level']").ChildNodes[0];
            int level;
            t.Role = GetLevelAndRoleFrom(current.InnerText, out level).Copy();
            t.Level = level;
            current = detailNode.SelectSingleNode("//p[attribute::class='flavor']");
            t.ReadAloud = current.InnerText;
            string TrapLead = CleanHtml(detailNode.SelectSingleNode("//span[attribute::class='traplead']").InnerText);
            if (TrapLead.StartsWith("Trap:"))
                TrapLead = TrapLead.Substring("Trap:".Length).Trim();
            t.Details = TrapLead;
            HtmlNodeCollection trapBlockTitles = detailNode.SelectNodes("//span[attribute::class='trapblocktitle']");
            
            Match match = _regInitiative.Match(detailNode.InnerHtml);
            if (match.Success)
            {
                t.Attack.HasInitiative = true;
                t.Attack.Initiative = Convert.ToInt32(match.Groups[1].Value);
            }
            foreach (HtmlNode node in trapBlockTitles)
            {
                match = _regSkillName.Match(node.InnerText.Trim());
                if (match.Success)
                {
                    ReadSkillBlock(node, t, match.Value);
                    continue;
                }

                if (node.InnerText.Trim().StartsWith("Trigger"))
                {
                    ReadTrigger(node, t);
                    continue;
                }
                if (node.InnerText.Trim().StartsWith("Attack"))
                {
                    ReadAttackBlock(node, t);
                    continue;
                }
                if (node.InnerText.Trim() == "Countermeasures")
                {
                    ReadCounterMeasures(node, t);
                    continue;
                }
                // Published in is not converted
                throw new NotImplementedException(node.InnerText + " not converted");
            }
            return t;
        }

        private void ReadTrigger(HtmlNode node, Trap t)
        {
             string trigger= CleanHtml(node.NextSibling.InnerText);
             if (!String.IsNullOrEmpty(t.Attack.Trigger))
             {
                 t.Attack.Trigger += Environment.NewLine;
             }
            t.Attack.Trigger += trigger;
        }

        private void ReadCounterMeasures(HtmlNode node, Trap t)
        {
            string textCountermeasures = node.NextSibling.InnerHtml;
            string cm;
            int pos = textCountermeasures.IndexOf("images/bullet.gif");
            while (pos > -1)
            {
                pos = textCountermeasures.IndexOf(">", pos) + 1;
                cm = GetTextUntilTag(textCountermeasures.Substring(pos), "");
                t.Countermeasures.Add(cm);
                pos = textCountermeasures.IndexOf("images/bullet.gif",pos);
            }
        }

        /// <summary>
        /// Reads the attack block.
        /// </summary>
        /// <param name="node">Current node.</param>
        /// <param name="t">Current Trap.</param>
        private void ReadAttackBlock(HtmlNode node, Trap t)
        {
            HtmlNode block = node.NextSibling;
            Match match = _regActionType.Match(block.InnerText);
            if (!match.Success)
                throw new ArgumentException(String.Format("Invalid action type string : {0} for {1}", block.InnerText, t.Name));
            t.Attack.Action = (ActionType)Enum.Parse(typeof(ActionType), match.Value);

            t.Attack.Range = GetTextUntilTag(block.InnerHtml, "<b>", 2);
            match = _regRange.Match(block.InnerText);
            if (match.Success)
            {
                t.Attack.Range = match.Value;
            }
            else
            {
                throw new InvalidStructureException(t.Name,"No range found");
            }
            block = block.NextSibling;
            t.Attack.Target = GetTextUntilTag(block.InnerHtml, "<b>Target: </b>");
            block = block.NextSibling;
            t.Attack.Attack = TryToGetAttackFrom(block.InnerText);
            if (t.Attack.Attack == null)
            {
                throw new InvalidStructureException("No attack",t.Name);
            }
            block = block.NextSibling; //Hit
            t.Attack.OnHit = GetTextUntilTag(block.InnerHtml, "<b>Hit: </b>");
            block = block.NextSibling;//Miss
            t.Attack.OnMiss = GetTextUntilTag(block.InnerHtml, "<b>Miss: </b>");

        }

        /// <summary>
        /// Reads a skill block.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="node">Current node.</param>
        /// <param name="t">Current Trap.</param>
        private void ReadSkillBlock(HtmlNode node, Trap t, string skillName)
        {
            while ((node.NextSibling != null) && (node.NextSibling.Attributes["class"].Value == "trapblockbody"))
            {
                node = node.NextSibling;
                t.Skills.Add(ReadSkill(node.InnerText, skillName));
            }

        }

        /// <summary>
        /// Reads skill value DC xx: Details
        /// </summary>
        /// <param name="p">text</param>
        /// <param name="skillName">Skill name.</param>
        /// <returns>TrapSkillData filled</returns>
        private TrapSkillData ReadSkill(string p, string skillName)
        {
            TrapSkillData tsd = new TrapSkillData();
            Match match = _regSkillLine.Match(p);
            tsd.DC = Convert.ToInt32(match.Groups[1].Value);
            tsd.SkillName = skillName;
            tsd.Details =CleanHtml( match.Groups[2].Value);
            return tsd;
        }
    }
}
