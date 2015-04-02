using System;

namespace Athena.Toolbox
{
	public interface IUpdateService
	{
		TimeSpan TargetInterval { get; set; }
	}
}