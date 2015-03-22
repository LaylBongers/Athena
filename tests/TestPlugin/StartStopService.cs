using Athena;

namespace TestPlugin
{
	[Service("Start Cleanup Service", "46044C95-5C89-4918-BB8A-7C14871A3F64")]
	public class StartStopService : IService
	{
		public static bool Started { get; set; }
		public static bool Cleaned { get; set; }

		public void Start()
		{
			Started = true;
		}

		public void Cleanup()
		{
			Cleaned = true;
		}
	}
}