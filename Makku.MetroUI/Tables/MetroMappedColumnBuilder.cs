using Makku.MetroUI.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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

        public MetroMappedColumnBuilder<TModel> Hidden()
        {
            Column.Show = false;
            return this;
        }

        public MetroMappedColumnButtonBuilder<TModel> WithButton(string name)
            => MetroMappedColumnButtonBuilder<TModel>.WithName(this, name);

        public MetroMappedColumnBuilder<TModel> WithButton(string name, Expression<Func<TModel, string>> action)
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
                var buttonExpressions = Column.Buttons.Select(b =>
                {
                    var sExpression = Expression.Call(
                        typeof(string).GetMethod("Concat", new[] { typeof(string), typeof(string), typeof(string) }),
                        Expression.Constant($"<button class=\"button {string.Join(" ", b.Classes)}\" onclick=\""),
                        b.Action.Body,
                        Expression.Constant($"\">{b.Text}</button>")
                    );

                    return Expression.Lambda<Func<TModel, string>>(sExpression, b.Action.Parameters[0]);
                });

                var surroundExpression = Expression.Call(
                    typeof(string).GetMethod("Concat", new[] { typeof(string[]) }),
                    Expression.NewArrayInit(typeof(string), buttonExpressions.Select(bEx => bEx.Body).ToArray())
                );

                Column.Mapping = Expression.Lambda<Func<TModel, object>>(surroundExpression, Column.Buttons[0].Action.Parameters[0]);
            }

            return TableBuilder.WithColumn(Column);
        }

        public MetroMappedColumnBuilder<TModel> WithColumn(string name) => Save().WithColumn(name);
        public MetroMappedColumnBuilder<TModel> WithColumn(string name, string title) => Save().WithColumn(name, title);
        public MetroMappedColumnBuilder<TModel> WithSortableColumn(string name) => Save().WithSortableColumn(name);
        public MetroMappedColumnBuilder<TModel> WithSortableColumn(string name, string title) => Save().WithSortableColumn(name, title);

        public MetroMappedColumnBuilder<TModel> WithMapping<TValue>(Expression<Func<TModel, TValue>> expression) where TValue : class
        {
            var mappingParameter = Expression.Parameter(typeof(TModel), "x");
            var mappingBody = Expression.Convert(Expression.Invoke(expression, mappingParameter), typeof(object));
            Column.Mapping = Expression.Lambda<Func<TModel, object>>(mappingBody, mappingParameter);
            return this;
        }

        public MetroMappedColumnBuilder<TModel> WithMapping<TValue>(Expression<Func<TModel, TValue>> expression, Expression<Func<TValue, string>> postProcess)
        {
            var mappingParameter = Expression.Parameter(typeof(TModel), "x");
            var mappingBody = Expression.Convert(Expression.Invoke(expression, mappingParameter), typeof(object));
            Column.Mapping = Expression.Lambda(mappingBody, mappingParameter);

            var ppParameter = Expression.Parameter(typeof(TValue), "x");
            var ppBody = Expression.Invoke(postProcess, Expression.Convert(ppParameter, typeof(TValue)));
            Column.PostProcess = Expression.Lambda<Func<object, string>>(ppBody, ppParameter);
            return this;
        }

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

            var entityParameter = existingMapping.Parameters[0];
            var surroundExpression = Expression.Call(
                method: typeof(string).GetMethod("Concat", new[] { typeof(string), typeof(string), typeof(string) }),
                Expression.Constant("<img class=\"table-image\" src=\""),
                existingMapping.Body,
                Expression.Constant("\" />")
            );

            Column.Mapping = Expression.Lambda<Func<TModel, object>>(surroundExpression, entityParameter);
            return this;
        }

        public MetroMappedDataTableBuilder<TModel> WithData(IEnumerable<TModel> data) => Save().WithData(data);
        public MetroCustomTableBuilder WithCustom() => Save().WithCustom();

        public static MetroMappedColumnBuilder<TModel> WithMapping(MetroMappedTableBuilder<TModel> tableBuilder, DataColumn<TModel> column, Expression<Func<TModel, object>> expression)
            => new MetroMappedColumnBuilder<TModel>(tableBuilder, column).WithMapping(expression);

        public static MetroMappedColumnBuilder<TModel> WithMapping<TValue>(MetroTable table, DataColumn<TModel> column, Expression<Func<TModel, TValue>> expression) where TValue : class
        {
            var tableBuilder = new MetroMappedTableBuilder<TModel>(table);
            var instance = new MetroMappedColumnBuilder<TModel>(tableBuilder, column);
            return instance.WithMapping(expression);
        }

        public static MetroMappedColumnBuilder<TModel> WithMapping<TValue>(MetroTable table, Column column, Expression<Func<TModel, TValue>> expression) where TValue : class
        {
            var instance = new MetroMappedColumnBuilder<TModel>(new MetroMappedTableBuilder<TModel>(table), new DataColumn<TModel>(column));
            return instance.WithMapping(expression);
        }

        public static MetroMappedColumnBuilder<TModel> Mapping<TValue>(MetroMappedTableBuilder<TModel> tableBuilder, DataColumn<TModel> column, Expression<Func<TModel, TValue>> expression, Expression<Func<TValue, string>> postProcess)
        {
            var instance = new MetroMappedColumnBuilder<TModel>(tableBuilder, column);
            return instance.WithMapping(expression, postProcess);
        }
    }
}
