using System;
using System.Collections.Generic;
using System.Xml;
using System.Diagnostics;

namespace CompendiumImport.Data
{
    public class CompendiumSource
    {
        public string Name { get; set; }

        public int Value { get; set; }

        public string SourceType { get; set; }

        internal static IEnumerable<CompendiumSource> FromUrl(string url = "http://www.wizards.com/dndinsider/compendium/CompendiumSearch.asmx/GetFilterSelect")
        {
            XmlData xmlData = new XmlData(url);
            return AnalyzeDoc(xmlData.XmlDoc);
        }

        private static IEnumerable<CompendiumSource> AnalyzeDoc(XmlDocument doc)
        {
            List<CompendiumSource> l = new List<CompendiumSource>();
            XmlNodeList nodes = doc.SelectNodes("//Option");// SelectNodes("//Option[type='MonsterSourceBook']");
            foreach (XmlNode node in nodes)
            {
                CompendiumSource cs = new CompendiumSource
                                          {
                                              Name = node["val"].InnerText,
                                              Value = Convert.ToInt32(node["id"].InnerText),
                                              SourceType = node["type"].InnerText
                                          };
                l.Add(cs);
            }
            AddDefaultSourceOption(l);
            return l;
        }

        private static void AddDefaultSourceOption(List<CompendiumSource> l)
        {
            l.Add(new CompendiumSource { Value = -2, Name = "Rule Books" });
            l.Add(new CompendiumSource { Value = -3, Name = "Printed Adventures" });
            l.Add(new CompendiumSource { Value = -4, Name = "Dragon Magazines" });
            l.Add(new CompendiumSource { Value = -5, Name = "Dungeon Magazines" });
            l.Add(new CompendiumSource { Value = -6, Name = "RPGA Adventures" });
            l.Add(new CompendiumSource { Value = -7, Name = "Miniatures" });
        }
    }


}
