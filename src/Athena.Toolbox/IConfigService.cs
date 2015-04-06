namespace Athena.Toolbox
{
	public interface IConfigService
	{
		Config Default { get; }
		Config LoadJson(string json);
	}
}