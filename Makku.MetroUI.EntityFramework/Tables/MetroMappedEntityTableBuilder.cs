using Makku.MetroUI.Helpers;
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
            IEnumerable<IEnumerable<object>> data;

            var mappings = Table.Columns.OfType<DataColumn<TEntity>>().Select(c => c.Mapping).ToArray() ?? Array.Empty<Expression<Func<TEntity, object>>>();

            var convert = CombineExpressions(mappings);

            data = await Query.Select(convert).ToListAsync();

            if (!data.Any())
            {
                return MetroFilledTableBuilder.FromTable(Table);
            }

            var postProcessing = Table.Columns.OfType<DataColumn<TEntity>>().Select(c => c.PostProcess);

            if (postProcessing.Count() != data.First().Count())
            {
                throw new Exception("");
            }

            Table.Rows = data.Select(row => row.Zip(postProcessing, (value, process) => process.Compile().Invoke(value)));

            return MetroFilledTableBuilder.FromTable(Table);
        }

        public static Expression<Func<TEntity, object[]>> CombineExpressions(params Expression[] expressions)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "x");

            var arrayInit = Expression.NewArrayInit(
                typeof(object),
                expressions.Cast<Expression<Func<TEntity, dynamic>>>().Select(e => ReplaceParameter(e.Body, e.Parameters[0], parameter))
            );

            return Expression.Lambda<Func<TEntity, object[]>>(arrayInit, parameter);
        }

        public static Expression ReplaceParameter(Expression expression, ParameterExpression source, Expression target)
        {
            return new ParameterReplacer(source, target).Visit(expression);
        }

        public async Task<MetroTable> QueryAndCompileAsync()
            => (await QueryAsync()).Compile();
    }
}
