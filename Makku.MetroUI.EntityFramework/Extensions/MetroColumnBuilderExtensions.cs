using Makku.MetroUI.Tables;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Makku.MetroUI.Extensions
{
    public static class MetroColumnBuilderExtensions
    {
        public static MetroEntityTableBuilder<T> WithEFQuery<T>(this MetroColumnBuilder columnBuilder, IQueryable<IEnumerable<string>> query) where T : class
            => MetroEntityTableBuilder<T>.WithQuery(columnBuilder.WithCustom().Table, query);

        public static MetroMappedEntityTableBuilder<T> WithEFQuery<T>(this MetroMappedColumnBuilder<T> columnBuilder, IQueryable<T> query) where T : class
            => MetroMappedEntityTableBuilder<T>.WithQuery(columnBuilder.WithCustom().Table, query);

        public static MetroMappedEntityTableBuilder<T> WithContext<T>(this MetroMappedColumnBuilder<T> columnBuilder, DbContext dbContext) where T : class
            => MetroMappedEntityTableBuilder<T>.WithContext(columnBuilder.WithCustom().Table, dbContext);
    }
}
