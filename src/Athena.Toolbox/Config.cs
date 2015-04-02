using System.Collections.Generic;

namespace Athena.Toolbox
{
	public class Config
	{
		private readonly Dictionary<string, string> _values;

		public Config(Dictionary<string, string> values)
		{
			_values = values;
		}

		public string GetValue(string key, string defaultValue = null)
		{
			string value;

			if (!_values.TryGetValue(key, out value))
				return defaultValue;

			return value;
		}
	}
}