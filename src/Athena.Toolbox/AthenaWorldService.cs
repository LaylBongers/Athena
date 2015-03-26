using System.Threading;

namespace Athena.Toolbox
{
	[Service("Athena World Service", "C4B112C1-0391-4A22-9CC2-81795B871060")]
	[DependConstraint(typeof (IWorldService))]
	public sealed class AthenaWorldService : IService, IWorldService
	{
		private EventWaitHandle _waitHandle;

		public void Initialize()
		{
			_waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
        }

		public void Dispose()
		{
			_waitHandle.Dispose();
		}

		public void WaitForUpdate()
		{
			_waitHandle.WaitOne();
		}

		public void TempSignalUpdated()
		{
			_waitHandle.Set();
		}
	}
}