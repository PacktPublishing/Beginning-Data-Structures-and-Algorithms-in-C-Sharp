using System.Collections.Generic;

namespace System
{
	/// <summary>
	/// extensions for <see cref="IComparable"/>
	/// </summary>
	internal static class IComparableExtensions
	{
		/// <summary>
		/// 	Determines whether the specified value is between the the defined minimum and maximum range (including those values).
		/// </summary>
		/// <typeparam name = "T"></typeparam>
		/// <param name = "value">The value.</param>
		/// <param name = "minValue">The minimum value.</param>
		/// <param name = "maxValue">The maximum value.</param>
		/// <returns>
		/// 	<c>true</c> if the specified value is between min and max; otherwise, <c>false</c>.
		/// </returns>
		/// <example>
		/// 	var value = 5;
		/// 	if(value.IsBetween(1, 10)) { 
		/// 	// ... 
		/// 	}
		/// </example>
		public static bool IsBetween<T>(this T value, T minValue, T maxValue) where T : IComparable<T>
		{
			return IsBetween(value, minValue, maxValue, null);
		}

		/// <summary>
		/// 	Determines whether the specified value is between the the defined minimum and maximum range (including those values).
		/// </summary>
		/// <typeparam name = "T"></typeparam>
		/// <param name = "value">The value.</param>
		/// <param name = "minValue">The minimum value.</param>
		/// <param name = "maxValue">The maximum value.</param>
		/// <param name = "comparer">An optional comparer to be used instead of the types default comparer.</param>
		/// <returns>
		/// 	<c>true</c> if the specified value is between min and max; otherwise, <c>false</c>.
		/// </returns>
		/// <example>
		/// 	var value = 5;
		/// 	if(value.IsBetween(1, 10)) {
		/// 	// ...
		/// 	}
		/// </example>
		public static bool IsBetween<T>(this T value, T minValue, T maxValue, IComparer<T> comparer)
			where T : IComparable<T>
		{
			comparer = comparer ?? Comparer<T>.Default;

			int minMaxCompare = comparer.Compare(minValue, maxValue);
			if (minMaxCompare < 0)
			{
				return ((comparer.Compare(value, minValue) >= 0) && (comparer.Compare(value, maxValue) <= 0));
			}
			if (minMaxCompare == 0)
			{
				return (comparer.Compare(value, minValue) == 0);
			}
			return ((comparer.Compare(value, maxValue) >= 0) && (comparer.Compare(value, minValue) <= 0));
		}

		/// <summary>
		/// Determines whether the left and right are equal using the default comparer.
		/// </summary>
		/// <typeparam name="T">The type of the specified values.</typeparam>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>true if equal, false if not.</returns>
		public static bool IsEqual<T>(this T left, T right) where T : IComparable<T>
		{
			return left.CompareTo(right) == 0;
		}

		/// <summary>
		/// Determines whether the left and right are equal using the specified comparer.
		/// </summary>
		/// <typeparam name="T">The type of the specified values.</typeparam>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <param name="comparer">The comparer.</param>
		/// <returns>true if equal, false if not.</returns>
		public static bool IsEqual<T>(this T left, T right, IComparer<T> comparer) where T : IComparable<T>
		{
			return comparer.Compare(left, right) == 0;
		}

		/// <summary>
		/// Determines whether the specified left is equal.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <param name="comparer">The comparer.</param>
		/// <returns>
		/// 	<c>true</c> if the specified left is equal; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsEqual<T>(this T left, T right, Func<T, T, int> comparer) where T : IComparable<T>
		{
			return comparer(left, right) == 0;
		}

		/// <summary>
		/// Determines whether the left is greater than the right using the default comparer.
		/// </summary>
		/// <typeparam name="T">The type of the specified values.</typeparam>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>true if greater than, false if not.</returns>
		public static bool IsGreaterThan<T>(this T left, T right) where T : IComparable<T>
		{
			return left.CompareTo(right) > 0;
		}

		/// <summary>
		/// Determines whether the left is greater than the right using the specified comparer.
		/// </summary>
		/// <typeparam name="T">The type of the specified values.</typeparam>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <param name="comparer">The comparer.</param>
		/// <returns>true if greater than, false if not.</returns>
		public static bool IsGreaterThan<T>(this T left, T right, IComparer<T> comparer) where T : IComparable<T>
		{
			return comparer.Compare(left, right) > 0;
		}

