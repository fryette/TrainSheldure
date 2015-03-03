using System;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Caliburn.Micro;
using TrainShedule_HubVersion.Infrastructure;

namespace TrainShedule_HubVersion.ViewModels
{
    public class SectionPageViewModel : Screen
    {
        private readonly INavigationService _navigationService;
        public MenuDataItem Parameter { get; set; }
        public SectionPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        #region properties

        private ImageSource _businessBackground = new BitmapImage(new Uri("ms-appx:///Assets/IRLB.png"));
        public ImageSource BusinessBackground
        {
            get { return _businessBackground; }
            set
            {
                _businessBackground = value;
                NotifyOfPropertyChange(() => BusinessBackground);
            }
        }
        private ImageSource _economBackground = new BitmapImage(new Uri("ms-appx:///Assets/IRLE.png"));
        public ImageSource EсonomBackground
        {
            get { return _economBackground; }
            set
            {
                _economBackground = value;
                NotifyOfPropertyChange(() => BusinessBackground);
            }
        }
        #endregion

        #region action
        protected override void OnActivate()
        {
            Parameter.SpecialSearch = true;
            if (Parameter.UniqueId != "Menu-Regional") return;
            BusinessBackground = new BitmapImage(new Uri("ms-appx:///Assets/RLB.png"));
            EсonomBackground = new BitmapImage(new Uri("ms-appx:///Assets/RLE.png"));
        }

        private void ClickBusiness()
        {
            _navigationService.NavigateToViewModel<ItemPageViewModel>(Parameter);
        }

        private void ClickEconom()
        {
            Parameter.IsEconom = true;
            _navigationService.NavigateToViewModel<ItemPageViewModel>(Parameter);
        }
        #endregion
    }
}
