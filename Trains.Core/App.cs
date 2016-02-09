using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.ViewModels;
using Trains.Core.Services;
using Trains.Core.ViewModels;
using Trains.Infrastructure;
using Trains.Infrastructure.Interfaces;
using Trains.Infrastructure.Interfaces.Platform;
using Trains.Infrastructure.Interfaces.Services;

namespace Trains.Core
{
	public class App : MvxApplication
	{
		public override void Initialize()
		{
			CreatableTypes()
				.EndingWith("Service")
				.AsInterfaces()
				.RegisterAsLazySingleton();

			var isFirstRun = Mvx.GetSingleton<ISerializableService>().Desserialize<string>(Defines.Common.IsFirstRun);
			if (isFirstRun == null || isFirstRun != Defines.Common.IsFirstRun)
				RegisterAppStart<FeaturesViewModel>();
			else
				RegisterAppStart<MainViewModel>();


			Mvx.LazyConstructAndRegisterSingleton<IAppSettings, AppSettings>();
			Mvx.LazyConstructAndRegisterSingleton<ILocalDataService, LocalData>();

		}
	}
}