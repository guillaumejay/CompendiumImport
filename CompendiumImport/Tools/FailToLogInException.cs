using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CompendiumImport.Tools
{
    public class FailToLogInException:Exception
    {
        private readonly string _item;
        private readonly string _login;

        public FailToLogInException(string item, string login)
        {
            this._item = item;
            this._login = login;
        }

        public override string Message
        {
            get { return String.Format("Was not able to log on D&D insider with login {0}, loading {1}",_login,_item); }
        }
    }
}
