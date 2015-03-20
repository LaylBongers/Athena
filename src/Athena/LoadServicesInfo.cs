using System;
using System.Collections.ObjectModel;
using System.IO;

namespace Athena
{
	public class LoadServicesInfo
	{
		public Collection<ServiceReference> Services { get; } = new Collection<ServiceReference>();

		public static LoadServicesInfo FromFile(string file)
		{
			return FromJson(File.ReadAllText(file));
		}

		public static LoadServicesInfo FromJson(string json)
		{
			throw new NotImplementedException();
		}
	}
}