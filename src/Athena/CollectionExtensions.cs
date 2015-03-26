using System;
using System.Collections.Specialized;
using System.Linq;

namespace Athena
{
	public static class CollectionExtensions
	{
		public static void ExecuteForAdded<T>(this NotifyCollectionChangedEventArgs args, Action<T> action)
		{
			Validate.NotNull(args, "args");
			Validate.NotNull(action, "action");

			if (args.NewItems == null)
				return;

			// Set this class as parent for the new added children
			foreach (var item in args.NewItems.Cast<T>())
			{
				action(item);
			}
		}

		public static void ExecuteForRemoved<T>(this NotifyCollectionChangedEventArgs args, Action<T> action)
		{
			Validate.NotNull(args, "args");
			Validate.NotNull(action, "action");

			if (args.OldItems == null)
				return;

			// Unset this class as parent for the old removed children
			foreach (var item in args.OldItems.Cast<T>())
			{
				action(item);
			}
		}
	}
}