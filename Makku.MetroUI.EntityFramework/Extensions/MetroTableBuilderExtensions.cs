using Makku.MetroUI.Tables;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Makku.MetroUI.Extensions
{
    public static class MetroTableBuilderExtensions
    {
        public static MetroEntityTableBuilder WithEFQuery(this MetroTableBuilder tableBuilder, IQueryable<IEnumerable<string>> query)
            => MetroEntityTableBuilder.WithQuery(tableBuilder.WithCustom().Table, query);

        public static MetroMappedEntityTableBuilder<T> WithEFQuery<T>(this MetroMappedTableBuilder<T> tableBuilder, IQueryable<T> query) where T : class
            => MetroMappedEntityTableBuilder<T>.WithQuery(tableBuilder.WithCustom().Table, query);

        public static MetroMappedEntityTableBuilder<T> WithContext<T>(this MetroMappedTableBuilder<T> tableBuilder, DbContext dbContext) where T : class
            => MetroMappedEntityTableBuilder<T>.WithContext(tableBuilder.WithCustom().Table, dbContext);
    }
}
