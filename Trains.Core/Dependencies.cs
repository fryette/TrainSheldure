using Cirrious.CrossCore;
using Trains.Core.Interfaces;

namespace Trains.Core
{
	public static class Dependencies
	{
		public static ILocalizationService LocalizationService => Mvx.Resolve<ILocalizationService>();
	}
}
