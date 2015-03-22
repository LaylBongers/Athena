using Athena;

namespace TestPlugin
{
	[Service("Dependency Service", "9F606563-259C-49D7-A5BC-BBF2BF98070B")]
	[DependConstraint(typeof (IDependencyService))]
	public class DependencyService : IDependencyService
	{
		public void Start()
		{
		}

		public void Cleanup()
		{
		}
	}
}