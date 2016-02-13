using Trains.Model.Entities;

namespace Trains.Infrastructure.Interfaces
{
	public interface ILocalizationService
	{
		string GetString(string key);
		void ChangeLocale(string newLocale);
		string CurrentLanguageId { get;}
	}
}
