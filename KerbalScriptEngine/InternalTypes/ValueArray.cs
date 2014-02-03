using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KerboScriptEngine.InternalTypes
{
    class ValueArray : Value, ICollection<Value>
    {
        List<Value> items;

        public ValueArray()
        {
            items = new List<Value>();
            IsReadOnly = false;
        }

        public void Add(Value item)
        {
            items.Add(item);
        }

        public void Clear()
        {
            items.Clear();
        }

        public bool Contains(Value item)
        {
            return items.Contains(item);
        }

        public void CopyTo(Value[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return items.Count; }
        }

        public bool IsReadOnly
        {
            get;
            private set;
        }

        public bool Remove(Value item)
        {
            return items.Remove(item);
        }

        public IEnumerator<Value> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Makes the array read-only. Once this is done, it cannot be undone. This should not be exposed to script.
        /// </summary>
        public void Lock()
        {
            IsReadOnly = true;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{ ");
            for (int i = 0; i < Count; i++)
            {
                sb.Append(items[i].ToString());
                if (i < Count - 1) sb.Append(", ");
            }
            sb.Append(" }");

            return sb.ToString();
        }
    }
}
