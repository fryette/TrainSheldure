using System.Collections.Generic;
using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using Trains.Core.Interfaces;
using Trains.Core.Services.Interfaces;
using Trains.Entities;
using Trains.Model.Entities;

namespace Trains.Core.ViewModels
{
	public class InformationViewModel : MvxViewModel
	{
		#region readonlyProperties

		/// <summary>
		/// Used to grab train stops.
		/// </summary>
		private readonly ITrainStopService _trainStop;

		private readonly ILocalizationService _localizationService;

		#endregion

		#region ctor

		public InformationViewModel(ITrainStopService trainStop,ILocalizationService localizationService)
		{
			_trainStop = trainStop;
			_localizationService = localizationService;
		}

		#endregion

		#region properties

		#region UIproperties

		public string DownloadStopPoints { get; set; }

		#endregion

		private IEnumerable<TrainStop> _stopPointList;
		public IEnumerable<TrainStop> StopPointList
		{
			get { return _stopPointList; }
			set
			{
				_stopPointList = value;
				RaisePropertyChanged(() => StopPointList);
			}
		}

		/// <summary>
		/// User-selected train.
		/// </summary>
		public Train Train { get; set; }

		/// <summary>
		/// Used for process control.
		/// </summary>
		private bool _isTaskRun;

		public bool IsTaskRun
		{
			get { return _isTaskRun; }
			set
			{
				_isTaskRun = value;
				RaisePropertyChanged(() => IsTaskRun);
			}
		}

		#endregion

		#region action

		public void Init(string param)
		{
			RestoreUiBindings();
			IsTaskRun = true;
			Train = JsonConvert.DeserializeObject<Train>(param);
			SearchStopPoint();
		}

		private async void SearchStopPoint()
		{
			StopPointList = await _trainStop.GetTrainStop(Train.Link);
			IsTaskRun = false;
		}

		private void RestoreUiBindings()
		{
			DownloadStopPoints = _localizationService.GetString("DownloadStopPoints");
		}
		#endregion
	}
}