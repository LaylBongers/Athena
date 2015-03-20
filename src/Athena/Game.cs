using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Athena
{
	public class Game
	{
		private readonly List<IService> _services = new List<IService>();

		public Game()
		{
			Services = new ReadOnlyCollection<IService>(_services);
		}

		/// <summary>
		///     Gets a collection of services available to be loaded on LoadServices.
		/// </summary>
		public Collection<Type> AvailableServices { get; } = new Collection<Type>();

		/// <summary>
		///     Gets a collection of services that have been loaded by LoadServices.
		/// </summary>
		public ReadOnlyCollection<IService> Services { get; }

		public void LoadPlugins(LoadPluginsInfo info)
		{
			foreach (var assemblyName in info.PluginAssemblies)
			{
				// Load in the assembly, .NET automatically prevents duplicate loading
				var assembly = Assembly.Load(assemblyName);

				// Scan the assembly for exported types that implement IService
				var types = assembly.ExportedTypes.Where(t => typeof (IService).IsAssignableFrom(t));

				// Now that we have the service types, put them into the available list
				foreach (var type in types)
				{
					// Only add if the service has the ServiceAttribute
					if (type.GetCustomAttribute<ServiceAttribute>() == null)
						continue;

					// Actually add the service to the list of available services
					AvailableServices.Add(type);
				}
			}
		}

		public void LoadServices(LoadServicesInfo info)
		{
			foreach (var requestedService in info.Services)
			{
				// Look up the requested service by Guid
				var foundService = AvailableServices
					.FirstOrDefault(f => f.GetCustomAttribute<ServiceAttribute>().Guid == requestedService.Guid);

				// If we didn't find one, that's a problem
				if (foundService == null)
				{
					throw new InvalidOperationException(
						"Unable to load service \"" + requestedService.FriendlyName +
						"\", no matching service Guid found.");
				}

				// We did find one, so find the constructor so we can create it
				var constructor = foundService.GetConstructor(new Type[0]);

				// If we didn't find a constructor, that's a problem
				if (constructor == null)
				{
					throw new InvalidOperationException(
						"Service \"" + requestedService.FriendlyName + "\" does not have an empty constructor.");
				}

				// Finally, construct and add
				var service = (IService) constructor.Invoke(new object[0]);
				_services.Add(service);
			}
		}

		public void Run()
		{
			throw new NotImplementedException();
		}
	}
}