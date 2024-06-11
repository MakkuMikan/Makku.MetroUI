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

        public MetroColumnBuilder WithColumn(string name)
        {
            return new MetroColumnBuilder(this, name);
        }

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

    public class MetroTableBuilder<TModel>
    {
        public static MetroMappedTableBuilder<TModel> NewTable() => new MetroMappedTableBuilder<TModel>();

        public static MetroMappedColumnBuilder<TModel> WithColumn(string name) => NewTable().WithColumn(name);

        public static MetroMappedTableBuilder<TModel> WithColumn(DataColumn<TModel> column) => NewTable().WithColumn(column);

        public static MetroMappedColumnBuilder<TModel> WithColumn(string name, string title = null, string size = null, bool sortable = false, MetroTable.SortDirection? sortDirection = null, MetroTable.Format? format = null)
            => NewTable().WithColumn(name, title, size, sortable, sortDirection, format);

        public static MetroMappedColumnBuilder<TModel> WithSortableColumn(string name, string title = null, string size = null, MetroTable.SortDirection? sortDirection = null, MetroTable.Format? format = null)
            => WithColumn(name, title, size, true, sortDirection, format);
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
            instance.Table.Rows = (ConcurrentBag<List<string>>)values;
            return instance;
        }

        public MetroTable Compile() => Table;
    }

    //public class MetroQueryTableBuilder<T>
    //{
    //    private readonly IMetroTable Table;
    //    private IQueryable<IEnumerable<T>> _Query { get; set; }

    //    public MetroQueryTableBuilder(IMetroTable table)
    //    {
    //        Table = table;
    //    }

    //    public static MetroQueryTableBuilder<T> WithQuery(IMetroTable table, IQueryable<IEnumerable<T>> query)
    //    {
    //        var instance = new MetroQueryTableBuilder<T>(table);
    //        instance._Query = query;
    //        return instance;
    //    }

    //    public MetroFilledTableBuilder<T> Query()
    //    {
    //        Table.Rows = (ConcurrentBag<List<T>>)_Query.AsEnumerable();

    //        return MetroFilledTableBuilder<T>.FromTable(Table);
    //    }

    //    public IMetroTable QueryAndCompile() => Query().Compile();
    //}

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
