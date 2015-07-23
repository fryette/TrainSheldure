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

		private void RestoreUiBinding()
		{
			Header = ResourceLoader.Instance.Resource["SocialNetworks"];
			Title = ResourceLoader.Instance.Resource["SupportUs"];
		}
	}
}
