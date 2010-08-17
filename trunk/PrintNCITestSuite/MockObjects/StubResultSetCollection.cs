using System;
using System.Collections;
using CollectionBase=System.Collections.CollectionBase;

namespace PrintNCITestSuite.MockObjects
{
    /// <summary>
    /// Represents a collection of <see cref="StubResultSet">ResultSet</see>.
    /// </summary>
    [Serializable]
    public class StubResultSetCollection : CollectionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StubResultSetCollection">ResultSetCollection</see> class.
        /// </summary>
        public StubResultSetCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StubResultSetCollection">ResultSetCollection</see> class containing the elements of the specified source collection.
        /// </summary>
        /// <param name="value">A <see cref="StubResultSetCollection">ResultSetCollection</see> with which to initialize the collection.</param>
        public StubResultSetCollection(StubResultSetCollection value)
        {
            if (value != null)
            {
                AddRange(value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StubResultSetCollection">ResultSetCollection</see> class containing the specified array of <see cref="StubResultSet">ResultSet</see> Components.
        /// </summary>
        /// <param name="value">An array of <see cref="StubResultSet">ResultSet</see> Components with which to initialize the collection. </param>
        public StubResultSetCollection(StubResultSet[] value)
        {
            if (value != null)
            {
                AddRange(value);
            }
        }

        /// <summary>
        /// Gets the <see cref="StubResultSetCollection">ResultSetCollection</see> at the specified index in the collection.
        /// <para>
        /// In C#, this property is the indexer for the <see cref="StubResultSetCollection">ResultSetCollection</see> class.
        /// </para>
        /// </summary>
        public StubResultSet this[int index]
        {
            get { return ((StubResultSet)(List[index])); }
        }

        public int Add(StubResultSet value)
        {
            if (value != null)
            {
                return List.Add(value);
            }
            return -1;
        }

        /// <summary>
        /// Copies the elements of the specified <see cref="StubResultSet">ResultSet</see> array to the end of the collection.
        /// </summary>
        /// <param name="value">An array of type <see cref="StubResultSet">ResultSet</see> containing the Components to add to the collection.</param>
        public void AddRange(StubResultSet[] value)
        {
            if (value == null)
                throw new ArgumentNullException("value", "Cannot add a range from null.");

            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                Add(value[i]);
            }

        }

        /// <summary>
        /// Adds the contents of another <see cref="StubResultSetCollection">ResultSetCollection</see> to the end of the collection.
        /// </summary>
        /// <param name="value">A <see cref="StubResultSetCollection">ResultSetCollection</see> containing the Components to add to the collection. </param>
        public void AddRange(StubResultSetCollection value)
        {
            if (value == null)
                throw new ArgumentNullException("value", "Cannot add a range from null.");

            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                Add((StubResultSet)value.List[i]);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains the specified <see cref="StubResultSetCollection">ResultSetCollection</see>.
        /// </summary>
        /// <param name="value">The <see cref="StubResultSetCollection">ResultSetCollection</see> to search for in the collection.</param>
        /// <returns><b>true</b> if the collection contains the specified object; otherwise, <b>false</b>.</returns>
        public bool Contains(StubResultSet value)
        {
            return List.Contains(value);
        }

        /// <summary>
        /// Copies the collection Components to a one-dimensional <see cref="T:System.Array">Array</see> instance beginning at the specified index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array">Array</see> that is the destination of the values copied from the collection.</param>
        /// <param name="index">The index of the array at which to begin inserting.</param>
        public void CopyTo(StubResultSet[] array, int index)
        {
            List.CopyTo(array, index);
        }

        /// <summary>
        /// Gets the index in the collection of the specified <see cref="StubResultSetCollection">ResultSetCollection</see>, if it exists in the collection.
        /// </summary>
        /// <param name="value">The <see cref="StubResultSetCollection">ResultSetCollection</see> to locate in the collection.</param>
        /// <returns>The index in the collection of the specified object, if found; otherwise, -1.</returns>
        public int IndexOf(StubResultSet value)
        {
            return List.IndexOf(value);
        }

        public void Insert(int index, StubResultSet value)
        {
            List.Insert(index, value);
        }

        public void Remove(StubResultSet value)
        {
            List.Remove(value);
        }

        public void Sort()
        {
            InnerList.Sort(new SortListCategoryComparer());
        }

        private sealed class SortListCategoryComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                var first = (StubResultSet)x;
                var second = (StubResultSet)y;
                return first.ToString().CompareTo(second.ToString());
            }
        }


        /// <summary>
        /// Returns an enumerator that can iterate through the <see cref="StubResultSetCollection">ResultSetCollection</see> instance.
        /// </summary>
        /// <returns>An <see cref="ResultSetCollectionEnumerator">ResultSetCollectionEnumerator</see> for the <see cref="StubResultSetCollection">ResultSetCollection</see> instance.</returns>
        public new ResultSetCollectionEnumerator GetEnumerator()
        {
            return new ResultSetCollectionEnumerator(this);
        }

        /// <summary>
        /// Supports a simple iteration over a <see cref="StubResultSetCollection">ResultSetCollection</see>.
        /// </summary>
        public class ResultSetCollectionEnumerator : IEnumerator
        {
            private readonly IEnumerator _enumerator;
            private readonly IEnumerable _temp;

            /// <summary>
            /// Initializes a new instance of the <see cref="ResultSetCollectionEnumerator">ResultSetCollectionEnumerator</see> class referencing the specified <see cref="StubResultSetCollection">ResultSetCollection</see> object.
            /// </summary>
            /// <param name="mappings">The <see cref="StubResultSetCollection">ResultSetCollection</see> to enumerate.</param>
            public ResultSetCollectionEnumerator(IEnumerable mappings)
            {
                _temp = mappings;
                _enumerator = _temp.GetEnumerator();
            }

            /// <summary>
            /// Gets the current element in the collection.
            /// </summary>
            public StubResultSet Current
            {
                get { return ((StubResultSet)(_enumerator.Current)); }
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
