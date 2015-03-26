using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Athena.Toolbox
{
	public sealed class Entity : IDisposable
	{
		public Entity()
		{
			Behaviors = new ObservableCollection<IBehavior>();
			Children = new ObservableCollection<Entity>();
			Components = new ObservableCollection<object>();

			Children.CollectionChanged += Children_CollectionChanged;
		}

		public bool Initialized { get; private set; }
		public Entity Parent { get; private set; }
		public ObservableCollection<IBehavior> Behaviors { get; }
		public ObservableCollection<Entity> Children { get; }
		public ObservableCollection<object> Components { get; }

		public void Dispose()
		{
			foreach (var behavior in Behaviors)
			{
				behavior.Dispose();
			}
		}

		public void Destroy()
		{
			Parent.Children.Remove(this);
			Dispose();
		}

		/// <summary>
		///     Forcefully initializes all behaviors and child entitites immediately.
		/// </summary>
		public void ForceInitialize(ICollection<object> externalDependencies)
		{
			// Update all children
			foreach (var child in Children)
			{
				child.ForceInitialize(externalDependencies);
			}

			// Prevent double initialization of ourselves
			if (Initialized)
				return;

			var dependencies = externalDependencies
				.Concat(Components)
				.Concat(new[] {this})
				.ToList();

			foreach (var behavior in Behaviors)
			{
				Inject.Into(behavior, dependencies);
				behavior.Initialize();
			}

			Initialized = true;
		}

		private void Children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs evt)
		{
			evt.ExecuteForAdded<Entity>(e => e.Parent = this);
			evt.ExecuteForRemoved<Entity>(e => e.Parent = null);
		}
	}
}