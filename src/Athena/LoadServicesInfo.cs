using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Athena
{
	public static class LoadServicesInfo
	{
		public static List<ServiceReference> FromFile(string file)
		{
			return FromJson(File.ReadAllText(file));
		}

		public static List<ServiceReference> FromJson(string json)
		{
			return JsonConvert.DeserializeObject<List<ServiceReference>>(json);
		}
	}
}