using System;
using System.Threading;

namespace Athena.Toolbox
{
	[Service("Athena Game Loop Service", "DE51302A-1D4D-4001-8BB8-1FD404B010D9")]
	public class AthenaGameLoopService : IService
	{
		[Depend] private Game _game;

		public void Initialize()
		{
			_game.RegisterRuntime(Run);
		}

		public void Cleanup()
		{
		}

		private void Run(CancellationToken token)
		{
			while (!token.IsCancellationRequested)
			{
				Console.WriteLine("Hello from Game Loop!");

				Thread.Sleep(100);
			}
		}
	}
}