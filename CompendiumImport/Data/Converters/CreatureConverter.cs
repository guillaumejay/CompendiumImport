using System;
using System.Collections.Generic;
using System.Linq;
using Masterplan.Data;
using HtmlAgilityPack;
using System.Diagnostics;
using CompendiumImport.Tools;
using System.Text.RegularExpressions;

namespace CompendiumImport.Data.Converters
{
    public class CreatureConverter : Converter<Creature>
    {
        #region  Regular Expressions for conversion
        private static string[] _actionTypes = EnumUtility.GetValue(typeof(ActionType));
        private static string[] _powerUseTypes = EnumUtility.GetValue(typeof(PowerUseType));
        private static Regex _regRange = new Regex("(^melee )|(^close burst )|(^area burst )|(^close blast)|(^ranged )", RegexOptions.IgnoreCase);
        private static Regex _regAlignement = new Regex(@"Alignment (chaotic evil|unaligned|evil|Lawful Good|Good|Any)", RegexOptions.IgnoreCase);
        private static Regex _RegLanguagesHtml = new Regex("<b>( )?Languages( )?</b>", RegexOptions.IgnoreCase);
        private static Regex _regRecharge = new Regex(@"(<b>)?( )?Recharge(s on)?( |</b>)?(\d|\<img src=| when | if | , or recharge \d while bloodied| at the start| at the end of the| on a miss)", RegexOptions.IgnoreCase);
        private static Regex _regBasicAttack = new Regex("symbol/S3.gif|symbol/S2.gif", RegexOptions.IgnoreCase);
        private static Regex _regConditionsFrequency = new Regex(@"\d/round|\d/encounter|usable (once|twice) per encounter", RegexOptions.IgnoreCase);
        private static Regex _regTriggerWhen = new Regex(@"(,|;|\() when (\w|\s)+(;|,|\))", RegexOptions.IgnoreCase);
        private static Regex _regRequires = new Regex(@"((,|;|\() requires (\w|\s)+(;|,|\)))|(Requirement: (?<reg>[A-z .]+))", RegexOptions.IgnoreCase|RegexOptions.ExplicitCapture);
        private static Regex _regPowerUsage = new Regex(@" (Encounter|at-will|daily)", RegexOptions.IgnoreCase);
        #endregion

        protected string CurrentName { get; set; }

