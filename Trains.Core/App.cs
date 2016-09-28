using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Trains.Core.ViewModels;
using Trains.Infrastructure;
using Trains.Infrastructure.Interfaces;
using Trains.Infrastructure.Interfaces.Platform;
using Trains.Infrastructure.Interfaces.Providers;
using Trains.Infrastructure.Interfaces.Services;
using Trains.Infrastructure.Json;
using Trains.Infrastructure.Providers;
using Trains.Services;

namespace Trains.Core
{
	public class App : MvxApplication
	{
		public override void Initialize()
		{
			Mvx.LazyConstructAndRegisterSingleton<IAppSettings, AppSettings>();
			Mvx.LazyConstructAndRegisterSingleton<ILocalDataService, LocalData>();
			Mvx.LazyConstructAndRegisterSingleton<IJsonConverter, NewtonsoftJsonConverter>();
			Mvx.LazyConstructAndRegisterSingleton<ISerializableService, SerializableService>();
			Mvx.LazyConstructAndRegisterSingleton<ISearchService, SearchService>();
			Mvx.LazyConstructAndRegisterSingleton<IHttpService, BaseHttpService>();
			Mvx.LazyConstructAndRegisterSingleton<IConfigurationProvider, JsonFileBasedConfigurationProvider>();
			Mvx.LazyConstructAndRegisterSingleton<ITrainStopService, TrainStopService>();

			//var isFirstRun = Mvx.GetSingleton<ISerializableService>().Desserialize<string>(Defines.Common.IsFirstRun);
			//if (isFirstRun == null || isFirstRun != Defines.Common.IsFirstRun)
			//	RegisterAppStart<FeaturesViewModel>();
			//else
				RegisterAppStart<StartViewModel>();
		}
	}
}