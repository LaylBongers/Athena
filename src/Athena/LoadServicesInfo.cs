using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Athena
{
	public static class LoadServicesInfo
	{
		public static Collection<ServiceReference> FromJson(string json)
		{
			return new Collection<ServiceReference>(JsonConvert.DeserializeObject<List<ServiceReference>>(json));
		}
	}
}