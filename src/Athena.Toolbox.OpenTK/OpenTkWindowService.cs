using System;
using System.Drawing;
using System.Threading;
using OpenTK;
using OpenTK.Graphics;

namespace Athena.Toolbox.OpenTK
{
	[Service("OpenTK Window Service", "E80A2ADB-D094-4E1B-8B2C-82CFA9BAEE47")]
	[DependConstraint(typeof (IWindowService))]
	public class OpenTkWindowService : IService, IWindowService
	{
		[Depend] private Game _game;
		private int _threadId;
		private GameWindow _window;

		public void Initialize()
		{
		}

		public void Cleanup()
		{
			_window?.Dispose();
			_window = null;
		}

		public void CreateWindow()
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
					"Athena",
					GameWindowFlags.FixedWindow);

				_window.Visible = true;
				_window.VSync = VSyncMode.Adaptive;
				_window.Closing += _window_Closing;
				//_window.Resize += _window_ResizeMoveFocus;
				//_window.Move += _window_ResizeMoveFocus;
				//_window.FocusedChanged += _window_ResizeMoveFocus;

				_threadId = Thread.CurrentThread.ManagedThreadId;
			}
			catch
			{
				_window?.Dispose();
				throw;
			}
		}

		private void _window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			_game.Quit();
			_window.Dispose();
			_window = null;
		}

		public void ProcessEvents()
		{
			if (_threadId != Thread.CurrentThread.ManagedThreadId)
				throw new InvalidOperationException("ProcessEvents must be called on the same thread that created the window.");

			_window?.ProcessEvents();
		}

		public void SwapBuffers()
		{
			_window?.SwapBuffers();
		}
	}
}