using System;
using System.IO;

namespace Athena
{
	public class LoadServicesInfo
	{
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