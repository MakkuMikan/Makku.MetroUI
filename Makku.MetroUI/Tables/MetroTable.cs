using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Makku.MetroUI.Tables
{
    public class MetroTable
    {
        [JsonProperty("header")]
        public List<Column> Columns { get; set; }
        [JsonProperty("data")]
        public IEnumerable<IEnumerable<string>> Rows { get; set; } = new List<IEnumerable<string>>();

        public void AddRow(List<string> values) => Rows = Rows.Append(values);
        public void AddRow(params string[] values) => Rows = Rows.Append(values);

        public enum SortDirection
        {
            Ascending,
            Descending
        }

        public static string ToString(SortDirection? direction)
        {
            switch (direction)
            {
                case SortDirection.Ascending: return "asc";
                case SortDirection.Descending: return "desc";
                default: return null;
            }
        }

        public enum Format
        {
            String,
            Date,
            Number,
            Integer,
            Float,
            Money
        }

        public static string ToString(Format? format)
        {
            switch (format)
            {
                case Format.String: return "string";
                case Format.Date: return "date";
                case Format.Number: return "number";
                case Format.Integer: return "int";
                case Format.Float: return "float";
                case Format.Money: return "money";
                default: return null;
            }
        }

        public MetroTable()
        {
            Columns = new List<Column>();
            Rows = new List<IEnumerable<string>>();
        }
    }
}
