using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace QualRazorLib.Observer
{
    public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>,INotifyCollectionChanged, INotifyPropertyChanged where TKey:notnull
    {
        private readonly Dictionary<TKey, TValue> _dictionary = new();

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public TValue this[TKey key]
        {
            get => _dictionary[key];
            set
            {
                bool exists = _dictionary.ContainsKey(key);
                _dictionary[key] = value;
                if (exists)
                {
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                        NotifyCollectionChangedAction.Replace, value, key));
                }
                else
                {
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                        NotifyCollectionChangedAction.Add, value, key));
                    OnPropertyChanged(nameof(Count));
                }
            }
        }

        public ICollection<TKey> Keys => _dictionary.Keys;
        public ICollection<TValue> Values => _dictionary.Values;
        public int Count => _dictionary.Count;
        public bool IsReadOnly => false;

        public void Add(TKey key, TValue value)
        {
            _dictionary.Add(key, value);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Add, value, key));
            OnPropertyChanged(nameof(Count));
        }

        public bool Remove(TKey key)
        {
            if (_dictionary.TryGetValue(key, out var value) && _dictionary.Remove(key))
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Remove, value, key));
                OnPropertyChanged(nameof(Count));
                return true;
            }
            return false;
        }

        public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);
        public bool TryGetValue(TKey key, out TValue value) => _dictionary.TryGetValue(key, out value);
        public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);
        public void Clear()
        {
            _dictionary.Clear();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            OnPropertyChanged(nameof(Count));
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) => _dictionary.ContainsKey(item.Key);
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) =>
            ((IDictionary<TKey, TValue>)_dictionary).CopyTo(array, arrayIndex);
        public bool Remove(KeyValuePair<TKey, TValue> item) => Remove(item.Key);
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e) =>
            CollectionChanged?.Invoke(this, e);

        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
