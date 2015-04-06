using System;
using System.Diagnostics.CodeAnalysis;

namespace Athena.Toolbox
{
	[SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments")]
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class EntityBehaviorAttribute : Attribute
	{
		public EntityBehaviorAttribute(string friendlyName, string guid)
		{
			FriendlyName = friendlyName;
			Guid = new Guid(guid);
		}

		public string FriendlyName { get; }
		public Guid Guid { get; }
	}
}