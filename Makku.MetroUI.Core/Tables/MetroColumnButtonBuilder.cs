using System;
using System.Collections.Generic;

namespace Makku.MetroUI.Tables
{
    public class MetroMappedColumnButtonBuilder<TModel>
    {
        public MetroMappedColumnButtonBuilder(MetroMappedColumnBuilder<TModel> columnBuilder)
        {
            ColumnBuilder = columnBuilder;
        }

        public MetroMappedColumnBuilder<TModel> ColumnBuilder { get; }
        public ColumnButton<TModel> Button { get; } = new ColumnButton<TModel>();

        public static MetroMappedColumnButtonBuilder<TModel> WithName(MetroMappedColumnBuilder<TModel> columnBuilder, string name)
        {
            var instance = new MetroMappedColumnButtonBuilder<TModel>(columnBuilder);
            instance.Button.Text = name;
            return instance;
        }

        public MetroMappedColumnButtonBuilder<TModel> WithAction(Func<TModel, string> action)
        {
            Button.Action = action;
            return this;
        }

        public MetroMappedColumnButtonBuilder<TModel> WithHref(Func<TModel, string> href)
        {
            Button.Action = model => $"window.location.href = '{href(model) ?? "#"}'";
            return this;
        }

        public MetroMappedColumnBuilder<TModel> Save() => ColumnBuilder.WithButton(Button);

        public MetroMappedColumnBuilder<TModel> WithColumn(string name) => Save().WithColumn(name);
        public MetroMappedColumnBuilder<TModel> WithSortableColumn(string name) => Save().WithSortableColumn(name);

        public MetroMappedDataTableBuilder<TModel> WithData(IEnumerable<TModel> data) => Save().WithData(data);
        public MetroCustomTableBuilder WithCustom() => Save().WithCustom();
    }
}
