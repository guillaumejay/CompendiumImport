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
    internal class MagicItemConverter:Converter<MagicItem>
    {
        public static Regex _LevelCondition = new Regex(@"Level (\d{1,2})\+? (Uncommon|Common|Rare|None)");
        public override MagicItem GetMasterPlanObjectFromDoc(HtmlAgilityPack.HtmlDocument doc, List<string> warnings)
        {
            MagicItem mi = new MagicItem();
             HtmlNode detailNode = doc.DocumentNode.SelectSingleNode("//div[attribute::id='detail']");
             HtmlNode titleNode = doc.DocumentNode.SelectSingleNode("//h1[attribute::class='mihead']");
            mi.Name = GetTextUntilTag(titleNode.InnerHtml, "");
            Match match = _LevelCondition.Match(titleNode.InnerHtml);
            mi.Level = Convert.ToInt32(match.Groups[1].Value);
            // Rarity= match.Groups[2].Value

            return mi;
        }
    }
}
