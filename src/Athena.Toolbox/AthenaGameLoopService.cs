using System.Threading;

namespace Athena.Toolbox
{
	[Service("Athena Game Loop Service", "DE51302A-1D4D-4001-8BB8-1FD404B010D9")]
	public class AthenaGameLoopService : IService
	{
		[Depend] private Game _game;
		[Depend] private IWindowService _window;

		public void Initialize()
		{
			_game.RegisterRuntime(RunService);
		}

		public void Cleanup()
		{
		}

		public void Update()
		{
			_window.ProcessEvents();
		}

		public void RunService(CancellationToken token)
		{
			_window.CreateWindow();

			while (!token.IsCancellationRequested)
			{
				Update();

				Thread.Sleep(10);
			}
		}
	}
}