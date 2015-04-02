using System;
using System.Collections.Generic;
using System.Threading;

namespace Athena.Toolbox
{
	[Service("Athena World Service", "C4B112C1-0391-4A22-9CC2-81795B871060")]
	[DependConstraint(typeof (IWorldService))]
	public sealed class AthenaWorldService : IService, IWorldService
	{
		private readonly List<object> _dependencies = new List<object>();
		[Depend] private IConfigService _config;
		[Depend] private Game _game;
		private EventWaitHandle _waitHandle;
		public Entity Root { get; set; } = new Entity();

		/// <summary>
		///     Gets a lock object that can be used to synchronize access to the world.
		/// </summary>
		public object Lock { get; } = new object();

		public void Initialize()
		{
			_waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

			// Populate the dependencies so we can inject them on update
			_dependencies.Add(_game);
			_dependencies.AddRange(_game.Services);

			var world = _config.Default.GetValue("defaultWorld");
			if (world != null)
				Root = World.LoadFile(world);
		}

		public void Dispose()
		{
			_waitHandle.Dispose();
			Root.Dispose();
		}

		public void WaitForUpdate()
		{
			_waitHandle.WaitOne();
		}

		public void Update(TimeSpan elapsed)
		{
			// When updating the world should be locked
			lock (Lock)
			{
				Root.ForceInitialize(_dependencies);
				_waitHandle.Set();
			}
		}
	}
}