using System;

namespace Athena.Toolbox
{
	public interface IBehavior : IDisposable
	{
		void Initialize();
	}
}