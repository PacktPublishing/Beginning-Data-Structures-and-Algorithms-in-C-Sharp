using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;

/// <summary>
/// Performs common validation.
/// </summary>
internal static class Guard
{
	/// <summary>
	/// Throws an exception if the assertion is true.
	/// </summary>
	/// <param name="assertion">if set to <c>true</c> throw.</param>
	/// <param name="message">The message.</param>
	[DebuggerStepThrough]
	internal static void Against(bool assertion, string message)
	{
		if (assertion)
		{
			Throw.GuardAgainstFailure(message);
		}
	}

	/// <summary>
	/// Throws an exception if the assertion is true.
	/// </summary>
	/// <param name="assertion">if set to <c>true</c> throw.</param>
	/// <param name="message">The message.</param>
	[DebuggerStepThrough]
	internal static void Against(bool assertion, Func<string> message)
	{
		if (assertion)
		{
			Throw.GuardAgainstFailure(message());
		}
	}

	/// <summary>
	/// Throws an exception if the assertion is false.
	/// </summary>
	/// <param name="assertion">if set to <c>false</c> throw.</param>
	/// <param name="message">The message.</param>
	[DebuggerStepThrough]
	internal static void Assert(bool assertion, string message)
	{
		if (!assertion)
		{
			Throw.GuardAssertFailure(message);
		}
	}

	/// <summary>
	/// Throws an exception if the assertion is false.
	/// </summary>
	/// <param name="assertion">if set to <c>false</c> throw.</param>
	/// <param name="message">The message.</param>
	[DebuggerStepThrough]
	internal static void Assert(bool assertion, Func<string> message)
	{
		if (!assertion)
		{
			Throw.GuardAssertFailure(message());
		}
	}

	/// <summary>
	/// Determines whether a type can be assigned from another specified type.
	/// </summary>
	/// <param name="typeToAssign">The type to be assigned.</param>
	/// <param name="targetType">Type of the target.</param>
	/// <param name="name">Name of the argument being checked.</param>
	[DebuggerStepThrough]
	internal static void CanBeAssigned(Type typeToAssign, Type targetType, string name = "Type")
	{
		if (targetType.IsAssignableFrom(typeToAssign))
		{
			return;
		}
		Throw.GuardTypeAssignmentFailure(typeToAssign, targetType, name);
	}

	/// <summary>
	/// Determines whether an object can be assigned to the specified type.
	/// </summary>
	/// <typeparam name="TTargetType">The type to assign to.</typeparam>
	/// <param name="value">The value.</param>
	[DebuggerStepThrough]
	internal static void CanBeAssignedTo<TTargetType>(object value)
	{
		CanBeAssigned(value.GetType(), typeof(TTargetType));
	}

	/// <summary>
	/// Throws an exception if the specified file does not exist.
	/// </summary>
	/// <param name="filename">The filename.</param>
	[DebuggerStepThrough]
	internal static void FileExists(string filename)
	{
		if (!File.Exists(filename))
		{
			Throw.FileNotFoundException(filename);
		}
	}

	/// <summary>
	/// Throws an exception if the value is less than or equal to the minimum.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="minimum">The minimum.</param>
	/// <param name="value">The value.</param>
	/// <param name="reference">The reference.</param>
	[DebuggerStepThrough]
	internal static void GreaterThan<T>(T minimum, T value, Expression<Func<T>> reference) where T : IComparable<T>
	{
		if (value.CompareTo(minimum) > 0)
		{
			return;
		}
		Throw.GuardFailureNotGreaterThan(GetName(reference), value, minimum);
	}

	/// <summary>
	/// Throws an exception if the value is less than or equal to the minimum.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="minimum">The minimum.</param>
	/// <param name="value">The value.</param>
	/// <param name="name">The name.</param>
	[DebuggerStepThrough]
	internal static void GreaterThan<T>(T minimum, T value, string name) where T : IComparable<T>
	{
		if (value.CompareTo(minimum) > 0)
		{
			return;
		}
		Throw.GuardFailureNotGreaterThan(name, value, minimum);
	}

	/// <summary>
	/// Throws an exception if the value is less than the minimum.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="minimum">The minimum.</param>
	/// <param name="value">The value.</param>
	/// <param name="reference">The reference.</param>
	[DebuggerStepThrough]
	internal static void GreaterThanOrEqualTo<T>(T minimum, T value, Expression<Func<T>> reference) where T : IComparable<T>
	{
		if (value.CompareTo(minimum) >= 0)
		{
			return;
		}
		Throw.GuardFailureNotGreaterThanOrEqualTo(GetName(reference), value, minimum);
	}

	/// <summary>
	/// Throws an exception if the value is less than the minimum.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="minimum">The minimum.</param>
	/// <param name="value">The value.</param>
	/// <param name="name">The name.</param>
	[DebuggerStepThrough]
	internal static void GreaterThanOrEqualTo<T>(T minimum, T value, string name) where T : IComparable<T>
	{
		if (value.CompareTo(minimum) >= 0)
		{
			return;
		}
		Throw.GuardFailureNotGreaterThanOrEqualTo(name, value, minimum);
	}

