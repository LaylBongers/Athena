namespace Athena.Toolbox
{
	public interface IWindowService
	{
		void CreateWindow();
		void ProcessEvents();
		void SwapBuffers();
		void MakeCurrent();
	}
}