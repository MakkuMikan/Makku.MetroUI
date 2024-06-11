using System;
using System.Collections.Generic;

namespace Makku.MetroUI.Tables
{
    public class MetroColumnBuilder
    {
        protected virtual Column Column { get; }
        private MetroTableBuilder TableBuilder { get; }

        public MetroColumnBuilder(MetroTableBuilder tableBuilder)
        {
            Column = new Column();
            TableBuilder = tableBuilder;
        }

        public MetroColumnBuilder(MetroTableBuilder tableBuilder, string name)
        {
            Column = new Column(name);
            TableBuilder = tableBuilder;
        }

        public MetroColumnBuilder(MetroTableBuilder tableBuilder, Column column)
        {
            Column = column;
            TableBuilder = tableBuilder;
        }

        public MetroColumnBuilder(MetroTableBuilder tableBuilder, string name, string title = null, string size = null, bool sortable = false, MetroTable.SortDirection? sortDirection = null, MetroTable.Format? format = null)
        {
            Column = new Column(name: name, title: title, size: size, sortable: sortable)
            {
                SortDir = MetroTable.ToString(sortDirection),
                Format = MetroTable.ToString(format)
            };

            TableBuilder = tableBuilder;
        }

        public MetroColumnBuilder WithName(string name)
        {
            Column.Name = name;
            return this;
        }

        public MetroTableBuilder Save() => TableBuilder.WithColumn(Column);

        public MetroColumnBuilder WithColumn(string name) => Save().WithColumn(name);
        public MetroColumnBuilder WithSortableColumn(string name) => Save().WithSortableColumn(name);

        public MetroMappedColumnBuilder<T> WithMapping<T>(Func<T, object> expression)
            => MetroMappedColumnBuilder<T>.WithMapping(TableBuilder.Table, Column, e => expression.Invoke(e)?.ToString() ?? "");

        public MetroDataTableBuilder WithData(IEnumerable<IEnumerable<string>> data) => Save().WithData(data);
        public MetroCustomTableBuilder WithCustom() => Save().WithCustom();
    }
}
