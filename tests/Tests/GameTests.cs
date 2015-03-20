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
			Assert.Contains(typeof(TestService), game.AvailableServices.Select(s => s.ServiceType));
		}
	}
}