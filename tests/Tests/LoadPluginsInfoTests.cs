using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Athena;
using Xunit;

namespace Tests
{
	[Trait("Type", "Unit")]
	[Trait("Class", "Athena.LoadPluginsInfo")]
	public class LoadPluginsInfoTests
	{
		[Fact]
		public void FromDirectory_ValidAssembly_LoadsName()
		{
			// Arrange
			var assemblyGuid = Guid.NewGuid().ToString();
			var tempDirectory = Guid.NewGuid().ToString();
			var assemblyLocation = "./" + tempDirectory + "/" + assemblyGuid + ".dll";

			Directory.CreateDirectory(tempDirectory);

			var expectedName = new AssemblyName(assemblyGuid);
			var builder = AppDomain.CurrentDomain.DefineDynamicAssembly(expectedName, AssemblyBuilderAccess.RunAndSave);
			builder.Save(assemblyGuid + ".dll");
			File.Move(assemblyGuid + ".dll", assemblyLocation);

			// Act
			var info = LoadPluginsInfo.FromDirectory(tempDirectory);

			// Cleanup
			File.Delete(assemblyLocation);
			Directory.Delete(tempDirectory);

			// Assert
			Assert.Equal(1, info.PluginAssemblies.Count);
			Assert.Contains(expectedName.Name, info.PluginAssemblies.Select(a => a.Name));
		}
	}
}