using Trains.Core.Interfaces;

namespace Trains.Core.Resources
{
    public class Patterns : IPattern
    {
        public string TrainsPattern { get; set; }

        public string PlacesAndPricesPattern { get; set; }

        public string AdditionParameterPattern { get; set; }

        public string TrainPointPAttern { get; set; }
    }
}
