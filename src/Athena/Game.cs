using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Athena
{
	public sealed class Game
	{
		private readonly CancellationTokenSource _runtimeCancel = new CancellationTokenSource();
		private readonly List<Action<CancellationToken>> _runtimes = new List<Action<CancellationToken>>();
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
			Validate.NotNull(info, "info");

			foreach (var assemblyName in info.PluginAssemblies)
			{
				// Load in the assembly, .NET automatically prevents duplicate loading
				Trace.WriteLine("Loading plugin \"" + assemblyName + "\"...");
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

		public void LoadServices(IEnumerable<ServiceReference> services)
		{
			Validate.NotNull(services, "services");

			// Add all the services to the list
			foreach (var requestedService in services)
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

			// Now resolve all dependencies for all services
			var dependencies = GetAvailableDependencies();
			foreach (var service in Services)
			{
				var fields = service.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

				// Go through all fields
				foreach (var field in fields)
				{
					// If this field does not have a [Depend] attribute we don't need to resolve it
					if (field.GetCustomAttribute<DependAttribute>() == null)
						continue;

					// Find a service that fits the type of the field
					var dependency = dependencies.FirstOrDefault(s => field.FieldType.IsInstanceOfType(s));

					// If we couldn't find one, leave it null
					if (dependency == null)
						continue;

					// We found one, set the value
					field.SetValue(service, dependency);
				}
			}
		}

		/// <summary>
		///     Registers a new runtime to be started during game execution.
		/// </summary>
		/// <param name="action">The runtime to be started.</param>
		public void RegisterRuntime(Action<CancellationToken> action)
		{
			_runtimes.Add(action);
		}

		public void Run()
		{
			// Initializa all the services
			foreach (var service in Services)
				service.Initialize();

			// Start all the runtimes
			var tasks = new List<Task>();
			foreach (var action in _runtimes)
			{
				var task = Task.Factory.StartNew(
					() => action(_runtimeCancel.Token), _runtimeCancel.Token,
					TaskCreationOptions.LongRunning, TaskScheduler.Default);
				tasks.Add(task);
			}

			// Wait for all the runtimes to finish
			foreach (var task in tasks)
			{
				task.Wait();
			}

			// Clean up all the services
			foreach (var service in Services)
				service.Cleanup();
		}

		/// <summary>
		///		Starts halting game execution. Will send a cancelation to all runtimes.
		/// </summary>
		public void Quit()
		{
			_runtimeCancel.Cancel();
		}

		private List<object> GetAvailableDependencies()
		{
			return Services
				.Concat(new object[] {this})
				.ToList();
		}
	}
}