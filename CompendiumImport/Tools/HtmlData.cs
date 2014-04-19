using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CompendiumImport.Tools;
using HtmlAgilityPack;
using Masterplan.Data;
using CompendiumImport.Data.Converters;

namespace CompendiumImport.Tools
{
    public class HtmlData
    {
        BrowserSession _browserSession;
        protected string _login;
        protected string _password;
        protected string _objectType;

        private string _LoginUrl = "http://www.wizards.com/dndinsider/compendium/login.aspx?page={0}&id={1}";
        private string _NonLoginUrl = "http://www.wizards.com/dndinsider/compendium/{0}.aspx?id={1}";
        
        protected string LoginUrl(int id)
        {
            return String.Format(_LoginUrl,_objectType,id);
        }

        protected string NonLoginUrl(int id)
        {
            return String.Format(_NonLoginUrl,_objectType, id);
        }

        public HtmlData(string login, string password,string objectType)
        {
            _login = login;
            _password = password;
            _objectType = objectType;
        }


        public HtmlDocument  GetHtmlDocument(int id)
        {
            string response;
            if (_browserSession == null)
            {
                _browserSession = new BrowserSession();
                response = _browserSession.Get(NonLoginUrl(id));
                if (response.Contains("Gain full access"))
                {
                    _browserSession.FormElements["email"] = _login;
                    _browserSession.FormElements["password"] = _password;
                    response = _browserSession.Post(LoginUrl(id));
                }
            }
            else
            {
                response = _browserSession.Get(NonLoginUrl(id));
            }
            if (response.Contains("Gain full access"))
            {
                throw new FailToLogInException(id.ToString(),_login);    
            }
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(response);
            
            WriteHtmlIfDebug(NonLoginUrl(id), response);
            return doc;
        }

       

        private void WriteHtmlIfDebug(string url, string response)
        {
            if (url.ToLower().StartsWith("http://www.wizards.com") && Debugger.IsAttached)
            {
                SaveHtmlAsFile(url, response);
            }
        }

        private void SaveHtmlAsFile(string url, string response)
        {
            string name = url.Substring(url.LastIndexOf("/") + 1);
            name = Path.GetInvalidFileNameChars().Aggregate(name, (current, c) => current.Replace(c.ToString(), "_"));
            name += ".html";
            using (TextWriter tw = new StreamWriter(name))
            {
                tw.Write(response);
            }
        }
    }
}