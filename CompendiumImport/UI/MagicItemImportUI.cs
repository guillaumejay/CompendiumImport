using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CompendiumImport.Data;
using Masterplan.Data;

namespace CompendiumImport.UI
{
    internal class MagicItemImportUI : BasicImportForm
    {
        protected override string ImportType
        {
            get { return "item"; }
        }

        protected override IEnumerable<SearchResult> GetSearchResults(CompendiumSource cs)
        {
            return SearchResultMagicItem.BySource(cs);
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

        protected override bool IsInAnotherLibrary(SearchResult searchResult,Library current)
        {
            bool duplicate = false;
            SearchResultMagicItem srm = searchResult as SearchResultMagicItem;
            foreach (Library lib in _mpApp.Libraries)
            {
                if (lib.ID==current.ID)
                    continue;
                MagicItem c = lib.FindMagicItem(srm.Name);
                if (c != null)
                {
                    duplicate = true;
                }
            }
            return duplicate;
        }

        protected override void LoadSources()
        {
            cboSource.DataSource = Common.MagicItemSources.ToList();
            if (Common.MagicItemSources.Any(x => x.Name == Common.AddinSettings.SelectedSource))
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
    }
}