        public override Creature GetMasterPlanObjectFromDoc(HtmlDocument doc, List<string> warnings)
        {
            string temp, temp2;
            int pos;
            string[] tempArray;
            Creature c = new Creature();
            if (doc.DocumentNode.InnerText.Contains("Get full access"))
                throw new Exception("Stuck on login page !");
            HtmlNode monsterNode = doc.DocumentNode.SelectSingleNode("//h1[attribute::class='monster']");
            HtmlNode detailNode = doc.DocumentNode.SelectSingleNode("//div[attribute::id='detail']");
            HtmlNode levelBlock= monsterNode.SelectSingleNode("//span[@class='level']");
            c.Name = CleanHtml(monsterNode.FirstChild.InnerText);
            CurrentName = c.Name;
            if (levelBlock == null)
                throw new InvalidStructureException("no level found", c.Name);
            HtmlNode current = monsterNode.SelectSingleNode("//span[@class='type']");
            temp = current.InnerText;
            if (temp.Contains("("))
            {
                tempArray = temp.Split(new string[] { "(" }, 2, StringSplitOptions.RemoveEmptyEntries);
                c.Keywords = CleanParenthesis(tempArray[1]);
                temp = tempArray[0].Trim();
            }
            if (temp.Contains(",")) //keywords
            {
                Trace.Assert(String.IsNullOrEmpty(c.Keywords));
                tempArray = temp.Split(new string[] { "," }, 2, StringSplitOptions.RemoveEmptyEntries);
                c.Keywords = CleanParenthesis(tempArray[1].Trim());
                temp = tempArray[0].Trim();
            }
            temp = temp.Replace("magical beast", "MagicalBeast");
            tempArray = temp.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (tempArray.Count() < 3)
            {
                if (! HardFix(c))
                   SetPhenotypeValues(tempArray, c);
            }
            else
            {
                SetPhenotypeValues(tempArray, c);
            }
            

            int level;
            c.Role = GetLevelAndRoleFrom(levelBlock.InnerText, out level).Copy();
            c.Level = level;
           
            HtmlNodeCollection paragraphs = monsterNode.SelectNodes("//h2");
            MonsterStatType version = MonsterStatType.Old;
            if (paragraphs != null)
                version = MonsterStatType.MM3;
            ExtractAurasFromHtml(c, detailNode.InnerHtml, version);
            Match match = _regAlignement.Match(detailNode.InnerText);
            c.Alignment = CleanHtml(match.Value.Substring(match.Value.IndexOf(" ")));
            Debug.Assert(!String.IsNullOrEmpty(c.Alignment));
            match = _RegLanguagesHtml.Match(detailNode.InnerHtml);
            Debug.Assert(match.Success);
            c.Languages = GetTextUntilTag(detailNode.InnerHtml.Substring(match.Index + match.Value.Length), "");
            if (c.Languages == "-") c.Languages = "";
            temp = detailNode.InnerHtml;
            c.HP = GetIntValue(temp, "<b>HP</b>");
            c.Initiative = GetIntValue(temp, "<b>Initiative</b>");
            c.AC = GetIntValue(temp, "<b>AC</b>");
            c.Fortitude = GetIntValue(temp, "<b>Fortitude</b>");
            c.Reflex = GetIntValue(temp, "<b>Reflex</b>");
            c.Will = GetIntValue(temp, "<b>Will</b>");
            c.Movement=GetSpeed(temp,warnings);
            
            c.Immune = GetTextUntilTag(temp, "<b>Immune</b>") ?? String.Empty;
            AddVulnerab(c, temp);
            SetRegeneration(c, temp);
            temp2 = GetTextUntilTag(temp, "<b>Senses</b>");
            if (string.IsNullOrEmpty(temp2))
            {
                temp2 = GetTextUntilTag(temp, "<b>Perception</b>");
            }
            
            temp2 = temp2.Replace("Perception", "").Trim();
            if (string.IsNullOrEmpty(temp2)) // Xander GravelStoke is wrong in compendium (2011/10/22)
            {
                c.Skills = "Perception Invalid";    
            }
            else
            {
                c.Skills = "Perception " + AsBonus(GetIntValue(temp2, null));    
            }
            
            if (temp2.Contains(';'))
            {
                c.Senses = Upper1(CleanHtml(temp2.Substring(temp2.IndexOf(";") + 1)));
            }
            else
            {
                if (version == MonsterStatType.MM3)
                {
                    pos = temp.IndexOf("<b>Perception</b>");
                    temp2 = temp.Substring(pos);
                    c.Senses = GetTextUntilTag(temp2, "<td class=\"rightalign\">");
                }
            }
            AddResist(c, temp);
            ReadPowers(c, monsterNode, version);

            if (c.Equipment.EndsWith("."))
                c.Equipment = c.Equipment.Substring(0, c.Equipment.Length - 1).Trim();
            //if (c.Skills.Trim().EndsWith(","))
            //    c.Skills = c.Skills.Trim().Substring(0, c.Skills.Trim().Length - 1);
            return c;
        }

        private void SetPhenotypeValues(string[] tempArray, Creature c)
        {
            c.Size = (CreatureSize) Enum.Parse(typeof (CreatureSize), tempArray[0]);
            c.Origin = (CreatureOrigin) Enum.Parse(typeof (CreatureOrigin), Upper1(tempArray[1]));
            c.Type = (CreatureType) Enum.Parse(typeof (CreatureType), Upper1(tempArray[2]));
        }

        private string GetSpeed(string temp, List<string> warnings)
        {
            string t = String.Empty;
            if (temp.Contains("<b>Speed</b>"))
            {
                t=GetTextUntilTag(temp, "<b>Speed</b>");    
            }
            else
            {
                warnings.Add(String.Format("{0} : No speed found (Import Not Stopped)",CurrentName));
            }
            return t;
        }

        /// <summary>
        /// Fix special case (bugs in database compendium)
        /// </summary>
        /// <param name="c">Current creature</param>
        /// <returns>true if fixed</returns>
        private bool HardFix(Creature c)
        {
            if (c.Name == "Coil of Zehir")
            {
                c.Size = CreatureSize.Large;
                c.Origin = CreatureOrigin.Natural;
                c.Type = CreatureType.MagicalBeast;
                return true;
            }
            return false;
        }

        private void SetRegeneration(Creature c, string temp)
        {
            string temp2;
            string[] tempArray;
            temp2 = GetTextUntilTag(temp, "<b>Regeneration</b>");
            if (!String.IsNullOrEmpty(temp2))
            {
                Regeneration rg = new Regeneration();
                rg.Value = GetIntValue(temp2, null);
                tempArray = temp2.Split(new char[] {' '}, 2);
                if (tempArray.Count() == 2)
                {
                    // Draconomicon Metallic Storm Abissai is just "Regeneration 5"
                    rg.Details = tempArray[1].Trim();
                    if (rg.Details.StartsWith("("))
                        rg.Details = rg.Details.Substring(1);
                    if (rg.Details.EndsWith(")"))
                        rg.Details = rg.Details.Substring(0, rg.Details.Length - 1);
                }
                c.Regeneration = rg;
            }
        }


