using System;
using System.Collections.Generic;
using System.Linq;
using Athena;
using Athena.Toolbox;
using Newtonsoft.Json.Linq;
using NSubstitute;
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
			var service = new AthenaWorldService();

			var config = Substitute.For<IConfigService>();
			config.Default.Returns(new Config(new Dictionary<string, string>()));
			Inject.Into(service, new object[] {new Game(), config});

			service.Initialize();
			var behavior = new InitializableBehavior();
			service.Root.Children.Add(
				new Entity
				{
					Behaviors = {behavior}
				});

			// Act
			service.Update(TimeSpan.FromSeconds(0.016));

			// Assert
			Assert.True(behavior.IsInitialized);
		}

		[Fact]
		public void LoadJson_WithRootOnly_LoadsDataAndBehaviors()
		{
			// Arrange
			var service = new AthenaWorldService();
			service.RegisterDataType(typeof (TestData));
			service.RegisterBehaviorType(typeof (TestBehavior));

			var json = new JObject
			{
				["identifier"] = "Test Identifier",
				["data"] = new JArray
				{
					new JObject
					{
						["name"] = "Test Data",
						["typeGuid"] = "B04A03AE-64D9-486A-8DFA-76F14B2A45D5",
						["properties"] = new JObject
						{
							["TestProperty"] = "Some Value",
							["TestIntProperty"] = "5"
						}
					}
				},
				["behaviors"] = new JArray
				{
					new JObject
					{
						["name"] = "Test Behavior",
						["typeGuid"] = "E71E8DDA-CDC7-4C9F-93FB-D7F1C10975C9"
					}
				}
			};

			// Act
			var world = service.LoadWorld(json.ToString());

			// Assert
			Assert.Equal("Test Identifier", world.Identifier);

			var data = world.Data.OfType<TestData>().FirstOrDefault();
			Assert.NotNull(data);
			Assert.Equal("Some Value", data.TestProperty);
			Assert.Equal(5, data.TestIntProperty);

			Assert.True(world.Behaviors.OfType<TestBehavior>().Any());
		}

		[Fact]
		public void LoadJson_WithChild_LoadsChild()
		{
			// Arrange
			var service = new AthenaWorldService();
			var json = new JObject
			{
				["children"] = new JArray
				{
					new JObject()
				}
			};

			// Act
			var world = service.LoadWorld(json.ToString());

			// Assert
			Assert.True(world.Children.Any());
		}

		private class InitializableBehavior : IBehavior
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

		[EntityData("Test Data", "B04A03AE-64D9-486A-8DFA-76F14B2A45D5")]
		private class TestData
		{
			public string TestProperty { get; set; }
			public int TestIntProperty { get; set; }
		}

		[EntityBehavior("Test Behavior", "E71E8DDA-CDC7-4C9F-93FB-D7F1C10975C9")]
		private class TestBehavior : IBehavior
		{
			public void Initialize()
			{
			}

			public void Dispose()
			{
			}
		}
	}
}