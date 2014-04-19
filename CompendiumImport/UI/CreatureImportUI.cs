using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompendiumImport.Data.Converters;
using CompendiumImport.Tools;
using System.Windows.Forms;
using CompendiumImport.Data;
using Masterplan.Data;

namespace CompendiumImport.UI
{
    internal class CreatureImportUI : BasicImportForm
    {

        protected override HtmlData HtmlData
        {
            get
            {
                return new HtmlData(txtLogin.Text, txtPassword.Text, "monster");
            }
        }
        private IConverter<Creature> _converter;
        protected IConverter<Creature> Converter
        {
            get { return _converter ?? (_converter = new CreatureConverter()); }
        }
        protected override string ImportType
        {
            get { return "creature"; }
        }

        protected override IEnumerable<SearchResult> GetSearchResults(CompendiumSource cs)
        {
            return SearchResultMonster.BySource(cs);
        }

        protected override void AddItem(SearchResult sr)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Text = sr.Name;
            lvi.Tag = sr;
            lvi.SubItems.Add((sr as SearchResultMonster).Level.ToString());
            lvi.SubItems.Add((sr as SearchResultMonster).CombatRole);
            lvi.SubItems.Add((sr as SearchResultMonster).GroupRole);
            lvi.SubItems.Add(sr.SourceBookReduced);
            lvi.Checked = true;
            lvi.ToolTipText = sr.SourceBook;
            lvwResults.Items.Add(lvi);
        }

        protected override bool IsInAnotherLibrary(SearchResult searchResult, Library current)
        {
            bool duplicate = false;
            SearchResultMonster srm = searchResult as SearchResultMonster;
            foreach (Library lib in _mpApp.Libraries)
            {
                if (lib.ID == current.ID)
                    continue;
                duplicate = FoundInLibrary(lib, searchResult);
                if (duplicate)
                    break;
            }
            return duplicate;
        }

        protected override bool FoundInLibrary(Library lib, SearchResult searchResult)
        {
            bool duplicate = false;
            SearchResultMonster srm = searchResult as SearchResultMonster;
            Creature c = lib.FindCreatureCaseInsensitive(srm.Name, Convert.ToInt32(srm.Level));
            if (c != null)
            {
                if (c.HP == 1)
                {
                    Minion m = c.Role as Minion;
                    if (m.HasRole)
                    {
                        duplicate = m.Type.ToString() == srm.CombatRole;
                    }
                }
                else
                {
                    ComplexRole cr = c.Role as ComplexRole;
                    duplicate = srm.CombatRole.StartsWith(cr.Type.ToString()) &&
                                (cr.Leader == srm.CombatRole.Contains("Leader")) &&
                                (cr.Flag.ToString() == srm.GroupRole);
                }
            }
            return duplicate;
        }

        protected override void InitListViewColumn()
        {
            base.InitListViewColumn();
            ColumnHeader c = new ColumnHeader { Width = 132, Text = "Main Role" };
            lvwResults.Columns.Insert(2, c);
            c = new ColumnHeader { Width = 86, Text = "Group Role" };
            lvwResults.Columns.Insert(3, c);
        }

        protected override void LoadSources()
        {
            cboSource.DataSource = Common.MonsterSources.ToList();
            if (Common.MonsterSources.Any(x => x.Name == Common.AddinSettings.SelectedSource))
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

        protected override bool SpecificAddTo(ListViewItem lvi, bool log, HtmlData htmlData)
        {
            if (lvi.SubItems[2].Text.ToLower() == "no role" && lvi.SubItems[3].Text.ToLower() != "minion")
            {
                //  MessageBox.Show(String.Format("Creature {0} has No role, and is not a Minion !", lvi.Text), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ImportResult.WarningOn.Add(String.Format("{0} : no Role and is not a minion => Not Imported",
                                                         lvi.Text));
                return false;
            }
            int id = Convert.ToInt32((lvi.Tag as SearchResult).ID);
            HtmlAgilityPack.HtmlDocument doc = htmlData.GetHtmlDocument(id);
            Creature c = Converter.GetMasterPlanObjectFromDoc(doc, ImportResult.WarningOn);
            CurrentLib.Creatures.Add(c);
            return true;
        }
    }
}
