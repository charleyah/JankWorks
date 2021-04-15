using System;
using System.IO;
using System.Reflection;

using JankWorks.Core;

namespace JankWorks.Util
{
    public sealed class CSVReader : Disposable
    {
        private TextReader reader;

        private string[] currentrow;
        private string[] headers;

        public CSVReader(TextReader reader, bool headers = true)
        {
            this.reader = reader;
            if (headers && this.ReadRow())
            {
                this.headers = new string[this.currentrow.Length];
                for (int i = 0; i < currentrow.Length; i++)
                {
                    this.headers[i] = this.currentrow[i];
                }
            }
        }

        public bool ReadRow()
        {
            var line = this.reader.ReadLine();
            if (line == null) { return false; }

            this.currentrow = line.Split(',');
            return true;
        }

        public string ReadCell(int column)
        {
            if (column < 0)
            {
                throw new ArgumentException();
            }
            return this.currentrow?[column] ?? null;
        }

        public string ReadCell(string columnname)
        {
            try
            {
                var coli = Array.IndexOf(this.headers, columnname);
                return ReadCell(coli);
            }
            catch (ArgumentNullException)
            {
                return null;
            }
        }

        public T ReadObject<T>() where T : new() => this.ReadObject(new T());

        public T ReadObject<T>(T obj)
        {
            var typedata = typeof(T);

            foreach (var field in typedata.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                var fielddata = field.GetCustomAttribute<CSVReader.Column>();
                if (fielddata != null)
                {
                    var fieldname = fielddata.Name;
                    for (int i = 0; i < this.headers.Length; i++)
                    {
                        if (fieldname.Equals(this.headers[i], StringComparison.Ordinal))
                        {
                            if (field.FieldType == typeof(string))
                            {
                                field.SetValue(obj, this.ReadCell(i));
                            }
                            else
                            {
                                var value = Convert.ChangeType(this.ReadCell(i), field.FieldType);
                                field.SetValue(obj, value);
                            }

                        }
                    }
                }
            }

            foreach (var property in typedata.GetProperties(BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.Instance))
            {
                var propdata = property.GetCustomAttribute<CSVReader.Column>();
                if (propdata != null)
                {
                    var propname = propdata.Name;
                    for (int i = 0; i < this.headers.Length; i++)
                    {
                        if (propname.Equals(this.headers[i], StringComparison.Ordinal))
                        {
                            if (property.PropertyType == typeof(string))
                            {
                                property.SetValue(obj, this.ReadCell(i));
                            }
                            else
                            {
                                if (property.PropertyType.IsEnum)
                                {
                                    var value = Enum.Parse(property.PropertyType, this.ReadCell(i));
                                    property.SetValue(obj, value);
                                }
                                else
                                {
                                    var value = Convert.ChangeType(this.ReadCell(i), property.PropertyType);
                                    property.SetValue(obj, value);
                                }
                            }
                        }
                    }
                }
            }
            return obj;
        }

        [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
        public sealed class Column : Attribute
        {
            public string Name { get; set; }

            public Column(string Name) { this.Name = Name; }
        }

        protected override void Dispose(bool finalising)
        {
            this.reader.Dispose();
            base.Dispose(finalising);
        }
    }
}