        private void AddVulnerab(Creature c, string html)
        {
            c.Vulnerable = ExtractResistVulnerability(c, html, "<b>Vulnerable</b>", 1);
        }

        /// <summary>
        /// Extracts the resistance or vulnerability : as Damage modifier if possible 
        /// </summary>
        /// <param name="c">Current creature.</param>
        /// <param name="html">The HTML.</param>
        /// <param name="toSearch">To search.</param>
        /// <param name="multiply">1 for resistance, -1 for vulnerability </param>
        /// <returns>the vulnerability or resistance, if not converted to damage modifier</returns>
        private string ExtractResistVulnerability(Creature c, string html, string toSearch, int multiply)
        {
            string stringValue = String.Empty; //will be put directly in Resistance, Vulnerability
            string temp = GetTextUntilTag(html, toSearch);
            if (temp == null)
            {
                return String.Empty;
            }
            if (temp.EndsWith(";"))
                temp = temp.Substring(0, temp.Length - 1);
            string[] rA = temp.Split(',');
            foreach (string currentString in rA)
            {
                bool result;
                DamageModifier dm = new DamageModifier();
                string[] tempA = currentString.Split(new char[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
                int value;
                result = Int32.TryParse(tempA[0], out  value);
                if (!result)
                {
                    if (!String.IsNullOrEmpty(stringValue))
                        stringValue += ", ";
                    stringValue += currentString;
                }
                else
                {
                    if (tempA.Count() == 1)
                        dm.Type = DamageType.Untyped;
                    else
                    {
                        DamageType dmType;
                        if (EnumUtility.TryParse<DamageType>(Upper1(tempA[1]), out dmType))
                        {
                            dm.Type = dmType;
                        }
                        else
                        {
                            dm = null;
                            if (!String.IsNullOrEmpty(stringValue))
                                stringValue += ", ";
                            stringValue += currentString;
                        }
                    }
                    if (dm != null)
                    {
                        dm.Value = Convert.ToInt32(value) * multiply;
                        c.DamageModifiers.Add(dm);
                    }
                }
            }
            return stringValue;
        }

        private void AddResist(Creature c, string html)
        {
            c.Resist = ExtractResistVulnerability(c, html, "<b>Resist</b>", -1);
        }

        /// <summary>
        /// Extracts the auras from HTML.
        /// </summary>
        /// <param name="c">Current Creature.</param>
        /// <param name="html">The HTML.</param>
        /// <param name="version">The version.</param>
        private void ExtractAurasFromHtml(Creature c, string html, MonsterStatType version)
        {
            int auraStartsAt = GetAuraPos(html, 0);
            while (auraStartsAt > 0)
            {
                int pos2 = html.ToLower().LastIndexOf("<b>", auraStartsAt); // farther than the name, so go back to find it
                if (pos2 == -1)
                    break;
                Aura aura = new Aura();
                aura.Name = GetTextUntilTag(html.Substring(pos2), "<b>");
                string temp = html.Substring(pos2 + aura.Name.Length + 7).Trim();
                if (temp.StartsWith(">"))
                    temp = temp.Substring(1).Trim();
                int pos;
                if (temp.StartsWith("(")) //keyword
                {
                    pos = temp.IndexOf(")");
                    aura.Keywords = Upper1(CleanHtml(temp.Substring(1, pos - 1)));
                }

                if (version == MonsterStatType.MM3)
                {
                    pos = temp.ToLower().IndexOf("<b>aura</b>") + "<b>aura</b>".Length;
                }
                else
                {
                    pos = temp.ToLower().IndexOf("aura ") + "aura ".Length;
                }
                temp = temp.Substring(pos).Trim().Replace("  ", " ");
                aura.Details = GetTextUntilTag(temp, null);
                pos2 = aura.Details.IndexOf(";");
                if (pos2 < 5)
                {
                    if (pos2 == -1)
                    { // for postMM3
                        pos2 = temp.IndexOf("<p ");
                        pos2 = temp.IndexOf(">", pos2) + 1;
                        temp = temp.Substring(pos2);
                        temp = GetTextUntilTag(temp, null);
                        aura.Details += ": " + temp;
                        auraStartsAt += aura.Details.Length + 40; // text + img link size
                    }
                    else
                    {
                        aura.Details = aura.Details.Remove(pos2, 1).Insert(pos2, ":");
                        auraStartsAt = html.ToLower().IndexOf(" aura ", auraStartsAt) + 10 + 1;
                    }
                }
                else
                {
                    auraStartsAt = html.ToLower().IndexOf(" aura ", auraStartsAt) + 10 + 1;
                }
                c.Auras.Add(aura);
                auraStartsAt = GetAuraPos(html, auraStartsAt);
            }
        }

        /// <summary>
        /// Gets Start position for Aura power
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="startPos">start position for search in HTML.</param>
        /// <returns></returns>
        private static int GetAuraPos(string html, int startPos)
        {
            string symbolAuraPng = "symbol/aura.png";
            int pos = html.ToLower().IndexOf(symbolAuraPng, startPos);
            if (pos == -1)
                pos = html.ToLower().IndexOf("</b> aura ", startPos);
            else
            {
                //name is after
                pos = html.ToLower().IndexOf("</b>", pos);
            }
            if (pos == -1)
                pos = html.ToLower().IndexOf(") aura ", startPos);
            return pos;
        }

        private void ReadPowers(Creature c, HtmlNode monsterNode, MonsterStatType versionStat)
        {
            bool IsTrait = false;
            HtmlNode currentPar;
            HtmlNodeCollection paragraphs = null;
            switch (versionStat)
            {
                case MonsterStatType.Old:
                    paragraphs = monsterNode.SelectNodes("//p");//[@class='flavor alt']");

                    break;
                case MonsterStatType.MM3:
                    paragraphs = monsterNode.SelectNodes(("//h2|//p"));
                    int pos = monsterNode.ParentNode.InnerText.IndexOf("Equipment:");
                    if (pos > -1)
                    {
                        pos += "Equipment:".Length;
                        int pos2 = monsterNode.ParentNode.InnerText.IndexOf("Published", pos);
                        c.Equipment = CleanHtml(monsterNode.ParentNode.InnerText.Substring(pos, pos2 - pos));
                    }
                    // currentPar = monsterNode.SelectSingleNode(("//h2"));
                    //currentPar = currentPar.NextSibling;
                    //  Debug.Assert(currentPar.Name == "h2");
                    break;
                default:
                    throw new ArgumentException("Invalid monsterStatType " + versionStat);
            }
            ActionType currentActionType = ActionType.None;

            for (int posParagraph = 0; posParagraph < paragraphs.Count(); posParagraph++)
            {
                currentPar = paragraphs[posParagraph];
                string innerHtmlPar = currentPar.InnerHtml.Trim();
                if (String.IsNullOrEmpty(innerHtmlPar.Trim()))
                {
                    continue;
                }
                if (innerHtmlPar.StartsWith("<b>Initiative</b>"))
                {
                    continue;
                }
                if (innerHtmlPar.StartsWith("<b>Str</b>"))
                {
                    ReadAbilities(c, innerHtmlPar);
                    continue;
                }

                if (innerHtmlPar.StartsWith("<b>Skills</b>"))
                {
                    ManageBlockAlignement(c, currentPar, versionStat);
                    continue;
                }
                if (innerHtmlPar.StartsWith("<b>Alignment") || (currentPar.Name == "b" && innerHtmlPar.StartsWith("Alignment")))
                {
                    ManageBlockAlignement(c, currentPar, versionStat);
                    continue;
                }
                if (innerHtmlPar.StartsWith("<b>Equipment"))// || (currentPar.Name == "b" && innerHtmlPar.StartsWith("Equipment")))
                {
                    ManageBlockEquipment(c, currentPar, versionStat);
                    continue;
                }
                if (innerHtmlPar.StartsWith("<b>Description"))
                {
                    ManageBlockDescription(c, currentPar);
                    currentPar = currentPar.NextSibling;
                    continue;
                }
                if (innerHtmlPar.StartsWith("Published in"))
                {
                    ManageBlockPublished(c, currentPar, versionStat);
                    currentPar = currentPar.NextSibling;
                    continue;
                }
                if (innerHtmlPar.StartsWith("Update"))
                {
                    string update = innerHtmlPar;
                    while (currentPar.NextSibling != null && currentPar.NextSibling.Name != "<p>")
                    {
                        currentPar = currentPar.NextSibling;
                        update += Environment.NewLine + innerHtmlPar;
                    }
                    ManageblockUpdate(c, update, versionStat);
                    continue;
                }
                if (versionStat == MonsterStatType.MM3)
                {
                    if (currentPar.Name == "h2")
                    {
                        IsTrait = innerHtmlPar.Trim() == "Traits";
                        if (innerHtmlPar == "Standard Actions")
                        {
                            currentActionType = ActionType.Standard;
                        }
                        if (innerHtmlPar == "Minor Actions")
                            currentActionType = ActionType.Minor;
                        if (innerHtmlPar == "Move Actions")
                            currentActionType = ActionType.Move;
                        if (innerHtmlPar == "Triggered Actions")
                            currentActionType = ActionType.Reaction;
                        continue;
                    }
                }
                if (currentPar.Name != "p" || (currentPar.Attributes["class"].Value == "flavorIndent") || currentPar.InnerHtml.StartsWith("<i>")) //power details
                {
                    continue;
                }
                if (currentPar.InnerHtml.Contains("<b>Aura</b>"))
                    continue;
                CreaturePower cp = new CreaturePower();
                cp.Name = GetTextUntilTag(innerHtmlPar, "<b>");
                string afterName = GetTextUntilTag(innerHtmlPar, "</b>");
                cp.Keywords = GetTextUntilTag(innerHtmlPar, "<b>", 2) ?? String.Empty;
                string src = String.Empty;
                if (currentPar.FirstChild.Name == "img")
                {
                    src = currentPar.FirstChild.Attributes["src"].Value.ToString();
                }
                cp.Action = new PowerAction();
                if (_regBasicAttack.Match(src).Success)
                {
                    cp.Action.Use = PowerUseType.Basic;
                }
                if (src.Contains("symbol/Z2a.gif"))
                {
                    cp.Range = "Melee";
                }

                string powerTitleHtml = innerHtmlPar;
                currentPar = currentPar.NextSibling;
                switch (versionStat)
                {
                    case MonsterStatType.Old:
                        ReadOldPower(currentPar, cp, String.IsNullOrEmpty(src) //trait should have no action image
                            , afterName, powerTitleHtml);
                        currentPar = currentPar.NextSibling;
                        while ((currentPar != null) && (currentPar.Name == "p") && (currentPar.Attributes["class"] != null) && (currentPar.Attributes["class"].Value.ToLower() == "flavorindent"))
                        { // more details for action
                            cp.Details = cp.Details + Environment.NewLine + CleanHtml(currentPar.InnerText);
                            posParagraph++;    
                            currentPar = currentPar.NextSibling;
                        }
                        break;
                    case MonsterStatType.MM3:
                        if (IsTrait)
                        {
                            cp.Action = null;
                        }
                        else
                        {
                            cp.Action.Action = currentActionType;
                        }
                        currentPar = ReadMM3Power(currentPar, cp, IsTrait, powerTitleHtml.Replace("  ", " "));
                        break;
                    default:
                        throw new ArgumentException("Invalid monsterStatType " + versionStat);
                }

                c.CreaturePowers.Add(cp);
            }
        }

        private void ReadAbilities(Creature c, string innerHtml)
        {
            c.Strength = GetAbility("str", innerHtml);
            c.Charisma = GetAbility("cha", innerHtml);
            c.Wisdom = GetAbility("wis", innerHtml);
            c.Intelligence = GetAbility("int", innerHtml);
            c.Dexterity = GetAbility("dex", innerHtml);
            c.Constitution = GetAbility("con", innerHtml);
        }

        private HtmlNode ReadMM3Power(HtmlNode currentLine, CreaturePower cp, bool IsTrait, string powerTitleHtml)
        {
            string temp;
            cp.Keywords = CleanParenthesis(GetTextUntilTag(powerTitleHtml, "</b>"));
            Match match = _regConditionsFrequency.Match(powerTitleHtml);
            if (match.Success)
            {
                cp.Condition = match.Value.Trim();
                powerTitleHtml = powerTitleHtml.Replace(match.Value, "");
            }
            if (!IsTrait && cp.Action.Use != PowerUseType.Basic)
            {
                string afterName = powerTitleHtml.Replace(",", ";"); 
                match = _regRequires.Match(afterName);
                if (match.Success)
                { //requires
                    cp.Condition = CleanExtraComma(CleanHtml(match.Groups[1].Value));
                    powerTitleHtml = powerTitleHtml.Replace(match.Value, "");
                }
                match = _regRecharge.Match(powerTitleHtml);
                if (match.Success)
                {
                    CalculateRecharge(powerTitleHtml.Substring(match.Index), cp,MonsterStatType.MM3);
                }
                else
                {
                    temp = GetTextUntilTag(powerTitleHtml, "<b>", 2);
                    cp.Action.Use = ActionUseFromText(temp);
                }
            }

            // Read power details
            while (currentLine != null && currentLine.Name == "p" &&
                (currentLine.Attributes["Class"].Value != "flavor alt")) //mean new power
            {
                bool textAffected = false;
                if (currentLine.InnerHtml.ToLower().StartsWith("<i>attack:</i>"))
                {
                    temp = GetTextUntilTag(currentLine.InnerHtml, "</i>");
                    ReadAttackDetailsBlock(temp, cp, "");
                    cp.ExtractAttackDetails();
                    textAffected = true;
                }
                if (currentLine.InnerHtml.ToLower().StartsWith("<i>requirement:</i>"))
                {
                    cp.Condition = GetTextUntilTag(currentLine.InnerHtml, "</i>");
                    textAffected = true;
                }
                if (currentLine.InnerHtml.ToLower().StartsWith("<i>trigger:</i>"))
                {
                    cp.Action.Trigger = GetTextUntilTag(currentLine.InnerHtml, "</i>");
                    textAffected = true;
                }
                if (!textAffected)
                {
                    if (!String.IsNullOrEmpty(cp.Details))
                        cp.Details += Environment.NewLine;
                    cp.Details += CleanHtml(currentLine.InnerText);
                }
                currentLine = currentLine.NextSibling;
            }
            if (cp.Attack == null)
            { // BloodSpear Save Throng Move actions containing an attack in the details text
                cp.Attack= TryToGetAttackFrom(cp.Details);
            }
            return currentLine;
        }

        private void ReadOldPower(HtmlNode currentPar, CreaturePower cp, bool IsTrait, string afterName, string powerTitleHtml)
        {
            string[] tempA;
            string actionUse, action = String.Empty;
            string blockDetail = currentPar.InnerHtml;

            if (!String.IsNullOrEmpty(afterName))
            {
                IsTrait = IsTrait && (false == _powerUseTypes.Any(x => afterName.ToLower().Replace("at-will", "atwill").Contains(x.ToLower())))
                          && (blockDetail.Contains(" vs ") == false) && (afterName.ToLower().Contains(" recharge") == false);
                // trait have no power use neither attack
            }

            if (String.IsNullOrEmpty(afterName) || IsTrait)
            { // Trait
                cp.Action = null;
            }
            else
            {
                afterName = afterName.Replace(",", ";"); // Ogre Warhulk (standard; requires heavy flail, encounterà
                Match match = _regRequires.Match(afterName);
                if (match.Success)
                { //requires
                    cp.Condition = CleanExtraComma(CleanHtml(match.Value));
                    afterName = afterName.Replace(match.Value, ";");
                }
                match = _regTriggerWhen.Match(afterName);
                if (match.Success)
                { // when first bloodied 
                    cp.Action.Trigger = CleanExtraComma(CleanHtml(match.Value));
                    if (cp.Action.Trigger.StartsWith("("))
                        cp.Action.Trigger = cp.Action.Trigger.Substring(1).Trim();
                    afterName = afterName.Replace(match.Value, ";");
                    cp.Action.Action = ActionType.Reaction;
                }
                match = _regPowerUsage.Match(afterName);
                if (match.Success && cp.Action.Use != PowerUseType.Basic)
                {//this could make some further code obsolete
                    cp.Action.Use = ActionUseFromText(match.Value);
                }
                if ((afterName.IndexOf(")") > -1 || afterName.IndexOf("(") > -1) && ((afterName.IndexOf(";") > -1)))
                { // ( , ,) or ;
                    tempA = CleanParenthesis(afterName).Split(new [] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    actionUse = string.Empty;
                    action = String.Empty;
                    if (tempA.Count() > 0)
                    {
                        actionUse = ((tempA.Count() == 1) ? tempA[0] : tempA[1]).Trim();
                        action = tempA[0].Trim();
                        if (tempA.Count() > 1)
                        {
                            if ((tempA.Count() > 2))
                            {
                                if (tempA[2].Trim() == "starts uncharged") //rimfire griffon
                                {
                                    cp.Condition = tempA[2].Trim();
                                    powerTitleHtml = powerTitleHtml.Replace(tempA[2], "");
                                }
                                else
                                {
                                    action += tempA[1]; // trigger condition
                                    actionUse = tempA[2].Trim();
                                }
                            }
                        }

                        GetActionType(cp.Action, action);
                        if (cp.Action.Trigger.ToLower().StartsWith("requires") ||
                            cp.Action.Trigger.ToLower().StartsWith("usable only"))
                        {
                            cp.Condition = cp.Action.Trigger;
                            cp.Action.Trigger = String.Empty;
                        }
                        if (cp.Action.Use != PowerUseType.Basic)
                        {
                            if (actionUse.StartsWith("recharge"))
                            {
                                CalculateRecharge(powerTitleHtml, cp,MonsterStatType.Old);
                                cp.Action.Use = PowerUseType.Encounter;
                            }
                            else
                            {
                                if (actionUse.ToLower().StartsWith("usable ") && actionUse.ToLower().EndsWith("per encounter"))
                                {
                                    // condition is " usable ony>[..] , Encounter"
                                    string[] tempc = actionUse.Split(new [] { ',' },
                                                                     StringSplitOptions.RemoveEmptyEntries);
                                    cp.Condition = tempc[0].Trim();
                                    if (tempc.Count() < 2)
                                    {
                                        actionUse = "At-will";
                                    }
                                    else
                                    {
                                        actionUse = tempc[1].Trim();    
                                    }
                                    
                                }
                                if (String.IsNullOrEmpty(actionUse))
                                {
                                    // charnel cinder house Charnel Pyre ( when first bloodied and again when the charnel cinderhouse is reduced to 0 hit points, ) Fire, Necrotic
                                    cp.Action.Use = PowerUseType.AtWill;
                                    if (cp.Action.Trigger.ToLower().StartsWith("when"))
                                    {
                                        cp.Action.Action = ActionType.Reaction;
                                    }
                                }
                                else
                                {
                                    cp.Action.Use = ActionUseFromText(actionUse);
                                }
                            }
                        }
                    }
                }
            }
            if (IsTrait)
            {
                cp.Condition = CleanParenthesis(afterName) ?? "";
                cp.Action = null;
            }
            ReadAttackDetailsBlock(blockDetail, cp, action);
        }

        private PowerUseType ActionUseFromText(string actionUse)
        {
            if (string.IsNullOrEmpty(actionUse))
            {
                return PowerUseType.Encounter;
            }
            actionUse = actionUse.Replace("()", "");
            return (PowerUseType)Enum.Parse(typeof(PowerUseType), Upper1(actionUse.ToLower().Replace("at-will", "AtWill").Trim()));
        }

        private void ManageblockUpdate(Creature c, string update, MonsterStatType monsterStatType)
        {
        }

        private void ReadAttackDetailsBlock(string html, CreaturePower cp, string action)
        {
            cp.Details = CleanHtml(html);

            if (_regRange.IsMatch(cp.Details))
            {
                string[] temp = cp.Details.Split(new char[] { ';' }, 2, StringSplitOptions.RemoveEmptyEntries);
                cp.Details = temp[1].Trim();
                cp.Range = temp[0].Trim();
            }

            if (cp.Action != null) // not a trait
            {
                cp.ExtractAttackDetails();
                Match match = _regConditionsFrequency.Match(cp.Action.Trigger);
                if (match.Success)
                { // Charnel Cinderhouse Crushing Prison (free 1/round, at-will)
                    cp.Condition = cp.Action.Trigger;
                    cp.Action.Trigger = String.Empty;
                }
                if (action.ToLower().StartsWith("when ") && String.IsNullOrEmpty(cp.Action.Trigger))
                { // Charnel Cinderhouse Crushing Prison /Charnel Pyre
                    cp.Action.Trigger = action;
                    cp.Action.Action = ActionType.Reaction;
                }
            }
        }

        public bool IsActionType(string tmp)
        {
            ActionType a;
            return EnumUtility.TryParse(Upper1(tmp), out a);
        }

        private void ManageBlockDescription(Creature c, HtmlNode currentPar)
        {
            c.Details = currentPar.InnerText.Replace("Description:", "").Trim();
        }

        /// <summary>
        /// Set the recharge text, and PowerUse to Encounter
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="cp">The cp.</param>
        private void CalculateRecharge(string html, CreaturePower cp,MonsterStatType version)
        {
            cp.Action.Use = PowerUseType.Encounter;
            //if (currentPar.InnerHtml.Contains("/images/symbol/1a.gif"))
            //{
            //    cp.Action.Recharge=
            //}
            if (html.Contains("/images/symbol/2a.gif"))
            {
                cp.Action.Recharge = "Recharges on 2-6";
                return;
            }
            if (html.Contains("/images/symbol/3a.gif"))
            {
                cp.Action.Recharge = "Recharges on 3-6";
                return;
            }
            if (html.Contains("/images/symbol/4a.gif"))
            {
                cp.Action.Recharge = "Recharges on 4-6";
                return;
            }
            if (html.Contains("/images/symbol/5a.gif"))
            {
                cp.Action.Recharge = "Recharges on 5-6";
                return;
            }
            if (html.Contains("/images/symbol/6a.gif"))
            {
                cp.Action.Recharge = "Recharges on 6";
                return;
            }
            if (version == MonsterStatType.MM3)
            {
                html = html.Replace("<b>", "");
                html = html.Replace("</b>", "");
                cp.Action.Recharge = CleanExtraComma(GetTextUntilTag(html, null));
                return;
            }
            string match = " at the end of the ";
            if (!html.Contains(match))
                match = " at the start ";
            if (!html.Contains(match))
                match = " when ";
            if (!html.Contains(match))
                match = " if ";
            if (!html.Contains(match))
                match = " after ";
            if (html.Contains(match))
            {
                int pos = html.IndexOf(match);
                html = html.Substring(pos);
                cp.Action.Recharge = "Recharge " + CleanExtraComma(GetTextUntilTag(html, null));
                if (cp.Action.Recharge.EndsWith(")"))
                    cp.Action.Recharge = cp.Action.Recharge.Substring(0, cp.Action.Recharge.Length - 1);
                return;
            }
            if (html.ToLower().Contains(" , or recharge ")) // Ancien abyssal Wurm: <b>Recharge 6 , or recharge 5 while bloodied</b>
            {
                html = html.Replace("<b>", "");
                cp.Action.Recharge = CleanExtraComma(GetTextUntilTag(html, null));
                return;
            }
            throw new Exception("Invalid recharge value " + html);
        }

        private void ManageBlockEquipment(Creature c, HtmlNode currentPar, MonsterStatType monsterStatType)
        {
            switch (monsterStatType)
            {
                case MonsterStatType.Old:
                    c.Equipment = CleanHtml(currentPar.InnerText.Replace("Equipment: ", "")).Trim();
                    break;
                case MonsterStatType.MM3:
                    //should not happen
                    throw new NotImplementedException();
            }
        }

        private void GetActionType(PowerAction powerAction, string temp)
        {
            string potentielTrigger = string.Empty;
            if (temp.ToLower().StartsWith("standard"))
            {
                powerAction.Action = ActionType.Standard;
                potentielTrigger = temp.Substring("standard".Length);
            }
            if (temp.ToLower().StartsWith("move"))
            {
                powerAction.Action = ActionType.Move;
                potentielTrigger = temp.Substring("move".Length);
            }
            if (temp.ToLower().StartsWith("minor"))
            {
                powerAction.Action = ActionType.Minor;
                potentielTrigger = temp.Substring("minor".Length);
            }
            if (temp.ToLower().StartsWith("immediate interrupt"))
            {
                powerAction.Action = ActionType.Interrupt;
                potentielTrigger = temp.Substring("immediate interrupt".Length);
            }
            if (temp.ToLower().StartsWith("immediate reaction"))
            {
                powerAction.Action = ActionType.Reaction;
                potentielTrigger = temp.Substring("immediate reaction".Length);
            }
            if (temp.ToLower().StartsWith("opportunity"))
            {
                powerAction.Action = ActionType.Opportunity;
                potentielTrigger = temp.Substring("opportunity".Length);
            }
            if (temp.ToLower().StartsWith("free"))
            {
                powerAction.Action = ActionType.Free;
                potentielTrigger = temp.Substring("free".Length);
            }
            if (temp.ToLower().StartsWith("none"))
            {
                powerAction.Action = ActionType.None;
                potentielTrigger = temp.Substring("none".Length);
            }
            if (temp.ToLower().StartsWith("no action"))
            {
                powerAction.Action = ActionType.None;
                potentielTrigger = temp.Substring("no action".Length);
            }
            if (temp.StartsWith("when "))
            {
                powerAction.Action = ActionType.Reaction;
                potentielTrigger = temp;
            }
            if (temp == "")
            {
                powerAction.Action = ActionType.None;
            }
            if (!String.IsNullOrEmpty(potentielTrigger))
            { //trigger can already be set
                powerAction.Trigger = potentielTrigger;
            }
            powerAction.Trigger = CleanExtraComma(powerAction.Trigger);
            if (powerAction.Trigger.StartsWith(","))
            {
                powerAction.Trigger = powerAction.Trigger.Substring(1).Trim();
            }
        }

        private void ManageBlockPublished(Creature c, HtmlNode currentPar, MonsterStatType monsterStatType)
        {
        }

        private void ManageBlockAlignement(Creature c, HtmlNode currentPar, MonsterStatType monsterStatType)
        {
            string skills = GetTextUntilTag(currentPar.InnerHtml, "<b>Skills</b>");
            if (!String.IsNullOrEmpty(skills))
            {
                if (!string.IsNullOrEmpty(c.Skills))
                { //perception already in skills
                    c.Skills += ", ";
                }
                c.Skills += skills.Trim();
            }
            ReadAbilities(c, currentPar.InnerHtml);
        }

        private Ability GetAbility(string tosearch, string html)
        {
            Ability a = new Ability();
            a.Score = GetIntValue(html.ToLower(), "<b>" + tosearch + "</b>");
            return a;
        }

      
    }
}
