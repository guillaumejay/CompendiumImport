using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompendiumImport.Data
{
    internal class SearchResult 
    {
        public static readonly string  MultipleSources = "Multiple sources";
        public int ID { get; set; }

        public string Name { get; set; }

        public string SourceBook { get; set; }
        
        public bool IsDuplicate { get; set; }

        public bool IsChecked { get; set; }

        public string Level { get; set; }

        public string SourceBookReduced
        {
            get
            {
                if (SourceBook.Contains(","))
                {
              
                    return MultipleSources;
                }
                return SourceBook;
            }
        }
    }
}
