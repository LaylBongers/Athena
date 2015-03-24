namespace Athena.Launcher
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			using (var game = new Game())
			{
				var loadPluginsInfo = LoadPluginsInfo.FromDirectory("./Plugins/");
				game.LoadPlugins(loadPluginsInfo);

				var loadServicesInfo = LoadServicesInfo.FromFile("./Services.json");
				game.LoadServices(loadServicesInfo);

				game.Run();
			}
		}
	}
}