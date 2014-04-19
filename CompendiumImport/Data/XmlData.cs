using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace CompendiumImport.Data
{
    internal class XmlData
    {
        private string url;

        private XmlDocument xmlDoc;

        public XmlDocument XmlDoc
        {
            get
            {
                if (xmlDoc == null)
                {
                    xmlDoc = new XmlDocument();
                    xmlDoc.Load(url);
                    WriteXmlIfDebug(url, XmlDoc);
                }
                return xmlDoc;
            }
        }

        public XmlData(string Url)
        {
            url = Url;
        }
        /// <summary>
        /// Writes the XML if in debugmode
        /// </summary>
        /// <param name="url">>The URL which got the file.</param>
        /// <param name="doc">The doc.</param>
        private void WriteXmlIfDebug(string url, XmlDocument doc)
        {
            if (url.ToLower().StartsWith("http://www.wizards.com") && Debugger.IsAttached)
            {
                SaveXmlAsFile(url, doc);
            }
        }
        /// <summary>
        /// Saves the XML as file.
        /// </summary>
        /// <param name="url">The URL which got the file.</param>
        /// <param name="doc">The doc.</param>
        private void SaveXmlAsFile(string url, XmlDocument doc)
        {
            string name = url.Substring(url.LastIndexOf("/") + 1);
            //if (name.Contains("?"))
            //{
            //    name=name.Substring(0,name.LastIndexOf('?'))
            //}
            name= Path.GetInvalidFileNameChars().Aggregate(name, (current, c) => current.Replace(c.ToString(), " "));
            name = Path.ChangeExtension(name, "xml");
            using (XmlWriter writer = XmlWriter.Create(name))
            {
                doc.WriteTo(writer);
            }
        }
    }
}
