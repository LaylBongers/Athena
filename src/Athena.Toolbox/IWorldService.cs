using System;

namespace Athena.Toolbox
{
	public interface IWorldService
	{
		void WaitForUpdate();
		void Update(TimeSpan elapsed);
	}
}