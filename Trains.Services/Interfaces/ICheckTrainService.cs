using System;

namespace Trains.Services.Interfaces
{
   public interface ICheckTrainService
    {
        void ShowMessageBox(string message);
        void Swap(ref string from, ref string to);
        string GetDate(DateTimeOffset datum, string selectedVariantOfSearch);
        bool CheckDate(DateTimeOffset datum);
        bool CheckInput(string from, string to, DateTimeOffset datum);
    }
}
