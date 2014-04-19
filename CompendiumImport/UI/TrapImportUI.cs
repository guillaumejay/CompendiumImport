using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CompendiumImport.Data;
using CompendiumImport.Data.Converters;
using CompendiumImport.Tools;
using Masterplan.Data;

namespace CompendiumImport.UI
{
    internal class TrapImportUI : BasicImportForm
    {
        protected override HtmlData HtmlData
        {
            get
            {
                return new HtmlData(txtLogin.Text, txtPassword.Text, "trap");
            }
        }
        private IConverter<Trap> _converter;
        protected IConverter<Trap> Converter
        {
            get { return _converter ?? (_converter = new TrapConverter()); }
        }
        protected override string ImportType
        {
            get { return "trap"; }
        }

        protected override IEnumerable<SearchResult> GetSearchResults(CompendiumSource cs)
        {
            return SearchResultTrap.BySource(cs);
        }

        protected override void AddItem(SearchResult sr)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Text = sr.Name;
            lvi.Tag = sr;

            lvi.SubItems.Add((sr as SearchResultTrap).Type);
            lvi.SubItems.Add((sr as SearchResultTrap).Role);
            lvi.SubItems.Add((sr as SearchResultTrap).Level.ToString());
            lvi.SubItems.Add(sr.SourceBookReduced);
            lvi.Checked = true;
            lvi.ToolTipText = sr.SourceBook;
            lvwResults.Items.Add(lvi);
        }

        protected override bool IsInAnotherLibrary(SearchResult searchResult, Library current)
        {
            bool duplicate = false;
            SearchResultTrap srm = searchResult as SearchResultTrap;
            foreach (Library lib in _mpApp.Libraries)
            {
                if (lib.ID == current.ID)
                    continue;
                Trap c = lib.FindTrap(srm.Name, Convert.ToInt32(srm.Level), srm.Role);
                if (c != null)
                {
                    duplicate = true;
                }
            }
            return duplicate;
        }

        protected override void InitListViewColumn()
        {
            base.InitListViewColumn();
            ColumnHeader c = new ColumnHeader { Width = 90, Text = "Type" };
            lvwResults.Columns.Insert(1, c);
            c = new ColumnHeader { Width = 90, Text = "Role" };
            lvwResults.Columns.Insert(2, c);
        }

        protected override void LoadSources()
        {
            cboSource.DataSource = Common.TrapSources.ToList();
            if (Common.TrapSources.Any(x => x.Name == Common.AddinSettings.SelectedSource))
            {
                for (int i = 0; i < cboSource.Items.Count; i++)
                {
                    if (((cboSource.Items[i]) as CompendiumSource).Name == Common.AddinSettings.SelectedSource)
                    {
                        cboSource.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        protected override bool FoundInLibrary(Library lib, SearchResult searchResult)
        {
            bool duplicate = false;
            SearchResultTrap srm = searchResult as SearchResultTrap;
            Trap c = lib.FindTrapCaseInsensitive(srm.Name, Convert.ToInt32(srm.Level));
            if (c != null)
            {
                duplicate = false;
            }
            return duplicate;
        }

        protected override bool SpecificAddTo(ListViewItem lvi, bool log, HtmlData htmlData)
        {
            if (lvi.SubItems[2].Text.ToLower() == "no role" && lvi.SubItems[3].Text.ToLower() != "minion")
            {
                //MessageBox.Show(String.Format("Trap {0} has No role, and is not a Minion !", lvi.Text), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblProgress.Value++;
                ImportResult.WarningOn.Add(String.Format("{0} : no Role and is not a minion => Not Imported",
                                                         lvi.Text));
                return false;
            }
            int id = Convert.ToInt32((lvi.Tag as SearchResult).ID);
            HtmlAgilityPack.HtmlDocument doc = htmlData.GetHtmlDocument(id);
            Trap trap = Converter.GetMasterPlanObjectFromDoc(doc, ImportResult.WarningOn);
            CurrentLib.Traps.Add(trap);
            return true;
        }
    }
}
