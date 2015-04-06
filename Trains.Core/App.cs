using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using Trains.Infrastructure.Infrastructure;
using Trains.Infrastructure.Interfaces;
using Trains.Service.Implementation;
using Trains.Services.Implementation;
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

            Mvx.LazyConstructAndRegisterSingleton<IAppSettings, AppSettings>();

            Mvx.LazyConstructAndRegisterSingleton<IHttpService, BaseHttpService>();
            Mvx.LazyConstructAndRegisterSingleton<IStartService, Start>();
            Mvx.LazyConstructAndRegisterSingleton<ISearchService, Search>();
            Mvx.LazyConstructAndRegisterSingleton<ILocalDataService, LocalData>();
            Mvx.LazyConstructAndRegisterSingleton<ITrainStopService, TrainStop>();
            Mvx.LazyConstructAndRegisterSingleton<IFavoriteManageService, FavoriteManage>();
            Mvx.LazyConstructAndRegisterSingleton<ITrainStopService, TrainStop>();
        }
    }
}