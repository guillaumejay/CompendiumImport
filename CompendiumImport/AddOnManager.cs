using System;
using System.Collections.Generic;

using Masterplan.Extensibility;
using CompendiumImport.Tools;
using System.IO;

namespace CompendiumImport
{
    public class AddOnManager : IAddIn
    {
        private List<ICommand> _commands;

        private IApplication _MPApp = null;

        /// <summary>
        /// Gets the list of combat commands supplied by the add-in.
        /// </summary>
        public List<ICommand> CombatCommands
        {
            get { return new List<ICommand>(); }
        }

        /// <summary>
        /// Gets the list of commands supplied by the add-in.
        /// </summary>
        public List<ICommand> Commands
        {
            get { return _commands; }
        }

        public string Description
        {
            get { return "Add-in used to import data from compendium"; }
        }

        public bool Initialise(IApplication app)
        {
            //Set bool to return whether this Add-In has initialized correctly
            bool initializeSuccessful = true;

            try
            {
                this._MPApp = app;
                _commands = new List<ICommand>();
                _commands.Add(new CreatureImportCommand(app));
                _commands.Add(new TrapImportCommand(app));
                if (File.Exists(Common.SettingsPath))
                {
                    Common.AddinSettings = Settings.Load(Common.SettingsPath);
                }
                if (Common.AddinSettings == null)
                {
                    Common.AddinSettings = new Settings();
                }
                AppDomain currentDomain = AppDomain.CurrentDomain;
                currentDomain.AssemblyResolve += new ResolveEventHandler(Common.LoadFromSameFolder);
            }
            catch (Exception ex)
            {
                initializeSuccessful = false;
                Utils.LogSystem.Trace(ex);
            }

            return initializeSuccessful;
        }

        public string Name
        {
            get { return "Compendium Import"; }
        }

        /// <summary>
        /// Gets the list of tab pages supplied by the add-in.
        /// </summary>
        public List<IPage> Pages
        {
            get { return new List<IPage>(); }
        }

        /// <summary>
        /// Gets the list of quick reference tab pages supplied by the add-in.
        /// </summary>
        public List<IPage> QuickReferencePages
        {
            get { return new List<IPage>(); }
        }

        public Version Version
        {
            get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version; }
        }
    }
}
