using System;

namespace Makku.MetroUI.Tables
{
    public class ColumnButton<TModel>
    {
        public string Text { get; set; }
        public Func<TModel, string> Action { get; set; }
        public string[] Classes { get; set; } = new string[] { "primary", "active" };
    }
}
