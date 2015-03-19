using System.Reflection;
using System.Reflection.Emit;
using Xunit;

namespace Tests
{
	[Trait("Category", "Athena.Game")]
	public class GameTests
	{
		[Fact]
		public void LoadPlugins_ValidAssembly_Loads()
		{
			// Arrange
			var assemblyName = AssemblyName.GetAssemblyName("./TestPlugin.dll");
			//Assert.False(/*Make sure not loaded before this*/);

			// Act

			// Assert
		}
	}
}