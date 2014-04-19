using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompendiumImport.Data
{
    internal class ImportResult
    {
        public ImportResult(string importType)
        {
            ImportType = importType;
            BugOn=new List<string>();
            WarningOn=new List<string>();
            SuccessOn=new List<string>();
        }

        public int NumberOfSuccess
        {
            get { return SuccessOn.Count; }
        }
        public string ImportType { get; set; }

        public List<string> BugOn { get; set; }

        public List<string> WarningOn { get; set; }

        public List<string> SuccessOn { get; set; }
    }
}
