using System;

namespace Trains.Services.Interfaces
{
   public interface ICheckTrainService
    {
        void ShowMessageBox(string message);
        string GetDate(DateTimeOffset datum, string selectedVariantOfSearch);
        bool CheckDate(DateTimeOffset datum);
        bool CheckInput(string from, string to, DateTimeOffset datum);
        bool CheckFavorite(string from, string to);

    }
}
