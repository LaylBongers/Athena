using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Athena.Toolbox
{
	[Service("Athena Config Service", "3EE02552-F00A-428C-85E8-65A7FB42D1AF")]
	[DependConstraint(typeof (IConfigService))]
	public class AthenaConfigService : IService, IConfigService
	{
		public Config Default
		{
			get { throw new NotImplementedException(); }
		}

		public void Initialize()
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public Config LoadJson(string json)
		{
			return new Config(JsonConvert.DeserializeObject<Dictionary<string, string>>(json));
		}

		public Config LoadFile(string file)
		{
			return LoadJson(File.ReadAllText(file));
		}

	}
}