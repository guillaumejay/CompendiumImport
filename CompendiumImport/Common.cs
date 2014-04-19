using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using CompendiumImport.Data;
using CompendiumImport.Tools;

namespace CompendiumImport
{
    internal static class Common
    {
        internal static Settings AddinSettings;

        internal static string DDIPassword { get; set; }

        private static DateTime? LastImportForSourcesSelection { get; set; }

        private static IEnumerable<CompendiumSource> allSources;
        internal static IEnumerable<CompendiumSource> AllSources
        {
            get
            {
                if (NeedToImport)
                {
                    allSources = CompendiumSource.FromUrl();
                    LastImportForSourcesSelection = DateTime.Now;
                }
                return allSources;
            }
        }

        internal static bool NeedToImport
        {
            get { return !LastImportForSourcesSelection.HasValue || (DateTime.Now.Subtract(LastImportForSourcesSelection.Value).Hours > 4); }
        }


        //http://stackoverflow.com/questions/1373100/how-to-add-folder-to-assembly-search-path-at-runtime-in-net
        internal static Assembly LoadFromSameFolder(object sender, ResolveEventArgs args)
        {
            string folderPath = CurrentDLLPath;
            string assemblyPath = Path.Combine(folderPath, new AssemblyName(args.Name).Name + ".dll");
            if (File.Exists(assemblyPath) == false) return null;
            Assembly assembly = Assembly.LoadFrom(assemblyPath);
            return assembly;
        }

        internal static string CurrentDLLPath
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
        }

        internal static string SettingsPath
        {
            get
            {
                return Path.Combine(Common.CurrentDLLPath, "CompendiumImport.settings");
            }
        }
        
        internal static void WriteToLog(string text)
        {
            using (TextWriter tw = new StreamWriter(LogPath, true))
            {
                tw.WriteLine("{0}:{1}", DateTime.Now.ToString("yyMMdd-hhmmss"), text);
            }
        }
        internal static string LogPath
        {
            get
            {
                return Path.Combine(Common.CurrentDLLPath, "CompendiumImport.log");
            }
        }

        internal static string CacheFolder
        {
            get
            {
                string path = Path.Combine(Common.CurrentDLLPath, "CompendiumImportCache");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }
        }

        internal static IEnumerable<CompendiumSource> MonsterSources
        {
            get
            {
                return
                    AllSources.Where(
                        x =>
                        x.SourceType == "MonsterSourceBook" || String.IsNullOrEmpty(x.SourceType) ||
                        x.SourceType == "MonsterSourceOther");
            }
        }

        internal static IEnumerable<CompendiumSource> MagicItemSources
        {
            get
            {
                return
                    AllSources.Where(
                        x =>
                        x.SourceType == "ItemSourceBook" || String.IsNullOrEmpty(x.SourceType) ||
                        x.SourceType == "ItemSourceOther");
            }
        }

        internal static IEnumerable<CompendiumSource> TrapSources
        {
            get
            {
                return
                    AllSources.Where(
                        x =>
                        x.SourceType == "TrapSourceBook" || String.IsNullOrEmpty(x.SourceType) ||
                        x.SourceType == "TrapSourceOther");
            }
        }
    }
}
