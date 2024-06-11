using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Makku.MetroUI.Tables
{
    public class MetroMappedTableBuilder<TModel>
    {
        public MetroMappedTableBuilder()
        {
            Table = new MetroTable();
        }

        public MetroMappedTableBuilder(MetroTable table)
        {
            Table = table;
        }

        private MetroTable Table { get; }

        public MetroMappedColumnBuilder<TModel> WithColumn(string name)
        {
            return new MetroMappedColumnBuilder<TModel>(this, name);
        }

        public MetroMappedTableBuilder<TModel> WithColumn(DataColumn<TModel> column)
        {
            Table.Columns.Add(column);
            return this;
        }

        public MetroMappedColumnBuilder<TModel> WithColumn(string name, string title = null, string size = null, bool sortable = false, MetroTable.SortDirection? sortDirection = null, MetroTable.Format? format = null)
        {
            return new MetroMappedColumnBuilder<TModel>(this, name, title, size, sortable, sortDirection, format);
        }

        public MetroMappedColumnBuilder<TModel> WithSortableColumn(string name, string title = null, string size = null, MetroTable.SortDirection? sortDirection = null, MetroTable.Format? format = null)
            => WithColumn(name, title, size, true, sortDirection, format);

        public MetroMappedDataTableBuilder<TModel> WithData(IEnumerable<TModel> values)
            => MetroMappedDataTableBuilder<TModel>.WithData(Table, values);

        public MetroCustomTableBuilder WithCustom()
            => MetroCustomTableBuilder.WithTable(Table);
    }

    public class MetroMappedDataTableBuilder<TModel>
    {
        private readonly MetroTable Table;

        public MetroMappedDataTableBuilder(MetroTable table)
        {
            Table = table;
        }

        public static MetroMappedDataTableBuilder<TModel> WithData(MetroTable table, IEnumerable<TModel> values)
        {
            var instance = new MetroMappedDataTableBuilder<TModel>(table);

            var mappings = instance.Table.Columns.Select(c => (c is DataColumn<TModel> dt) ? dt.Mapping : e => "").ToArray() ?? Enumerable.Empty<Func<TModel, string>>();
            IEnumerable<string> processFunc(TModel entity) => mappings.Select(m => m.Invoke(entity));

            instance.Table.Rows = (ConcurrentBag<List<string>>)values.Select(processFunc);
            return instance;
        }

        public MetroTable Compile() => Table;
    }
}
