using System;
using System.IO;
using System.Xml.Serialization;

namespace CompendiumImport.Tools
{
    [Serializable]
    public class Settings
    {
        public Settings()
        {
            IsDirty = false;
        }

        private bool IsDirty { get; set; }

        private string _loginName;

        public string LoginName
        {
            get { return _loginName; }
            set
            {
                if (_loginName == value)
                    return;
                _loginName = value;
                IsDirty = true;
            }
        }

        private string _selectedSource;

        public string SelectedSource
        {
            get { return _selectedSource; }
            set
            {
                if (_selectedSource == value)
                    return;
                _selectedSource = value;
                IsDirty = true;
            }
        }

        private Guid _selectedLibrary;

        public Guid SelectedLibrary
        {
            get { return _selectedLibrary; }
            set
            {
                if (_selectedLibrary == value)
                    return;
                _selectedLibrary = value;
                IsDirty = true;
            }
        }

        internal void Save(string settingsPath)
        {
            if (!IsDirty)
                return;
            XmlSerializer x = new XmlSerializer(this.GetType());
            using (StreamWriter sw = new StreamWriter(settingsPath))
            {
                x.Serialize(sw, this);
            }
            IsDirty = false;
        }

        internal static Settings Load(string settingsPath)
        {
            Settings set = new Settings();
            XmlSerializer x = new XmlSerializer(set.GetType());

            using (StreamReader sr = new StreamReader(settingsPath))
            {
                set = x.Deserialize(sr) as Settings;
            }
            return set;
        }
    }
}