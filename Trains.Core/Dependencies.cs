using Cirrious.CrossCore;
using Trains.Infrastructure.Interfaces;

namespace Trains.Core
{
	public static class Dependencies
	{
		public static ILocalizationService LocalizationService => Mvx.Resolve<ILocalizationService>();
	}
}
