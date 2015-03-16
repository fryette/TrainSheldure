using System;
using System.Threading.Tasks;

namespace Trains.Services.Interfaces
{
   public interface ICheckTrainService
    {
        void ShowMessageBox(string message);
        string GetDate(DateTimeOffset datum, string selectedVariantOfSearch);
        bool CheckDate(DateTimeOffset datum);
        string CheckInput(string from, string to, DateTimeOffset datum);
        bool CheckFavorite(string from, string to);

    }
}
