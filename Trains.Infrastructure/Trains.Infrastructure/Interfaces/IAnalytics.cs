namespace Trains.Infrastructure.Interfaces
{
	public interface IAnalytics
	{
		void SentView(string view);
		void SentEvent(string mainCategory, string subCategory1 = "", string subCategory2 = "", long value = 0);
		void SentException(string description, bool isFatal = false);
	}
}
