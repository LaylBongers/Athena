using System;

namespace Athena
{
	public sealed class ServiceAttribute : Attribute
	{
		public ServiceAttribute(string friendlyName, string guid)
		{
			FriendlyName = friendlyName;
			Guid = new Guid(guid);
		}

		public string FriendlyName { get; }
		public Guid Guid { get; }
	}
}