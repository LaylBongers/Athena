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
			var info = new LoadServicesInfo();
			info.Services.Add(new ServiceReference
			{
				Guid = typeof (TestService).GetCustomAttribute<ServiceAttribute>().Guid
			});

			// Act
			game.LoadServices(info);

			// Assert
			Assert.Contains(typeof (TestService), game.Services.Select(s => s.GetType()));
		}
	}
}