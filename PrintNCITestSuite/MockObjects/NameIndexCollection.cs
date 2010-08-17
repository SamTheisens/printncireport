using System;
using System.Collections.Specialized;

namespace PrintNCITestSuite.MockObjects
{
    //We can always replace the internal implementation with something better later.
    public class NameIndexCollection : NameObjectCollectionBase
    {
        /// <summary>
        /// Adds the specified name.
        /// </summary>
        /// <param name="key">The name.</param>
        /// <param name="index">The index.</param>
        public void Add(string key, int index)
        {
            BaseAdd(key, index);
        }

        /// <summary>
        /// Gets the <see cref="Int32"/> with the specified key.
        /// </summary>
        /// <value></value>
        public int this[string key]
        {
            get
            {
                return (int)BaseGet(key);
            }
        }

        public string[] GetAllKeys()
        {
            return BaseGetAllKeys();
        }
    }

    public class StubRow
    {
        readonly object[] rowValues;

        /// <summary>
        /// Initializes a new instance of the <see cref="StubRow"/> class.
        /// </summary>
        /// <param name="values">The values.</param>
        public StubRow(params object[] values)
        {
            rowValues = values;
        }

        /// <summary>
        /// Gets the <see cref="Object"/> with the specified i.
        /// </summary>
        /// <value></value>
        public object this[int i]
        {
            get
            {
                return rowValues[i];
            }
        }
    }
}
