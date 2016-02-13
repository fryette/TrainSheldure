namespace Trains.Infrastructure.Interfaces.Providers
{
	public interface IConfigurationProvider
	{
		T Load<T>(string configName) where T : class;
	}
}
