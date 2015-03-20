using System;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Athena
{
	public class LoadPluginsInfo
	{
		public static LoadPluginsInfo FromDirectory(string directory)
		{
			throw new NotImplementedException();
		}

		public Collection<AssemblyName> PluginAssemblies { get; } = new Collection<AssemblyName>();
	}
}