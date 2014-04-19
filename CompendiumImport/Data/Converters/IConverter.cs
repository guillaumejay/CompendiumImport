using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace CompendiumImport.Data.Converters
{
    /// <summary>
    /// Basic interface for converter
    /// </summary>
    /// <typeparam name="TMasterplanData">The type of the masterplan data.</typeparam>
    interface IConverter<out TMasterplanData>
    {
        TMasterplanData GetMasterPlanObjectFromDoc(HtmlDocument doc,List<string> warnings);
    }
}
