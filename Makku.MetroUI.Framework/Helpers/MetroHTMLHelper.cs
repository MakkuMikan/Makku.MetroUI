using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Makku.MetroUI.Helpers
{
    public abstract partial class MetroHTMLHelper<T> where T : class
    {
        private readonly TagBuilder Builder;

        public MetroHTMLHelper()
        {
            Builder = new TagBuilder(Tag);
            Init();
        }

        protected abstract string Tag { get; }

        protected abstract void Init();

        private bool ForceSelfClosingState { get; set; } = false;

        protected IDictionary<string, string> Attributes => Builder.Attributes;
        protected DatasetAttributes Dataset => new DatasetAttributes(Builder.Attributes);

        public T SetAttribute(string name, object value)
        {
            Builder.Attributes[name] = value.ToString();
            return this as T;
        }

        public T SetDataset(string name, object value)
        {
            if (value == null)
            {
                Builder.Attributes.Remove(name);
            }
            else
            {
                Builder.Attributes[$"data-{name}"] = value?.ToString();
            }

            return this as T;
        }

        public T SetClassName(string className)
        {
            Builder.Attributes["class"] = className;
            return this as T;
        }

        public T AddClass(string className)
        {
            Builder.AddCssClass(className);
            return this as T;
        }

        protected T ForceSelfClosing()
        {
            ForceSelfClosingState = true;
            Builder.InnerHtml = "";

            return this as T;
        }

        protected T SetInnerHTML(string innerHTML)
        {
            if (ForceSelfClosingState)
            {
                throw new InvalidOperationException($"Element <{Tag}> is self closing in this state. Cannot set InnerHTML.");
            }

            Builder.InnerHtml = innerHTML;
            return this as T;
        }

        protected T _(Action action)
        {
            action.Invoke();
            return this as T;
        }

        public IHtmlString Render()
        {
            return new MvcHtmlString(Builder.ToString());
        }
    }
}
