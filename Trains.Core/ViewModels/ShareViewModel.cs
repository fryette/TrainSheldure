using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrious.MvvmCross.ViewModels;
using Trains.Model.Entities;
using Trains.Core.Resources;

namespace Trains.Core.ViewModels
{
    public class ShareViewModel : MvxViewModel
    {

        #region UIproperties

        public string Header { get; set; }
        public string Title { get; set; }

        #endregion
        public IEnumerable<ShareSocial> ShareItems { get; set; }
        public void Init()
        {
            RestoreUIBinding();
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

        private void RestoreUIBinding()
        {
            Header = ResourceLoader.Instance.Resource["SocialNetworks"];
            Title = ResourceLoader.Instance.Resource["SupportUs"];
        }
    }
}
