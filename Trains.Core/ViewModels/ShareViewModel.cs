using System.Collections.Generic;
using Cirrious.MvvmCross.ViewModels;
using Trains.Core.Interfaces;
using Trains.Model.Entities;

namespace Trains.Core.ViewModels
{
    public class ShareViewModel : MvxViewModel
    {
	    private ILocalizationService _localizationService;

	    #region UIproperties

        public string Header { get; set; }
        public string Title { get; set; }

        #endregion
        public IEnumerable<ShareSocial> ShareItems { get; set; }

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
        }

	    public ShareViewModel(ILocalizationService localizationService)
	    {
		    _localizationService = localizationService;
	    }

        private void RestoreUiBinding()
        {
            Header = _localizationService.GetString("SocialNetworks");
            Title = _localizationService.GetString("SupportUs");
        }
    }
}
