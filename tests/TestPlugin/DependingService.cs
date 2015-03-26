using Athena;

namespace TestPlugin
{
	[Service("Depending Service", "DFAAFE92-9B3F-4717-B54B-DD2D5A53F009")]
	public sealed class DependingService : IService
	{
		[Depend] private IDependencyService _dependency;
		[Depend] private Game _game;
		public IDependencyService Dependency => _dependency;
		public Game Game => _game;

		public void Initialize()
		{
		}

		public void Dispose()
		{
		}
	}
}