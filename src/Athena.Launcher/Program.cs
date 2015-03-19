namespace Athena.Launcher
{
	internal class Program
	{
		// Just a little note for once I get to services
		//[Depend] private ISomeService _service;
		//[Depend] private SomeComponent _component;
		//[DependConstraint<ISomeService>]
		//public class StandardSomeService : ISomeService {}

		private static void Main(string[] args)
		{
			var game = new Game();

			var loadPluginsInfo = LoadPluginsInfo.FromDirectory("./Plugins/");
			game.LoadPlugins(loadPluginsInfo);

			var loadServicesInfo = LoadServicesInfo.FromFile("./Services.json");
			game.LoadServices(loadServicesInfo);

			game.Run();
		}
	}
}