using System.Collections.Generic;

namespace System.DataStructures
{
	/// <summary>
	/// Compares various functions of two objects
	/// </summary>
	/// <typeparam name="T">The type of function comparer</typeparam>
	internal class FuncComparer<T> : IComparer<T>, IEqualityComparer<T>
	{
		private readonly Func<T, T, int> _compare;

		/// <summary>
		/// Initializes a new instance of the <see cref="FuncComparer&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="comparer">The comparer.</param>
		public FuncComparer(Func<T, T, int> comparer)
		{
			_compare = comparer;
		}

		/// <summary>
		/// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
		/// </summary>
		/// <param name="x">The first object to compare.</param>
		/// <param name="y">The second object to compare.</param>
		/// <returns>
		/// Value Condition Less than zero<paramref name="x"/> is less than <paramref name="y"/>.Zero<paramref name="x"/> equals <paramref name="y"/>.Greater than zero<paramref name="x"/> is greater than <paramref name="y"/>.
		/// </returns>
		public int Compare(T x, T y)
		{
			return _compare(x, y);
		}

		/// <summary>
		/// Determines whether the specified objects are equal.
		/// </summary>
		/// <param name="x">The first object of type T to compare.</param>
		/// <param name="y">The second object of type T to compare.</param>
		/// <returns>
		/// true if the specified objects are equal; otherwise, false.
		/// </returns>
		public bool Equals(T x, T y)
		{
			return (_compare(x, y) == 0);
		}

		/// <summary>
		/// Returns a hash code for the specified object.
		/// </summary>
		/// <param name="obj">The <see cref="T:System.Object"/> for which a hash code is to be returned.</param>
		/// <returns>A hash code for the specified object.</returns>
		/// <exception cref="T:System.ArgumentNullException">The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.</exception>
		public int GetHashCode(T obj)
		{
			return obj.GetHashCode();
		}
	}
}