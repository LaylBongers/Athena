using System;
using System.Diagnostics;
using System.Threading;

namespace Athena.Toolbox
{
	[Service("Athena Game Loop Service", "DE51302A-1D4D-4001-8BB8-1FD404B010D9")]
	public sealed class AthenaGameLoopService : IService, IGameLoopService
	{
		[Depend] private Game _game;
		[Depend] private ILoggingService _logging;
		[Depend] private IWindowService _window;
		[Depend] private IWorldService _world;
		public TimeSpan TargetInterval { get; set; } = TimeSpan.FromSeconds(0.016);

		public void Initialize()
		{
			_game.RegisterRuntime(Runtime);
		}

		public void Dispose()
		{
		}

		public void Update(TimeSpan elapsed)
		{
			_window.ProcessEvents();
			_world.TempSignalUpdated();
		}

		public void Runtime(CancellationToken token)
		{
			_logging.Info("Starting Game Loop Runtime...");
			_window.CreateWindow();

			var stopwatch = new Stopwatch();
			var accumulator = new TimeSpan();
			while (!token.IsCancellationRequested)
			{
				accumulator += stopwatch.Elapsed;
				stopwatch.Restart();

				// Limit the accumulator to avoid sudden fast spikes after freezes
				var accumulationLimit = new TimeSpan(TargetInterval.Ticks*4);
				if (accumulator > accumulationLimit)
				{
					accumulator = accumulationLimit;
				}

				// Loop until we've run enough ticks to reduce the accumulator to under the target
				for (; accumulator > TargetInterval; accumulator -= TargetInterval)
				{
					Update(TargetInterval);
				}

				Thread.Sleep(1);
			}
		}
	}
}