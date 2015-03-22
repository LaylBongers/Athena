using System;

namespace Athena
{
	/// <summary>
	///     Constrains dependency resolving to only happen when a certain type is requested.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class DependConstraintAttribute : Attribute
	{
		public DependConstraintAttribute(Type constraintType)
		{
			ConstraintType = constraintType;
		}

		public Type ConstraintType { get; }
	}
}