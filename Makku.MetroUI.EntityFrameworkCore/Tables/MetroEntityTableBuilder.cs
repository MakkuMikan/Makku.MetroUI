using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace Makku.MetroUI.Tables
{
    public class MetroEntityTableBuilder<TEntity>
    {
        private IMetroTable Table { get; }
        private IQueryable<IEnumerable<string>> Query { get; set; }

        public MetroEntityTableBuilder(IMetroTable table)
        {
            Table = table;
        }

        public static MetroEntityTableBuilder<TEntity> WithQuery(IMetroTable table, IQueryable<IEnumerable<string>> query)
        {
            var instance = new MetroEntityTableBuilder<TEntity>(table);
            instance.Query = query;
            return instance;
        }

        public async Task<MetroFilledTableBuilder> QueryAsync()
        {
            IEnumerable<IEnumerable<string>> data;

            data = await Query.ToListAsync();

            Table.Rows = (ConcurrentBag<List<string>>)data;

            return MetroFilledTableBuilder.FromTable(Table);
        }

        public async Task<IMetroTable> QueryAndCompileAsync()
            => (await QueryAsync()).Compile();
    }
}
