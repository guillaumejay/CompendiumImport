using System.Net;
using System.Text;
using HtmlAgilityPack;

namespace CompendiumImport.Tools
{
    /// <summary>
    /// Adapted from http://refactoringaspnet.blogspot.com/2010/04/using-htmlagilitypack-to-get-and-post.html
    /// </summary>
    internal class BrowserSession
    {
        private bool _isPost;
        private HtmlDocument _htmlDoc;

        public BrowserSession()
        {
            //UserAgent = "Opera/9.80 (Windows NT 6.1; U; en) Presto/2.6.30 Version/10.61";
            // UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:6.0.2) Gecko/20100101 Firefox/6.0.2";
            UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; OfficeLiveConnector.1.4; OfficeLivePatch.1.3; .NET4.0C; .NET4.0E)";
        }

        public string UserAgent { get; set; }

        /// <summary>
        /// System.Net.CookieCollection. Provides a collection container for instances of Cookie class
        /// </summary>
        public CookieCollection Cookies { get; set; }

        /// <summary>
        /// Provide a key-value-pair collection of form elements
        /// </summary>
        public FormElementCollection FormElements { get; set; }

        /// <summary>
        /// Makes a HTTP GET request to the given URL
        /// </summary>
        public string Get(string url)
        {
            _isPost = false;
            CreateWebRequestObject().Load(url);
            return _htmlDoc.DocumentNode.InnerHtml;
        }

        /// <summary>
        /// Makes a HTTP POST request to the given URL
        /// </summary>
        public string Post(string url)
        {
            _isPost = true;
            CreateWebRequestObject().Load(url, "POST");
            return _htmlDoc.DocumentNode.InnerHtml;
        }

        /// <summary>
        /// Creates the HtmlWeb object and initializes all event handlers.
        /// </summary>
        private HtmlWeb CreateWebRequestObject()
        {
            HtmlWeb web = new HtmlWeb();
            web.UseCookies = true;
            web.PreRequest = new HtmlWeb.PreRequestHandler(OnPreRequest);
            web.PostResponse = new HtmlWeb.PostResponseHandler(OnAfterResponse);
            web.PreHandleDocument = new HtmlWeb.PreHandleDocumentHandler(OnPreHandleDocument);
            return web;
        }

        /// <summary>
        /// Event handler for HtmlWeb.PreRequestHandler. Occurs before an HTTP request is executed.
        /// </summary>
        protected bool OnPreRequest(HttpWebRequest request)
        {
            AddCookiesTo(request);               // Add cookies that were saved from previous requests
            if (_isPost) AddPostDataTo(request); // We only need to add post data on a POST request
            return true;
        }

        /// <summary>
        /// Event handler for HtmlWeb.PostResponseHandler. Occurs after a HTTP response is received
        /// </summary>
        protected void OnAfterResponse(HttpWebRequest request, HttpWebResponse response)
        {
            //    SaveCookiesFrom(response); // Save cookies for subsequent requests
            SaveCookiesFrom(request);
        }

        /// <summary>
        /// Event handler for HtmlWeb.PreHandleDocumentHandler. Occurs before a HTML document is handled
        /// </summary>
        protected void OnPreHandleDocument(HtmlDocument document)
        {
            SaveHtmlDocument(document);
        }

        /// <summary>
        /// Assembles the Post data and attaches to the request object
        /// </summary>
        private void AddPostDataTo(HttpWebRequest request)
        {
            string payload = FormElements.AssemblePostPayload();
            byte[] buff = Encoding.UTF8.GetBytes(payload.ToCharArray());
            request.ContentLength = buff.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = UserAgent;
            System.IO.Stream reqStream = request.GetRequestStream();
            reqStream.Write(buff, 0, buff.Length);
        }

        /// <summary>
        /// Add cookies to the request object
        /// </summary>
        private void AddCookiesTo(HttpWebRequest request)
        {
            if (Cookies != null && Cookies.Count > 0)
            {
                request.CookieContainer.Add(Cookies);
            }
        }

        private void SaveCookiesFrom(HttpWebRequest request)
        {
            bool save = false;
            CookieCollection collection = request.CookieContainer.GetCookies(request.RequestUri);
            // look for ident cookie
            for (int i = 0; i < collection.Count; i++)
            {
                Cookie c = collection[i];
                if (c.Name == "iPlanetDirectoryPro")
                {
                    save = true;
                    break;
                }
            }
            if (save)
            {
                if (Cookies == null) Cookies = new CookieCollection();
                Cookies.Add(request.CookieContainer.GetCookies(request.RequestUri));
            }
        }

        /// <summary>
        /// Saves the form elements collection by parsing the HTML document
        /// </summary>
        private void SaveHtmlDocument(HtmlDocument document)
        {
            _htmlDoc = document;
            FormElements = new FormElementCollection(_htmlDoc);
        }
    }
}