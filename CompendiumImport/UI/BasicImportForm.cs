using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using CompendiumImport.Data;
using CompendiumImport.Data.Converters;
using CompendiumImport.Tools;
using Masterplan.Data;
using Masterplan.Extensibility;

namespace CompendiumImport.UI
{
    internal  partial class BasicImportForm : Form
    {
        protected ImportResult ImportResult { get; set; }       
       
        protected Library CurrentLib
        {
            get
            {
                if (cboLibraries.SelectedItem == null)
                    return null;
                return cboLibraries.SelectedItem as Library;
            }
        }
        protected IApplication _mpApp;
        protected virtual string ImportType { get { throw new NotImplementedException(); } }
        protected string Title
        {
            get
            {
                Version version = Assembly.GetExecutingAssembly().GetName().Version;

                return String.Format("CompendiumImport {3} {0}.{1}.{2}", version.Major, version.Minor, version.Build, CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ImportType));
            }
        }
        internal EventHandler OnCloseAddin { get; set; }

        internal void Open(IApplication _MPApp)
        {
            _mpApp = _MPApp;
            Show();
        }
        public BasicImportForm()
        {
            InitializeComponent();
        }

        private void BasicImportForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cboLibraries.SelectedItem != null)
                Common.AddinSettings.SelectedLibrary = (cboLibraries.SelectedItem as Library).ID;
            Common.AddinSettings.LoginName = txtLogin.Text;
            Common.AddinSettings.SelectedSource = cboSource.Text;
            Common.AddinSettings.Save(Common.SettingsPath);
            Common.DDIPassword = txtPassword.Text;
        }

        private void BasicImportForm_Load(object sender, EventArgs e)
        {
            cboLibraries.DataSource = _mpApp.Libraries;
            Library l = _mpApp.Libraries.FirstOrDefault(x => x.ID == Common.AddinSettings.SelectedLibrary);
            if (l != null)
                cboLibraries.SelectedItem = l;

            txtLogin.Text = Common.AddinSettings.LoginName;
            txtPassword.Text = Common.DDIPassword;
            this.Text = Title;
            InitListViewColumn();
 
        }

        private void BasicImportForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (OnCloseAddin != null)
            {
                OnCloseAddin(this, e);
            }

        }

        private void BasicImportForm_Activated(object sender, EventArgs e)
        {
            if (Common.NeedToImport)
            {
                lblLoading.BringToFront();
                lblLoading.Visible = true;
                Application.DoEvents();
            }
            if (cboSource.DataSource == null)
            {
                LoadSources();
                
            }
            lblLoading.Visible = false;
        }

        protected virtual void LoadSources()
        {
            throw new NotImplementedException();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            lvwResults.Items.Clear();
            Cursor = Cursors.WaitCursor;
            //int ID = Convert.ToInt32(cboSource.SelectedValue);
            CompendiumSource cs = cboSource.SelectedItem as CompendiumSource;
            IEnumerable<SearchResult> results = GetSearchResults(cs);
            lvwResults.BeginUpdate();
            foreach (SearchResult sr in results.OrderBy(x => x.Name))
            {
                AddItem(sr);
            }
            ManageDuplicates();
            lvwResults.EndUpdate();
            Cursor = Cursors.Default;
        }

        protected virtual void AddItem(SearchResult sr)
        {
            throw new NotImplementedException();
        }
        protected virtual IEnumerable<SearchResult> GetSearchResults(CompendiumSource cs)
        {
            throw new NotImplementedException();
        }

        private void btnSelectNone_Click(object sender, EventArgs e)
        {
            SetCheck(false);
        }

        private void SetCheck(bool state)
        {
            Cursor = Cursors.WaitCursor;
            if (state)
            {
                foreach (ListViewItem li in lvwResults.Items)
                {
                    li.Checked = state;
                }
            }
            else
            {
                foreach (ListViewItem li in lvwResults.CheckedItems)
                {
                    li.Checked = false;
                }
            }
            SetStateAddTo();
            Cursor = Cursors.Default;
        }

        private void SetStateAddTo()
        {
            int count = lvwResults.CheckedItems.Count;
            btnAddTo.Enabled = count > 0 && (cboLibraries.SelectedItem != null);
            lblInfo.Text = count + " " + ImportType + ((count > 1) ? "s" : "") + " checked";
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            SetCheck(true);
        }

        private void btnAddTo_Click(object sender, EventArgs e)
        {
            AddToLibrary(false,false);
        }

        private void lvwResults_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            SetStateAddTo();
        }

        private void cboSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetStateAddTo();
            ManageDuplicates();
        }

        private void ManageDuplicates()
        {
            if (cboLibraries.SelectedItem == null)
                return;
            lvwResults.BeginUpdate();
            Library library = cboLibraries.SelectedItem as Library;
            for (int i=lvwResults.Items.Count - 1;i >= 0;i--)
            {
                ListViewItem lvi = lvwResults.Items[i];
                bool duplicate = library.FindCreature(lvi.Text) != null;
                lvi.ForeColor = (duplicate) ? Color.Gray : lvwResults.ForeColor;
                lvi.Checked = !duplicate;
                (lvi.Tag as SearchResult).IsDuplicate = duplicate;
            }
            lvwResults.EndUpdate();
        }

        private void btnSingleSourceOnly_Click(object sender, EventArgs e)
        {
            lvwResults.BeginUpdate();
            foreach (ListViewItem lvi in lvwResults.Items)
            {
                if (lvi.SubItems[4].Text == SearchResult.MultipleSources)
                {
                    lvi.Checked = false;
                }
            }
            lvwResults.EndUpdate();
        }

        private void btnUncheckPresentInAnotherLibrary_Click(object sender, EventArgs e)
        {
            if (cboLibraries.SelectedItem == null)
                return;
            Library current = cboLibraries.SelectedItem as Library;
            lvwResults.BeginUpdate();
            foreach (ListViewItem lvi in lvwResults.Items)
            {
                if (lvi.Checked)
                {
                    lvi.Checked = !IsInAnotherLibrary(lvi.Tag as SearchResult,current);
                }
            }
            lvwResults.EndUpdate();
        }

        protected virtual bool IsInAnotherLibrary(SearchResult searchResult,Library current)
        {
            throw new NotImplementedException();
        }

        protected void AddToLibrary(bool log,bool DoNotSave)
        {
            ImportResult=new ImportResult(ImportType);
            if (( CurrentLib== null)
                
               || (lvwResults.CheckedIndices.Count == 0))
            {
                btnAddTo.Enabled = false;
                return;
            }
            Cursor = Cursors.WaitCursor;
            Refresh();
            HtmlData htmlData = HtmlData;
            lblProgress.Value = 0;
            lblProgress.Maximum = lvwResults.CheckedItems.Count;

            foreach (ListViewItem lvi in lvwResults.CheckedItems)
            {
                lblInfo.Text = "Working on " + lvi.Text;
                statusStrip1.Refresh();
                try
                {
                    if (SpecificAddTo(lvi, log, htmlData))
                    {
                        if (log)
                            Common.WriteToLog(String.Format("Adding {0} {1} to {2}", ImportType, lvi.Text, CurrentLib.Name));
                        ImportResult.SuccessOn.Add(lvi.Text);
                    }
                }
                catch (InvalidStructureException cex)
                {
                    if (log)
                        Common.WriteToLog(cex.Message);
                    Utils.LogSystem.Trace(cex);
                  //  MessageBox.Show(cex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ImportResult.BugOn.Add(cex.Message);
                  
                    continue;
                }
                catch (FailToLogInException)
                {
                    if (log)
                        Common.WriteToLog("Could not log to D&D insider !");
                    MessageBox.Show("Could not log to D&D insider !" + Environment.NewLine + "Check your login and password.", "Error", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    Cursor = Cursors.Default;
                    return;
                }
                catch (Exception ex)
                {
                    if (log) Common.WriteToLog(ex.Message);
                    //MessageBox.Show("Error on importing : " + lvi.Text + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
                    Utils.LogSystem.Trace(lvi.Text + " " + ex);
                    ImportResult.BugOn.Add(lvi.Text + " : " + ex.Message);
                    continue;
                }
                
                finally
                {
                    lblProgress.Value++;
                }
              
            }
            string temp = ImportResult.NumberOfSuccess + " " + ImportType + ((ImportResult.NumberOfSuccess > 1) ? "s" : "") + ((DoNotSave) ? " converted without saving" : " added in " + CurrentLib.Name);
            lblInfo.Text = temp;
            ResultForm rf=new ResultForm();
            rf.Open(CurrentLib,ImportType,ImportResult);
            if (log) Common.WriteToLog(temp);
            if (ImportResult.SuccessOn.Count > 0 && !DoNotSave)
            {
                int index = cboLibraries.SelectedIndex;
                _mpApp.SaveLibrary(CurrentLib);
                cboLibraries.SelectedIndex = index;
            }
            Cursor = Cursors.Default;
        }

        protected virtual HtmlData HtmlData
        {
            get { throw new NotImplementedException(); }
        }

        protected virtual bool SpecificAddTo(ListViewItem lvi, bool log, HtmlData htmlData)
        {
            throw new NotImplementedException();
        }

        private void btnAddTo_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DialogResult res =
                    MessageBox.Show("Do you want to Import and Log (Yes), or just test import without saving (No) ?",
                                    "Debug mode", MessageBoxButtons.YesNoCancel);
                AddToLibrary(res==DialogResult.Yes,res==DialogResult.No);
            }
        }

        protected virtual void InitListViewColumn()
        {
            lvwResults.Items.Clear();
            lvwResults.Columns.Clear();
            ColumnHeader c = new ColumnHeader {Width = 263, Text = "Name"};
            lvwResults.Columns.Add(c);
            c = new ColumnHeader {Width = 45, Text = "Level"};
            lvwResults.Columns.Add(c);
            c = new ColumnHeader { Width = 163, Text = "Source" };
            lvwResults.Columns.Add(c);
        }

        protected virtual bool FoundInLibrary(Library lib, SearchResult searchResult)
        {
            throw new NotImplementedException();
        }
    }
}
