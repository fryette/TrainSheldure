using System.Collections.Generic;
using Caliburn.Micro;
using System;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Phone.UI.Input;
using TrainShedule_HubVersion.ViewModels;
using TrainShedule_HubVersion.Views;

namespace TrainShedule_HubVersion
{
    public sealed partial class App
    {
        public App()
        {
            InitializeComponent();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }
        private WinRTContainer _container;
        protected override void Configure()
        {
            _container = new WinRTContainer();

            _container.RegisterWinRTServices();

            MessageBinder.SpecialValues.Add("$clickeditem", c => ((ItemClickEventArgs)c.EventArgs).ClickedItem);
            _container.PerRequest<StopPointPageViewModel>();
            _container.PerRequest<SchedulePageViewModel>();
            _container.PerRequest<InformationPageViewModel>();
            _container.PerRequest<SectionPageViewModel>();
            _container.PerRequest<ItemPageViewModel>();
            _container.PerRequest<MainPageViewModel>();
        }

        protected override void PrepareViewFirst(Frame rootFrame)
        {
            _container.RegisterNavigationService(rootFrame);
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            DisplayRootView<MainPageView>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
        private static void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            var frame = Window.Current.Content as Frame;
            if (frame == null) return;
            if (!frame.CanGoBack) return;
            frame.GoBack();
            e.Handled = true;
        }
    }
}