	/// <summary>
	/// Throws an exception if the value is greater than or equal to the maximum.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="maximum">The maximum.</param>
	/// <param name="value">The value.</param>
	/// <param name="reference">The reference.</param>
	[DebuggerStepThrough]
	internal static void LessThan<T>(T maximum, T value, Expression<Func<T>> reference) where T : IComparable<T>
	{
		if (value.CompareTo(maximum) < 0)
		{
			return;
		}
		Throw.GuardFailureNotLessThan(GetName(reference), value, maximum);
	}

	/// <summary>
	/// Throws an exception if the value is greater than or equal to the maximum.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="maximum">The maximum.</param>
	/// <param name="value">The value.</param>
	/// <param name="name">The name.</param>
	[DebuggerStepThrough]
	internal static void LessThan<T>(T maximum, T value, string name) where T : IComparable<T>
	{
		if (value.CompareTo(maximum) < 0)
		{
			return;
		}
		Throw.GuardFailureNotLessThan(name, value, maximum);
	}

	/// <summary>
	/// Throws an exception if the value is greater than the maximum.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="maximum">The maximum.</param>
	/// <param name="value">The value.</param>
	/// <param name="reference">The reference.</param>
	[DebuggerStepThrough]
	internal static void LessThanOrEqualTo<T>(T maximum, T value, Expression<Func<T>> reference) where T : IComparable<T>
	{
		if (value.CompareTo(maximum) <= 0)
		{
			return;
		}
		Throw.GuardFailureNotLessThanOrEqualTo(GetName(reference), value, maximum);
	}

	/// <summary>
	/// Throws an exception if the value is greater than the maximum.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="maximum">The maximum.</param>
	/// <param name="value">The value.</param>
	/// <param name="name">The name.</param>
	[DebuggerStepThrough]
	internal static void LessThanOrEqualTo<T>(T maximum, T value, string name) where T : IComparable<T>
	{
		if (value.CompareTo(maximum) <= 0)
		{
			return;
		}
		Throw.GuardFailureNotLessThanOrEqualTo(name, value, maximum);
	}

	/// <summary>
	/// Throws an exception if the value is null.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="value">The value.</param>
	/// <param name="reference">The reference.</param>
	[DebuggerStepThrough]
	internal static void NotNull<T>(T value, Expression<Func<T>> reference)
	{
		if (!IsNullableType(typeof(T)))
		{
			return;
		}
		if (value == null)
		{
			Throw.ArgumentNullException(GetName(reference));
		}
	}

	/// <summary>
	/// Throws an exception if the value is null.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="value">The value.</param>
	/// <param name="name">The name.</param>
	[DebuggerStepThrough]
	internal static void NotNull<T>(T value, string name)
	{
		if (!IsNullableType(typeof(T)))
		{
			return;
		}
		if (value == null)
		{
			Throw.ArgumentNullException(name);
		}
	}

	/// <summary>
	/// Throws an exception if the value is null or empty.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="value">The value.</param>
	/// <param name="reference">The reference.</param>
	[DebuggerStepThrough]
	internal static void NotNullOrEmpty<T>(IEnumerable<T> value, Expression<Func<IEnumerable<T>>> reference)
	{
		NotNull(value, reference);
		using (IEnumerator<T> enumerator = value.GetEnumerator())
		{
			if (!enumerator.MoveNext())
			{
				Throw.ArgumentEmptyException(GetName(reference));
			}
		}
	}

	/// <summary>
	/// Throws an exception if the value is null or empty.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="value">The value.</param>
	/// <param name="name">The name.</param>
	[DebuggerStepThrough]
	internal static void NotNullOrEmpty<T>(IEnumerable<T> value, string name)
	{
		NotNull(value, name);
		using (IEnumerator<T> enumerator = value.GetEnumerator())
		{
			if (!enumerator.MoveNext())
			{
				Throw.ArgumentEmptyException(name);
			}
		}
	}

	/// <summary>
	/// Throws an exception if the value is null or empty.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <param name="reference">The reference.</param>
	[DebuggerStepThrough]
	internal static void NotNullOrEmpty(string value, Expression<Func<string>> reference)
	{
		NotNull(value, reference);
		if (value.Length == 0)
		{
			Throw.ArgumentEmptyException(GetName(reference));
		}
	}

	/// <summary>
	/// Throws an exception if the value is null or empty.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <param name="name">The name.</param>
	[DebuggerStepThrough]
	internal static void NotNullOrEmpty(string value, string name)
	{
		NotNull(value, name);
		if (value.Length == 0)
		{
			Throw.ArgumentEmptyException(name);
		}
	}

	private static string GetName(Expression reference)
	{
		LambdaExpression lambda = reference as LambdaExpression;
		MemberExpression member = lambda.Body as MemberExpression;
		return member.Member.Name;
	}

	private static bool IsNullableType(Type type)
	{
		if (!type.IsValueType) // ref-type
		{
			return true;
		}
		return Nullable.GetUnderlyingType(type) != null;
	}
}