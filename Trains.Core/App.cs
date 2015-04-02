using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using Trains.Core.ServicesAndInterfaces;
using Trains.Infrastructure.Infrastructure;
using Trains.Infrastructure.Interfaces;
using Trains.Services.Implementations;
using Trains.Services.Interfaces;

namespace Trains.Core
{
    public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<ViewModels.MainViewModel>();
            
			Mvx.LazyConstructAndRegisterSingleton<IAppSettings,AppSettings>();

			Mvx.LazyConstructAndRegisterSingleton<IHttpService, BaseHttpService>();
			Mvx.LazyConstructAndRegisterSingleton<ITestService, TestService>();
            Mvx.LazyConstructAndRegisterSingleton<IStartService, Start>();
            Mvx.LazyConstructAndRegisterSingleton<ISearchService, Search>();
            Mvx.LazyConstructAndRegisterSingleton<ILocalDataService, CountryStopPointData>();

        }
    }
}