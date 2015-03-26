using Athena;

namespace TestPlugin
{
	[Service("Initialize Cleanup Service", "46044C95-5C89-4918-BB8A-7C14871A3F64")]
	public class InitializeCleanupService : IService
	{
		public static bool Started { get; set; }
		public static bool Disposed { get; set; }

		public void Initialize()
		{
			Started = true;
		}

		public void Dispose()
		{
			Disposed = true;
		}
	}
}