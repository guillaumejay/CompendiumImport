using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CompendiumImport.Data;
using Masterplan.Data;

namespace CompendiumImport.UI
{
    internal partial class ResultForm : Form
    {
        private string _libName;
        public ResultForm()
        {
            InitializeComponent();
        }

        public void Open(Library lib,string ImportType,ImportResult result)
        {
            Text = "Import " + ImportType + "s to " + lib.Name;
            _libName = lib.Name;
            foreach (string s in result.SuccessOn)
            {
                lboSuccess.Items.Add(s);
            }
            foreach (string s in result.WarningOn)
            {
                lboWarnings.Items.Add(s);
            }
            foreach (string s in result.BugOn)
            {
                lboBugs.Items.Add(s);
            }
            this.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            string export = Path.Combine(Path.GetTempPath(), "export" + Guid.NewGuid() + "export.txt");
            using (StreamWriter sw = new StreamWriter(export))
            {
                sw.WriteLine(String.Format("Export to {0} on {1} ",_libName,DateTime.Now.ToString("yyyy-MM-dd HH:mm")));
                sw.WriteLine("Bugs :");
                foreach (string s in lboBugs.Items)
                {
                    sw.WriteLine("      " + s);
                }
                sw.WriteLine("Warnings :");
                foreach (string s in lboWarnings.Items)
                {
                    sw.WriteLine("      " + s);
                }
                sw.WriteLine("Success :");
                foreach (string s in lboSuccess.Items)
                {
                    sw.WriteLine("      " + s);
                }
            }
            System.Diagnostics.Process.Start(export);
        }
    }
}
