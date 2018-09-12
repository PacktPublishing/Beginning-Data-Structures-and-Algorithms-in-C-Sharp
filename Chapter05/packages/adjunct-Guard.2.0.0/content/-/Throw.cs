using System;
using System.Diagnostics;
using System.IO;

internal static class Throw
{
	[DebuggerStepThrough]
	internal static void ArgumentEmptyException(string paramName)
	{
		throw new ArgumentException(string.Format("{0} cannot be empty.", paramName), paramName);
	}

	[DebuggerStepThrough]
	internal static void ArgumentNullException(string paramName)
	{
		throw new ArgumentNullException(paramName, string.Format("{0} cannot be null.", paramName));
	}

	[DebuggerStepThrough]
	internal static void Exception(string message)
	{
		throw new Exception(message);
	}

	[DebuggerStepThrough]
	internal static void FileNotFoundException(string filename)
	{
		string message = string.Format("File [{0}] was not found.", filename);
#if !CompactFramework && !SILVERLIGHT
		throw new FileNotFoundException(message, filename);
#else
		throw new FileNotFoundException(message);
#endif
	}

	[DebuggerStepThrough]
	internal static void GuardAgainstFailure(string message)
	{
		throw new Exception(string.Format("Guard Against: {0}", message));
	}

	[DebuggerStepThrough]
	internal static void GuardAssertFailure(string message)
	{
		throw new Exception(string.Format("Guard Assert: {0}", message));
	}

	[DebuggerStepThrough]
	internal static void GuardFailureNotGreaterThan<T>(string name, T value, T minimum)
	{
		string message = string.Format("Guard Greater: {0} is '{1}' but must be greater than '{2}'.", name, value, minimum);
		throw new Exception(message);
	}

	[DebuggerStepThrough]
	internal static void GuardFailureNotGreaterThanOrEqualTo<T>(string name, T value, T minimum)
	{
		string message = string.Format("Guard Greater or Equal: {0} is '{1}' but must be greater than or equal to '{2}'.",
		                               name,
		                               value,
		                               minimum);
		throw new Exception(message);
	}

	[DebuggerStepThrough]
	internal static void GuardFailureNotLessThan<T>(string name, T value, T maximum)
	{
		string message = string.Format("Guard Less: {0} is '{1}' but must be less than '{2}'.", name, value, maximum);
		throw new Exception(message);
	}

	[DebuggerStepThrough]
	internal static void GuardFailureNotLessThanOrEqualTo<T>(string name, T value, T maximum)
	{
		string message = string.Format("Guard Less or Equal: {0} is '{1}' but must be less than or equal to '{2}'.",
		                               name,
		                               value,
		                               maximum);
		throw new Exception(message);
	}

	[DebuggerStepThrough]
	internal static void GuardTypeAssignmentFailure(Type typeToAssign, Type targetType, string name)
	{
		string message = string.Format("Guard Type Assignment: {0} is '{1}' but does not {2} '{3}'.",
		                               name,
		                               typeToAssign,
		                               targetType.IsInterface ? "implement required interface" : "convert to required type",
		                               targetType);
		throw new Exception(message);
	}
}