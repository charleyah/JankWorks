using System;
using System.Collections.Generic;

namespace JankWorks.Game.Configuration
{
    public sealed class Settings
    {
        private Dictionary<string, Dictionary<string, string>> entries;
        private SettingsSource source;

        public Settings(SettingsSource source)
        {
            this.source = source;
            this.entries = new Dictionary<string, Dictionary<string, string>>();
            this.entries.Add(string.Empty, new Dictionary<string, string>());
        }

        public Settings(Settings copy, SettingsSource source)
        {
            this.entries = new Dictionary<string, Dictionary<string, string>>(copy.entries.Count);

            foreach(var section in copy.entries)
            {
                this.entries.Add(section.Key, new Dictionary<string, string>(section.Value));
            }

            this.source = source;
        }

        public T GetEntry<T>(string key, Func<string, T> cast, string section = null, T defaultValue = default)
        {
            var strvalue = this.ReadEntry(key, section);

            if (strvalue == null) { return defaultValue; }
            else
            {
                return cast(strvalue);
            }
        }

        public T ReadEntry<T>(string key, Func<string, T> cast, string section = null)
        {
            var entry = this.ReadEntry(key, section);

            if(entry != null)
            {
                return cast(entry);
            }
            else
            {
                return default(T);
            }
        }

        public bool TryGetEntry<T>(string key, Func<string, T> cast, out T value, string section = null)
        {
            var entry = this.ReadEntry(key, section);
            var found = entry != null;

            value = found ? cast(entry) : default(T);

            return found;        
        }

        public void Clear()
        {
            foreach(var section in this.entries)
            {
                section.Value.Clear();
            }
            this.entries.Clear();
        }

        public bool ContainsSection(string section) => this.entries.ContainsKey(section);
        public IEnumerable<string> GetSections() => this.entries.Keys;
        public IEnumerable<KeyValuePair<string, string>> GetEntries(string section = null)
        {
            section ??= string.Empty;

            return this.entries[section];
        }

        public string ReadEntry(string key, string section = null)
        {
            section ??= string.Empty;

            if (this.entries.TryGetValue(section, out var values) && values.TryGetValue(key, out var value))
            {
                return value;
            }
            else
            {
                return null;
            }
        }

        public bool TryGetEntry(string key, out string value, string section = null)
        {
            section ??= string.Empty;
            value = null;
            var found = this.entries.TryGetValue(section, out var values) && values.TryGetValue(key, out value);
            return found;
        }

        public string GetEntry(string key, string section = null, string defaultValue = null) => this.ReadEntry(key, section) ?? defaultValue;           
        
        public void SetEntry(string key, string value, string section = null)
        {
            section ??= string.Empty;

            var groupExists = this.entries.TryGetValue(section, out var entrygroup);

            entrygroup ??= new Dictionary<string, string>();

            if (!entrygroup.TryAdd(key, value))
            {
                entrygroup[key] = value;
            }

            if (!groupExists)
            {
                this.entries.Add(section, entrygroup);
            }
        }

        public void Save() => this.source.Flush(this);

        public void Load() => this.source.Refresh(this);
    }
}