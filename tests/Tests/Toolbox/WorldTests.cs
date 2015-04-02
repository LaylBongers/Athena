using Newtonsoft.Json.Linq;
using Xunit;

namespace Tests.Toolbox
{
	public class WorldTests
	{
		[Fact]
		public void LoadJson_WithSingleEntity_LoadsComponentsAndBehaviors()
		{
			// Arrange
			var json = new JObject
			{
				[""] = ""
			};

			// Act

			// Assert
		}
	}
}