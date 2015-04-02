using System;
using System.IO;

namespace Athena.Toolbox
{
	public static class World
	{
		public static Entity LoadJson(string json)
		{
			throw new InvalidOperationException();
		}

		public static Entity LoadFile(string file)
		{
			return LoadJson(File.ReadAllText(file));
		}
	}
}