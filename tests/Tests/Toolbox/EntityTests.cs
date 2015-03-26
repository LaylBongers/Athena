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
	}
}