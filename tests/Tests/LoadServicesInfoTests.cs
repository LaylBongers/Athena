using System;
using Athena;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Tests
{
	[Trait("Type", "Unit")]
	[Trait("Class", "Athena.LoadServicesInfo")]
	public class LoadServicesInfoTests
	{
		[Fact]
		public void FromJson_ValidJson_Deserializes()
		{
			// Arrange
			const string expectedName = "My Service";
			var expectedGuid = new Guid("4E31F625-3C2D-4B6E-B3ED-740193AE40B1");
			var json = new JArray
			{
				new JObject
				{
					["name"] = expectedName,
					["guid"] = expectedGuid
				}
			}.ToString();

			// Act
			var result = LoadServicesInfo.FromJson(json);

			// Assert
			Assert.Equal(1, result.Count);
			Assert.Equal(expectedName, result[0].FriendlyName);
			Assert.Equal(expectedGuid, result[0].Guid);
		}
    }
}