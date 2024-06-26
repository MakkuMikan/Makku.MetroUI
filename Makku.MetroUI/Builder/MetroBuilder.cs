using System;
using System.Collections.Generic;
using System.Text;
using Makku.MetroUI.Tables;

namespace Makku.MetroUI.Builder
{
    public static class MetroBuilder
    {
        public static class Table
        {
            public static MetroTableBuilder NewTable() => new MetroTableBuilder();

            public static MetroColumnBuilder WithColumn(string name) => NewTable().WithColumn(name);
            public static MetroColumnBuilder WithColumn(string name, string title) => NewTable().WithColumn(name, title);
            public static MetroColumnBuilder WithSortableColumn(string name) => NewTable().WithSortableColumn(name);
            public static MetroColumnBuilder WithSortableColumn(string name, string title) => NewTable().WithSortableColumn(name, title);
            public static MetroColumnBuilder WithHiddenColumn(string name) => NewTable().WithHiddenColumn(name);
            public static MetroColumnBuilder WithHiddenColumn(string name, string title) => NewTable().WithHiddenColumn(name, title);

            public static MetroTableBuilder WithColumn(Column column) => NewTable().WithColumn(column);

            public static MetroColumnBuilder WithColumn(string name, string title = null, string size = null, bool sortable = false, MetroTable.SortDirection? sortDirection = null, MetroTable.Format? format = null)
                => NewTable().WithColumn(name, title, size, sortable, sortDirection, format);

            public static MetroColumnBuilder WithSortableColumn(string name, string title = null, string size = null, MetroTable.SortDirection? sortDirection = null, MetroTable.Format? format = null)
                => WithColumn(name, title, size, true, sortDirection, format);
        }

        public static class Table<TModel>
        {
            public static MetroMappedTableBuilder<TModel> NewTable() => new MetroMappedTableBuilder<TModel>();

            public static MetroMappedColumnBuilder<TModel> WithColumn(string name) => NewTable().WithColumn(name);
            public static MetroMappedColumnBuilder<TModel> WithColumn(string name, string title) => NewTable().WithColumn(name, title);
            public static MetroMappedColumnBuilder<TModel> WithSortableColumn(string name) => NewTable().WithSortableColumn(name);
            public static MetroMappedColumnBuilder<TModel> WithSortableColumn(string name, string title) => NewTable().WithSortableColumn(name, title);
            public static MetroMappedColumnBuilder<TModel> WithHiddenColumn(string name) => NewTable().WithHiddenColumn(name);
            public static MetroMappedColumnBuilder<TModel> WithHiddenColumn(string name, string title) => NewTable().WithHiddenColumn(name, title);

            public static MetroMappedTableBuilder<TModel> WithColumn(DataColumn<TModel> column) => NewTable().WithColumn(column);

            public static MetroMappedColumnBuilder<TModel> WithColumn(string name, string title = null, string size = null, bool sortable = false, MetroTable.SortDirection? sortDirection = null, MetroTable.Format? format = null)
                => NewTable().WithColumn(name, title, size, sortable, sortDirection, format);

            public static MetroMappedColumnBuilder<TModel> WithSortableColumn(string name, string title = null, string size = null, MetroTable.SortDirection? sortDirection = null, MetroTable.Format? format = null)
                => WithColumn(name, title, size, true, sortDirection, format);
        }
    }
}
