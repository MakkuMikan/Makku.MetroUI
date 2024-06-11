using Makku.MetroUI.Services;
using System.Web.Mvc;

namespace Makku.MetroUI.Framework.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static MetroHelper<TModel> Metro<TModel>(this HtmlHelper<TModel> helper)
        {
            return new MetroHelper<TModel>(helper);
        }
    }
}
