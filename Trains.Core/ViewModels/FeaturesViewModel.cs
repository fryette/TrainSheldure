using System.Collections.Generic;
using System.Windows.Input;
using Chance.MvvmCross.Plugins.UserInteraction;
using Cirrious.MvvmCross.ViewModels;
using Trains.Model.Entities;

namespace Trains.Core.ViewModels
{
    public class FeaturesViewModel : MvxViewModel
    {

        public ICommand GoToMainViewCommand { get; private set; } 

        private List<ImageFeature> _imageFeatures;
        public List<ImageFeature> ImageFeatures
        {
            get { return _imageFeatures; }
            set
            {
                _imageFeatures = value;
                RaisePropertyChanged(() => ImageFeatures);

            }
        }

        public FeaturesViewModel(IUserInteraction userInteraction)
        {
            ImageFeatures = new List<ImageFeature>
                {
                    new ImageFeature {Path = "ms-appx:///Assets/Screenshots/1.png", Description = "Появилось бронирование и напоминания"},
                    new ImageFeature {Path = "ms-appx:///Assets/Screenshots/2.png", Description = "Бронирование \"Основные настройки\""},
                    new ImageFeature {Path = "ms-appx:///Assets/Screenshots/3.png", Description = "Бронирование \"Необязательные\""},
                    new ImageFeature {Path = "ms-appx:///Assets/Screenshots/4.png", Description = "Удобное управление любимыми маршрутами"},
                    new ImageFeature {Path = "ms-appx:///Assets/Screenshots/5.png", Description = "Обновленные настройки"},
                    new ImageFeature {Path = "ms-appx:///Assets/Screenshots/6.png", Description = "Дополнительные страны"},
                };

            GoToMainViewCommand=new MvxCommand(GoToMainView);
            userInteraction.AlertAsync("Пролистайте(свайп вправо).После ознакомления с новыми функциями нажмите на кнопку внизу экрана");
        }

        private void GoToMainView()
        {
            ShowViewModel<MainViewModel>();
        }
    }
}
