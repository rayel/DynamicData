using System;
using System.Collections.Generic;

namespace DynamicData.Operators
{
    /// <summary>
    /// Represents a new page request
    /// </summary>
    public sealed class PageRequest : IPageRequest, IEquatable<IPageRequest>
    {
        private readonly int _page=1;
        private readonly int _size=25;

        /// <summary>
        /// The default page request
        /// </summary>
        public readonly static IPageRequest Default = new PageRequest();
        /// <summary>
        /// Represents an empty page
        /// </summary>
        public readonly static IPageRequest Empty = new PageRequest(0, 0);

        /// <summary>
        /// Initializes a new instance of the <see cref="PageRequest"/> class.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="size">The size.</param>
        public PageRequest(int page, int size)
        {
            if (page < 0) throw new ArgumentException("Page must be positive");
            if (size < 0) throw new ArgumentException("Size must be positive");
            _page = page;
            _size = size;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public PageRequest()
        {
        }


        /// <summary>
        /// The page to move to
        /// </summary>
        public int Page
        {
            get { return _page; }
        }

        /// <summary>
        /// The page size
        /// </summary>
        public int Size
        {
            get { return _size; }
        }

        #region Equality members

        public bool Equals(IPageRequest other)
        {
            return DefaultComparer.Equals(this, other);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. </param>
        public override bool Equals(object obj)
        {
            if (!(obj is IPageRequest))
                return false;
            return Equals((IPageRequest) obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (_size*397) ^ _page;
            }
        }

        #endregion

        #region PageSizeEqualityComparer

        private sealed class PageSizeEqualityComparer : IEqualityComparer<IPageRequest>
        {
            public bool Equals(IPageRequest x, IPageRequest y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Page == y.Page && x.Size == y.Size;
            }

            public int GetHashCode(IPageRequest obj)
            {
                unchecked
                {
                    return (obj.Page*397) ^ obj.Size;
                }
            }
        }

        private static readonly IEqualityComparer<IPageRequest> PageSizeComparerInstance = new PageSizeEqualityComparer();

        /// <summary>
        /// Gets the default comparer.
        /// </summary>
        /// <value>
        /// The default comparer.
        /// </value>
        public IEqualityComparer<IPageRequest> DefaultComparer
        {
            get { return PageSizeComparerInstance; }
        }

        #endregion

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Page: {0}, Size: {1}", _page, _size);
        }
    }
}