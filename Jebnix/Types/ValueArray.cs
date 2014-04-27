using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jebnix.Types
{
    public class ValueArray : JObject, ICollection<JObject>
    {
        public new const string TYPENAME = "Array";

        List<JObject> items;

        public ValueArray()
            : base(false, TYPENAME)
        {
            items = new List<JObject>();
            IsReadOnly = false;
        }

        public override bool IsNull
        {
            get
            {
                return false;
            }
            protected set
            {
                throw new NotImplementedException();
            }
        }

        public void Add(JObject item)
        {
            items.Add(item);
        }

        public void Clear()
        {
            items.Clear();
        }

        public bool Contains(JObject item)
        {
            return items.Contains(item);
        }

        public void CopyTo(JObject[] array, int arrayIndex)
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

        public bool Remove(JObject item)
        {
            return items.Remove(item);
        }

        public IEnumerator<JObject> GetEnumerator()
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

        protected override bool IsEqual(JObject a)
        {
            if (a.ObjectType != "Array")
                return false;

            if (IsNull & a.IsNull)
                return true;

            ValueArray array = (ValueArray)a;

            if (array.Count != Count)
                return false;

            for (int i = 0; i < Count; i++)
            {
                if (items[i] != array.items[i])
                    return false;
            }

            return true;
        }

        protected override bool IsNotEqual(JObject a)
        {
            return !IsEqual(a);
        }

        protected override JObject IsLessThan(JObject a)
        {
            throw new InvalidOperationException();
        }

        protected override JObject IsLessThanOrEqual(JObject a)
        {
            throw new InvalidOperationException();
        }

        protected override JObject IsGreaterThan(JObject a)
        {
            throw new InvalidOperationException();
        }

        protected override JObject IsGreaterThanOrEqual(JObject a)
        {
            throw new InvalidOperationException();
        }

        protected override JObject Addition(JObject a)
        {
            throw new InvalidOperationException();
        }

        protected override JObject Subtract(JObject a)
        {
            throw new InvalidOperationException();
        }

        protected override JObject Multiply(JObject a)
        {
            throw new InvalidOperationException();
        }

        protected override JObject Divide(JObject a)
        {
            throw new InvalidOperationException();
        }

        protected override JObject Modulus(JObject a)
        {
            throw new InvalidOperationException();
        }

        protected override JObject Pow(JObject a)
        {
            throw new InvalidOperationException();
        }

        protected override JObject And(JObject a)
        {
            throw new InvalidOperationException();
        }

        protected override JObject Or(JObject a)
        {
            throw new InvalidOperationException();
        }

        protected override JObject Positive()
        {
            throw new InvalidOperationException();
        }

        protected override JObject Negative()
        {
            throw new InvalidOperationException();
        }

        protected override JObject Not()
        {
            throw new InvalidOperationException();
        }

        protected override JObject Increment()
        {
            throw new InvalidOperationException();
        }

        protected override JObject Decrement()
        {
            throw new InvalidOperationException();
        }
    }
}
