using Makku.MetroUI.Tables;
using System.Data.Entity;
using System.Linq;

namespace Makku.MetroUI.Extensions
{
    public static class MetroMappedColumnButtonBuilderExtensions
    {
        public static MetroMappedEntityTableBuilder<T> WithEFQuery<T>(this MetroMappedColumnButtonBuilder<T> buttonBuilder, IQueryable<T> query) where T : class
            => MetroMappedEntityTableBuilder<T>.WithQuery(buttonBuilder.WithCustom().Table, query);

        public static MetroMappedEntityTableBuilder<T> WithContext<T>(this MetroMappedColumnButtonBuilder<T> buttonBuilder, DbContext dbContext) where T : class
            => MetroMappedEntityTableBuilder<T>.WithContext(buttonBuilder.WithCustom().Table, dbContext);
    }
}
