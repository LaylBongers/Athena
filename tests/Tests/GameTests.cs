using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Athena;
using NSubstitute;
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
		public void Run_WithService_InitializesAndCleansServices()
		{
			// Arrange
			InitializeCleanupService.Started = false;
			InitializeCleanupService.Cleaned = false;

			var game = new Game();
			game.AvailableServices.Add(typeof (InitializeCleanupService));
			var services = GetReferencesFor(typeof (InitializeCleanupService));
			game.LoadServices(services);

			// Act
			game.Run();

			// Assert
			Assert.True(InitializeCleanupService.Started);
			Assert.True(InitializeCleanupService.Cleaned);
		}

		[Fact]
		public void Run_WithRuntime_RunsRuntime()
		{
			// Arrange
			var action = Substitute.For<Action<CancellationToken>>();
			var game = new Game();
			game.RegisterRuntime(action);

			// Act
			game.Run();

			// Assert
			action.Received(1).Invoke(Arg.Any<CancellationToken>());
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