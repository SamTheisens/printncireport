using System;
using System.Collections;
using CollectionBase=System.Collections.CollectionBase;

namespace PrintNCITestSuite.MockObjects
{
    /// <summary>
    /// Represents a collection of <see cref="StubRow">StubRow</see>.
    /// </summary>
    [Serializable]
    public class StubRowCollection : CollectionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StubRowCollection">StubRowCollection</see> class.
        /// </summary>
        public StubRowCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StubRowCollection">StubRowCollection</see> class containing the elements of the specified source collection.
        /// </summary>
        /// <param name="value">A <see cref="StubRowCollection">StubRowCollection</see> with which to initialize the collection.</param>
        public StubRowCollection(StubRowCollection value)
        {
            if (value != null)
            {
                AddRange(value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StubRowCollection">StubRowCollection</see> class containing the specified array of <see cref="StubRow">StubRow</see> Components.
        /// </summary>
        /// <param name="value">An array of <see cref="StubRow">StubRow</see> Components with which to initialize the collection. </param>
        public StubRowCollection(StubRow[] value)
        {
            if (value != null)
            {
                AddRange(value);
            }
        }

        /// <summary>
        /// Gets the <see cref="StubRowCollection">StubRowCollection</see> at the specified index in the collection.
        /// <para>
        /// In C#, this property is the indexer for the <see cref="StubRowCollection">StubRowCollection</see> class.
        /// </para>
        /// </summary>
        public StubRow this[int index]
        {
            get { return ((StubRow)(List[index])); }
        }

        public int Add(StubRow value)
        {
            if (value != null)
            {
                return List.Add(value);
            }
            return -1;
        }

        /// <summary>
        /// Copies the elements of the specified <see cref="StubRow">StubRow</see> array to the end of the collection.
        /// </summary>
        /// <param name="value">An array of type <see cref="StubRow">StubRow</see> containing the Components to add to the collection.</param>
        public void AddRange(StubRow[] value)
        {
            if (value == null)
                throw new ArgumentNullException("value", "Cannot add a range from null.");

            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                Add(value[i]);
            }
        }

        /// <summary>
        /// Adds the contents of another <see cref="StubRowCollection">StubRowCollection</see> to the end of the collection.
        /// </summary>
        /// <param name="value">A <see cref="StubRowCollection">StubRowCollection</see> containing the Components to add to the collection. </param>
        public void AddRange(StubRowCollection value)
        {
            if (value == null)
                throw new ArgumentNullException("value", "Cannot add a range from null.");

            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                Add((StubRow)value.List[i]);
            }

        }

        /// <summary>
        /// Gets a value indicating whether the collection contains the specified <see cref="StubRowCollection">StubRowCollection</see>.
        /// </summary>
        /// <param name="value">The <see cref="StubRowCollection">StubRowCollection</see> to search for in the collection.</param>
        /// <returns><b>true</b> if the collection contains the specified object; otherwise, <b>false</b>.</returns>
        public bool Contains(StubRow value)
        {
            return List.Contains(value);
        }

        /// <summary>
        /// Copies the collection Components to a one-dimensional <see cref="T:System.Array">Array</see> instance beginning at the specified index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array">Array</see> that is the destination of the values copied from the collection.</param>
        /// <param name="index">The index of the array at which to begin inserting.</param>
        public void CopyTo(StubRow[] array, int index)
        {
            List.CopyTo(array, index);
        }

        /// <summary>
        /// Gets the index in the collection of the specified <see cref="StubRowCollection">StubRowCollection</see>, if it exists in the collection.
        /// </summary>
        /// <param name="value">The <see cref="StubRowCollection">StubRowCollection</see> to locate in the collection.</param>
        /// <returns>The index in the collection of the specified object, if found; otherwise, -1.</returns>
        public int IndexOf(StubRow value)
        {
            return List.IndexOf(value);
        }

        public void Insert(int index, StubRow value)
        {
            List.Insert(index, value);
        }

        public void Remove(StubRow value)
        {
            List.Remove(value);
        }

        public void Sort()
        {
            InnerList.Sort();
        }

        /// <summary>
        /// Returns an enumerator that can iterate through the <see cref="StubRowCollection">StubRowCollection</see> instance.
        /// </summary>
        /// <returns>An <see cref="StubRowCollectionEnumerator">StubRowCollectionEnumerator</see> for the <see cref="StubRowCollection">StubRowCollection</see> instance.</returns>
        public new StubRowCollectionEnumerator GetEnumerator()
        {
            return new StubRowCollectionEnumerator(this);
        }

        /// <summary>
        /// Supports a simple iteration over a <see cref="StubRowCollection">StubRowCollection</see>.
        /// </summary>
        public class StubRowCollectionEnumerator : IEnumerator
        {
            private readonly IEnumerator _enumerator;
            private readonly IEnumerable _temp;

            /// <summary>
            /// Initializes a new instance of the <see cref="StubRowCollectionEnumerator">StubRowCollectionEnumerator</see> class referencing the specified <see cref="StubRowCollection">StubRowCollection</see> object.
            /// </summary>
            /// <param name="mappings">The <see cref="StubRowCollection">StubRowCollection</see> to enumerate.</param>
            public StubRowCollectionEnumerator(IEnumerable mappings)
            {
                _temp = mappings;
                _enumerator = _temp.GetEnumerator();
            }

            /// <summary>
            /// Gets the current element in the collection.
            /// </summary>
            public StubRow Current
            {
                get { return ((StubRow)(_enumerator.Current)); }
            }

            object IEnumerator.Current
            {
                get { return _enumerator.Current; }
            }

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns><b>true</b> if the enumerator was successfully advanced to the next element; <b>false</b> if the enumerator has passed the end of the collection.</returns>
            public bool MoveNext()
            {
                return _enumerator.MoveNext();
            }

            bool IEnumerator.MoveNext()
            {
                return _enumerator.MoveNext();
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            public void Reset()
            {
                _enumerator.Reset();
            }

            void IEnumerator.Reset()
            {
                _enumerator.Reset();
            }
        }
    }
}
