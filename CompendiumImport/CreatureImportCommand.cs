using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Masterplan.Extensibility;
using CompendiumImport.UI;

namespace CompendiumImport
{
    public class CreatureImportCommand : ICommand,IDisposable
    {
        private IApplication _MPApp;

        private CreatureImportUI ci;

        public CreatureImportCommand(IApplication mpApp)
        {
            _MPApp = mpApp;
        }

        public bool Active
        {
            get { return false; }
        }

        public bool Available
        {
            get
            {
                return true;
            }
        }

        public string Description
        {
            get { return "Display the Import creature Window"; }
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public void Execute()
        {
           // System.Windows.Forms.MessageBox.Show((ci == null).ToString());
            if (ci == null)
            {
                ci = new CreatureImportUI();
                ci.OnCloseAddin += OnCloseUI;
            }
            
            ci.Open(_MPApp);
            
        }

        private void OnCloseUI(Object sender,EventArgs fcea)
        {
            ci = null;
         //      System.Windows.Forms.MessageBox.Show("closed");
        }

        public string Name
        {
            get { return "Import Creature"; }
        }

        public void Dispose()
        {
    
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (ci != null)
                {
                    ci.Dispose();
                    ci = null;
                }
            }
        }
    }
}
