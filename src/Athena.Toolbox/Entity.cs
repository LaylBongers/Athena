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
			Data = new ObservableCollection<object>();

			Children.CollectionChanged += Children_CollectionChanged;
		}

		/// <summary>
		///     Gets or sets the identifier of the entity.
		/// </summary>
		public string Identifier { get; set; }

		/// <summary>
		///     Gets if this entity has been initialized.
		/// </summary>
		public bool Initialized { get; private set; }

		/// <summary>
		///     Gets the parent of this entity, can be set by adding this entity to another entity's children.
		/// </summary>
		public Entity Parent { get; private set; }

		public ObservableCollection<IBehavior> Behaviors { get; }
		public ObservableCollection<object> Data { get; }
		public ObservableCollection<Entity> Children { get; }

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
				.Concat(Data)
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