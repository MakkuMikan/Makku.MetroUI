using Newtonsoft.Json;

namespace Makku.MetroUI.Tables
{
    public class Column
    {
        #region Properties
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("size")]
        public object Size { get; set; }// = 100;
        [JsonProperty("sortable")]
        public bool Sortable { get; set; } = true;
        [JsonProperty("sortDir")]
        public string SortDir { get; set; } = null;
        [JsonProperty("format")]
        public string Format { get; set; } = null;
        [JsonProperty("formatMask")]
        public string FormatMask { get; set; } = null;
        [JsonProperty("show")]
        public bool Show { get; set; } = true;
        [JsonProperty("template")]
        public string Template { get; set; } = null;
        #endregion Properties

        #region Constants
        public static class SortDirections
        {
            public const string Ascending = "asc";
            public const string Descending = "desc";
        }
        public static class Formats
        {
            public const string String = "string";
            public const string Date = "date";
            public const string Number = "number";
            public const string Integer = "int";
            public const string Float = "float";
            public const string Money = "money";
        }
        #endregion Constants

        #region Self-Setters
        public Column SortAsc()
        {
            Sortable = true;
            SortDir = SortDirections.Ascending;
            return this;
        }
        public Column SortDesc()
        {
            Sortable = true;
            SortDir = SortDirections.Descending;
            return this;
        }
        #endregion Self-Setters

        #region Constructors
        public Column()
        {
            Sortable = true;
            Show = true;
        }

        public Column(string name) : this(name: name, name) { }
        public Column(string name, int size) : this(name: name, title: name, size: $"{size}") { }
        public Column(string name, string size) : this(name: name, title: name, size: size) { }
        public Column(string name, string title, int size) : this(name: name, title: title, size: $"{size}") { }

        public Column(string name, string title, string size, bool sortable = false)
        {
            Name = name;
            Title = title ?? name;
            Size = size;
            Sortable = sortable;
        }
        #endregion Constructors
    }
}
