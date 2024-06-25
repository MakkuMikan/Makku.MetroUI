using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Makku.MetroUI.Tables
{
    public class DataColumn<T> : Column
    {
        [JsonIgnore]
        public LambdaExpression Mapping { get; set; } = default;

        [JsonIgnore]
        public LambdaExpression PostProcess { get; set; } = (Expression<Func<object, string>>)(i => i == null ? "" : i.ToString());

        [JsonIgnore]
        public Expression<Func<T, string>> DirectMapping { get; set; } = default;

        [JsonIgnore]
        public Type PropertyType { get; set; }

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
