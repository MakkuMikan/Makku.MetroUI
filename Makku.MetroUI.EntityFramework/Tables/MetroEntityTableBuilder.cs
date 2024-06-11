﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Makku.MetroUI.Tables
{
    public class MetroEntityTableBuilder<TEntity>
    {
        private MetroTable Table { get; }
        private IQueryable<IEnumerable<string>> Query { get; set; }

        public MetroEntityTableBuilder(MetroTable table)
        {
            Table = table;
        }

        public static MetroEntityTableBuilder<TEntity> WithQuery(MetroTable table, IQueryable<IEnumerable<string>> query)
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

        public async Task<MetroTable> QueryAndCompileAsync()
            => (await QueryAsync()).Compile();
    }
}
