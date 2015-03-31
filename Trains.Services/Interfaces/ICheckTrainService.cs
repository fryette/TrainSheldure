using System;

namespace Trains.Services.Interfaces
{
   public interface ICheckTrainService
    {
        bool CheckInput(string from, string to, DateTimeOffset datum);
        bool CheckFavorite(string from, string to);
    }
}
