using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Athena;
using TestPlugin;
using Xunit;

namespace Tests
{
	[Trait("Type", "Unit")]
	[Trait("Class", "Athena.Game")]
	public class GameTests
	{
		[Fact]
		public void LoadPlugins_ValidAssembly_LoadsAvailableServices()
		{
			// Arrange
			var game = new Game();
			var pluginsInfo = new LoadPluginsInfo();
			pluginsInfo.PluginAssemblies.Add(AssemblyName.GetAssemblyName("./TestPlugin.dll"));

			// Act
			game.LoadPlugins(pluginsInfo);

			// Assert
			Assert.Contains(typeof (TestService), game.AvailableServices);
		}

		[Fact]
		public void LoadServices_AvailableService_LoadsService()
		{
			// Arrange
			var game = new Game();
			game.AvailableServices.Add(typeof (TestService));
			var services = GetReferencesFor(typeof (TestService));

			// Act
			game.LoadServices(services);

			// Assert
			Assert.Contains(typeof (TestService), game.Services.Select(s => s.GetType()));
		}

		[Fact]
		public void LoadServices_ServiceWithDependencies_ResolvesDependencies()
		{
			// Arrange
			var game = new Game();
			game.AvailableServices.Add(typeof (DependingService));
			game.AvailableServices.Add(typeof (DependencyService));
			var services = GetReferencesFor(typeof (DependingService), typeof (DependencyService));

			// Act
			game.LoadServices(services);

			// Assert
			var service = game.Services.OfType<DependingService>().First();
			Assert.NotNull(service.Dependency);
			Assert.NotNull(service.Game);
		}

		[Fact]
		public void Run_WithService_StartsAndCleansServices()
		{
			// Arrange
			StartStopService.Started = false;
			StartStopService.Cleaned = false;

			var game = new Game();
			game.AvailableServices.Add(typeof (StartStopService));
			var services = GetReferencesFor(typeof (StartStopService));
			game.LoadServices(services);

			// Act
			game.Run();

			// Assert
			Assert.True(StartStopService.Started);
			Assert.True(StartStopService.Cleaned);
		}

		private static IEnumerable<ServiceReference> GetReferencesFor(params Type[] args)
		{
			return args.Select(a => new ServiceReference
			{
				Guid = a.GetCustomAttribute<ServiceAttribute>().Guid
			});
		}
	}
}