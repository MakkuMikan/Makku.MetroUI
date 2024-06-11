using System;
using System.Collections.Generic;
using System.Linq;

namespace Makku.MetroUI.Tables
{
    public class MetroMappedColumnBuilder<TModel>
    {
        protected virtual DataColumn<TModel> Column { get; }
        private MetroMappedTableBuilder<TModel> TableBuilder { get; }

        public MetroMappedColumnBuilder(MetroMappedTableBuilder<TModel> tableBuilder)
        {
            Column = new DataColumn<TModel>();
            TableBuilder = tableBuilder;
        }

        public MetroMappedColumnBuilder(MetroMappedTableBuilder<TModel> tableBuilder, string name)
        {
            Column = new DataColumn<TModel>(name);
            TableBuilder = tableBuilder;
        }

        public MetroMappedColumnBuilder(MetroMappedTableBuilder<TModel> tableBuilder, DataColumn<TModel> column)
        {
            Column = column;
            TableBuilder = tableBuilder;
        }

        public MetroMappedColumnBuilder(MetroMappedTableBuilder<TModel> tableBuilder, string name, string title = null, string size = null, bool sortable = false, MetroTable.SortDirection? sortDirection = null, MetroTable.Format? format = null)
        {
            Column = new DataColumn<TModel>(name: name, title: title, size: size, sortable: sortable)
            {
                SortDir = MetroTable.ToString(sortDirection),
                Format = MetroTable.ToString(format)
            };

            TableBuilder = tableBuilder;
        }

        public MetroMappedColumnBuilder<TModel> WithName(string name)
        {
            Column.Name = name;
            return this;
        }

        public MetroMappedColumnButtonBuilder<TModel> WithButton(string name)
            => MetroMappedColumnButtonBuilder<TModel>.WithName(this, name);

        public MetroMappedColumnBuilder<TModel> WithButton(string name, Func<TModel, string> action)
            => MetroMappedColumnButtonBuilder<TModel>.WithName(this, name).WithAction(action).Save();

        public MetroMappedColumnBuilder<TModel> WithButton(ColumnButton<TModel> button)
        {
            Column.Buttons.Add(button);
            return this;
        }

        public MetroMappedTableBuilder<TModel> Save()
        {
            if (Column.Buttons.Count > 0)
            {
                Func<TModel, string> mapping = model => Column.Buttons.Select(b => $"<button class=\"button {string.Join(" ", b.Classes)}\" onclick=\"{b.Action.Invoke(model)}\">{b.Text}</button>").Aggregate((a, b) => $"{a}{b}");
                Column.Mapping = mapping;
            }

            return TableBuilder.WithColumn(Column);
        }

        public MetroMappedColumnBuilder<TModel> WithColumn(string name) => Save().WithColumn(name);
        public MetroMappedColumnBuilder<TModel> WithColumn(string name, string title) => Save().WithColumn(name, title);
        public MetroMappedColumnBuilder<TModel> WithSortableColumn(string name) => Save().WithSortableColumn(name);
        public MetroMappedColumnBuilder<TModel> WithSortableColumn(string name, string title) => Save().WithSortableColumn(name, title);

        public MetroMappedColumnBuilder<TModel> WithMapping(Func<TModel, object> expression)
            => MetroMappedColumnBuilder<TModel>.WithMapping(TableBuilder, Column, e => expression.Invoke(e)?.ToString() ?? "");

        public MetroMappedColumnBuilder<TModel> WithSize(int size)
        {
            Column.Size = size;
            return this;
        }

        public MetroMappedColumnBuilder<TModel> WithSize(string size)
        {
            Column.Size = size;
            return this;
        }

        public MetroMappedColumnBuilder<TModel> AsImage()
        {
            var existingMapping = Column.Mapping;
            Column.Mapping = (TModel model) =>
            {
                var a = existingMapping.Invoke(model);
                return $"<img class=\"table-image\" src=\"{a}\" />";
            };
            return this;
        }

        public MetroMappedDataTableBuilder<TModel> WithData(IEnumerable<TModel> data) => Save().WithData(data);
        public MetroCustomTableBuilder WithCustom() => Save().WithCustom();

        public static MetroMappedColumnBuilder<TModel> WithMapping(MetroMappedTableBuilder<TModel> tableBuilder, DataColumn<TModel> column, Func<TModel, string> expression)
        {
            var instance = new MetroMappedColumnBuilder<TModel>(tableBuilder, column);
            instance.Column.Mapping = expression;
            return instance;
        }

        public static MetroMappedColumnBuilder<TModel> WithMapping(MetroTable table, DataColumn<TModel> column, Func<TModel, string> expression)
        {
            var tableBuilder = new MetroMappedTableBuilder<TModel>(table);
            var instance = new MetroMappedColumnBuilder<TModel>(tableBuilder, column);
            instance.Column.Mapping = expression;
            return instance;
        }

        public static MetroMappedColumnBuilder<TModel> WithMapping(MetroTable table, Column column, Func<TModel, string> expression)
        {
            var instance = new MetroMappedColumnBuilder<TModel>(new MetroMappedTableBuilder<TModel>(table), new DataColumn<TModel>(column));
            instance.Column.Mapping = expression;
            return instance;
        }

        public MetroMappedColumnBuilder<TModel> WithMapping(Func<TModel, string> expression)
        {
            Column.Mapping = model => expression(model) ?? "";
            return this;
        }
    }
}
