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
		public void ForceInitialize_EntityBehaviorAndExternal_InjectsDependencies()
		{
			// Arrange
			var entity = new Entity();
			var behavior = new DependingBehavior();
			var data = new TestData();
			var external = new TestExternal();

			entity.Data.Add(data);
			entity.Behaviors.Add(behavior);

			// Act
			entity.ForceInitialize(new object[] {external});

			// Assert
			Assert.Same(entity, behavior.Entity);
			Assert.Same(data, behavior.Data);
			Assert.Same(external, behavior.External);
		}

		[Fact]
		public void ForceInitialize_EntityWithBehavior_InitializesOnlyOnce()
		{
			// Arrange
			var entity = new Entity();
			var behavior = new InitializingBehavior();
			entity.Behaviors.Add(behavior);

			// Act
			entity.ForceInitialize(new object[0]);
			entity.ForceInitialize(new object[0]);

			// Assert
			Assert.Equal(1, behavior.InitializationCount);
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

		private sealed class TestData
		{
		}

		private sealed class TestExternal
		{
		}

		private sealed class DependingBehavior : IBehavior
		{
			[Depend] private TestData _data;
			[Depend] private Entity _entity;
			[Depend] private TestExternal _external;
			public TestData Data => _data;
			public Entity Entity => _entity;
			public TestExternal External => _external;

			public void Initialize()
			{
			}

			public void Dispose()
			{
			}
		}

		private sealed class InitializingBehavior : IBehavior
		{
			public int InitializationCount { get; set; }

			public void Initialize()
			{
				InitializationCount++;
			}

			public void Dispose()
			{
			}
		}
	}
}