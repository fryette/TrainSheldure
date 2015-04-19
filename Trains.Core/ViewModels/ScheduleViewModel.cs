using System.Collections.Generic;
using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using Trains.Core.Interfaces;
using Trains.Entities;

namespace Trains.Core.ViewModels
{
    public class ScheduleViewModel : MvxViewModel
    {
        #region readonlyProperty

        private readonly IAppSettings _appSettings;

        #endregion

        #region command

        public IMvxCommand GoToHelpPageCommand { get; private set; }
        public MvxCommand<Train> SelectTrainCommand { get; private set; }

        #endregion

        #region ctor

        public ScheduleViewModel(IAppSettings appSettings)
        {
            _appSettings = appSettings;

            GoToHelpPageCommand = new MvxCommand(GoToHelpPage);
            SelectTrainCommand = new MvxCommand<Train>(ClickItem);
        }

        #endregion

        #region properties

        /// <summary>
        /// �ontains information on all trains on the route selected by the user.
        /// </summary> 
        public IEnumerable<Train> Trains { get; set; }

        public string Request { get; set; }

        #endregion

        #region action
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// Set the default parameter of some properties.
        /// </summary>
        public void Init(string param)
        {
            Trains = JsonConvert.DeserializeObject<List<Train>>(param);
            Request = _appSettings.UpdatedLastRequest.From + " - " + _appSettings.UpdatedLastRequest.To;
        }

        /// <summary>
        /// Invoked when the user selects his train of interest.
        /// Go to the information page which show additional information about this train.
        /// </summary>
        /// <param name="train">Data that describes user-selected train(prices,seats,stop points and other)</param>
        private void ClickItem(Train train)
        {
            if (train == null) return;
            ShowViewModel<InformationViewModel>(new { param = JsonConvert.SerializeObject(train) });
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