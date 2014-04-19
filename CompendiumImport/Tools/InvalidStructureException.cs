using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompendiumImport.Tools
{
  public class InvalidStructureException:Exception
    {
        private readonly string _detail;
        private readonly string _element;

        public InvalidStructureException(string detail, string creature)
        {
            this._detail = detail;
            this._element = creature;
        }

        public override string Message
        {
            get { return String.Format("Invalid structure for {0} : {1}", _element, _detail); }
        }
    }
}
