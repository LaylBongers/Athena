using System;
using System.Diagnostics.CodeAnalysis;

namespace Athena
{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class ServiceAttribute : Attribute
	{
		[SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments")]
		public ServiceAttribute(string friendlyName, string guid)
		{
			FriendlyName = friendlyName;
			Guid = new Guid(guid);
		}

		public string FriendlyName { get; }
		public Guid Guid { get; }
	}
}