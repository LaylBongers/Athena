using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Athena.Toolbox
{
	public interface IGameLoopService
	{
		TimeSpan TargetInterval { get; set; }
	}
}
