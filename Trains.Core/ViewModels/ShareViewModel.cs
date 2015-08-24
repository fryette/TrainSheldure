using System.Collections.Generic;
using Cirrious.MvvmCross.ViewModels;
using Trains.Core.Resources;
using Trains.Model.Entities;

namespace Trains.Core.ViewModels
{
    public class ShareViewModel : MvxViewModel
    {

        #region UIproperties

        public string Header { get; set; }
        public string Title { get; set; }

        #endregion
        public IEnumerable<ShareSocial> ShareItems { get; set; }

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

        public void Init()
        {
            RestoreUiBinding();
            ShareItems = new List<ShareSocial>
            {
                ShareSocial.Vkontakte,
                ShareSocial.Facebook,
                ShareSocial.Twitter,
                ShareSocial.GooglePlus,
                ShareSocial.LinkedIn,
                ShareSocial.Odnoklassniki
            };

            ImageFeatures = new List<ImageFeature>
            {
                new ImageFeature {Path = "ms-appx:///Assets/Screenshots/1.png",Description = "New Featuer/1"},
                new ImageFeature {Path = "ms-appx:///Assets/Screenshots/2.png",Description = "New Featuer/2"},
                new ImageFeature {Path = "ms-appx:///Assets/Screenshots/3.png",Description = "New Featuer/3"},
                new ImageFeature {Path = "ms-appx:///Assets/Screenshots/4.png",Description = "New Featuer/4"},
                new ImageFeature {Path = "ms-appx:///Assets/Screenshots/5.png",Description = "New Featuer/5"},
                new ImageFeature {Path = "ms-appx:///Assets/Screenshots/6.png",Description = "New Featuer/6"},
            };
        }

        private void RestoreUiBinding()
        {
            Header = ResourceLoader.Instance.Resource["SocialNetworks"];
            Title = ResourceLoader.Instance.Resource["SupportUs"];
        }
    }
}
