using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jebnix
{
    public class QueueStack<T> : ICollection<T>
    {
        List<T> list;

        public QueueStack()
        {
            list = new List<T>();
        }

        public void Add(T item)
        {
            list.Add(item);
        }

        public void Enqueue(T item)
        {
            Add(item);
        }

        public T Dequeue()
        {
            if (Count > 0)
            {
                T item = list[0];
                list.RemoveAt(0);
                return item;
            }
            else
            {
                throw new InvalidOperationException("The collection is empty.");
            }
        }

        public void Push(T item)
        {
            Add(item);
        }

        public T Pop()
        {
            if (Count > 0)
            {
                T item = list[Count - 1];
                list.RemoveAt(Count - 1);
                return item;
            }
            else
            {
                throw new InvalidOperationException("The collection is empty.");
            }
        }

        public T PeekFront()
        {
            if (Count > 0)
            {
                return list[0];
            }
            else
            {
                throw new InvalidOperationException("The collection is empty.");
            }
        }

        public T PeekEnd()
        {
            if (Count > 0)
            {
                return list[Count - 1];
            }
            else
            {
                throw new InvalidOperationException("The collection is empty.");
            }
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(T item)
        {
            return list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return list.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            return list.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
