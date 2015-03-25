namespace Athena.Toolbox
{
	public interface ILoggingService
	{
		void Info(string message);
		void Info(string format, params object[] args);
	}
}