using System.Diagnostics;
using System.Globalization;

namespace Athena.Toolbox
{
	[Service("Athena Logging Service", "C12E0D36-B9A1-4EA4-8FC9-D37217A9D2D1")]
	[DependConstraint(typeof (ILoggingService))]
	public sealed class AthenaLoggingService : IService, ILoggingService
	{
		public void Initialize()
		{
		}

		public void Dispose()
		{
		}
		
		public void Info(string message)
		{
			Trace.WriteLine(message);
		}

		public void Info(string format, params object[] args)
		{
			Info(string.Format(CultureInfo.InvariantCulture, format, args));
		}
	}
}