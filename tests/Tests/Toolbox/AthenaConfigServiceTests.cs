using Athena.Toolbox;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Tests.Toolbox
{
	[Trait("Type", "Unit")]
	[Trait("Class", "Athena.Toolbox.AthenaConfigService")]
	public class AthenaConfigServiceTests
	{
		[Fact]
		public void LoadConfig_WithValidJson_LoadsValues()
		{
			// Arrange
			const string expected1 = "some Value";
			const string expected2 = "12345";
			var json = new JObject
			{
				{"Value1", expected1},
				{"Value2", expected2}
			};
			var service = new AthenaConfigService();

			// Act
			var result = service.LoadJson(json.ToString());

			// Assert
			Assert.Equal(expected1, result.GetValue("Value1"));
			Assert.Equal(expected2, result.GetValue("Value2"));
		}
	}
}