using Athena;
using Athena.Toolbox;
using Xunit;

namespace Tests.Toolbox
{
	[Trait("Type", "Unit")]
	[Trait("Class", "Athena.Toolbox.Entity")]
	public sealed class EntityTests
	{
		[Fact]
		public void Destroy_DisposesAndRemoves()
		{
			// Arrange
			var parent = new Entity();
			var child = new Entity();
			var grandChild = new Entity();
			var behavior = new TestBehavior();

			child.Behaviors.Add(behavior);
			parent.Children.Add(child);
			child.Children.Add(grandChild);

			// Act
			child.Destroy();

			// Assert
			Assert.DoesNotContain(child, parent.Children);
			Assert.True(behavior.IsDisposed);
		}

		[Fact]
		public void ForceInitialize_EntityComponentAndExternal_InjectsDependencies()
		{
			// Arrange
			var entity = new Entity();
			var behavior = new DependingBehavior();
			var component = new TestComponent();
			var external = new TestExternal();

			entity.Components.Add(component);
			entity.Behaviors.Add(behavior);

			// Act
			entity.ForceInitialize(new object[] {external});

			// Assert
			Assert.Same(entity, behavior.Entity);
			Assert.Same(component, behavior.Component);
			Assert.Same(external, behavior.External);
		}

		private sealed class TestBehavior : IBehavior
		{
			public bool IsDisposed { get; private set; }

			public void Initialize()
			{
			}

			public void Dispose()
			{
				IsDisposed = true;
			}
		}

		private sealed class TestComponent
		{
		}

		private sealed class TestExternal
		{
		}

		private sealed class DependingBehavior : IBehavior
		{
			[Depend] private TestComponent _component;
			[Depend] private Entity _entity;
			[Depend] private TestExternal _external;
			public TestComponent Component => _component;
			public Entity Entity => _entity;
			public TestExternal External => _external;

			public void Initialize()
			{
			}

			public void Dispose()
			{
			}
		}
	}
}