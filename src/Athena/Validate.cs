using System;
using System.Diagnostics;

namespace Athena
{
	internal static class Validate
	{
		[DebuggerHidden]
		public static void NotNull(object value, string paramName)
		{
			if (value == null)
				throw new ArgumentNullException(paramName);
		}
	}
}