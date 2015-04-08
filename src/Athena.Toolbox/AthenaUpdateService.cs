using System;
using System.Diagnostics;
using System.Threading;

namespace Athena.Toolbox
{
	[Service("Athena Update Service", "DE51302A-1D4D-4001-8BB8-1FD404B010D9")]
	public sealed class AthenaUpdateService : IService, IUpdateService
	{
		public void Initialize()
		{
			_game.RegisterRuntime(Runtime);
		}

		public void Dispose()
		{
		}

		public TimeSpan TargetInterval { get; set; } = TimeSpan.FromSeconds(0.016);

		public void Update(TimeSpan elapsed)
		{
			_window.ProcessEvents();

			_world.Update(elapsed);
		}

		private void Runtime(CancellationToken token)
		{
			_logging.Info("Starting Update Runtime...");

			// Unfortunately, windows have to be created from the thread that will be polling for updates
			// This means the game loop service runtime will have to be responsible for opening it
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

		#region Dependencies

		[Depend] private Game _game;
		[Depend] private ILoggingService _logging;
		[Depend] private IWindowService _window;
		[Depend] private IWorldService _world;

		#endregion
	}
}