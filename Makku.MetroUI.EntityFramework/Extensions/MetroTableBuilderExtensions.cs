using Makku.MetroUI.Tables;
using Microsoft.EntityFrameworkCore;

namespace Makku.MetroUI.Extensions
{
    public static class MetroTableBuilderExtensions
    {
        public static MetroEntityTableBuilder<T> WithEFQuery<T>(this MetroTableBuilder tableBuilder, IQueryable<IEnumerable<string>> query) where T : class
            => MetroEntityTableBuilder<T>.WithQuery(tableBuilder.WithCustom().Table, query);

        public static MetroMappedEntityTableBuilder<T> WithEFQuery<T>(this MetroMappedTableBuilder<T> tableBuilder, IQueryable<T> query) where T : class
            => MetroMappedEntityTableBuilder<T>.WithQuery(tableBuilder.WithCustom().Table, query);

        public static MetroMappedEntityTableBuilder<T> WithContext<T>(this MetroMappedTableBuilder<T> tableBuilder, DbContext dbContext) where T : class
            => MetroMappedEntityTableBuilder<T>.WithContext(tableBuilder.WithCustom().Table, dbContext);
    }
}
