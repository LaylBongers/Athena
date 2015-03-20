using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Athena
{
	public class Game
	{
		public void LoadPlugins(LoadPluginsInfo info)
		{
			foreach (var assemblyName in info.PluginAssemblies)
			{
				// Load in the assembly, .NET automatically prevents duplicate loading
				var assembly = Assembly.Load(assemblyName);
				
				// Scan the assembly for exported types that implement IService
				var types = assembly.ExportedTypes.Where(t => typeof(IService).IsAssignableFrom(t));

				// Now that we have the service types, put them into the available list
				foreach (var type in types)
				{
					AvailableServices.Add(new ServiceInfo(type));
				}
			}
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