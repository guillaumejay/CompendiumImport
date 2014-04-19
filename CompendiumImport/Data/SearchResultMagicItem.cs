using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;

namespace CompendiumImport.Data
{
    internal class SearchResultMagicItem:SearchResult
    {
        protected static string SearchUrl = "http://www.wizards.com/dndinsider/compendium/CompendiumSearch.asmx/KeywordSearchWithFilters?keywords=null&tab=Item&filters=null|-1|-1|-1|-1|-1|-1|null|{0}|null&nameonly=false";

        public string Category { get; set; }

        public string Cost { get; set; }

        public string Rarity { get; set; }

        internal static IEnumerable<SearchResult> BySource(CompendiumSource source)
        {
            string url = String.Format(SearchUrl, source.Value);
            XmlData xmlData=new XmlData(url);
            return AnalyzeDoc(xmlData.XmlDoc);
        }

        private static IEnumerable<SearchResult> AnalyzeDoc(XmlDocument doc)
        {
            List<SearchResult> l = new List<SearchResult>();
            XmlNodeList nodes = doc.SelectNodes("//Data/Results/Item");
            foreach (XmlNode node in nodes)
            {
                SearchResultMagicItem cs = new SearchResultMagicItem
                {
                    Name = node["Name"].InnerText,
                    ID = Convert.ToInt32(node["ID"].InnerText),
                    Level = node["Level"].InnerText.Trim(),
                SourceBook=node["SourceBook"].InnerText
                };
                l.Add(cs);
            }
        
            return l;
        }



    }
}
