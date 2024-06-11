using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Makku.MetroUI.Tables
{
    public class MetroMappedEntityTableBuilder<TEntity> where TEntity : class
    {
        private MetroTable Table { get; }
        private IQueryable<TEntity> Query { get; set; }

        public MetroMappedEntityTableBuilder(MetroTable table)
        {
            Table = table;
        }

        public static MetroMappedEntityTableBuilder<TEntity> WithContext(MetroTable table, DbContext context)
        {
            var instance = new MetroMappedEntityTableBuilder<TEntity>(table);
            instance.Query = context.Set<TEntity>().AsQueryable();
            return instance;
        }

        public static MetroMappedEntityTableBuilder<TEntity> WithQuery(MetroTable table, IQueryable<TEntity> query)
        {
            var instance = new MetroMappedEntityTableBuilder<TEntity>(table);
            instance.Query = query;
            return instance;
        }

        public MetroMappedEntityTableBuilder<TEntity> AndInclude<TProperty>(Expression<Func<TEntity, TProperty>> navigationPropertyPath)
        {
            Query = Query.Include(navigationPropertyPath);
            return this;
        }

        public async Task<MetroFilledTableBuilder> QueryAsync()
        {
            IEnumerable<IEnumerable<string>> data;

            var mappings = Table.Columns.Select(c => (c is DataColumn<TEntity> dt) ? dt.Mapping : e => "").ToArray() ?? [];
            IEnumerable<string> processFunc(TEntity entity) => mappings.Select(m => m.Invoke(entity));
            data = (await Query.ToListAsync()).Select(r => mappings.Select(m => m.Invoke(r)).ToList()).ToList();

            Table.Rows = data;

            return MetroFilledTableBuilder.FromTable(Table);
        }

        public async Task<MetroTable> QueryAndCompileAsync()
            => (await QueryAsync()).Compile();
    }
}
