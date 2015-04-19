using System.Collections.Generic;
using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using Trains.Model.Entities;

namespace Trains.Core.ViewModels
{
    public class StopPointViewModel : MvxViewModel
    {
        #region readonlyProperties
        /// <summary>
        /// Used to display stop points and time of arrival and time of departure.
        /// </summary> 
        public List<TrainStop> TrainStops { get; set; }
        #endregion

        #region command

        public IMvxCommand GoToHelpPageCommand { get; private set; }

        #endregion

        #region actions

        public StopPointViewModel()
        {
            GoToHelpPageCommand = new MvxCommand(GoToHelpPage);
        }

        public void Init(string param)
        {
            TrainStops = JsonConvert.DeserializeObject<List<TrainStop>>(param);
        }

        /// <summary>
        /// Go to saved by user routes page.
        /// </summary>
        private void GoToHelpPage()
        {
            ShowViewModel<HelpViewModel>();
        }

        #endregion
    }
}
