using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Makku.MetroUI.Tables
{
    public class DataColumn<T> : Column
    {
        [JsonIgnore]
        public Func<T, string> Mapping { get; set; } = i => i?.ToString() ?? "";

        [JsonIgnore]
        public List<ColumnButton<T>> Buttons { get; set; } = new List<ColumnButton<T>>();

        public DataColumn(Column column)
        {
            Name = column.Name;
            Size = column.Size;
            Title = column.Title;
            Sortable = column.Sortable;
            SortDir = column.SortDir;
            Format = column.Format;
        }

        public DataColumn() : base() { }
        public DataColumn(string name) : base(name: name, name) { }
        public DataColumn(string name, int size) : base(name: name, title: name, size: $"{size}") { }
        public DataColumn(string name, string size) : base(name: name, title: name, size: size) { }
        public DataColumn(string name, string title, int size) : base(name: name, title: title, size: $"{size}") { }
        public DataColumn(string name, string title, string size, bool sortable = false) : base(name, title, size, sortable) { }
    }
}
