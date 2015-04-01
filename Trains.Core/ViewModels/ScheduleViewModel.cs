using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using Trains.Entities;
using Trains.Services.Interfaces;

namespace Trains.Core.ViewModels
{
    public class ScheduleViewModel : MvxViewModel
    {
        #region properties
        /// <summary>
        /// Ñontains information on all trains on the route selected by the user.
        /// </summary> 
        public IEnumerable<Train> Parameter { get; set; }

        public string Request { get; set; }

        /// <summary>
        /// Used to serialization/deserialization objects.
        /// </summary>
        private readonly ISerializableService _serializable;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="navigationService">Used to navigate between pages.</param>
        /// <param name="serializable">Used to serialization/deserialization objects.</param>
        public ScheduleViewModel(ISerializableService serializable)
        {
            _serializable = serializable;
        }

        public void Init(IEnumerable<Train> trains)
        {
            Parameter = trains;
        }
        #endregion
    }
}