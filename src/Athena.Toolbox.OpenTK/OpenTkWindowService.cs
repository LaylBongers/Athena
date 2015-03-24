using System.Drawing;
using OpenTK;
using OpenTK.Graphics;

namespace Athena.Toolbox.OpenTK
{
	[Service("OpenTK Window Service", "E80A2ADB-D094-4E1B-8B2C-82CFA9BAEE47")]
	public class OpenTkWindowService : IService
	{
		private GameWindow _window;

		public void Initialize()
		{
			try
			{
				var size = new Size(1280, 720);

				// Do not use initialization list, it might throw an exception in the list and then it's not yet set
				_window = new GameWindow(
					size.Width, size.Height,
					// Deferred rendering so no samples.
					// If you want AA, it has to be post-process.
					new GraphicsMode(32, 16, 0, 0),
					"Subterran",
					GameWindowFlags.FixedWindow);

				_window.Visible = true;
				_window.VSync = VSyncMode.Adaptive;
				//_window.Closing += _window_Closing;
				//_window.Resize += _window_ResizeMoveFocus;
				//_window.Move += _window_ResizeMoveFocus;
				//_window.FocusedChanged += _window_ResizeMoveFocus;
			}
			catch
			{
				_window?.Dispose();
				throw;
			}
		}

		public void Cleanup()
		{
			_window.Dispose();
			_window = null;
		}
	}
}