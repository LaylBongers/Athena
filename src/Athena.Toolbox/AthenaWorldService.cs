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
		[Depend] private Game _game;
		private EventWaitHandle _waitHandle;
		public Entity Root { get; set; } = new Entity();

		public void Initialize()
		{
			_waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

			// Populate the dependencies so we can inject them on update
			_dependencies.Add(_game);
			_dependencies.AddRange(_game.Services);
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
			Root.ForceInitialize(_dependencies);
			_waitHandle.Set();
		}
	}
}