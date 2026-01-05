using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace com.fabioscagliola.Core.DataAccess
{
    public class DataAccessList<T> : IList<T>, INotifyCollectionChanged
    {
        protected IList<T> list;

        public DataAccessList(IList<T> list)
        {
            this.list = list;

            foreach (T item in list)
            {
                if (item is INotifyPropertyChanged)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged += DataAccessList_PropertyChanged;
                }
            }
        }

        private void DataAccessList_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public T this[int index]
        {
            get
            {
                return list[index];
            }
            set
            {
                T oldItem = list[index];
                if (oldItem != null && oldItem is INotifyPropertyChanged)
                {
                    ((INotifyPropertyChanged)oldItem).PropertyChanged -= DataAccessList_PropertyChanged;
                }
                list[index] = value;
                if (value != null && value is INotifyPropertyChanged)
                {
                    ((INotifyPropertyChanged)value).PropertyChanged += DataAccessList_PropertyChanged;
                }
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, oldItem, index));
            }
        }

        public int Count
        {
            get
            {
                return list.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return list.IsReadOnly;
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void Add(T item)
        {
            list.Add(item);
            int index = list.IndexOf(item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
            if (item is INotifyPropertyChanged)
            {
                ((INotifyPropertyChanged)item).PropertyChanged += DataAccessList_PropertyChanged;
            }
        }

        public void Clear()
        {
            foreach (INotifyPropertyChanged item in list.OfType<INotifyPropertyChanged>())
            {
                item.PropertyChanged -= DataAccessList_PropertyChanged;
            }
            list.Clear();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(T item)
        {
            return list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            list.Insert(index, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
            if (item is INotifyPropertyChanged)
            {
                ((INotifyPropertyChanged)item).PropertyChanged += DataAccessList_PropertyChanged;
            }
        }

        public bool Remove(T item)
        {
            int index = list.IndexOf(item);
            bool result = list.Remove(item);
            if (result)
            {
                if (item is INotifyPropertyChanged)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged += DataAccessList_PropertyChanged;
                }

                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
            }
            return result;
        }

        public void RemoveAt(int index)
        {
            T item = list[index];
            if (item is INotifyPropertyChanged)
            {
                ((INotifyPropertyChanged)item).PropertyChanged += DataAccessList_PropertyChanged;
            }
            list.RemoveAt(index);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
        }

        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            CollectionChanged?.Invoke(this, args);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}

