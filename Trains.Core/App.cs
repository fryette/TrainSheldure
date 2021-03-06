using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.ViewModels;
using Trains.Core.Interfaces;
using Trains.Core.Resources;
using Trains.Core.Services;
using Trains.Core.Services.Interfaces;
using Trains.Core.ViewModels;

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


			RegisterAppStart<MainViewModel>();

			Mvx.LazyConstructAndRegisterSingleton<IAppSettings, AppSettings>();
			Mvx.LazyConstructAndRegisterSingleton<ILocalDataService, LocalData>();

		}
	}
}