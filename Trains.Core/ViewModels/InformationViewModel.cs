using System.Collections.Generic;
using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using Trains.Entities;
using Trains.Infrastructure.Interfaces.Services;
using Trains.Model.Entities;

namespace Trains.Core.ViewModels
{
	public class InformationViewModel : MvxViewModel
	{
		#region readonlyProperties

		private readonly ITrainStopService _trainStop;

		#endregion

		#region ctor

		public InformationViewModel(ITrainStopService trainStop)
		{
			_trainStop = trainStop;
		}

		#endregion

		#region properties

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
			IsTaskRun = true;
			Train = JsonConvert.DeserializeObject<Train>(param);
			SearchStopPoint();
		}

		private async void SearchStopPoint()
		{
			StopPointList = await _trainStop.GetTrainStop(Train.Link);
			IsTaskRun = false;
		}
		#endregion
	}
}