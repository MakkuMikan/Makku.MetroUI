using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Makku.MetroUI.Helpers
{
    public abstract partial class MetroHTMLHelper<T> where T : class
    {
        public class DatasetAttributes : IDictionary<string, object>, IReadOnlyDictionary<string, object>
        {
            public DatasetAttributes(IDictionary<string, string> _dictionary)
            {
                dictionary = _dictionary;
            }

            private readonly IDictionary<string, string> dictionary;

            public object this[string key]
            {
                get => ((IDictionary<string, object>)dictionary)[$"data-{key}"];
                set => ((IDictionary<string, object>)dictionary)[$"data-{key}"] = value?.ToString();
            }

            public IDictionary<string, string> S => this.ToDictionary(k => k.Key, k => k.Value?.ToString());

            public ICollection<string> Keys => ((IDictionary<string, object>)dictionary).Keys;

            public ICollection<object> Values => ((IDictionary<string, object>)dictionary).Values;

            public int Count => ((ICollection<KeyValuePair<string, object>>)dictionary).Count;

            public bool IsReadOnly => ((ICollection<KeyValuePair<string, object>>)dictionary).IsReadOnly;

            IEnumerable<string> IReadOnlyDictionary<string, object>.Keys => ((IReadOnlyDictionary<string, object>)dictionary).Keys;

            IEnumerable<object> IReadOnlyDictionary<string, object>.Values => ((IReadOnlyDictionary<string, object>)dictionary).Values;

            public void Add(string key, object value)
            {
                ((IDictionary<string, object>)dictionary).Add(key, value);
            }

            public void Add(KeyValuePair<string, object> item)
            {
                ((ICollection<KeyValuePair<string, object>>)dictionary).Add(item);
            }

            public void Clear()
            {
                ((ICollection<KeyValuePair<string, object>>)dictionary).Clear();
            }

            public bool Contains(KeyValuePair<string, object> item)
            {
                return ((ICollection<KeyValuePair<string, object>>)dictionary).Contains(item);
            }

            public bool ContainsKey(string key)
            {
                return ((IDictionary<string, object>)dictionary).ContainsKey(key);
            }

            public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
            {
                ((ICollection<KeyValuePair<string, object>>)dictionary).CopyTo(array, arrayIndex);
            }

            public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
            {
                return ((IEnumerable<KeyValuePair<string, object>>)dictionary).GetEnumerator();
            }

            public bool Remove(string key)
            {
                return ((IDictionary<string, object>)dictionary).Remove(key);
            }

            public bool Remove(KeyValuePair<string, object> item)
            {
                return ((ICollection<KeyValuePair<string, object>>)dictionary).Remove(item);
            }

            public bool TryGetValue(string key, out object value)
            {
                return ((IDictionary<string, object>)dictionary).TryGetValue(key, out value);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)dictionary).GetEnumerator();
            }
        }
    }
}
