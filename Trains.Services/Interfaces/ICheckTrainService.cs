using System;

namespace Trains.Services.Interfaces
{
   public interface ICheckTrainService
    {
        string CheckDate(DateTimeOffset datum);
        string CheckInput(string from, string to, DateTimeOffset datum);
        bool CheckFavorite(string from, string to);

    }
}