		/// <summary>
		/// Determines whether [is greater than] [the specified left].
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <param name="comparer">The comparer.</param>
		/// <returns>
		/// 	<c>true</c> if [is greater than] [the specified left]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsGreaterThan<T>(this T left, T right, Func<T, T, int> comparer) where T : IComparable<T>
		{
			return comparer(left, right) > 0;
		}

		/// <summary>
		/// Determines whether the left is greater than or equal to the right using the default comparer.
		/// </summary>
		/// <typeparam name="T">The type of the specified values.</typeparam>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>
		/// true if greater than or equal, false if not.
		/// </returns>
		public static bool IsGreaterThanOrEqual<T>(this T left, T right) where T : IComparable<T>
		{
			return left.CompareTo(right) >= 0;
		}

		/// <summary>
		/// Determines whether the left is greater than or equal to the right using the specified comparer.
		/// </summary>
		/// <typeparam name="T">The type of the specified values.</typeparam>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <param name="comparer">The comparer.</param>
		/// <returns>
		/// true if greater than or equal, false if not.
		/// </returns>
		public static bool IsGreaterThanOrEqual<T>(this T left, T right, IComparer<T> comparer)
			where T : IComparable<T>
		{
			return comparer.Compare(left, right) >= 0;
		}

		/// <summary>
		/// Determines whether [is greater than or equal] [the specified left].
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <param name="comparer">The comparer.</param>
		/// <returns>
		/// 	<c>true</c> if [is greater than or equal] [the specified left]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsGreaterThanOrEqual<T>(this T left, T right, Func<T, T, int> comparer)
			where T : IComparable<T>
		{
			return comparer(left, right) >= 0;
		}

		/// <summary>
		/// Determines whether the left is less than the right using the default comparer.
		/// </summary>
		/// <typeparam name="T">The type of the specified values.</typeparam>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>true if less than, false if not.</returns>
		public static bool IsLessThan<T>(this T left, T right) where T : IComparable<T>
		{
			return left.CompareTo(right) < 0;
		}

		/// <summary>
		/// Determines whether the left is less than the right using the specified comparer.
		/// </summary>
		/// <typeparam name="T">The type of the specified values.</typeparam>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <param name="comparer">The comparer.</param>
		/// <returns>true if less than, false if not.</returns>
		public static bool IsLessThan<T>(this T left, T right, IComparer<T> comparer) where T : IComparable<T>
		{
			return comparer.Compare(left, right) < 0;
		}

		/// <summary>
		/// Determines whether [is less than] [the specified left].
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <param name="comparer">The comparer.</param>
		/// <returns>
		/// 	<c>true</c> if [is less than] [the specified left]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsLessThan<T>(this T left, T right, Func<T, T, int> comparer) where T : IComparable<T>
		{
			return comparer(left, right) < 0;
		}

		/// <summary>
		/// Determines whether the left is less than or equal to the right using the default comparer.
		/// </summary>
		/// <typeparam name="T">The type of the specified values.</typeparam>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>true if less than or equal, false if not.</returns>
		public static bool IsLessThanOrEqual<T>(this T left, T right) where T : IComparable<T>
		{
			return left.CompareTo(right) <= 0;
		}

		/// <summary>
		/// Determines whether the left is less than or equal to the right using the specified comparer.
		/// </summary>
		/// <typeparam name="T">The type of the specified values.</typeparam>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <param name="comparer">The comparer.</param>
		/// <returns>true if less than or equal, false if not.</returns>
		public static bool IsLessThanOrEqual<T>(this T left, T right, IComparer<T> comparer)
			where T : IComparable<T>
		{
			return comparer.Compare(left, right) <= 0;
		}

		/// <summary>
		/// Determines whether [is less than or equal] [the specified left].
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <param name="comparer">The comparer.</param>
		/// <returns>
		/// 	<c>true</c> if [is less than or equal] [the specified left]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsLessThanOrEqual<T>(this T left, T right, Func<T, T, int> comparer)
			where T : IComparable<T>
		{
			return comparer(left, right) <= 0;
		}
	}
}