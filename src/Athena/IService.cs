using System;

namespace Athena
{
	public interface IService : IDisposable
	{
		void Initialize();
	}
}