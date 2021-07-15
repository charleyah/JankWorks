using System;
using System.Linq;
using System.Text;
using System.IO;

namespace JankWorks.Game.Configuration
{
    public sealed class IniSettingsSource : SettingsSource
    {
        private FileInfo file;
        private Encoding encoding;

        public IniSettingsSource(string path, Encoding encoding) : this(new FileInfo(path), encoding) { }

        public IniSettingsSource(FileInfo file, Encoding encoding)
        {
            this.file = file;
            this.encoding = encoding;
        }

        public override void Flush(Settings settings)
        {
            using var file = new FileStream(this.file.FullName, FileMode.Create, FileAccess.Write);
            using var writer = new StreamWriter(file, this.encoding);

            var ungrouped = settings.GetEntries();

            foreach (var kvp in ungrouped)
            {
                writer.Write(kvp.Key);
                writer.Write("=");
                writer.WriteLine(kvp.Value);
            }

            foreach(var section in from sections in settings.GetSections() where !string.IsNullOrWhiteSpace(sections) select sections)
            {
                writer.WriteLine();
                var entries = settings.GetEntries(section);

                writer.Write("[");
                writer.Write(section);
                writer.Write("]");
                writer.WriteLine();

                foreach(var entry in entries)
                {
                    writer.Write(entry.Key);
                    writer.Write("=");
                    writer.WriteLine(entry.Value);
                }
            }
        }

        public override void Refresh(Settings settings)
        {
            if(this.file.Exists)
            {
                settings.Clear();

                using var file = new FileStream(this.file.FullName, FileMode.Open, FileAccess.Read);
                using var reader = new StreamReader(file, this.encoding);
                try
                {
                    var currentSection = string.Empty;
                    var currentLine = reader.ReadLine();

                    while (currentLine != null)
                    {
                        currentLine = currentLine.TrimStart();

                        if (string.IsNullOrWhiteSpace(currentLine))
                        {
                            currentLine = reader.ReadLine();
                            continue;
                        }
                        else if (currentLine.StartsWith('#') || currentLine.StartsWith(';'))
                        {
                            currentLine = reader.ReadLine();
                            continue;
                        }
                        else if (currentLine.StartsWith('['))
                        {
                            currentSection = currentLine.Substring(1, currentLine.LastIndexOf(']') - 1);
                            currentLine = reader.ReadLine();
                            continue;
                        }

                        var linesplit = currentLine.Split('=');

                        var entry = linesplit[0];
                        var value = linesplit[1];

                        settings.SetEntry(entry, value, currentSection);

                        currentLine = reader.ReadLine();
                    }
                }
                catch (Exception e)
                {
                    throw new FormatException("INI format error", e);
                }
            }
        }
    }
}
