using System.Threading;

namespace Athena.Toolbox
{
	[Service("Athena World Service", "C4B112C1-0391-4A22-9CC2-81795B871060")]
	public class AthenaWorldService : IService, IWorldService
	{
		private EventWaitHandle _waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

		public void Initialize()
		{
		}

		public void Cleanup()
		{
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