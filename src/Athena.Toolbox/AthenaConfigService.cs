using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Athena.Toolbox
{
	[Service("Athena Config Service", "3EE02552-F00A-428C-85E8-65A7FB42D1AF")]
	[DependConstraint(typeof (IConfigService))]
	public class AthenaConfigService : IService, IConfigService
	{
		private Config _default;

		public Config Default => _default ?? (_default = LoadJson(File.ReadAllText("Config.json")));

		public Config LoadJson(string json)
		{
			return new Config(JsonConvert.DeserializeObject<Dictionary<string, string>>(json));
		}
		
		public void Initialize()
		{
		}

		public void Dispose()
		{
		}
	}
}