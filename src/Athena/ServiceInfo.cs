using System;

namespace Athena
{
	public class ServiceInfo
	{
		public ServiceInfo(Type type)
		{
			ServiceType = type;
		}

		public Type ServiceType { get; }
	}
}