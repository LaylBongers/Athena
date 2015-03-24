using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Athena
{
	public static class Inject
	{
		public static void Into(object target, ICollection<object> dependencies)
		{
			Validate.NotNull(target, "target");
			Validate.NotNull(dependencies, "dependencies");

			var fields = target.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

			// Go through all fields
			foreach (var field in fields)
			{
				// If this field does not have a [Depend] attribute we don't need to resolve it
				if (field.GetCustomAttribute<DependAttribute>() == null)
					continue;

				// Find a service that fits the type of the field
				var dependency = dependencies.FirstOrDefault(s => field.FieldType.IsInstanceOfType(s));

				// If we couldn't find one, leave it null
				if (dependency == null)
					continue;

				// We found one, set the value
				field.SetValue(target, dependency);
			}
		}
	}
}