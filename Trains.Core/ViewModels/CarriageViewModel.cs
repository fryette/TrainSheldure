using Cirrious.MvvmCross.ViewModels;
using Trains.Infrastructure.Interfaces;
using Trains.Model.Entities;

namespace Trains.Core.ViewModels
{
	public class CarriageViewModel : MvxViewModel
	{
		private readonly IJsonConverter _jsonConverter;

		public CarriageViewModel(IJsonConverter jsonConverter)
		{
			_jsonConverter = jsonConverter;
		}

		public CarriageModel CarriageModel { get; set; }

		public void Init(string param)
		{
			CarriageModel = _jsonConverter.Deserialize<CarriageModel>(param);
		}
	}
}
