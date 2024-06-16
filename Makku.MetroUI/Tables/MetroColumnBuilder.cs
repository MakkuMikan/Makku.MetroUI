using Makku.MetroUI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

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

        public MetroColumnBuilder Hidden()
        {
            Column.Show = false;
            return this;
        }

        public MetroTableBuilder Save() => TableBuilder.WithColumn(Column);

        public MetroColumnBuilder WithColumn(string name) => Save().WithColumn(name);
        public MetroColumnBuilder WithColumn(string name, string title) => Save().WithColumn(name, title);
        public MetroColumnBuilder WithSortableColumn(string name) => Save().WithSortableColumn(name);
        public MetroColumnBuilder WithSortableColumn(string name, string title) => Save().WithSortableColumn(name, title);
        public MetroColumnBuilder WithHiddenColumn(string name) => Save().WithHiddenColumn(name);
        public MetroColumnBuilder WithHiddenColumn(string name, string title) => Save().WithHiddenColumn(name, title);

        public MetroMappedColumnBuilder<T> WithMapping<T>(Expression<Func<T, object>> expression)
        {
            //// Create a new parameter expression
            //ParameterExpression parameter = Expression.Parameter(typeof(T), "param");

            //// Replace the parameter in the original expression with the new parameter
            //Expression body = new ParameterReplacer(expression.Parameters[0], parameter).Visit(expression.Body);

            //// Add the null check and call to ToString() to the body
            //body = Expression.Condition(
            //    Expression.Equal(body, Expression.Constant(null)),
            //    Expression.Constant(null, typeof(string)),
            //    Expression.Call(body, typeof(object).GetMethod("ToString"))
            //);

            //// Create and return the new expression
            //return MetroMappedColumnBuilder<T>.WithMapping(TableBuilder.Table, Column, Expression.Lambda<Func<T, string>>(body, parameter));

            return MetroMappedColumnBuilder<T>.WithMapping(TableBuilder.Table, Column, expression);
        }

        public MetroDataTableBuilder WithData(IEnumerable<IEnumerable<string>> data) => Save().WithData(data);
        public MetroCustomTableBuilder WithCustom() => Save().WithCustom();
    }
}
