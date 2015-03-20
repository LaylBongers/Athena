using System;
using System.Collections.ObjectModel;

namespace Athena
{
	public class Game
	{
		public void LoadPlugins(LoadPluginsInfo info)
		{
			throw new NotImplementedException();
		}

		public void LoadServices(LoadServicesInfo info)
		{
			throw new NotImplementedException();
		}

		public void Run()
		{
			throw new NotImplementedException();
		}

		public Collection<ServiceInfo> AvailableServices { get; } = new Collection<ServiceInfo>();
	}
}