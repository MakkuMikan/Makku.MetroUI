using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Makku.MetroUI.Tables
{
    public partial class MetroTableBuilder
    {
        public MetroTableBuilder()
        {
            Table = new MetroTable();
        }

        public MetroTable Table { get; }

        public static MetroMappedTableBuilder<TModel> NewTable<TModel>()
            => new MetroMappedTableBuilder<TModel>();

        public MetroColumnBuilder WithColumn(string name) => new MetroColumnBuilder(this, name);
        public MetroColumnBuilder WithColumn(string name, string title) => new MetroColumnBuilder(this, name, title);
        public MetroColumnBuilder WithSortableColumn(string name) => WithSortableColumn(name, name);
        public MetroColumnBuilder WithSortableColumn(string name, string title) => new MetroColumnBuilder(this, name, title, sortable: true);
        public MetroColumnBuilder WithHiddenColumn(string name) => new MetroColumnBuilder(this, name).Hidden();
        public MetroColumnBuilder WithHiddenColumn(string name, string title) => new MetroColumnBuilder(this, name, title).Hidden();

        public MetroTableBuilder WithColumn(Column column)
        {
            Table.Columns.Add(column);
            return this;
        }

        public MetroColumnBuilder WithColumn(string name, string title = null, string size = null, bool sortable = false, MetroTable.SortDirection? sortDirection = null, MetroTable.Format? format = null)
        {
            return new MetroColumnBuilder(this, name, title, size, sortable, sortDirection, format);
        }

        public MetroColumnBuilder WithSortableColumn(string name, string title = null, string size = null, MetroTable.SortDirection? sortDirection = null, MetroTable.Format? format = null)
            => WithColumn(name, title, size, true, sortDirection, format);

        public MetroDataTableBuilder WithData(IEnumerable<IEnumerable<string>> values)
            => MetroDataTableBuilder.WithData(Table, values);

        public MetroCustomTableBuilder WithCustom()
            => MetroCustomTableBuilder.WithTable(Table);
    }

    public class MetroCustomTableBuilder
    {
        public readonly MetroTable Table;

        public MetroCustomTableBuilder(MetroTable table)
        {
            Table = table;
        }

        public static MetroCustomTableBuilder WithTable(MetroTable table)
            => new MetroCustomTableBuilder(table);
    }

    public class MetroDataTableBuilder
    {
        private readonly MetroTable Table;

        public MetroDataTableBuilder(MetroTable table)
        {
            Table = table;
        }

        public static MetroDataTableBuilder WithData(MetroTable table, IEnumerable<IEnumerable<string>> values)
        {
            var instance = new MetroDataTableBuilder(table);
            instance.Table.Rows = values;
            return instance;
        }

        public MetroTable Compile() => Table;
    }

    public class MetroFilledTableBuilder
    {
        private readonly MetroTable Table;

        public MetroFilledTableBuilder(MetroTable table)
        {
            Table = table;
        }

        public static MetroFilledTableBuilder FromTable(MetroTable table)
            => new MetroFilledTableBuilder(table);

        public MetroTable Compile() => Table;
    }
}
