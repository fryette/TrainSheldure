using System.Collections.Generic;
using Cirrious.MvvmCross.ViewModels;
using Trains.Model.Entities;

namespace Trains.Core.ViewModels
{
    public class ShareViewModel : MvxViewModel
    {
        public IEnumerable<ShareSocial> ShareItems { get; set; }

        public void Init()
        {
            ShareItems = new List<ShareSocial>
            {
                ShareSocial.VKONTAKTE,
                ShareSocial.FACEBOOK,
                ShareSocial.TWITTER,
                ShareSocial.GOOGLE_PLUS,
                ShareSocial.LINKED_IN,
                ShareSocial.ODNOKLASSNIKI
            };
        }
    }
}
