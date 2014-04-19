using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;

namespace CompendiumImport.Data
{
    internal class SearchResultMonster:SearchResult
    {
        protected static string SearchUrl = "http://www.wizards.com/dndinsider/compendium/CompendiumSearch.asmx/KeywordSearchWithFilters?keywords=null&tab=Monster&filters=-1|-1|null|null|null|-1|-1|{0}&nameOnly=false";

        public string GroupRole { get; set; }

        public string CombatRole { get; set; }

        internal static IEnumerable<SearchResult> BySource(CompendiumSource source)
        {
            string url = String.Format(SearchUrl, source.Value);
            XmlData xmlData=new XmlData(url);
            return AnalyzeDoc(xmlData.XmlDoc);
        }

        private static IEnumerable<SearchResult> AnalyzeDoc(XmlDocument doc)
        {
            List<SearchResult> l = new List<SearchResult>();
            XmlNodeList nodes = doc.SelectNodes("//Data/Results/Monster");
            foreach (XmlNode node in nodes)
            {
                SearchResultMonster cs = new SearchResultMonster
                {
                    Name = node["Name"].InnerText,
                    ID = Convert.ToInt32(node["ID"].InnerText),
                    Level =node["Level"].InnerText.Trim(),
                GroupRole=node["GroupRole"].InnerText, 
                CombatRole=node["CombatRole"].InnerText,
                SourceBook=node["SourceBook"].InnerText
                };
                l.Add(cs);
            }
        
            return l;
        }



    }
}
