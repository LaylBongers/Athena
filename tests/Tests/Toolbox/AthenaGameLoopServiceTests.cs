﻿using System;
using Athena;
using Athena.Toolbox;
using NSubstitute;
using Xunit;

namespace Tests.Toolbox
{
	[Trait("Type", "Unit")]
	[Trait("Class", "Athena.Toolbox.AthenaGameLoopService")]
	public sealed class AthenaGameLoopServiceTests
	{
		[Fact]
		public void Update_RunsRequiredCalls()
		{
			// Arrange
			var loop = new AthenaGameLoopService();
			var window = Substitute.For<IWindowService>();
			var world = Substitute.For<IWorldService>();
            Inject.Into(loop, new object[] {window, world, Substitute.For<ILoggingService>()});

			// Act
			loop.Update(TimeSpan.FromSeconds(0.016));

			// Assert
			window.Received(1).ProcessEvents();
			world.Received(1).Update(Arg.Any<TimeSpan>());
		}
	}
}