using System;
using Athena.Toolbox;
using Xunit;

namespace Tests.Toolbox
{
	[Trait("Type", "Unit")]
	[Trait("Class", "Athena.Toolbox.AthenaWorldService")]
	public sealed class AthenaWorldServiceTests
	{
		[Fact]
		public void Update_WithSingleNewEntity_InitializesBehaviors()
		{
			// Arrange
			var world = new AthenaWorldService();
			world.Initialize();
			var behavior = new TestBehavior();
			world.Root.Children.Add(
				new Entity
				{
					Behaviors = {behavior}
				});

			// Act
			world.Update(TimeSpan.FromSeconds(0.016));

			// Assert
			Assert.True(behavior.IsInitialized);
		}
		
		private class TestBehavior : IBehavior
		{
			public bool IsInitialized { get; private set; }

			public void Initialize()
			{
				IsInitialized = true;
			}

			public void Dispose()
			{
			}
		}
	}
}