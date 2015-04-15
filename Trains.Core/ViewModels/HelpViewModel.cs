using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using Trains.Core.Interfaces;
using Trains.Model.Entities;
using Trains.Services.Interfaces;

namespace Trains.Core.ViewModels
{
    public class HelpViewModel : MvxViewModel
    {
        #region readonlyProperties

        private readonly IAppSettings _appSettings;

        #endregion

        #region commands

        public MvxCommand<CarriageModel> SelectCarriageCommand { get; private set; }

        #endregion

        #region ctor

        public HelpViewModel(IAppSettings appSettings)
        {
            _appSettings = appSettings;
            SelectCarriageCommand = new MvxCommand<CarriageModel>((carriage)=>SelectCarriage(carriage));
        }

        #endregion

        #region properties

        /// <summary>
        /// Used to dispalying informations about belarussian railway icons.
        /// </summary>
        private IEnumerable<HelpInformationItem> _helpInformation;
        public IEnumerable<HelpInformationItem> HelpInformation
        {
            get { return _helpInformation; }
            set
            {
                _helpInformation = value;
                RaisePropertyChanged(() => HelpInformation);
            }
        }

        private List<CarriageModel> _carriageInformation;
        public List<CarriageModel> CarriageInformation
        {
            get { return _carriageInformation; }
            set
            {
                _carriageInformation = value;
                RaisePropertyChanged(() => CarriageInformation);
            }
        }

        #endregion

        #region action

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        public void Init()
        {
            HelpInformation = _appSettings.HelpInformation;
            CarriageInformation = _appSettings.CarriageModel;
        }

        private void SelectCarriage(CarriageModel selectedCarriageModel)
        {
            ShowViewModel<CarriageViewModel>(new { param = JsonConvert.SerializeObject(selectedCarriageModel) });
        }

        #endregion
    }
}