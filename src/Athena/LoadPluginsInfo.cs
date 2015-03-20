using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Athena
{
	public sealed class LoadPluginsInfo
	{
		public Collection<AssemblyName> PluginAssemblies { get; } = new Collection<AssemblyName>();

		public static LoadPluginsInfo FromDirectory(string directory)
		{
			var info = new LoadPluginsInfo();

			// Verify the directory exists
			var directoryInfo = new DirectoryInfo(directory);
			if (!directoryInfo.Exists)
				throw new InvalidOperationException("Directory \"" + directory + "\" does not exist.");
			
			// Find all files ending in .dll, those are plugins
			var pluginFiles = directoryInfo.GetFiles("*.dll");

			// Create assembly names for those files so they're easier to load in later on
			var assemblyNames = pluginFiles.Select(f => AssemblyName.GetAssemblyName(f.FullName));

			// Add those assembly names to the collection
			foreach (var name in assemblyNames)
			{
				info.PluginAssemblies.Add(name);
			}

			return info;
		}
	}
}