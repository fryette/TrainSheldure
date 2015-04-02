using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trains.Model.Entities;

namespace Trains.Core.ViewModels
{
    public class StopPointViewModel : MvxViewModel
    {
        #region readonlyProperties
        /// <summary>
        /// Used to display stop points and time of arrival and time of departure.
        /// </summary> 
        public IEnumerable<TrainStop> Parameter { get; set; }
        #endregion

        #region command

        public IMvxCommand GoToHelpPageCommand { get; private set; }

        #endregion

        #region actions

        public StopPointViewModel(string param)
        {
            Parameter = JsonConvert.DeserializeObject<List<TrainStop>>(param);
            GoToHelpPageCommand = new MvxCommand(GoToHelpPage);
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
