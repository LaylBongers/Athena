namespace Athena.Toolbox
{
	public interface IWorldService
	{
		void WaitForUpdate();
		void TempSignalUpdated();
	}
}